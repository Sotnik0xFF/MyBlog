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

        public async Task<IActionResult> Index()
        {
            IEnumerable<TagViewModel> tags = await _tagService.FindAll();
            return Json(tags, _jsonOptions);
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

        public async Task<IActionResult> Create(string name)
        {
            TagViewModel tag = await _tagService.Create(name);
            return Json(tag, _jsonOptions);
        }

        public async Task<IActionResult> Update(long id, string name)
        {
            try
            {
                TagViewModel tag = await _tagService.Update(id, name);
                return Ok($"Тэг [Id = {id}] обновлен. Название тега: {name}");
            }
            catch(EntityAlreadyExistsException)
            {
                return BadRequest($"Тэг {name} уже существует.");
            }
            catch(KeyNotFoundException)
            {
                return BadRequest($"Тэг [Id = {id}] не найден.");
            }
        }

        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                TagViewModel tag = await _tagService.Delete(id);
                return Ok($"Тэг {tag.Name} удален.");
            }
            catch (KeyNotFoundException)
            {
                return BadRequest($"Тэг [Id = {id}] не найден.");
            }           
        }
    }
}
