using AutoMapper;
using HotelShare.Domain.Enums;
using HotelShare.Domain.Models.SqlModels.CommentModels;
using HotelShare.Interfaces.DAL.Data;
using HotelShare.Interfaces.DAL.RepositorySql;
using HotelShare.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HotelShare.Services.Services
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Comment> _commentRepository;
        private readonly IHotelRepository _hotelRepository;
        private readonly IMapper _mapper;

        public CommentService(IUnitOfWork unitOfWork,
            IMapper mapper,
            IHotelRepository hotelRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _hotelRepository = hotelRepository;
            _commentRepository = _unitOfWork.GetRepository<Comment>();
        }

        public void AddComment(Comment entity, Guid hotelId)
        {
            entity.Id = Guid.NewGuid();
            entity.HotelId = hotelId;

            _commentRepository.Insert(entity);
            _unitOfWork.Commit();
        }

        public void AnswerComment(Comment entity, Guid hotelId)
        {
            entity.Id = Guid.NewGuid();
            entity.HotelId = hotelId;

            _commentRepository.Insert(entity);
            _unitOfWork.Commit();
        }

        public void DeleteComment(Guid commentId)
        {
            var comment = _commentRepository.FirstOrDefault(c => c.Id == commentId, includes: c => c.ParentComment);
            var allComments = _commentRepository.GetMany(includes: c => c.ParentComment);

            DeleteCommentHierarchy(allComments, commentId);
            _commentRepository.Delete(comment);
            _unitOfWork.Commit();
        }

        public void Ban(Guid commentId, BanDuration duration)
        {
            // We don’t have users for now, so make empty service methods and test it.
        }

        public Comment GetCommentById(Guid commentId)
        {
            var comment = _commentRepository.FirstOrDefault(c => c.Id == commentId);

            return comment;
        }

        private void DeleteCommentHierarchy(IEnumerable<Comment> allComments, Guid deleteCommentId)
        {
            var toDeleteComments = allComments.Where(a => a.ParentCommentId == deleteCommentId).ToList();

            foreach (var comment in toDeleteComments)
            {
                _commentRepository.Delete(comment);

                DeleteCommentHierarchy(allComments, comment.Id);
            }
        }

        private List<DisplayCommentModel> CreateCommentsHierarchy(Guid hotelId, IEnumerable<Comment> allComments, Guid? currentParentId = null)
        {
            var commentDtos = allComments.Where(c => c.ParentCommentId == currentParentId).Select(c => _mapper.Map<DisplayCommentModel>(c)).ToList();

            commentDtos.ForEach(c =>
            {
                c.HotelId = hotelId;
                c.ChildrenComments =
                    CreateCommentsHierarchy(hotelId, allComments, c.CommentId);
            });

            return commentDtos;
        }

        public List<DisplayCommentModel> GetAllCommentsByGameKey(Guid hotelId)
        {
            var comments = _hotelRepository.FirstOrDefault(g => g.Id == hotelId).Comments;
            var commentsHierarchy = CreateCommentsHierarchy(hotelId, comments);

            return commentsHierarchy;
        }
    }
}