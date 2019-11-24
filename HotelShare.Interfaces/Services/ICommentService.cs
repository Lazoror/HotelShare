using System;
using System.Collections.Generic;
using HotelShare.Domain.Enums;
using HotelShare.Domain.Models.SqlModels.CommentModels;

namespace HotelShare.Interfaces.Services
{
    public interface ICommentService
    {
        void AddComment(Comment entity, Guid hotelId);

        void AnswerComment(Comment entity, Guid hotelId);

        void DeleteComment(Guid commentId);

        void Ban(Guid commentId, BanDuration duration);

        Comment GetCommentById(Guid commentId);

        List<DisplayCommentModel> GetAllCommentsByGameKey(Guid hotelId);
    }
}