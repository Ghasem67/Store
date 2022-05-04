using FluentAssertions;
using Store.Entities;
using Store.Infrastracture.Tests;
using Store.Persistence.EF;
using Store.Persistence.EF.Categories;
using Store.Services.Categories;
using Store.Services.Categories.Contracts;
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
    [Scenario("ویرایش دسته بندی کالا")]
    [Feature("",
     AsA = "فروشنده",
        IWantTo = "مدیریت دسته بندی کالا داشته باشم",
        InOrderTo = "تا بتوانم کالا را ویرایش کنم")]
    public class UpdateCategory : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private UpdateCategoryDTO _dto;

        public UpdateCategory(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
        }
        [Given("دسته بندی با عنوان 'لبنیات' وجود دارد")]
        private void Given()
        {
            Category category = new Category
            {
                Title = "لبنیات"
            };
            _dataContext.Manipulate(_ => _.Add(category));

        }
        [When("عنوان دسته بندی 'لبنیات' را به 'خشکبار' تغییر می کند")]
        private void When()
        {
            var _unitOfWork = new EFUnitOfWork(_dataContext);
            _dto = new UpdateCategoryDTO
            {
                Title = "خشکبار"
            };
            CategoryRepository _categoryRepository = new EFCategoryRepository(_dataContext);
            CategoryService _sut = new CategoryAppService(_categoryRepository,
                _unitOfWork);
            var category = _dataContext.Categories.FirstOrDefault();
            _sut.Update(_dto, category.Id);
        }
        [Then("باید دسته بندی کالایی با عنوان 'خشکبار' در فهرست دسته بندی کالا وجود داشته باشد")]
        private void Then()
        {
            var expect = _dataContext.Categories.FirstOrDefault();
            expect.Title.Should().Be("خشکبار");
        }
        [Fact]
        public void Run()
        {
            Given();
            When();
            Then();
        }
    }
}
