using elr_API.Common;
using elr_API.IService.IDapper;

namespace elr_API.Service.Background_services
{
    public class PatientReportService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public string ELR_db = CommonFunctions.GetConnectionString("elr_DB");

        public PatientReportService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {

                try
                {
                    using var scope = _serviceScopeFactory.CreateScope();

                    var db = scope.ServiceProvider.GetRequiredService<IDapperSqlService>();

                    // Create 24 Hours Report
                    await db.Execute(PatientReportDTO.create_24_hours_report(), null, ELR_db);

                    // Create Final Report
                    //await db.Execute(PatientReportDTO.create_final_report(), null, ELR_db);
                }
                catch (Exception ex)
                {
                    CommonFunctions.ILogging("PatientReportBackgroundService", ex.Message.ToString());
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }

    }
}
