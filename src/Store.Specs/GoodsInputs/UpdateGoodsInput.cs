using FluentAssertions;
using Store.Entities;
using Store.Infrastracture.Application;
using Store.Infrastracture.Tests;
using Store.Persistence.EF;
using Store.Persistence.EF.GoodsInputs;
using Store.Services.GoodsInputs;
using Store.Services.GoodsInputs.Contracts;
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
    [Scenario("ویرایش ورودی کالا")]
    [Feature("",
          AsA = "فروشنده",
          IWantTo = "مدیریت   کالا داشته باشم",
          InOrderTo = "تا بتوانیم ورودی  کالا ویرایش کنیم")]
    public class UpdateGoodsInputdto : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        public UpdateGoodsInputdto(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
        }
        [Given("ورود کالا 'شیر' به تعداد '2 عدد' قیمت '2000' به شماره' 20' وجود دارد")]
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
                GoodsCode = 20,
                MaxInventory = 100,
                Inventory = 0,
                MinInventory = 10,
                Name = "شیر",
            };
            _dataContext.Manipulate(_ => _.Goodses.Add(dto));
            GoodsInput goodsInput = new GoodsInput
            {
                Number = 14,
                Count = 2,
                Date = new DateTime(2022, 4, 5, 0, 0, 0, 0),
                GoodsCode = 20,
                Price = 1000
            };
            _dataContext.Manipulate(_ => _.GoodsInputs.Add(goodsInput));
        }
        [When("زمانی که  '14' به '16' تغییر می کند")]
        private void When()
        {
            UpdateGoodsInputDTO updateGoodsInput = new UpdateGoodsInputDTO
            {
                Number = 16,
                Count = 2,
                Date = "2022, 4, 5, 0, 0, 0, 0",
                GoodsCode = 1,
                Price = 1000
            };
            UnitOfWork _unitOfWork = new EFUnitOfWork(_dataContext);
            GoodsInputRepository goodsInputRepository = new EFGoodsInputRepository(_dataContext);
            var _sut = new GoodsInputAppService(_unitOfWork, goodsInputRepository);
            _sut.Update(updateGoodsInput, updateGoodsInput.GoodsCode);
        }
        [Then("ورود کالا  با شماره '14' باید وجود داشته باشد ")]
        private void Then()
        {
         var expect=   _dataContext.GoodsInputs.OrderByDescending(x=>x.Date).FirstOrDefault(_ => _.Number.Equals(14));
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
