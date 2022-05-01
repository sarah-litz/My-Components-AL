using System;
using System.ComponentModel.DataAnnotations;

namespace AL.WebAPI.Models
{
    public class ProductUploadFiles
    {
        [Key]
        public int ProductFileUploadId { get; set; }
        public DateTime? LoadedDate { get; set; }
        public string? LoadedBy { get; set; }
        public DateTime? UploadedDate { get; set; }
        public string? UploadedBy { get; set; }
        public Guid FileID { get; set; }
        public int ManufacturerId { get; set; }
    }

    public class ViewUploadedProductFileData
    {
        [Key]
        public int ProductUploadDataId { get; set; }
        public string? SupplierStyleName { get; set; }
        public string? SupplierStyleNumber { get; set; }
        public string? Manufacturer { get; set; }
        public string? ProductPrice_New { get; set; }
        public string? ProductPrice_Old { get; set; }
        public string? UnitOfMeasure { get; set; }
        public string? ItemLength { get; set; }
        public string? ItemWidth { get; set; }
        public string? CartonQty { get; set; }
        public string? ProductType { get; set; }
        public string? PriceChange { get; set; }
        public string? CarpetType { get; set; }
        public string? PatternMatch { get; set; }
        public string? FiberType { get; set; }
        public string? ColorNames { get; set; }
        public int? ProductFileUploadId { get; set; }

    }


}
