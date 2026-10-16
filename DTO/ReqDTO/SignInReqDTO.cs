using elr_API.Common;
using elr_API.DTO.ReqDTO;

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
        public static string existing_patient()
        {
            return @"
                SELECT TOP 1
                    patient_id,
                    phone_no,
                    email_id
                FROM tbl_patient
                WHERE is_active = 1
                  AND (phone_no = @phone_no OR email_id = @email_id)
                  AND (@patient_id = 0 OR patient_id <> @patient_id)";
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

    internal static string user_logIn()
    {
        return $@"update {Tbl.user_master} set is_logIn = 1 where user_id = @user_id and is_logIn = 0 and is_active = 1";
    }

    internal static string user_logOut()
    {
        return $@"update {Tbl.user_master} set is_logIn = 0 where user_id = @user_id and is_logIn = 1 and is_active = 1";
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
    public bool is_logIn { get; set; }
    public Guid? login_gu_id { get; set; }

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
                         where (@user_name is null or user_name = @user_name) and is_active =1  order by user_name asc";
    }

    internal static string select_physicians_list()
    {
        return $@"
                    select count(1) as totalCount from {Tbl.user_master} 
                        WHERE is_active = 1
                      AND user_type_id = 5;

                    SELECT
                        user_id,
                        user_name,
                        phone_no,
                        speciality,
                        is_active
                    FROM {Tbl.user_master}
                    WHERE is_active = 1
                      AND user_type_id = 5
                    ORDER BY user_name ASC";
    }
    internal static string select_technicians_list()
    {
        return $@"
                    select count(1) as totalCount from {Tbl.user_master}
                    where is_active = 1
                      AND user_type_id = 4;

                    SELECT
                        user_id,
                        user_name,
                        phone_no,
                        is_active
                    FROM {Tbl.user_master}
                    WHERE is_active = 1
                      AND user_type_id = 4
                    ORDER BY user_name ASC";
    }
    #endregion

}
public class PatientEcgSearchReqDTO
{
    public long patient_id { get; set; }
    public string patient_name { get; set; }
    public long patient_enrollment_id { get; set; }
    public bool active { get; set; }
    public bool historical { get; set; }
    public int pageNumber { get; set; }
    public int pageSize { get; set; }
    public int offSet => (pageNumber - 1) * pageSize;

    public string sortBy { get; set; }
    public string orderBy { get; set; }

    #region Query 
    internal static string selelct_pateint_details(PatientEcgSearchReqDTO obj)
    {
        return $@"
                SELECT COUNT(*)
                FROM {Tbl.patient_enrollment} AS a
                INNER JOIN {Tbl.patient} AS b
                    ON a.patient_id = b.patient_id
               WHERE a.is_active = 1
                AND (
                    a.patient_enrollment_id = @patient_enrollment_id
                    OR @patient_enrollment_id = 0
                )
                AND (
                    a.patient_id = @patient_id
                    OR @patient_id = 0
                )
                AND (
                    b.patient_name LIKE '%' + @patient_name + '%'
                    OR @patient_name IS NULL
                    OR @patient_name = ''
                )
                AND ( 
                            (@active = 1 and a.enrollment_status = 'monitoring' ) or
                            (@historical = 1 and a.enrollment_status = 'completed' ) or 
                            (@active = 0 AND @historical = 0)
                        );

                    SELECT
                        a.patient_enrollment_id,
                        a.patient_id,
                        a.referred_physician_id,
                        a.actual_hookup,
                        a.enrollment_status,
                        b.patient_name,
                        a.is_active,
                        e.duration_in_day,
                        c.user_name AS referred_physician
                    FROM {Tbl.patient_enrollment} AS a
                    INNER JOIN {Tbl.patient} AS b
                        ON a.patient_id = b.patient_id
                    INNER JOIN {Tbl.user_master} AS c
                        ON c.user_id = a.referred_physician_id
                    inner join {Tbl.test_prices} as d
                        on d.test_prices_id = a.estimated_test_prices_id
                        inner join {Tbl.test_mast} as e
                        on e.test_id = d.test_id
                    WHERE a.is_active = 1
                        AND (a.patient_enrollment_id = @patient_enrollment_id OR @patient_enrollment_id = 0)
                        AND (a.patient_id = @patient_id OR @patient_id = 0)
                        AND (
                            b.patient_name LIKE '%' + @patient_name + '%'
                            OR @patient_name IS NULL
                            OR @patient_name = ''
                        )
                    AND ( 
                            (@active = 1 and a.enrollment_status = 'monitoring' ) or
                            (@historical = 1 and a.enrollment_status = 'completed' ) or 
                            (@active = 0 AND @historical = 0)
                        )
                    ORDER BY {obj.sortBy} {obj.orderBy}
                    OFFSET @offset ROWS
                    FETCH NEXT @pageSize ROWS ONLY;";
    }
    #endregion

}


public class PatientReportReqDTO
{
    public long patient_report_id { get; set; }
    public long patient_id { get; set; }
    public string patient_name { get; set; }
    public DateTime? report_due_datetime { get; set; }
    public DateTime? report_started_datetime { get; set; }
    public string report_type_name { get; set; }
    public string referred_physician_name { get; set; }
    public int pageNumber { get; set; }
    public int pageSize { get; set; }
    public int offSet => (pageNumber - 1) * pageSize;
    public string sortBy { get; set; }
    public string orderBy { get; set; }
}

public class PatientReportDTO
{

    public long patient_report_id { get; set; }
    public long patient_enrollment_id { get; set; }
    public long patient_id { get; set; }
    public string patient_name { get; set; }
    public DateTime? actual_hookup { get; set; }
    public DateTime? report_due_datetime { get; set; }
    public DateTime? report_started_datetime { get; set; }
    public DateTime? report_generated_datetime { get; set; }
    public long generate_to { get; set; }
    public string report_type_name { get; set; }
    public string referred_physician_name { get; set; }

    public long user_id { get; set; }
    public string user_name { get; set; }

    public long device_id { get; set; }
    public string device_name { get; set; }
    public DateTime? report_finalized_datetime { get; set; }
    public long report_type_id { get; set; }



    public long report_assigned_id { get; set; }
    public DateTime? last_activity_time { get; set; }
    public string assign_status { get; set; }

    public long? report_assign_to { get; set; }

    public List<PatientReportGenerateStripDTO> report_strips { get; set; }




    #region Query
    internal static string current_report_assign_detatils()
    {
        return $@" SELECT a.report_assigned_id,a.patient_report_id,a.patient_enrollment_id,p.patient_id ,p.patient_name, b.report_type_id, b.report_started_datetime, b.report_due_datetime, a.assign_status,dm.device_id,dm.device_name
                   from {Tbl.report_assigned} as a
                       inner join {Tbl.patient_report} as b
                        on a.patient_report_id = b.patient_report_id
                          inner join {Tbl.patient_enrollment} as c
                        on a.patient_enrollment_id = c.patient_enrollment_id
                            inner join {Tbl.patient} as p
                        on c.patient_id = p.patient_id 
                            inner join {Tbl.device_mast} as dm
                        on dm.device_id = c.device_id
                       where  a.report_assigned_id = @report_assigned_id and a.is_active = 1 ";

    }
    internal static string select_assigned_report()
    {
        return $@"select report_assigned_id,last_activity_time,assign_status from {Tbl.report_assigned} where report_assigned_id =@report_assigned_id  order by report_assigned_id desc";
    }
    internal static string get_next_patient_report_list()
    {
        return $@" 
                SELECT
                pr.patient_report_id,
                pr.patient_enrollment_id,
                pr.report_type_id,
                pr.report_status,
                pr.report_due_datetime,
                pr.report_started_datetime,
                dm.device_id,
                dm.device_name
            FROM {Tbl.patient_report} pr
            inner join {Tbl.patient_enrollment} as pe
                on pe.patient_enrollment_id = pr.patient_enrollment_id and pe.is_active =1
            inner join {Tbl.device_mast} as dm
                on dm.device_id = pe.device_id
            WHERE pr.patient_report_id = @patient_report_id  AND pr.report_status = 'pending' and pr.report_started_datetime is not null
              AND pr.is_active = 1";
    }
    internal static string reset_patient_report_status()
    {
        return $@"
                UPDATE {Tbl.patient_report}
                SET
                    report_status = 'pending',
                    modified_by = @modified_by,
                    modified_datetime = GETDATE()
                WHERE patient_report_id =
                (
                    SELECT patient_report_id
                    FROM {Tbl.report_assigned}
                    WHERE report_assigned_id = @report_assigned_id
                )
                AND report_status = 'inprogress'
                AND is_active = 1";
    }

    internal static string close_patient_report_assgin()
    {
        return $@"update {Tbl.report_assigned}
                    set assign_status = 'pending',
                        modified_by = @modified_by,
                        modified_datetime = GETDATE()
                        where report_assigned_id = @report_assigned_id ";
    }


    internal static string update_report_inprogress()
    {
        return $@"
            UPDATE {Tbl.patient_report}
            SET
                report_status = 'inprogress',
                modified_by = @modified_by,
                modified_datetime = GETDATE()
            WHERE patient_report_id = @patient_report_id
              AND report_status = 'pending'
              AND is_active = 1;";
    }


    internal static string insert_report_assigned()
    {
        return $@"
            INSERT INTO {Tbl.report_assigned}
            (
                patient_report_id,
                patient_enrollment_id,
                assign_status,
                created_by
            )
            VALUES
            (
                @patient_report_id,
                @patient_enrollment_id,
                'processing',
                @created_by
            );";
    }
    internal static string get_next_report()
    {
        return $@"
            SELECT TOP 1
                pr.patient_report_id,
                pr.patient_enrollment_id,
                pr.report_type_id,
                pr.report_status,
                pr.report_due_datetime,
                pr.report_started_datetime
            FROM {Tbl.patient_report} pr
            WHERE pr.report_status = 'pending'
              AND pr.is_active = 1 and  pr.report_started_datetime is not null

              AND NOT EXISTS
              (
                  SELECT 1
                  FROM {Tbl.report_assigned} ra
                  WHERE ra.patient_report_id = pr.patient_report_id
                    AND ra.assign_status = 'processing'
                    AND ra.is_active = 1
              )

            ORDER BY
                pr.report_due_datetime ASC,
                pr.patient_report_id ASC";
    }


    internal static string get_current_report()
    {
        return $@"
                SELECT TOP 1
                    ra.report_assigned_id,
                    ra.patient_report_id,
                    ra.patient_enrollment_id,
                    ra.assign_status,
                    pr.report_type_id,
                    pr.report_status,
                    pr.report_due_datetime,
                    pr.report_started_datetime,
                    p.patient_id,
                    dm.device_id,
                    dm.device_name
                FROM {Tbl.report_assigned} ra
                INNER JOIN {Tbl.patient_report} pr
                    ON ra.patient_report_id = pr.patient_report_id
                inner join {Tbl.patient_enrollment} as pe
                    on pe.patient_enrollment_id = pr.patient_enrollment_id
                inner join {Tbl.patient} as p
                    on p.patient_id = pe.patient_id
                inner join {Tbl.device_mast} as dm
                    on dm.device_id = pe.device_id
                WHERE ra.created_by = @emp_id
                  AND ra.assign_status = 'processing'
                  AND ra.is_active = 1
                  AND pr.is_active = 1
                ORDER BY ra.report_assigned_id DESC";
    }

    internal static string update_patient_report_status()
    {
        return $@"
            UPDATE {Tbl.patient_report}
            SET report_status = 'inprogress',
                generate_to = @created_by,
                report_generated_datetime = GETDATE()
            WHERE patient_report_id = @patient_report_id
              AND is_active = 1";
    }
    internal static string update_assign_processing_status()
    {
        return $@"
            UPDATE {Tbl.report_assigned}
            SET assign_status = 'generated',
                 modified_by = @modified_by,
                modified_datetime = GETDATE()
            WHERE report_assigned_id = @report_assigned_id
              AND is_active = 1 and assign_status = 'processing'";
    }

    internal static string get_patient_report_list(PatientReportReqDTO obj)
    {
        string sortBy;

        if (string.IsNullOrEmpty(obj.sortBy))
        {
            sortBy = "pr.created_datetime";
        }
        else if (obj.sortBy == "patient_id")
        {
            sortBy = "p.patient_id";
        }
        else if (obj.sortBy == "patient_name")
        {
            sortBy = "p.patient_name";
        }
        else if (obj.sortBy == "report_due_datetime")
        {
            sortBy = "pr.report_due_datetime";
        }
        else if (obj.sortBy == "report_type_name")
        {
            sortBy = "rt.report_type_name";
        }
        else if (obj.sortBy == "referred_physician_name")
        {
            sortBy = "c.user_name";
        }
        else
        {
            sortBy = "pr.created_datetime";
        }

        string orderBy = string.IsNullOrEmpty(obj.orderBy) ? "DESC" : (obj.orderBy.ToUpper() == "ASC" ? "ASC" : "DESC");


        return $@"
                SELECT COUNT(*) as totalCount
                FROM {Tbl.patient_report} AS pr
                INNER JOIN {Tbl.patient_enrollment} AS pe
                    ON pr.patient_enrollment_id = pe.patient_enrollment_id 
                INNER JOIN {Tbl.patient} AS p
                    ON pe.patient_id = p.patient_id
                INNER JOIN {Tbl.report_type} AS rt
                    ON pr.report_type_id = rt.report_type_id
                LEFT JOIN {Tbl.user_master} AS c
                    ON pe.referred_physician_id = c.user_id
                LEFT JOIN {Tbl.device_mast} AS dm
                    ON pe.device_id = dm.device_id
                WHERE pr.is_active = 1 and pr.report_started_datetime is not null
                  AND pe.is_active = 1 and  pr.report_status NOT IN ('completed', 'inprogress');


                SELECT
                    pr.patient_report_id,
                    pr.report_status,
                    p.patient_id,
                    p.patient_name,
                    dm.device_id,
                    dm.device_name,
                    pr.generate_to,
                    pr.report_due_datetime,
                    pr.report_started_datetime,
                    pr.report_generated_datetime,
                    pe.patient_enrollment_id,
                    rt.report_type_name,
                    c.user_name AS referred_physician_name
                FROM {Tbl.patient_report} AS pr 
                INNER JOIN {Tbl.patient_enrollment} AS pe
                    ON pr.patient_enrollment_id = pe.patient_enrollment_id 
                INNER JOIN {Tbl.patient} AS p
                    ON pe.patient_id = p.patient_id
                INNER JOIN {Tbl.report_type} AS rt
                    ON pr.report_type_id = rt.report_type_id
                LEFT JOIN {Tbl.user_master} AS c
                    ON pe.referred_physician_id = c.user_id
                LEFT JOIN {Tbl.device_mast} AS dm
                    ON pe.device_id = dm.device_id
                WHERE pr.is_active = 1 
                  AND pe.is_active = 1 and pr.report_status NOT IN ('completed', 'inprogress')
                ORDER BY {sortBy} {orderBy}
                OFFSET @offset ROWS
                FETCH NEXT @pageSize ROWS ONLY;";
    }

    internal static string create_24_hours_report()
    {
        return $@"
        INSERT INTO {Tbl.patient_report}
        (
            patient_enrollment_id,
            report_type_id,
            report_status,
            report_due_datetime,
            report_started_datetime,
            created_datetime,
            is_active
        )
        SELECT
            pe.patient_enrollment_id,
            rt.report_type_id,
            'pending',

            DATEADD(
                HOUR,
                24,
                pe.actual_hookup
            ) AS report_due_datetime,

            pe.actual_hookup AS report_started_datetime,

            GETDATE(),
            1

        FROM {Tbl.patient_enrollment} pe

        INNER JOIN {Tbl.test_prices} tp
            ON tp.test_prices_id = pe.estimated_test_prices_id
            AND tp.is_active = 1

        INNER JOIN {Tbl.test_mast} tm
            ON tm.test_id = tp.test_id
            AND tm.is_active = 1

        INNER JOIN {Tbl.report_type} rt
            ON rt.report_type_id = 1
            AND rt.is_active = 1

        WHERE pe.actual_hookup IS NOT NULL

          -- 24 Hours Report only for tests more than 1 day
          AND tm.duration_in_day > 1

          -- 24 hours completed
          AND DATEADD(
                HOUR,
                24,
                pe.actual_hookup
              ) <= GETDATE()

          AND pe.is_active = 1

          AND NOT EXISTS
          (
              SELECT 1
              FROM {Tbl.patient_report} pr
              WHERE pr.patient_enrollment_id = pe.patient_enrollment_id
                AND pr.report_type_id = rt.report_type_id
                AND pr.is_active = 1
          );
    ";
    }
    internal static string create_final_report()
    {
        return $@"
        INSERT INTO {Tbl.patient_report}
        (
            patient_enrollment_id,
            report_type_id,
            report_status,
            report_due_datetime,
            report_started_datetime,
            created_datetime,
            is_active
        )
        SELECT
            pe.patient_enrollment_id,
            rt.report_type_id,
            'pending',

            DATEADD(
                DAY,
                tm.duration_in_day,
                pe.actual_hookup
            ),

            pe.actual_hookup,

            GETDATE(),
            1

        FROM {Tbl.patient_enrollment} pe

        INNER JOIN {Tbl.test_prices} tp
            ON tp.test_prices_id = pe.estimated_test_prices_id
            AND tp.is_active = 1

        INNER JOIN {Tbl.test_mast} tm
            ON tm.test_id = tp.test_id
            AND tm.is_active = 1

        INNER JOIN {Tbl.report_type} rt
            ON rt.report_type_id = 2
            AND rt.is_active = 1

        WHERE pe.actual_hookup IS NOT NULL
          AND tm.duration_in_day >= 1

          -- Full test duration completed
          AND DATEADD(
                DAY,
                tm.duration_in_day,
                pe.actual_hookup
              ) <= GETDATE()

          AND pe.is_active = 1

          -- Enrollment must have at least one event
          AND EXISTS
          (
              SELECT 1
              FROM {Tbl.queued_events_records} q
              WHERE q.patient_enrollment_id = pe.patient_enrollment_id
          )

          -- ALL events must be completed
          AND NOT EXISTS
          (
              SELECT 1
              FROM {Tbl.queued_events_records} q
              WHERE q.patient_enrollment_id = pe.patient_enrollment_id
                AND
                (
                    q.event_status <> 'completed'
                    OR q.event_status IS NULL
                    OR q.completed_by IS NULL
                    OR q.completed_datetime IS NULL
                )
          )

          -- Don't create duplicate Final Report
          AND NOT EXISTS
          (
              SELECT 1
              FROM {Tbl.patient_report} pr
              WHERE pr.patient_enrollment_id = pe.patient_enrollment_id
                AND pr.report_type_id = 2
                --AND pr.is_active = 1
          );
    ";
    }

    #endregion
}



public class PatientEcgSearechResDTO
{
    public long patient_id { get; set; }
    public string patient_name { get; set; }
    public long patient_enrollment_id { get; set; }
    public DateTime? created_datetime { get; set; }
    public DateTime? actual_hookup { get; set; }
    public string enrollment_status { get; set; }
    public int duration_in_day { get; set; }
    public long referred_physician_id { get; set; }
    public string referred_physician { get; set; }
    public bool is_active { get; set; }



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


public class EventProcessTrackingDTO
{
    public long Taken_by { get; set; }
    public string Taken_by_name { get; set; }
    public DateTime? Taken_datetime { get; set; }
    public string work_status { get; set; }
    public string event_status { get; set; }
    public long queued_events_records_id { get; set; }





    #region Query 
    internal static string select_event_process_tracking()
    {
        return $@"
                  select count(1) as totalCount from tbl_event_assign as a
                    inner join tbl_queued_events_records as b
                    on a.queued_events_records_id = b.queued_events_records_id
                    where a.work_status = 'inprogress' and b.event_status = 'processing' and a.is_active = 1 and b.is_active = 1 ;

                   select b.modified_by as Taken_by,c.user_name as Taken_by_name,a.work_status,b.modified_datetime as Taken_datetime,a.queued_events_records_id,b.event_status
                    from tbl_event_assign as a
                    inner join tbl_queued_events_records as b
                    on a.queued_events_records_id = b.queued_events_records_id
                    left join tbl_user_mast as c
                    on c.user_id = b.modified_by
                    where a.work_status = 'inprogress' and b.event_status = 'processing' and a.is_active = 1 and b.is_active = 1 ";
    }
    #endregion
}