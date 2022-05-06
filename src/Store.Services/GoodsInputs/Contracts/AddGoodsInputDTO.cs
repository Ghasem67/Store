namespace Store.Services.GoodsInputs.Contracts
{
    public class AddGoodsInputDTO
    {
        public int Number { get; set; }
        public string Date { get; set; }
        public int GoodsId { get; set; }
        public int Price { get; set; }
        public int Count { get; set; }
        public string  GoodsName { get; set; }
    }
}