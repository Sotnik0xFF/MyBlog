using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Application.Exceptions;
using MyBlog.Application.Models;
using MyBlog.Application.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyBlog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController(TagService tagService) : ControllerBase
    {
        private readonly TagService _tagService = tagService;

        [HttpGet]
        [Route("")]
        public async Task<Ok<IEnumerable<TagDTO>>> All()
        {
            return TypedResults.Ok(await _tagService.FindAll());
        }

        [HttpGet("{id:long}")]
        public async Task<Results<Ok<TagDTO>, NotFound>> Get(long id)
        {
            try
            {
                TagDTO tag = await _tagService.FindById(id);
                return TypedResults.Ok(tag);
            }
            catch (KeyNotFoundException)
            {
                return TypedResults.NotFound();
            }
        }

        [HttpPost]
        public async Task<TagDTO> Post([FromBody] string value)
        {
            return await _tagService.Create(value);
        }

        [HttpPut("{id:long}")]
        public async Task<Results<Ok<TagDTO>, NotFound, BadRequest<string>>> Put(long id, string newTagName)
        {
            try
            {
                UpdateTagRequest updateTagRequest = new()
                {
                    Id = id,
                    NewTagName = newTagName
                };

                TagDTO updatedTag = await _tagService.Update(updateTagRequest);
                return TypedResults.Ok(updatedTag);
            }
            catch (KeyNotFoundException)
            {
                return TypedResults.NotFound();
            }
            catch(EntityAlreadyExistsException)
            {
                return TypedResults.BadRequest($"Уже существует тэг с названием [{newTagName}].");
            }

        }

        [HttpDelete("{id:long}")]
        public async Task<Results<Ok<TagDTO>, NotFound>> Delete(long id)
        {
            try
            {
                TagDTO deletedTag = await _tagService.Delete(id);
                return TypedResults.Ok(deletedTag);
            }
            catch (KeyNotFoundException)
            {
                return TypedResults.NotFound();
            }
        }
    }
}
