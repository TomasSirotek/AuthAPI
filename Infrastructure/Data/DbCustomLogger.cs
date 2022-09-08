using DbUp.Engine.Output;

namespace AuthAPI.Infrastructure.Data; 

    public class DbCustomLogger<T> : IUpgradeLog where T : class
    {
        private readonly ILogger<T> _logger;

        public DbCustomLogger(ILogger<T> logger)
        {
            _logger = logger;
        }

        public void WriteInformation(string format, params object[] args)
        {
            _logger.LogInformation(format, args);
        }

        public void WriteError(string format, params object[] args)
        {
            _logger.LogError(format, args);
        }

        public void WriteWarning(string format, params object[] args)
        {
            _logger.LogWarning(format, args);
        }
    }