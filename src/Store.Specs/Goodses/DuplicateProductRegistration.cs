using Store.Entities;
using Store.Infrastracture.Application;
using Store.Persistence.EF;
using Store.Persistence.EF.Goodses;
using Store.Services.Goodses;
using Store.Services.Goodses.Contracts;
using Store.Specs.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Store.Specs.BDDHelper;
using Xunit;
using Store.Infrastracture.Tests;
using FluentAssertions;
using Store.Services.Goodses.Exceptions;

namespace Store.Specs.Goodses
{
    [Scenario("تعریف کالا")]
    [Feature("",
          AsA = "فروشنده",
          IWantTo = "مدیریت   کالا داشته باشم",
          InOrderTo = "تا بنوانیم ورودی و خروجی کالا ثبت کنیم")]
    public class DuplicateProductRegistration: EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        UnitOfWork unitOfWork;
        GoodsRepository repository;
        GoodsService _sut;
        Action expect;
        Category category;

        public DuplicateProductRegistration(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            unitOfWork = new EFUnitOfWork(_dataContext);
            repository = new EFGoodsRepository(_dataContext);
            _sut = new GoodsAppService(repository, unitOfWork);
        }

        [Given("محصولی با عنوان 'شیر' در دسته بندی 'لبنیات' وجود دارد")]
        private void DuplicateGiven()
        {
            category = new Category()
            {
                Title = "لبنیات"
            };
            _dataContext.Manipulate(_ => _.Categories.Add(category));
            Goods goods = new Goods()
            {
                CategoryId = category.Id,
                Cost = 100,
                GoodsCode = 3,
                Inventory = 15,
                MaxInventory = 100,
                MinInventory = 14,
                Name = "شیر"
            };
            _dataContext.Manipulate(_ => _.Goodses.Add(goods));
        }
        [When("محصولی با عنوان 'شیر' در دسته بندی 'لبنیات' تعریف می کنم")]
        private void When()
        {
            AddGoodsDTO addGoodsDTO = new AddGoodsDTO()
            {
                CategoryId = category.Id,
                Cost = 100,
                GoodsCode = 5,
                Inventory = 10,
                MaxInventory = 100,
                MinInventory = 10,
                Name = "شیر"
            };
            expect = () => _sut.Add(addGoodsDTO);
        }
        [Then("تنها یک محصول  با عنوان 'شیر' باید در دسته بندی 'لبنیات' وجود داشته باشد")]
        private void Then()
        {
            _dataContext.Goodses.Where(_ => _.Category.Title.Equals("لبنیات") && _.Name.Equals("شیر"))
                .Should().HaveCount(1);
        }
        [Then("خطا با عنوان 'نام محصول تکراری است' باید رخ دهد")]
        private void AndThen()
        {
            expect.Should().ThrowExactly<DuplicateNameException>();
        }
        [Fact]
        private void DuplicateRun()
        {
            Runner.RunScenario(
                _ => DuplicateGiven(),
                _ => When(),
                _ => Then(),
                _ => AndThen()
                );
        }
    }
}
