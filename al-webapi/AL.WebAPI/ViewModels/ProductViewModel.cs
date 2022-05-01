using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AL.WebAPI.ViewModels
{
    public class ProductViewModel
    {
		public int ProductId { get; set; }
		public bool Inactive { get; set; }
		public string ProductDescription { get; set; }
		public string PrivateDescription { get; set; }
		public string ProductSKU { get; set; }
		public int ProductTypeId { get; set; }
		public int RecordTypeId { get; set; }
		public int? VendorId { get; set; }
		public int? ManufacturerId { get; set; }
		public int? ProductCategoryId { get; set; }
		public int? UnitOfMeasureId_Base { get; set; }
		public int? UnitOfMeasureId_Price { get; set; }
		public decimal? Length { get; set; }
		public decimal? Width { get; set; }
		public decimal? Height { get; set; }
		public decimal? UnitCost { get; set; }
		public decimal? CartonSize { get; set; }
		public string? Manufacturer_Description { get; set; }
		public string? ProductCategory_Description { get; set; }
		public string? UnitOfMeasure_Description { get; set; }
		public string? SizeDescription { get; set; }
		public string? ProductDescription_Extended { get; set; }
		public string? RecordType_Description { get; set; }
		public int? ProductCategoryId_Parent { get; set; }
		public string? ProductCategoryId_Parent_Description { get; set; }
		public DateTime? CertificateOfCompletionSignedDate { get; set; }
		public DateTime? CostUpdateDate { get; set; }
		public DateTime? DroppedDate { get; set; }
		public List<ProductToProductSubtypeViewModel>? Subtypes { get; set; }

	}
}
