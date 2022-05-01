using AL.WebAPI.Models;
using AL.WebAPI.ViewModels;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace AL.WebAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly Models.ARContext _ARContext;
        public ProductService(Models.ARContext ARContext)
        {
            _ARContext = ARContext;

        }
        public List<ProductViewModel> ListProducts()
        {
            var products =
            from record in _ARContext.vwProduct
            orderby record.ProductDescription_Extended
            orderby record.ProductCategory_Description
            where record.RecordTypeId == 1
            select new ProductViewModel
            {
                ProductDescription = record.ProductDescription,
                Inactive = record.Inactive, 
                CartonSize = record.CartonSize,
                Height = record.Height,
                Length = record.Length,
                ManufacturerId = record.ManufacturerId,
                PrivateDescription = record.PrivateDescription,
                ProductCategoryId = record.ProductCategoryId,
                ProductId = record.ProductId,
                ProductSKU = record.ProductSKU,
                UnitCost = record.UnitCost,
                UnitOfMeasureId_Base = record.UnitOfMeasureId_Base,
                UnitOfMeasureId_Price = record.UnitOfMeasureId_Price,
                VendorId = record.VendorId,
                Width = record.Width,
                Manufacturer_Description = record.Manufacturer_Description,
                ProductCategory_Description = record.ProductCategory_Description,
                UnitOfMeasure_Description = record.UnitOfMeasure_Description,
                SizeDescription = record.SizeDescription,
                ProductCategoryId_Parent = record.ProductCategoryId_Parent,
                ProductCategoryId_Parent_Description = record.ProductCategoryId_Parent_Description,
                ProductDescription_Extended = record.ProductDescription_Extended,
                RecordTypeId = record.RecordTypeId,
                RecordType_Description = record.RecordType_Description
            };
            return products.ToList();
        }
        public List<ColorViewModel> ListColorsByProductId(int productID)
        {
            var colors =
            from record in _ARContext.Color
            where record.ProductId == productID
            select new ColorViewModel
            {
                CartonSize = record.CartonSize,
                ColorCode = record.ColorCode,
                ColorDescription = record.ColorDescription,
                ColorId = record.ColorId,
                isDropped = record.isDropped,
                ProductId = record.ProductId,
                UnitCost = record.UnitCost
            };
            return colors.ToList();
        }
        public List<ProductAndColorListViewModel> ListProductsAndColors()
        {
            var products =
            from product in _ARContext.Product
            join color in _ARContext.Color on product.ProductId equals color.ProductId into gj
            from subpet in gj.DefaultIfEmpty()
            join productCategory in _ARContext.ProductCategory on product.ProductCategoryId equals productCategory.ProductCategoryId
            join manufacturer in _ARContext.Manufacturer on product.ManufacturerId equals manufacturer.ManufacturerId
            where product.RecordTypeId == 1
            select new ProductAndColorListViewModel
            {
                ProductDescription = product.ProductDescription,
                CartonSize = product.CartonSize,
                Height = product.Height,
                Length = product.Length,
                ManufacturerId = product.ManufacturerId,
                PrivateDescription = product.PrivateDescription,
                ProductCategoryId = product.ProductCategoryId,
                ProductId = product.ProductId,
                ProductSKU = product.ProductSKU,
                UnitCost = product.UnitCost,
                UnitOfMeasureId_Base = product.UnitOfMeasureId_Base,
                UnitOfMeasureId_Price = product.UnitOfMeasureId_Price,
                VendorId = product.VendorId,
                Width = product.Width,
                ColorCode = subpet.ColorCode,
                ColorDescription = subpet.ColorDescription,
                ColorId = subpet.ColorId,
                ProductCategoryDescription = productCategory.CategoryDescription,
                ManufacturerDescription = manufacturer.Name
            };
            return products.OrderBy(x => x.ProductCategoryDescription).ThenBy(x => x.ManufacturerDescription).ThenBy(x => x.PrivateDescription).ThenBy(x => x.ColorDescription).ToList();
        }
        public List<ProductViewModel> ListProducts(ProductSearchViewModel productSearchViewModel)
        {
            var products =
            from record in _ARContext.vwProduct
            orderby record.ProductDescription_Extended
            orderby record.ProductCategory_Description
            where record.RecordTypeId == 1
            select new ProductViewModel
            {
                ProductDescription = record.ProductDescription,
                CartonSize = record.CartonSize,
                Height = record.Height,
                Length = record.Length,
                ManufacturerId = record.ManufacturerId,
                PrivateDescription = record.PrivateDescription,
                ProductCategoryId = record.ProductCategoryId,
                ProductId = record.ProductId,
                ProductSKU = record.ProductSKU,
                UnitCost = record.UnitCost,
                UnitOfMeasureId_Base = record.UnitOfMeasureId_Base,
                UnitOfMeasureId_Price = record.UnitOfMeasureId_Price,
                VendorId = record.VendorId,
                Width = record.Width,
                Manufacturer_Description = record.Manufacturer_Description,
                ProductCategory_Description = record.ProductCategory_Description,
                UnitOfMeasure_Description = record.UnitOfMeasure_Description,
                SizeDescription = record.SizeDescription,
                ProductCategoryId_Parent = record.ProductCategoryId_Parent,
                ProductCategoryId_Parent_Description = record.ProductCategoryId_Parent_Description,
                ProductDescription_Extended = record.ProductDescription_Extended,
                RecordTypeId = record.RecordTypeId,
                RecordType_Description = record.RecordType_Description


            };
            
            if ((productSearchViewModel.ProductDescription ?? "") != "")
            {
                products = products.Where(x => x.ProductDescription.Contains(productSearchViewModel.ProductDescription));
            }
            if ((productSearchViewModel.ProductCategoryId ?? 0) != 0)
            {
                products = products.Where(x => x.ProductCategoryId == productSearchViewModel.ProductCategoryId);
            }
            //if (productSearchViewModel.Manufacturer != "")
            //{
            //    products.Where(x => x.Man == productSearchViewModel.ProductCategoryId);
            //}



            return products.ToList();
        }
        public ProductViewModel GetProduct(int ProductId)
        {
            var product =
                from record in _ARContext.vwProduct
                join manuf in _ARContext.Manufacturer on record.ManufacturerId equals manuf.ManufacturerId into gj
                from manufOuterJoin in gj.DefaultIfEmpty()
                join prodCategory in _ARContext.ProductCategory on record.ProductCategoryId equals prodCategory.ProductCategoryId into pc
                from prodCategoryOuterJoin in pc.DefaultIfEmpty()
                join uom in _ARContext.UnitOfMeasure on record.UnitOfMeasureId_Base equals uom.UnitOfMeasureID into um
                from uomOuterJoin in um.DefaultIfEmpty()
                where record.ProductId == ProductId
                select new ProductViewModel
                {
                    Inactive = record.Inactive, 
                    CartonSize = record.CartonSize,
                    Height = record.Height,
                    Length = record.Length,
                    Width = record.Width,
                    ManufacturerId = record.ManufacturerId,
                    PrivateDescription = record.PrivateDescription,
                    ProductCategoryId = record.ProductCategoryId,
                    ProductDescription = record.ProductDescription,
                    ProductId = record.ProductId,
                    ProductSKU = record.ProductSKU,
                    RecordTypeId = record.RecordTypeId,
                    UnitCost = record.UnitCost,
                    UnitOfMeasureId_Base = record.UnitOfMeasureId_Base     ,
                    Manufacturer_Description = record.Manufacturer_Description,
                    ProductCategory_Description = record.ProductCategory_Description,
                    UnitOfMeasure_Description = uomOuterJoin.Description,
                    SizeDescription = record.SizeDescription,
                    ProductCategoryId_Parent = record.ProductCategoryId_Parent,
                    ProductCategoryId_Parent_Description = record.ProductCategoryId_Parent_Description,
                    ProductDescription_Extended = record.ProductDescription_Extended,
                    RecordType_Description = record.RecordType_Description,
                    CostUpdateDate = record.CostUpdateDate
                    ,Subtypes =
                        (
                            from productSubTypes in _ARContext.ProductToProductSubtype
                            join subTypes in _ARContext.ProductSubType on productSubTypes.ProdSubTypeId equals subTypes.ProdSubTypeId
                            where productSubTypes.ProductId == record.ProductId
                            select new ProductToProductSubtypeViewModel
                            {
                                ProductId = record.ProductId,
                                ProdSubTypeId = subTypes.ProdSubTypeId,
                                ProductSubTypeDescription = subTypes.Description
                            }
                        ).ToList()
                };
            
            return product.FirstOrDefault();
        }
        //public LookupTablesViewModel GetTableValues()
        //{
        //    LookupTablesViewModel tables = new LookupTablesViewModel();
            
        //    IEnumerable<ManufacturerViewModel> manufacturers = 
        //        from manufs in _ARContext.Manufacturer
        //        select new ManufacturerViewModel
        //        {
        //            ManufacturerId = manufs.ManufacturerId,
        //            ManufacturerName = manufs.Name
        //        };
        //    tables.Manufacturers = manufacturers.ToList();

        //    IEnumerable<VendorViewModel> vendors =
        //        from vends in _ARContext.Vendor
        //        select new VendorViewModel
        //        {
        //            VendorId = vends.VendorId,
        //            VendorName = vends.Name
        //        };

        //    tables.Vendors = vendors.ToList();

        //    IEnumerable<ProductCategoryViewModel> productCategories =
        //        from productCategs in _ARContext.ProductCategory
        //        select new ProductCategoryViewModel
        //        {
        //            ProductCategoryId = productCategs.ProductCategoryId,
        //            ProductCategoryName = productCategs.CategoryDescription,
        //            isDirectionRequired = productCategs.isDirectionRequired ?? false,
        //            isInstallMethodRequired = productCategs.isInstallMethodRequired ?? false,
        //            isCuttable= productCategs.isCuttable ?? false,
        //            isRoundedToSlabSize = productCategs.isRoundedToSlabSize ?? false,
        //            isAdhesiveMethodRequired = productCategs.isAdhesiveMethodRequired ?? false,
        //            isGroutRequired = productCategs.isGroutRequired ?? false,
        //            isJointSizeRequired = productCategs.isJointSizeRequired ?? false,
        //            isRollGoods = productCategs.isRollGoods ?? false,
        //            isSolidSurface = productCategs.isSolidSurface ?? false,
        //            ParentProductCategoryId = productCategs.ParentProductCategoryId ?? 0
        //        };
        //    tables.ProductCategories = productCategories.ToList();


        //    IEnumerable<UOMViewModel> unitOfMeasures =
        //        from units in _ARContext.UnitOfMeasure
        //        select new UOMViewModel
        //        {
        //            UOMId = units.UnitOfMeasureID,
        //            UOMName = units.Description
        //        };
        //    tables.UnitOfMeasures = unitOfMeasures.ToList();




        //    IEnumerable<JobStatusViewModel> statuses =
        //        from status in _ARContext.JobStatus
        //        select new JobStatusViewModel
        //        {
        //            Description = status.Description,
        //            isContracted = status.isContracted,
        //            isLockedForChanges = status.isLockedForChanges,
        //            JobSatusID = status.JobSatusID
        //        };
        //    tables.JobStatuses = statuses.ToList();



        //    return tables;
        //}
        public List<ColorViewModel> GetProductColors(int ProductId)
        {
            var productColors =
            from record in _ARContext.Color
            where record.ProductId == ProductId
            select new ColorViewModel
            {
                ColorCode = record.ColorCode,
                ColorDescription = record.ColorDescription,
                ColorId = record.ColorId,
                isDropped = record.isDropped,
                ProductId = record.ProductId
            };
            return productColors.ToList();


        }
        public AL.WebAPI.Models.Product UpdateProduct(AL.WebAPI.Models.Product product)
        {
            _ARContext.Product.Update(product);
            _ARContext.SaveChanges();
            return product;
        }
        public AL.WebAPI.Models.Product AddProduct(AL.WebAPI.Models.Product product)
        {
            _ARContext.Product.Add(product);
            _ARContext.SaveChanges();
            return product;
        }
        public AL.WebAPI.Models.ProductToProductSubtype AddProductSubtype(AL.WebAPI.Models.ProductToProductSubtype productSubtype)
        {
            _ARContext.ProductToProductSubtype.Add(productSubtype);
            _ARContext.SaveChanges();
            return productSubtype;
        }
        public AL.WebAPI.Models.Color AddColor(AL.WebAPI.Models.Color color)
        {
            _ARContext.Color.Add(color);
            _ARContext.SaveChanges();
            return color;
        }
        public bool DeleteProductSubtypeByProductId(int ProductId)
        {
            IEnumerable<ProductToProductSubtype> subtypes = _ARContext.ProductToProductSubtype.Where(x => x.ProductId == ProductId).Select(x => x);            
            foreach (ProductToProductSubtype subtype in subtypes)
            {
                _ARContext.ProductToProductSubtype.Remove(subtype);
            }
            _ARContext.SaveChanges();
            return true;            
        }




        public int UploadVendorInputSheet(String fileNameAndPath, int ProductFileUploadId)
        {

            try
            {
                //
                // open and read doc
                //
                using (SpreadsheetDocument doc = SpreadsheetDocument.Open(fileNameAndPath, false))
                {
                    //create the object for workbook part  
                    WorkbookPart workbookPart = doc.WorkbookPart;
                    Sheets thesheetcollection = workbookPart.Workbook.GetFirstChild<Sheets>();
                    StringBuilder excelResult = new StringBuilder();
                    int rowCount = 0;

                    //using for each loop to get the sheet from the sheetcollection
                    int sheetCount = 0;
                    foreach (Sheet thesheet in thesheetcollection) // loops thru sheets in the xcel file 
                    {
                        sheetCount += 1;
                        if (sheetCount == 3)
                        {
                            Worksheet theWorksheet = ((WorksheetPart)workbookPart.GetPartById(thesheet.Id)).Worksheet;

                            SheetData thesheetdata = (SheetData)theWorksheet.GetFirstChild<SheetData>();
                            var stringTable = workbookPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault();
                            foreach (Row thecurrentrow in thesheetdata) 
                            {
                                rowCount++;
                                if (rowCount < 3)
                                    continue; // skips the first two rows that contains the column headings which we don't care about

                                if (thecurrentrow.ChildElements.Count == 0)
                                    continue; // ChildElements are the cells w/in the row, so if there are no cells w/ data we want to skip this row 

                                try
                                {

                                    // file data stuff
                                    ProductUploadData data = new ProductUploadData(); // each new row gets a new entry in ProductUploadData table
                                    data.ProductFileUploadId = ProductFileUploadId; // set foreign key pointing to ProductFileUpload entry


                                    //traverse the row in the file to retrieve the corresponding values
                                    //
                                    // LEAVING OFF HERE: throwing error because the openxml thing is skipping over any cell that is blank.
                                    // So if there are only 5 cells in the row with data, itll throw an out of range exception when we prompt it for ElementAt(6). 
                                    //

                                    // Assign cells to enumerable - including empty cells
                                    IEnumerable<Cell> cells = AL.WebAPI.Helpers.SpreedsheetHelper.GetRowCells(thecurrentrow);

                                    try
                                    {
                                        data.PriceChange = cells.ElementAt(0).DataType != null ? stringTable.SharedStringTable.ElementAt(int.Parse(cells.ElementAt(0).InnerText)).InnerText : cells.ElementAt(0).InnerText;
                                    }
                                    catch (FormatException ex) { }

                                    // Skip to next row if first column is blank.
                                    if (data.PriceChange == "")
                                        continue;



                                    try
                                    {
                                        data.Manufacturer = cells.ElementAt(1).DataType != null ? stringTable.SharedStringTable.ElementAt(int.Parse(cells.ElementAt(1).InnerText)).InnerText : cells.ElementAt(1).InnerText;
                                    }
                                    catch (FormatException ex) { }

                                    // TODO: Need to add SupplierName to underlying database table
                                    //try
                                    //{
                                    //    data.SupplierName = stringTable.SharedStringTable.ElementAt(int.Parse(cells.ElementAt(2).InnerText)).InnerText;
                                    //}
                                    //catch (FormatException ex) { }

                                    try
                                    {
                                        data.SupplierStyleName = cells.ElementAt(3).DataType != null ? stringTable.SharedStringTable.ElementAt(int.Parse(cells.ElementAt(3).InnerText)).InnerText : cells.ElementAt(3).InnerText;
                                    }
                                    catch (FormatException ex) { }

                                    try
                                    {
                                        data.SupplierStyleNumber = cells.ElementAt(4).DataType != null ? stringTable.SharedStringTable.ElementAt(int.Parse(cells.ElementAt(4).InnerText)).InnerText : cells.ElementAt(4).InnerText;
                                    }
                                    catch (FormatException ex) { }

                                    try
                                    {
                                        data.ProductPrice_New = cells.ElementAt(5).DataType != null ? stringTable.SharedStringTable.ElementAt(int.Parse(cells.ElementAt(5).InnerText)).InnerText : cells.ElementAt(5).InnerText;
                                    }
                                    catch (FormatException ex) { }

                                    try
                                    {
                                        data.ProductPrice_Old = cells.ElementAt(6).DataType != null ? stringTable.SharedStringTable.ElementAt(int.Parse(cells.ElementAt(6).InnerText)).InnerText : cells.ElementAt(6).InnerText;
                                    }
                                    catch (FormatException ex) { }

                                    try
                                    {
                                        data.UnitOfMeasure = cells.ElementAt(7).DataType != null ? stringTable.SharedStringTable.ElementAt(int.Parse(cells.ElementAt(7).InnerText)).InnerText : cells.ElementAt(7).InnerText;
                                    }
                                    catch (FormatException ex) { }

                                    try
                                    {
                                        data.ItemWidth = cells.ElementAt(8).DataType != null ? stringTable.SharedStringTable.ElementAt(int.Parse(cells.ElementAt(8).InnerText)).InnerText : cells.ElementAt(8).InnerText;
                                    }
                                    catch (FormatException ex) { }

                                    try
                                    {
                                        data.ItemLength = cells.ElementAt(9).DataType != null ? stringTable.SharedStringTable.ElementAt(int.Parse(cells.ElementAt(9).InnerText)).InnerText : cells.ElementAt(9).InnerText;
                                    }
                                    catch (FormatException ex) { }

                                    try
                                    {
                                        data.CartonQty = cells.ElementAt(10).DataType != null ? stringTable.SharedStringTable.ElementAt(int.Parse(cells.ElementAt(10).InnerText)).InnerText : cells.ElementAt(10).InnerText;
                                    }
                                    catch (FormatException ex) { }

                                    try
                                    {
                                        data.ProductType = cells.ElementAt(11).DataType != null ? stringTable.SharedStringTable.ElementAt(int.Parse(cells.ElementAt(11).InnerText)).InnerText : cells.ElementAt(11).InnerText;
                                    }
                                    catch (FormatException ex) { }

                                    // TODO:  Fill in 2 missing columns here - Need to add fields to database first

                                    try
                                    {
                                        data.PatternMatch = cells.ElementAt(14).DataType != null ? stringTable.SharedStringTable.ElementAt(int.Parse(cells.ElementAt(14).InnerText)).InnerText : cells.ElementAt(14).InnerText;
                                    }
                                    catch (FormatException ex) { }

                                    // TODO:  Fill in 1 missing columns here - Need to add field to database first

                                    try
                                    {
                                        data.ColorNames = cells.ElementAt(15).DataType != null ? stringTable.SharedStringTable.ElementAt(int.Parse(cells.ElementAt(8).InnerText)).InnerText : cells.ElementAt(15).InnerText;
                                    }
                                    catch (FormatException ex) { }


                                    //data.StoneType = stringTable.SharedStringTable.ElementAt(int.Parse(thecurrentrow.ChildElements.ElementAt(12).InnerText)).InnerText;
                                    //data.CarpetType = stringTable.SharedStringTable.ElementAt(int.Parse(thecurrentrow.ChildElements.ElementAt(13).InnerText)).InnerText;
                                    //data.StoneType = stringTable.SharedStringTable.ElementAt(int.Parse(thecurrentrow.ChildElements.ElementAt(15).InnerText)).InnerText;
                                    //data.OriginCountry = stringTable.SharedStringTable.ElementAt(int.Parse(thecurrentrow.ChildElements.ElementAt(17).InnerText)).InnerText;
                                    //data.Notes = stringTable.SharedStringTable.ElementAt(int.Parse(thecurrentrow.ChildElements.ElementAt(16).InnerText)).InnerText;
                                    _ARContext.ProductUploadData.Add(data); // saving to memory (but not the database) 
                                    _ARContext.SaveChanges();

                                }
                                catch (Exception e) { } // todo: add error logging statement
                            }
                        }


                        // Save Changes -- actually writing the entry into database 
                        try
                        {
                            _ARContext.SaveChanges();
                        }
                        catch (Exception e) { } // todo: add error logging statement 

                    }

                }

            }
            catch (Exception e) { } // todo: error logging 

            return 0;
        }






        public List<ProductUploadFilesViewModel> ListUploadedProductFiles()
        {
            var uploadedProductFiles =


                from record in _ARContext.ProductUploadFiles

                join manuf in _ARContext.Manufacturer on record.ManufacturerId equals manuf.ManufacturerId // join with the manufacturer table so we can get the string version of the manufacturer's name, instead of an integer id that is stored in the ProductUploadFiles table. 

                join file in _ARContext.FileUploads on record.FileID equals file.FileID // join with fileuploads table to get the string version of the file's name 


                // get data that we wanna display
                select new ProductUploadFilesViewModel
                {
                    // displayed fields 
                    FileName = file.FileName,
                    ManufacturerName = manuf.Name,
                    UploadedDate = record.UploadedDate,
                    UploadedBy = record.UploadedBy,
                    LoadedDate = record.LoadedDate,
                    // # Errors   (todo) -- num of errors encountered during the load

                    // undisplayed/hidden fields
                    FileID = record.FileID,
                    LoadedBy = record.LoadedBy,
                    ManufacturerId = record.ManufacturerId,
                    ProductFileUploadId = record.ProductFileUploadId

                };

            return uploadedProductFiles.ToList(); 
                
        }


        public List<ViewUploadedProductFileDataViewModel> ViewUploadedProductFileData(int ProductFileUploadId)
        {
            var data =
                from record in _ARContext.ProductUploadData
                where record.ProductFileUploadId == ProductFileUploadId
                select new ViewUploadedProductFileDataViewModel
                {
                    ProductUploadDataId = record.ProductUploadDataId,
                    ProductFileUploadId = record.ProductFileUploadId,
                    SupplierStyleName = record.SupplierStyleName,
                    SupplierStyleNumber = record.SupplierStyleNumber,
                    Manufacturer = record.Manufacturer,
                    ProductPrice_New = record.ProductPrice_New,
                    ProductPrice_Old = record.ProductPrice_Old,
                    UnitOfMeasure = record.UnitOfMeasure,
                    ItemLength = record.ItemLength,
                    ItemWidth = record.ItemWidth,
                    CartonQty = record.CartonQty,
                    ProductType = record.ProductType,
                    PriceChange = record.PriceChange,
                    CarpetType = record.CarpetType,
                    PatternMatch = record.PatternMatch,
                    FiberType = record.FiberType, 
                    ColorNames = record.ColorNames
                };

            return data.ToList();
        }


        

        
    }
}
