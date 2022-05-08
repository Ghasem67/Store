using FluentAssertions;
using Store.Entities;
using Store.Infrastracture.Application;
using Store.Infrastracture.Tests;
using Store.Persistence.EF;
using Store.Persistence.EF.GoodsOutputs;
using Store.Services.GoodsOutputs;
using Store.Services.GoodsOutputs.Contracts;
using Store.Specs.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static Store.Specs.BDDHelper;

namespace Store.Specs.GoodsOutputs
{
    [Scenario("حذف خروجی کالا")]
    [Feature("",
          AsA = "فروشنده",
          IWantTo = "مدیریت   کالا داشته باشم",
          InOrderTo = "تا بتوانیم خروجی  کالا حذف کنیم")]
    public class DeleteGoodsOutput : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _context;
        public DeleteGoodsOutput(ConfigurationFixture configuration) : base(configuration)
        {
            _context = CreateDataContext();
        }
        [Given("خروجی کالا با شماره '12' در سیستم وجود دارد")]
        private void Given()
        {
            Category category = new Category()
            {
                Title = "لبنیات"
            };
            _context.Manipulate(_ => _.Categories.Add(category));
            Goods goods = new Goods()
            {
                CategoryId = category.Id,
                Cost = 1000,
                GoodsCode = 29,
                Inventory = 123,
                MaxInventory = 1000,
                MinInventory = 10,
                Name = "نسکافه"
            };
            _context.Manipulate(_ => _.Goodses.Add(goods));
            GoodsOutput goodsOutput = new GoodsOutput()
            {
                Count = 1,
                Date = new DateTime(2022, 2, 3, 0, 0, 0, 0),
                GoodsCode = 29,
                Number = 12,
                Price = 1000,
            };
            _context.Manipulate(_ => _.GoodsOutputs.Add(goodsOutput));
        }
        [When("درخواست حذف خروجی کالا با شماره '12' ارسال می کنیم")]
        private void When()
        {
            UnitOfWork _unitOfWork = new EFUnitOfWork(_context);
            GoodsOutputRepository goodsOutputRepository = new EFGoodsOutPutRepository(_context);
            var _sut = new GoodsOutputAppService(_unitOfWork, goodsOutputRepository);
            _sut.Delete(12);
        }
        [Then("خروجی کالا با شماره '12' حذف می شود")]
        private void Then()
        {
            var expect = _context.GoodsOutputs.OrderByDescending(x => x.Date).FirstOrDefault(_ => _.Number.Equals(12));
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
