using FOLYFOOD.Dto;
using FOLYFOOD.Dto.ProductDto;
using FOLYFOOD.Entitys;
using FOLYFOOD.Hellers;
using FOLYFOOD.Hellers.imageChecks;
using FOLYFOOD.IService.IProduct;
using ImageProcessor.Processors;
using Microsoft.EntityFrameworkCore;

namespace FOLYFOOD.Services.product
{
    public class ProductService : ProductInterface
    {
        public readonly Context DBContext;

        public ProductService()
        {
            DBContext = new Context();
        }
        public async Task<RetunObject<Product>> CreateProduct(ProductDto product)
        {
            ProductType checkTypeProduct = null;
            try
            {
                if (!product.ProductTypeId.HasValue || product.Price == null || product.Discount == null || product.Status == null || string.IsNullOrEmpty(product.NameProduct) || string.IsNullOrEmpty(product.Title) || product.AvartarImageProduct == null || product.Quantity < 0)
                {
                    throw new Exception("dữ liệu sản phẩm chuyền lên không đầy đủ");
                }
                if (!ImageChecker.IsImage(product.AvartarImageProduct))
                {
                    throw new Exception("có lỗi trong quá trình sử lý ảnh");
                }
                checkTypeProduct = await DBContext.ProductTypes.SingleOrDefaultAsync(x => x.ProductTypeId == product.ProductTypeId);
                if (checkTypeProduct == null)
                {
                    throw new Exception("loại sản phẩm thêm vào không tồn tại");
                }
                if(product.Price < 0) {
                    throw new Exception("giá không được là số âm");

                }
                if(product.Discount < 0 || product.Discount > 100) {
                    throw new Exception("giảm giá là % không được âm hoặc lớn hơn 100");
                }

            }
            catch (Exception ex)
            {
                return new RetunObject<Product>()
                {
                    data = null,
                    mess = ex.Message,
                    statusCode = 401
                };
            }
            string link = await uplloadFile.UploadFile(product.AvartarImageProduct);
            Product productCreate = new Product()
            {
                ProductTypeId = product.ProductTypeId.Value,
                AvartarImageProduct = link,
                NameProduct = product.NameProduct,
                Price = product.Price,
                Quantity = product.Quantity,
                Discount = product.Discount.Value,
                Status = product.Status.Value,
                Title = product.Title,

            };
            await DBContext.Products.AddAsync(productCreate);
            await DBContext.SaveChangesAsync();
            return new RetunObject<Product>()
            {
                data = productCreate,
                mess = "thêm thành công sản phẩm",
                statusCode = 201
            };
        }

        public async Task<RetunObject<Product>> deleteProduct(int productId)
        {
            Product checkProduct = null;
            try
            {
               checkProduct =  await DBContext.Products.FirstOrDefaultAsync(x => x.ProductId == productId);
                if(checkProduct == null)
                {
                    throw new Exception("không tìm thấy sản phẩm cần tìm ");
                }
            }
            catch (Exception ex)
            {
                return new RetunObject<Product>()
                {
                    data = null,
                    mess = ex.Message,
                    statusCode = 404
                };
            }
            await uplloadFile.DeleteFile(checkProduct.AvartarImageProduct);
            DBContext.Products.Remove(checkProduct);

            return new RetunObject<Product>()
            {
                data = checkProduct,
                mess = "đã xóa thành công",
                statusCode = 200
            };
        }

        public async Task<RetunObject<Product>> increaseViews(int productId)
        {
            Product checkProduct = null;
            try
            {
                checkProduct = await DBContext.Products.Include(x => x.ProductType).SingleOrDefaultAsync(x => x.ProductId == productId);
                if (checkProduct == null)
                {
                    throw new Exception("sản phẩm cần tăng lượt xem không tồn tại");
                }
            }
            catch (Exception ex)
            {
                return new RetunObject<Product>()
                {
                    data = null,
                    mess = ex.Message,
                    statusCode = 401
                };
            }
            checkProduct.number_of_views += 1;
            DBContext.Products.Update(checkProduct);
            DBContext.SaveChanges();
            return new RetunObject<Product>()
            {
                data = checkProduct,
                mess = "lấy sản phẩm thành công",
                statusCode = 200
            };

        }

        public async Task<RetunObject<Product>> getDetailproduct(int productId)
        {
            Product checkProduct = null;
            try
            {
                checkProduct = await DBContext.Products.Include(x => x.ProductType).SingleOrDefaultAsync(x => x.ProductId == productId);
                if (checkProduct == null)
                {
                    throw new Exception("sản phẩm cần lấy không tồn tại");
                }
            }
            catch (Exception ex)
            {
                return new RetunObject<Product>()
                {
                    data = null,
                    mess = ex.Message,
                    statusCode = 401
                };
            }
            return new RetunObject<Product>()
            {
                data = checkProduct,
                mess = "lấy sản phẩm thành công",
                statusCode = 200
            };
        }

        public async Task<IQueryable<Product>> getProducts(String? search = "", Double? priceFrom = 0, Double? priceTo = 0)
        {
            var data = DBContext.Products.Include(x => x.ProductType).Where(x => x.Status == 1).AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                data = data.Where(x => x.NameProduct.ToLower().Contains(search));
            }
            if(priceFrom != null && priceTo != null)
            {
                data = data.Where(x => x.Price >= priceFrom && x.Price <= priceTo);
            }
            foreach (var product in data)
            {
                product.ProductType.Products = null;
            }

            return data.OrderByDescending(x=>x.number_of_views).ThenByDescending(x=>x.ProductId);
        }

        public async Task<RetunObject<Product>> updateProduct(int productId, ProductDto product)
        {
            ProductType checkTypeProduct = null;
            Product checkProduct = null;
            try
            {
                if (!product.ProductTypeId.HasValue || product.Price == null || product.Discount == null || product.Status == null || string.IsNullOrEmpty(product.NameProduct) || string.IsNullOrEmpty(product.Title) || product.Quantity < 0)
                {
                    throw new Exception("dữ liệu sản phẩm chuyền lên không đầy đủ");
                }
                if (product.AvartarImageProduct != null)
                {
                    if (!ImageChecker.IsImage(product.AvartarImageProduct))
                    {
                        throw new Exception("có lỗi trong quá trình sử lý ảnh");
                    }
                }
                if (product.Price < 0)
                {
                    throw new Exception("giá không được là số âm");

                }
                if (product.Discount < 0 || product.Discount > 100)
                {
                    throw new Exception("giảm giá là % không được âm hoặc lớn hơn 100");
                }

                checkTypeProduct = await DBContext.ProductTypes.SingleOrDefaultAsync(x => x.ProductTypeId == product.ProductTypeId);
                if (checkTypeProduct == null)
                {
                    throw new Exception("loại sản phẩm  không tồn tại");
                }
                checkProduct = await DBContext.Products.SingleOrDefaultAsync(x => x.ProductId == productId);
                if (checkProduct == null)
                {
                    throw new Exception("sản phẩm không tồn tại");
                }
            }
            catch (Exception ex)
            {
                return new RetunObject<Product>()
                {
                    data = null,
                    mess = ex.Message,
                    statusCode = 401
                };
            }
            string link = checkProduct.AvartarImageProduct;
            if (product.AvartarImageProduct != null)
            {
                link = await uplloadFile.UpdateFile(link, product.AvartarImageProduct);
            }
            checkProduct.ProductTypeId = product.ProductTypeId.Value;
            checkProduct.AvartarImageProduct = link;
            checkProduct.NameProduct = product.NameProduct;
            checkProduct.Price = product.Price;
            checkProduct.Quantity = product.Quantity;
            checkProduct.Discount = product.Discount.Value;
            checkProduct.Status = product.Status.Value;
            checkProduct.Title = product.Title;
            checkProduct.UpdatedAt = DateTime.Now;
          DBContext.Products.Update(checkProduct);
          await DBContext.SaveChangesAsync();
            return new RetunObject<Product>()
            {
                data = checkProduct,
                mess = "sửa sản phẩm thành công",
                statusCode = 200
            };
        }

        public async Task<RetunObject<Product>> updateStatus(int id)
        {
            Product checkProduct = null;
            try
            {
                checkProduct = await DBContext.Products.Include(x => x.ProductType).SingleOrDefaultAsync(x => x.ProductId == id);
                if (checkProduct == null)
                {
                    throw new Exception("sản phẩm cần lấy không tồn tại");
                }
            }
            catch (Exception ex)
            {
                return new RetunObject<Product>()
                {
                    data = null,
                    mess = ex.Message,
                    statusCode = 401
                };
            }
            checkProduct.Status = checkProduct.Status == 1 ? 0 : 1;
            checkProduct.UpdatedAt = DateTime.Now;
            DBContext.Products.Update(checkProduct);
            DBContext.SaveChanges();
            return new RetunObject<Product>()
            {
                data = checkProduct,
                mess = "cập nhật trạng thái thành công",
                statusCode = 200
            };
            throw new NotImplementedException();
        }
    }
}
