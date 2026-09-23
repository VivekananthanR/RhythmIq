using elr_API.DTO.ResDTO;

namespace elr_API.IService.IReport_template
{
    public interface IReportTemplateService
    {
        Task<ResponseDTO> ElrGetTemplateReportData(long patient_report_id);
        Task<byte[]> ElrGenerateReportPDF(long patient_report_id);
    }
}
