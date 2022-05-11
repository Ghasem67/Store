using Store.Entities;
using Store.Infrastracture.Application;
using Store.Services.Categories.Contracts;
using Store.Services.Categories.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.Categories
{
    public class CategoryAppService : CategoryService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly CategoryRepository _categoryRepository;

        public CategoryAppService(CategoryRepository categoryRepository, UnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public void Add(AddCategoryDTO addCategoryDTO)
        {
            var OneGoods = _categoryRepository.GetByTitle(addCategoryDTO.Title);
            if (OneGoods != null)
            {
                throw new DuplicateValueException();
            }
            Category category = new()
            {
                Title = addCategoryDTO.Title
            };
            _categoryRepository.Add(category);
            _unitOfWork.Commit();
        }

        public void Delete(int id)
        {
            var category = CheckIsNull(id);
            _categoryRepository.Delete(category);
            _unitOfWork.Commit();
        }

        public HashSet<ShowCategoryDTO> GetAll()
        {
          return  _categoryRepository.GetAll();
        }

        public ShowCategoryDTO GetById(int id)
        {
            return _categoryRepository.GetOne(id);
        }

        public void Update(UpdateCategoryDTO updateCategoryDTO,int id)
        {
            var category = CheckIsNull(id);
            category.Title = updateCategoryDTO.Title;
            _unitOfWork.Commit();
        }
        private Category CheckIsNull(int id)
        {
            var category = _categoryRepository.GetById(id);
            if (category == null)
            {
                throw new CategoryNotFoundException();
            }
            return category;
        }
    }
}
