using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Application.Exceptions;
using MyBlog.Application.Models;
using MyBlog.Application.Services;
using System.ComponentModel.DataAnnotations;


namespace MyBlog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class TagController(TagService tagService) : ControllerBase
    {
        private readonly TagService _tagService = tagService;

        /// <summary>
        /// Возвращает список всех тегов.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TagDTO>), StatusCodes.Status200OK)]
        public async Task<Ok<IEnumerable<TagDTO>>> All()
        {
            return TypedResults.Ok(await _tagService.FindAll());
        }

        /// <summary>
        /// Возвращает тэг с указанным идентификатором.
        /// </summary>
        /// <param name="id">Идентификатор тэга</param>
        /// <response code="404">Если тэг с указанным id не найден.</response>
        [HttpGet("{id:long}")]
        [ProducesResponseType(typeof(TagDTO), StatusCodes.Status200OK)]
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

        /// <summary>
        /// Создает новый тэг или возвращает уже существующий с таким же названием.
        /// </summary>
        /// <response code="400">Если tagName null или пустая строка.</response>
        [HttpPost()]
        [ProducesResponseType(typeof(TagDTO), StatusCodes.Status200OK)]
        public async Task<Results<Ok<TagDTO>, BadRequest>> Post([Required]string tagName)
        {
            try
            {
                return TypedResults.Ok(await _tagService.Create(tagName));
            }
            catch (Exception)
            {
                return TypedResults.BadRequest();
            }
            
        }

        /// <summary>
        /// Редактирует тэг.
        /// </summary>
        /// <param name="id">Идентификатор тэга.</param>
        /// <param name="newTagName">Новое название тега.</param>
        /// <response code="400">Если тэг с указанным названием уже существует или newTagName null или пустая строка.</response>
        /// <response code="404">Если тэг с указанным id не найден.</response>
        [HttpPut("{id:long}")]
        [ProducesResponseType(typeof(TagDTO), StatusCodes.Status200OK)]
        public async Task<Results<Ok<TagDTO>, NotFound, BadRequest>> Put(long id, string newTagName)
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
                return TypedResults.BadRequest();
            }

        }

        /// <summary>
        /// Удаляет тэг.
        /// </summary>
        /// <param name="id">Идентификатор тэга</param>
        /// <response code="404">Если тэг с указанным id не найден.</response>
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
