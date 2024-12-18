﻿using Blog.Application.Common.DTOs;
using Blog.Application.Common.Requests;

namespace Blog.Application.Abstractions.Services;

public interface ICategoriesService
{
    Task<Category> CreateAsync(CreateCategoryRequest request, CancellationToken cancellationToken = default);
    Task<Category> UpdateAsync(int id, UpdateCategoryRequest request, CancellationToken cancellationToken = default);
}
