using FluentAssertions;
using Store.Entities;
using Store.Infrastracture.Application;
using Store.Infrastracture.Tests;
using Store.Persistence.EF;
using Store.Persistence.EF.Categories;
using Store.Services.Categories;
using Store.Services.Categories.Contracts;
using Store.Services.Categories.Exceptions;
using Store.Services.GoodsInputs.Contracts;
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
    public class AddDuplicatesCategory: EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        UnitOfWork _unitOfWork;
        CategoryRepository categoryRepository;
        CategoryService _sut;
        private AddCategoryDTO _dto;
        Action expect;

        public AddDuplicatesCategory(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork=new EFUnitOfWork(_dataContext);
            categoryRepository=new EFCategoryRepository(_dataContext);
            _sut=new CategoryAppService(categoryRepository, _unitOfWork);
        }

        [Given("دسته بندی کالایی با عنوان 'لبنیات' وجود دارد")]
        private void Given()
        {
            Category category = new Category
            {
                Title = "لبنیات"
            };
            _dataContext.Manipulate(_ => _.Categories.Add(category));
        }
        [When("دسته بندی با عنوان 'لبنیات' تعریف می کنم")]
        private void When()
        {
            _dto = new AddCategoryDTO
            {
                Title = "لبنیات"
            };
            expect = () => _sut.Add(_dto);
        }
        [Then("تنها یک دسته بندی با عنوان 'لبنیات' باید وجود داشته باشد")]
        private void Then()
        {
            _dataContext.Categories.Where(_ => _.Title.Equals(_dto.Title)).Should().HaveCount(1);
        }
        [And("خطا با عنوان 'عنوان دسته بندی تکراری است' باید رخ دهد")]
        private void And()
        {
            expect.Should().ThrowExactly<DuplicateNameException>();
        }
        [Fact]
        private void Run()
        {

            Given();
            When();
           Then();
            And();
        }
    }
}
