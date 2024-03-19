using Microsoft.AspNetCore.Mvc;
using MyBlog.Application.Models;
using MyBlog.Application.Services;

namespace MyBlog.WebApp.Controllers
{
    public class PostController(PostService postService) : Controller
    {
        private readonly PostService _postService = postService;

        [HttpPost]
        public async Task<PostDetails?> Create(CreatePostRequest createPostRequest)
        {
            return await _postService.Create(createPostRequest);
        }

        [HttpPost]
        public async Task<PostDetails?> Update(UpdatePostRequest updatePostRequest)
        {
            return await _postService.Update(updatePostRequest);
        }

        public async Task<PostDetails?> Delete(long id)
        {
            PostDetails? post = await _postService.Delete(id);

            return post;
        }

        public async Task<IEnumerable<PostDetails>> ByAuthorId(long id)
        {
            IEnumerable<PostDetails> posts = await _postService.FindByAuthorId(id);

            return posts;
        }

        public async Task<IEnumerable<PostDetails>> All()
        {
            IEnumerable<PostDetails> posts = await _postService.FindAll();

            return posts;
        }
    }
}
