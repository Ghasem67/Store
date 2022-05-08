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

        public void Add(AddgoodsoutputDTO addgoodsoutputDTO)
        {
            DateTime dateTime = new DateTime();
            bool IsDate = DateTime.TryParse(addgoodsoutputDTO.Date, out dateTime);
            if (!IsDate)
            {
                throw new DatetimeFormatException();
            }
            GoodsOutput goodoutput=new GoodsOutput
            {
                Count = addgoodsoutputDTO.Count,
                Date = dateTime,
                GoodsCode = addgoodsoutputDTO.GoodsCode,
                Number = addgoodsoutputDTO.Number,
                Price = addgoodsoutputDTO.Price
            };
            _repository.Add(goodoutput);
            _unitOfWork.Commit();
        }

        public void Delete(int id)
        {

            var goodsInput = _repository.GetById(id);
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

        public ShowGoodsOutputDTO GetById(int id)
        {
            return _repository.GetOne(id);
        }

        public void Update(UpdateGoodsOutputDTO updateGoodsOutputDTO,int id )
        {
            var goodsoutput = _repository.GetById(id);
            if (goodsoutput == null)
            {
                throw new GoodsOutputNotFoundException();
            }
            DateTime dateTime = new DateTime();
            bool IsDate = DateTime.TryParse(updateGoodsOutputDTO.Date, out dateTime);
            if (!IsDate)
            {
                throw new DatetimeFormatException();
            }
            goodsoutput.Number = updateGoodsOutputDTO.Number;
            goodsoutput.GoodsCode = updateGoodsOutputDTO.GoodsCode;
            goodsoutput.Price = updateGoodsOutputDTO.Price;
            goodsoutput.Count = updateGoodsOutputDTO.Count;
            goodsoutput.Date = dateTime;
            _unitOfWork.Commit();
        }
    }
}
