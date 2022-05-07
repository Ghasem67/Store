namespace Store.Services.GoodsOutputs.Contracts
{
    public class UpdateGoodsOutputDTO
    {
        public int Number { get; set; }
        public string Date { get; set; }
        public int GoodsCode { get; set; }
        public int Price { get; set; }
        public int Count { get; set; }
    }
}