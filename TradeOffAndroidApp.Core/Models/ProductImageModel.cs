﻿namespace TradeOffAndroidApp.Core
{
    public class ProductImageModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public byte[] Image { get; set; }
        public bool IsMainImage { get; set; }
    }
}
