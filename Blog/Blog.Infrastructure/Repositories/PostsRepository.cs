using Blog.Application.Abstractions.Repositories;
using Blog.Domain.Entities;

namespace Blog.Infrastructure.Repositories;

internal sealed class PostsRepository : BaseIntRepository<Post>, IPostsRepository
{
    public PostsRepository(AppDbContext dbContext) : base(dbContext) { }
}
