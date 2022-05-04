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
        [HttpPost]
        public void Add(AddGoodsDTO addGoodsDTO)
        {
            _goodsService.Add(addGoodsDTO);
        }
        [HttpPatch("{id}")]
        public void Update(UpdateGoodsDTO updateGoodsDTO,int id)
        {
            _goodsService.Update(updateGoodsDTO,id);
        }
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _goodsService.Delete(id);
        }
    }
}
