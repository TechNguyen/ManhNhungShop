using ManhNhungShop.DataContext;
using ManhNhungShop.DataReturn;
using ManhNhungShop.Interfaces;
using ManhNhungShop.Models;
using ManhNhungShop.Services;
using ManhNhungShop_Product_Service.DataReturn;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ManhNhungShop.Repository
{
    public class ProductRepo : IProduct
    {
        private readonly DbShopContext _dbShopContext;

        private readonly ICachingServer _cachingServer;

        public ProductRepo(DbShopContext dbShopContext, ICachingServer cachingServer )
        {
            _dbShopContext = dbShopContext;
            _cachingServer = cachingServer;
        }
        //create product
        public async Task<ProductCreateRes> CreateProduct(Products product)
        {
            ProductCreateRes productExist = new ProductCreateRes();
            if (ProductExist(product.ProductId))
            {
                productExist.isSuccess = false;
                productExist.Message = "Products has been in Shop";
                productExist.data = product;
            }
            else
            {
                _dbShopContext.Products.Add(product);
                _dbShopContext.SaveChangesAsync();
                // add to redis
                var expriseTime = DateTime.Now.AddMinutes(30);
                _cachingServer.SetData<Products>($"products/{product.ProductId}", product, expriseTime);
                productExist.isSuccess = true;
                productExist.Message = "Product was added Successfully";
                productExist.data = product;
            }
            return productExist;
        }
        // get all product
        public async Task<ProductRescs> GetAllProduct(int page)
        {
            var pageSizes = 10f;
            var pageCounts = Math.Ceiling(_dbShopContext.Products.Count() / pageSizes);
            if (page <= 0)
            {
                page = 1;
            }
            ProductRescs productRescs = new ProductRescs();
            var cachedata = _cachingServer.GetData<ICollection<Products>>($"products?page={page}");
            if (cachedata != null && cachedata.Count() > 0)
            {
                //Refresh time redis
                var isUpdate = await _cachingServer.RefreshData($"products?page={page}");
                if (isUpdate)
                {
                    productRescs.pageSize = (int)pageSizes;
                    productRescs.pageIndex = page;
                    productRescs.pageCount = (int)pageCounts;
                    productRescs.isSuccess = true;
                    productRescs.data = cachedata;
                    productRescs.Message = "Get Products Successfully!";
                }
                return productRescs;
            } else
            {
                var products = _dbShopContext.Products.Skip((page - 1) * (int)pageSizes).Take((int)pageSizes).ToList();
                var expriseTime = DateTime.Now.AddMinutes(30);
                _cachingServer.SetData<ICollection<Products>>($"products?page={page}", products, expriseTime);
                //set data get  response
                productRescs.pageSize = (int)pageSizes;
                productRescs.pageIndex = page;
                productRescs.pageCount = (int)pageCounts;
                productRescs.isSuccess = true;
                productRescs.data = products;
                productRescs.Message = "Get Products Successfully!";

                return productRescs;

            }
        }
        //// get product by id
        public async Task<ProductCreateRes> GetProductById(int productId)
        {
            ProductCreateRes productRescs = new ProductCreateRes();
            var data = _cachingServer.GetData<Products>($"product?id={productId}");
            if (data != null)
            {
                //refresh datacaching
                var isRefresh = await _cachingServer.RefreshData($"product?id={productId}");
                if (isRefresh)
                {
                    productRescs.isSuccess = true;
                    productRescs.data = data;
                }
                return productRescs;
            }
            // not has data
            var product = _dbShopContext.Products.Where(p => p.ProductId == productId).FirstOrDefault();
            if (product != null)
            {
                productRescs.isSuccess = true;
                productRescs.data = product;
            }
            var expirationTime = DateTime.Now.AddMinutes(30);
            _cachingServer.SetData<Products>($"product?id={productId}", product, expirationTime);
            return productRescs;
        }
        //get product detail by id
        public async Task<ProductDetailRes> GetProductDetail(int productId)
        {
            ProductDetailRes productDetail = new ProductDetailRes();
            var cachingdata =  _cachingServer.GetData<ProductsDetails>($"productDetail?Id={productId}");
            if (cachingdata != null)

            {
                //refresh data
                _cachingServer.RefreshData($"productDetail?Id={productId}");
                productDetail.isSuccess = true;
                productDetail.returnCode = 200;
                productDetail.Message = "Get Detail Product successfully!";
                productDetail.data = cachingdata;
            }
            else
            {
                var productData = _dbShopContext.ProductsDetails.Where(p => p.ProductId == productId).FirstOrDefault();
                var epriseTime = DateTime.Now.AddMinutes(30);
                _cachingServer.SetData<ProductsDetails>($"productDetail?Id={productId}", productData, epriseTime);

                if(productData != null)
                {
                    productDetail.isSuccess = true;
                    productDetail.returnCode = 200;
                    productDetail.Message = "Get Detail Prodcut successffully!";
                    productDetail.data = productData;
                }
                productDetail.isSuccess = false;
                productDetail.Message = "Not found Product";
            }
            return productDetail;
        }
        //sort product by type
        public ICollection<Products> SortProductsByType(string type)
        {
            List<Products> listProduct = new List<Products>();
            // sort by type product
            if(type != null)
            {
                var cachingdata = _cachingServer.GetData<ICollection<Products>>($"products?type={type}");
                if (cachingdata != null && cachingdata.Count() > 0)
                {
                    return cachingdata;
                }
                else
                {
                    var productsType = _dbShopContext.Products.Where(p => p.ProductType == type).ToList();
                    var expiretime = DateTime.Now.AddMinutes(30);
                    _cachingServer.SetData<ICollection<Products>>($"product?type={type}", productsType, expiretime);
                    return productsType; 
                    
                }
            } else
            {
                return listProduct;
            }
        }
        //sort by money
        public ProductMainRes SortByMoney(int typesort)
        {
            ProductMainRes productMres = new ProductMainRes();

            // desc = 0
            if(typesort == 0)
            {
                var cachingData = _cachingServer.GetData<ICollection<Products>>($"products?sortby={typesort}");
                if(cachingData != null && cachingData.Count() > 0)
                {
                    _cachingServer.RefreshData($"products?sortby={typesort}");
                    productMres.returncode = 200;
                    productMres.isSuccess = true;
                    productMres.data = cachingData;
                    return productMres;
                } else
                {
                    var dataProduct = _dbShopContext.Products.OrderBy(p => p.ProductPrice).ToList();
                    var exppireTime = DateTime.Now.AddMinutes(30);
                    _cachingServer.SetData<ICollection<Products>>($"product?sortby={typesort}", dataProduct, exppireTime);

                    productMres.isSuccess = true;
                    productMres.data = dataProduct;
                    productMres.returncode = 200;
                    return productMres;

                }
            }
            return productMres; 
        }
        
        

        //check exit product
        public bool ProductExist(int ProductId)
        {
            return _dbShopContext.Products.Any(p => p.ProductId == ProductId);
        }


        //Update information product
        public async Task<ProductUpdateRes> UpdateProduct(ProductsDetails products, int productId)
        {
            //check product has been
            ProductUpdateRes productDetailRes = new ProductUpdateRes();
            if(ProductExist(productId))
            {
                // Update 
                var updateProduct =  _dbShopContext.ProductsDetails.Where(p => p.ProductId == productId).FirstOrDefault();
                //udpdate redis
                var expriseTime = DateTime.Now.AddMinutes(30);
                _cachingServer.SetData<ProductsDetails>($"productsDetail?id={updateProduct.ProductId}", updateProduct, expriseTime);
                _dbShopContext.Entry(updateProduct).CurrentValues.SetValues(products);    

                _dbShopContext.SaveChangesAsync();

                productDetailRes.isSuccess = true;
                productDetailRes.returncode = 200 ;
                productDetailRes.Mesage = "Update Product Successffully!";

            } else
            {
                productDetailRes.isSuccess = false;
                productDetailRes.Mesage = "Product Not has been!";
                productDetailRes.returncode = 401;
            }
            return productDetailRes;
        }
        // deletesoft a product
        public async Task<bool> DeleteSoftProduct(int productId)
        {
            if(ProductExist(productId))
            {
                var product = _dbShopContext.Products.FirstOrDefault(p => p.ProductId == productId);
                var productDetail = _dbShopContext.ProductsDetails.FirstOrDefault(p => p.ProductId == productId);
                ProductDeleSofts productDele = new ProductDeleSofts() {
                    ProductId = productId,
                    ProductName = product.ProductName,
                    ProductDescrip = product.ProductDescrip,
                    ProductPrice = product.ProductPrice,
                    ProductQuanlity = product.ProductQuanlity,
                    ProductType = product.ProductType,
                };
               _dbShopContext.ProductDeleSofts.Add(productDele);
               _dbShopContext.Products.Remove(product);
               _dbShopContext.ProductsDetails.Remove(productDetail);
                //delete in redis
                _cachingServer.RemoveData($"product?id={productId}");
                _dbShopContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        // restore a product
        public async Task<bool> RestoreProduct(int productId)
        {
            if(ProductDeleteSoftExits(productId))
            {
                var product = _dbShopContext.ProductDeleSofts.Where(p => p.ProductId == productId).FirstOrDefault();
                Products restore = new Products()
                {
                    ProductId = productId,
                    ProductName = product.ProductName,
                    ProductDescrip = product.ProductDescrip,
                    ProductPrice = product.ProductPrice,
                    ProductQuanlity = product.ProductQuanlity,
                    ProductType = product.ProductType,
                };

                _dbShopContext.Products.Add(restore);
                _dbShopContext.ProductDeleSofts.Remove(product);
                _dbShopContext.SaveChangesAsync();
                return true;
            }
            return false;
        }
        //check product delete
        public bool ProductDeleteSoftExits(int productId)
        {
            return _dbShopContext.ProductDeleSofts.Any(p => p.ProductId == productId);
        }

        public async Task<bool> DeleteProduct(int productId)
        {
            if(ProductDeleteSoftExits(productId))
            {
                var product = _dbShopContext.ProductDeleSofts.FirstOrDefault(p => p.ProductId == productId);
                _dbShopContext.ProductDeleSofts.Remove(product);
                _dbShopContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        //public Task<List<Products>> SortProductByTime(string typesort)
        //{
        //    if(typesort == "day")
        //    {
        //        var listProduct = _dbShopContext.Products.Where(p => p.ProductCreateAt == typesort).ToList();
        //        return listProduct;
        //    }
        //    else if(typesort == "month") {
        //        var listProduct = _dbShopContext.Products.Where();
        //    }
        //}
    }
}