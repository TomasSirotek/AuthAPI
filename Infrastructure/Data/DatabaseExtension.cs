using DbUp;

namespace ProductAPI.Infrastructure.Data {
        public class DatabaseExtension : IStartupFilter
        {
            private readonly IConfiguration _configuration;
            private readonly DbCustomLogger<DatabaseExtension> _logger;

            public DatabaseExtension(IConfiguration config, DbCustomLogger<DatabaseExtension> logger)
            {
                _configuration = config;
                _logger = logger;
            }

            public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
            {
                var connectionString = _configuration.GetConnectionString("SqlConnector");

                EnsureDatabase.For.SqlDatabase(connectionString);

                var dbUpgradeEngineBuilder = DeployChanges.To
                    .SqlDatabase(connectionString)
                    .WithScriptsEmbeddedInAssembly(typeof(Program).Assembly)
                    .WithTransaction()
                    .LogTo(_logger);
                    

                var dbUpgradeEngine = dbUpgradeEngineBuilder.Build();
                if (dbUpgradeEngine.IsUpgradeRequired())
                {
                    _logger.WriteInformation("Upgrades have been detected. Upgrading database now...");
                    var operation = dbUpgradeEngine.PerformUpgrade();
                    if (operation.Successful)
                    {
                        _logger.WriteInformation("Upgrade completed successfully");
                    }

                    _logger.WriteInformation("Error happened in the upgrade. Please check the logs");
                }

                return next;
            }
        }
    }