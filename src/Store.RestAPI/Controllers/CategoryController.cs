using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Services.Categories.Contracts;
using System.Collections.Generic;

namespace Store.RestAPI.Controllers
{
    [Route("api/Category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryService _categoryService;

        public CategoryController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpGet]
        public HashSet<ShowCategoryDTO> GetAll()
        {
            return _categoryService.GetAll();
        }
        [HttpGet("{id}")]
        public ShowCategoryDTO GetById(int id)
        {
            return _categoryService.GetById(id);
        }
        [HttpPost]
        public void Add(AddCategoryDTO addCategoryDTO)
        {
            _categoryService.Add(addCategoryDTO);
        }
        [HttpPut("{id}")]
        public void Update(UpdateCategoryDTO updateCategoryDTO,int id)
        {
            _categoryService.Update(updateCategoryDTO, id);
        }
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _categoryService.Delete(id);
        }
    }
}
