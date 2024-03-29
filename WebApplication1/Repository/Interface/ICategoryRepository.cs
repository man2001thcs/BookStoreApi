﻿using WebApplication1.Model;
using WebApplication1.Model.VirtualModel;

namespace WebApplication1.Repository.Interface
{
    public interface ICategoryRepository
    {
        Response GetAll(string? search, string? sortBy, int page = 1, int pageSize = 5);
        Response getById(Guid id);
        Response getByName(String name);
        Response Add(CategoryModel categoryModel);
        Response Update(CategoryModel categoryModel);
        Response Delete(Guid id);
    }
}
