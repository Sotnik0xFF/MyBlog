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
            if (HttpContext.User.Identity != null && HttpContext.User.Identity.Name != null && HttpContext.User.Identity.IsAuthenticated)
            {
                UserDTO user = await _userService.FindByEmail(HttpContext.User.Identity.Name);
                CreatePostViewModel createPostViewModel = new()
                {
                    AuthorId = user.Id,
                    AllTagNames = _tagService.FindAll().Result.Select(t => t.Name),
                    Text = String.Empty,
                    Title = String.Empty,
                };
                return View(createPostViewModel);
            }

            return BadRequest();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreatePostViewModel createPostViewModel)
        {
            createPostViewModel.AllTagNames = _tagService.FindAll().Result.Select(t => t.Name);

            if (ModelState.IsValid)
            {
                CreatePostRequest createPostRequest = new CreatePostRequest(
                    createPostViewModel.AuthorId,
                    createPostViewModel.Title,
                    createPostViewModel.Text,
                    createPostViewModel.PostTagNames);

                await _postService.Create(createPostRequest);
                return RedirectToAction("All");
            }
            return View(createPostViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(long id)
        {
            PostViewModel updatingPost = await _postService.FindById(id);

            EditPostViewModel editPostViewModel = new()
            {
                Id = updatingPost.Id,
                Text = updatingPost.Title,
                Title = updatingPost.Text,
                PostTagNames = updatingPost.Tags.Select(x => x.Name),
                AllTagNames = _tagService.FindAll().Result.Select(x => x.Name)
            };

            return View(editPostViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditPostViewModel editPostViewModel)
        {
            editPostViewModel.AllTagNames = _tagService.FindAll().Result.Select(x => x.Name);

            if (ModelState.IsValid)
            {
                UpdatePostRequest request = new UpdatePostRequest(editPostViewModel.Id, editPostViewModel.Title, editPostViewModel.Text, editPostViewModel.PostTagNames);
                await _postService.Update(request);
                return RedirectToAction("All");
            }


            return View(editPostViewModel);
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
