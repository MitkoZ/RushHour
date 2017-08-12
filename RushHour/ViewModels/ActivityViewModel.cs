using RushHour.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RushHour.ViewModels
{
    public class ActivityViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [CustomNumberAttribute]
        [DataType(DataType.Duration)]
        public float Duration { get; set; } //must be BG culture with decimal symbol "."
        [Required]
        [CustomNumberAttribute]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; } //must be BG culture with decimal symbol "."
    }
}