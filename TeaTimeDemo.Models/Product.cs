using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeaTimeDemo.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        [Display(Name="產品名單")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "大小")]
        public string Size { get; set; }
        [Required]
        [Range(1, 10000)]
        [Display(Name = "價格")]
        public double Price { get; set; }
        [Display(Name = "備註")]
        public string Description { get; set; }
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category Category { get; set; }
        [ValidateNever]
        public string ImageUrl { get; set; }
    }
}
