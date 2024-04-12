using MyBlog.Application.Exceptions;
using MyBlog.Application.Models;
using MyBlog.Domain.Interfaces;
using MyBlog.Domain.Models;
using System.Linq;

namespace MyBlog.Application.Services;

public class PostService(IPostRepository postRepository, ITagRepository tagRepository, CommentService commentService, UserService userService)
{
    private readonly IPostRepository _postRepository = postRepository;
    private readonly ITagRepository _tagRepository = tagRepository;

    private readonly CommentService _commentService = commentService;
    private readonly UserService _UserService = userService;

    public async Task<PostViewModel> Create(CreatePostRequest newPostRequest)
    {
        Post newPost = new(newPostRequest.AuthorId, newPostRequest.Title, newPostRequest.Text);

        foreach (String tagName in newPostRequest.TagNames)
        {
            Tag? foundTag = await _tagRepository.FindByValue(tagName);
            if (foundTag != null)
            {
                newPost.AddTag(foundTag);
            }
            else
            {
                Tag newTag = new(tagName);
                _tagRepository.Add(newTag);
                await _tagRepository.UnitOfWork.SaveChangesAsync();
                newPost.AddTag(newTag);
            }
        }

        _postRepository.Add(newPost);
        _postRepository.UnitOfWork.SaveChangesAsync().Wait();

        return Map(newPost);
    }

    public async Task<PostViewModel> Update(UpdatePostRequest updatePostRequest)
    {
        Post? editablePost = await _postRepository.FindById(updatePostRequest.Id);

        if (editablePost == null)
            throw new KeyNotFoundException(nameof(updatePostRequest.Id));

        editablePost.ClearTags();

        foreach (string tagName in updatePostRequest.TagNames)
        {
            Tag? tag = await _tagRepository.FindByValue(tagName);
            if (tag != null)
            {
                editablePost.AddTag(tag);
            }
            else
            {
                Tag newTag = new(tagName);
                _tagRepository.Add(newTag);
                await _tagRepository.UnitOfWork.SaveChangesAsync();
                editablePost.AddTag(newTag);
            }
        }

        editablePost.Title = updatePostRequest.Title;
        editablePost.Text = updatePostRequest.Text;

        await _postRepository.UnitOfWork.SaveChangesAsync();
        return Map(editablePost);
    }

    public async Task<PostViewModel> Delete(long id)
    {
        Post? post = await _postRepository.FindById(id);

        if (post == null)
            throw new KeyNotFoundException(nameof(id));

        _postRepository.Delete(post);
        await _postRepository.UnitOfWork.SaveChangesAsync();
        return Map(post);
    }

    public async Task<IEnumerable<PostViewModel>> FindByAuthorId(long authorId)
    {
        IEnumerable<Post> posts = await _postRepository.FindByUserId(authorId);

        List<PostViewModel> postsDetails = new();

        foreach (Post post in posts)
        {
            PostViewModel model = Map(post);
            postsDetails.Add(model);
        }

        return postsDetails;
    }

    public async Task<PostViewModel> FindById(long id)
    {
        Post? post = await _postRepository.FindById(id);

        if (post == null) 
            throw new KeyNotFoundException(nameof(id));

        return Map(post);
    }

    public async Task<IEnumerable<PostHeaderViewModel>> GetAllPostHeaders()
    {
        IEnumerable<Post> posts = await _postRepository.FindAll();

        List<PostHeaderViewModel> postHeaders = new();

        foreach (Post post in posts)
        {
            PostHeaderViewModel postHeader = new()
            {
                Id = post.Id,
                Title = post.Title,
                Author = await _UserService.FindById(post.UserId),
                Tags = post.Tags.Select(t => new TagViewModel() { Id = t.Id, Name = t.Value }).ToArray()
            };
            postHeaders.Add(postHeader);
        }

        return postHeaders;
    }

    private PostViewModel Map(Post post)
    {
        PostViewModel postDetails = new PostViewModel()
        {
            Id = post.Id,
            Author = _UserService.FindById(post.UserId).Result,
            Text = post.Text,
            Title = post.Title,
            Tags = post.Tags.Select(t => new TagViewModel() { Id = t.Id, Name = t.Value }).ToArray(),
            Comments = _commentService.FindByPostId(post.Id).Result
        };

        return postDetails;
    }
}
