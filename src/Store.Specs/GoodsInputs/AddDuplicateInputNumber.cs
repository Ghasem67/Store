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
    public class AddDuplicateInputNumber : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        UnitOfWork _unitOfWork;
        GoodsInputRepository goodsInputRepository;
        GoodsInputService _sut;
        Action expect;
        public AddDuplicateInputNumber(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            goodsInputRepository = new EFGoodsInputRepository(_dataContext);
            _sut = new GoodsInputAppService(_unitOfWork, goodsInputRepository);
        }
        [Given("ورود کالا 'شیر' به تعداد '2 عدد' قیمت '2000' به شماره' 12' ثبت   می شود")]
        private void Given()
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
            GoodsInput goodsInput = new GoodsInput
            {
                Number = 12,
                Count = 2,
                Date = new DateTime(2022, 4, 5, 0, 0, 0, 0),
                GoodsCode = 12,
                Price = 1000
            };

            _dataContext.Manipulate(_ => _.GoodsInputs.Add(goodsInput));
        }
        [When("ورود کالا با شماره'12' ثبت می شود")]
        private void When()
        {
            AddGoodsInputDTO goodsInput = new AddGoodsInputDTO
            {
                Number = 12,
                Count = 2,
                Date = "2022 - 4 - 5",
                GoodsCode = 12,
                Price = 1000
            };

            expect = () => _sut.Add(goodsInput);
        }
        [Then("خطایی با عنوان ‘ورودی کالا با عنوان 12 وجود دارد’ رخ می دهد")]
        private void Then()
        {
            expect.Should().ThrowExactly<DuplicateFactorNumberException>();
        }
        [Fact]
        private void DuplicateRun()
        {
            Runner.RunScenario(
                _ => Given(),
                _ => When(),
                _ => Then()
                );
        }
    }
}
