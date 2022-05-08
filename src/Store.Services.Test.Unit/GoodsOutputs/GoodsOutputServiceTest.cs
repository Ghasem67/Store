using FluentAssertions;
using Store.Entities;
using Store.Infrastracture.Application;
using Store.Infrastracture.Tests;
using Store.Persistence.EF;
using Store.Persistence.EF.GoodsOutputs;
using Store.Services.GoodsOutputs;
using Store.Services.GoodsOutputs.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Store.Services.Test.Unit.GoodsOutputs
{
    public class GoodsOutputServiceTest
    {
        private readonly EFDataContext _context;
        private readonly UnitOfWork _unitOfWork;
        private readonly GoodsOutputRepository goodsOutputRepository;
        private readonly GoodsOutputService _sut;

        public GoodsOutputServiceTest()
        {
            _context = new EFInMemoryDatabase().CreateDataContext<EFDataContext>();
            _unitOfWork = new EFUnitOfWork(_context);
            goodsOutputRepository = new EFGoodsOutPutRepository(_context);
            _sut = new GoodsOutputAppService(_unitOfWork, goodsOutputRepository);
        }
        [Fact]
        private void Adds_adds_goodsoutput_properly()
        {
            var goods = generategoods();
            AddgoodsoutputDTO addGoodsOutputDTO = new AddgoodsoutputDTO
            {
                Count = 1,
                Date = DateTime.Now.ToShortDateString(),
                Number = 12,
                Price = 1000,
                GoodsCode = goods.GoodsCode,
            };
            _sut.Add(addGoodsOutputDTO);
            _context.GoodsOutputs.Should().Contain(_ => _.Count.Equals(addGoodsOutputDTO.Count));
            _context.GoodsOutputs.Should().Contain(_ => _.Date.ToShortDateString().Equals(addGoodsOutputDTO.Date));
            _context.GoodsOutputs.Should().Contain(_ => _.GoodsCode.Equals(addGoodsOutputDTO.GoodsCode));
        }
        [Fact]
        private void Update_updates_goods_Properly()
        {
            var goodsOutput = GenerateGoodsOutput();
            UpdateGoodsOutputDTO updategoodsoutputDTO = new UpdateGoodsOutputDTO()
            {
                Count = 5,
                Date = new DateTime(2022, 2, 3).ToShortDateString(),
                Number = 12,
                Price = 2000,
                GoodsCode = goodsOutput.GoodsCode,
            };
            _sut.Update(updategoodsoutputDTO, goodsOutput.GoodsCode);
            var expect = _context.GoodsOutputs.FirstOrDefault(_ => _.GoodsCode == updategoodsoutputDTO.GoodsCode);
            expect.Number.Should().Be(updategoodsoutputDTO.Number);
            expect.Date.ToShortDateString().Should().Be(updategoodsoutputDTO.Date);
            expect.Count.Should().Be(updategoodsoutputDTO.Count);
            expect.GoodsCode.Should().Be(updategoodsoutputDTO.GoodsCode);
            expect.Price.Should().Be(updategoodsoutputDTO.Price);

        }
        [Fact]
        private void Delete_deletes_goodsOutput_properly()
        {
            var goodsOutput = GenerateGoodsOutput();
            _sut.Delete(goodsOutput.GoodsCode);
            var expect = _context.GoodsOutputs.FirstOrDefault(_ => _.GoodsCode.Equals(goodsOutput.GoodsCode));
            expect.Should().BeNull();
        }
        [Fact]
        private void GEtAll_getalls_goodsoutput_properly()
        {
          var goodsOutputList=  genaratelistgoodsOutput();
           var expect= _sut.GetAll();
            expect.Should().Contain(_ => _.Date == goodsOutputList[0].Date.ToShortDateString());
            expect.Should().Contain(_ => _.Count == goodsOutputList[0].Count);
            expect.Should().Contain(_ => _.GoodsCode == goodsOutputList[0].GoodsCode);
            expect.Should().Contain(_ => _.Number == goodsOutputList[0].Number);
            expect.Should().Contain(_ => _.Price == goodsOutputList[0].Price);

            expect.Should().Contain(_ => _.Date == goodsOutputList[1].Date.ToShortDateString());
            expect.Should().Contain(_ => _.Count == goodsOutputList[1].Count);
            expect.Should().Contain(_ => _.GoodsCode == goodsOutputList[1].GoodsCode);
            expect.Should().Contain(_ => _.Number == goodsOutputList[1].Number);
            expect.Should().Contain(_ => _.Price == goodsOutputList[1].Price);

            expect.Should().Contain(_ => _.Date == goodsOutputList[2].Date.ToShortDateString());
            expect.Should().Contain(_ => _.Count == goodsOutputList[2].Count);
            expect.Should().Contain(_ => _.GoodsCode == goodsOutputList[2].GoodsCode);
            expect.Should().Contain(_ => _.Number == goodsOutputList[2].Number);
            expect.Should().Contain(_ => _.Price == goodsOutputList[2].Price);
        }
        private List<GoodsOutput> genaratelistgoodsOutput()
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
                GoodsCode = 27,
                Inventory = 12,
                MaxInventory = 100,
                MinInventory = 10,
                Name = "شیر",
            };
            _context.Manipulate(_ => _.Goodses.Add(goods));
            List<GoodsOutput> goodsOutputs = new List<GoodsOutput>()
            {
                 new GoodsOutput{Count=2,Date=new DateTime(2022-2-4),GoodsCode=goods.GoodsCode,Number=1,Price=1000 },
                  new GoodsOutput{Count=2,Date=new DateTime(2022-2-3),GoodsCode=goods.GoodsCode,Number=2,Price=2000 },
                 new GoodsOutput{Count=2,Date=new DateTime(2022-2-3),GoodsCode=goods.GoodsCode,Number=3,Price=3000 }
            };
            _context.Manipulate(_ => _.GoodsOutputs.AddRange(goodsOutputs));
            return goodsOutputs;
        }
        private Goods generategoods()
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
                GoodsCode = 38,
                Inventory = 12,
                MaxInventory = 100,
                MinInventory = 10,
                Name = "شیر",
            };
            _context.Manipulate(_ => _.Goodses.Add(goods));
            return goods;
        }
        private GoodsOutput GenerateGoodsOutput()
        {
            var goods = generategoods();
            GoodsOutput goodsOutput = new GoodsOutput()
            {
                Count = 1,
                Date = new DateTime(2022, 2, 2),
                Number = 12,
                Price = 1000,
                GoodsCode = goods.GoodsCode,
            };
            _context.Manipulate(_ => _.GoodsOutputs.Add(goodsOutput));
            return goodsOutput;
        }

    }
}
