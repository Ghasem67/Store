using Store.Entities;
using Store.Infrastracture.Application;
using Store.Services.GoodsOutputs.Contracts;
using Store.Services.GoodsOutputs.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.GoodsOutputs
{
    public class GoodsOutputAppService : GoodsOutputService
    {
        private readonly GoodsOutputRepository _repository;
        private readonly UnitOfWork _unitOfWork;

        public GoodsOutputAppService(UnitOfWork unitOfWork, GoodsOutputRepository repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public void Add(AddgoodsOutputDTO addgoodsoutputDTO)
        {
            CheckDuplicate(addgoodsoutputDTO.Number);
            GoodsOutput goodoutput=new GoodsOutput
            {
                Count = addgoodsoutputDTO.Count,
                Date = IsDateTimeFormat(addgoodsoutputDTO.Date),
                GoodsCode = addgoodsoutputDTO.GoodsCode,
                Number = addgoodsoutputDTO.Number,
                Price = addgoodsoutputDTO.Price
            };
            _repository.Add(goodoutput);
            _unitOfWork.Commit();
        }

        public void Delete(int number)
        {

            var goodsInput = _repository.GetById(number);
            if (goodsInput == null)
            {
                throw new GoodsOutputNotFoundException();
            }
            _repository.Delete(goodsInput);
            _unitOfWork.Commit();
        }

        public HashSet<ShowGoodsOutputDTO> GetAll()
        {
          return  _repository.GetAll();
        }

        public ShowGoodsOutputDTO GetById(int number)
        {
            return _repository.GetOne(number);
        }

        public void Update(UpdateGoodsOutputDTO updateGoodsOutputDTO,int number )
        {
            var goodsoutput = CheckIsNull(number);


            goodsoutput.Number = updateGoodsOutputDTO.Number;
            goodsoutput.GoodsCode = updateGoodsOutputDTO.GoodsCode;
            goodsoutput.Price = updateGoodsOutputDTO.Price;
            goodsoutput.Count = updateGoodsOutputDTO.Count;
            goodsoutput.Date = IsDateTimeFormat(updateGoodsOutputDTO.Date);
            _unitOfWork.Commit();
        }
        private GoodsOutput CheckIsNull(int number)
        {
            var goodsOutput = _repository.GetById(number);
            if (goodsOutput == null)
            {
                throw new GoodsOutputNotFoundException();
            }
            return goodsOutput;
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
        private GoodsOutput CheckDuplicate(int number)
        {
            var goodsOutput = _repository.GetById(number);
            if (goodsOutput != null)
            {
                throw new DuplicateFactorNumberException();
            }
            return goodsOutput;
        }
    }
}
