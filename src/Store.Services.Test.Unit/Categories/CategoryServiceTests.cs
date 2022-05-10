using FluentAssertions;
using Store.Entities;
using Store.Infrastracture.Application;
using Store.Infrastracture.Tests;
using Store.Persistence.EF;
using Store.Persistence.EF.Categories;
using Store.Services.Categories;
using Store.Services.Categories.Contracts;
using Store.Services.Categories.Exceptions;
using Store.Test.Tools.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Store.Services.Test.Unit.Categories
{
    public class CategoryServiceTests
    {
        private readonly EFDataContext _context;
        private readonly UnitOfWork _unitOfWork;
        private readonly CategoryRepository _categoryRepository;
        private readonly CategoryService _Sut;
        public CategoryServiceTests()
        {
            _context = new EFInMemoryDatabase()
                .CreateDataContext<EFDataContext>();
            _unitOfWork = new EFUnitOfWork(_context);
            _categoryRepository = new EFCategoryRepository(_context);
            _Sut = new CategoryAppService(_categoryRepository, _unitOfWork);
        }
        [Fact]
        private void Add_adds_category_properly()
        {
            AddCategoryDTO addCategoryDTO = GenerateAddCategoryDto();
            _Sut.Add(addCategoryDTO);
            _context.Categories.Should().Contain(x => x.Title == addCategoryDTO.Title);
        }
        [Fact]
        private void Update_updates_category_properly()
        {

            var category = CategoryFactory.CreateCategory("ghalat");
            _context.Manipulate(_ => _.Categories.Add(category));
            UpdateCategoryDTO updateCategoryDTO = GenerateUpdateCategoryDto("labaniat");
            _Sut.Update(updateCategoryDTO, category.Id);
            var expect = _context.Categories.FirstOrDefault(_ => _.Id.Equals(category.Id));
            expect.Title.Should().Be(updateCategoryDTO.Title);
        }

        [Fact]
        private void Delete_Delete_category_properly()
        {
            var category = CategoryFactory.CreateCategory("khoshkbar");
            _context.Manipulate(_ => _.Categories.Add(category));
            _Sut.Delete(category.Id);
            _context.Categories.Should().HaveCount(0);
        }
        [Fact]
        private void GetById_getbyids_category_properly()
        {
            Category category = new Category()
            {
                Title = "نان"
            };
            _context.Manipulate(_ => _.Categories.Add(category));
            var expect = _Sut.GetById(category.Id);
            expect.Title.Should().Contain(category.Title);
        }
        [Fact]
        private void GetAll_return_all_categories()
        {
            CreateCategoriesInDataBase();
            var expected = _Sut.GetAll();
            expected.Should().HaveCount(3);
            expected.Should().Contain(_ => _.Title == "khoshkbar");
            expected.Should().Contain(_ => _.Title == "labaniat");
            expected.Should().Contain(_ => _.Title == "Mobile");
        }
        [Fact]
        private void Update_throw_Category_NotFoundException_when_category_with_given_id_is_not_exist()
        {
            var categoryId = 2000;
            var categoryDTO = GenerateUpdateCategoryDto("editcategory");
            Action expect = () => _Sut.Update(categoryDTO, categoryId);
            expect.Should().ThrowExactly<CategoryNotFoundException>();
        }
        [Fact]
        private void Delete_throw_Category_NotFoundException_When_category_with_given_id_is_not_exist()
        {
            var categoryId = 2000;
            Action expect = () => _Sut.Delete(categoryId);
            expect.Should().ThrowExactly<CategoryNotFoundException>();
        }
        [Fact]
        private void Add_adds_throw_IsExistException_when_add_title_Category_is_exist()
        {
            var category = CategoryFactory.CreateCategory("khoshkbar");
            _context.Manipulate(_ => _.Categories.Add(category));
            AddCategoryDTO updateCategoryDTO = new AddCategoryDTO()
            {

                Title = "khoshkbar"
            };
            Action expect = () => _Sut.Add(updateCategoryDTO);
            expect.Should().ThrowExactly<DuplicateValueException>();
        }
        private static AddCategoryDTO GenerateAddCategoryDto()
        {
            return new AddCategoryDTO
            {
                Title = "Labaniat"
            };
        }
        private static UpdateCategoryDTO GenerateUpdateCategoryDto(string title)
        {
            return new UpdateCategoryDTO
            {
                Title = title,
            };
        }
        private void CreateCategoriesInDataBase()
        {
            var categories = new List<Category>
            {
                new Category { Title = "khoshkbar"},
                new Category { Title = "labaniat"},
                new Category { Title = "Mobile"}
            };
            _context.Manipulate(_ =>
            _.Categories.AddRange(categories));
        }
    }
}
