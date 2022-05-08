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
    [Scenario("نمایش ورودی کالا")]
    [Feature("",
        AsA ="فروشنده",
        IWantTo = "مدیریت   کالا داشته باشم",
        InOrderTo = "تا بتوانیم ورودی  کالا را نمایش دهیم")]
    public class GetGoodsInput : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _context;
        private List<GoodsInput> GoodsInputList;
        private HashSet<ShowGoodsInputDTO> expect;
        public GetGoodsInput(ConfigurationFixture configuration) : base(configuration)
        {
            _context = CreateDataContext();
        }
        [Given("ورود کالا با شماره '12' وجود دارد")]
        private void Given()
        {
            Category category = new Category()
            {
                Title = "لبنیات"
            };
            _context.Manipulate(_ => _.Categories.Add(category));
            Goods goods = new Goods()
            {
                CategoryId = category.Id,
                Cost = 1000,
                GoodsCode = 17,
                Inventory = 123,
                MaxInventory = 1000,
                MinInventory = 10,
                Name = "نسکافه"
                
            };
            _context.Manipulate(_ => _.Goodses.Add(goods));
            GoodsInputList = new List<GoodsInput>
            {   new GoodsInput(){
                Count = 1,
                Date = new DateTime(2022, 2, 3, 0, 0, 0, 0),
                GoodsCode = 17,
                Number = 12,
                Price = 1000,
            } ,
             new GoodsInput(){
                Count = 2,
                Date = new DateTime(2022, 3, 3, 0, 0, 0, 0),
                GoodsCode = 17,
                Number = 15,
                Price = 1000,
            }
            };
            _context.Manipulate(_ => _.GoodsInputs.AddRange(GoodsInputList));
        }
        [When("درخواست نمایش ورود کالا ارسال می شود")]
        public void When()
        {
            UnitOfWork _unitOfWork = new EFUnitOfWork(_context);
            GoodsInputRepository goodsInputRepository = new EFGoodsInputRepository(_context);
            var _sut = new GoodsInputAppService(_unitOfWork, goodsInputRepository);
            expect= _sut.GetAll();
        }
        [Then("فهرست ورود کالا نمایش داده می شود.")]
        private void Then()
        {
            expect.Should().Contain(_ => _.Count == GoodsInputList[0].Count);
            expect.Should().Contain(_ => _.GoodsCode == GoodsInputList[0].GoodsCode);
            expect.Should().Contain(_ => _.Price == GoodsInputList[0].Price);
            expect.Should().Contain(_ => _.Date == GoodsInputList[0].Date.ToShortDateString());
            expect.Should().Contain(_ => _.Number == GoodsInputList[0].Number);

            expect.Should().Contain(_ => _.Count == GoodsInputList[1].Count);
            expect.Should().Contain(_ => _.GoodsCode == GoodsInputList[1].GoodsCode);
            expect.Should().Contain(_ => _.Price == GoodsInputList[1].Price);
            expect.Should().Contain(_ => _.Date == GoodsInputList[1].Date.ToShortDateString());
            expect.Should().Contain(_ => _.Number == GoodsInputList[1].Number);
        }
        [Fact]
        private void Run()
        {
            Given();
            When () ;
            Then();
        }
    }
}
