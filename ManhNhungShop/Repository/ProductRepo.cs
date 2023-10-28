using Amazon.Util.Internal.PlatformServices;
using Firebase.Storage;
using ManhNhungShop.DataContext;
using ManhNhungShop.DataReturn;
using ManhNhungShop.Interfaces;
using ManhNhungShop.Models;
using ManhNhungShop.Services;
using ManhNhungShop_Product_Service.DataReturn;
using ManhNhungShop_Product_Service.Models;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Hosting;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Firebase.Auth.Providers;
using System.Runtime.CompilerServices;
using Firebase.Auth;
using Amazon.S3.Model;
using System.Linq.Expressions;
using Firebase.Auth.Repository;
using Google.Cloud.Storage.V1;
using Google.Apis.Auth.OAuth2;
using Grpc.Auth;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using System.Security.AccessControl;
using System.ComponentModel;
using Google.Type;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ManhNhungShop.Repository
{
    public class ProductRepo : IProduct
    {
        private readonly DbShopContext _dbShopContext;

        private readonly ICachingServer _cachingServer;


        private readonly Microsoft.Extensions.Hosting.IHostingEnvironment _env;


        private static string apiKey = "AIzaSyCInZMWu8KLyTbH54GhFciTNDQRdfY7DO8";
        private static string Bucket = "manhnhungmart.appspot.com";
        private static string AuthEmail = "ndt13102003@gmail.com";
        private static string AuthPassword = "leluuly1306@";
        private static string authDomain =  "manhnhungmart.firebaseapp.com";
        private static string projectId = "manhnhungmart";

        public ProductRepo(DbShopContext dbShopContext, ICachingServer cachingServer, Microsoft.Extensions.Hosting.IHostingEnvironment env) 
        {
            _dbShopContext = dbShopContext;
            _cachingServer = cachingServer;
            _env = env;
        }
        //create product
        public async Task<ProductCreateRes> CreateProduct(Products productCre, FileUpload files)
        {
            Products product = new Products();
            product.ProductDescrip = productCre.ProductDescrip;
            product.ProductName = productCre.ProductName;
            product.ProductType = productCre.ProductType;
            product.ProductQuanlity = productCre.ProductQuanlity;
            product.ProductImage = productCre.ProductImage; 
            product.ProductCreateAt = System.DateTime.UtcNow;
            product.ProductPrice = productCre.ProductPrice;
            product.deleted = 0;
            product.ProductUpdateAt = productCre.ProductUpdateAt;
            ProductCreateRes productExist = new ProductCreateRes();
            var filename = await Uploadfile(files);
            product.ProductImage = filename;
            if (product.ProductImage != null)
            {   
                _dbShopContext.Products.Add(product);
                _dbShopContext.SaveChangesAsync();
                // add to redis
                var expriseTime = System.DateTime.Now.AddMinutes(30);

                //_cachingServer.SetData<Products>($"products/{product.ProductId}", product, expriseTime);
                productExist.isSuccess = true;
                productExist.message = "Product was added Successfully";
                productExist.data = product;
                productExist.statuscode = 200;
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
                var expriseTime = System.DateTime.Now.AddMinutes(30);
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
            var expirationTime = System.DateTime.Now.AddMinutes(30);
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
                var epriseTime = System.DateTime.Now.AddMinutes(30);
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
                    var expiretime = System.DateTime.Now.AddMinutes(30);
                    _cachingServer.SetData<ICollection<Products>>($"product?type={type}", productsType, expiretime);
                    return productsType; 
                    
                }
            } else
            {
                return listProduct;
            }
        }
        // Get product by typoe
        public async Task<List<Products>> GetProductByType(string typeProduct)
        {
            var dataching = _cachingServer.GetData<List<Products>>($"product?type={typeProduct}");
            if(dataching != null)
            {
                //refresh data redis
                _cachingServer.RefreshData($"product?type={typeProduct}");
                return dataching;
            } else
            {
                var dataProduct = _dbShopContext.Products.Where(p => p.ProductType == typeProduct).ToList();
                var expiration = System.DateTime.Now.AddMinutes(30);
                _cachingServer.SetData<List<Products>>($"product?type={typeProduct}", dataProduct, expiration);
                return dataProduct;  
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
                    var exppireTime = System.DateTime.Now.AddMinutes(30);
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
                var expriseTime = System.DateTime.Now.AddMinutes(30);
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
            //if(ProductExist(productId))
            //{
            //    var product = _dbShopContext.Products.FirstOrDefault(p => p.ProductId == productId);
            //    var productDetail = _dbShopContext.ProductsDetails.FirstOrDefault(p => p.ProductId == productId);
            //    ProductDeleSofts productDele = new ProductDeleSofts() {
            //        ProductId = productId,
            //        ProductName = product.ProductName,
            //        ProductDescrip = product.ProductDescrip,
            //        ProductPrice = product.ProductPrice,
            //        ProductQuanlity = product.ProductQuanlity,
            //        ProductType = product.ProductType,
            //    };
            //   _dbShopContext.ProductDeleSofts.Add(productDele);
            //   _dbShopContext.Products.Remove(product);
            //   _dbShopContext.ProductsDetails.Remove(productDetail);
            //    //delete in redis
            //    _cachingServer.RemoveData($"product?id={productId}");
            //    _dbShopContext.SaveChangesAsync();
            //    return true;
            //}
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

        public async Task<bool> SaleProduct(System.DateTime dateSale, SaleOff saleoff)
        {
            if(System.DateTime.Now.Date == dateSale)
            {
                //sale off theo tung mat hang
                if(System.DateTime.Now.Date == dateSale.Date)
                {
                    foreach (var typeProduct in saleoff.TypeProduct)
                    {
                        var productSaleOffs =  await GetProductByType(typeProduct);
                        if(productSaleOffs != null)
                        {
                            for(int i = 0; i < productSaleOffs.Count; i++)
                            {
                                foreach (var product in productSaleOffs)
                                {
                                    UpdatePriceSaleOff(product, saleoff.SaleOffPercent);
                                }
                            }
                        }
                    }
                }
            }
            throw new NotImplementedException();
        }


        public async Task SaleOffBuying()
        {

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

        //Update Price product when has discount
        private bool UpdatePriceSaleOff(Products product, int percent)
        { 
            try
            {
                product.ProductPrice = product.ProductPrice * (percent / 100);
                return true;
            } catch(Exception ex)
            {
                return false;
            }
        }


        //upload img data product to firebase cloud
        public async Task<string> UploadImageToFirebase(string FileName)
        {
            string folderName = "product_image";
            string path = Path.Combine(_env.ContentRootPath, $"Images\\{folderName}");
            string filePath = Path.Combine(path, FileName);
            var objectname = $"product_img/{FileName}";
            var credentialPath = Path.Combine(_env.ContentRootPath, "account.json");
            var credential = GoogleCredential.FromFile(credentialPath)
            .CreateScoped("https://www.googleapis.com/auth/cloud-platform");
            var storageClient = await StorageClient.CreateAsync(credential);

            var uploadOptions = new UploadObjectOptions
            {
                PredefinedAcl = PredefinedObjectAcl.PublicRead
            };
            using(FileStream fs = new FileStream(filePath, FileMode.Open))
            {
                var task = await storageClient.UploadObjectAsync(Bucket, objectname, null, fs, uploadOptions);
                try
                {
                    return task.MediaLink;
                }catch
                {
                    return null;
                }
            }
        }
        //Uplaod file to firebase

        public async Task<string> Uploadfile(FileUpload fileUpload)
        {
            if(fileUpload.files.Length > 0)
            {
                string fileName = $"{Path.GetFileNameWithoutExtension(fileUpload.fileName)}_{System.DateTime.Now.ToString("dd-mm-yyyy-h-mm-tt")}{Path.GetExtension(fileUpload.files.FileName)}";

                string path = $"Images\\product_image";
                string filePath = Path.Combine(path, fileName);
                using (FileStream fs = new FileStream(Path.Combine(_env.ContentRootPath, filePath), FileMode.Create)) {
                    fileUpload.files.CopyToAsync(fs);
                    fs.FlushAsync();
                }
                try
                {
                    string downloadUrl = await UploadImageToFirebase(fileName);
           
                    if(downloadUrl != null)
                    {
                        File.Delete(Path.Combine(_env.ContentRootPath, filePath));
                    }
                    return downloadUrl;
                } catch
                {
                    return null;
                }
            }
            return null;
        }
        // get product categories 
        public async Task<List<Categories>> getallCategories()
        {
            try
            {
                List<Categories> listCate = await _dbShopContext.Categories.ToListAsync();
                return listCate;
            } catch (Exception ex)
            {
                return null;
            }
        }
        

    }

}