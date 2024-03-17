using MyBlog.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Domain.Models
{
    public class Comment : Entity
    {
        private long _userId;
        private long _postId;

        public Comment(long userId, long postId, string title, string text)
        {
            _userId = userId;
            _postId = postId;
            Title = title;
            Text = text;
        }

        public Comment(long id, long userId, long postId, string title, string text) :
            this(userId, postId, title, text)
        {
            Id = id;
        }

        public string Title { get; set; }
        public string Text { get; set; }
        public long UserId => _userId;
        public long PostId => _postId;
    }
}
