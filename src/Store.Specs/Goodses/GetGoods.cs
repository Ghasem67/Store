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
    [Scenario("نمایش کالا")]
    [Feature("",
        AsA = "فروشنده",
        IWantTo = "مدیریت کالا داشته باشیم",
        InOrderTo = "تا بتوانیم اطلاعات را نمایش دهیم")]
    public class GetGoods : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private List<Goods> goodsList;
        public GetGoods(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
        }
        [Given("کالای شیر پگاه با قیمت 1000 ، حداقل موجودی 10 ،حداکثر موجودی 100 ، موجودی 15" +
            " کالای شیر رامک با قیمت 1000 ، حداقل موجودی 10 ،حداکثر موجودی 100 ، موجودی 15  ")]
        private void Given()
        {
            Category category = new Category()
            {
                Title = "لبنیات"
            };

            _dataContext.Manipulate(_ => _.Add(category));
             goodsList = new List<Goods>()
            {
                new Goods{CategoryId=category.Id,Cost=1000,MinInventory=10,MaxInventory=100,GoodsCode=21,Name="شیر پگاه",Inventory=15},
                new Goods{CategoryId=category.Id,Cost=1000,MinInventory=10,MaxInventory=100,GoodsCode=22,Name="شیر رامک",Inventory=15}

            };
            _dataContext.Manipulate(_=>_.Goodses.AddRange(goodsList));
        }
        [When("درخواست نمایش اطلاعات ارسال می شود")]
        private void When()
        {
            UnitOfWork unitOfWork = new EFUnitOfWork(_dataContext);
            GoodsRepository goodsRepository = new EFGoodsRepository(_dataContext);
            GoodsService goodsService = new GoodsAppService(goodsRepository, unitOfWork);
           var expect= goodsService.GetAll();
            expect.Should().Contain(_ => _.Name == goodsList[0].Name);
            expect.Should().Contain(_ => _.GoodsCode == goodsList[0].GoodsCode);
            expect.Should().Contain(_ => _.MaxInventory == goodsList[0].MaxInventory);
            expect.Should().Contain(_ => _.MinInventory == goodsList[0].MinInventory);
            expect.Should().Contain(_ => _.Cost == goodsList[0].Cost);
            expect.Should().Contain(_ => _.Inventory == goodsList[0].Inventory); 
            
            expect.Should().Contain(_ => _.Name == goodsList[1].Name);
            expect.Should().Contain(_ => _.GoodsCode == goodsList[1].GoodsCode);
            expect.Should().Contain(_ => _.MaxInventory == goodsList[1].MaxInventory);
            expect.Should().Contain(_ => _.MinInventory == goodsList[1].MinInventory);
            expect.Should().Contain(_ => _.Cost == goodsList[1].Cost);
            expect.Should().Contain(_ => _.Inventory == goodsList[1].Inventory);
        }
        [Then("فهرست محصولات نمایش داده می شود")]
        private void Then()
        {

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
