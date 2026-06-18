using elr_API.Common;
using elr_API.DTO.ReqDTO;
using elr_API.IService.IDapper;

namespace elr_API.Service.Background_services
{
    public class EventReassignService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public string ELR_db = CommonFunctions.GetConnectionString("elr_db");

        public EventReassignService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceScopeFactory.CreateScope();

                var db = scope.ServiceProvider.GetRequiredService<IDapperSqlService>();
                await db.Execute(EventAssignDTO.reassign_idle_events(), null, ELR_db);
                await db.Execute(EventAssignDTO.reassign_expired_events(), null, ELR_db);
                await db.Execute(EventAssignDTO.reset_reassigned_events(), null, ELR_db);

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}
