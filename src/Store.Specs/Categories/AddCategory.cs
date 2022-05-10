using FluentAssertions;
using Store.Entities;
using Store.Infrastracture.Application;
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
    [Scenario("تعریف دسته بندی کالا")]
    [Feature(""
        , AsA = "فروشنده",
        IWantTo = "مدیریت دسته بندی کالا داشته باشم "
        , InOrderTo = "تا بتوانیم کالا ثبت کنم")]
    public class AddCategory : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly UnitOfWork _unitOfWork;
        private readonly CategoryRepository _categoryRepository;
        private readonly CategoryService _sut;
        Action expect;
        private AddCategoryDTO _dto;
        public AddCategory(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _categoryRepository = new EFCategoryRepository(_dataContext);
            _sut = new CategoryAppService(_categoryRepository, _unitOfWork);
        }
        [Given("هیچ دسته بندی در فهرست دسته بندی کالا وجود ندارد")]
        private void Given()
        {

        }
        [When("دسته بندی با عنوان 'لبنیات' تعریف می کنم")]
        private void When()
        {
            _dto = new AddCategoryDTO
            {
                Title = "لبنیات"
            };
            _sut.Add(_dto);
        }
        [Then("باید دسته بندی با عنوان 'لبنیات' وجود داشته باشد")]
        private void Then()
        {
            var expect = _dataContext.Categories.OrderByDescending(_ => _.Id).FirstOrDefault();
            expect.Title.Should().Be("لبنیات");
        }
        [Fact]
        public void Run()
        {
            Given();
            When();
            Then();
        }
        [Given("دسته بندی کالایی با عنوان 'لبنیات' وجود دارد")]
        private void DuplicateGiven()
        {
            Category category = new Category
            {
                Title = "لبنیات"
            };
            _dataContext.Manipulate(_ => _.Add(category));
        }
        [When("دسته بندی با عنوان 'لبنیات' تعریف می کنم")]
        private void DuplicateWhen()
        {
            _dto = new AddCategoryDTO
            {
                Title = "لبنیات"
            };
            expect = () => _sut.Add(_dto);
        }
        [Then("تنها یک دسته بندی با عنوان 'لبنیات' باید وجود داشته باشد")]
        private void DuplicateThen()
        {
            _dataContext.Categories.Where(_ => _.Title.Equals(_dto.Title)).Should().HaveCount(1);
        }
        [And("خطا با عنوان 'عنوان دسته بندی تکراری است' باید رخ دهد")]
        private void And()
        {
            expect.Should().ThrowExactly<DuplicateValueException>();
        }
        [Fact]
        private void DuplicateRun()
        {
            Runner.RunScenario(
                _=> DuplicateGiven()
                ,_=> DuplicateWhen()
                ,_=> DuplicateThen()
                ,_=>And());
        }
    }
}
