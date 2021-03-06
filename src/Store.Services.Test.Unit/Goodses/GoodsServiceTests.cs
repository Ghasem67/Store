using FluentAssertions;
using Store.Entities;
using Store.Infrastracture.Application;
using Store.Infrastracture.Tests;
using Store.Persistence.EF;
using Store.Persistence.EF.Goodses;
using Store.Services.Goodses;
using Store.Services.Goodses.Contracts;
using Store.Services.Goodses.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Store.Test.Tools.Categories;
using Xunit;
using Store.Test.Tools.Goodses;

namespace Store.Services.Test.Unit.Goodses
{
    public class GoodsServiceTests
    {
        private readonly EFDataContext _context;
        private readonly UnitOfWork _unitOfWork;
        private readonly GoodsRepository _goodsrepository;
        private readonly GoodsService _Sut;

        public GoodsServiceTests()
        {
            _context = new EFInMemoryDatabase()
                .CreateDataContext<EFDataContext>();
            _unitOfWork = new EFUnitOfWork(_context);
            _goodsrepository = new EFGoodsRepository(_context);
            _Sut = new GoodsAppService(_goodsrepository, _unitOfWork);
        }
        [Fact]
        private void Add_adds_goods_properly()
        {
            Category Category = new Category
            {
                Title = "لبنیات"

            };
            _context.Manipulate(_ => _.Add(Category));
            AddGoodsDTO dto = new AddGoodsDTO
            {
                CategoryId = _context.Categories.FirstOrDefault().Id,
                Cost = 1000,
                GoodsCode = 12,
                MaxInventory = 1000,
                MinInventory = 10,
                Name = "شیر پگاه"
            };
            _Sut.Add(dto);
            _context.Goodses.Should().Contain(_ => _.Name.Equals(dto.Name));
            _context.Goodses.Should().Contain(_ => _.CategoryId.Equals(dto.CategoryId));
            _context.Goodses.Should().Contain(_ => _.Cost.Equals(dto.Cost));
            _context.Goodses.Should().Contain(_ => _.GoodsCode.Equals(dto.GoodsCode));
            _context.Goodses.Should().Contain(_ => _.MaxInventory.Equals(dto.MaxInventory));
            _context.Goodses.Should().Contain(_ => _.MinInventory.Equals(dto.MinInventory));
        }

        [Fact]
        private void Adds_adds_goods_throwException_IsExist()
        {
            Category Category = new Category
            {
                Title = "لبنیات"

            };
            _context.Manipulate(_ => _.Add(Category));
            Goods goods = new Goods
            {
                CategoryId = Category.Id,
                Cost = 1000,
                GoodsCode = 12,
                MaxInventory = 1000,
                MinInventory = 10,
                Name = "شیر پگاه"
            };
            _context.Manipulate(_ => _.Goodses.Add(goods));
            AddGoodsDTO addGoodsDTO = new AddGoodsDTO
            {
                CategoryId = Category.Id,
                Cost = 1000,
                GoodsCode = 12,
                MaxInventory = 1000,
                MinInventory = 10,
                Name = "شیر پگاه"
            };
            Action expect = () => _Sut.Add(addGoodsDTO);
            expect.Should().ThrowExactly<DuplicateGoodsCodeException>();
        }

        [Fact]
        private void Update_throw_goods_NotFoundException_when_goods_with_given_id_is_not_exist()
        {
            var GoodsCode = 2000;
            var GoodsDTO = GenerateUpdateGoodsDto("لبنیات","شیر پگاه فارس");
            Action expect = () => _Sut.Update(GoodsDTO, GoodsCode);
            expect.Should().ThrowExactly<GoodsNotFoundException>();
        }
        [Fact]
        private void Delete_throw_Goods_NotFoundException_When_Goods_with_given_id_is_not_exist()
        {

            var GoodsCode = 2000;
            Action expect = () => _Sut.Delete(GoodsCode);
            expect.Should().ThrowExactly<GoodsNotFoundException>();
        }
        [Fact]
        private void Update_updates_goods_properly()
        {
            Category Category = CategoryFactory.CreateCategory("لبنیات");
            _context.Manipulate(_ => _.Add(Category));
            Goods coods = GoodsFactory.CreateGoods(12, "شیر پگاه", Category.Id);
                
            _context.Manipulate(_ => _.Goodses.Add(coods));
            var Goods = _context.Goodses.First();

            UpdateGoodsDTO dto = GoodsFactory.CreateGoodsDTO("ماست پگاه", Category.Id);
            _Sut.Update(dto, coods.GoodsCode);
            var expect = _context.Goodses.FirstOrDefault(_ => _.GoodsCode == coods.GoodsCode);
            expect.Name.Should().Be(dto.Name);
            expect.Cost.Should().Be(dto.Cost);
            expect.CategoryId.Should().Be(dto.CategoryId);
            expect.MaxInventory.Should().Be(dto.MaxInventory);
            expect.MinInventory.Should().Be(dto.MinInventory);
        }
        [Fact]
        private void Delete_delete_goods_properly()
        {
            Category category = CategoryFactory.CreateCategory("لبنیات");
            _context.Manipulate(_ => _.Add(category));
            Goods good = GoodsFactory.CreateGoods(12, "شیر پگاه", category.Id);
            
            _context.Manipulate(_ => _.Goodses.Add(good));
            var Goods = _context.Goodses.First();
            _Sut.Delete(Goods.GoodsCode);
            var expect = _context.Goodses.FirstOrDefault(_=>_.GoodsCode.Equals(good.GoodsCode));
            expect.Should().BeNull();
        }
        [Fact]
        private void GetAll_getalls_goods_properly()
        {
            Category Category =CategoryFactory.CreateCategory("لبنیات");
            _context.Manipulate(_ => _.Add(Category));
            List<Goods> goodslist = new List<Goods>();
           goodslist.Add(GoodsFactory.CreateGoods(13, "شیر پگاه", _context.Categories.FirstOrDefault().Id));
           goodslist.Add(GoodsFactory.CreateGoods(14, "شیر رامک", _context.Categories.FirstOrDefault().Id));
           goodslist.Add(GoodsFactory.CreateGoods(15, "شیر دامداران", _context.Categories.FirstOrDefault().Id));
           
            _context.Manipulate(_ => _.Goodses.AddRange(goodslist));
            var except = _Sut.GetAll();
            except.Should().HaveCount(3);
            except.Should().Contain(_ => _.GoodsCode.Equals(goodslist[0].GoodsCode));
            except.Should().Contain(_ => _.Name.Equals(goodslist[0].Name));
            except.Should().Contain(_ => _.CategoryName.Equals(goodslist[0].Category.Title));
            except.Should().Contain(_ => _.MaxInventory.Equals(goodslist[0].MaxInventory));
            except.Should().Contain(_ => _.MinInventory.Equals(goodslist[0].MinInventory));
            except.Should().Contain(_ => _.Inventory.Equals(goodslist[0].Inventory));
            except.Should().Contain(_ => _.Cost.Equals(goodslist[0].Cost));

            except.Should().Contain(_ => _.GoodsCode.Equals(goodslist[1].GoodsCode));
            except.Should().Contain(_ => _.Name.Equals(goodslist[1].Name));
            except.Should().Contain(_ => _.CategoryName.Equals(goodslist[1].Category.Title));
            except.Should().Contain(_ => _.MaxInventory.Equals(goodslist[1].MaxInventory));
            except.Should().Contain(_ => _.MinInventory.Equals(goodslist[1].MinInventory));
            except.Should().Contain(_ => _.Inventory.Equals(goodslist[1].Inventory));
            except.Should().Contain(_ => _.Cost.Equals(goodslist[1].Cost));

            except.Should().Contain(_ => _.GoodsCode.Equals(goodslist[2].GoodsCode));
            except.Should().Contain(_ => _.Name.Equals(goodslist[2].Name));
            except.Should().Contain(_ => _.CategoryName.Equals(goodslist[2].Category.Title));
            except.Should().Contain(_ => _.MaxInventory.Equals(goodslist[2].MaxInventory));
            except.Should().Contain(_ => _.MinInventory.Equals(goodslist[2].MinInventory));
            except.Should().Contain(_ => _.Inventory.Equals(goodslist[2].Inventory));
            except.Should().Contain(_ => _.Cost.Equals(goodslist[2].Cost));
        }
        [Fact]
        private void GetById_getByIds_goods_properly()
        {
            Category Category = CategoryFactory.CreateCategory("لبنیات");
            _context.Manipulate(_ => _.Categories.Add(Category));
            Goods goods = GoodsFactory.CreateGoods(13, "شیر پگاه", _context.Categories.FirstOrDefault().Id);
            _context.Manipulate(_ => _.Goodses.Add(goods));
            var except = _Sut.GetbyId(goods.GoodsCode);
            except.Should().NotBeNull();
            except.GoodsCode.Should().Be(goods.GoodsCode);
            except.Name.Should().Be(goods.Name);
            except.CategoryName.Should().Be(goods.Category.Title);
            except.MaxInventory.Should().Be(goods.MaxInventory);
            except.MinInventory.Should().Be(goods.MinInventory);
            except.Inventory.Should().Be(goods.Inventory);
            except.Cost.Should().Be(goods.Cost);
        }
        private UpdateGoodsDTO GenerateUpdateGoodsDto(string title,string name)
        {
            Category Category = CategoryFactory.CreateCategory("لبنیات");
            _context.Manipulate(_ => _.Categories.Add(Category));
            return GoodsFactory.CreateGoodsDTO(name, Category.Id);
          

        }
    }
}
