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
    public class TagController(TagService tagService) : Controller
    {
        private readonly TagService _tagService = tagService;
        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions() 
        { 
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping 
        };

        [HttpGet]
        public async Task<IActionResult> All()
        {
            IEnumerable<TagViewModel> tags = await _tagService.FindAll();
            return View(tags);
        }

        public async Task<IActionResult> ById(long id)
        {
            TagViewModel? tag = await _tagService.FindById(id);
            if (tag != null)
            {
                return Json(tag, _jsonOptions);
            }
            else
            {
                return BadRequest("Тэг не найден.");
            }
            
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
            }
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
            try
            {
                TagViewModel tag = await _tagService.Update(updateTagRequest);
                return RedirectToAction("All");
            }
            catch(EntityAlreadyExistsException)
            {
                ModelState.AddModelError("", "Тег с таким названием уже существует");
                return View();
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
