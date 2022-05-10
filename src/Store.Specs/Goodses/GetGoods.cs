using FluentAssertions;
using Store.Entities;
using Store.Services.Goodses.Exceptions;
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
        UnitOfWork unitOfWork ;
        GoodsRepository goodsRepository ;
        GoodsService _sut;
        private List<Goods> goodsList;
        Action Expect;
        private HashSet<ShowgoodsDTO> goodsHashset;
        public GetGoods(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            unitOfWork = new EFUnitOfWork(_dataContext);
            goodsRepository = new EFGoodsRepository(_dataContext);
            _sut = new GoodsAppService(goodsRepository, unitOfWork);
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
           
            goodsHashset = _sut.GetAll();
           
        }
        [Then("فهرست محصولات نمایش داده می شود")]
        private void Then()
        {
            goodsHashset.Should().Contain(_ => _.Name == goodsList[0].Name);
            goodsHashset.Should().Contain(_ => _.GoodsCode == goodsList[0].GoodsCode);
            goodsHashset.Should().Contain(_ => _.MaxInventory == goodsList[0].MaxInventory);
            goodsHashset.Should().Contain(_ => _.MinInventory == goodsList[0].MinInventory);
            goodsHashset.Should().Contain(_ => _.Cost == goodsList[0].Cost);
            goodsHashset.Should().Contain(_ => _.Inventory == goodsList[0].Inventory);

            goodsHashset.Should().Contain(_ => _.Name == goodsList[1].Name);
            goodsHashset.Should().Contain(_ => _.GoodsCode == goodsList[1].GoodsCode);
            goodsHashset.Should().Contain(_ => _.MaxInventory == goodsList[1].MaxInventory);
            goodsHashset.Should().Contain(_ => _.MinInventory == goodsList[1].MinInventory);
            goodsHashset.Should().Contain(_ => _.Cost == goodsList[1].Cost);
            goodsHashset.Should().Contain(_ => _.Inventory == goodsList[1].Inventory);
        }
        [Fact]
        private void Run()
        {
            Given();
            When();
            Then();
        }
        [Given("کالایی در سیستم وجود ندارد")]
        private void NotHaveGiven()
        {

        }

        [When("درخواست نمایش اطلاعات ارسال می شود")]
        private void NotHaveWhen()
        {

            Expect = () => _sut.GetAll();
        }
        [Then(" خطایی با عنوان 'اطلاعاتی جهت نمایش وجود ندارد' در سیستم رخ می دهد")]
        private void NotHaveThen()
        {
            Expect.Should().ThrowExactly<ThereIsnotInformationToDisplay>();
        }
        [Fact]
        private void NotHaveRun()
        {
            Runner.RunScenario(
            _ => NotHaveGiven(),
            _ => NotHaveWhen(),
            _ => NotHaveThen()
            );
        }
    }
}
