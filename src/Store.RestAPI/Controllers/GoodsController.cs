using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Services.Goodses.Contracts;
using System.Collections.Generic;

namespace Store.RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoodsController : ControllerBase
    {
        private readonly GoodsService _goodsService;

        public GoodsController(GoodsService goodsService)
        {
            _goodsService = goodsService;
        }
        [HttpGet]
        public HashSet<ShowgoodsDTO> GetAll()
        {
            return _goodsService.GetAll();
        }
        [HttpGet("{GoodsCode}")]
        public ShowgoodsDTO GetById(int GoodsCode)
        {
            return _goodsService.GetbyId(GoodsCode);
        }
        [HttpPost]
        public void Add(AddGoodsDTO addGoodsDTO)
        {
            _goodsService.Add(addGoodsDTO);
        }
        [HttpPut("{GoodsCode}")]
        public void Update(UpdateGoodsDTO updateGoodsDTO,int goodsCode)
        {
            _goodsService.Update(updateGoodsDTO, goodsCode);
        }
        [HttpDelete("{GoodsCode}")]
        public void Delete(int goodsCode)
        {
            _goodsService.Delete(goodsCode);
        }
    }
}
