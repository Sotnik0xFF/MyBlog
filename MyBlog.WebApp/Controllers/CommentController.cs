using Microsoft.AspNetCore.Mvc;
using MyBlog.Application.Exceptions;
using MyBlog.Application.Models;
using MyBlog.Application.Services;

namespace MyBlog.WebApp.Controllers
{
    public class CommentController(CommentService commentService) : Controller
    {
        private readonly CommentService _commentService = commentService;

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
            catch (EntityNotFoundException)
            {
                return BadRequest("Коментарий не найден.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCommentRequest createCommentRequest)
        {
            return Json(await _commentService.Create(createCommentRequest));
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
            catch (EntityNotFoundException)
            {
                return BadRequest("Коментарий не найден.");
            }
        }
    }
}
