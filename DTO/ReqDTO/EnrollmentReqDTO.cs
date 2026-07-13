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

        public DateTime? schedule_hookup { get; set; }
        public DateTime? schedule_removal { get; set; }
        public DateTime? actual_hookup { get; set; }
        public DateTime? actual_removal { get; set; }
        public int? hr_min { get; set; }
        public int? hr_max { get; set; }
        public int? hr_avg { get; set; }
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

        #region Query

        internal static string chack_valid_enrollment_id()
        {
            return $@" select * from {Tbl.patient_enrollment} where patient_enrollment_id = @patient_enrollment_id and is_active = 1 ";
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
        public int hr_min { get; set; }
        public int hr_max { get; set; }
        public int hr_avg { get; set; }

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

        // Join fields (select-ல் மட்டும்)
        public string patient_name { get; set; }
        public string patient_dob { get; set; }
        public int patient_age { get; set; }
        public string patient_gender { get; set; }
        public string patient_city { get; set; }
        public string patient_state { get; set; }
        public string patient_pincode { get; set; }
        public string patient_phone { get; set; }
        public string patient_email { get; set; }
        public string test_name { get; set; }
        public decimal test_price { get; set; }
        public string physician_name { get; set; }
        public string hospital_name { get; set; }
        public string technician_name { get; set; }

        #region Query

        internal static string insert_enrollment()
        {
            return $@"INSERT INTO tbl_patient_enrollment
                    (user_id, patient_id,referred_physician_id, actual_technician_id, hospital_id,
                     schedule_hookup,estimated_test_prices_id, actual_amount,total_payed_amount, payment_status,payment_datetime)
                  VALUES
                    (@user_id, @patient_id, 
                     @referred_physician_id, @actual_technician_id, @hospital_id,
                     @schedule_hookup, @estimated_test_prices_id, @actual_amount,@total_payed_amount,@payment_status,@payment_datetime)";
        }

        internal static string update_enrollment()
        {
            return $@"UPDATE tbl_patient_enrollment SET
                    user_id                  = @user_id,
                    patient_id               = @patient_id,
                    device_id                = @device_id,
                    patch_id                 = @patch_id,
                    referred_physician       = @referred_physician,
                    actual_technician_id     = @actual_technician_id,
                    hospital_id              = @hospital_id,
                    schedule_hookup          = @schedule_hookup,
                    schedule_removal         = @schedule_removal,
                    actual_hookup            = @actual_hookup,
                    actual_removal           = @actual_removal,
                    hr_min                   = @hr_min,
                    hr_max                   = @hr_max,
                    hr_avg                   = @hr_avg,
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

        internal static string select_enrollment()
        {
            return $@"SELECT
                    pe.patient_enrollment_id,
                    pe.gu_id,
                    pe.user_id,
                    pe.patient_id,
                    pe.device_id,
                    pe.patch_id,
                    pe.referred_physician,
                    pe.actual_technician_id,
                    pe.hospital_id,
                    pe.schedule_hookup,
                    pe.schedule_removal,
                    pe.actual_hookup,
                    pe.actual_removal,
                    pe.hr_min,
                    pe.hr_max,
                    pe.hr_avg,
                    pe.estimated_test_prices_id,
                    pe.actual_amount,
                    pe.total_payed_amount,
                    pe.enrollment_status,
                    pe.payment_datetime,
                    pe.payment_status,
                    pe.remarks,
                    pe.sign_patient_img,
                    pe.clinical_user_id,
                    pe.created_by,
                    pe.created_datetime,
                    pe.is_active,
                    pe.hookup_start_device_img,
                    pe.hookup_end_device_img,
                    pe.is_active,

                    -- Patient Info
                    p.full_name          AS patient_name,
                    p.dob                AS patient_dob,
                    p.age                AS patient_age,
                    p.gender             AS patient_gender,
                    p.city               AS patient_city,
                    p.state_name         AS patient_state,
                    p.pincode            AS patient_pincode,
                    p.phone_no           AS patient_phone,
                    p.email_id           AS patient_email,

                    -- Test Info
                    tm.test_name         AS test_name,
                    tp.test_price        AS test_price,

                    -- Physician
                    phy.full_name        AS physician_name,

                    -- Hospital
                    h.hospital_name      AS hospital_name,

                    -- Technician
                    tech.full_name       AS technician_name

                  FROM tbl_patient_enrollment pe
                  LEFT JOIN tbl_patient         p    ON p.patient_id       = pe.patient_id
                  LEFT JOIN tbl_test_prices     tp   ON tp.test_prices_id  = pe.estimated_test_prices_id
                  LEFT JOIN tbl_test_mast       tm   ON tm.test_id         = tp.test_id
                  LEFT JOIN tbl_user_mast       phy  ON phy.user_id        = pe.referred_physician
                  LEFT JOIN tbl_hospital_mast   h    ON h.hospital_id      = pe.hospital_id
                  LEFT JOIN tbl_user_mast       tech ON tech.user_id       = pe.actual_technician_id
                  WHERE pe.is_active = 1
                  AND (@patient_enrollment_id = 0 OR pe.patient_enrollment_id = @patient_enrollment_id)
                  AND (@patient_id            = 0 OR pe.patient_id            = @patient_id)
                  AND (@user_id               = 0 OR pe.user_id               = @user_id)
                  AND (@enrollment_status IS NULL OR pe.enrollment_status     = @enrollment_status)";
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

        internal static string start_hookup_status(long patient_enrollment_id)
        {
            return $@"UPDATE {Tbl.patient_enrollment}
              SET actual_hookup = GETDATE(),
                  enrollment_status = 'hookuped'
              WHERE patient_enrollment_id = {patient_enrollment_id}";
        }

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
                        auto_check_status='waiting',
                        checking_datetime=NULL
                    WHERE enrollment_raw_data_id=@enrollment_raw_data_id";
        }

        internal static string update_completed_status()
        {
            return $@"UPDATE {Tbl.enrollment_raw_data}
                      SET auto_check_status = 'completed',
                        completed_datetime = GETDATE()
                    WHERE enrollment_raw_data_id = @enrollment_raw_data_id";
        }

        #endregion

    }
}
