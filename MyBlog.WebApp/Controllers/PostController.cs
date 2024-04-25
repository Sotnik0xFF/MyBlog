using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Application.Models;
using MyBlog.Application.Services;
using MyBlog.WebApp.Models;

namespace MyBlog.WebApp.Controllers
{
    public class PostController(PostService postService, TagService tagService, UserService userService, CommentService commentService) : Controller
    {
        private readonly PostService _postService = postService;
        private readonly TagService _tagService = tagService;
        private readonly UserService _userService = userService;
        private readonly CommentService _commentService = commentService;

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
            PostDTO updatingPost = await _postService.FindById(id);

            EditPostViewModel editPostViewModel = new()
            {
                Id = updatingPost.Id,
                Text = updatingPost.Text,
                Title = updatingPost.Title,
                PostTagNames = updatingPost.Tags,
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
            PostDTO? post = await _postService.Delete(id);
            return RedirectToAction("All");
        }

        [HttpGet]
        public async Task<IActionResult> Details(long id)
        {
            PostDTO post = await _postService.FindById(id);
            UserDTO user = await _userService.FindById(post.AuthorId);
            IEnumerable<CommentDTO> comments = await _commentService.FindByPostId(post.Id);

            PostDetailsViewModel postDetailsViewModel = new PostDetailsViewModel()
            {
                Id = post.Id,
                Title = post.Title,
                Text = post.Text,
                AuthorId = post.AuthorId,
                AuthorFirstName = user.FirstName,
                AuthorLastName = user.LastName,
                Comments = comments.Select(c => new CommentViewModel()
                {
                    Id = c.Id,
                    Text = c.Text,
                    Title = c.Title,
                    UserFirstName = _userService.FindById(c.UserId).Result.FirstName,
                    UserLastName = _userService.FindById(c.UserId).Result.LastName
                }),
                
                Tags = post.Tags
            };

            return View(postDetailsViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Details(PostDetailsViewModel postDetailsViewModel)
        {
            if (postDetailsViewModel.commentTitle == null)
            {
                ModelState.AddModelError("", "Укажите заголовок комментария.");
            }

            if (postDetailsViewModel.commentText == null)
            {
                ModelState.AddModelError("", "Комментарий не должен быть пустым.");
            }

            if (ModelState.IsValid && HttpContext.User.Identity != null && HttpContext.User.Identity.Name != null) 
            {
                UserDTO user = await _userService.FindByEmail(HttpContext.User.Identity.Name);
                CreateCommentRequest createCommentRequest = new CreateCommentRequest(user.Id, postDetailsViewModel.Id, postDetailsViewModel.commentTitle!, postDetailsViewModel.commentText!);
                postDetailsViewModel.commentTitle = String.Empty;
                postDetailsViewModel.commentText = String.Empty;
                await _commentService.Create(createCommentRequest);
                return RedirectToAction("Details", new { Id = postDetailsViewModel.Id });
            }

            PostDTO post = await _postService.FindById(postDetailsViewModel.Id);
            UserDTO author = await _userService.FindById(post.AuthorId);
            IEnumerable<CommentDTO> comments = await _commentService.FindByPostId(post.Id);

            postDetailsViewModel.Title = post.Title;
            postDetailsViewModel.Text = post.Text;
            postDetailsViewModel.AuthorId = post.AuthorId;
            postDetailsViewModel.AuthorFirstName = author.FirstName;
            postDetailsViewModel.AuthorLastName = author.LastName;
            postDetailsViewModel.Comments = comments.Select(c => new CommentViewModel()
            {
                Id = c.Id,
                Text = c.Text,
                Title = c.Title,
                UserFirstName = _userService.FindById(c.UserId).Result.FirstName,
                UserLastName = _userService.FindById(c.UserId).Result.LastName
            });
            
            postDetailsViewModel.Tags = post.Tags;


            return View(postDetailsViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            IEnumerable<PostHeaderViewModel> posts = await _postService.GetAllPostHeaders();

            return View(posts);
        }
    }
}
