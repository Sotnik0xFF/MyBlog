using Microsoft.AspNetCore.Mvc;
using MyBlog.Application.Exceptions;
using MyBlog.Application.Models;
using MyBlog.Application.Services;

namespace MyBlog.WebApp.Controllers
{
    public class CommentController(CommentService commentService, UserService userService) : Controller
    {
        private readonly CommentService _commentService = commentService;
        private readonly UserService _userService = userService;

        public async Task<IActionResult> All()
        {
            return Json(await _commentService.FindAll());
        }

        public async Task<IActionResult> ById(long id)
        {
            try
            {
                return Json(await _commentService.FindById(id));
            }
            catch (KeyNotFoundException)
            {
                return BadRequest("Коментарий не найден.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(long postId, string? commentTitle, string? commentText)
        {
            if (commentTitle == null || commentText == null)
            {
                return RedirectToAction("Details", "Post", new { id = postId });
            }

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                UserViewModel user = await _userService.FindByEmail(HttpContext.User.Identity.Name);
                CreateCommentRequest createCommentRequest = new() { PostId = postId, Title = commentTitle, Text = commentText, UserId = user.Id };
                await _commentService.Create(createCommentRequest);
                return RedirectToAction("Details", "Post", new { id = postId } );
            }

            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateCommentRequest updateCommentRequest)
        {
            return Json(await _commentService.Update(updateCommentRequest));
        }

        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                await _commentService.Delete(id);
                return Ok("Коментарий удален.");
            }
            catch (KeyNotFoundException)
            {
                return BadRequest("Коментарий не найден.");
            }
        }
    }
}
