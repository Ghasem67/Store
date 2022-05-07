using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Services.GoodsOutputs.Contracts;
using System.Collections.Generic;

namespace Store.RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoodsOutputController : ControllerBase
    {
        private readonly GoodsOutputService _service;

        public GoodsOutputController(GoodsOutputService service)
        {
            _service = service;
        }

        [HttpGet]
        public HashSet<ShowGoodsOutputDTO> GetAll()
        {
            return _service.GetAll();
        }
        [HttpGet("{id}")]
        public ShowGoodsOutputDTO GetById(int id)
        {
            return _service.GetById(id);
        }
        [HttpPost()]
        public void Add(AddgoodsoutputDTO addgoodsoutputDTO)
        {
            _service.Add(addgoodsoutputDTO);
        }
        [HttpPut("{id}")]
        public void Update(UpdateGoodsOutputDTO updateGoodsOutputDTO,int id)
        {
            _service.Update(updateGoodsOutputDTO,id);
        }
        [HttpDelete("{id}")]
        public void Delete( int id)
        {
            _service.Delete(id);
        }
    }
}
