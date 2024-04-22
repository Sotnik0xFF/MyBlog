using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Application.Models;
using MyBlog.Application.Services;
using MyBlog.WebApp.Models;

namespace MyBlog.WebApp.Controllers
{
    public class PostController(PostService postService, TagService tagService, UserService userService) : Controller
    {
        private readonly PostService _postService = postService;
        private readonly TagService _tagService = tagService;
        private readonly UserService _userService = userService;

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            if (HttpContext.User.Identity != null && HttpContext.User.Identity.IsAuthenticated)
            {
                UserDTO user = await _userService.FindByEmail(HttpContext.User.Identity.Name);
                CreatePostViewModel createPostViewModel = new()
                {
                    AllTags = await _tagService.FindAll()
                };
                createPostViewModel.Request.AuthorId = user.Id;
                return View(createPostViewModel);
            }

            return BadRequest();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreatePostViewModel createPostViewModel)
        {
            if (createPostViewModel.Request.Title == null || createPostViewModel.Request.Text == null)
            {
                createPostViewModel.AllTags = await _tagService.FindAll();
                return View(createPostViewModel);
            }
            await _postService.Create(createPostViewModel.Request);
            return RedirectToAction("All");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(long id)
        {
            PostViewModel updatingPost = await _postService.FindById(id);

            EditPostViewModel editPostViewModel = new() { AllTags = await _tagService.FindAll() };

            editPostViewModel.Request.Id = updatingPost.Id;
            editPostViewModel.Request.Title = updatingPost.Title;
            editPostViewModel.Request.Text = updatingPost.Text;
            editPostViewModel.Request.TagNames = updatingPost.Tags.Select(x => x.Name).ToArray();

            return View(editPostViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditPostViewModel editPostViewModel)
        {
            editPostViewModel.AllTags = await _tagService.FindAll();

            await _postService.Update(editPostViewModel.Request);
            return RedirectToAction("All");
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

        [HttpGet]
        public async Task<IActionResult> All()
        {
            IEnumerable<PostHeaderViewModel> posts = await _postService.GetAllPostHeaders();

            return View(posts);
        }
    }
}
