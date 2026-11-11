using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using CrownATTime.Server.Models.DistributorProducts;

namespace CrownATTime.Server.Data
{
    public partial class DistributorProductsContext : DbContext
    {
        public DistributorProductsContext()
        {
        }

        public DistributorProductsContext(DbContextOptions<DistributorProductsContext> options) : base(options)
        {
        }

        partial void OnModelBuilding(ModelBuilder builder);

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefgalleryBeforeV27>().HasNoKey();

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.IngramProductsForQuoting>().HasNoKey();

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.IngramProductsWithDetail>().HasNoKey();

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefapprovedSkuImage>().HasNoKey();

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefcurrentFamily>().HasNoKey();

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefimageStorageSummary>().HasNoKey();

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefunmatchedSku>().HasNoKey();

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductsForQuoting>().HasNoKey();

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductsWithEnrichment>().HasNoKey();

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.IngramManufacturerContent>().HasKey(table => new {
                table.VendorName, table.VendorPartNumber
            });

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.IngramWarehouseInventory>().HasKey(table => new {
                table.IngramPartNumber, table.WarehouseLocation
            });

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.StreakwaveWarehouseStock>().HasKey(table => new {
                table.Sku, table.WarehouseCode
            });

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.IngramProductEnrichment>()
              .HasOne(i => i.IngramProduct)
              .WithMany(i => i.IngramProductEnrichments)
              .HasForeignKey(i => i.IngramPartNumber)
              .HasPrincipalKey(i => i.IngramPartNumber)
              .OnDelete(DeleteBehavior.ClientNoAction);

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.IngramWarehouseInventory>()
              .HasOne(i => i.IngramProduct)
              .WithMany(i => i.IngramWarehouseInventories)
              .HasForeignKey(i => i.IngramPartNumber)
              .HasPrincipalKey(i => i.IngramPartNumber)
              .OnDelete(DeleteBehavior.ClientNoAction);

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamily>()
              .HasOne(i => i.LenovoPsrefedition)
              .WithMany(i => i.LenovoPsreffamilies)
              .HasForeignKey(i => i.EditionId)
              .HasPrincipalKey(i => i.EditionId)
              .OnDelete(DeleteBehavior.ClientNoAction);

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamilyImage>()
              .HasOne(i => i.LenovoPsreffamily)
              .WithMany(i => i.LenovoPsreffamilyImages)
              .HasForeignKey(i => i.FamilyId)
              .HasPrincipalKey(i => i.FamilyId)
              .OnDelete(DeleteBehavior.ClientNoAction);

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefimageDatum>()
              .HasOne(i => i.LenovoPsreffamilyImage)
              .WithMany(i => i.LenovoPsrefimageData)
              .HasForeignKey(i => i.ImageId)
              .HasPrincipalKey(i => i.ImageId)
              .OnDelete(DeleteBehavior.ClientNoAction);

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductEnrichment>()
              .HasOne(i => i.StreakwaveProduct)
              .WithMany(i => i.StreakwaveProductEnrichments)
              .HasForeignKey(i => i.Sku)
              .HasPrincipalKey(i => i.Sku)
              .OnDelete(DeleteBehavior.ClientNoAction);

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.StreakwaveWarehouseStock>()
              .HasOne(i => i.StreakwaveProduct)
              .WithMany(i => i.StreakwaveWarehouseStocks)
              .HasForeignKey(i => i.Sku)
              .HasPrincipalKey(i => i.Sku)
              .OnDelete(DeleteBehavior.ClientNoAction);

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.IngramLenovoFamilyImage>()
              .Property(p => p.IsApproved)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.IngramLenovoFamilyImage>()
              .Property(p => p.UpdatedAtUtc)
              .HasDefaultValueSql(@"(sysutcdatetime())");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.IngramLenovoScrapeSource>()
              .Property(p => p.IsEnabled)
              .HasDefaultValueSql(@"((1))");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.IngramManufacturerContent>()
              .Property(p => p.IsApproved)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.IngramManufacturerContent>()
              .Property(p => p.LastUpdatedAtUtc)
              .HasDefaultValueSql(@"(sysutcdatetime())");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.IngramProduct>()
              .Property(p => p.IsDiscontinued)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.IngramProduct>()
              .Property(p => p.FirstSeenAtUtc)
              .HasDefaultValueSql(@"(sysutcdatetime())");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.IngramProduct>()
              .Property(p => p.LastSeenAtUtc)
              .HasDefaultValueSql(@"(sysutcdatetime())");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.IngramProduct>()
              .Property(p => p.LastUpdatedAtUtc)
              .HasDefaultValueSql(@"(sysutcdatetime())");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.IngramProductEnrichment>()
              .Property(p => p.HasApprovedManufacturerContent)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.IngramProductEnrichment>()
              .Property(p => p.ImageCount)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.IngramSyncRule>()
              .Property(p => p.IsEnabled)
              .HasDefaultValueSql(@"((1))");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.IngramSyncRule>()
              .Property(p => p.PageSize)
              .HasDefaultValueSql(@"((50))");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.IngramSyncRule>()
              .Property(p => p.MaxPages)
              .HasDefaultValueSql(@"((100))");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.IngramSyncRule>()
              .Property(p => p.CreatedAtUtc)
              .HasDefaultValueSql(@"(sysutcdatetime())");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.IngramWarehouseInventory>()
              .Property(p => p.LastCheckedAtUtc)
              .HasDefaultValueSql(@"(sysutcdatetime())");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefedition>()
              .Property(p => p.Status)
              .HasDefaultValueSql(@"('STAGING')");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefedition>()
              .Property(p => p.ImportedAtUtc)
              .HasDefaultValueSql(@"(sysutcdatetime())");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamily>()
              .Property(p => p.ExtractedAtUtc)
              .HasDefaultValueSql(@"(sysutcdatetime())");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamilyImage>()
              .Property(p => p.IsApproved)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefimageDatum>()
              .Property(p => p.StoredAtUtc)
              .HasDefaultValueSql(@"(sysutcdatetime())");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefimageDatum>()
              .Property(p => p.AccessKey)
              .HasDefaultValueSql(@"(newid())").ValueGeneratedOnAdd();

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefskuFamilyMap>()
              .Property(p => p.MatchMethod)
              .HasDefaultValueSql(@"('MANUAL')");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefskuFamilyMap>()
              .Property(p => p.ApprovalStatus)
              .HasDefaultValueSql(@"('PENDING')");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefskuFamilyMap>()
              .Property(p => p.UpdatedAtUtc)
              .HasDefaultValueSql(@"(sysutcdatetime())");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.StreakwaveProduct>()
              .Property(p => p.IsDiscontinued)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.StreakwaveProduct>()
              .Property(p => p.LastCatalogSeenAtUtc)
              .HasDefaultValueSql(@"(sysutcdatetime())");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.StreakwaveProduct>()
              .Property(p => p.CreatedAtUtc)
              .HasDefaultValueSql(@"(sysutcdatetime())");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.StreakwaveProduct>()
              .Property(p => p.UpdatedAtUtc)
              .HasDefaultValueSql(@"(sysutcdatetime())");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductEnrichment>()
              .Property(p => p.ScrapeStatus)
              .HasDefaultValueSql(@"(N'PENDING')");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductEnrichment>()
              .Property(p => p.UpdatedAtUtc)
              .HasDefaultValueSql(@"(sysutcdatetime())");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.StreakwaveSyncRule>()
              .Property(p => p.IsEnabled)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.StreakwaveSyncRule>()
              .Property(p => p.CreatedAtUtc)
              .HasDefaultValueSql(@"(sysutcdatetime())");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.IngramLenovoFamilyImage>()
              .Property(p => p.UpdatedAtUtc)
              .HasColumnType("datetime2(3)");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.IngramLenovoScrapeSource>()
              .Property(p => p.LastScrapeAttemptAtUtc)
              .HasColumnType("datetime2(3)");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.IngramLenovoScrapeSource>()
              .Property(p => p.LastScrapeSuccessAtUtc)
              .HasColumnType("datetime2(3)");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.IngramManufacturerContent>()
              .Property(p => p.LastUpdatedAtUtc)
              .HasColumnType("datetime2(3)");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.IngramProduct>()
              .Property(p => p.FirstSeenAtUtc)
              .HasColumnType("datetime2(3)");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.IngramProduct>()
              .Property(p => p.LastSeenAtUtc)
              .HasColumnType("datetime2(3)");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.IngramProduct>()
              .Property(p => p.LastUpdatedAtUtc)
              .HasColumnType("datetime2(3)");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.IngramProduct>()
              .Property(p => p.LastStockAttemptAtUtc)
              .HasColumnType("datetime2(3)");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.IngramProduct>()
              .Property(p => p.LastStockCheckAtUtc)
              .HasColumnType("datetime2(3)");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.IngramProductEnrichment>()
              .Property(p => p.LastEnrichmentAttemptAtUtc)
              .HasColumnType("datetime2(3)");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.IngramProductEnrichment>()
              .Property(p => p.LastEnrichmentSuccessAtUtc)
              .HasColumnType("datetime2(3)");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.IngramSyncRule>()
              .Property(p => p.CreatedAtUtc)
              .HasColumnType("datetime2(3)");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.IngramWarehouseInventory>()
              .Property(p => p.LastCheckedAtUtc)
              .HasColumnType("datetime2(3)");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefedition>()
              .Property(p => p.ImportedAtUtc)
              .HasColumnType("datetime2(3)");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefedition>()
              .Property(p => p.PublishedAtUtc)
              .HasColumnType("datetime2(3)");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamily>()
              .Property(p => p.ExtractedAtUtc)
              .HasColumnType("datetime2(3)");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefgalleryBeforeV27>()
              .Property(p => p.BackedUpAtUtc)
              .HasColumnType("datetime2");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefimageDatum>()
              .Property(p => p.StoredAtUtc)
              .HasColumnType("datetime2(3)");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefskuFamilyMap>()
              .Property(p => p.UpdatedAtUtc)
              .HasColumnType("datetime2(3)");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.StreakwaveProduct>()
              .Property(p => p.LastCatalogSeenAtUtc)
              .HasColumnType("datetime2(0)");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.StreakwaveProduct>()
              .Property(p => p.LastStockAttemptAtUtc)
              .HasColumnType("datetime2(0)");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.StreakwaveProduct>()
              .Property(p => p.LastStockCheckAtUtc)
              .HasColumnType("datetime2(0)");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.StreakwaveProduct>()
              .Property(p => p.CreatedAtUtc)
              .HasColumnType("datetime2(0)");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.StreakwaveProduct>()
              .Property(p => p.UpdatedAtUtc)
              .HasColumnType("datetime2(0)");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.StreakwaveProduct>()
              .Property(p => p.FeedGeneratedAtUtc)
              .HasColumnType("datetime2(3)");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.StreakwaveProduct>()
              .Property(p => p.LastSourceUpdatedAtUtc)
              .HasColumnType("datetime2(3)");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductEnrichment>()
              .Property(p => p.LastScrapeAttemptAtUtc)
              .HasColumnType("datetime2(3)");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductEnrichment>()
              .Property(p => p.LastScrapeSuccessAtUtc)
              .HasColumnType("datetime2(3)");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductEnrichment>()
              .Property(p => p.UpdatedAtUtc)
              .HasColumnType("datetime2(3)");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.StreakwaveSyncRule>()
              .Property(p => p.CreatedAtUtc)
              .HasColumnType("datetime2(0)");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.StreakwaveWarehouseStock>()
              .Property(p => p.LastCheckedAtUtc)
              .HasColumnType("datetime2(0)");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.IngramProductsForQuoting>()
              .Property(p => p.FirstSeenAtUtc)
              .HasColumnType("datetime2(3)");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.IngramProductsForQuoting>()
              .Property(p => p.LastSeenAtUtc)
              .HasColumnType("datetime2(3)");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.IngramProductsForQuoting>()
              .Property(p => p.LastUpdatedAtUtc)
              .HasColumnType("datetime2(3)");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.IngramProductsForQuoting>()
              .Property(p => p.LastStockAttemptAtUtc)
              .HasColumnType("datetime2(3)");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.IngramProductsForQuoting>()
              .Property(p => p.LastStockCheckAtUtc)
              .HasColumnType("datetime2(3)");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.IngramProductsWithDetail>()
              .Property(p => p.FirstSeenAtUtc)
              .HasColumnType("datetime2(3)");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.IngramProductsWithDetail>()
              .Property(p => p.LastSeenAtUtc)
              .HasColumnType("datetime2(3)");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.IngramProductsWithDetail>()
              .Property(p => p.LastUpdatedAtUtc)
              .HasColumnType("datetime2(3)");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.IngramProductsWithDetail>()
              .Property(p => p.LastStockAttemptAtUtc)
              .HasColumnType("datetime2(3)");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.IngramProductsWithDetail>()
              .Property(p => p.LastStockCheckAtUtc)
              .HasColumnType("datetime2(3)");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.IngramProductsWithDetail>()
              .Property(p => p.LastEnrichmentAttemptAtUtc)
              .HasColumnType("datetime2(3)");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.IngramProductsWithDetail>()
              .Property(p => p.LastEnrichmentSuccessAtUtc)
              .HasColumnType("datetime2(3)");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductsForQuoting>()
              .Property(p => p.LastCatalogSeenAtUtc)
              .HasColumnType("datetime2(0)");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductsForQuoting>()
              .Property(p => p.LastStockAttemptAtUtc)
              .HasColumnType("datetime2(0)");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductsForQuoting>()
              .Property(p => p.LastStockCheckAtUtc)
              .HasColumnType("datetime2(0)");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductsForQuoting>()
              .Property(p => p.CreatedAtUtc)
              .HasColumnType("datetime2(0)");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductsForQuoting>()
              .Property(p => p.UpdatedAtUtc)
              .HasColumnType("datetime2(0)");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductsForQuoting>()
              .Property(p => p.FeedGeneratedAtUtc)
              .HasColumnType("datetime2(3)");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductsForQuoting>()
              .Property(p => p.LastSourceUpdatedAtUtc)
              .HasColumnType("datetime2(3)");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductsWithEnrichment>()
              .Property(p => p.LastCatalogSeenAtUtc)
              .HasColumnType("datetime2(0)");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductsWithEnrichment>()
              .Property(p => p.LastStockAttemptAtUtc)
              .HasColumnType("datetime2(0)");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductsWithEnrichment>()
              .Property(p => p.LastStockCheckAtUtc)
              .HasColumnType("datetime2(0)");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductsWithEnrichment>()
              .Property(p => p.CreatedAtUtc)
              .HasColumnType("datetime2(0)");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductsWithEnrichment>()
              .Property(p => p.UpdatedAtUtc)
              .HasColumnType("datetime2(0)");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductsWithEnrichment>()
              .Property(p => p.FeedGeneratedAtUtc)
              .HasColumnType("datetime2(3)");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductsWithEnrichment>()
              .Property(p => p.LastSourceUpdatedAtUtc)
              .HasColumnType("datetime2(3)");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductsWithEnrichment>()
              .Property(p => p.LastScrapeSuccessAtUtc)
              .HasColumnType("datetime2(3)");

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.IngramLenovoFamilyImage>()
              .Property(p => p.UpdatedAtUtc)
              .HasPrecision(3);

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.IngramLenovoScrapeSource>()
              .Property(p => p.LastScrapeAttemptAtUtc)
              .HasPrecision(3);

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.IngramLenovoScrapeSource>()
              .Property(p => p.LastScrapeSuccessAtUtc)
              .HasPrecision(3);

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.IngramManufacturerContent>()
              .Property(p => p.LastUpdatedAtUtc)
              .HasPrecision(3);

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.IngramProduct>()
              .Property(p => p.FirstSeenAtUtc)
              .HasPrecision(3);

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.IngramProduct>()
              .Property(p => p.LastSeenAtUtc)
              .HasPrecision(3);

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.IngramProduct>()
              .Property(p => p.LastUpdatedAtUtc)
              .HasPrecision(3);

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.IngramProduct>()
              .Property(p => p.CustomerCost)
              .HasPrecision(19,4);

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.IngramProduct>()
              .Property(p => p.LastStockAttemptAtUtc)
              .HasPrecision(3);

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.IngramProduct>()
              .Property(p => p.LastStockCheckAtUtc)
              .HasPrecision(3);

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.IngramProductEnrichment>()
              .Property(p => p.LastEnrichmentAttemptAtUtc)
              .HasPrecision(3);

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.IngramProductEnrichment>()
              .Property(p => p.LastEnrichmentSuccessAtUtc)
              .HasPrecision(3);

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.IngramSyncRule>()
              .Property(p => p.CreatedAtUtc)
              .HasPrecision(3);

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.IngramWarehouseInventory>()
              .Property(p => p.LastCheckedAtUtc)
              .HasPrecision(3);

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefedition>()
              .Property(p => p.ImportedAtUtc)
              .HasPrecision(3);

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefedition>()
              .Property(p => p.PublishedAtUtc)
              .HasPrecision(3);

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamily>()
              .Property(p => p.ExtractedAtUtc)
              .HasPrecision(3);

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefimageDatum>()
              .Property(p => p.StoredAtUtc)
              .HasPrecision(3);

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefskuFamilyMap>()
              .Property(p => p.UpdatedAtUtc)
              .HasPrecision(3);

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.StreakwaveProduct>()
              .Property(p => p.CustomerCost)
              .HasPrecision(19,4);

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.StreakwaveProduct>()
              .Property(p => p.Msrp)
              .HasPrecision(19,4);

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.StreakwaveProduct>()
              .Property(p => p.LastCatalogSeenAtUtc)
              .HasPrecision(0);

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.StreakwaveProduct>()
              .Property(p => p.LastStockAttemptAtUtc)
              .HasPrecision(0);

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.StreakwaveProduct>()
              .Property(p => p.LastStockCheckAtUtc)
              .HasPrecision(0);

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.StreakwaveProduct>()
              .Property(p => p.CreatedAtUtc)
              .HasPrecision(0);

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.StreakwaveProduct>()
              .Property(p => p.UpdatedAtUtc)
              .HasPrecision(0);

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.StreakwaveProduct>()
              .Property(p => p.Map)
              .HasPrecision(19,4);

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.StreakwaveProduct>()
              .Property(p => p.FeedGeneratedAtUtc)
              .HasPrecision(3);

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.StreakwaveProduct>()
              .Property(p => p.LastSourceUpdatedAtUtc)
              .HasPrecision(3);

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductEnrichment>()
              .Property(p => p.LastScrapeAttemptAtUtc)
              .HasPrecision(3);

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductEnrichment>()
              .Property(p => p.LastScrapeSuccessAtUtc)
              .HasPrecision(3);

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductEnrichment>()
              .Property(p => p.UpdatedAtUtc)
              .HasPrecision(3);

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.StreakwaveSyncRule>()
              .Property(p => p.CreatedAtUtc)
              .HasPrecision(0);

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.StreakwaveWarehouseStock>()
              .Property(p => p.LastCheckedAtUtc)
              .HasPrecision(0);

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.IngramProductsForQuoting>()
              .Property(p => p.FirstSeenAtUtc)
              .HasPrecision(3);

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.IngramProductsForQuoting>()
              .Property(p => p.LastSeenAtUtc)
              .HasPrecision(3);

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.IngramProductsForQuoting>()
              .Property(p => p.LastUpdatedAtUtc)
              .HasPrecision(3);

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.IngramProductsForQuoting>()
              .Property(p => p.CustomerCost)
              .HasPrecision(19,4);

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.IngramProductsForQuoting>()
              .Property(p => p.LastStockAttemptAtUtc)
              .HasPrecision(3);

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.IngramProductsForQuoting>()
              .Property(p => p.LastStockCheckAtUtc)
              .HasPrecision(3);

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.IngramProductsWithDetail>()
              .Property(p => p.FirstSeenAtUtc)
              .HasPrecision(3);

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.IngramProductsWithDetail>()
              .Property(p => p.LastSeenAtUtc)
              .HasPrecision(3);

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.IngramProductsWithDetail>()
              .Property(p => p.LastUpdatedAtUtc)
              .HasPrecision(3);

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.IngramProductsWithDetail>()
              .Property(p => p.CustomerCost)
              .HasPrecision(19,4);

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.IngramProductsWithDetail>()
              .Property(p => p.LastStockAttemptAtUtc)
              .HasPrecision(3);

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.IngramProductsWithDetail>()
              .Property(p => p.LastStockCheckAtUtc)
              .HasPrecision(3);

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.IngramProductsWithDetail>()
              .Property(p => p.LastEnrichmentAttemptAtUtc)
              .HasPrecision(3);

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.IngramProductsWithDetail>()
              .Property(p => p.LastEnrichmentSuccessAtUtc)
              .HasPrecision(3);

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductsForQuoting>()
              .Property(p => p.CustomerCost)
              .HasPrecision(19,4);

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductsForQuoting>()
              .Property(p => p.Msrp)
              .HasPrecision(19,4);

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductsForQuoting>()
              .Property(p => p.LastCatalogSeenAtUtc)
              .HasPrecision(0);

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductsForQuoting>()
              .Property(p => p.LastStockAttemptAtUtc)
              .HasPrecision(0);

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductsForQuoting>()
              .Property(p => p.LastStockCheckAtUtc)
              .HasPrecision(0);

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductsForQuoting>()
              .Property(p => p.CreatedAtUtc)
              .HasPrecision(0);

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductsForQuoting>()
              .Property(p => p.UpdatedAtUtc)
              .HasPrecision(0);

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductsForQuoting>()
              .Property(p => p.Map)
              .HasPrecision(19,4);

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductsForQuoting>()
              .Property(p => p.FeedGeneratedAtUtc)
              .HasPrecision(3);

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductsForQuoting>()
              .Property(p => p.LastSourceUpdatedAtUtc)
              .HasPrecision(3);

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductsWithEnrichment>()
              .Property(p => p.CustomerCost)
              .HasPrecision(19,4);

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductsWithEnrichment>()
              .Property(p => p.Msrp)
              .HasPrecision(19,4);

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductsWithEnrichment>()
              .Property(p => p.LastCatalogSeenAtUtc)
              .HasPrecision(0);

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductsWithEnrichment>()
              .Property(p => p.LastStockAttemptAtUtc)
              .HasPrecision(0);

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductsWithEnrichment>()
              .Property(p => p.LastStockCheckAtUtc)
              .HasPrecision(0);

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductsWithEnrichment>()
              .Property(p => p.CreatedAtUtc)
              .HasPrecision(0);

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductsWithEnrichment>()
              .Property(p => p.UpdatedAtUtc)
              .HasPrecision(0);

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductsWithEnrichment>()
              .Property(p => p.Map)
              .HasPrecision(19,4);

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductsWithEnrichment>()
              .Property(p => p.FeedGeneratedAtUtc)
              .HasPrecision(3);

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductsWithEnrichment>()
              .Property(p => p.LastSourceUpdatedAtUtc)
              .HasPrecision(3);

            builder.Entity<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductsWithEnrichment>()
              .Property(p => p.LastScrapeSuccessAtUtc)
              .HasPrecision(3);
            this.OnModelBuilding(builder);
        }

        public DbSet<CrownATTime.Server.Models.DistributorProducts.IngramLenovoFamilyImage> IngramLenovoFamilyImages { get; set; }

        public DbSet<CrownATTime.Server.Models.DistributorProducts.IngramLenovoScrapeSource> IngramLenovoScrapeSources { get; set; }

        public DbSet<CrownATTime.Server.Models.DistributorProducts.IngramManufacturerContent> IngramManufacturerContents { get; set; }

        public DbSet<CrownATTime.Server.Models.DistributorProducts.IngramProduct> IngramProducts { get; set; }

        public DbSet<CrownATTime.Server.Models.DistributorProducts.IngramProductEnrichment> IngramProductEnrichments { get; set; }

        public DbSet<CrownATTime.Server.Models.DistributorProducts.IngramSyncRule> IngramSyncRules { get; set; }

        public DbSet<CrownATTime.Server.Models.DistributorProducts.IngramWarehouseInventory> IngramWarehouseInventories { get; set; }

        public DbSet<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefedition> LenovoPsrefeditions { get; set; }

        public DbSet<CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamily> LenovoPsreffamilies { get; set; }

        public DbSet<CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamilyImage> LenovoPsreffamilyImages { get; set; }

        public DbSet<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefgalleryBeforeV27> LenovoPsrefgalleryBeforeV27S { get; set; }

        public DbSet<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefimageDatum> LenovoPsrefimageData { get; set; }

        public DbSet<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefskuFamilyMap> LenovoPsrefskuFamilyMaps { get; set; }

        public DbSet<CrownATTime.Server.Models.DistributorProducts.StreakwaveProduct> StreakwaveProducts { get; set; }

        public DbSet<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductEnrichment> StreakwaveProductEnrichments { get; set; }

        public DbSet<CrownATTime.Server.Models.DistributorProducts.StreakwaveSyncRule> StreakwaveSyncRules { get; set; }

        public DbSet<CrownATTime.Server.Models.DistributorProducts.StreakwaveWarehouseStock> StreakwaveWarehouseStocks { get; set; }

        public DbSet<CrownATTime.Server.Models.DistributorProducts.IngramProductsForQuoting> IngramProductsForQuotings { get; set; }

        public DbSet<CrownATTime.Server.Models.DistributorProducts.IngramProductsWithDetail> IngramProductsWithDetails { get; set; }

        public DbSet<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefapprovedSkuImage> LenovoPsrefapprovedSkuImages { get; set; }

        public DbSet<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefcurrentFamily> LenovoPsrefcurrentFamilies { get; set; }

        public DbSet<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefimageStorageSummary> LenovoPsrefimageStorageSummaries { get; set; }

        public DbSet<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefunmatchedSku> LenovoPsrefunmatchedSkus { get; set; }

        public DbSet<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductsForQuoting> StreakwaveProductsForQuotings { get; set; }

        public DbSet<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductsWithEnrichment> StreakwaveProductsWithEnrichments { get; set; }
        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Conventions.Add(_ => new BlankTriggerAddingConvention());
            configurationBuilder.Conventions.Remove(typeof(Microsoft.EntityFrameworkCore.Metadata.Conventions.CascadeDeleteConvention));
            configurationBuilder.Conventions.Remove(typeof(Microsoft.EntityFrameworkCore.Metadata.Conventions.SqlServerOnDeleteConvention));
        }
    }
}