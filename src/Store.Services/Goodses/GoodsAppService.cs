using Store.Entities;
using Store.Infrastracture.Application;
using Store.Services.Goodses.Contracts;
using Store.Services.Goodses.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.Goodses
{
    public class GoodsAppService : GoodsService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly GoodsRepository _goodsRepository;

        public GoodsAppService(GoodsRepository goodsRepository, UnitOfWork unitOfWork)
        {
            _goodsRepository = goodsRepository;
            _unitOfWork = unitOfWork;
        }

        public void Add(AddGoodsDTO addGoodsDTO)
        {
            CheckingDuplicateCode(addGoodsDTO.GoodsCode);
            CheckingDuplicateName(addGoodsDTO.Name);
            Goods goods = new Goods()
            {
                CategoryId = addGoodsDTO.CategoryId,
                Cost = addGoodsDTO.Cost,
                GoodsCode = addGoodsDTO.GoodsCode,
                Inventory = addGoodsDTO.Inventory,
                MaxInventory = addGoodsDTO.MaxInventory,
                Name = addGoodsDTO.Name,
                MinInventory = addGoodsDTO.MinInventory
            };
            _goodsRepository.Add(goods);
            _unitOfWork.Commit();
        }

        public void Delete(int GoodsCode)
        {
            var goods = CheckingNull(GoodsCode);
            HasChild(goods);
            _goodsRepository.Delete(goods);
            _unitOfWork.Commit();
        }

       

        public HashSet<ShowgoodsDTO> GetAll()
        {
            var goods = _goodsRepository.GetAll();
            ListisNull(goods);
            return goods;
        }

        public ShowgoodsDTO GetbyId(int id)
        {
            return _goodsRepository.GetOne(id);
        }

        public void Update(UpdateGoodsDTO updateGoodsDTO, int GoodsCode)
        {
            CheckingDuplicateName(updateGoodsDTO.Name);
            var goods = CheckingNull(GoodsCode);
            goods.Cost = updateGoodsDTO.Cost;
            goods.Name = updateGoodsDTO.Name;
            goods.Inventory = updateGoodsDTO.Inventory;
            goods.MinInventory = updateGoodsDTO.MinInventory;
            goods.MaxInventory = updateGoodsDTO.MaxInventory;
            goods.Inventory = updateGoodsDTO.Inventory;
            goods.CategoryId = updateGoodsDTO.CategoryId;
            _unitOfWork.Commit();
        }
        private Goods CheckingNull(int GoodsCode)
        {
            var goods = _goodsRepository.GetbyId(GoodsCode);
            if (goods == null)
            {
                throw new GoodsNotFoundException();
            }
            return goods;
        }
        private void CheckingDuplicateName(string GoodsName)
        {
            var goods = _goodsRepository.GetByName(GoodsName);
            if (goods != null)
            {
                throw new DuplicateNameException();
            }
        }
        private void CheckingDuplicateCode(int GoodsCode)
        {
            var OneGoods = _goodsRepository.GetbyId(GoodsCode);
            if (OneGoods != null)
            {
                throw new DuplicateGoodsCodeException();
            }
        }
        private void ListisNull(HashSet<ShowgoodsDTO> goods)
        {
            if (goods.Count() == 0)
            {
                throw new ThereIsnotInformationToDisplay();
            }
        }
        private void HasChild(Goods goods)
        {
            if (goods.GoodsInputs.Count > 0 || goods.GoodsOutputs.Count() > 0)
            {
                throw new GoodsHasChildrenException();
            }
        }
    }
}
