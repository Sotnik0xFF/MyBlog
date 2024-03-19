using MyBlog.Application.Exceptions;
using MyBlog.Application.Models;
using MyBlog.Domain.Interfaces;
using MyBlog.Domain.Models;
using MyBlog.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Application.Services;

public class TagService(ITagRepository tagRepository)
{
    private readonly ITagRepository _tagRepository = tagRepository;

    public async Task<TagViewModel> Create(string tagValue)
    {
        Tag? tag = await _tagRepository.FindByValue(tagValue);
        if (tag != null)
            return Map(tag);

        tag = new Tag(tagValue);
        _tagRepository.Add(tag);
        await _tagRepository.UnitOfWork.SaveChangesAsync();

        return Map(tag);
    }

    public async Task<TagViewModel> Update(long id, string newValue)
    {
        Tag? tagToUpdate = await _tagRepository.FindById(id);

        if (tagToUpdate == null)
            throw new KeyNotFoundException(nameof(id));

        Tag? existedTag = await _tagRepository.FindByValue(newValue);
        if (existedTag != null && tagToUpdate != existedTag)
            throw new EntityAlreadyExistsException();

        tagToUpdate.Value = newValue;
        _tagRepository.Update(tagToUpdate);
        await _tagRepository.UnitOfWork.SaveChangesAsync();

        return Map(tagToUpdate);
    }

    public async Task<TagViewModel> Delete(long id)
    {
        Tag? tag = await _tagRepository.FindById(id);

        if (tag == null)
            throw new KeyNotFoundException(nameof(id));

        _tagRepository.Delete(tag);
        await _tagRepository.UnitOfWork.SaveChangesAsync();

        return Map(tag);
    }

    public async Task<TagViewModel> FindById(long id)
    {
        Tag? tag = await _tagRepository.FindById(id);

        if (tag == null)
            throw new KeyNotFoundException(nameof(id));

        return Map(tag);
    }

    public async Task<IEnumerable<TagViewModel>> FindAll()
    {
        IEnumerable<Tag> tags = await _tagRepository.FindAll();
        List<TagViewModel> tagModels = new();

        foreach (Tag tag in tags)
        {
            tagModels.Add(Map(tag));
        }

        return tagModels;
    }

    private TagViewModel Map(Tag tag)
    {
        return new TagViewModel() { Id = tag.Id, Name = tag.Value };
    }
}
