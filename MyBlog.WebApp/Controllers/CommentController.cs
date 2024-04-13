using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Application.Exceptions;
using MyBlog.Application.Models;
using MyBlog.Application.Services;

namespace MyBlog.WebApp.Controllers
{
    public class CommentController(CommentService commentService, UserService userService, PostService postService) : Controller
    {
        private readonly CommentService _commentService = commentService;
        private readonly UserService _userService = userService;
        private readonly PostService _postService = postService;

        [Authorize(Roles ="Администратор, Модератор")]
        public async Task<IActionResult> All()
        {
            return View(await _commentService.FindAll());
        }

        [HttpGet]
        public async Task<IActionResult> Edit(long id)
        {
            CommentViewModel comment = await _commentService.FindById(id);
            UpdateCommentRequest updateCommentRequest = new() { Id = comment.Id, Text = comment.Text, Title = comment.Title };
            return View(updateCommentRequest);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateCommentRequest updateCommentRequest)
        {
            await _commentService.Update(updateCommentRequest);
            return RedirectToAction("All");
        }
 

        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                await _commentService.Delete(id);
                return RedirectToAction("All");
            }
            catch (KeyNotFoundException)
            {
                return BadRequest("Коментарий не найден.");
            }
        }
    }
}
