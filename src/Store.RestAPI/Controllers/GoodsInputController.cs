using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Infrastracture.Application;
using Store.Services.GoodsInputs.Contracts;
using System.Collections.Generic;

namespace Store.RestAPI.Controllers
{
    [Route("api/GoodsInput")]
    [ApiController]
    public class GoodsInputController : ControllerBase
    {
        private readonly GoodsInputService _goodsInputService;
        private readonly UnitOfWork _unitOfWork;

        public GoodsInputController(UnitOfWork unitOfWork, GoodsInputService goodsInputService)
        {
            _unitOfWork = unitOfWork;
            _goodsInputService = goodsInputService;
        }
        [HttpGet]
        public HashSet<ShowGoodsInputDTO> GetAll()
        {
            return _goodsInputService.GetAll();
        }
        [HttpGet("{id}")]
        public ShowGoodsInputDTO GetById(int id)
        {
            return _goodsInputService.GetById(id);
        }
        [HttpPost]
        public void Add(AddGoodsInputDTO addGoodsInputDTO)
        {
            _goodsInputService.Add(addGoodsInputDTO);
        }
        [HttpDelete("{id}")]
        public void Delete(int Id)
        {
            _goodsInputService.Delete(Id);
        }
        [HttpPut("{id}")]
        public void Update(UpdateGoodsInputDTO updateGoodsInputDTO, int id)
        {
            _goodsInputService.Update(updateGoodsInputDTO, id);
        }

    }
}
