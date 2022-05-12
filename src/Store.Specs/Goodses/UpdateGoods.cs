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
    [Scenario("ویرایش کالا")]
    [Feature("",
           AsA = "فروشنده",
           IWantTo = "مدیریت   کالا داشته باشم",
           InOrderTo = "تا بنوانیم ورودی و خروجی کالا ثبت کنیم")]
    public class UpdateGoods: EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _context;
        GoodsRepository goodsRepository;
        UnitOfWork unitOfWork;
      GoodsService   _sut;
        private Category category;
        private UpdateGoodsDTO updateGoodsDTO;
        private Goods goods;
        Action expect;
        public UpdateGoods(ConfigurationFixture configuration) : base(configuration)
        {
            _context = CreateDataContext();
            goodsRepository = new EFGoodsRepository(_context);
            unitOfWork = new EFUnitOfWork(_context);
            _sut = new GoodsAppService(goodsRepository, unitOfWork);
        }

        [Given("کالایی با نام 'شیر' در دسته بندی 'لبنیات'  وجود دارد")]
        private void Given()
        {
            category = new Category()
            {
                Title = "لبنیات"
            };
            _context.Manipulate(_ => _.Categories.Add(category));
            Goods goods = new Goods()
            {
                Name = "شیر",
                CategoryId = _context.Categories.First().Id,
                Cost=1000,
                GoodsCode=19,
                Inventory=0,
                MaxInventory=1000,
                MinInventory=100,
                
            };
            _context.Manipulate(_ => _.Goodses.Add(goods));
        }
        [When("کالایی 'شیر' به 'ماست' تغییر می کند")]
        private void When()
        {
            updateGoodsDTO = new UpdateGoodsDTO
            {
                Name="ماست",
                CategoryId= _context.Categories.First().Id,
                MaxInventory=123,
                Cost=2000,
                MinInventory=12,
                Inventory=0,
            };
           
            _sut.Update(updateGoodsDTO, _context.Goodses.FirstOrDefault().GoodsCode);
        }
        [Then("باید کالایی 'ماست' در دسته بندی لبنیات وجود داشته باشد ")]
        public void Then()
        {
           var expect= _context.Goodses.FirstOrDefault();
            expect.Name.Should().Be("ماست");
        }
        [Fact]
        private void Run()
        {
            Given();
            When();
            Then();
        }
        [Given("محصول’شیر’ در دسته بندی لبنیات وجود دارد")]
        private void DuplicateGiven()
        {
             category = new Category
            {
                Title = "لبنیات"
            };
            _context.Manipulate(_ => _.Categories.Add(category));
            goods = new Goods
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
        [And("محصول 'ماست' در دسته بندی 'لبنیات' وجود دارد")]
        private void DuplicateAndGiven()
        {
            Goods goods = new Goods
            {
                CategoryId = category.Id,
                Cost = 1000,
                GoodsCode = 0988,
                Inventory = 10,
                MaxInventory = 100,
                MinInventory = 10,
                Name = "ماست"
            };
            _context.Manipulate(_ => _.Goodses.Add(goods));
        }
        [When("محصول 'شیر' را به 'ماست' تغییر می دهم")]
        private void DuplicateWhen()
        {
            updateGoodsDTO = new UpdateGoodsDTO
            {
                Name = "ماست",
                CategoryId = _context.Categories.First().Id,
                MaxInventory = 123,
                Cost = 2000,
                MinInventory = 12,
                Inventory = 0,
            };
          expect=()=>  _sut.Update(updateGoodsDTO, goods.GoodsCode);
        }
        [Then("تنها یک محصول  با عنوان 'ماست ' باید در دسته بندی 'لبنیات' وجود داشته باشد")]
        private void DuplicateThen()
        {
            _context.Goodses.Where(_ => _.Category.Title.Equals("لبنیات") && _.Name.Equals("ماست"))
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
                _ => DuplicateGiven(),
                _ => DuplicateAndGiven(),
                _ => DuplicateWhen(),
                _ => DuplicateThen(),
                _ => DuplicateAndThen()
                ); ;
        }
    }
}
