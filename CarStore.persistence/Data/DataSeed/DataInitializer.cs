using CarStore.Domain.Contracts;
using CarStore.Domain.Entities.CarModule;
using CarStore.persistence.Data.DbContexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace CarStore.persistence.Data.DataSeed
{
    public class DataInitializer : IDataInitializer
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<DataInitializer> _logger;

        public DataInitializer(ApplicationDbContext dbContext,
                               IWebHostEnvironment webHostEnvironment,
                               ILogger<DataInitializer>logger)
        {
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }

        public async Task InitializeAsync()
        {
            try
            {
                if (!await _dbContext.Brands.AnyAsync())
                {
                   await SeedDataFromJsonFileAsync("brands.json",_dbContext.Brands);
                }
                if (!await _dbContext.Categories.AnyAsync())
                {
                   await SeedDataFromJsonFileAsync("categories.json",_dbContext.Categories);
                }

                if (!await _dbContext.Colors.AnyAsync())
                {
                    await SeedDataFromJsonFileAsync("colors.json", _dbContext.Colors);
                }
                await _dbContext.SaveChangesAsync();

                if (!await _dbContext.Cars.AnyAsync())
                {
                    await SeedDataFromJsonFileAsync("cars.json", _dbContext.Cars);
                }

                if (!await _dbContext.Features.AnyAsync())
                {
                    await SeedDataFromJsonFileAsync("features.json", _dbContext.Features);
                }
                await _dbContext.SaveChangesAsync();

                if (!await _dbContext.CarFeatures.AnyAsync())
                {
                    await SeedDataFromJsonFileAsync("carFeatures.json", _dbContext.CarFeatures);
                }
                await _dbContext.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError($"Data Seeding was Failed ->> {ex}");

            }

        }
        private async Task SeedDataFromJsonFileAsync<TEntity>(string fileName, DbSet<TEntity> dbSet) where TEntity : class
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentNullException("File Name is Null, Empty or WhiteSpace!");
            if (!Path.HasExtension(fileName))
                throw new ArgumentNullException($"Invalid File Name Which Read Data From -->>> {fileName} ");

            var fullFilePath = Path.Combine(_webHostEnvironment.ContentRootPath, "Data", "JsonFiles", fileName);
            if (!Path.Exists(fullFilePath))
                throw new Exception($"{fullFilePath}-->>> Specified Directory is Not Exist");
            try
            {
                using var stream = File.OpenRead(fullFilePath);
                var jsonSelializerOptions = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
                var data = JsonSerializer.Deserialize<List<TEntity>>(stream,jsonSelializerOptions);
                if (data is not null)
                    dbSet.AddRange(data);
            }
            catch (Exception ex)
            {

                _logger.LogError($"Failed To Seed [{typeof(TEntity)}] Data From Json => {ex.Message}");
            }

        }
    }
}
