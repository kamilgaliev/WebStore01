using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;
using WebStore.DAL.Context;

namespace WebStore.Data
{
    public class WebStoreDbInitializer
    {
        private readonly WebStoreDB _db;
        private readonly ILogger<WebStoreDbInitializer> _Logger;

        public WebStoreDbInitializer(WebStoreDB db, ILogger<WebStoreDbInitializer> Logger)
        {
            _db = db;
            _Logger = Logger;
        }

        public void Initialize()
        {
            var timer = Stopwatch.StartNew();

            _Logger.LogInformation("Инициализация базы данных...");

            //_db.Database.EnsureCreated();

            var db = _db.Database;

            if (db.GetPendingMigrations().Any())
            {
                _Logger.LogInformation("Выполнение миграций...");

                db.Migrate();

                _Logger.LogInformation("Выполнение миграций выполнено успешно");
            }
            else
            {
                _Logger.LogInformation("База данных находится в актуальной версии ({0:0.0###}) с", timer.Elapsed.TotalSeconds);
            }

            try
            {
                InitializeProducts();
            }
            catch (Exception error)
            {
                _Logger.LogError(error, "Ошибка при выолнении инициализации БД");
                throw;
            }
            _Logger.LogInformation("Инициализация БД выполнена успешно ({0:0.0###}) с", timer.Elapsed.TotalSeconds);
        }

        private void InitializeProducts()
        {
            var timer = Stopwatch.StartNew();

            if (_db.Products.Any())
            {
                _Logger.LogInformation("Инициализация товаров не требуется");
                return;
            }

            _Logger.LogInformation("Инициализация товаров...");

            _Logger.LogInformation("Добавление секций...");

            using (_db.Database.BeginTransaction())
            {
                _db.Sections.AddRange(TestData.Sections);

                _db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Sections] ON");

                _db.SaveChanges();

                _db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Sections] OFF");

                _db.Database.CommitTransaction();
            }

            _Logger.LogInformation("Добавление секций выполнено успешно");

            _Logger.LogInformation("Добавление брендов...");

            using (_db.Database.BeginTransaction())
            {
                _db.Brands.AddRange(TestData.Brands);

                _db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Brands] ON");

                _db.SaveChanges();

                _db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Brands] OFF");

                _db.Database.CommitTransaction();
            }

            _Logger.LogInformation("Добавление брендов выполнено успешно");


            _Logger.LogInformation("Добавление товаров...");

            using (_db.Database.BeginTransaction())
            {
                _db.Products.AddRange(TestData.Products);

                _db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Products] ON");

                _db.SaveChanges();

                _db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Products] OFF");

                _db.Database.CommitTransaction();
            }

            _Logger.LogInformation("Добавление товаров выполнено успешно");

            _Logger.LogInformation("Инициализация товаров выполнена успешно ({0:0.0###}) с", timer.Elapsed.TotalSeconds);
        }
    }
}
