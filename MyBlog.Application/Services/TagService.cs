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

    public async Task<TagDetails> Create(string tagValue)
    {
        Tag? tag = await _tagRepository.FindByValue(tagValue);
        if (tag != null)
            return Map(tag);

        tag = new Tag(tagValue);
        _tagRepository.Add(tag);
        await _tagRepository.UnitOfWork.SaveChangesAsync();

        return Map(tag);
    }

    public async Task<TagDetails> Update(long id, string newValue)
    {
        Tag? tag = await _tagRepository.FindById(id);

        if (tag == null)
            throw new EntityNotFoundException();

        Tag? tagByValue = await _tagRepository.FindByValue(newValue);
        if (tagByValue != null && tag != tagByValue)
            throw new EntityAlreadyExistsException();

        tag.Value = newValue;
        _tagRepository.Update(tag);
        await _tagRepository.UnitOfWork.SaveChangesAsync();

        return Map(tag);
    }

    public async Task<TagDetails> Delete(long id)
    {
        Tag? tag = await _tagRepository.FindById(id);

        if (tag == null)
            throw new EntityNotFoundException();

        _tagRepository.Delete(tag);
        await _tagRepository.UnitOfWork.SaveChangesAsync();

        return Map(tag);
    }

    public async Task<TagDetails?> FindById(long id)
    {
        Tag? tag = await _tagRepository.FindById(id);

        TagDetails? model = tag != null ? Map(tag) : null;

        return model;
    }

    public async Task<IEnumerable<TagDetails>> FindAll()
    {
        IEnumerable<Tag> tags = await _tagRepository.FindAll();
        List<TagDetails> tagModels = new();

        foreach (Tag tag in tags)
        {
            tagModels.Add(Map(tag));
        }

        return tagModels;
    }

    private TagDetails Map(Tag tag)
    {
        return new TagDetails() { Id = tag.Id, Name = tag.Value };
    }
}
