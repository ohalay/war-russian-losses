using Microsoft.EntityFrameworkCore;

public class WarContext : DbContext
{
    public WarContext(DbContextOptions<WarContext> options)
        : base(options)
    {
    }

    public DbSet<RussinLoss> RussinLosses { get; set; }
    public DbSet<LossType> LossTypes { get; set; }
}

public class RussinLoss
{
    public int Id { get; set; }
    public int Count { get; set; }
    public DateOnly Date { get; set; }

    public int LossTypeId { get; set; }
    public LossType LossType { get; set; }
}

public class LossType
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string NameEnglish { get; set; }  
    public string? Description { get; set; }
    public Uri? Uri { get; set; }
    public List<RussinLoss> RussinLosses { get; set; }
}



