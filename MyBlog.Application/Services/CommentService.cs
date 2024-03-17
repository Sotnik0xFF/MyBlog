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
    public class CommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;

        public CommentService(ICommentRepository commentRepository, IUserRepository userRepository, IPostRepository postRepository)
        {
            _commentRepository = commentRepository;
            _userRepository = userRepository;
            _postRepository = postRepository;
        }

        public async Task<CommentDetails> Create(CreateCommentRequest createCommentRequest)
        {
            Post? post = await _postRepository.FindById(createCommentRequest.PostId);
            if (post == null)
                throw new EntityNotFoundException();

            User? user = await _userRepository.FindById(createCommentRequest.UserId);
            if (user == null)
                throw new EntityNotFoundException();

            Comment newComment = new(user.Id, post.Id, createCommentRequest.Title, createCommentRequest.Text);
            _commentRepository.Add(newComment);
            await _commentRepository.UnitOfWork.SaveChangesAsync();

            return Map(newComment, user);
        }

        public async Task<CommentDetails> Update(UpdateCommentRequest updateCommentRequest)
        {
            Comment? comment = await _commentRepository.FindById(updateCommentRequest.Id);
            if (comment == null)
                throw new EntityNotFoundException();

            User? user = await _userRepository.FindById(comment.UserId);
            if (user == null)
                throw new EntityNotFoundException();

            comment.Text = updateCommentRequest.Text;
            comment.Title = updateCommentRequest.Title;
            _commentRepository.Update(comment);
            await _commentRepository.UnitOfWork.SaveChangesAsync();
            return Map(comment, user);
        }

        public async Task<CommentDetails> Delete(long id)
        {
            Comment? comment = await _commentRepository.FindById(id);
            if (comment == null)
                throw new EntityNotFoundException();

            User? user = await _userRepository.FindById(comment.UserId);
            if (user == null)
                throw new EntityNotFoundException();

            _commentRepository.Delete(comment);
            await _commentRepository.UnitOfWork.SaveChangesAsync();
            return Map(comment, user);
        }

        public async Task<CommentDetails> FindById(long id)
        {
            Comment? comment = await _commentRepository.FindById(id);
            if (comment == null)
                throw new EntityNotFoundException();

            User? user = await _userRepository.FindById(comment.UserId);
            if (user == null)
                throw new EntityNotFoundException();

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
                    throw new EntityNotFoundException();
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
