using System.Threading.Tasks;
using HotelShare.Domain.Models.SqlModels.CommentModels;
using Microsoft.AspNetCore.Mvc;

namespace HotelShare.Infrastructure.Components
{
    public class CommentRender : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(DisplayCommentModel comment)
        {
            return await Task.FromResult((IViewComponentResult)View("CommentSection", comment));
        }
    }
}