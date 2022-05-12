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
    [Scenario("ویرایش ورودی کالا")]
    [Feature("",
          AsA = "فروشنده",
          IWantTo = "مدیریت   کالا داشته باشم",
          InOrderTo = "تا بتوانیم ورودی  کالا ویرایش کنیم")]
    public class UpdateGoodsInputdto : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        UnitOfWork _unitOfWork;
        GoodsInputRepository goodsInputRepository;
        GoodsInputService _sut;
        GoodsInput goodsInput;
        public UpdateGoodsInputdto(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            goodsInputRepository = new EFGoodsInputRepository(_dataContext);
            _sut = new GoodsInputAppService(_unitOfWork, goodsInputRepository);
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
             goodsInput = new GoodsInput
            {
                Number = 100,
                Count = 1,
                Date = new DateTime(2022, 4, 5, 0, 0, 0, 0),
                GoodsCode = 20,
                Price = 1000
            };
            _dataContext.Manipulate(_ => _.GoodsInputs.Add(goodsInput));
        }
        [When("زمانی که قیمت را از  '1000' به '2000' و تعداد را به 2 تقییر دهیم تغییر می کند")]
        private void When()
        {
            UpdateGoodsInputDTO updateGoodsInput = new UpdateGoodsInputDTO
            {
                Count = 2,
                Date =new DateTime(2022, 4, 5, 0, 0, 0, 0).ToShortDateString(),
                GoodsCode = 20,
                Price = 2000
            };


             _sut.Update(updateGoodsInput, goodsInput.Number);
        }
        [Then("ورود کالا  با تعداد 2 و قیمت 2000 باید وجود داشته باشد ")]
        private void Then()
        {
            var expect = _dataContext.GoodsInputs.FirstOrDefault(_=>_.Number.Equals(100));
            expect.Count.Should().Be(2);
            expect.Price.Should().Be(2000);

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
