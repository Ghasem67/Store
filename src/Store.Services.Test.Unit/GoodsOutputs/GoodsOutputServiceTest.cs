using FluentAssertions;
using Store.Entities;
using Store.Infrastracture.Application;
using Store.Infrastracture.Tests;
using Store.Persistence.EF;
using Store.Persistence.EF.GoodsOutputs;
using Store.Services.GoodsOutputs;
using Store.Services.GoodsOutputs.Contracts;
using Store.Services.GoodsOutputs.Exceptions;
using Store.Test.Tools.Categories;
using Store.Test.Tools.Goodses;
using Store.Test.Tools.GoodsOutputs;
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
            var goods = Generategoods();
            AddgoodsOutputDTO addGoodsOutputDTO = GoodsOutputFactory.CreateAddGoodsOutputDTO(goods.GoodsCode, 213);
                new AddgoodsOutputDTO
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
        private void Adds_adds_goodsoutput_throwException_IsExist()
        {
            var goods = Generategoods();
            GoodsOutput goodsOutput = new GoodsOutput
            {
                Count = 1,
                Date = DateTime.Now,
                Number = 12,
                Price = 1000,
                GoodsCode = goods.GoodsCode,
            };
            _context.Manipulate(_ => _.GoodsOutputs.Add(goodsOutput));
            AddgoodsOutputDTO addGoodsOutputDTO = new AddgoodsOutputDTO
            {
                Count = 1,
                Date = DateTime.Now.ToShortDateString(),
                Number = 12,
                Price = 1000,
                GoodsCode = goods.GoodsCode,
            };
           Action expect=()=> _sut.Add(addGoodsOutputDTO);
            expect.Should().ThrowExactly<DuplicateFactorNumberException>();
           
        }
        [Fact]
        private void Update_updates_goodsoutPut_Properly()
        {
            var goodsOutput = GenerateGoodsOutput();
            UpdateGoodsOutputDTO updategoodsoutputDTO =
                GoodsOutputFactory.CreateUpdateGoodsOutputDTO(goodsOutput.GoodsCode);
             
            _sut.Update(updategoodsoutputDTO, goodsOutput.Number);
            var expect = _context.GoodsOutputs.
                FirstOrDefault(_ => _.Number == goodsOutput.Number);
            expect.Number.Should().Be(goodsOutput.Number);
            expect.Date.ToShortDateString().Should().Be(updategoodsoutputDTO.Date);
            expect.Count.Should().Be(updategoodsoutputDTO.Count);
            expect.GoodsCode.Should().Be(updategoodsoutputDTO.GoodsCode);
            expect.Price.Should().Be(updategoodsoutputDTO.Price);

        }
        [Fact]
        private void Update_throw_goodsInput_NotFoundException_when_goodsInput_with_given_id_is_not_exist()
        {
            var number = 2000;
            var goodsInputDTO = GenerateUpdateGoodsOutputDto();
            Action expect = () => _sut.Update(goodsInputDTO, number);
            expect.Should().ThrowExactly<GoodsOutputNotFoundException>();
        }
        [Fact]
        private void Delete_throw_goodsInput_NotFoundException_When_goodsInput_with_given_id_is_not_exist()
        {

            var number = 2000;
            Action expect = () => _sut.Delete(number);
            expect.Should().ThrowExactly<GoodsOutputNotFoundException>();
        }
        [Fact]
        private void Delete_deletes_goodsOutput_properly()
        {
            var goodsOutput = GenerateGoodsOutput();
            _sut.Delete(goodsOutput.Number);
            var expect = _context.GoodsOutputs.FirstOrDefault(_ => _.GoodsCode.Equals(goodsOutput.GoodsCode));
            expect.Should().BeNull();
        }
        [Fact]
        private void GEtAll_getalls_goodsoutput_properly()
        {
          var goodsOutputList=  GenaratelistgoodsOutput();
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
        [Fact]
        private void GEtById_getbyid_goodsoutput_properly()
        {
            var goodsOutput = GenerateGoodsOutput();
            var expect = _sut.GetById(goodsOutput.Number);
            expect.Count.Should().Be(goodsOutput.Count);
            expect.GoodsCode.Should().Be(goodsOutput.GoodsCode);
            expect.Date.Should().Be(goodsOutput.Date.ToShortDateString());
           
        }
        private List<GoodsOutput> GenaratelistgoodsOutput()
        {
          var goods=  Generategoods();
            List<GoodsOutput> goodsOutputs = new List<GoodsOutput>();
            goodsOutputs.Add(GoodsOutputFactory.CreateGoodsOutput(goods.GoodsCode,123));
            goodsOutputs.Add(GoodsOutputFactory.CreateGoodsOutput(goods.GoodsCode,124));
            goodsOutputs.Add(GoodsOutputFactory.CreateGoodsOutput(goods.GoodsCode,125));
            _context.Manipulate(_ => _.GoodsOutputs.AddRange(goodsOutputs));
            return goodsOutputs;
        }
        private Goods Generategoods()
        {
            Category category = CategoryFactory.CreateCategory("لبنیات");
            _context.Manipulate(_ => _.Categories.Add(category));
            Goods goods = GoodsFactory.CreateGoods(97, "شیر", category.Id);
            _context.Manipulate(_ => _.Goodses.Add(goods));
            return goods;
        }
        private GoodsOutput GenerateGoodsOutput()
        {
            var goods = Generategoods();
            GoodsOutput goodsOutput = GoodsOutputFactory.CreateGoodsOutput(goods.GoodsCode, 43);
            _context.Manipulate(_ => _.GoodsOutputs.Add(goodsOutput));
            return goodsOutput;
        }
        private UpdateGoodsOutputDTO GenerateUpdateGoodsOutputDto()
        {
            var goods = Generategoods();
           return  GoodsOutputFactory.CreateUpdateGoodsOutputDTO(goods.GoodsCode);

        }
    }
}
