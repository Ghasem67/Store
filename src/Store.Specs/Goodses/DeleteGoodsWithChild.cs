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
    [Scenario("حذف کالا")]
    [Feature("",
          AsA = "فروشنده",
          IWantTo = "مدیریت کالا داشته باشیم",
          InOrderTo = "تا بتوانیم کالا را حذف کنیم")]
    public class DeleteGoodsWithChild: EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _context;
        private readonly UnitOfWork _unitOfWork;
        private readonly GoodsRepository _goodsRyrepository;
        private readonly GoodsService _sut;
        private  Goods goods;
        Action expect;

        public DeleteGoodsWithChild(ConfigurationFixture configuration) : base(configuration)
        {
            _context = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_context);
            _goodsRyrepository = new EFGoodsRepository(_context);

            _sut = new GoodsAppService(_goodsRyrepository, _unitOfWork);
        }
        [Given(" محصولی با نام 'شیر' در دسته بندی 'لبنیات'  وجود دارد")]
        private void Given()
        {
            Category category = new Category
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
        [And("فروخته شده")]
        private void AndGiven()
        {
            GoodsOutput output = new GoodsOutput()
            {
                GoodsCode = goods.GoodsCode,
                Count = 1,
                Date = DateTime.Now.Date,
                Number = 123,
                Price = 1000
            };
            _context.Manipulate(_ => _.GoodsOutputs.Add(output));
        }
        [When("درخواست حذف محصول 'شیر' از دسته بندی 'لبنیات' ارسال می شود")]
        private void When()
        {

            expect = () => _sut.Delete(goods.GoodsCode);
        }
        [Then("محصول 'شیر' حذف نمی شود")]
        private void Then()
        {
            var expect = _context.Goodses.Where(x => x.GoodsCode.Equals(goods.GoodsCode)).ToList();
            expect.Should().HaveCount(1);

        }
        [And("خطا با عنوان 'دسته بندی  دارای فرزند می باشد' رخ می دهد")]
        private void AndThen()
        {
            expect.Should().ThrowExactly<GoodsHasChildrenException>();
        }
        [Fact]
        private void Run()
        {
            Runner.RunScenario(
                _ => Given()
               , _ => AndGiven()
                , _ => When()
                , _ => Then()
                , _ => AndThen());
        }
    }
}
