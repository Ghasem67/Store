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
        [When("زمانی که تاریخ  '5/ 4/ 2022' به '5/ 5/ 2022' تغییر می کند")]
        private void When()
        {
            UpdateGoodsOutputDTO updateGoodsOutput = new UpdateGoodsOutputDTO
            {
                Count = 2,
                 Date= new DateTime( 2022, 5, 5,0,0,0,0).ToShortDateString(),
                GoodsCode = _context.Goodses.FirstOrDefault().GoodsCode,
                Price = 1000
            };
           
            _sut.Update(updateGoodsOutput, _context.GoodsOutputs.FirstOrDefault().Number);
        }
        [Then("خروجی کالا  با شماره '18' باید وجود داشته باشد ")]
        private void Then()
        {
            var expect = _context.GoodsOutputs.FirstOrDefault(_ => _.Number.Equals(14));
            expect.Date.Should().Be(new DateTime(2022, 5, 5, 0, 0, 0, 0));

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

