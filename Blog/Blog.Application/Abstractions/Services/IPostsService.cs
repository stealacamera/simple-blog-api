﻿using Blog.Application.Common.DTOs;
using Blog.Application.Common.DTOs.Requests.PostRequests;

namespace Blog.Application.Abstractions.Services;

public interface IPostsService
{
    Task<Post> CreateAsync(int requesterId, CreatePostRequest request, CancellationToken cancellationToken = default);
    Task<PostDetails> UpdateAsync(int id, int requesterId, UpdatePostRequest request, CancellationToken cancellationToken = default);
    Task<PostDetails> DeleteAsync(int id, int requesterId, CancellationToken cancellationToken = default);

    Task<Post> AddCategoriesToPostAsync(int id, int requesterId, UpdateCategoriesForPostRequest request, CancellationToken cancellationToken = default);
    Task<Post> RemoveCategoriesFromPostAsync(int id, int requesterId, UpdateCategoriesForPostRequest request, CancellationToken cancellationToken = default);

    Task<IList<Post>> GetAllAsync(
        int? requesterId = null,
        string? filterByTitle = null, 
        string? filterByContent = null, 
        DateOnly? filterByPublicationDate = null,
        CancellationToken cancellationToken = default);
}
