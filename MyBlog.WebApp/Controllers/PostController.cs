using Microsoft.AspNetCore.Mvc;
using MyBlog.Application.Models;
using MyBlog.Application.Services;

namespace MyBlog.WebApp.Controllers
{
    public class PostController(PostService postService) : Controller
    {
        private readonly PostService _postService = postService;

        [HttpPost]
        public async Task<PostViewModel?> Create(CreatePostRequest createPostRequest)
        {
            return await _postService.Create(createPostRequest);
        }

        [HttpPost]
        public async Task<PostViewModel?> Update(UpdatePostRequest updatePostRequest)
        {
            return await _postService.Update(updatePostRequest);
        }

        public async Task<IActionResult> Delete(long id)
        {
            PostViewModel? post = await _postService.Delete(id);
            return RedirectToAction("All");
        }

        [HttpGet]
        public async Task<IActionResult> Details(long id)
        {
            PostViewModel postViewModel = await _postService.FindById(id);

            return View(postViewModel);
        }

        public async Task<IEnumerable<PostViewModel>> ByAuthorId(long id)
        {
            IEnumerable<PostViewModel> posts = await _postService.FindByAuthorId(id);

            return posts;
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            IEnumerable<PostHeaderViewModel> posts = await _postService.GetAllPostHeaders();

            return View(posts);
        }
    }
}
