using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TJM103.Models.Entity
{
    public class Product
    {
        [Key] // 設定為主鍵
        public int Id { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
        public string Category { get; set; }
        public int Price { get; set; }
        public string Pic { get; set; }
        public bool IsSell { get; set; }

    }
}
