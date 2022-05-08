using FluentAssertions;
using Store.Entities;
using Store.Infrastracture.Tests;
using Store.Persistence.EF;
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
        private Goods goods;
        public DeleteGoods(ConfigurationFixture configuration) : base(configuration)
        {
            _context = CreateDataContext();
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
            _context.Manipulate(_=>_.Goodses.Remove(goods));    

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
