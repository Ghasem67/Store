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
        

        public DeleteCategory(ConfigurationFixture configuration) : base(configuration)
        {
            _context = CreateDataContext();
        }

        [Given("دسته بندی با عنوان 'لبنیات' در سیستم وجود دارد")]
        private void Given()
        {
            Category category = new Category
            {
                Title="لبنیات"
            };
            _context.Manipulate(_ => _.Add(category));
        }
        [When("درخواست حذف دسته بندی 'لبنیات' ارسال می کنیم")]
        private void When()
        {
            var _unitOfWork=new EFUnitOfWork(_context);
            var categoRyrepository=new EFCategoryRepository(_context);
            var _sut = new CategoryAppService(categoRyrepository, _unitOfWork);
            var id = _context.Categories.FirstOrDefault().Id;
            _sut.Delete(id);
        }
        [Then("دسته بندی با عنوان 'لبنیات' حذف می شود")]
        private void Then()
        {
            var expect =_context.Categories.ToList();
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
