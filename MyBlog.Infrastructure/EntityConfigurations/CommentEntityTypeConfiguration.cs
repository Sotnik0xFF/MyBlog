using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyBlog.Domain.Models;

namespace MyBlog.Infrastructure.EntityConfigurations;

internal class CommentEntityTypeConfiguration : IEntityTypeConfiguration<Comment>
{
	public void Configure(EntityTypeBuilder<Comment> builder)
	{
        builder.Property(e => e.PostId).HasField("_postId").HasColumnName("post_id");
        builder.Property(e => e.UserId).HasField("_userId").HasColumnName("user_id");
    }
}
