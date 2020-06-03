using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HotelShare.Domain.Enums;
using HotelShare.Domain.Models.SqlModels.AccountModels;
using HotelShare.Domain.Models.SqlModels.OrderModels;
using HotelShare.Interfaces.DAL.Data;
using HotelShare.Interfaces.DAL.RepositorySql;
using HotelShare.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace HotelShare.Services.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<OrderDetail> _orderDetailRepository;
        private readonly IRepository<Order> _orderRepository;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderService(
            IUnitOfWork unitOfWork,
            IUserService userService,
            IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
            _orderDetailRepository = _unitOfWork.GetRepository<OrderDetail>();
            _orderRepository = _unitOfWork.GetRepository<Order>();
        }

        public void DeleteOrder(Guid userId)
        {
            var order = _orderRepository.FirstOrDefault(a => a.CustomerId == userId && a.OrderStatus != OrderStatus.Paid);

            if (order != null)
            {
                _orderRepository.Delete(order);
                _unitOfWork.Commit();
            }
        }

        public void EditOrderDetail(OrderDetail entity)
        {
            var oldOrderDetail = JsonConvert.SerializeObject(_orderDetailRepository.FirstOrDefault(od => od.Id == entity.Id));

            if (entity.Quantity <= 0)
            {
                _orderDetailRepository.Delete(entity);
            }
            else
            {
                _orderDetailRepository.Update(entity);
            }

            _unitOfWork.Commit();
        }

        public void EditOrder(Order order)
        {
            if (order != null)
            {
                _orderRepository.Update(order);
                _unitOfWork.Commit();
            }
        }

        public IEnumerable<Order> GetOrderedRooms(Guid customerId)
        {
            var paidOrders = _orderRepository.GetMany(filter: x => x.OrderStatus == OrderStatus.Paid && x.OrderDate > DateTime.Now.AddDays(-30) && x.CustomerId == customerId, includes: x => x.OrderDetails);

            return paidOrders;
        }

        public MemoryStream GenerateInvoiceFile(ProcessPaymentModel orderInfo)
        {
            PdfDocument document = new PdfDocument();
            document.Info.Title = "Invoice file";

            // Create an empty page
            PdfPage page = document.AddPage();

            // Get an XGraphics object for drawing
            XGraphics gfx = XGraphics.FromPdfPage(page);

            // Create a font
            XFont font = new XFont("Verdana", 10, XFontStyle.Regular);

            // Draw the text
            gfx.DrawString($"Customer ID: {orderInfo.CustomerId}", font, XBrushes.Black,
                new XRect(10, 20, page.Width, 0), XStringFormats.BaseLineLeft);
            gfx.DrawString($"Order ID: {orderInfo.OrderId}", font, XBrushes.Black,
                new XRect(10, 35, page.Width, 0), XStringFormats.BaseLineLeft);
            gfx.DrawString($"Total: {orderInfo.OrderSum}", font, XBrushes.Black,
                new XRect(10, 50, page.Width, 0), XStringFormats.BaseLineLeft);

            // Send PDF to browser
            MemoryStream stream = new MemoryStream();
            document.Save(stream, false);

            return stream;
        }

        public Order GetOrderById(Guid orderId)
        {
            var order = _orderRepository.FirstOrDefault(o => o.Id == orderId, o => o.OrderDetails);

            return order;
        }

        public OrderDetail GetOrderDetail(Guid orderDetailId)
        {
            var orderDetail = _orderDetailRepository.FirstOrDefault(od => od.Id == orderDetailId);

            return orderDetail;
        }

        public IEnumerable<Order> GetHistoryOrder(DateTime fromDate, DateTime toDate)
        {
            return _orderRepository.GetMany(orderBy: o => o.OrderStatus).Where(o => o.OrderDate >= fromDate && o.OrderDate <= toDate).ToList();
        }

        public void DeleteOrderDetail(Guid roomId)
        {
            var orderDetail = _orderDetailRepository.FirstOrDefault(a => a.RoomId == roomId);

            if (orderDetail != null)
            {
                var order = _orderRepository.FirstOrDefault(a => a.Id == orderDetail.OrderId, includes: o => o.OrderDetails);

                _orderDetailRepository.Delete(orderDetail);

                if (!order.OrderDetails.Any())
                {
                    _orderRepository.Delete(order);
                }

                _unitOfWork.Commit();
            }
        }

        public void CreateOrder(OrderDetail orderDetails, string email)
        {
            var orderId = Guid.NewGuid();
            var futureUserId = Guid.NewGuid();
            var user = _userService.GetUserByEmail(email);
            orderDetails.Id = Guid.NewGuid();

            if (user == null)
            {
                user = new User();
                user.Id = futureUserId;
            }

            var isOrderExist = _orderRepository.GetMany(0, int.MaxValue, a => a.CustomerId == user.Id)
                .Any(a => a.OrderStatus != OrderStatus.Paid);
            var totalPrice = (orderDetails.Price * orderDetails.Quantity) - ((orderDetails.Discount / 100) * orderDetails.Price);
            orderDetails.Price = totalPrice;

            SetCookieOrder(ref isOrderExist, email, futureUserId, ref user, ref orderId);

            if (!isOrderExist)
            {
                AddNonExistingOrder(user, ref orderDetails, orderId);
            }
            else
            {
                UpdateExistingOrder(user, ref orderDetails);
            }
        }

        public Order GetAllCartOrder(string email)
        {
            var user = new User();

            if (!String.IsNullOrEmpty(email))
            {
                user = _userService.GetUserByEmail(email);
            }

            if (_httpContextAccessor.HttpContext.Request.Cookies.ContainsKey("Order") && String.IsNullOrEmpty(email))
            {
                var cachedOrderCookie = _httpContextAccessor.HttpContext.Request.Cookies["Order"];
                var cachedOrderModified = JsonConvert.DeserializeObject<GuestBasketModel>(cachedOrderCookie);

                if (user.Id == Guid.Empty)
                {
                    user.Id = cachedOrderModified.UserId;
                }
            }

            var currentOrder = _orderRepository.FirstOrDefault(a => a.CustomerId == user.Id && a.OrderStatus != OrderStatus.Paid, includes: od => od.OrderDetails);

            return currentOrder;
        }

        private void SetCookieOrder(ref bool isOrderExist, string email, Guid futureUserId, ref User user, ref Guid orderId)
        {
            if (String.IsNullOrEmpty(email))
            {
                if (_httpContextAccessor.HttpContext.Request.Cookies.ContainsKey("Order"))
                {
                    var cachedOrderCookie = _httpContextAccessor.HttpContext.Request.Cookies["Order"];
                    var cachedOrderModified = JsonConvert.DeserializeObject<GuestBasketModel>(cachedOrderCookie);

                    orderId = cachedOrderModified.OrderId;
                    user.Id = cachedOrderModified.UserId;

                    isOrderExist = true;
                }
                else
                {
                    CookieOptions option = new CookieOptions();
                    option.Expires = DateTime.Now.AddMonths(1);
                    var httpContext = _httpContextAccessor.HttpContext;

                    var cachedOrder = new GuestBasketModel
                    {
                        UserId = futureUserId,
                        OrderId = orderId
                    };

                    httpContext.Response.Cookies.Append("Order", JsonConvert.SerializeObject(cachedOrder, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
                }
            }
        }

        private void AddNonExistingOrder(User user, ref OrderDetail orderDetails, Guid orderId)
        {
            var orderNew = new Order { CustomerId = user.Id, Id = orderId, OrderDate = DateTime.UtcNow };
            _orderRepository.Insert(orderNew);

            orderDetails.OrderId = orderId;
            _orderDetailRepository.Insert(orderDetails);

            _unitOfWork.Commit();
        }

        private void UpdateExistingOrder(User user, ref OrderDetail orderDetails)
        {
            var order = _orderRepository.FirstOrDefault(a => a.CustomerId == user.Id && a.OrderStatus != OrderStatus.Paid, o => o.OrderDetails);
            var hotelId = orderDetails.RoomId;
            var isGameInBasket = order.OrderDetails.Any(a => a.RoomId == hotelId);

            if (isGameInBasket)
            {
                var orderDetailOld = _orderDetailRepository.FirstOrDefault(a => a.RoomId == hotelId);
                orderDetailOld.Quantity += orderDetails.Quantity;
                orderDetailOld.Price += orderDetails.Price;
                _orderDetailRepository.Update(orderDetailOld);
            }
            else
            {
                orderDetails.OrderId = order.Id;

                _orderDetailRepository.Insert(orderDetails);
            }

            _unitOfWork.Commit();
        }
    }
}