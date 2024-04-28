using Microsoft.AspNetCore.Mvc;
using MyBlog.Application.Exceptions;
using MyBlog.Application.Models;
using MyBlog.Application.Services;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace MyBlog.WebApp.Controllers
{
    public class TagController(TagService tagService, ILogger<TagController> logger) : Controller
    {
        private readonly TagService _tagService = tagService;
        private readonly ILogger<TagController> _logger = logger;

        [HttpGet]
        public async Task<IActionResult> All()
        {
            IEnumerable<TagViewModel> tags = await _tagService.FindAll();
            return View(tags);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(string tagName)
        {
            if (tagName != null)
            {
                await _tagService.Create(tagName);
                _logger.LogInformation($"Добавлен тег {tagName}");
                return RedirectToAction("All");
            }
            ModelState.AddModelError("", "Введите название тега");
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Edit(long id)
        {
            try
            {
                TagViewModel tag = await _tagService.FindById(id);
                UpdateTagRequest updateTagRequest = new() { Id = tag.Id, NewTagName = tag.Name };
                return View(updateTagRequest);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateTagRequest updateTagRequest)
        {
            if (updateTagRequest.NewTagName == null)
            {
                ModelState.AddModelError("", "Введите название тега");
                return View();
            }
            try
            {
                TagViewModel tag = await _tagService.Update(updateTagRequest);
                _logger.LogInformation($"Название тега ID {tag.Id} изменено на {tag.Name}.");
                return RedirectToAction("All");
            }
            catch(EntityAlreadyExistsException)
            {
                ModelState.AddModelError("", "Тег с таким названием уже существует");
                return View(updateTagRequest);
            }
            catch(KeyNotFoundException)
            {
                return BadRequest($"Тег [Id = {updateTagRequest.Id}] не найден.");
            }
        }

        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                TagViewModel tag = await _tagService.Delete(id);
                return RedirectToAction("All");
            }
            catch (KeyNotFoundException)
            {
                return BadRequest($"Тэг [Id = {id}] не найден.");
            }           
        }
    }
}
