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
    [Scenario("حذف دسته بندی کالا")]
    [Feature("",
         AsA = "فروشنده",
         IWantTo = "مدیریت دسته بندی کالا داشته باشم",
         InOrderTo = "تا بتوانم کالا را حذف کنم")]
    public class DeleteCategoryWithChild : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _context;
        private readonly UnitOfWork _unitOfWork;
        private readonly CategoryRepository _categoryRepository;
        private readonly CategoryService _sut;
        Action expect;
        Category category;
        public DeleteCategoryWithChild(ConfigurationFixture configuration) : base(configuration)
        {
            _context = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_context);
            _categoryRepository = new EFCategoryRepository(_context);
            _sut = new CategoryAppService(_categoryRepository, _unitOfWork);
        }
        [Given("دسته بندی با عنوان 'لبنیات' در سیستم وجود دارد")]
        private void Given()
        {
            category = new Category
            {
                Title = "لبنیات"
            };
            _context.Manipulate(_ => _.Categories.Add(category));
            Goods goods = new Goods
            {
                CategoryId = category.Id,
                Cost = 1000,
                GoodsCode = 0987,
                Inventory = 10,
                MaxInventory = 100,
                MinInventory = 10,
                Name = "شیر"
            };
            _context.Manipulate(_ => _.Goodses.Add(goods));
        }
        [When("درخواست حذف دسته بندی 'لبنیات' ارسال می کنیم")]
        private void When()
        {
            expect = () => _sut.Delete(category.Id);
        }
        [Then("دسته بندی با عنوان 'لبنیات' حذف نمی شود")]
        private void Then()
        {
            var expect = _context.Categories.Where(x => x.Id.Equals(category.Id)).ToList();
            expect.Should().HaveCount(1);
        }
        [And("خطا با عنوان 'دسته بندی  دارای فرزند می باشد' رخ می دهد")]
        private void AndThen()
        {
            expect.Should().ThrowExactly<CategoryHasChildrenException>();
        }
        [Fact]
        private void Run()
        {
            Runner.RunScenario(
                _ => Given()
                , _ => When()
                , _ => Then()
                , _ => AndThen());
        }
    }
}
