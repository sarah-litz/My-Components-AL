using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AL.WebAPI.ViewModels;
using AL.WebAPI.Services;
using AL.WebAPI.Models;
using AL.WebAPI.ApplicationState;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Hosting;

namespace AL.WebAPI.Controllers
{
    [Route("api/Product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ARContext _context;
        protected readonly AppSettings _appsettings;
        protected readonly IProductService _productService;
        protected readonly IWebHostEnvironment _hostEnvironment;

        public ProductController(ARContext context, IProductService productService, IOptions<AppSettings> settings, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _appsettings = settings.Value;
            _productService = productService;
            _hostEnvironment = hostingEnvironment;
        }

        [HttpGet]
        [Route("ListProducts")]
        public ActionResult<IEnumerable<ProductViewModel>> ListProducts()
        {
            return _productService.ListProducts();
        }

        //[HttpGet]
        //[Route("ListProductsWithSearch")]
        //public ActionResult<IEnumerable<ProductViewModel>> ListProducts(ProductSearchViewModel? productSearchViewModel)
        //{
        //    return _productService.ListProducts(productSearchViewModel);
        //}

        [HttpPost]
        [Route("ListProductsWithSearch")]
        public ActionResult<IEnumerable<ProductViewModel>> ListProducts(ProductSearchViewModel? productSearchViewModel)
        {
            return _productService.ListProducts(productSearchViewModel);
        }

        [HttpGet]
        [Route("ListProductsAndColors")]
        public ActionResult<IEnumerable<ProductAndColorListViewModel>> ListProductsAndColors()
        {
            return _productService.ListProductsAndColors();
        }

        [HttpGet]
        [Route("GetProduct/{ProductId}")]
        public ActionResult<ProductViewModel> GetProduct(int ProductId)
        {
            return _productService.GetProduct(ProductId);
        }

        [HttpGet]
        [Route("GetProductColors/{ProductId}")]
        public ActionResult<List<ColorViewModel>> GetProductColors(int ProductId)
        {
            return _productService.GetProductColors(ProductId);
        }

        //[HttpGet]
        //[Route("GetTableValues")]
        //public ActionResult<LookupTablesViewModel> GetTableValues()
        //{
        //    return _productService.GetTableValues();
        //}

        [HttpPut]
        [Route("Product")]
        public ActionResult<ProductViewModel> Product(ProductViewModel productViewModel)
        {
            if (productViewModel.ProductId == 0)
            {
                var product = _productService.AddProduct(new Product()
                {
                    PrivateDescription = productViewModel.PrivateDescription,
                    Inactive = productViewModel.Inactive, 
                    ManufacturerId = productViewModel.ManufacturerId,
                    Length = productViewModel.Length,
                    Height = productViewModel.Height,
                    CartonSize = productViewModel.CartonSize,
                    ProductCategoryId = productViewModel.ProductCategoryId,
                    ProductDescription = productViewModel.ProductDescription,
                    ProductSKU = productViewModel.ProductSKU,
                    RecordTypeId = 1,
                    UnitCost = productViewModel.UnitCost,
                    UnitOfMeasureId_Base = productViewModel.UnitOfMeasureId_Base,
                    Width = productViewModel.Width,
                    CostUpdateDate = productViewModel.CostUpdateDate                    
                }
                );
                productViewModel.ProductId = product.ProductId;


                //
                // Add Product Subtypes
                //
                if (productViewModel.Subtypes != null)
                {
                    foreach (ProductToProductSubtypeViewModel subtype in productViewModel.Subtypes)
                    {
                        var productSubtype = _productService.AddProductSubtype(
                            new ProductToProductSubtype()
                            {
                                ProductId = productViewModel.ProductId,
                                ProdSubTypeId = subtype.ProdSubTypeId
                            }
                        );
                    }
                }

            }
            else
            {
                var product = _productService.UpdateProduct(new Product()
                {
                    PrivateDescription = productViewModel.PrivateDescription,
                    Inactive = productViewModel.Inactive, 
                    ManufacturerId = productViewModel.ManufacturerId,
                    Length = productViewModel.Length,
                    Height = productViewModel.Height,
                    CartonSize = productViewModel.CartonSize,
                    ProductCategoryId = productViewModel.ProductCategoryId,
                    ProductDescription = productViewModel.ProductDescription,
                    ProductSKU = productViewModel.ProductSKU,
                    RecordTypeId = 1,
                    UnitCost = productViewModel.UnitCost,
                    UnitOfMeasureId_Base = productViewModel.UnitOfMeasureId_Base,
                    Width = productViewModel.Width,
                    ProductId = productViewModel.ProductId,
                    CostUpdateDate = productViewModel.CostUpdateDate
                }
                );

                //
                // Delete existing and Add Product Subtypes
                //

                _productService.DeleteProductSubtypeByProductId(product.ProductId);

                // todo  - rewrite this to pass a collection of producttoproductsubtype objects to a single call of the service to 
                //          avoid multiple calls to .savechanges
                foreach (ProductToProductSubtypeViewModel subtype in productViewModel.Subtypes)
                {
                    var productSubtype = _productService.AddProductSubtype(
                        new ProductToProductSubtype()
                        {
                            ProductId = productViewModel.ProductId,
                            ProdSubTypeId = subtype.ProdSubTypeId
                        }
                    );
                }


            }
            return new OkObjectResult(productViewModel);
        }
 
        [HttpPost]
        [Route("Color")]
        public ActionResult<ColorViewModel> Color(ColorViewModel colorViewModel)
        {
            var color = _productService.AddColor(new Color()
            {
                ProductId = colorViewModel.ProductId,
                ColorCode = colorViewModel.ColorCode,
                ColorDescription = colorViewModel.ColorDescription,
                isDropped = colorViewModel.isDropped
            }
            );
            colorViewModel.ColorId = color.ColorId;
            return new OkObjectResult(colorViewModel);
        }

        [HttpPost]
        [Route("UploadVendorInputSheet/{manufacturerId}")]
        public ActionResult UploadVendorInputSheet(int manufacturerId)
        {
            ActionResult result = new OkResult();
            var httpRequest = HttpContext.Request;
            if (httpRequest.Form.Files.Count > 0)
            {
                //
                // Save file entry
                //
                FileUploads ful = new FileUploads();
                ful.FileUploadTypeID = (int)Enums.FileUploadType.XactimateFile;
                ful.FK_ID = manufacturerId;
                ful.FileID = Guid.NewGuid();
                ful.CreatedDate = DateTime.Now;
                ful.CreatedByUserID = 0;
                ful.FileName = System.IO.Path.GetFileNameWithoutExtension(httpRequest.Form.Files[0].FileName) + "_" + ful.FileID.ToString() + System.IO.Path.GetExtension(httpRequest.Form.Files[0].FileName);
                ful.UploadedDate = DateTime.Now;
                _context.FileUploads.Add(ful);
                _context.SaveChanges();
                try
                {
                    //_hostEnvironment.WebRootPath
                    System.IO.FileStream fs = new System.IO.FileStream(_hostEnvironment.WebRootPath + Path.DirectorySeparatorChar +  "fileuploads" + Path.DirectorySeparatorChar + ful.FileName.ToString(), System.IO.FileMode.Create);
                    //System.IO.FileStream fs = new System.IO.FileStream(_appsettings.FileUploadsPath + "\\" + ful.FileName.ToString(), System.IO.FileMode.Create);
                    //log.Debug("LeanDesignService: SaveCustomSelection: Copy file to  " + settings.FileUploadsPath + "\\" + ful.FileName.ToString());
                    httpRequest.Form.Files[0].CopyTo(fs);
                    fs.Close();
                }
                catch (Exception exp)
                {
                    //log.Error("LeanDesignService: SaveCustomSelection: Error Copying file. " + exp.Message);
                }



                //
                // Save product upload file entry (table with more specifics about the same file) 
                //
                ProductUploadFiles file = new ProductUploadFiles()
                {
                    ManufacturerId = manufacturerId,
                    FileID = ful.FileID, // foreign key pointing to file entry in the FileUploads table
                    // todo: add in uploadedby
                    UploadedDate = ful.UploadedDate
                };
                _context.ProductUploadFiles.Add(file);
                _context.SaveChanges();


                //
                // Service Layer Call --> service layer opens the file and reads thru and grabs/parses data. Then for each row in spreadsheet will insert a record into the database
                //
                _productService.UploadVendorInputSheet(_hostEnvironment.WebRootPath + Path.DirectorySeparatorChar + "fileuploads" + Path.DirectorySeparatorChar + ful.FileName.ToString(), file.ProductFileUploadId);



            }
            {
                result = new BadRequestResult();
            }
            return result;
        }


        [HttpGet]
        [Route("ListUploadedProductFiles")]
        public ActionResult<IEnumerable<ProductUploadFilesViewModel>> ListUploadedProductFiles()
        {
            // service method call for getting data on the files that have been uploaded so it can be displayed on the UI 
            return _productService.ListUploadedProductFiles(); 
        }


        [HttpGet]
        [Route("ViewUploadedProductFileData/{ProductFileUploadId}")]
        public ActionResult<List<ViewUploadedProductFileDataViewModel>> ViewUploadedProductFileData(int ProductFileUploadId)
        {
            return _productService.ViewUploadedProductFileData(ProductFileUploadId); 
        }


    }
}