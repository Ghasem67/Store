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
    [Scenario("ویرایش دسته بندی کالا")]
    [Feature("",
     AsA = "فروشنده",
        IWantTo = "مدیریت دسته بندی کالا داشته باشم",
        InOrderTo = "تا بتوانم کالا را ویرایش کنم")]
    public class UpdateCategory : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly CategoryRepository _categoryRepository;
        private readonly CategoryService _sut;
        private UpdateCategoryDTO _dto;
        private Category _category;
        Action expect;

        public UpdateCategory(ConfigurationFixture configuration) : base(configuration)
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
            Category category = new Category
            {
                Title = "لبنیات"
            };
            _dataContext.Manipulate(_ => _.Add(category));

        }
        [When("عنوان دسته بندی 'لبنیات' را به 'خشکبار' تغییر می کند")]
        private void When()
        {

            _dto = new UpdateCategoryDTO
            {
                Title = "خشکبار"
            };

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
        [Given("دسته بندی با عنوان 'لبنیات' وجود دارد")]
        private void DuplicateGiven()
        {
          Category  category = new Category()
            {
                Title = "لبنیات"
            };
            _dataContext.Manipulate(_ => _.Categories.Add(category));
        }
        [And("دسته بندی با عنوان 'خشکبار' وجود دارد")]
        private void DuplicateAnd()
        {
            _category = new Category()
            {
                Title = "خشکبار"
            };
            _dataContext.Manipulate(_ => _.Categories.Add(_category));
        }
        [When("عنوان دسته بندی 'لبنیات' را به 'خشکبار' تغییر می کند")]
        private void DuplicateWhen()
        {
            _dto = new UpdateCategoryDTO
            {
                Title = "خشکبار"
            };
          expect=()=>  _sut.Update(_dto, _category.Id);
        }
        [Then("تنها یک دسته بندی با  عنوان 'خشکبار' باید وجود داشته باشد ")]
        private void DuplicateThen()
        {
            _dataContext.Categories.Where(_ => _.Title.Equals(_category.Title)).Should().HaveCount(1);
        }
        [And("خطا با عنوان 'دسته بندی تکراری است' نمایش داده شود")]
        private void DuplicateAndThen()
        {
            expect.Should().ThrowExactly<DuplicateValueException>();
        }
        [Fact]
        private void DuplicateRun()
        {
            Runner.RunScenario(
                _ => DuplicateGiven()
                , _ => DuplicateWhen()
                , _ => DuplicateThen()
                , _ => DuplicateAndThen());
        }
    }
}
