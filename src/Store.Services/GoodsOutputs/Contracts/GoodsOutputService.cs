﻿using Store.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.GoodsOutputs.Contracts
{
    public interface GoodsOutputService
    {
        void Add(AddgoodsoutputDTO addgoodsoutputDTO);
        void Delete(int id);
        void Update(UpdateGoodsOutputDTO updateGoodsOutputDTO, int id);
        ShowGoodsOutputDTO GetById(int id);
        HashSet<ShowGoodsOutputDTO> GetAll();
    }
}