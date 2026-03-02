using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TJM103.Models.Entity
{
    public class Product
    {
        [Key] // 設定為主鍵
        public int Id { get; set; }

        [Required] // 不可為空
        [MaxLength(100)] // 限制字串長度
        public string Name { get; set; }

        [Column(TypeName = "decimal(18,2)")] // 指定 SQL Server 中的精確度
        public decimal Price { get; set; }

        public int Stock { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}
