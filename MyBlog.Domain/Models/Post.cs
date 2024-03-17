using MyBlog.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Domain.Models;

public class Post : Entity
{
    private readonly long _userId;
    private readonly List<Tag> _tags;

    public Post(long userId, string title, string text) : this(title, text)
    {
        _userId = userId;
    }

    public Post(string title, string text)
    {
        Title = title;
        Text = text;
        _tags = new List<Tag>();
    }

    public string Title { get; set; }
    public string Text { get; set; }
    public long UserId => _userId;
    public IEnumerable<Tag> Tags => _tags;

    public void AddTag(Tag tag)
    {
        _tags.Add(tag);
    }

    public void RemoveTag(Tag tag)
    {
        _tags.Remove(tag);
    }

    public void ClearTags()
    {
        _tags.Clear();
    }
}
