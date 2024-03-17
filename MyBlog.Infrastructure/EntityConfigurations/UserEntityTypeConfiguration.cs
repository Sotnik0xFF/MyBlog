using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyBlog.Domain.Base;
using MyBlog.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Infrastructure.EntityConfigurations;

internal class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasAlternateKey(u => u.Login);

		builder.HasMany(u => u.Roles).WithMany().UsingEntity<UserRole>(ur =>
		{
			ur.ToTable("users_roles");
			ur.Property("UserId").HasColumnName("user_id");
			ur.Property("RoleId").HasColumnName("role_id");
		});

        //builder.Property("Roles").HasField("_roles");
		builder.Property("Login").HasField("_login").HasColumnName("login");
        builder.Property("Password").HasField("_password").HasColumnName("password");
        builder.Property("FirstName").HasColumnName("first_name");
        builder.Property("LastName").HasColumnName("last_name");
    }
}

internal class UserRole : Entity
{
    public long UserId { get; set; }
    public long RoleId { get; set; }
}
