using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Domain.Entities
{
    public class Product
    {
        [HiddenInput(DisplayValue=false)]
        public int Id { get; set; }
        [Display(Name="Название")]
        [Required(ErrorMessage="Пожалуйста, введите название товара")]
        public string Name { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name="Описание")]
        [Required(ErrorMessage="Пожалуйста, введите описание товара")]
        public string Description { get; set; }
        [Display(Name="Категория")]
        [Required(ErrorMessage="Пожалуйста, введите категорию товара")]
        public string Category { get; set; }
        [Display(Name="Цена (руб)")]
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage="Пожалуйста, введите корректную цену товара")]
        public decimal Price { get; set; }
        public byte[] ImageData { get; set; }
        public string ImageMimeType {  get; set; }
    }
}
