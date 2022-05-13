using FluentAssertions;
using Store.Entities;
using Store.Infrastracture.Application;
using Store.Infrastracture.Tests;
using Store.Persistence.EF;
using Store.Persistence.EF.GoodsInputs;
using Store.Services.GoodsInputs;
using Store.Services.GoodsInputs.Contracts;
using Store.Services.GoodsInputs.Exceptions;
using Store.Test.Tools.Categories;
using Store.Test.Tools.Goodses;
using Store.Test.Tools.GoodsInputs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Store.Services.Test.Unit.GoodsInputs
{
    public class GoodsInputServiceTests
    {
        private readonly EFDataContext _eFDataContext;
        private readonly UnitOfWork _unitOfWork;
        private readonly GoodsInputRepository _goodsInputRepository;
        private readonly GoodsInputService _Sut;
        public GoodsInputServiceTests()
        {
            _eFDataContext = new EFInMemoryDatabase()
                .CreateDataContext<EFDataContext>();
            _unitOfWork = new EFUnitOfWork(_eFDataContext);
            _goodsInputRepository = new EFGoodsInputRepository(_eFDataContext);
            _Sut = new GoodsInputAppService(_unitOfWork, _goodsInputRepository);
        }
        [Fact]
        private void Adds_adds_goodsInput_properly()
        {
            var goods = generategoods();
            AddGoodsInputDTO addGoodsInputDTO = GoodsInputFactory.CreateAddGoodsInputDTO(goods.GoodsCode, 12);
           
            _Sut.Add(addGoodsInputDTO);
            _eFDataContext.GoodsInputs.Should().Contain(_ => _.Count.Equals(addGoodsInputDTO.Count));
            _eFDataContext.GoodsInputs.Should().Contain(_ => _.Date.ToShortDateString().Equals(addGoodsInputDTO.Date));
            _eFDataContext.GoodsInputs.Should().Contain(_ => _.GoodsCode.Equals(addGoodsInputDTO.GoodsCode));
        }

        [Fact]
        private void Adds_adds_goodsinput_throwException_IsExist()
        {
            var goods = generategoods();
            GoodsInput goodsInput = new GoodsInput
            {
                Count = 1,
                Date = DateTime.Now,
                Number = 12,
                Price = 1000,
                GoodsCode = goods.GoodsCode,
            };
            _eFDataContext.Manipulate(_ => _.GoodsInputs.Add(goodsInput));
            AddGoodsInputDTO addGoodsInputDTO = new AddGoodsInputDTO
            {
                Count = 1,
                Date = DateTime.Now.ToShortDateString(),
                Number = 12,
                Price = 1000,
                GoodsCode = goods.GoodsCode,
            };
            Action expect = () => _Sut.Add(addGoodsInputDTO);
            expect.Should().ThrowExactly<DuplicateFactorNumberException>();
        }

        [Fact]
        private void GetById_Getbyids_goodsInput_properly()
        {
            var goods = GenerateGoodsInput();
           
          var expect=  _Sut.GetById(goods.Number);
            expect.Count.Should().Be(goods.Count);
            expect.Date.Should().Be(goods.Date.ToShortDateString());
            expect.GoodsCode.Should().Be(goods.GoodsCode);
            expect.GoodsName.Should().Be(goods.Goods.Name);
        }
        [Fact]
        private void Update_updates_goodsinput_Properly()
        {
            var goodsInput = GenerateGoodsInput();
            UpdateGoodsInputDTO updateGoodsInputDTO = new UpdateGoodsInputDTO()
            {
                Count = 5,
                Date = new DateTime(2022, 2, 3).ToShortDateString(),
                Price = 2000,
                GoodsCode = goodsInput.GoodsCode,
            };
            _Sut.Update(updateGoodsInputDTO, goodsInput.Number);
            var expect = _eFDataContext.GoodsInputs.FirstOrDefault(_ => _.GoodsCode == updateGoodsInputDTO.GoodsCode);
            expect.Date.ToShortDateString().Should().Be(updateGoodsInputDTO.Date);
            expect.Count.Should().Be(updateGoodsInputDTO.Count);
            expect.GoodsCode.Should().Be(updateGoodsInputDTO.GoodsCode);
            expect.Price.Should().Be(updateGoodsInputDTO.Price);

        }
        [Fact]
        private void Update_throw_goodsInput_NotFoundException_when_goodsInput_with_given_id_is_not_exist()
        {
            var number = 2000;
            var goodsInputDTO = GenerateUpdateGoodsInputDto();
            Action expect = () => _Sut.Update(goodsInputDTO, number);
            expect.Should().ThrowExactly<GoodsInputNotFoundException>();
        }
        [Fact]
        private void Delete_throw_goodsInput_NotFoundException_When_goodsInput_with_given_id_is_not_exist()
        {

            var number = 2000;
            Action expect = () => _Sut.Delete(number);
            expect.Should().ThrowExactly<GoodsInputNotFoundException>();
        }
        [Fact]
        private void Delete_deletes_goodsInput_properly()
        {
            var goodsInput = GenerateGoodsInput();
            _Sut.Delete(goodsInput.Number);
            var expect = _eFDataContext.GoodsInputs.FirstOrDefault(_ => _.GoodsCode.Equals(goodsInput.GoodsCode));
            expect.Should().BeNull();
        }
        [Fact]
        private void GEtAll_getalls_goodsinput_properly()
        {
          var goodsInputList=  genaratelistgoodsInput();
           var expect= _Sut.GetAll();
            expect.Should().Contain(_ => _.Date == goodsInputList[0].Date.ToShortDateString());
            expect.Should().Contain(_ => _.Count == goodsInputList[0].Count);
            expect.Should().Contain(_ => _.GoodsCode == goodsInputList[0].GoodsCode);
            expect.Should().Contain(_ => _.Number == goodsInputList[0].Number);
            expect.Should().Contain(_ => _.Price == goodsInputList[0].Price);

            expect.Should().Contain(_ => _.Date == goodsInputList[1].Date.ToShortDateString());
            expect.Should().Contain(_ => _.Count == goodsInputList[1].Count);
            expect.Should().Contain(_ => _.GoodsCode == goodsInputList[1].GoodsCode);
            expect.Should().Contain(_ => _.Number == goodsInputList[1].Number);
            expect.Should().Contain(_ => _.Price == goodsInputList[1].Price);

            expect.Should().Contain(_ => _.Date == goodsInputList[2].Date.ToShortDateString());
            expect.Should().Contain(_ => _.Count == goodsInputList[2].Count);
            expect.Should().Contain(_ => _.GoodsCode == goodsInputList[2].GoodsCode);
            expect.Should().Contain(_ => _.Number == goodsInputList[2].Number);
            expect.Should().Contain(_ => _.Price == goodsInputList[2].Price);
        }
        private List<GoodsInput> genaratelistgoodsInput()
        {
            Category category = new Category()
            {
                Title = "لبنیات"
            };
            _eFDataContext.Manipulate(_ => _.Categories.Add(category));
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
            _eFDataContext.Manipulate(_ => _.Goodses.Add(goods));
            List<GoodsInput> goodsInputs = new List<GoodsInput>()
            {
                 new GoodsInput{Count=2,Date=new DateTime(2022-2-4),GoodsCode=goods.GoodsCode,Number=1,Price=1000 },
                  new GoodsInput{Count=2,Date=new DateTime(2022-2-3),GoodsCode=goods.GoodsCode,Number=2,Price=2000 },
                 new GoodsInput{Count=2,Date=new DateTime(2022-2-3),GoodsCode=goods.GoodsCode,Number=3,Price=3000 }
            };
            _eFDataContext.Manipulate(_ => _.GoodsInputs.AddRange(goodsInputs));
            return goodsInputs;
        }

        private Goods generategoods()
        {
            Category category = CategoryFactory.CreateCategory("لبنیات");
            _eFDataContext.Manipulate(_ => _.Categories.Add(category));
            Goods goods = GoodsFactory.CreateGoods(17, "خامه", category.Id);
            _eFDataContext.Manipulate(_ => _.Goodses.Add(goods));
            return goods;
        }
        private GoodsInput GenerateGoodsInput()
        {
            var goods = generategoods();
            GoodsInput goodsInput = GoodsInputFactory.CreateGoodsInput(goods.GoodsCode, 57);
                new GoodsInput()
            {
                Count = 1,
                Date = new DateTime(2022, 2, 2),
                Number = 12,
                Price = 1000,
                GoodsCode = goods.GoodsCode,
            };
            _eFDataContext.Manipulate(_ => _.GoodsInputs.Add(goodsInput));
            return goodsInput;
        }
        private UpdateGoodsInputDTO GenerateUpdateGoodsInputDto()
        {
         var goods=generategoods();

            return GoodsInputFactory.CreateUpdateGoodsInputDTO(goods.GoodsCode);
              

        }
    }
}
