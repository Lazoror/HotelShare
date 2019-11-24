using AutoMapper;
using HotelShare.Domain;
using HotelShare.Domain.Enums;
using HotelShare.Domain.Models.SqlModels.CommentModels;
using HotelShare.Interfaces.Services;
using HotelShare.Web.Attributes;
using HotelShare.Web.ViewModels.Comment;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Threading;

namespace HotelShare.Web.Controllers
{
    [Route("comments")]
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly IHotelService _hotelService;
        private readonly IMapper _mapper;
        private readonly string _lang;

        public CommentController(ICommentService commentService, IMapper mapper, IHotelService hotelService)
        {
            _commentService = commentService;
            _mapper = mapper;
            _hotelService = hotelService;
            _lang = Thread.CurrentThread.CurrentCulture.Name;
        }

        [HttpPost("{hotelId}/newcomment")]
        [StoreAuthorize(AuthorizePermission.Disallow, RoleName.Admin)]
        public IActionResult Add(CommentViewModel comment, Guid hotelId)
        {
            var hotel = _hotelService.Get(comment.HotelId);

            if (!hotel.IsDeleted)
            {
                if (ModelState.IsValid)
                {
                    var commentModel = _mapper.Map<Comment>(comment);

                    _commentService.AddComment(commentModel, comment.HotelId);
                }
            }

            var hotelComments = _commentService.GetAllCommentsByGameKey(hotelId);
            var commentViewModel = new DisplayCommentViewModel { HotelId = hotelId, Comments = hotelComments };

            return PartialView("_CommentsRender", hotelComments);
        }


        [HttpGet("{hotelId}/reply")]
        [StoreAuthorize(AuthorizePermission.Disallow, RoleName.Admin)]
        public IActionResult Reply(Guid hotelId, Guid parentCommentId)
        {
            var replyCommentViewModel = new CreateCommentViewModel
            {
                HotelId = hotelId,
                ParentCommentId = parentCommentId
            };

            return View(nameof(Reply), replyCommentViewModel);
        }

        [HttpPost("{hotelId}/reply")]
        [StoreAuthorize(AuthorizePermission.Disallow, RoleName.Admin)]
        public IActionResult Reply(CreateCommentViewModel replyComment)
        {
            var hotel = _hotelService.Get(replyComment.HotelId);

            if (hotel.IsDeleted)
            {
                return RedirectToAction("NotFound", "Hotel");
            }

            if (ModelState.IsValid)
            {
                var comment = _mapper.Map<Comment>(replyComment);

                _commentService.AnswerComment(comment, replyComment.HotelId);

                return RedirectToAction("GetDetailsByKey", "Hotel", new { hotelId = replyComment.HotelId });
            }


            return View(nameof(Reply), replyComment);
        }

        [HttpGet("{hotelId}/quote")]
        [StoreAuthorize(AuthorizePermission.Disallow, RoleName.Admin)]
        public IActionResult Quote(Guid hotelId, Guid parentCommentId)
        {
            var parrentComment = _commentService.GetCommentById(parentCommentId);

            var replyCommentViewModel = new CreateCommentViewModel
            {
                HotelId = hotelId,
                ParentCommentId = parentCommentId,
                Quote = parrentComment.Body
            };

            return View(nameof(Quote), replyCommentViewModel);
        }

        [HttpPost("{hotelId}/quote")]
        [StoreAuthorize(AuthorizePermission.Disallow, RoleName.Admin)]
        public IActionResult Quote(CreateCommentViewModel quoteComment)
        {
            var hotel = _hotelService.Get(quoteComment.HotelId);

            if (hotel.IsDeleted)
            {
                return RedirectToAction("NotFound", "Hotel");
            }

            if (ModelState.IsValid)
            {
                var comment = _mapper.Map<Comment>(quoteComment);

                _commentService.AnswerComment(comment, quoteComment.HotelId);

                var hotelComments = _commentService.GetAllCommentsByGameKey(hotel.Id);
                var commentViewModel = new DisplayCommentViewModel { HotelId = hotel.Id, Comments = hotelComments };

                return PartialView("_GameComments", commentViewModel);
            }

            return RedirectToAction("GetDetailsByKey", "Hotel", new { hotelId = hotel.Id });
        }

        [HttpGet("{hotelId}/delete")]
        [StoreAuthorize(AuthorizePermission.Allow, RoleName.Moderator)]
        public IActionResult Delete(Guid commentId, Guid hotelId)
        {
            _commentService.DeleteComment(commentId);

            return RedirectToAction("GetDetailsByKey", "Hotel", new { hotelId = hotelId });
        }

        [HttpGet("ban")]
        [StoreAuthorize(AuthorizePermission.Allow, RoleName.Moderator)]
        public ViewResult Ban(Guid commentId)
        {
            return View(commentId);
        }

        [HttpPost("ban")]
        [StoreAuthorize(AuthorizePermission.Allow, RoleName.Moderator)]
        public IActionResult Ban(Guid commentId, BanDuration banDuration)
        {
            _commentService.Ban(commentId, banDuration);

            return RedirectToAction(nameof(Index), "Hotel");
        }
    }
}