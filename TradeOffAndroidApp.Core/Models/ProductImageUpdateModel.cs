namespace TradeOffAndroidApp.Core
{
    public class ProductImageUpdateModel
    {
        public int ProductId { get; set; }
        public byte[] Image { get; set; }
        public bool IsMainImage { get; set; }
    }
}
