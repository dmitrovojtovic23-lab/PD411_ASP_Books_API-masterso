using PD411_Books.DAL.Repositories;

namespace PD411_Books.BLL.Services
{
    public class TokenCleanupService : BackgroundService
    {
        private readonly ILogger<TokenCleanupService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public TokenCleanupService(ILogger<TokenCleanupService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Token Cleanup Service is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Running token cleanup task.");

                using (var scope = _serviceProvider.CreateScope())
                {
                    var refreshTokenRepository = scope.ServiceProvider.GetRequiredService<RefreshTokenRepository>();
                    
                    try
                    {
                        var deletedCount = await refreshTokenRepository.DeleteExpiredTokensAsync();
                        _logger.LogInformation("Deleted {Count} expired refresh tokens.", deletedCount);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error occurred while cleaning up expired tokens.");
                    }
                }

                await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
            }

            _logger.LogInformation("Token Cleanup Service is stopping.");
        }
    }
}
