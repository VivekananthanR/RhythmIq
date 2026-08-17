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
        internal static string existing_select_patient()
        {
            return $@"
                    SELECT TOP 1
                        p.patient_id,
                        p.patient_name,
                        p.dob,
                        p.age,
                        p.phone_no,
                        p.emergency_phone_no,
                        p.gender,
                        p.email_id,
                        p.city,
                        p.state_id,
                        p.country_id,
                        p.district_id,
                        p.pincode,
                        p.is_active
                    FROM {Tbl.patient} p
                    WHERE p.phone_no = @phone_no
                      AND p.is_active = 1";
        }
        internal static string Send_otp_update()
        {
            return $@"
                    UPDATE {Tbl.patient}
                    SET otp = @otp,
                        expiry_date = DATEADD(MINUTE,10,GETDATE())
                    WHERE phone_no = @phone_no
                      AND is_active = 1";
        }
        internal static string otp_verify_select()
        {
            return $@"
                SELECT TOP 1
                    p.patient_id,
                    p.patient_name,
                    p.dob,
                    p.age,
                    p.phone_no,
                    p.emergency_phone_no,
                    p.gender,
                    p.email_id,
                    p.city,
                    p.state_id,
                    p.country_id,
                    p.district_id,
                    p.pincode,
                    p.otp,
                    p.expiry_date,
                    p.is_active,
                    c.country_name,
                    s.state_name,
                    d.district_name, 
                    p.created_by,
                    p.created_datetime,
                    p.modified_by,
                    p.modified_datetime
                FROM {Tbl.patient} AS p
                LEFT JOIN {Tbl.country} AS c
                    ON c.country_id = p.country_id
                LEFT JOIN {Tbl.state} AS s
                    ON s.state_id = p.state_id
                LEFT JOIN {Tbl.district} AS d
                    ON d.district_id = p.district_id
                WHERE p.phone_no = @phone_no
                  AND p.otp = @otp
                  AND p.expiry_date >= GETDATE()
                  AND p.is_active = 1";
        }
    }
        #endregion
}

public class PatientVerifyLoginResDTO
{
    public long patient_id { get; set; }
    public string patient_name { get; set; }
    public DateTime? dob { get; set; }
    public int age { get; set; }
    public string phone_no { get; set; }
    public string emergency_phone_no { get; set; }
    public string email_id { get; set; }
    public string gender { get; set; }
    public string city { get; set; }
    public int? otp { get; set; }
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

    #region Query

    internal static string insert_patient_details()
    {
        return $@"Insert Into {Tbl.patient} (patient_name,dob,age,phone_no,emergency_phone_no,email_id,gender,state_id,country_id,district_id,city,pincode,created_by)
                        Values (@patient_name, @dob, @age, @phone_no, @emergency_phone_no ,@email_id ,@gender, @state_id ,@country_id, @district_id,@city ,@pincode,@created_by)";
    }

    internal static string update_patient_details()
    {
        return $@"Update {Tbl.patient}
                      Set patient_name =@patient_name,dob = @dob,age = @age,phone_no = @phone_no,emergency_phone_no = @emergency_phone_no, email_id = @email_id,
                             gender= @gender,state_id =@state_id,country_id=@country_id, modified_by =@modified_by,modified_datetime = getdate(),
                            district_id = @district_id,city = @city,pincode=@pincode
                      where patient_id = @patient_id";

    }
    internal static string patient_details_select()
    {
        return $@"select p.patient_id, p.patient_name as patient_fullname, p.dob as patient_dob, p.age as patient_age, p.phone_no as patient_mobileno, p.emergency_phone_no as emergency_phone_no, p.email_id as patient_emailid, p.gender as patient_gender, p.city as patient_cityname, p.pincode as patient_pincode, p.district_id, p.state_id,p.country_id, p.created_by, p.created_datetime, p.is_active ,
                        d.district_name,c.country_name, s.state_name
                        from {Tbl.patient} as p
                        left join {Tbl.district} as d
                        on p.district_id = d.district_id
                        left join {Tbl.state} as s
                        on p.state_id = s.state_id
                        left join {Tbl.country} as c
                        on p.country_id = c.country_id
                        where p.is_active = 1 and p.phone_no = @phone_no";
    }

    internal static string patient_detatils()
    {
        return $@"select p.patient_id, p.patient_name, p.dob, p.age, p.phone_no, p.emergency_phone_no, p.email_id, p.gender, p.city, p.pincode, p.district_id, p.state_id,p.country_id, p.created_by, p.created_datetime, p.is_active ,
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
    #endregion
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
