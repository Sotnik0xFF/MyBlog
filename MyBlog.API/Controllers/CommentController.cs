using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Application.Models;
using MyBlog.Application.Services;


namespace MyBlog.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class CommentController(CommentService commentService) : ControllerBase
{
    private readonly CommentService _commentService = commentService;

    /// <summary>
    /// Возвращает список всех комментариев.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CommentDTO>), StatusCodes.Status200OK)]
    public async Task<Ok<IEnumerable<CommentDTO>>> Get()
    {
        return TypedResults.Ok(await _commentService.FindAll());
    }

    /// <summary>
    /// Возвращает комментарий с указанным Id.
    /// </summary>
    /// <param name="id">Идентификатор комментария</param>
    /// <response code="404">Если комментарий с указанным id не найден.</response>
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(CommentDTO), StatusCodes.Status200OK)]
    public async Task<Results<Ok<CommentDTO>, NotFound>> Get(long id)
    {
        try
        {
            CommentDTO comment = await _commentService.FindById(id);
            return TypedResults.Ok(comment);
        }
        catch (KeyNotFoundException)
        {
            return TypedResults.NotFound();
        }
    }

    /// <summary>
    /// Создает комментарий.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(CommentDTO), StatusCodes.Status200OK)]
    public async Task<Ok> Post(CreateCommentRequest createCommentRequest)
    {
        await _commentService.Create(createCommentRequest);
        return TypedResults.Ok();
    }

    /// <summary>
    /// Редактирует комментарий.
    /// </summary>
    /// <response code="404">Если комментарий с указанным id не найден.</response>
    [HttpPut]
    [ProducesResponseType(typeof(CommentDTO), StatusCodes.Status200OK)]
    public async Task<Results<Ok<CommentDTO>, NotFound>> Put(UpdateCommentRequest updateCommentRequest)
    {
        try
        {
            CommentDTO updatedComment = await _commentService.Update(updateCommentRequest);
            return TypedResults.Ok(updatedComment);
        }
        catch (KeyNotFoundException)
        {
            return TypedResults.NotFound();
        }
    }

    /// <summary>
    /// Удаляет комментарий.
    /// </summary>
    /// <response code="404">Если комментарий с указанным id не найден.</response>
    [HttpDelete("{id:long}")]
    public async Task<Results<Ok<CommentDTO>, NotFound>> Delete(long id)
    {
        try
        {
            CommentDTO deletedComment = await _commentService.Delete(id);
            return TypedResults.Ok(deletedComment);
        }
        catch (KeyNotFoundException)
        {
            return TypedResults.NotFound();
        }
    }
}
