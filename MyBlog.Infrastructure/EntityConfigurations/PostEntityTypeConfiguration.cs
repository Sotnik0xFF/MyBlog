using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyBlog.Domain.Base;
using MyBlog.Domain.Models;

namespace MyBlog.Infrastructure.EntityConfigurations
{
    internal class PostEntityTypeConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.HasMany(p => p.Tags).WithMany().UsingEntity<PostsTags>(e =>
            {
                e.ToTable("posts_tags");
                e.Property("PostId").HasColumnName("post_id");
                e.Property("TagId").HasColumnName("tag_id");
            });
            builder.Property(e => e.UserId).HasField("_userId").HasColumnName("user_id");
        }
    }

    internal class PostsTags : Entity
    {
        public long PostId { get; set; }
        public long TagId { get; set; }
    }
}
