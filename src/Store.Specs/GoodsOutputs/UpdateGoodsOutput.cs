using FluentAssertions;
using Store.Entities;
using Store.Infrastracture.Application;
using Store.Infrastracture.Tests;
using Store.Persistence.EF;
using Store.Persistence.EF.GoodsOutputs;
using Store.Services.GoodsOutputs;
using Store.Services.GoodsOutputs.Contracts;
using Store.Services.GoodsOutputs.Exceptions;
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
    [Scenario("ویرایش خروجی کالا")]
    [Feature("",
          AsA = "فروشنده",
          IWantTo = "مدیریت   کالا داشته باشم",
          InOrderTo = "تا بتوانیم خروجی  کالا را ویرایش کنیم")]
    public class UpdateGoodsOutput : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _context;
        UnitOfWork _unitOfWork ;
        GoodsOutputRepository goodsOutputRepository ;
        GoodsOutputService _sut ;
        Action expect;
        private Goods dto;
        public UpdateGoodsOutput(ConfigurationFixture configuration) : base(configuration)
        {
            _context = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_context);
            goodsOutputRepository = new EFGoodsOutPutRepository(_context);
            _sut = new GoodsOutputAppService(_unitOfWork, goodsOutputRepository);
        }
        [Given("خروجی کالا 'شیر' به تعداد '2 عدد' قیمت '2000' به شماره' 20' وجود دارد")]
        private void Given()
        {
            var _category = new Category
            {
                Title = "لبنیات"
            };

            _context.Manipulate(_ => _.Categories.Add(_category));
            Goods dto = new Goods()
            {
                CategoryId = _category.Id,
                Cost = 1000,
                GoodsCode = 55,
                MaxInventory = 100,
                Inventory = 0,
                MinInventory = 10,
                Name = "شیر"


            };
            _context.Manipulate(_ => _.Goodses.Add(dto));
            GoodsOutput goodsOutput = new GoodsOutput
            {
                Number = 14,
                Count = 2,
                Date = new DateTime(2022, 4, 5, 0, 0, 0, 0),
                GoodsCode = _context.Goodses.FirstOrDefault().GoodsCode,
                Price = 1000
            };
            _context.Manipulate(_ => _.GoodsOutputs.Add(goodsOutput));
        }
        [When("زمانی که  '14' به '18' تغییر می کند")]
        private void When()
        {
            UpdateGoodsOutputDTO updateGoodsOutput = new UpdateGoodsOutputDTO
            {
                Number = 14,
                Count = 2,
                Date = "2022, 4, 5, 0, 0, 0, 0",
                GoodsCode = 54,
                Price = 1000
            };
           
            _sut.Update(updateGoodsOutput, _context.GoodsOutputs.FirstOrDefault().Number);
        }
        [Then("خروجی کالا  با شماره '14' باید وجود داشته باشد ")]
        private void Then()
        {
            var expect = _context.GoodsOutputs.FirstOrDefault(_ => _.Number.Equals(14));
            expect.Number.Should().Be(14);

        }
        [Fact]
        private void Run()
        {
            Given();
            When();
            Then();
        }
        [Given("ورود کالا 'شیر' به تعداد '2 عدد' قیمت '2000' به شماره' 20' وجود دارد")]
        private void DuplicateGiven()
        {
            var _category = new Category
            {
                Title = "لبنیات"
            };

            _context.Manipulate(_ => _.Categories.Add(_category));
            dto = new Goods()
            {
                CategoryId = _context.Categories.FirstOrDefault().Id,
                Cost = 1000,
                GoodsCode = 20,
                MaxInventory = 100,
                Inventory = 0,
                MinInventory = 10,
                Name = "شیر",
            };
            _context.Manipulate(_ => _.Goodses.Add(dto));
            GoodsOutput goodsOutput = new GoodsOutput
            {
                Number = 20,
                Count = 2,
                Date = new DateTime(2022, 4, 5, 0, 0, 0, 0),
                GoodsCode = 20,
                Price = 1000
            };
            _context.Manipulate(_ => _.GoodsOutputs.Add(goodsOutput));

        }
        [And("ورود کالا با شماره '14' وجود دارد")]
        private void DuplicateAnd()
        {
            GoodsOutput goodsOutput = new GoodsOutput
            {
                Number = 14,
                Count = 2,
                Date = new DateTime(2022, 4, 5, 0, 0, 0, 0),
                GoodsCode = dto.GoodsCode,
                Price = 1000
            };
            _context.Manipulate(_ => _.GoodsOutputs.Add(goodsOutput));
        }
        [When("زمانی که  '20' به '14' تغییر می کند")]
        private void DuplicateWhen()
        {
            UpdateGoodsOutputDTO updateGoodsOutput = new UpdateGoodsOutputDTO
            {
                Number = 14,
                Count = 2,
                Date = "2022, 4, 5, 0, 0, 0, 0",
                GoodsCode = dto.GoodsCode,
                Price = 1000
            };


            expect=()=> _sut.Update(updateGoodsOutput, updateGoodsOutput.GoodsCode);
        }
        [Then("خطا با عنوان 'شماره فاکتور تکراری است' باید رخ دهد")]
        private void DuplicateThen()
        {
            expect.Should().ThrowExactly<DuplicateFactorNumberException>();
        }

        public void DuplicateRun()
        {
            Runner.RunScenario(
                _ => DuplicateGiven(),
                _ => DuplicateAnd(),
                _ => DuplicateWhen(),
                _ => DuplicateThen()
                );
        }
    }
}

