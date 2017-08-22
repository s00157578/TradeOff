namespace TradeOffAndroidApp.Core
{
    public class ProductModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public string UserId { get; set; }        
    }
}
