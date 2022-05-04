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
            Goods goods = new Goods()
            {
                CategoryId = addGoodsDTO.CategoryId,
                Cost = addGoodsDTO.Cost,
                GoodsCode = addGoodsDTO.GoodsCode,
                Inventory=addGoodsDTO.Inventory,
                MaxInventory=addGoodsDTO.MaxInventory,
                Name=addGoodsDTO.Name,
                MinInventory=addGoodsDTO.MinInventory
            };
        _goodsRepository.Add(goods);
            _unitOfWork.Commit();
        }

        public void Delete(int id)
        {
           var goods = _goodsRepository.GetbyId(id);
            if (goods==null)
            {
                throw new GoodsNotFoundException();
            }
            _goodsRepository.Delete(goods);
            _unitOfWork.Commit();
        }

        public HashSet<ShowgoodsDTO> GetAll()
        {
            return _goodsRepository.GetAll();
        }

        public ShowgoodsDTO GetOneGoods(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(UpdateGoodsDTO updateGoodsDTO, int id)
        {
            var goods = _goodsRepository.GetbyId(id);
            if (goods == null)
            {
              throw new GoodsNotFoundException();
            }
            goods.Cost = updateGoodsDTO.Cost;
            goods.Name = updateGoodsDTO.Name;
            goods.Inventory = updateGoodsDTO.Inventory;
            goods.MinInventory = updateGoodsDTO.MinInventory;
            goods.MaxInventory = updateGoodsDTO.MaxInventory;   
            goods.Inventory = updateGoodsDTO.Inventory;
            goods.CategoryId= updateGoodsDTO.CategoryId;
            _unitOfWork.Commit();
        }
    }
}
