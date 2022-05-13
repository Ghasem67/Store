using FluentAssertions;
using Store.Entities;
using Store.Infrastracture.Application;
using Store.Infrastracture.Tests;
using Store.Persistence.EF;
using Store.Persistence.EF.GoodsInputs;
using Store.Services.GoodsInputs;
using Store.Services.GoodsInputs.Contracts;
using Store.Services.GoodsInputs.Exceptions;
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
    [Scenario("تعریف ورودی کالا")]
    [Feature("",
        AsA = "فروشنده",
        IWantTo = "مدیریت   کالا داشته باشم",
        InOrderTo = "تا بتوانیم ورودی  کالا ثبت کنیم")]
    public class AddGoodsInput : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        UnitOfWork _unitOfWork;
        GoodsInputRepository goodsInputRepository;
        GoodsInputService _sut;
        public AddGoodsInput(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            goodsInputRepository = new EFGoodsInputRepository(_dataContext);
            _sut = new GoodsInputAppService(_unitOfWork, goodsInputRepository);
        }
        [Given("هیچ خریدی  وجود ندارد")]
        private void Given()
        {

        }
        [When("ورود کالا 'شیر' به تعداد '2 عدد' قیمت '2000' به شماره' 14' ثبت   می شود")]
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
                GoodsCode = 12,
                MaxInventory = 100,
                Inventory = 0,
                MinInventory = 10,
                Name = "شیر",
            };
            _dataContext.Manipulate(_ => _.Goodses.Add(dto));
            AddGoodsInputDTO goodsInput = new AddGoodsInputDTO
            {
                Number = 14,
                Count = 2,
                Date = "2022 - 4 - 5",
                GoodsCode = 12,
                Price = 1000
            };

            _sut.Add(goodsInput);
        }
        [Then("باید ورود کالای 'شیر' به تعداد '2 عدد' قیمت '2000' به شماره' 12' وجود داشته باشد")]
        private void Then()
        {
            var expect = _dataContext.GoodsInputs.FirstOrDefault();
            expect.Number.Should().Be(14);
            expect.Date.Should().Be(new DateTime(2022, 4, 5, 0, 0, 0, 0));
            expect.Price.Should().Be(1000);
            expect.GoodsCode.Should().Be(12);
            expect.Count.Should().Be(2);
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
