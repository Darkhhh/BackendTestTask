using Microsoft.EntityFrameworkCore;

namespace BackendTestTask.Models.Test;

public class AppContext : DbContext
{
    public DbSet<UserInfo> Users { get; set; } = null!;
    
    public DbSet<SignInLog> SignInLogs { get; set; } = null!;

    public DbSet<ReportRequestLog> ReportRequestLogs { get; set; } = null!;
    
    public AppContext(DbContextOptions<AppContext> options) : base(options) { }
}