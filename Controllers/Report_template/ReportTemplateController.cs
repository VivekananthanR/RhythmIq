using elr_API.DTO.ResDTO;
using elr_API.IService.IReport_template;
using Microsoft.AspNetCore.Mvc;

namespace elr_API.Controllers.Report_template
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportTemplateController : ControllerBase
    {
        private readonly IReportTemplateService _reportTemplateService;


        public ReportTemplateController(IReportTemplateService reportTemplateService)
        {
            _reportTemplateService = reportTemplateService;
        }

        #region HttpGet = api/ReportTemplate/ElrGetTemplateReportData?patient_report_id=

        [HttpGet("ElrGetTemplateReportData")]
        public async Task<ActionResult<ResponseDTO>> ElrGetTemplateReportData(long patient_report_id)
        {
            return await _reportTemplateService.ElrGetTemplateReportData(patient_report_id);
        }
        #endregion


        #region HttpGet =  api/ReportTemplate/ElrGenerateReportPDF

        [HttpGet("ElrGenerateReportPDF")]
        public async Task<ActionResult> ElrGenerateReportPDF(long patient_report_id)
        {
            var pdfBytes = await _reportTemplateService.ElrGenerateReportPDF(patient_report_id);

            return File(pdfBytes, "application/pdf");
        }
        #endregion


    }
}
