using MyBlog.Application.Exceptions;
using MyBlog.Application.Models;
using MyBlog.Domain.Interfaces;
using MyBlog.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Application.Services
{
    public class CommentService(ICommentRepository commentRepository, IUserRepository userRepository, IPostRepository postRepository, UserService userService)
    {
        private readonly ICommentRepository _commentRepository = commentRepository;
        private readonly IPostRepository _postRepository = postRepository;
        private readonly IUserRepository _userRepository = userRepository;

        private readonly UserService _userService = userService;

        public async Task<CommentViewModel> Create(CreateCommentRequest createCommentRequest)
        {
            Post? post = await _postRepository.FindById(createCommentRequest.PostId);
            if (post == null)
                throw new KeyNotFoundException(nameof(createCommentRequest.PostId));

            Comment newComment = new(createCommentRequest.UserId, post.Id, createCommentRequest.Title, createCommentRequest.Text);
            _commentRepository.Add(newComment);
            await _commentRepository.UnitOfWork.SaveChangesAsync();

            return Map(newComment);
        }

        public async Task<CommentViewModel> Update(UpdateCommentRequest updateCommentRequest)
        {
            Comment? commentToUpdate = await _commentRepository.FindById(updateCommentRequest.Id);
            if (commentToUpdate == null)
                throw new KeyNotFoundException(nameof(updateCommentRequest.Id));


            commentToUpdate.Text = updateCommentRequest.Text;
            commentToUpdate.Title = updateCommentRequest.Title;
            _commentRepository.Update(commentToUpdate);
            await _commentRepository.UnitOfWork.SaveChangesAsync();
            return Map(commentToUpdate);
        }

        public async Task<CommentViewModel> Delete(long id)
        {
            Comment? comment = await _commentRepository.FindById(id);
            if (comment == null)
                throw new KeyNotFoundException(nameof(id));

            _commentRepository.Delete(comment);
            await _commentRepository.UnitOfWork.SaveChangesAsync();
            return Map(comment);
        }

        public async Task<CommentViewModel> FindById(long id)
        {
            Comment? comment = await _commentRepository.FindById(id);
            if (comment == null)
                throw new KeyNotFoundException(nameof(id));

            return Map(comment);
        }

        public async Task<IEnumerable<CommentViewModel>> FindByPostId(long postId)
        {
            List<CommentViewModel> list = new List<CommentViewModel>();

            IEnumerable<Comment> comments = await _commentRepository.FindByPostId(postId);
            foreach (Comment comment in comments)
            {
                list.Add(Map(comment));
            }
            return list;
        }

        public async Task<IEnumerable<CommentViewModel>> FindAll()
        {
            List<CommentViewModel> list = new List<CommentViewModel>();

            IEnumerable<Comment> comments = await _commentRepository.FindAll();
            foreach (Comment comment in comments)
            {
                list.Add(Map(comment));
            }
            return list;
        }

        private CommentViewModel Map(Comment comment) 
        {
            return new CommentViewModel()
            {
                Id = comment.Id,
                PostId = comment.PostId,
                Author = _userService.FindById(comment.UserId).Result,
                Text = comment.Text,
                Title = comment.Title
            };
        }
    }
}
