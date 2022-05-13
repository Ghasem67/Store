using FluentAssertions;
using Store.Entities;
using Store.Infrastracture.Application;
using Store.Infrastracture.Tests;
using Store.Persistence.EF;
using Store.Persistence.EF.Goodses;
using Store.Services.Categories;
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
    [Scenario("حذف کالا")]
    [Feature("",
        AsA = "فروشنده",
        IWantTo = "مدیریت کالا داشته باشیم",
        InOrderTo = "تا بتوانیم کالا را حذف کنیم")]
    public class DeleteGoods : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _context;
        UnitOfWork _unitOfWork;
        GoodsRepository goodsRyrepository;
       GoodsService _sut;
        private Goods goods;
        public DeleteGoods(ConfigurationFixture configuration) : base(configuration)
        {
            _context = CreateDataContext();
           _unitOfWork = new EFUnitOfWork(_context);
            goodsRyrepository = new EFGoodsRepository(_context);

            _sut = new GoodsAppService(goodsRyrepository, _unitOfWork);
        }
        [Given("کالایی با نام 'شیر' با قیمت 1000 و حداکثر موجودی 1000 و حداقل موجودی 100 در دسته بندی 'لبنیات'  وجود دارد")]
        private void Given()
        {
            Category category = new Category()
            {
                Title = "لبنیات"
            };
            _context.Manipulate(_ => _.Categories.Add(category));
            goods = new Goods()
            {
                Name = "شیر",
                Cost = 1000,
                MinInventory = 10,
                MaxInventory = 100,
                CategoryId = _context.Categories.First().Id,
                GoodsCode = 15,
                Inventory = 12
            };
            _context.Manipulate(_ => _.Goodses.Add(goods));
        }

        [When("درخواست حذف کالا 'شیر' از دسته بندی 'لبنیات' ارسال می کنیم")]
        private void When()
        {
            _sut.Delete(goods.GoodsCode);    

        }
        [Then("کالا 'شیر' از دسته بندی 'لبنیات' حذف می شود")]
        private void Then()
        {
            var expect = _context.Goodses.FirstOrDefault(_ => _.GoodsCode.Equals(goods.GoodsCode));
            expect.Should().BeNull();

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
