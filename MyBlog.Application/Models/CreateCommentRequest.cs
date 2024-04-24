﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Application.Models
{
    public class CreateCommentRequest
    {
        public CreateCommentRequest(long userId, long postId, string title, string text)
        {
            UserId = userId;
            PostId = postId;
            Title = title;
            Text = text;
        }

        public string Title { get;}
        public string Text { get; }
        public long UserId { get; }
        public long PostId { get; }
    }
}
