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
    [Scenario("")]
    [Feature("",
        AsA ="فروشنده",
        IWantTo = "مدیریت   کالا داشته باشم",
        InOrderTo ="تا بنوانیم ورودی و خروجی کالا ثبت کنیم")]
    public class AddGoods: EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        public AddGoods(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
        }

        private void Given()
        {

        }
        [When("کالایی با عنوان 'شیر' با کد 13 وحداقل موجودی 10 و حداکثر موجودی 100 و موجودی 12 در دسته بندی لبنیات در فهرست کالا تعریف می شود")]
        private void When()
        {
          var  _category = new Category
            {
                Title = "لبنیات"
            };

            _dataContext.Manipulate(_ => _.Categories.Add(_category));
            AddGoodsDTO dto = new AddGoodsDTO()
            {
                CategoryId=_dataContext.Categories.FirstOrDefault().Id,
                Cost=1000,
                GoodsCode=13,
                MaxInventory=100,
                Inventory=0,
                MinInventory=10,
                Name="شیر",
            };
            UnitOfWork unitOfWork = new EFUnitOfWork(_dataContext);
            GoodsRepository repository = new EFGoodsRepository(_dataContext);
           var _sut=new GoodsAppService(repository,unitOfWork);
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
    }
}
