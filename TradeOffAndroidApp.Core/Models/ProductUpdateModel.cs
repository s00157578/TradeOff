namespace TradeOffAndroidApp.Core
{
    public class ProductUpdateModel
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }
        public decimal Price { get; set; }
        public int UserId { get; set; }
    }
}
