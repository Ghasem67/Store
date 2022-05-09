using Store.Infrastracture.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.GoodsInputs.Contracts
{
    public interface GoodsInputService:Service 
    {
        void Add(AddGoodsInputDTO addGoodsInputDto);
        void Update(UpdateGoodsInputDTO updateGoodsInputDTO,int id);
        void Delete(int id);
        HashSet<ShowGoodsInputDTO> GetAll();
        ShowGoodsInputDTO GetById(int id);
    }
}
