using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ManageProductDetails.Models
{
    public class Products
    {
        
        public int ID { get; set; }
        //[StringLength(60, MinimumLength = 3)]
        public string Name { get; set; }
        public Categories Category { get; set; }
        public float Price { get; set; }
        //[Range(1, 10)]
        public int Quantity { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        [NotMapped]
        public HttpPostedFileBase SmallImage {
            get; set;
         
        }
        [NotMapped]
        public HttpPostedFileBase LargeImage { get; set; }
        //[Display(Name = "Small Image")]
        public string SmallImagePath { get; set; }
        //[Display(Name = "Large Image")]
        public string LargeImagePath { get; set; }

    }
    public enum Categories
    {
        FruitsVegetables,
        CleaningStuff,
        Stationery,
        Snacks
    }
    public class ProductsDBContext : DbContext
    {

        public ProductsDBContext(): base("DefaultConnection")
        { }
        public DbSet<Products> Products { get; set; }
        //public DbSet<Image> Images { get; set; }
    }
}
