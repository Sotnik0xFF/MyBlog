using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Application.Models;
using MyBlog.Application.Services;


namespace MyBlog.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class PostController(PostService postService) : ControllerBase
{
    private readonly PostService _postService = postService;


    /// <summary>
    /// Возвращает список всех статей.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PostHeaderViewModel>), StatusCodes.Status200OK)]
    public async Task<Ok<IEnumerable<PostHeaderViewModel>>> Get()
    {
        return TypedResults.Ok(await _postService.GetAllPostHeaders());
    }

    /// <summary>
    /// Возвращает статью с указанным ID.
    /// </summary>
    /// <param name="id">Идентификатор статьи</param>
    /// <response code="404">Если статья с указанным id не найдена.</response>
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(PostDTO), StatusCodes.Status200OK)]
    public async Task<Results<Ok<PostDTO>, NotFound>> Get(long id)
    {
        try
        {
            PostDTO post = await _postService.FindById(id);
            return TypedResults.Ok(post);
        }
        catch (Exception)
        {
            return TypedResults.NotFound();
        }
    }

    /// <summary>
    /// Создает статью.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(PostDTO), StatusCodes.Status200OK)]
    public async Task<Results<Ok<PostDTO>, BadRequest>> Post([FromBody] CreatePostRequest createPostRequest)
    {
        try
        {
            PostDTO createdPost = await _postService.Create(createPostRequest);
            return TypedResults.Ok(createdPost);
        }
        catch (Exception)
        {
            return TypedResults.BadRequest();
        }
        
    }

    /// <summary>
    /// Редактирует статью.
    /// </summary>
    [HttpPut]
    [ProducesResponseType(typeof(PostDTO), StatusCodes.Status200OK)]
    public async Task<Results<Ok<PostDTO>, NotFound>> Put(UpdatePostRequest updatePostRequest)
    {
        try
        {
            PostDTO post = await _postService.Update(updatePostRequest);
            return TypedResults.Ok(post);
        }
        catch (KeyNotFoundException)
        {
            return TypedResults.NotFound();
        }
    }


    /// <summary>
    /// Удаляет статью.
    /// </summary>
    /// <param name="id">Идентификатор статьи</param>
    /// <response code="404">Если статья с указанным id не найдена.</response>
    [HttpDelete("{id:long}")]
    [ProducesResponseType(typeof(PostDTO), StatusCodes.Status200OK)]
    public async Task<Results<Ok<PostDTO>, NotFound>> Delete(long id)
    {
        try
        {
            PostDTO deletedPost = await _postService.Delete(id);
            return TypedResults.Ok(deletedPost);
        }
        catch (KeyNotFoundException)
        {
            return TypedResults.NotFound();
        }
    }
}
