using Grpc.Core;
using ManhNhungShop.DataContext;
using ManhNhungShop.DataReturn;
using ManhNhungShop.Interfaces;
using ManhNhungShop.Models;
using ManhNhungShop.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Hosting;
using ManhNhungShop_Product_Service.Models;
using ManhNhungShop_Product_Service.DataReturn;
using Microsoft.AspNetCore.Authorization;

namespace ManhNhungShop.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly DbShopContext _dbShopContext;
        private readonly ILogger _logger;
        private readonly IProduct _product;
        public ProductController(ILogger<DbShopContext> logger, IProduct product, DbShopContext dbShopContext)
        {
            _product = product;
            _logger = logger;
            _dbShopContext = dbShopContext;
        }
        //get all products
        [HttpGet("page/{page}")]
        public async Task<IActionResult> GetProducts(int page)
        {
            try
            {
                var productsAll = await _product.GetAllProduct(page);
                return Ok(productsAll);

            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

        }
        //get product by id
        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetProductByProductId(int productId)
        {
            try
            {
                var product = await _product.GetProductById(productId);
                return Ok(product);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        //get detail product
        [HttpGet("detail")]
        public async Task<IActionResult> GetProductDetailByProductId([FromQuery]int productId)
        {
             try
            {
                var productDetail = await _product.GetProductDetail(productId);
                return Ok(productDetail);
            } catch(Exception ex)
            {
                return NotFound(ex.Message); 
            }
        }
        // get Product sort by money from moeny to money
        [HttpPost("sortbymoney/{type}")]
        public async Task<IActionResult> GetProductSortByMoney(int type)
        {
            try
            {
                var productSortbyMoney  = _product.SortByMoney(type);
                return Ok(productSortbyMoney);
            } catch (Exception ex)
            {
                return NotFound(ex.Message);

            }
        }
        // get Product by type
        [HttpPost("sort/{type}")]
        public async Task<IActionResult> GetProductSortByType(string type)
        {
            try
            {
                var productSortByType = _product.SortProductsByType(type);
                return Ok(productSortByType);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        //create new product
        [HttpPost("create")]
        public async Task<IActionResult> CraeteNewProduct([FromForm] Products pro, [FromForm] FileUpload files)
        {
            try
            {
                var productRes = await _product.CreateProduct(pro,files);
                return Ok(productRes);
            } catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //upddate productbyId
        [HttpPut("update/{productId}")]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductsDetails products, int productId)
        {
            try
            {
                var data = await _product.UpdateProduct(products, productId);
                return Ok(data);
            } catch(Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        //delete Product
        [HttpDelete("deleletesoft/{productId}")]
        public async Task<IActionResult> DeleteSoftProductById(int productId)
        {
            try
            {
                var isSucces = await _product.DeleteSoftProduct(productId);
                return Ok(isSucces);
            } catch(Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        //delete 
        [HttpDelete("delete/{productId}")]
        public async Task<IActionResult> DeleteProductById(int productId)
        {
            try
            {
                var data = await _product.DeleteProduct(productId);
                return Ok(data);
            } catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        //Restore Product
        [HttpPost("restore/{productId}")]
        public async Task<IActionResult> RestoreProductById (int productId)
        {
            try
            {
                var isSucces = await _product.RestoreProduct(productId);
                return Ok(isSucces);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("uploadfiletofolder")]
        public async Task<IActionResult> UploadFileToFolder([FromForm] FileUpload files)
        {
            try
            {
                var result = await _product.Uploadfile(files);

                UploadRes uploadres = new UploadRes();
                if(result != null)
                {
                    uploadres.statusCode = 200;
                    uploadres.statusMessage = result;
                };
                return Ok(uploadres);
            } catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        //get category shop
        [HttpGet("/products")]
        public async Task<IActionResult> getCategory()
        {
            try
            {
                var listrs = await _product.getallCategories();
                ProceduCateRes proceduCateRes = new ProceduCateRes
                {
                    isSuccess = true,
                    message = "Get categories successfully!",
                    listCate = listrs.ToList()
                };
                return Ok(proceduCateRes);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        //[Authorize(Roles = "Admin")]
        //[HttpPost("createCate")]

    }
}
