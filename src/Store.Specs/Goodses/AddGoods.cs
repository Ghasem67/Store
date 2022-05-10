using FluentAssertions;
using Store.Entities;
using Store.Infrastracture.Application;
using Store.Infrastracture.Tests;
using Store.Persistence.EF;
using Store.Persistence.EF.Goodses;
using Store.Services.Goodses;
using Store.Services.Goodses.Contracts;
using Store.Services.Goodses.Exceptions;
using Store.Specs.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static Store.Specs.BDDHelper;

namespace Store.Specs.Goodses
{
    [Scenario("تعریف کالا")]
    [Feature("",
        AsA = "فروشنده",
        IWantTo = "مدیریت   کالا داشته باشم",
        InOrderTo = "تا بنوانیم ورودی و خروجی کالا ثبت کنیم")]
    public class AddGoods : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        UnitOfWork unitOfWork;
        GoodsRepository repository;
        GoodsService _sut;
        Action expect;
        Category category;
        public AddGoods(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            unitOfWork = new EFUnitOfWork(_dataContext);
            repository = new EFGoodsRepository(_dataContext);
            _sut = new GoodsAppService(repository, unitOfWork);
        }

        private void Given()
        {

        }
        [When("کالایی با عنوان 'شیر' با کد 13 وحداقل موجودی 10 و حداکثر موجودی 100 و موجودی 12 در دسته بندی لبنیات در فهرست کالا تعریف می شود")]
        private void When()
        {
            var _category = new Category
            {
                Title = "لبنیات"
            };

            _dataContext.Manipulate(_ => _.Categories.Add(_category));
            AddGoodsDTO dto = new AddGoodsDTO()
            {
                CategoryId = _dataContext.Categories.FirstOrDefault().Id,
                Cost = 1000,
                GoodsCode = 13,
                MaxInventory = 100,
                Inventory = 0,
                MinInventory = 10,
                Name = "شیر",
            };

            _sut.Add(dto);
        }
        [Then("باید تنها کالایی با عنوان 'شیر' با کد 13 وحداقل موجودی 10 و حداکثر موجودی 100  در دسته بندی لبنیات وجود داشته باشد  ")]
        private void Then()
        {
            var expect = _dataContext.Goodses.FirstOrDefault();
            expect.Category.Title.Should().Be("لبنیات");
            expect.GoodsCode.Should().Be(13);
            expect.MinInventory.Should().Be(10);
            expect.MaxInventory.Should().Be(100);

        }
        [Fact]
        private void Run()
        {
            Given();
            When();
            Then();
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
        private void DuplicateWhen()
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
            expect=()=>_sut.Add(addGoodsDTO);
        }
        [Then("تنها یک محصول  با عنوان 'شیر' باید در دسته بندی 'لبنیات' وجود داشته باشد")]
        private void DuplicateThen()
        {
            _dataContext.Goodses.Where(_ => _.Category.Title.Equals("لبنیات") && _.Name.Equals("شیر"))
                .Should().HaveCount(1);
        }
        [Then("خطا با عنوان 'نام محصول تکراری است' باید رخ دهد")]
        private void DuplicateAndThen()
        {
            expect.Should().ThrowExactly<DuplicateNameException>();
        }
        [Fact]
        private void DuplicateRun()
        {
            Runner.RunScenario(
                _=>DuplicateGiven(),
                _=>DuplicateWhen(),
                _=>DuplicateThen(),
                _=>DuplicateAndThen()
                );
        }
    }
}
