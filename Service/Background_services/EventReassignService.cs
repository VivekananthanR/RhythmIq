using elr_API.Common;
using elr_API.DTO.ReqDTO;
using elr_API.IService.IDapper;
using elr_API.IService.IEntrolement;

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

            try
            {
                using var scope = _serviceScopeFactory.CreateScope();

                var db = scope.ServiceProvider.GetRequiredService<IDapperSqlService>();

                await db.Execute(EventAssignDTO.reassign_expired_events(), null, ELR_db);
                await db.Execute(EventAssignDTO.reset_reassigned_events(), null, ELR_db);

                var service = scope.ServiceProvider.GetRequiredService<IEnrollment>();

                await service.ElrPhythonProcessEnrollmentRawData();
            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging("BackgroundService", ex.ToString());
            }

            await Task.Delay(TimeSpan.FromSeconds(15), stoppingToken);
        }
    }
}
