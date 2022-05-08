using FluentAssertions;
using Store.Entities;
using Store.Infrastracture.Application;
using Store.Infrastracture.Tests;
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
        private Category category;
        private UpdateGoodsDTO updateGoodsDTO;
        public UpdateGoods(ConfigurationFixture configuration) : base(configuration)
        {
            _context = CreateDataContext();
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
            GoodsRepository goodsRepository=new EFGoodsRepository(_context);
            UnitOfWork unitOfWork=new EFUnitOfWork(_context);
            var _sut=new GoodsAppService(goodsRepository,unitOfWork);
            _sut.Update(updateGoodsDTO, _context.Goodses.FirstOrDefault().GoodsCode);
        }
        [Then("باید کالایی 'ماست' در دسته بندی لبنیات وجود داشته باشد ")]
        public void then()
        {
           var expect= _context.Goodses.FirstOrDefault();
            expect.Name.Should().Be("ماست");
        }
        [Fact]
        private void Run()
        {
            Given();
            When();
            then();
        }
    }
}
