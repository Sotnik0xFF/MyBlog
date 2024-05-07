using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Application.Models;
using MyBlog.Application.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyBlog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController(CommentService commentService) : ControllerBase
    {
        private readonly CommentService _commentService = commentService;


        [HttpGet]
        public async Task<Ok<IEnumerable<CommentDTO>>> Get()
        {
            return TypedResults.Ok(await _commentService.FindAll());
        }

        // GET api/<CommentController>/5
        [HttpGet("{id:long}")]
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

        [HttpPost]
        public async Task<Ok> Post(CreateCommentRequest createCommentRequest)
        {
            await _commentService.Create(createCommentRequest);
            return TypedResults.Ok();
        }

        [HttpPut]
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
}
