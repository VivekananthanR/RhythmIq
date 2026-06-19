using elr_API.Common;

namespace elr_API.DTO.ReqDTO
{
    public class SignInReqDTO
    {
    }
    public class PatientVerifyLoginReqDTO
    {
        public string phone_no { get; set; }
        public int otp { get; set; }

        #region Query
        internal static string existing_select_patient(PatientVerifyLoginReqDTO obj)
        {
            string query = $@"Select top 1 p.patient_id, p.patient_name, p.dob, p.age, p.phone_no, p.emergency_phone_no, p.gender, p.email_id, p.city, p.state_id, p.country_id, p.district_id, p.pincode , p.is_active, c.country_name, s.state_name, d.district_name
                from {Tbl.patient} as p
                left join {Tbl.country} as c on p.country_id = c.country_id
                left join {Tbl.state} as s on p.state_id = s.state_id
                left join {Tbl.district} as d on p.district_id = d.district_id
                where phone_no = {obj.phone_no} and p.is_active = 1";

            if (obj.otp != 0)
            {
                query += $" AND otp={obj.otp}";
            }
            return query;

        }
        internal static string Send_otp_update(PatientVerifyLoginResDTO obj)
        {
            return $@"UPDATE {Tbl.patient} SET otp = {obj.otp}, expiry_date = DATEADD(MINUTE, 10, GETDATE())  WHERE phone_no = {obj.phone_no} and is_active = 1";
        }
        #endregion
    }

    public class PatientVerifyLoginResDTO
    {
        public int patient_id { get; set; }
        public string patient_name { get; set; }
        public DateTime dob { get; set; }
        public int age { get; set; }
        public string phone_no { get; set; }
        public string emergency_phone_no { get; set; }
        public string email_id { get; set; }
        public string gender { get; set; }
        public string city { get; set; }
        public int otp { get; set; }
        public DateTime? expiry_date { get; set; }
        public string pincode { get; set; }

        public long district_id { get; set; }
        public string district_name { get; set; }
        public long state_id { get; set; }
        public string state_name { get; set; }
        public long country_id { get; set; }
        public string country_name { get; set; }

        public long created_by { get; set; }
        public DateTime? created_datetime { get; set; }
        public long modified_by { get; set; }
        public DateTime? modified_datetime { get; set; }
        public bool is_active { get; set; }


        internal static string patient_detatils()
        {
            return $@"select p.patient_id, p.patient_name, p.dob, p.age, p.phone_no, p.emergency_phone_no, p.email_id, p.gender, p.city, p.pincode, p.district_id, p.state_id,p.country_id, p.created_by, p.created_datetime, p.is_active 
                        d.district_name,c.country_name, s.state_name
                        from {Tbl.patient} as p
                        left join {Tbl.district} as d
                        on p.district_id = d.district_id
                        left join {Tbl.state} as s
                        on p.state_id = s.state_id
                        left join {Tbl.country} as c
                        on p.country_id = c.country_id
                        where p.is_active = 1 and  p.patient_id = @patient_id";
        }

    }
    public class MasterEmployeeLoginReqDTO
    {

        public string user_name { get; set; }
        public string password { get; set; }
        //public string version { get; set; }
        //public string serivce { get; set; }


        #region Query
        internal static string employee_master_select()
        {
            return $@"Select * from {Tbl.user_master}  where (phone_no = @user_name OR email_id = @user_name OR emp_code = @user_name) and is_active = 1";
        }
        #endregion

    }
    public class MasterEmployeeLoginResDTO
    {
        public long user_id { get; set; }
        public Guid? gu_id { get; set; }
        public string user_name { get; set; }
        public string emp_code { get; set; }
        public string password { get; set; }
        public string phone_no { get; set; }
        public string email_id { get; set; }
        public int otp { get; set; }
        public DateTime? otp_expiry_time { get; set; }
        public bool is_active { get; set; }

        public long user_type_id { get; set; }
        public string user_type { get; set; }


        public string token { get; set; }
        public int token_expirytime { get; set; }
    }

    public class AddUpdateEmployeeDetailsReqDTO
    {
        public long user_id { get; set; }
        public Guid? gu_id { get; set; }
        public string user_name { get; set; }
        public string emp_code { get; set; }
        public string password { get; set; }
        public string phone_no { get; set; }
        public string email_id { get; set; }
        public string alter_ph_no { get; set; }
        public DateTime? date_of_birth { get; set; }
        public string gender { get; set; }

        public long user_type_id { get; set; }
        public long? hospital_id { get; set; }
        public long? location_id { get; set; }
        public long? state_id { get; set; }
        public long? country_id { get; set; }
        public long? district_id { get; set; }
        public string city { get; set; }
        public string? speciality { get; set; }
        public long? created_by { get; set; }
        public DateTime? created_datetime { get; set; }
        public long? modified_by { get; set; }
        public DateTime? modified_datetime { get; set; }
        public bool is_active { get; set; }

        #region Query

        internal static string employee_details_select_validation()
        {
            return $@" select phone_no, email_id, emp_code 
                       from {Tbl.user_master}
                       where is_active =1      
                          AND (
                                (@phone_no IS NOT NULL AND @email_id IS NOT NULL AND phone_no = @phone_no AND email_id = @email_id)
                                OR
                                (@phone_no IS NOT NULL AND @email_id IS NULL AND phone_no = @phone_no)
                                OR
                                (@phone_no IS NULL AND @email_id IS NOT NULL AND  email_id = @email_id)
                              )
                          AND (@user_id = 0 OR user_id <> @user_id)";
        }

        internal static string employee_details_insert()
        {
            return $@"insert into {Tbl.user_master} 
                      (user_name, emp_code, password, phone_no, email_id, alter_ph_no, date_of_birth, gender, user_type_id, hospital_id,speciality, location_id, state_id, country_id, district_id, city, created_by)
                      Values (@user_name, @emp_code, @password, @phone_no, @email_id, @alter_ph_no, @date_of_birth, @gender, @user_type_id, @hospital_id,@speciality, @location_id, @state_id, @country_id, @district_id, @city, @created_by)";
        }

        internal static string employee_details_update()
        {
            return $@"Update {Tbl.user_master}
                       Set user_name = @user_name, emp_code = @emp_code, password = @password, phone_no = @phone_no, email_id = @email_id, alter_ph_no = @alter_ph_no, date_of_birth = @date_of_birth, gender = @gender,user_type_id=@user_type_id, location_id = @location_id,
                          hospital_id =@hospital_id,speciality =@speciality, state_id = @state_id, country_id = @country_id, district_id = @district_id, city = @city, modified_by = @modified_by, modified_datetime = getdate(), is_active = @is_active
                        where user_id = @user_id";
        }

        internal static string select_employee_name_list()
        {
            return $@"select user_id,user_name,phone_no,email_id,emp_code,is_active
                        from {Tbl.user_master}
                         where (@user_name is null or user_name = @user_name)";
        }

        internal static string select_physicians_list()
        {
            return $@"
                    SELECT
                        user_id,
                        user_name,
                        phone_no,
                        speciality,
                        is_active
                    FROM {Tbl.user_master}
                    WHERE is_active = 1
                      AND user_type_id = 2
                      AND (@user_name IS NULL OR user_name LIKE '%' + @user_name + '%')
                    ORDER BY user_name ASC";
        }
        internal static string select_technicians_list()
        {
            return $@"
                    SELECT
                        user_id,
                        user_name,
                        phone_no,
                        is_active
                    FROM {Tbl.user_master}
                    WHERE is_active = 1
                      AND user_type_id = 1
                      AND (@user_name IS NULL OR user_name LIKE '%' + @user_name + '%')
                    ORDER BY user_name ASC";
        }
        #endregion

    }


    public class PhysiciansListResDTO
    {
        public long user_id { get; set; }
        public string user_name { get; set; }
        public string phone_no { get; set; }
        public string speciality { get; set; }
        public long hospital_id { get; set; }
        public string hospital_name { get; set; }
    }
}