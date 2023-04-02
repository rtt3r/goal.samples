namespace Goal.Samples.CQRS.Worker.Services
{
    public class ScopedBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ScopedBackgroundService> _logger;

        public ScopedBackgroundService(
            IServiceProvider serviceProvider,
            ILogger<ScopedBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(ScopedBackgroundService)} is working.");

            using IServiceScope scope = _serviceProvider.CreateScope();
            //IScopedProcessingService scopedProcessingService =
            //    scope.ServiceProvider.GetRequiredService<IScopedProcessingService>();

            //await scopedProcessingService.DoWorkAsync(stoppingToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                $"{nameof(ScopedBackgroundService)} is stopping.");

            await base.StopAsync(cancellationToken);
        }
    }
}