using Store.Entities;
using Store.Persistence.EF;
using Store.Specs.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Store.Specs.BDDHelper;
using Store.Infrastracture.Tests;
using Store.Persistence.EF.Categories;
using Store.Services.Categories;
using FluentAssertions;
using Xunit;
using Store.Infrastracture.Application;
using Store.Services.Categories.Contracts;
using Store.Services.Goodses.Exceptions;
using Store.Services.Categories.Exceptions;

namespace Store.Specs.Categories
{
    [Scenario("حذف دسته بندی کالا")]
    [Feature("",
         AsA = "فروشنده",
         IWantTo = "مدیریت دسته بندی کالا داشته باشم",
         InOrderTo = "تا بتوانم کالا را حذف کنم")]
    public class DeleteCategory : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _context;
        private readonly UnitOfWork _unitOfWork;
        private readonly CategoryRepository categoRyrepository ;
        private readonly CategoryService _sut ;
        Action expect;
        Category category;
        public DeleteCategory(ConfigurationFixture configuration) : base(configuration)
        {
            _context = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_context);
            categoRyrepository = new EFCategoryRepository(_context);
            _sut = new CategoryAppService(categoRyrepository, _unitOfWork);
        }

        [Given("دسته بندی با عنوان 'لبنیات' در سیستم وجود دارد")]
        private void Given()
        {
             category = new Category
            {
                Title="لبنیات"
            };
            _context.Manipulate(_ => _.Add(category));
        }
        [When("درخواست حذف دسته بندی 'لبنیات' ارسال می کنیم")]
        private void When()
        {
           
            
            _sut.Delete(category.Id);
        }
        [Then("دسته بندی با عنوان 'لبنیات' حذف می شود")]
        private void Then()
        {
            var expect =_context.Categories.Where(x=>x.Id.Equals(category.Id)).ToList();
            expect.Should().HaveCount(0);

        }

        [Fact]
        private void Run()
        {
            Given();
            When();
            Then();
        }
    }
}
