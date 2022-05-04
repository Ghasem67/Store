namespace Store.Services.Goodses.Contracts
{
    public class ShowgoodsDTO
    {
        public int GoodsCode { get; set; }
        public string Name { get; set; }
        public int Cost { get; set; }
        public int Inventory { get; set; }
        public int MinInventory { get; set; }
        public int MaxInventory { get; set; }
        public string CategoryName { get; set; }
       
    }
}