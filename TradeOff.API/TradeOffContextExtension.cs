using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using TradeOff.API.Entities;
namespace TradeOff.API
{
    public static class TradeOffContextExtension
    {        
        public static void EnsureSeedDataForContext(this TradeOffContext context)
        {
            var seedResources = new SeedResources();
            if (!context.Categories.Any())
            {
                var Categories = new List<Category>()
                {
                new Category()
                {
                    CategoryName = "Electronics",
                    Products = new List<Product>()
                    {
                        new Product()
                        {
                            Name = "iphone 7",
                            Location = "54.211,-8.288",
                            ShortDescription = "rarely used iphone 7",
                            FullDescription = "aquired as a gift, already got a brand new cup and string communication device this year, do not need new Iphone",
                            Price = 450,
                            UserId = 1,
                            ProductImages = new List<ProductImage>()
                            {
                                new ProductImage()
                                {
                                    Image = seedResources.ImageToByteArray(Image.FromFile("SeedImages/Iphone1.jpg")),
                                    IsMainImage = true
                                },
                                new ProductImage()
                                {
                                    Image = seedResources.ImageToByteArray(Image.FromFile("SeedImages/iphone2.png")),
                                    IsMainImage = false
                                }
                            }
                        },
                        new Product()
                        {
                            Name = "Acer Laptop",
                            Location = "54.311,-8.298",
                            ShortDescription = "new acer Laptop",
                            FullDescription = "brnad new acer laptop, faster than the speed of light. Used sporadically as a placemat",
                            Price = 150,
                            UserId = 2,
                            ProductImages = new List<ProductImage>()
                            {
                                new ProductImage()
                                {
                                    Image = seedResources.ImageToByteArray(Image.FromFile("SeedImages/Acer1.png")),
                                    IsMainImage = true
                                },
                                new ProductImage()
                                {
                                    Image = seedResources.ImageToByteArray(Image.FromFile("SeedImages/Acer2.png")),
                                    IsMainImage = false
                                }
                            }
                        },
                        new Product()
                        {
                            Name = "xbox one",
                            Location = "54.122,-8.333",
                            ShortDescription = "second hand xbox one",
                            FullDescription = "almost new, good for playing games and watching netflix",
                            Price = 450,
                            UserId = 1,
                            ProductImages = new List<ProductImage>()
                            {
                                new ProductImage()
                                {
                                    Image = seedResources.ImageToByteArray(Image.FromFile("SeedImages/xbox1.jpg")),
                                    IsMainImage = true
                                },
                                new ProductImage()
                                {
                                    Image = seedResources.ImageToByteArray(Image.FromFile("SeedImages/xbox2.jpg")),
                                    IsMainImage = false
                                }
                            }
                        },
                        new Product()
                        {
                            Name = "radio",
                            Location = "50.211,-7.288",
                            ShortDescription = "brand new portable radio",
                            FullDescription = "plays radio stations and other things",
                            Price = 20,
                            UserId = 4,
                            ProductImages = new List<ProductImage>()
                            {
                                new ProductImage()
                                {
                                    Image = seedResources.ImageToByteArray(Image.FromFile("SeedImages/radio1.jpg")),
                                    IsMainImage = true
                                },
                                new ProductImage()
                                {
                                    Image = seedResources.ImageToByteArray(Image.FromFile("SeedImages/radio2.jpg")),
                                    IsMainImage = false
                                }
                            }
                        },
                        new Product()
                        {
                            Name = "Nokia 3310",
                            Location = "55.211,-9.288",
                            ShortDescription = "Phone from the golden age",
                            FullDescription = "Possibly the greatest phone of all time, brand new selling at good price",
                            Price = 55,
                            UserId = 2,
                            ProductImages = new List<ProductImage>()
                            {
                                new ProductImage()
                                {
                                    Image = seedResources.ImageToByteArray(Image.FromFile("SeedImages/nokia1.jpg")),
                                    IsMainImage = true
                                },
                                new ProductImage()
                                {
                                    Image = seedResources.ImageToByteArray(Image.FromFile("SeedImages/nokia2.jpg")),
                                    IsMainImage = false
                                }
                            }
                        },
                        new Product()
                        {
                            Name = "Pc monitor",
                            Location = "34.211,-5.288",
                            ShortDescription = "second hand philips pc monitor",
                            FullDescription = "21' monitor, very useful for using a desktop",
                            Price = 125,
                            UserId = 4,
                            ProductImages = new List<ProductImage>()
                            {
                                new ProductImage()
                                {
                                    Image = seedResources.ImageToByteArray(Image.FromFile("SeedImages/phillips1.jpg")),
                                    IsMainImage = true
                                },
                                new ProductImage()
                                {
                                    Image = seedResources.ImageToByteArray(Image.FromFile("SeedImages/phillips2.jpg")),
                                    IsMainImage = false
                                }
                            }
                        },
                            new Product()
                            {
                                Name = "samsung s7",
                                Location = "51.211,-7.288",
                                ShortDescription = "brand new phone",
                                FullDescription = "great phone very fast at calling people and stuff",
                                Price = 550,
                                UserId = 3,
                                ProductImages = new List<ProductImage>()
                                {
                                    new ProductImage()
                                    {
                                        Image = seedResources.ImageToByteArray(Image.FromFile("SeedImages/samsung1.jpg")),
                                        IsMainImage = true
                                    },
                                    new ProductImage()
                                    {
                                        Image = seedResources.ImageToByteArray(Image.FromFile("SeedImages/samsung2.jpg")),
                                        IsMainImage = false
                                    }
                                }
                            }

                        }

                    },
                    new Category()
                    {
                        CategoryName = "Sports gear",
                        Products = new List<Product>
                        {
                            new Product()
                            {
                                Name = "football",
                                Location = "51.311,-8.278",
                                ShortDescription = "brand new soccer ball",
                                FullDescription = "good for kicking into goals!",
                                Price = 10 ,
                                UserId = 1,
                                ProductImages = new List<ProductImage>()
                                {
                                    new ProductImage()
                                    {
                                        Image = seedResources.ImageToByteArray(Image.FromFile("SeedImages/ball1.jpg")),
                                        IsMainImage = true
                                    },
                                    new ProductImage()
                                    {
                                        Image = seedResources.ImageToByteArray(Image.FromFile("SeedImages/ball2.jpg")),
                                        IsMainImage = false
                                    }
                                }
                            },
                            new Product()
                            {
                                Name = "surf board",
                                Location = "54.311,-8.212",
                                ShortDescription = "second hand surf bopard",
                                FullDescription = "Used surf board still in good condition",
                                Price = 150 ,
                                UserId = 2,
                                ProductImages = new List<ProductImage>()
                                {
                                    new ProductImage()
                                    {
                                        Image = seedResources.ImageToByteArray(Image.FromFile("SeedImages/board1.jpg")),
                                        IsMainImage = true
                                    },
                                    new ProductImage()
                                    {
                                        Image = seedResources.ImageToByteArray(Image.FromFile("SeedImages/board2.jpg")),
                                        IsMainImage = false
                                    }
                                }
                            },
                            new Product()
                            {
                                Name = "Kayak",
                                Location = "55.311,-8.278",
                                ShortDescription = "Second hand Kayak",
                                FullDescription = "great beginner kayak, rarely used",
                                Price = 200 ,
                                UserId = 3,
                                ProductImages = new List<ProductImage>()
                                {
                                    new ProductImage()
                                    {
                                        Image = seedResources.ImageToByteArray(Image.FromFile("SeedImages/kayak1.jpg")),
                                        IsMainImage = true
                                    },
                                    new ProductImage()
                                    {
                                        Image = seedResources.ImageToByteArray(Image.FromFile("SeedImages/kayak2.jpg")),
                                        IsMainImage = false
                                    }
                                }
                            },
                        }
                    },
                    new Category()
                    {
                        CategoryName = "Household Appliances",
                        Products = new List<Product>
                        {
                            new Product()
                            {
                                Name = "kettle",
                                Location = "51.311,-8.278",
                                ShortDescription = "brand new kettle",
                                FullDescription = "good for boiling water",
                                Price = 20 ,
                                UserId = 1,
                                ProductImages = new List<ProductImage>()
                                {
                                    new ProductImage()
                                    {
                                        Image = seedResources.ImageToByteArray(Image.FromFile("SeedImages/kettle1.jpg")),
                                        IsMainImage = true
                                    },
                                    new ProductImage()
                                    {
                                        Image = seedResources.ImageToByteArray(Image.FromFile("SeedImages/kettle2.jpg")),
                                        IsMainImage = false
                                    }
                                }
                            },
                            new Product()
                            {
                                Name = "Microwave",
                                Location = "52.311,-8.252",
                                ShortDescription = "second hand microwave",
                                FullDescription = "Used for one year, 700 watts still working perfect",
                                Price = 150 ,
                                UserId = 2,
                                ProductImages = new List<ProductImage>()
                                {
                                    new ProductImage()
                                    {
                                        Image = seedResources.ImageToByteArray(Image.FromFile("SeedImages/microwave1.jpg")),
                                        IsMainImage = true
                                    },
                                    new ProductImage()
                                    {
                                        Image = seedResources.ImageToByteArray(Image.FromFile("SeedImages/microwave2.jpg")),
                                        IsMainImage = false
                                    }
                                }
                            },
                            new Product()
                            {
                                Name = "Blender",
                                Location = "51.311,-8.278",
                                ShortDescription = "new blender",
                                FullDescription = "good for making smoothies and blending stuff",
                                Price = 200 ,
                                UserId = 3,
                                ProductImages = new List<ProductImage>()
                                {
                                    new ProductImage()
                                    {
                                        Image = seedResources.ImageToByteArray(Image.FromFile("SeedImages/blender1.jpg")),
                                        IsMainImage = true
                                    },
                                    new ProductImage()
                                    {
                                        Image = seedResources.ImageToByteArray(Image.FromFile("SeedImages/blender2.jpg")),
                                        IsMainImage = false
                                    }
                                }
                            },
                        }
                    },
                    new Category()
                    {
                        CategoryName = "gardening equipement",
                        Products = new List<Product>
                        {
                            new Product()
                            {
                                Name = "lawnmower",
                                Location = "54.311,-7.278",
                                ShortDescription = "second hand push lawnmower",
                                FullDescription = "second hand lawnmower great for cutting grass and staying in shape",
                                Price = 90 ,
                                UserId = 1,
                                ProductImages = new List<ProductImage>()
                                {
                                    new ProductImage()
                                    {
                                        Image = seedResources.ImageToByteArray(Image.FromFile("SeedImages/mower1.jpg")),
                                        IsMainImage = true
                                    },
                                    new ProductImage()
                                    {
                                        Image = seedResources.ImageToByteArray(Image.FromFile("SeedImages/mower2.jpg")),
                                        IsMainImage = false
                                    }
                                }
                            },
                            new Product()
                            {
                                Name = "Strimmer",
                                Location = "54.311,-8.212",
                                ShortDescription = "second hand strimmer",
                                FullDescription = "good strimmer for sale, only used for two seasons",
                                Price = 150 ,
                                UserId = 2,
                                ProductImages = new List<ProductImage>()
                                {
                                    new ProductImage()
                                    {
                                        Image = seedResources.ImageToByteArray(Image.FromFile("SeedImages/strimmer1.jpg")),
                                        IsMainImage = true
                                    },
                                    new ProductImage()
                                    {
                                        Image = seedResources.ImageToByteArray(Image.FromFile("SeedImages/strimmer2.jpg")),
                                        IsMainImage = false
                                    }
                                }
                            },
                            new Product()
                            {
                                Name = "hedge trimmer",
                                Location = "55.311,-8.278",
                                ShortDescription = "new electric hedge trimmer",
                                FullDescription = "brand new electric hedge trimmer with telescopic attachments",
                                Price = 200 ,
                                UserId = 3,
                                ProductImages = new List<ProductImage>()
                                {
                                    new ProductImage()
                                    {
                                        Image = seedResources.ImageToByteArray(Image.FromFile("SeedImages/trimmer1.jpg")),
                                        IsMainImage = true
                                    },
                                    new ProductImage()
                                    {
                                        Image = seedResources.ImageToByteArray(Image.FromFile("SeedImages/trimmer2.jpg")),
                                        IsMainImage = false
                                    }
                                }
                            },
                        }
                    },
                    new Category()
                    {
                        CategoryName = "furniture",
                        Products = new List<Product>
                        {
                            new Product()
                            {
                                Name = "sofa",
                                Location = "51.311,-8.278",
                                ShortDescription = "second hand sofa",
                                FullDescription = "still in great condition, fits four people, newly upholstered",
                                Price = 10 ,
                                UserId = 1,
                                ProductImages = new List<ProductImage>()
                                {
                                    new ProductImage()
                                    {
                                        Image = seedResources.ImageToByteArray(Image.FromFile("SeedImages/sofa1.jpg")),
                                        IsMainImage = true
                                    },
                                    new ProductImage()
                                    {
                                        Image = seedResources.ImageToByteArray(Image.FromFile("SeedImages/sofa2.jpg")),
                                        IsMainImage = false
                                    }
                                }
                            },
                            new Product()
                            {
                                Name = "oak table",
                                Location = "54.311,-8.212",
                                ShortDescription = "second hand oak dining table",
                                FullDescription = "good quality table solid oak, sits 8 people",
                                Price = 150 ,
                                UserId = 2,
                                ProductImages = new List<ProductImage>()
                                {
                                    new ProductImage()
                                    {
                                        Image = seedResources.ImageToByteArray(Image.FromFile("SeedImages/table1.jpg")),
                                        IsMainImage = true
                                    },
                                    new ProductImage()
                                    {
                                        Image = seedResources.ImageToByteArray(Image.FromFile("SeedImages/table2.jpg")),
                                        IsMainImage = false
                                    }
                                }
                            },
                            new Product()
                            {
                                Name = "bed frame",
                                Location = "55.311,-8.278",
                                ShortDescription = "Second hand aluminium bed frame",
                                FullDescription = "good bed frame, light weight, easy to transport",
                                Price = 200 ,
                                UserId = 3,
                                ProductImages = new List<ProductImage>()
                                {
                                    new ProductImage()
                                    {
                                        Image = seedResources.ImageToByteArray(Image.FromFile("SeedImages/bed1.jpg")),
                                        IsMainImage = true
                                    },
                                    new ProductImage()
                                    {
                                        Image = seedResources.ImageToByteArray(Image.FromFile("SeedImages/bed2.jpg")),
                                        IsMainImage = false
                                    }
                                }
                            },
                        }
                    },
                };
                context.Categories.AddRange(Categories);
                context.SaveChanges();
            }
        }
    }
    public class SeedResources
    {
        public byte[] ImageToByteArray(Image image)
        {
            byte[] arr;
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                arr = ms.ToArray();
            }
            return arr;
        }
    }
}
