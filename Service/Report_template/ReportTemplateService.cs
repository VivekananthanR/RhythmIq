using elr_API.Common;
using elr_API.DTO.ReqDTO;
using elr_API.DTO.ResDTO;
using elr_API.IService.IDapper;
using elr_API.IService.IReport_template;
using PuppeteerSharp;
using PuppeteerSharp.Media;
using System.Text.Json;

namespace elr_API.Service.Report_template
{
    public class ReportTemplateService : IReportTemplateService
    {
        private readonly IDapperSqlService _db;
        public string ErrorMessage = string.Empty;
        public string ELR_db = CommonFunctions.GetConnectionString("elr_DB");

        public ReportTemplateService(IDapperSqlService db)
        {
            _db = db;
        }

        public async Task<ResponseDTO> ElrGetTemplateReportData(long patient_report_id)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                if (patient_report_id == 0) return new ResponseDTO(false, ResponseMessage.Invalid_Details);

                query = PatientPdfDTO.get_patient_details();
                var patientData = (await _db.SelectQuery<PatientPdfDTO>(query, new { patient_report_id = patient_report_id }, ELR_db)).FirstOrDefault();

                if (patientData == null) return new ResponseDTO(false, ResponseMessage.Results_NotFound);

                query = EventSummaryPdfDTO.patient_evnet_count();
                long event_total_count = (await _db.SelectQuery<long>(query, new { patient_enrollment_id = patientData.patient_enrollment_id }, ELR_db)).FirstOrDefault();


                var eventSummary = new EventSummaryPdfDTO
                {
                    total_event_strip_count = event_total_count,
                    patient_event_count = 0,
                    auto_event_count = event_total_count
                };


                query = ArrhythmiaPdfDTO.get_arrhythmia_data();
                var result = (await _db.SelectQuery<ArrhythmiaResultDTO>(query, new { patient_report_id = patientData.patient_report_id }, ELR_db)).FirstOrDefault();


                var arrhythmiaData = new ArrhythmiaPdfDTO
                {
                    af = result != null && result.af > 0 ? "Found" : "None Found",

                    svt = result != null && result.svt > 0 ? "Found" : "None Found",

                    pause = result != null && result.pause > 0 ? "Found" : "None Found",

                    heart_block = result != null && result.heart_block > 0 ? "Found" : "None Found",

                    vt = result != null && result.vt > 0 ? "Found" : "None Found"
                };

                query = AF_BurdenDTO.get_af_burden_data();
                var af_Burden = (await _db.SelectQuery<AF_BurdenDTO>(query, new { patient_report_id = patient_report_id }, ELR_db)).FirstOrDefault();

                query = PVC_BurdenDTO.get_pvc_data();
                var pvc_Burden = (await _db.SelectQuery<PVC_BurdenDTO>(query, new { patient_report_id = patient_report_id }, ELR_db)).FirstOrDefault();

                query = PAC_BurdenDTO.get_pac_data();
                var pac_Burden = (await _db.SelectQuery<PAC_BurdenDTO>(query, new { patient_report_id = patient_report_id }, ELR_db)).FirstOrDefault();

                query = PauseDTO.get_pause_data();
                var pause_Burden = (await _db.SelectQuery<PauseDTO>(query, new { patient_report_id = patient_report_id }, ELR_db)).FirstOrDefault();

                query = ReportStripPdfDTO.get_strip_data();
                var stripList = (await _db.SelectQuery<ReportStripPdfDTO>(query, new { patient_report_id = patient_report_id }, ELR_db)).ToList();
                foreach (var strip in stripList)
                {
                    query = FindingsNameReportDTO.get_finding_data();

                    strip.finding_list = (await _db.SelectQuery<FindingsNameReportDTO>(query, new { sub_strip_event_id = strip.sub_strip_event_id }, ELR_db)).ToList();

                }

                query = ReportQerPdfDTO.get_qer_data();
                var qerList = (await _db.SelectQuery<ReportQerPdfDTO>(query, new { patient_report_id = patient_report_id }, ELR_db)).ToList();

                var reportData = new PatientReportPdfDTO
                {
                    patient_details = patientData,
                    event_summary = eventSummary,
                    arrhythmias_present = arrhythmiaData,
                    aF_Burden = af_Burden,
                    pvc = pvc_Burden,
                    pac = pac_Burden,
                    pause = pause_Burden,
                    strip_list = stripList,
                    qer_list = qerList,

                };

                return new ResponseDTO(reportData);

            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging("", ex.Message.ToString());
                ErrorMessage = ex.Message.ToString();
            }
            return new ResponseDTO(false, ErrorMessage);
        }


        public async Task<byte[]> ElrGenerateReportPDF(long patient_report_id)
        {
            try
            {
                var response = await ElrGetTemplateReportData(patient_report_id);

                if (response == null) throw new Exception("Report data not found.");

                var reportData = response.Data;

                string htmlTemplatePath = Path.Combine(AppContext.BaseDirectory, "Service", "Report_template", "report.html");

                if (!File.Exists(htmlTemplatePath)) throw new FileNotFoundException("Report HTML template not found.", htmlTemplatePath);

                string html = await File.ReadAllTextAsync(htmlTemplatePath);

                string jsonData = JsonSerializer.Serialize(reportData);
                //Console.WriteLine("json", jsonData);

                html = html.Replace("/*{{REPORT_DATA}}*/ null", jsonData);

                await new BrowserFetcher().DownloadAsync();

                await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });

                await using var page = await browser.NewPageAsync();

                await page.SetContentAsync(html, new SetContentOptions
                {
                    WaitUntil = new[]
                    {
                       WaitUntilNavigation.Load
                    }
                });

                var pdfBytes = await page.PdfDataAsync(new PdfOptions
                {
                    Format = PaperFormat.A4,
                    PrintBackground = true,
                    MarginOptions = new MarginOptions
                    {
                        Top = "8mm",
                        Bottom = "8mm",
                        Left = "8mm",
                        Right = "8mm"
                    }
                });

                return pdfBytes;

            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging("", ex.Message.ToString());
                ErrorMessage = ex.Message;
                throw;
            }




        }

    }
}
