using MyBlog.Application.Exceptions;
using MyBlog.Application.Models;
using MyBlog.Domain.Interfaces;
using MyBlog.Domain.Models;
using MyBlog.Infrastructure.Repositories;

namespace MyBlog.Application.Services;

public class PostService
{
    private readonly IPostRepository _postRepository;
    private readonly ITagRepository  _tagRepository;

    public PostService(IPostRepository postRepository, ITagRepository tagRepository)
    {
        _postRepository = postRepository;
        _tagRepository = tagRepository;
    }

    public async Task<PostDetails> Create(CreatePostRequest newPostRequest)
    {
        Post newPost = new(newPostRequest.AuthorId, newPostRequest.Title, newPostRequest.Text);

        foreach (string tagName in newPostRequest.Tags)
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

    public async Task<PostDetails> Update(UpdatePostRequest updatePostRequest)
    {
        Post? editablePost = await _postRepository.FindById(updatePostRequest.Id);

        if (editablePost == null)
            throw new EntityNotFoundException();

        editablePost.ClearTags();

        foreach (string tagName in updatePostRequest.Tags)
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

    public async Task<PostDetails> Delete(long id)
    {
        Post? post = await _postRepository.FindById(id);

        if (post == null)
            throw new EntityNotFoundException();

        _postRepository.Delete(post);
        await _postRepository.UnitOfWork.SaveChangesAsync();
        return Map(post);
    }

    public async Task<IEnumerable<PostDetails>> FindByAuthorId(long authorId)
    {
        IEnumerable<Post> posts = await _postRepository.FindByUserId(authorId);

        List<PostDetails> postsDetails = new();

        foreach (Post post in posts)
        {
            PostDetails model = Map(post);
            postsDetails.Add(model);
        }

        return postsDetails;
    }

    public async Task<IEnumerable<PostDetails>> FindAll()
    {
        IEnumerable<Post> posts = await _postRepository.FindAll();

        List<PostDetails> postsDetails = new();

        foreach (Post post in posts)
        {
            PostDetails model = Map(post);
            postsDetails.Add(model);
        }

        return postsDetails;
    }

    private PostDetails Map(Post post)
    {
        PostDetails postDetails = new PostDetails()
        {
            Id = post.Id,
            AuthorId = post.UserId,
            Text = post.Text,
            Title = post.Title
        };

        postDetails.Tags = post.Tags.Select(t => t.Value).ToList();

        return postDetails;
    }
}
