using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshop.Entities.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [DisplayName("Image")]
        [ValidateNever]
        [StringLength(300)]
        public string Img { get; set; }

        [Required]
        [Range(0.01, 100000)]
        public decimal Price { get; set; }

        [Required]
        [DisplayName("Category")]
        public int CategoryId { get; set; }

        [ValidateNever]
        public Category Category { get; set; }
    }
}
