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
    [Scenario("نمایش خروجی کالا")]
    [Feature("",
          AsA = "فروشنده",
          IWantTo = "مدیریت   کالا داشته باشم",
          InOrderTo = "تا بتوانیم خروجی  کالا را نمایش دهیم")]
    public class GetGoodsOutput : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _context;
        UnitOfWork _unitOfWork;
        GoodsOutputRepository goodsOutputRepository ;
       GoodsOutputService _sut;
        private HashSet<ShowGoodsOutputDTO> GoodsOutputHashSet;
        List<GoodsOutput> GoodsOutputList;
       
        public GetGoodsOutput(ConfigurationFixture configuration) : base(configuration)
        {
            _context = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_context);
            goodsOutputRepository = new EFGoodsOutPutRepository(_context);
            _sut = new GoodsOutputAppService(_unitOfWork, goodsOutputRepository);
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
                GoodsCode = 101,
                Inventory = 123,
                MaxInventory = 1000,
                MinInventory = 10,
                Name = "خامه"
            };
            _context.Manipulate(_ => _.Goodses.Add(goods));
           
            GoodsOutputList = new List<GoodsOutput>
            {
                new GoodsOutput()
                {
                Count = 1,
                Date = DateTime.Now,
                GoodsCode = 101,
                Number = 12,
                Price = 1000,
                }
             ,
             new GoodsOutput()
             {
                Count = 2,
                Date = DateTime.Now,
                GoodsCode = 101,
                Number = 15,
                Price = 1000,

            }
            };
            _context.Manipulate(_ => _.GoodsOutputs.AddRange(GoodsOutputList));
        }
        [When("درخواست نمایش ورود کالا ارسال می شود")]
        public void When()
        {
           
            GoodsOutputHashSet = _sut.GetAll();
        }
        [Then("فهرست ورود کالا نمایش داده می شود.")]
        private void Then()
        {
            GoodsOutputHashSet.Should().Contain(_ => _.Count == GoodsOutputList[0].Count);
            GoodsOutputHashSet.Should().Contain(_ => _.GoodsCode == GoodsOutputList[0].GoodsCode);
            GoodsOutputHashSet.Should().Contain(_ => _.Price == GoodsOutputList[0].Price);
            GoodsOutputHashSet.Should().Contain(_ => _.Date == GoodsOutputList[0].Date.ToShortDateString());
            GoodsOutputHashSet.Should().Contain(_ => _.Number == GoodsOutputList[0].Number);

            GoodsOutputHashSet.Should().Contain(_ => _.Count == GoodsOutputList[1].Count);
            GoodsOutputHashSet.Should().Contain(_ => _.GoodsCode == GoodsOutputList[1].GoodsCode);
            GoodsOutputHashSet.Should().Contain(_ => _.Price == GoodsOutputList[1].Price);
            GoodsOutputHashSet.Should().Contain(_ => _.Date == GoodsOutputList[1].Date.ToShortDateString());
            GoodsOutputHashSet.Should().Contain(_ => _.Number == GoodsOutputList[1].Number);
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
