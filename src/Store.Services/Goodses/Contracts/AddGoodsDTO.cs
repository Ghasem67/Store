namespace Store.Services.Goodses.Contracts
{
    public class AddGoodsDTO
    {
        public int GoodsCode { get; set; }
        public string Name { get; set; }
        public int Cost { get; set; }
        public int Inventory { get; set; }
        public int MinInventory { get; set; }
        public int MaxInventory { get; set; }
        public int CategoryId { get; set; }
    }
}