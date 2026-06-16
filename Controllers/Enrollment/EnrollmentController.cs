using elr_API.DTO.ReqDTO;
using elr_API.DTO.ResDTO;
using elr_API.IService.IEntrolement;
using Microsoft.AspNetCore.Mvc;

namespace elr_API.Controllers.Entrolement
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentController : ControllerBase
    {
        private readonly IEnrollment _enrollment;

        public EnrollmentController(IEnrollment enrollment)
        {
            _enrollment = enrollment;
        }

        #region HttpGet = api/Enrollment/ElrGetValidEnrollmentId?patient_enrollment_id=
        [HttpGet("ElrGetValidEnrollmentId")]
        public async Task<ActionResult<ResponseDTO>> ElrGetValidEnrollmentId(long patient_enrollment_id)
        {
            return await _enrollment.ElrGetValidEnrollmentId(patient_enrollment_id);
        }
        #endregion

        #region HttpGet = api/Enrollment/ElrValidDeviceId?device_no =
        [HttpGet("ElrValidDeviceId")]
        public async Task<ActionResult<ResponseDTO>> ElrValidDeviceId(string device_no)
        {
            return await _enrollment.ElrValidDeviceId(device_no);
        }
        #endregion

        #region HttpGet = api/Enrollment/ElrValidPatchId?patch_no =
        [HttpGet("ElrValidPatchId")]
        public async Task<ActionResult<ResponseDTO>> ElrValidPatchId(string patch_no)
        {
            return await _enrollment.ElrValidPatchId(patch_no);
        }
        #endregion

        #region HttpPost = api/Home/ElrUploadPatientDevice
        [HttpPost("ElrUploadPatientDevice")]
        public async Task<ActionResult<ResponseDTO>> ElrUploadPatientDevice([FromForm] PatientEnrollmentDTO obj)
        {
            return await _enrollment.ElrUploadPatientDevice(obj);
        }
        #endregion

        #region HttpPost = api/Home/ElrUploadSignature
        [HttpPost("ElrUploadSignature")]
        public async Task<ActionResult<ResponseDTO>> ElrUploadSignature([FromForm] PatientEnrollmentDTO obj)
        {
            return await _enrollment.ElrUploadSignature(obj);
        }
        #endregion

        #region HttpPost = api/Home/ElrStartHookUpPatient?patient_enrollment_id=
        [HttpPost("ElrStartHookUpPatient")]
        public async Task<ActionResult<ResponseDTO>> ElrStartHookUpPatient(long patient_enrollment_id)
        {
            return await _enrollment.ElrStartHookUpPatient(patient_enrollment_id);
        }
        #endregion

        #region HttpPost = api/Home/ElrCompleteHookUpPatient?patient_enrollment_id=
        [HttpPost("ElrCompleteHookUpPatient")]
        public async Task<ActionResult<ResponseDTO>> ElrCompleteHookUpPatient(long patient_enrollment_id)
        {
            return await _enrollment.ElrCompleteHookUpPatient(patient_enrollment_id);
        }
        #endregion


        #region HttpPost = api/Home/GetPatientEnrollment
        [HttpPost("GetPatientEnrollment")]
        public async Task<ActionResult<ResponseDTO>> GetPatientEnrollment(PatientEnrollmentDTO obj)
        {
            return await _enrollment.GetPatientEnrollment(obj);
        }
        #endregion

        #region HttpPost = api/Home/ElrAddUpdatePatientEnrollment
        [HttpPost("ElrAddUpdatePatientEnrollment")]
        public async Task<ActionResult<ResponseDTO>> ElrAddUpdatePatientEnrollment(PatientEnrollmentDTO obj)
        {
            return await _enrollment.ElrAddUpdatePatientEnrollment(obj);
        }
        #endregion

        #region HttpPost = api/Enrollment/ElrUploadEnrollmentRawData​
        [HttpPost("ElrUploadEnrollmentRawData")]
        public async Task<ActionResult<ResponseDTO>> ElrUploadEnrollmentRawData([FromForm] EnrollmentRawDataDTO obj)
        {
            //TokenClaims tokenClaims = Validation.ClaimValues(Authorization);
            //obj.created_by = tokenClaims.id;
            return await _enrollment.ElrUploadEnrollmentRawData(obj);
        }
        #endregion

        #region HttpGet = api/Enrollment/ElrGetRawDataList
        [HttpGet("ElrGetRawDataList")]
        public async Task<ActionResult<ResponseDTO>> ElrGetRawDataList()
        {
            return await _enrollment.ElrGetRawDataList();
        }
        #endregion

    }
}

