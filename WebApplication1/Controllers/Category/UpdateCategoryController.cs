﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Model;
using WebApplication1.Repository.Interface;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers.Category
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpdateCategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public UpdateCategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "UpdateCategoryAccess")]
        public IActionResult UpdateCategoryById(Guid id, CategoryModel CategoryModel)
        {
            {
                try
                {
                    _categoryRepository.Update(CategoryModel);
                    return NoContent();
                }
                catch
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }

            }

        }
    }
}
