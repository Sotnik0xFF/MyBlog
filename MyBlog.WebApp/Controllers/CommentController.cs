using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Application.Exceptions;
using MyBlog.Application.Models;
using MyBlog.Application.Services;
using MyBlog.Domain.Models;
using MyBlog.WebApp.Models;
using System.Collections.Generic;

namespace MyBlog.WebApp.Controllers
{
    public class CommentController(
        CommentService commentService,
        UserService userService,
        ILogger<CommentController> logger) : Controller
    {
        private readonly CommentService _commentService = commentService;
        private readonly UserService _userService = userService;
        private readonly ILogger<CommentController> _logger = logger;

        [Authorize(Roles ="Администратор, Модератор")]
        public async Task<IActionResult> All()
        {
            IEnumerable<CommentDTO> comments = await _commentService.FindAll();

            IEnumerable<CommentViewModel> model = comments.Select(c => new CommentViewModel()
            {
                Id = c.Id,
                Text = c.Text,
                Title = c.Title,
                UserFirstName = _userService.FindById(c.UserId).Result.FirstName,
                UserLastName = _userService.FindById(c.UserId).Result.LastName
            });
            return View(model);
        }

        [Authorize(Roles = "Администратор, Модератор")]
        [HttpGet]
        public async Task<IActionResult> Edit(long id)
        {
            try
            {
                CommentDTO comment = await _commentService.FindById(id);
                UpdateCommentRequest updateCommentRequest = new() { Id = comment.Id, Text = comment.Text, Title = comment.Title };
                return View(updateCommentRequest);
            }
            catch (KeyNotFoundException)
            {
                string errorMessage = $"Комментарий [Id = {id}] не найден.";
                _logger.LogError(errorMessage);
                return BadRequest(errorMessage);
            }
            
        }

        [Authorize(Roles = "Администратор, Модератор")]
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateCommentRequest updateCommentRequest)
        {
            if (updateCommentRequest.Title == null)
            {
                ModelState.AddModelError("", "Укажите заголовок комментария.");
            }

            if (updateCommentRequest.Text == null)
            {
                ModelState.AddModelError("", "Комментарий не должен быть пустым.");
            }

            if (ModelState.IsValid)
            {
                await _commentService.Update(updateCommentRequest);
                return RedirectToAction("All");
            }
            
            return View(updateCommentRequest);
        }

        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                await _commentService.Delete(id);
                _logger.LogInformation($"Комментарий [ID = {id}] удален.");
                return RedirectToAction("All");
            }
            catch (KeyNotFoundException)
            {
                string errorMessage = $"Комментарий [Id = {id}] не найден.";
                _logger.LogError(errorMessage);
                return BadRequest(errorMessage);
            }
        }
    }
}
