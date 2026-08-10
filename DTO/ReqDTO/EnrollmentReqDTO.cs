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

        public long created_by { get; set; }
        public DateTime? created_datetime { get; set; }
        public long modified_by { get; set; }
        public DateTime? modified_datetime { get; set; }
        public bool is_active { get; set; }
        public bool is_pacemaker { get; set; }

        #region Query

        internal static string check_enrollment_exists()
        {
            return $@" SELECT patient_enrollment_id, gu_id, user_id, patient_id, device_id, patch_id, schedule_hookup, schedule_removal, actual_hookup, actual_removal, 
                    enrollment_status, payment_status, remarks, clinical_user_id, created_by, actual_technician_id, is_pacemaker
                    FROM {Tbl.patient_enrollment}
                    WHERE patient_enrollment_id = @patient_enrollment_id and is_active = 1 and enrollment_status = 'requested' ";
        }

        #endregion
    }

    public class PatientEnrollmentDTO
    {
        public long patient_enrollment_id { get; set; }
        public Guid? gu_id { get; set; }

        public long user_id { get; set; }
        public long patient_id { get; set; }
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


        #region Query

        internal static string insert_enrollment()
        {
            return $@"
                    INSERT INTO tbl_patient_enrollment
                    (
                        user_id,
                        patient_id,
                        referred_physician_id,
                        actual_technician_id,
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
                        @actual_technician_id,
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
                    actual_technician_id     = @actual_technician_id,
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
                    WHERE patient_enrollment_id = @patient_enrollment_id";
        }
        internal static string device_img_upload()
        {
            return $@"
                    UPDATE {Tbl.patient_enrollment}
                    SET hookup_start_device_img = ISNULL(@hookup_start_device_img, hookup_start_device_img),
                        hookup_end_device_img = ISNULL(@hookup_end_device_img, hookup_end_device_img)
                    WHERE patient_enrollment_id = @patient_enrollment_id";
        }

        internal static string start_hookup_status()
        {
            return $@"UPDATE {Tbl.patient_enrollment}
              SET actual_hookup = GETDATE(),
                  enrollment_status = @enrollment_status,
                  device_id = @device_id,   
                  patch_id = @patch_id
              WHERE patient_enrollment_id = @patient_enrollment_id and enrollment_status = 'requested'";
        }
        //internal static string start_hookup_status()
        //{
        //    return $@"UPDATE {Tbl.patient_enrollment}
        //      SET actual_hookup = GETDATE(),
        //          enrollment_status = @enrollment_status,
        //          modified_by = @modified_by,   
        //          device_id = @device_id,   
        //          patch_id = @patch_id,
        //          modified_datetime = getdate()
        //      WHERE patient_enrollment_id = @patient_enrollment_id and enrollment_status = 'requested'";
        //}

        internal static string complete_hookup_status(long complete_hookup_patient)
        {
            return $@"UPDATE {Tbl.patient_enrollment}
              SET actual_removal = GETDATE(),
                  enrollment_status = 'completed'
              WHERE patient_enrollment_id = @patient_enrollment_id";
        }

        internal static string monitoring_hookup_status(long patient_enrollment_id)
        {
            return $@"UPDATE {Tbl.patient_enrollment}
              SET enrollment_status = 'monitoring'
              WHERE patient_enrollment_id = @patient_enrollment_id";
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
        public decimal actual_amount { get; set; }
        public DateTime? schedule_hookup { get; set; }
        public string actual_technician_name { get; set; }

        public long device_id { get; set; }
        public string device_no { get; set; }

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
        public string hospital_name { get; set; }
        public string referred_physician_name { get; set; }

        #region Query 
        internal static string select_request_patient_enrollment()
        {
            return $@"
            SELECT
                a.patient_enrollment_id,
                a.enrollment_status,
                a.actual_amount,
                a.schedule_hookup,
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
                e.hospital_name

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

            WHERE a.enrollment_status = 'requested' and  (a.patient_enrollment_id = @patient_enrollment_id or @patient_enrollment_id = 0 or @patient_enrollment_id is null)";
        }
        #endregion
    }

    public class SymtomsDTO
    {
        public long symptoms_id { get; set; }
        public long patient_enrollment_id { get; set; }
        public long? symptoms_type_id { get; set; }
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
            return $@"insert into {Tbl.symptoms}(patient_enrollment_id,symptoms_type_id,custom_symptom_text,symptom_start_datetime,symptom_end_datetime,symptom_notes)
                        values(@patient_enrollment_id, @symptoms_type_id, @custom_symptom_text, @symptom_start_datetime, @symptom_end_datetime, @symptom_notes)";
        }

        internal static string select_symtoms_list()
        {
            return $@"select * from {Tbl.symptoms}";
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
        internal static string insert_raw_data()
        {
            return $@"INSERT INTO {Tbl.enrollment_raw_data} 
                  (patient_enrollment_id, data_start,data_end,file_path,battery_percentage,auto_check_status, waiting_datetime, created_by) 
               VALUES(@patient_enrollment_id, @data_start,@data_end,@file_path,@battery_percentage,@auto_check_status, getdate(), @created_by)";

        }

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
                    ORDER BY waiting_datetime";
        }

        internal static string status_waiting_list()
        {
            return $@"select top 1 erd.enrollment_raw_data_id, erd.patient_enrollment_id, erd.data_start, erd.data_end, erd.file_path, erd.battery_percentage,
                    erd.auto_check_status, erd.waiting_datetime, erd.checking_datetime, erd.completed_datetime, erd.created_by, erd.created_datetime, erd.is_active
                    from {Tbl.enrollment_raw_data} as erd 
                    left join {Tbl.patient_enrollment} as pe
                    on erd.patient_enrollment_id = pe.patient_enrollment_id
                    where erd.is_active = 1 
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
}
