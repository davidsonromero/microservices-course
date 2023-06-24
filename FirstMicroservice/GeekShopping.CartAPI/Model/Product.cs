﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeekShopping.CartAPI.Model
{
    [Table("PRODUCT")]
    public class Product
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        [Column("id")]
        public long Id { get; set; }

        [Column("NAME")]
        [Required]
        [StringLength(150)]
        public string? Name { get; set; }

        [Column("PRICE")]
        [Required]
        [Range(0.01, 1000000)]
        public decimal Price { get; set; }

        [Column("DESCRIPTION")]
        [Required]
        [StringLength(1000)]
        public string? Description { get; set; }

        [Column("CATEGORY_NAME")]
        [Required]
        [StringLength(100)]
        public string? CategoryName { get; set; }

        [Column("IMAGE_URL")]
        [Required]
        [StringLength(300)]
        public string? ImageUrl { get; set; }
    }
}
