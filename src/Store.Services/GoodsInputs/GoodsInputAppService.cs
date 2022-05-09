﻿using Store.Entities;
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
        private readonly GoodsInputRepository _goodsInputRepository;
        private readonly UnitOfWork _unitOfWork;

        public GoodsInputAppService(UnitOfWork unitOfWork,
            GoodsInputRepository goodsRepository)
        {
            _unitOfWork = unitOfWork;
            _goodsInputRepository = goodsRepository;
        }

        public void Add(AddGoodsInputDTO addGoodsInputDto)
        {
           
            GoodsInput goodsInput = new()
            {
                Count = addGoodsInputDto.Count,
                Date = IsDateTimeFormat(addGoodsInputDto.Date),
                GoodsCode = addGoodsInputDto.GoodsCode,
                Number = addGoodsInputDto.Number,
                Price = addGoodsInputDto.Price
            };
            _goodsInputRepository.Add(goodsInput);
            _unitOfWork.Commit();
        }

        public void Delete(int id)
        {
            var goodsInput = CheckIsNull(id);
            _goodsInputRepository.Delete(goodsInput);
            _unitOfWork.Commit();
        }

        public HashSet<ShowGoodsInputDTO> GetAll()
        {
           return _goodsInputRepository.GetAll();
        }

        public ShowGoodsInputDTO GetOneGoodsInput(int id)
        {
            return _goodsInputRepository.GetOneGoodsInput(id);
        }

        public void Update(UpdateGoodsInputDTO updateGoodsInputDTO,int id)
        {
         var goodsInput=  CheckIsNull(id);

            goodsInput.Number = updateGoodsInputDTO.Number;
            goodsInput.GoodsCode = updateGoodsInputDTO.GoodsCode;
            goodsInput.Price = updateGoodsInputDTO.Price;
            goodsInput.Count = updateGoodsInputDTO.Count;
            goodsInput.Date = IsDateTimeFormat(updateGoodsInputDTO.Date);
            _unitOfWork.Commit();
        }
        private GoodsInput CheckIsNull(int id)
        {
            var goodsInput = _goodsInputRepository.GetById(id);
            if (goodsInput == null)
            {
                throw new GoodsInputNotFoundException();
            }
            return goodsInput;
        }
        private DateTime IsDateTimeFormat(string InputDate)
        {
            DateTime date = new DateTime();
            bool IsDate = DateTime.TryParse(InputDate, out date);
            if (!IsDate)
            {
                throw new DatetimeFormatException();
            }
            return date;
        }
    }
}
