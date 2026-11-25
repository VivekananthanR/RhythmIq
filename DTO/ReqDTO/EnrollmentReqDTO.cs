using elr_API.Common;

namespace elr_API.DTO.ReqDTO
{
    public class EnrollmentReqDTO
    {
    }

    public class PatientEnrollmentReqDTO
    {
        public long patient_enrollment_id { get; set; }
        public Guid? gu_id { get; set; }
        public long user_id { get; set; }
        public long patient_id { get; set; }
        public long device_id { get; set; }
        public long patch_id { get; set; }
        public long referred_physician { get; set; }
        public string device_no { get; set; }
        public string patch_no { get; set; }

        public DateTime? schedule_hookup { get; set; }
        public DateTime? schedule_removal { get; set; }
        public DateTime? actual_hookup { get; set; }
        public DateTime? actual_removal { get; set; }
        public long estimated_test_prices_id { get; set; }

        public double actual_amount { get; set; }
        public double total_payed_amount { get; set; }
        public string enrollment_status { get; set; }
        public DateTime? payment_datetime { get; set; }
        public string payment_status { get; set; }
        public string remarks { get; set; }
        public long clinical_user_id { get; set; }
        public string emp_code { get; set; }
        public long created_by { get; set; }
        public DateTime? created_datetime { get; set; }
        public long modified_by { get; set; }
        public DateTime? modified_datetime { get; set; }
        public bool is_active { get; set; }
        public bool is_pacemaker { get; set; }
        public bool is_patient_removal { get; set; }
        public bool is_patient_hookuped { get; set; }

        #region Query
        internal static string get_user_id_by_emp_code()
        {
            return $@"SELECT user_id FROM {Tbl.user_master} WHERE emp_code = @emp_code AND is_active = 1";
        }

        internal static string get_pateint_id_emp_code()
        {
            return $@"select patient_id from {Tbl.patient_enrollment} where patient_enrollment_id = @patient_enrollment_id  and is_active = 1";
        }
        internal static string check_enrollment_exists()
        {
            return $@" SELECT pe.patient_enrollment_id, pe.gu_id, pe.user_id, pe.patient_id, pe.device_id, pe.patch_id, pe.schedule_hookup, pe.schedule_removal, pe.actual_hookup, pe.actual_removal, pe.is_patient_hookuped, pe.is_patient_removal,
                    pe.enrollment_status, pe.payment_status, pe.remarks, pe.clinical_user_id, pe.created_by, pe.actual_technician_id, pe.is_pacemaker,pe.actual_amount,
                    tech.user_name AS actual_technician_name,
                    phy.user_name AS referred_physician_name,
                    l.device_id,
                    l.device_no,
                    b.patient_id,
                    b.patient_name,
                    b.dob AS patient_dob,
                    b.age AS patient_age,
                    b.phone_no AS patient_phone,
                    b.emergency_phone_no,
                    b.email_id AS patient_email,
                    b.gender AS patient_gender,
                    b.city AS patient_city,
                    b.pincode AS patient_pincode,
                    g.district_name,
                    h.state_name AS patient_state,
                    i.country_name,
                    k.test_name,
                    k.duration_in_day,
                    e.hospital_name,
                    m.patch_no,
                    m.patch_id
                    FROM {Tbl.patient_enrollment} as pe
                        LEFT JOIN {Tbl.patient} b
                            ON b.patient_id = pe.patient_id

                        LEFT JOIN {Tbl.user_master} tech
                            ON tech.user_id = pe.user_id

                        LEFT JOIN {Tbl.user_master} phy
                            ON phy.user_id = pe.referred_physician_id

                        LEFT JOIN {Tbl.hospital_mast} e
                            ON e.hospital_id = pe.hospital_id

                        LEFT JOIN {Tbl.district} g
                            ON g.district_id = b.district_id

                        LEFT JOIN {Tbl.state} h
                            ON h.state_id = b.state_id

                        LEFT JOIN {Tbl.country} i
                            ON i.country_id = h.country_id

                        LEFT JOIN {Tbl.test_prices} j
                            ON j.test_prices_id = pe.estimated_test_prices_id

                        LEFT JOIN {Tbl.test_mast} k
                            ON j.test_id = k.test_id

                        LEFT JOIN {Tbl.device_mast} l
                            ON l.device_id = pe.device_id

                        LEFT JOIN {Tbl.patch_mast} m
                            ON m.patch_id = pe.patch_id

                    WHERE pe.patient_enrollment_id = @patient_enrollment_id and pe.is_active = 1 and pe.enrollment_status = 'requested' ";
        }

        #endregion
    }

    public class GetPatientEnrollmentListReqDTO
    {
        public List<string> enrollment_status { get; set; }
        public string search { get; set; }
        public DateTime? date { get; set; }
    }

    public class PatientEnrollmentDTO
    {
        public long patient_enrollment_id { get; set; }
        public Guid? gu_id { get; set; }

        public long user_id { get; set; }
        public long patient_id { get; set; }
        public string patient_ph_number { get; set; }
        public long device_id { get; set; }
        public long patch_id { get; set; }
        public long referred_physician_id { get; set; }
        public long actual_technician_id { get; set; }
        public long hospital_id { get; set; }

        public DateTime? schedule_hookup { get; set; }
        public DateTime? schedule_removal { get; set; }
        public DateTime? actual_hookup { get; set; }
        public DateTime? actual_removal { get; set; }
        public string? sign_patient_img { get; set; }
        public IFormFile? sign_patient_file { get; set; }
        public string? sign_field_emp_img { get; set; }
        public IFormFile? sign_field_emp_file { get; set; }
        public string? hookup_start_device_img { get; set; }
        public IFormFile? hookup_start_device_file { get; set; }
        public string? hookup_end_device_img { get; set; }
        public IFormFile? hookup_end_device_file { get; set; }

        public long estimated_test_prices_id { get; set; }
        public decimal actual_amount { get; set; }
        public decimal total_payed_amount { get; set; }

        public string enrollment_status { get; set; }
        // 'requested','hookuped','monitoring','report_generated','completed'
        public DateTime? payment_datetime { get; set; }
        public string payment_status { get; set; }
        // 'pending','completed','failed'
        public string remarks { get; set; }
        public long clinical_user_id { get; set; }

        public long created_by { get; set; }
        public DateTime? created_datetime { get; set; }
        public long modified_by { get; set; }
        public DateTime? modified_datetime { get; set; }
        public bool is_active { get; set; }
        public bool is_pacemaker { get; set; }
        public bool is_patient_hookuped { get; set; }


        #region Query
        internal static string get_patient_details()
        {
            return $@"SELECT phone_no as patient_ph_number  FROM {Tbl.patient} WHERE patient_id = @patient_id AND is_active = 1";
        }
        internal static string check_enrollment_status()
        {
            return $@"SELECT patient_enrollment_id, enrollment_status FROM {Tbl.patient_enrollment} WHERE patient_enrollment_id = @patient_enrollment_id AND is_active = 1";
        }
        internal static string insert_enrollment()
        {
            return $@"
                    INSERT INTO tbl_patient_enrollment
                    (
                        user_id,
                        patient_id,
                        referred_physician_id,
                        hospital_id,
                        schedule_hookup,
                        schedule_removal,
                        estimated_test_prices_id,
                        actual_amount,
                        total_payed_amount,
                        payment_status,
                        payment_datetime,
                        enrollment_status,
                        is_pacemaker,
                        created_by
                    )
                    SELECT
                        @user_id,
                        @patient_id,
                        @referred_physician_id,
                        @hospital_id,
                        @schedule_hookup,
                        DATEADD(DAY, tm.duration_in_day, @schedule_hookup),
                        @estimated_test_prices_id,
                        @actual_amount,
                        @total_payed_amount,
                        @payment_status,
                        @payment_datetime,
                        @enrollment_status,
                        @is_pacemaker,
                        @created_by
                    FROM {Tbl.test_prices} tp
                    INNER JOIN {Tbl.test_mast} tm
                        ON tp.test_id = tm.test_id
                    WHERE tp.test_prices_id = @estimated_test_prices_id;";
        }

        internal static string update_enrollment()
        {
            return $@"UPDATE {Tbl.patient_enrollment} SET
                    user_id                  = @user_id,
                    patient_id               = @patient_id,
                    referred_physician_id       = @referred_physician_id,
                    hospital_id              = @hospital_id,
                    schedule_hookup          = @schedule_hookup,
                    schedule_removal         = @schedule_removal,
                    actual_hookup            = @actual_hookup,
                    actual_removal           = @actual_removal,
                    estimated_test_prices_id = @estimated_test_prices_id,
                    actual_amount            = @actual_amount,
                    total_payed_amount       = @total_payed_amount,
                    enrollment_status        = @enrollment_status,
                    payment_datetime         = @payment_datetime,
                    payment_status           = @payment_status,
                    remarks                  = @remarks,
                    clinical_user_id         = @clinical_user_id,
                    modified_by              = @modified_by,
                    modified_datetime        = GETDATE(),
                    is_active                = @is_active
                  WHERE patient_enrollment_id = @patient_enrollment_id";
        }
        internal static string update_signature()
        {
            return $@"
                    UPDATE {Tbl.patient_enrollment}
                    SET sign_patient_img = ISNULL(@sign_patient_img, sign_patient_img),
                        sign_field_emp_img = ISNULL(@sign_field_emp_img, sign_field_emp_img)
                    WHERE patient_enrollment_id = @patient_enrollment_id and is_active = 1";
        }
        internal static string device_img_upload()
        {
            return $@"
                    UPDATE {Tbl.patient_enrollment}
                    SET hookup_start_device_img = ISNULL(@hookup_start_device_img, hookup_start_device_img),
                        hookup_end_device_img = ISNULL(@hookup_end_device_img, hookup_end_device_img)
                    WHERE patient_enrollment_id = @patient_enrollment_id and is_active = 1";
        }
        //internal static string start_hookup_status()
        //{
        //    return $@"UPDATE {Tbl.patient_enrollment}
        //      SET actual_technician_id = @actual_technician_id,
        //          actual_hookup = GETDATE(),
        //          enrollment_status = @enrollment_status, 
        //          device_id = @device_id,  
        //          is_patient_hookuped = @is_patient_hookuped 
        //      WHERE patient_enrollment_id = @patient_enrollment_id and enrollment_status = 'requested'";
        //}
        internal static string start_hookup_status()
        {
            return $@"
                    UPDATE pe
                    SET
                        pe.actual_technician_id = @actual_technician_id,
                        pe.actual_hookup = GETDATE(),
                        pe.actual_removal = DATEADD(DAY, tm.duration_in_day, GETDATE()),
                        pe.enrollment_status = @enrollment_status,
                        pe.device_id = @device_id,
                        pe.is_patient_hookuped = @is_patient_hookuped
                    FROM {Tbl.patient_enrollment} pe
                    INNER JOIN {Tbl.test_prices} tp
                        ON pe.estimated_test_prices_id = tp.test_prices_id
                    INNER JOIN {Tbl.test_mast} tm
                        ON tp.test_id = tm.test_id
                    WHERE pe.patient_enrollment_id = @patient_enrollment_id
                      AND pe.enrollment_status = 'requested'
                      AND pe.is_active = 1";
        }

        internal static string complete_hookup_status(long patient_enrollment_id)
        {
            return $@"UPDATE {Tbl.patient_enrollment}
              SET enrollment_status = 'completed',
                    is_patient_removal =@is_patient_removal,
                    modified_by = @modified_by, 
                    modified_datetime = getdate()
              WHERE patient_enrollment_id = @patient_enrollment_id and is_active = 1";
        }

        internal static string monitoring_hookup_status()
        {
            return $@"UPDATE {Tbl.patient_enrollment}
              SET enrollment_status = 'monitoring'
              WHERE patient_enrollment_id = @patient_enrollment_id and is_active = 1 AND enrollment_status = 'hookuped'";
        }

        internal static string check_completed_enrollment()
        {
            return $@"select top 1 * from {Tbl.patient_enrollment} where patient_enrollment_id = @patient_enrollment_id and enrollment_status = 'completed' and is_active = 1";
        }


        #endregion
    }
    public class RequestPatientEnrollmentResDTO
    {
        public long patient_enrollment_id { get; set; }
        public string enrollment_status { get; set; }
        public string payment_status { get; set; }
        public decimal actual_amount { get; set; }
        public DateTime? schedule_hookup { get; set; }
        public DateTime? schedule_removal { get; set; }
        public DateTime? actual_hookup { get; set; }
        public DateTime? actual_removal { get; set; }
        public string actual_technician_name { get; set; }

        public long device_id { get; set; }
        public string device_no { get; set; }
        public string device_status { get; set; }
        public long patch_id { get; set; }
        public string patch_no { get; set; }

        public long patient_id { get; set; }
        public string patient_name { get; set; }
        public string patient_dob { get; set; }
        public int patient_age { get; set; }
        public string patient_gender { get; set; }
        public string patient_city { get; set; }
        public string patient_state { get; set; }
        public string patient_pincode { get; set; }
        public string patient_phone { get; set; }
        public string emergency_phone_no { get; set; }
        public string patient_email { get; set; }
        public string test_name { get; set; }
        public int duration_in_day { get; set; }
        public string hospital_name { get; set; }
        public string referred_physician_name { get; set; }

        #region Query 
        internal static string select_request_patient_enrollment()
        {
            return $@"

                     select count(*)  FROM {Tbl.patient_enrollment} a

                        LEFT JOIN {Tbl.patient} b
                            ON b.patient_id = a.patient_id 

                        LEFT JOIN {Tbl.user_master} tech
                            ON tech.user_id = a.actual_technician_id

                        LEFT JOIN {Tbl.user_master} phy
                            ON phy.user_id = a.referred_physician_id

                        LEFT JOIN {Tbl.hospital_mast} e
                            ON e.hospital_id = a.hospital_id

                        LEFT JOIN {Tbl.district} g
                            ON g.district_id = b.district_id

                        LEFT JOIN {Tbl.state} h
                            ON h.state_id = b.state_id

                        LEFT JOIN {Tbl.country} i
                            ON i.country_id = h.country_id

                        LEFT JOIN {Tbl.test_prices} j
                            ON j.test_prices_id = a.estimated_test_prices_id

                        LEFT JOIN {Tbl.test_mast} k
                            ON j.test_id = k.test_id

                        LEFT JOIN {Tbl.device_mast} l
                            ON l.device_id = a.device_id

                        LEFT JOIN {Tbl.patch_mast} m
                            ON m.patch_id = a.patch_id

                      WHERE
                            (
                                @enrollment_status = '[]' OR a.enrollment_status IN (SELECT value FROM OPENJSON(@enrollment_status) )
                            )
                            AND
                            (
                                @search IS NULL
                                OR @search = ''
                                OR CONVERT(VARCHAR(50), a.patient_enrollment_id) LIKE '%' + @search + '%'
                                OR b.patient_name LIKE '%' + @search + '%'
                                OR b.phone_no LIKE '%' + @search + '%'
                            ) 
                              AND
                                (
                                    @date IS NULL

                                    OR
                                    (
                                        a.enrollment_status IN ('requested', 'hookuped')
                                        AND CAST(a.schedule_hookup AS DATE) = @date
                                    )

                                    OR
                                    (
                                        a.enrollment_status = 'monitoring'
                                        AND CAST(a.actual_removal AS DATE) = @date
                                    )

                                    OR
                                    (
                                        a.enrollment_status = 'completed'
                                        AND CAST(a.actual_removal AS DATE) = @date
                                    )
                                ) and a.is_active = 1;




                        SELECT
                            a.patient_enrollment_id,
                            a.enrollment_status,
                            a.actual_amount,
                            a.payment_status,
                            a.schedule_hookup ,
                            a.schedule_removal ,
                            a.actual_hookup ,
                            a.actual_removal ,
                            tech.user_name AS actual_technician_name,
                            phy.user_name AS referred_physician_name,
                            l.device_id,
                            l.device_no,
                            b.patient_id,
                            b.patient_name,
                            b.dob AS patient_dob,
                            b.age AS patient_age,
                            b.phone_no AS patient_phone,
                            b.emergency_phone_no,
                            b.email_id AS patient_email,
                            b.gender AS patient_gender,
                            b.city AS patient_city,
                            b.pincode AS patient_pincode,
                            g.district_name,
                            h.state_name AS patient_state,
                            i.country_name,
                            k.test_name,
                            k.duration_in_day,
                            e.hospital_name,
                            m.patch_no,
                            m.patch_id

                        FROM {Tbl.patient_enrollment} a

                        LEFT JOIN {Tbl.patient} b
                            ON b.patient_id = a.patient_id 

                        LEFT JOIN {Tbl.user_master} tech
                            ON tech.user_id = a.actual_technician_id

                        LEFT JOIN {Tbl.user_master} phy
                            ON phy.user_id = a.referred_physician_id

                        LEFT JOIN {Tbl.hospital_mast} e
                            ON e.hospital_id = a.hospital_id

                        LEFT JOIN {Tbl.district} g
                            ON g.district_id = b.district_id

                        LEFT JOIN {Tbl.state} h
                            ON h.state_id = b.state_id

                        LEFT JOIN {Tbl.country} i
                            ON i.country_id = h.country_id

                        LEFT JOIN {Tbl.test_prices} j
                            ON j.test_prices_id = a.estimated_test_prices_id

                        LEFT JOIN {Tbl.test_mast} k
                            ON j.test_id = k.test_id

                        LEFT JOIN {Tbl.device_mast} l
                            ON l.device_id = a.device_id

                        LEFT JOIN {Tbl.patch_mast} m
                            ON m.patch_id = a.patch_id

                      WHERE
                            (
                                @enrollment_status = '[]' OR a.enrollment_status IN (SELECT value FROM OPENJSON(@enrollment_status) )
                            )
                            AND
                            (
                                @search IS NULL
                                OR @search = ''
                                OR CONVERT(VARCHAR(50), a.patient_enrollment_id) LIKE '%' + @search + '%'
                                OR b.patient_name LIKE '%' + @search + '%'
                                OR b.phone_no LIKE '%' + @search + '%'
                            ) 
                           AND
                                (
                                    @date IS NULL

                                    OR
                                    (
                                        a.enrollment_status IN ('requested', 'hookuped')
                                        AND CAST(a.schedule_hookup AS DATE) = @date
                                    )

                                    OR
                                    (
                                        a.enrollment_status = 'monitoring'
                                        AND CAST(a.actual_removal AS DATE) = @date
                                    )

                                    OR
                                    (
                                        a.enrollment_status = 'completed'
                                        AND CAST(a.actual_removal AS DATE) = @date
                                    )
                                )
                                AND a.is_active = 1";
        }
        #endregion
    }

    public class SymtomsDTO
    {
        public long symptoms_id { get; set; }
        public long patient_enrollment_id { get; set; }
        public long symptoms_type_id { get; set; }
        public long activity_type_id { get; set; }
        public string? symptoms_type { get; set; }
        public string? activity_type { get; set; }
        public string? custom_symptom_text { get; set; }
        public DateTime? symptom_start_datetime { get; set; }
        public DateTime? symptom_end_datetime { get; set; }
        public string? symptom_notes { get; set; }
        public long created_by { get; set; }
        public DateTime? created_datetime { get; set; }
        public bool is_active { get; set; }

        #region Query
        internal static string insert_symtoms()
        {
            return $@"insert into {Tbl.symptoms}(patient_enrollment_id,symptoms_type_id,activity_type_id,custom_symptom_text,symptom_start_datetime,symptom_end_datetime,symptom_notes)
                        values(@patient_enrollment_id, @symptoms_type_id, @activity_type_id, @custom_symptom_text, @symptom_start_datetime, @symptom_end_datetime, @symptom_notes)";
        }

        internal static string select_symtoms_list()
        {
            return $@"select a.symptoms_id, a.patient_enrollment_id, a.symptoms_type_id, b.symptoms_type, a.activity_type_id, c.activity_type, a.custom_symptom_text, a.symptom_start_datetime, a.symptom_end_datetime, a.symptom_notes, a.created_by, a.created_datetime, a.is_active,b.symptoms_type_id, b.symptoms_type
                      from {Tbl.symptoms} as a 
                      left join {Tbl.symptoms_type} as b
                        on a.symptoms_type_id = b.symptoms_type_id
                      left join {Tbl.patient_activity_type} as c
                        on a.activity_type_id = c.activity_type_id
                    where (@patient_enrollment_id = 0 or a.patient_enrollment_id = @patient_enrollment_id) and a.is_active = 1";
        }
        #endregion
    }
    public class EnrollmentRawDataDTO
    {
        public long enrollment_raw_data_id { get; set; }
        public long patient_enrollment_id { get; set; }
        public DateTime? data_start { get; set; }
        public DateTime? data_end { get; set; }
        public IFormFile? file_name { get; set; }
        public string file_path { get; set; }
        public int battery_percentage { get; set; }
        public string auto_check_status { get; set; }
        public DateTime? waiting_datetime { get; set; }
        public DateTime? checking_datetime { get; set; }
        public DateTime? completed_datetime { get; set; }
        public long created_by { get; set; }
        public DateTime? created_datetime { get; set; }
        public bool is_active { get; set; }

        #region Query
        //internal static string insert_raw_data()
        //{
        //    return $@"INSERT INTO {Tbl.enrollment_raw_data} 
        //          (patient_enrollment_id, data_start,data_end,file_path,battery_percentage,auto_check_status, waiting_datetime, created_by) 
        //       VALUES(@patient_enrollment_id, @data_start,@data_end,@file_path,@battery_percentage,@auto_check_status, getdate(), @created_by)";

        //}

        internal static string insert_raw_data()
        {
            return $@"
                    IF NOT EXISTS
                    (
                        SELECT 1
                        FROM {Tbl.enrollment_raw_data} WITH (UPDLOCK, HOLDLOCK)
                        WHERE patient_enrollment_id = @patient_enrollment_id
                          AND file_path = @file_path
                          AND is_active = 1
                    )
                    BEGIN
                        INSERT INTO {Tbl.enrollment_raw_data}
                        (
                            patient_enrollment_id,
                            data_start,
                            data_end,
                            file_path,
                            battery_percentage,
                            auto_check_status,
                            waiting_datetime,
                            created_by
                        )
                        VALUES
                        (
                            @patient_enrollment_id,
                            @data_start,
                            @data_end,
                            @file_path,
                            @battery_percentage,
                            @auto_check_status,
                            GETDATE(),
                            @created_by
                        );

                        SELECT CAST(SCOPE_IDENTITY() AS BIGINT);
                    END
                    ELSE
                        SELECT CAST(0 AS BIGINT);";
        }

        //internal static string GetWaitingRecords()
        //{
        //    return $@"
        //            SELECT TOP (50)
        //                enrollment_raw_data_id,
        //                patient_enrollment_id,
        //                file_path
        //            FROM {Tbl.enrollment_raw_data}
        //            WHERE auto_check_status = 'waiting'
        //              AND is_active = 1
        //            ORDER BY waiting_datetime";
        //}

        internal static string GetWaitingRecords()
        {
            return $@"
                        SELECT TOP (50)
                            enrollment_raw_data_id,
                            patient_enrollment_id,
                            file_path
                        FROM {Tbl.enrollment_raw_data}
                        WHERE auto_check_status = 'waiting'
                          AND is_active = 1
                          AND LOWER(file_path) LIKE '%.edf'
                        ORDER BY waiting_datetime";
        }


        internal static string status_waiting_list()
        {
            return $@"select top 1 erd.enrollment_raw_data_id, erd.patient_enrollment_id, erd.data_start, erd.data_end, erd.file_path, erd.battery_percentage,
                    erd.auto_check_status, erd.waiting_datetime, erd.checking_datetime, erd.completed_datetime, erd.created_by, erd.created_datetime, erd.is_active
                    from {Tbl.enrollment_raw_data} as erd 
                    left join {Tbl.patient_enrollment} as pe
                    on erd.patient_enrollment_id = pe.patient_enrollment_id
                    where erd.is_active = 1  and pe.is_active = 1 
                    order by enrollment_raw_data_id asc;";
        }
        internal static string update_checking_status()
        {
            return $@"UPDATE {Tbl.enrollment_raw_data}
                    SET auto_check_status = 'checking',
                        checking_datetime = GETDATE()
                    WHERE enrollment_raw_data_id = @enrollment_raw_data_id and auto_check_status = 'waiting'";
        }
        internal static string update_waiting_status()
        {
            return $@"
                    UPDATE {Tbl.enrollment_raw_data}
                    SET
                        auto_check_status = 'waiting',
                        completed_datetime = NULL, 
                        retry_count = retry_count + 1
                    WHERE enrollment_raw_data_id = @enrollment_raw_data_id";
        }

        internal static string update_completed_status()
        {
            return $@"
                    UPDATE {Tbl.enrollment_raw_data}
                    SET
                        auto_check_status = 'completed',
                        completed_datetime = GETDATE()
                    WHERE enrollment_raw_data_id = @enrollment_raw_data_id";
        }

        internal static string reset_stuck_checking_records()
        {
            return $@"
                    UPDATE {Tbl.enrollment_raw_data}
                    SET
                        auto_check_status = 'waiting',
                        checking_datetime = NULL,
                        remarks = 'Processing timeout'
                    WHERE auto_check_status = 'checking'
                      AND checking_datetime < DATEADD(MINUTE, -5, GETDATE());";
        }

        #endregion

    }


    public class PatientAuthorisedConsentDTO
    {
        public long patient_authorised_consent_id { get; set; }
        public string authorised_notes { get; set; }
        public long created_by { get; set; }
        public DateTime? created_datetime { get; set; }
        public long modified_by { get; set; }
        public DateTime? modified_datetime { get; set; }
        public bool is_active { get; set; }


        #region Query 
        internal static string insert_patient_authorised_consent()
        {
            return $@"INSERT INTO {Tbl.patient_authorised_consent_mast} (authorised_notes, created_by) VALUES (@authorised_notes, @created_by)";
        }

        internal static string select_patient_authorised_consent()
        {
            return $@"SELECT count(*) as totalCount FROM {Tbl.patient_authorised_consent_mast} WHERE is_active = 1;
                    SELECT patient_authorised_consent_id, authorised_notes, created_by, created_datetime, modified_by, modified_datetime, is_active FROM {Tbl.patient_authorised_consent_mast} WHERE is_active = 1";
        }

        internal static string update_patient_authorised_consent()
        {
            return $@"UPDATE {Tbl.patient_authorised_consent_mast} SET authorised_notes = @authorised_notes, modified_by = @modified_by, modified_datetime = GETDATE(), is_active = @is_active WHERE patient_authorised_consent_id = @patient_authorised_consent_id";
        }

        internal static string select_patient_authorised_consent_trans()
        {
            return $@"
                      SELECT count(*) as totalCount
                    FROM tbl_patient_authorised_consent_trans as pact
                    Inner join tbl_patient_authorised_consent_mast as pacm
                    on pacm.patient_authorised_consent_id =pact.patient_authorised_consent_id
                    WHERE patient_enrollment_id = @patient_enrollment_id
                    AND pact.is_active = 1;

                   SELECT pacm.patient_authorised_consent_id, pacm.authorised_notes,pact.is_active,pact.created_datetime
                    FROM tbl_patient_authorised_consent_trans as pact
                    Inner join tbl_patient_authorised_consent_mast as pacm
                    on pacm.patient_authorised_consent_id =pact.patient_authorised_consent_id
                    WHERE patient_enrollment_id = @patient_enrollment_id
                    AND pact.is_active = 1";
        }
        #endregion
    }


    public class PatientAuthorisedConsentTransDTO
    {
        public long patient_authorised_consent_trans_id { get; set; }
        public long patient_enrollment_id { get; set; }
        public List<long> patient_authorised_consent_ids { get; set; }
        public long created_by { get; set; }
        public DateTime? created_datetime { get; set; }
        public long modified_by { get; set; }
        public DateTime? modified_datetime { get; set; }
        public bool is_active { get; set; }
        public bool withdraw { get; set; } = false;

        #region Query 
        internal static string check_patient_authorised_consent_trans()
        {
            return $@"
                    SELECT *
                    FROM tbl_patient_authorised_consent_trans
                    WHERE patient_enrollment_id = @patient_enrollment_id
                    AND patient_authorised_consent_id = @patient_authorised_consent_id
                    AND is_active = 1";
        }

        internal static string insert_patient_authorised_consent_trans()
        {
            return $@" INSERT INTO tbl_patient_authorised_consent_trans ( patient_enrollment_id, patient_authorised_consent_id)
                       VALUES( @patient_enrollment_id, @patient_authorised_consent_id)";
        }
        internal static string update_patient_authorised_consent_trans()
        {
            return $@"
                    UPDATE tbl_patient_authorised_consent_trans
                    SET
                        is_active = 0,
                        modified_datetime = GETDATE()
                    WHERE patient_enrollment_id = @patient_enrollment_id
                    AND is_active = 1
                    AND patient_authorised_consent_id NOT IN @patient_authorised_consent_ids";
        }
        internal static string deactivate_all_patient_authorised_consent_trans()
        {
            return $@"
                    UPDATE tbl_patient_authorised_consent_trans
                    SET
                        is_active = 0,
                        modified_datetime = GETDATE()
                    WHERE patient_enrollment_id = @patient_enrollment_id
                    AND is_active = 1";
        }

        internal static string patient_enrollment_withdraw_status()
        {
            return $@"
                    UPDATE tbl_patient_enrollment
                    SET
                       enrollment_status = 'withdraw',
                        modified_datetime = GETDATE()
                    WHERE patient_enrollment_id = @patient_enrollment_id
                    AND is_active = 1";
        }

        internal static string update_inactive_patient_enrollment()
        {
            return $@"
                    UPDATE tbl_enrollment_raw_data
                    SET is_active = 0
                    WHERE patient_enrollment_id = @patient_enrollment_id
                      AND is_active = 1;

                    UPDATE tbl_patient_base_line
                    SET is_active = 0
                    WHERE patient_enrollment_id = @patient_enrollment_id
                      AND is_active = 1;

                    UPDATE tbl_queued_events_records
                    SET is_active = 0
                    WHERE patient_enrollment_id = @patient_enrollment_id
                      AND is_active = 1;
                ";
        }

        #endregion
    }




    public class PatientGuardianDTO
    {
        public long patient_guardianship_id { get; set; }
        public long patient_enrollment_id { get; set; }
        public string guardian_name { get; set; }
        public string guardian_father_name { get; set; }
        public DateTime? guardian_dob { get; set; }
        public string file_path { get; set; }
        public IFormFile? file_name { get; set; }
        public long created_by { get; set; }
        public DateTime? created_datetime { get; set; }
        public long modified_by { get; set; }
        public DateTime? modified_datetime { get; set; }
        public bool is_active { get; set; }

        #region Query
        internal static string insert_patient_guardian()
        {
            return $@"INSERT INTO {Tbl.patient_guardianship} (patient_enrollment_id, guardian_name, guardian_father_name, guardian_dob, file_path) VALUES (@patient_enrollment_id, @guardian_name, @guardian_father_name, @guardian_dob, @file_path)";
        }
        internal static string deactivate_patient_guardian()
        {
            return $@" UPDATE {Tbl.patient_guardianship}
                        SET is_active = 0,
                            modified_datetime = GETDATE()
                        WHERE patient_enrollment_id = @patient_enrollment_id
                        AND is_active = 1;";
        }


        internal static string select_patient_guardian()
        {
            return $@"SELECT TOP 1 *
                        FROM {Tbl.patient_guardianship}
                        WHERE patient_enrollment_id = @patient_enrollment_id
                        AND is_active = 1;";
        }
        #endregion
    }




    public class PatientEnrollmentPauseDTO
    {
        public long patient_enrollment_id { get; set; }
        public int? pause_duration_minutes { get; set; }
        public DateTime? pause_datetime { get; set; }
        public string? pause_reason { get; set; }
        public DateTime? resume_datetime { get; set; }
        public string? resume_remarks { get; set; }
        public bool is_resume { get; set; } = false;

        #region Query
        internal static string check_patient_enrollment_monitoring()
        {
            return $@"
                    SELECT COUNT(1)
                    FROM tbl_patient_enrollment
                    WHERE patient_enrollment_id = @patient_enrollment_id
                    AND enrollment_status = 'monitoring'
                    AND is_active = 1;";
        }
        internal static string check_patient_enrollment_pause()
        {
            return $@"
                    SELECT TOP 1 patient_enrollment_pause_trans_id
                    FROM tbl_patient_enrollment_pause_trans
                    WHERE patient_enrollment_id = @patient_enrollment_id
                    AND resume_datetime IS NULL
                    ORDER BY patient_enrollment_pause_trans_id DESC";
        }

        internal static string insert_patient_enrollment_pause()
        {
            return $@" INSERT INTO tbl_patient_enrollment_pause_trans
                        (
                            patient_enrollment_id,
                            pause_datetime,
                            pause_duration_minutes,
                            pause_reason
                        )
                        VALUES
                        (
                            @patient_enrollment_id,
                            @pause_datetime,
                            @pause_duration_minutes,
                            @pause_reason
                        );";
        }
        internal static string update_patient_enrollment_resume()
        {
            return $@"
                    UPDATE tbl_patient_enrollment_pause_trans
                    SET
                        resume_datetime = @resume_datetime,
                        resume_remarks = @resume_remarks,
                        modified_datetime = GETDATE()
                    WHERE patient_enrollment_id = @patient_enrollment_id
                    AND resume_datetime IS NULL;";
        }

        #endregion
    }
}
