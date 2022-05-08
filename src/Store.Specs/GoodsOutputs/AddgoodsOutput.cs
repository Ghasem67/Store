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
    [Scenario("تعریف خروجی کالا")]
    [Feature("",
        AsA = "فروشنده",
        IWantTo = "مدیریت   کالا داشته باشم",
        InOrderTo = "تا بتوانیم خروجی  کالا ثبت کنیم")]
    public class AddGoodsoutput : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        public AddGoodsoutput(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
        }
        [Given("هیچ خریدی  وجود ندارد")]
        private void Given()
        {

        }
        [When("خروج کالا 'شیر' به تعداد '2 عدد' قیمت '2000' به شماره' 14' ثبت   می شود")]
        private void When()
        {
            var _category = new Category
            {
                Title = "لبنیات"
            };

            _dataContext.Manipulate(_ => _.Categories.Add(_category));
            Goods dto = new Goods()
            {
                CategoryId = _dataContext.Categories.FirstOrDefault().Id,
                Cost = 1000,
                GoodsCode = 28,
                MaxInventory = 100,
                Inventory = 0,
                MinInventory = 10,
                Name = "شیر",
            };
            _dataContext.Manipulate(_ => _.Goodses.Add(dto));
            AddgoodsoutputDTO goodsoutput = new AddgoodsoutputDTO
            {
                Number = 28,
                Count = 2,
                Date = "2022 - 4 - 5",
                GoodsCode = 1,
                Price = 1000
            };
            UnitOfWork _unitOfWork = new EFUnitOfWork(_dataContext);
            EFGoodsOutPutRepository goodsOutputRepository = new EFGoodsOutPutRepository(_dataContext);
            var _sut = new GoodsOutputAppService(_unitOfWork, goodsOutputRepository);
            _sut.Add(goodsoutput);
            }
        [Then("باید خروج کالای 'شیر' به تعداد '2 عدد' قیمت '2000' به شماره' 12' وجود داشته باشد")]
        private void Then()
        {
            var expect = _dataContext.GoodsOutputs.OrderByDescending(_ => _.Date).FirstOrDefault();
            expect.Number.Should().Be(12);
            expect.Date.Should().Be(new DateTime(2022, 4, 5, 0, 0, 0, 0));
            expect.Price.Should().Be(1000);
            expect.GoodsCode.Should().Be(1);
            expect.Count.Should().Be(2);
        }
        [Fact]
        private void Run()
        {
           Given();
           When();
            //Then();
        }
    }
}
