using Microsoft.EntityFrameworkCore;

namespace BestellApp
{
    public class BestellDbContext : DbContext
    {
        public DbSet<User> Users {  get; set; }
        public DbSet<OrderItem> Items { get; set; }
        public DbSet<Order> Orders { get; set; }
        List<Order> CurrentOrders = new();
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Product> Products { get; set; }


        public string DbPath { get; }

        public BestellDbContext(DbContextOptions<BestellDbContext> options) : base(options)
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "BestellApp.db");
        }

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Restaurant>().HasData(
                new Restaurant { Id = 1, Name = "Big Boy Pizza"}
            );
        }
    }

    public class User
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public UserRole Role {  get; set; } = UserRole.Participant;
        public required string Email { get; set; }
        public List<string>? SavedCustomDesc { get; set; }
        public List<string>? SavedPayMethods { get; set; }
    }

    public enum UserRole
    {
        OrderCreator,
        Participant
    }

    public class Product
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public string Description { get; set; } = string.Empty;
        public required float Price { get; set; }
        public List<string> Extras { get; set; } = [];
    }

    public class OrderItem
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class Order
    {
        public int Id { get; set; }
        public string Creator { get; set; }
        public string? CustomDescription { get; set; }
        public Status Status {  get; set; }
        public List<OrderItem> Items { get; set; } = [];
        public List<string> PayMethods { get; set; } = [];
        public Guid OrderId { get; set; }
        public DateTimeOffset CreationDate { get; set; }
        public DateTimeOffset ClosingTime { get; set; }
    }

    public enum Status
    {
        finished,
        active
    }

    public class Restaurant
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    
    public class BestellDbService(BestellDbContext db)
    {
        private readonly BestellDbContext _db = db;

        public async Task<Order> StartOrderAsync(User usr)
        {
            var newOrder = new Order()
            {
                Id = new(),
                OrderId = Guid.NewGuid(),
                Creator = $"{usr.FirstName}, {usr.LastName}",
                CreationDate = DateTimeOffset.Now,
                Status = "active"
            };

            usr.Role = UserRole.OrderCreator;

            _db.Orders.Add(newOrder);
            await _db.SaveChangesAsync();

            return newOrder;
        }

        public void SubmitOrderEvent()
        {

        }

        public async Task<List<string>> getRestaurantsAsync()
        {
            return await _db.Restaurants
                .Select(r => r.Name)
                .ToListAsync();
        }
    }
}