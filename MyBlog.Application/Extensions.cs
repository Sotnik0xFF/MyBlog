using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyBlog.Application.Services;
using MyBlog.Domain.Interfaces;
using MyBlog.Infrastructure;
using MyBlog.Infrastructure.Repositories;

namespace MyBlog.Application;

public static class Extensions
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        string dbPath = Path.Combine(Path.GetDirectoryName(Environment.ProcessPath)!, "DB", "blog_data.db");
        services.AddDbContext<MyBlogDBContext>(options => options.UseSqlite($"Data Source={dbPath}"),
            ServiceLifetime.Scoped);

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IPostRepository, PostRepository>();
        services.AddScoped<ITagRepository, TagRepository>();
        services.AddScoped<ICommentRepository, CommentRepository>();


        services.AddScoped<UserService>();
        services.AddScoped<PostService>();
        services.AddScoped<TagService>();
        services.AddScoped<CommentService>();
        services.AddScoped<RoleService>();
    }
}
