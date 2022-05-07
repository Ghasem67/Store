using Store.Entities;
using Store.Infrastracture.Application;
using Store.Services.GoodsInputs.Contracts;
using Store.Services.GoodsInputs.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.GoodsInputs
{
    public class GoodsInputAppService : GoodsInputService
    {
        private readonly GoodsInputRepository _goodsRepository;
        private readonly UnitOfWork _unitOfWork;

        public GoodsInputAppService(UnitOfWork unitOfWork,
            GoodsInputRepository goodsRepository)
        {
            _unitOfWork = unitOfWork;
            _goodsRepository = goodsRepository;
        }

        public void Add(AddGoodsInputDTO addGoodsInputDto)
        {
            DateTime dateTime = new DateTime();
            bool IsDate = DateTime.TryParse(addGoodsInputDto.Date, out dateTime);
            if (!IsDate)
            {
                throw new DatetimeFormatException();
            }
            GoodsInput goodsInput = new()
            {
                Count = addGoodsInputDto.Count,
                Date = dateTime,
                GoodsCode = addGoodsInputDto.GoodsCode,
                Number = addGoodsInputDto.Number,
                Price = addGoodsInputDto.Price
            };
            _goodsRepository.Add(goodsInput);
            _unitOfWork.Commit();
        }

        public void Delete(int id)
        {
           var GoodsInput=_goodsRepository.GetById(id);
            if (GoodsInput==null)
            {
                throw new GoodsInputNotFoundException();
            }
            _goodsRepository.Delete(GoodsInput);
            _unitOfWork.Commit();
        }

        public HashSet<ShowGoodsInputDTO> GetAll()
        {
           return _goodsRepository.GetAll();
        }

        public ShowGoodsInputDTO GetOneGoodsInput(int id)
        {
            return _goodsRepository.GetOneGoodsInput(id);
        }

        public void Update(UpdateGoodsInputDTO updateGoodsInputDTO,int id)
        {
            var goodsInput = _goodsRepository.GetById(id);
            if (goodsInput == null)
            {
                throw new GoodsInputNotFoundException();
            }
            DateTime dateTime = new DateTime();
            bool IsDate = DateTime.TryParse(updateGoodsInputDTO.Date, out dateTime);
            if (!IsDate)
            {
                throw new DatetimeFormatException();
            }

            goodsInput.Number = updateGoodsInputDTO.Number;
            goodsInput.GoodsCode = updateGoodsInputDTO.GoodsCode;
            goodsInput.Price = updateGoodsInputDTO.Price;
            goodsInput.Count = updateGoodsInputDTO.Count;
            goodsInput.Date = dateTime;
            _unitOfWork.Commit();
        }
    }
}
