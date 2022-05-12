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
            CheckDuplicate(addGoodsInputDto.Number);

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
            var goodsinput= _goodsInputRepository.GetAll();
            if (goodsinput.Count==0)
            {
                throw new ThereIsnotInformationToDisplay();
            }
            return goodsinput;
        }

        public ShowGoodsInputDTO GetById(int numder)
        {
            return _goodsInputRepository.GetOneGoodsInput(numder);
        }

        public void Update(UpdateGoodsInputDTO updateGoodsInputDTO,int Number)
        {
         var goodsInput=  CheckIsNull(Number);
            CheckDuplicate(updateGoodsInputDTO.Number);
            //goodsInput.Number = updateGoodsInputDTO.Number;
           goodsInput.GoodsCode = updateGoodsInputDTO.GoodsCode;
            goodsInput.Price = updateGoodsInputDTO.Price;
            goodsInput.Count = updateGoodsInputDTO.Count;
            goodsInput.Date = IsDateTimeFormat(updateGoodsInputDTO.Date);
            _unitOfWork.Commit();
        }
        private GoodsInput CheckIsNull(int number)
        {
            var goodsInput = _goodsInputRepository.GetById(number);
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
        private void  CheckDuplicate(int number)
        {
            var goodsInput = _goodsInputRepository.GetById(number);
            if (goodsInput != null)
            {
                throw new DuplicateFactorNumberException();
            }
        }
    }
}
