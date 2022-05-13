using FluentAssertions;
using Store.Entities;
using Store.Infrastracture.Tests;
using Store.Persistence.EF;
using Store.Persistence.EF.Categories;
using Store.Services.Categories;
using Store.Services.Categories.Contracts;
using Store.Services.Categories.Exceptions;
using Store.Specs.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static Store.Specs.BDDHelper;

namespace Store.Specs.Categories
{
    public class UpdateDuplicateTitleNamesCategory : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly CategoryRepository _categoryRepository;
        private readonly CategoryService _sut;
        private UpdateCategoryDTO _dto;
        private Category _category;
        Action expect;
        public UpdateDuplicateTitleNamesCategory(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            var _unitOfWork = new EFUnitOfWork(_dataContext);
            _categoryRepository = new EFCategoryRepository(_dataContext);
            _sut = new CategoryAppService(_categoryRepository,
               _unitOfWork);
        }
        [Given("دسته بندی با عنوان 'لبنیات' وجود دارد")]
        private void Given()
        {
            Category category = new Category()
            {
                Title = "لبنیات"
            };
            _dataContext.Manipulate(_ => _.Categories.Add(category));
        }
        [And("دسته بندی با عنوان 'خشکبار' وجود دارد")]
        private void And()
        {
            _category = new Category()
            {
                Title = "خشکبار"
            };
            _dataContext.Manipulate(_ => _.Categories.Add(_category));
        }
        [When("عنوان دسته بندی 'لبنیات' را به 'خشکبار' تغییر می کند")]
        private void When()
        {
            _dto = new UpdateCategoryDTO
            {
                Title = "خشکبار"
            };
            expect = () => _sut.Update(_dto, _category.Id);
        }
        [Then("تنها یک دسته بندی با  عنوان 'خشکبار' باید وجود داشته باشد ")]
        private void Then()
        {
            _dataContext.Categories.Where(_ => _.Title.Equals(_category.Title)).Should().HaveCount(1);
        }
        [And("خطا با عنوان 'دسته بندی تکراری است' نمایش داده شود")]
        private void AndThen()
        {
            expect.Should().ThrowExactly<DuplicateNameException>();
        }
        [Fact]
        private void Run()
        {
            Runner.RunScenario(
                _ => Given()
               , _ => And()
                , _ => When()
                , _ => Then()
                , _ => AndThen());
        }
    }
}
