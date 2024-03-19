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
    public class CommentService(ICommentRepository commentRepository, IUserRepository userRepository, IPostRepository postRepository)
    {
        private readonly ICommentRepository _commentRepository = commentRepository;
        private readonly IPostRepository _postRepository = postRepository;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<CommentDetails> Create(CreateCommentRequest createCommentRequest)
        {
            Post? post = await _postRepository.FindById(createCommentRequest.PostId);
            if (post == null)
                throw new KeyNotFoundException(nameof(createCommentRequest.PostId));

            User? user = await _userRepository.FindById(createCommentRequest.UserId);
            if (user == null)
                throw new KeyNotFoundException(nameof(createCommentRequest.UserId));

            Comment newComment = new(user.Id, post.Id, createCommentRequest.Title, createCommentRequest.Text);
            _commentRepository.Add(newComment);
            await _commentRepository.UnitOfWork.SaveChangesAsync();

            return Map(newComment, user);
        }

        public async Task<CommentDetails> Update(UpdateCommentRequest updateCommentRequest)
        {
            Comment? commentToUpdate = await _commentRepository.FindById(updateCommentRequest.Id);
            if (commentToUpdate == null)
                throw new KeyNotFoundException(nameof(updateCommentRequest.Id));

            User? user = await _userRepository.FindById(commentToUpdate.UserId);
            if (user == null)
                throw new KeyNotFoundException(nameof(commentToUpdate.UserId));

            commentToUpdate.Text = updateCommentRequest.Text;
            commentToUpdate.Title = updateCommentRequest.Title;
            _commentRepository.Update(commentToUpdate);
            await _commentRepository.UnitOfWork.SaveChangesAsync();
            return Map(commentToUpdate, user);
        }

        public async Task<CommentDetails> Delete(long id)
        {
            Comment? comment = await _commentRepository.FindById(id);
            if (comment == null)
                throw new KeyNotFoundException(nameof(id));

            User? user = await _userRepository.FindById(comment.UserId);
            if (user == null)
                throw new KeyNotFoundException(nameof(comment.UserId));

            _commentRepository.Delete(comment);
            await _commentRepository.UnitOfWork.SaveChangesAsync();
            return Map(comment, user);
        }

        public async Task<CommentDetails> FindById(long id)
        {
            Comment? comment = await _commentRepository.FindById(id);
            if (comment == null)
                throw new KeyNotFoundException(nameof(id));

            User? user = await _userRepository.FindById(comment.UserId);
            if (user == null)
                throw new KeyNotFoundException(nameof(comment.UserId));

            return Map(comment, user);
        }

        public async Task<IEnumerable<CommentDetails>> FindAll()
        {
            List<CommentDetails> list = new List<CommentDetails>();

            IEnumerable<Comment> comments = await _commentRepository.FindAll();
            foreach (Comment comment in comments)
            {
                User? user = await _userRepository.FindById(comment.UserId);
                if (user == null)
                    throw new KeyNotFoundException(nameof(comment.UserId));
                list.Add(Map(comment, user));
            }
            return list;
        }

        private CommentDetails Map(Comment comment, User user) 
        {
            return new CommentDetails()
            {
                Id = comment.Id,
                PostId = comment.PostId,
                AuthorName = user.Login,
                Text = comment.Text,
                Title = comment.Title
            };
        }
    }
}
