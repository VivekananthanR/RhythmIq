using mct_API.Common;
using mct_API.DTO.ReqDTO;
using mct_API.DTO.ResDTO;
using mct_API.IService.ICommon_manager;
using mct_API.IService.IDapper;
using mct_API.IService.IToken;
using mct_API.IService.IUser_manager;

namespace mct_API.Service.User_manager
{
    public class UserService : IUserService
    {
        private readonly IDapperSqlService _dp;
        private readonly ITokenService _tokenService;
        private readonly IHelperService _helperService;

        public string Mct_db = CommonFunctions.GetConnectionString("mct_db");
        public string ErrorMessage { get; set; } = string.Empty;

        public UserService(IDapperSqlService db, ITokenService tokenService, IHelperService helperService)
        {
            _dp = db;
            _tokenService = tokenService;
            _helperService = helperService;
        }

        public async Task<ResponseDTO> MasterEmployeeLogin(MasterEmployeeLoginReqDTO obj)
        {
            ErrorMessage = string.Empty;
            string query = string.Empty;
            try
            {
                if (!Validation.IsValidPhoneNumber(obj.phone_no) && !Validation.IsValidEmail(obj.phone_no)) return new ResponseDTO(false, ResponseMessage.Invalid_Details);

                MasterEmployeeLoginResDTO data = await GetEmployeeDetails(obj);

                if (data == null) return new ResponseDTO(false, ResponseMessage.Invalid_email_id);

                if (obj.login_with_otp)
                {
                    if (obj.otp == 0)
                    {
                        obj.otp = CommonFunctions.GenerateOTP(6);

                    }
                }
                else
                {
                    #region Sync Employee Details from Meterbox

                    var meterboxData = await _helperService.GetEmployeeDetails(data.emp_code);

                    if (meterboxData == null) return new ResponseDTO(false, "Employee Not Found In Meterbox");

                    if (meterboxData.m_Status != "Active") return new ResponseDTO(false, ResponseMessage.Access_Denied);

                    if (meterboxData.m_Password != data.password || meterboxData.m_Email != data.email_id)
                    {
                        query = $@"Update {Tbl.user_master}
                                    Set password = @password, email_id = @email_id
                                    Where emp_code = @emp_code";
                        await _dp.Execute(query, new { password = meterboxData.m_Password, email_id = meterboxData.m_Email, emp_code = data.emp_code }, Mct_db);

                        data.password = meterboxData.m_Password;
                    }

                    #endregion

                    if (obj.password == data.password)
                    {
                        return new ResponseDTO(data);
                    }
                    else
                    {
                        return new ResponseDTO(false, "Invalid Password");
                    }
                }

            }
            catch (Exception ex)
            {
                CommonFunctions.ILogging(obj, ToString());
                ErrorMessage = ResponseMessage.Please_Try_Again;
            }
            return new ResponseDTO(false, ErrorMessage);
        }

        public async Task<MasterEmployeeLoginResDTO> GetEmployeeDetails(MasterEmployeeLoginReqDTO obj)
        {
            string query = MasterEmployeeLoginReqDTO.employee_master_select();
            MasterEmployeeLoginResDTO data = (await _dp.SelectQuery<MasterEmployeeLoginResDTO>(query, obj, Mct_db)).FirstOrDefault();
            if (data != null)
            {
                var tokenDetails = _tokenService.GetToken(data.user_id.ToString(), data.gu_id.ToString(), data.phone_no, CommonFunctions.MCT);
                data.token = tokenDetails.Token;
                data.token_expirytime = tokenDetails.Token_ExpiryTime;
            }
            return data;
        }



    }

    public class EmployeeMeterboxDetails
    {
        public string m_StaffID { get; set; }
        public string m_FName { get; set; }
        public string m_Designation { get; set; }
        public string m_Email { get; set; }
        public string m_Password { get; set; }
        public string m_Mobile { get; set; }
        public string m_Status { get; set; }
        public string m_Team { get; set; }
        public string m_Base { get; set; }
        public DateTime? m_DOB { get; set; }
    }

    public class ApiResponse
    {
        public bool status { get; set; }
        public string errorCode { get; set; }
        public List<EmployeeMeterboxDetails> data { get; set; }
    }
}
