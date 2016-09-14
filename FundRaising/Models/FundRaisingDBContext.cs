using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundRaising.Models
{
    public class FundRaisingDBContext: DbContext
    {
        public FundRaisingDBContext()
            : base("DBConnection")
        {
           Database.SetInitializer<FundRaisingDBContext>(null);
        }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    //modelBuilder.Entity<Organization>()
        //    //.HasMany(o => o.OrganizationCampaigns)
        //    //.WithOptional()
        //    //.HasForeignKey(c => c.OrganizatonID);
        //    //modelBuilder.Entity<Brochure>()
        //    //.HasMany<Category>(o => o.Categories)
        //    //.WithMany(c => c.Brochures)            
        //    //.Map(cs =>
        //    //    {
        //    //        cs.MapLeftKey("BorchureID");
        //    //        cs.MapRightKey("CategoryID");
        //    //        cs.ToTable("MapBrochureCategory");
        //    //    }
        //    //);


        //    //modelBuilder.Entity<Category>()
        //    //.HasMany<Product>(o => o.products)
        //    //.WithMany(c => c.Categories)
        //    //.Map(cs =>
        //    //{
        //    //    cs.MapLeftKey("CategoryID");
        //    //    cs.MapRightKey("ProductID");
        //    //    cs.ToTable("MapCategoryProducts");
        //    //}
        //    //);
            
            


        //    base.OnModelCreating(modelBuilder);
        //}

        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Campaign> Campaigns{ get; set; }
        public DbSet<Brochure> Brochures { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<MapCategory> mappings { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }
        public DbSet<RegisterModel> Distributors { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<DBRole> Roles { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        public DbSet<Coupon> Coupons { get; set; }

        public DbSet<CouponUser> CouponUsers { get; set; }

        public DbSet<ProductMapping> productToCategoryMap { get; set; }
        public DbSet<CategoryMapping> CategoryToBrochureMap { get; set; }

        public DbSet<ShippingCharge> ShippingCharges { get; set; }

        public DbSet<SalesTaxCharge> SalesTaxCharges { get; set; }

        public DbSet<OrderSummary> OrderSummaries { get; set; }    

        public DbSet<MagazinePriceMapping> MagazinePriceMappings { get; set; }

        public DbSet<GiftCard> GiftCards { get; set; }

    }
}
