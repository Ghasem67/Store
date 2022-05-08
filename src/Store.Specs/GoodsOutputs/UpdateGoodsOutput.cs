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
    [Scenario("ویرایش خروجی کالا")]
    [Feature("",
          AsA = "فروشنده",
          IWantTo = "مدیریت   کالا داشته باشم",
          InOrderTo = "تا بتوانیم خروجی  کالا را ویرایش کنیم")]
    public class UpdateGoodsOutput : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _context;
        public UpdateGoodsOutput(ConfigurationFixture configuration) : base(configuration)
        {
            _context = CreateDataContext();
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
                CategoryId = _context.Categories.OrderByDescending(_ => _.Id).FirstOrDefault().Id,
                Cost = 1000,
                GoodsCode = 54,
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
                GoodsCode = 54,
                Price = 1000
            };
            _context.Manipulate(_ => _.GoodsOutputs.Add(goodsOutput));
        }
        [When("زمانی که  '14' به '18' تغییر می کند")]
        private void When()
        {
            UpdateGoodsOutputDTO updateGoodsOutput = new UpdateGoodsOutputDTO
            {
                Number = 18,
                Count = 2,
                Date = "2022, 4, 5, 0, 0, 0, 0",
                GoodsCode = 54,
                Price = 1000
            };
            UnitOfWork _unitOfWork = new EFUnitOfWork(_context);
            GoodsOutputRepository goodsOutputRepository = new EFGoodsOutPutRepository(_context);
            var _sut = new GoodsOutputAppService(_unitOfWork, goodsOutputRepository);
            _sut.Update(updateGoodsOutput, updateGoodsOutput.GoodsCode);
        }
        [Then("خروجی کالا  با شماره '14' باید وجود داشته باشد ")]
        private void Then()
        {
            var expect = _context.GoodsOutputs.OrderByDescending(x => x.Date).FirstOrDefault(_ => _.Number.Equals(14));
            expect.Number.Should().Be(14);

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

