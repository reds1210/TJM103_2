using Microsoft.EntityFrameworkCore;
using TJM103.Models.Entity;

namespace TJM103.Models
{
    public class AppDbContext : DbContext
    {
        // 1. 建構函式：接收外部傳入的設定（如連線字串）
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}
