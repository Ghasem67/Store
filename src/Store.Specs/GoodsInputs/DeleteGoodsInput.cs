using FluentAssertions;
using Store.Entities;
using Store.Infrastracture.Application;
using Store.Infrastracture.Tests;
using Store.Persistence.EF;
using Store.Persistence.EF.GoodsInputs;
using Store.Services.GoodsInputs;
using Store.Services.GoodsInputs.Contracts;
using Store.Specs.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static Store.Specs.BDDHelper;

namespace Store.Specs.GoodsInputs
{
    [Scenario("")]
    [Feature("",
        AsA = "فروشنده",
        IWantTo = "مدیریت   کالا داشته باشم",
        InOrderTo = "تا بتوانیم ورودی  کالا حذف کنیم")]
    public class DeleteGoodsInput : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _context;

        public DeleteGoodsInput(ConfigurationFixture configuration) : base(configuration)
        {
            _context = CreateDataContext();
        }

        [Given("ورود کالا با شماره '12' در سیستم وجود دارد")]
        private void Given()
        {
            Category category = new Category()
            {
                Title = "لبنیات"
            };
            _context.Manipulate(_=>_.Categories.Add(category));
            Goods goods = new Goods()
            {
                CategoryId= category.Id,
                Cost=1000,
                GoodsCode=14,
                Inventory=123,
                MaxInventory=1000,
                MinInventory=10,
                Name="نسکافه"
            };
            _context.Manipulate(_=>_.Goodses.Add(goods));
            GoodsInput goodsInput = new GoodsInput()
            {
                Count=1,
                Date=new DateTime(2022,2,3,0,0,0,0),
                GoodsCode=14,
                Number=12,
                Price=1000,
            };
            _context.Manipulate(_ => _.GoodsInputs.Add(goodsInput));
        }
        [When("درخواست حذف ورود کالا با شماره '12' ارسال می کنیم")]
        private void When()
        {
            UnitOfWork _unitOfWork = new EFUnitOfWork(_context);
            GoodsInputRepository goodsInputRepository = new EFGoodsInputRepository(_context);
            var _sut = new GoodsInputAppService(_unitOfWork, goodsInputRepository);
            _sut.Delete(12);
        }
        [Then("ورود کالا با شماره '12' حذف می شود")]
        private void Then()
        {
            var expect = _context.GoodsInputs.OrderByDescending(x => x.Date).FirstOrDefault(_ => _.Number.Equals(12));
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
