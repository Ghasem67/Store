using FluentAssertions;
using Store.Entities;
using Store.Infrastracture.Application;
using Store.Infrastracture.Tests;
using Store.Persistence.EF;
using Store.Persistence.EF.Goodses;
using Store.Services.Goodses;
using Store.Services.Goodses.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

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
        private void Update_updates_goods_properly()
        {
            Category Category = new Category
            {
                Title = "لبنیات"

            };
            _context.Manipulate(_ => _.Add(Category));
            Goods dto1 = new Goods
            {
                CategoryId = _context.Categories.FirstOrDefault().Id,
                Cost = 1000,
                GoodsCode = 12,
                Inventory = 10,
                MaxInventory = 1000,
                MinInventory = 10,
                Name = "شیر پگاه"
            };
            _context.Manipulate(_ => _.Goodses.Add(dto1));
            var Goods = _context.Goodses.First();

            UpdateGoodsDTO dto = new UpdateGoodsDTO
            {
                CategoryId = dto1.CategoryId,
                MaxInventory = 2000,
                MinInventory = 20,
                Inventory = 0,
                Name = "ماست پگاه",
                Cost = 3000,
                GoodsCode = 20
            };
            _Sut.Update(dto, dto1.GoodsCode);
            var expect = _context.Goodses.FirstOrDefault(_ => _.GoodsCode == dto1.GoodsCode);
            expect.Name.Should().Be(dto.Name);
            expect.Cost.Should().Be(dto.Cost);
            expect.CategoryId.Should().Be(dto.CategoryId);
            expect.MaxInventory.Should().Be(dto.MaxInventory);
            expect.MinInventory.Should().Be(dto.MinInventory);

        }
        [Fact]
        private void Delete_delete_goods_properly()
        {
            Category Category = new Category
            {
                Title = "لبنیات"

            };
            _context.Manipulate(_ => _.Add(Category));
            Goods dto1 = new Goods
            {
                CategoryId = _context.Categories.FirstOrDefault().Id,
                Cost = 1000,
                GoodsCode = 12,
                Inventory = 10,
                MaxInventory = 1000,
                MinInventory = 10,
                Name = "شیر پگاه"
            };
            _context.Manipulate(_ => _.Goodses.Add(dto1));
            var Goods = _context.Goodses.First();
            _Sut.Delete(Goods.GoodsCode);
            var expect = _context.Goodses.Should().HaveCount(0);
        }
        [Fact]
        private void Get_gets_goods_properly()
        {
            Category Category = new Category
            {
                Title = "لبنیات"

            };
            _context.Manipulate(_ => _.Add(Category));
            List<Goods> goodslist = new List<Goods>
            {
              new Goods
            {
                CategoryId = _context.Categories.FirstOrDefault().Id,
                Cost = 1000,
                GoodsCode = 13,
                Inventory = 10,
                MaxInventory = 1000,
                MinInventory = 10,
                Name = "شیر پگاه"
            },
                new Goods
            {
                CategoryId = _context.Categories.FirstOrDefault().Id,
                Cost = 1000,
                GoodsCode = 14,
                Inventory = 10,
                MaxInventory = 1000,
                MinInventory = 10,
                Name = "شیر رامک"
            },
                 new Goods
            {
                CategoryId = _context.Categories.FirstOrDefault().Id,
                Cost = 1000,
                GoodsCode = 15,
                Inventory = 10,
                MaxInventory = 1000,
                MinInventory = 10,
                Name = "شیر دامداران"
            }
        };
            _context.Manipulate(_ => _.Goodses.AddRange(goodslist));
            _context.Goodses.Should().HaveCount(3);
            _context.Goodses.Should().Contain(_ => _.GoodsCode.Equals(goodslist[0].GoodsCode));
            _context.Goodses.Should().Contain(_ => _.Name.Equals(goodslist[0].Name));
            _context.Goodses.Should().Contain(_ => _.CategoryId.Equals(goodslist[0].CategoryId));
            _context.Goodses.Should().Contain(_ => _.MaxInventory.Equals(goodslist[0].MaxInventory));
            _context.Goodses.Should().Contain(_ => _.MinInventory.Equals(goodslist[0].MinInventory));
            _context.Goodses.Should().Contain(_ => _.Inventory.Equals(goodslist[0].Inventory));
            _context.Goodses.Should().Contain(_ => _.Cost.Equals(goodslist[0].Cost));

            _context.Goodses.Should().Contain(_ => _.GoodsCode.Equals(goodslist[1].GoodsCode));
            _context.Goodses.Should().Contain(_ => _.Name.Equals(goodslist[1].Name));
            _context.Goodses.Should().Contain(_ => _.CategoryId.Equals(goodslist[1].CategoryId));
            _context.Goodses.Should().Contain(_ => _.MaxInventory.Equals(goodslist[1].MaxInventory));
            _context.Goodses.Should().Contain(_ => _.MinInventory.Equals(goodslist[1].MinInventory));
            _context.Goodses.Should().Contain(_ => _.Inventory.Equals(goodslist[1].Inventory));
            _context.Goodses.Should().Contain(_ => _.Cost.Equals(goodslist[1].Cost));

            _context.Goodses.Should().Contain(_ => _.GoodsCode.Equals(goodslist[2].GoodsCode));
            _context.Goodses.Should().Contain(_ => _.Name.Equals(goodslist[2].Name));
            _context.Goodses.Should().Contain(_ => _.CategoryId.Equals(goodslist[2].CategoryId));
            _context.Goodses.Should().Contain(_ => _.MaxInventory.Equals(goodslist[2].MaxInventory));
            _context.Goodses.Should().Contain(_ => _.MinInventory.Equals(goodslist[2].MinInventory));
            _context.Goodses.Should().Contain(_ => _.Inventory.Equals(goodslist[2].Inventory));
            _context.Goodses.Should().Contain(_ => _.Cost.Equals(goodslist[2].Cost));
        }

    }
}
