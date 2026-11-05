namespace elr_API.DTO.ReqDTO
{
    public class DoctorReqDTO
    {
    }

    public class DoctorLoginResDTO
    {
        public long doctor_id { get; set; }
        public Guid? gu_id { get; set; }
        public Guid? login_gu_id { get; set; }
        public string doctor_name { get; set; }
        public string phone_no { get; set; }
        public string speciality { get; set; }
        public string email_id { get; set; }
        public DateTime? date_of_birth { get; set; }
        public string gender { get; set; }
        public string city { get; set; }
        public int? otp { get; set; }
        public DateTime? expiry_datetime { get; set; }

        public long created_by { get; set; }
        public DateTime? created_datetime { get; set; }
        public long modified_by { get; set; }
        public DateTime? modified_datetime { get; set; }
        public bool is_active { get; set; }


        public long user_type_id { get; set; }
        public string user_type { get; set; }
        public long hospital_id { get; set; }
        public string hospital_name { get; set; }


        public string token { get; set; }
        public int expiryTime { get; set; }

        #region Query
        internal static string select_doctor_login()
        {
            return @"SELECT d.user_id as doctor_id , d.gu_id, d.user_name as doctor_name,  d.login_gu_id, d.phone_no, d.speciality, d.email_id, d.date_of_birth, d.gender, d.city, d.otp, d.expiry_datetime,
                            d.created_by, d.created_datetime, d.modified_by, d.modified_datetime, d.is_active,
                            ut.user_type_id, ut.user_type,
                            h.hospital_id, h.hospital_name
                     FROM tbl_user_mast AS d
                     INNER JOIN tbl_user_type AS ut ON d.user_type_id = ut.user_type_id
                     INNER JOIN tbl_hospital_mast AS h ON d.hospital_id = h.hospital_id
                     WHERE d.phone_no = @phone_no AND d.is_active = 1 and d.user_type_id = 5";
        }

        internal static string insert_doctor_otp()
        {
            return $@" UPDATE tbl_user_mast
                      SET otp = @otp, expiry_datetime = DATEADD(MINUTE, 10, GETDATE())
                      WHERE phone_no = @phone_no AND is_active = 1 and user_type_id = 5";
        }
        #endregion



    }

    public class DoctorLivePatientMonitoringReqDTO
    {
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public int offSet => (pageNumber - 1) * pageSize;
        public string sortBy { get; set; } = "actual_hookup";
        public string orderBy { get; set; } = "DESC";
        public long referred_physician_id { get; set; }
        public bool? live_monitoring { get; set; }

        #region Query

        internal static string select_dr_completed_report(DoctorLivePatientMonitoringReqDTO obj)
        {
            string sortColum = "pe.actual_hookup";

            if (obj.sortBy == "patient_name")
                sortColum = "p.patient_name";
            else if (obj.sortBy == "patient_id")
                sortColum = "p.patient_id";
            else if (obj.sortBy == "actual_removal")
                sortColum = "pe.actual_removal";


            string orderBy = obj.orderBy?.ToUpper() == "ASC" ? "ASC" : "Desc";

            return $@" 
                       select count(*) as totalCount from tbl_patient_report as pr
                        inner join tbl_patient_enrollment as pe
                        on pe.patient_enrollment_id = pr.patient_enrollment_id
                        inner join tbl_patient as p
                        on p.patient_id = pe.patient_id
                        WHERE pe.is_active = 1
                          AND pe.enrollment_status = 'completed'
                          AND pr.is_active = 1
                          AND pr.report_status = 'completed'
                          AND (
                                (SELECT user_type_id
                                 FROM tbl_user_mast
                                 WHERE user_id = {obj.referred_physician_id} ) = 1
                                OR pe.referred_physician_id = {obj.referred_physician_id}
                              );

                       select pr.patient_report_id,pr.report_type_id,pr.report_started_datetime,pr.report_due_datetime,pr.report_status,pr.is_active,
                        pe.patient_enrollment_id,pe.referred_physician_id,pe.actual_hookup,pe.enrollment_status,pe.is_active, pe.actual_removal,
                        p.patient_id,p.patient_name,p.is_active from tbl_patient_report as pr
                        inner join tbl_patient_enrollment as pe
                        on pe.patient_enrollment_id = pr.patient_enrollment_id
                        inner join tbl_patient as p
                        on p.patient_id = pe.patient_id
                        WHERE pe.is_active = 1
                          AND pe.enrollment_status = 'completed'
                          AND pr.is_active = 1
                          AND pr.report_status = 'completed'
                          AND (
                                (SELECT user_type_id
                                 FROM tbl_user_mast
                                 WHERE user_id = {obj.referred_physician_id} ) = 1
                                OR pe.referred_physician_id = {obj.referred_physician_id}
                              )
                        order by " + sortColum + " " + orderBy + @",
                        pe.patient_enrollment_id DESC
                        offset @offSet rows
                        fetch next @pageSize rows only;";

        }



        internal static string select_dr_live_patient_monitoring(DoctorLivePatientMonitoringReqDTO obj)
        {

            string sortColumn = "pe.actual_hookup";

            if (obj.sortBy == "patient_name") sortColumn = "p.patient_name";
            else if (obj.sortBy == "actual_removal") sortColumn = "pe.actual_removal";
            else if (obj.sortBy == "patient_id") sortColumn = "p.patient_id";

            string sortOrder = obj.orderBy?.ToUpper() == "ASC" ? "ASC" : "DESC";

            return $@"
                        SELECT COUNT_BIG(*) as totalCount
                        FROM tbl_patient_enrollment AS pe
                        INNER JOIN tbl_patient AS p
                            ON pe.patient_id = p.patient_id
                        INNER JOIN tbl_user_mast AS um
                            ON um.user_id = pe.referred_physician_id
                          WHERE pe.enrollment_status = 'monitoring'
                          AND (
                                (SELECT user_type_id
                                 FROM tbl_user_mast
                                 WHERE user_id = {obj.referred_physician_id}) = 1
                                OR pe.referred_physician_id = {obj.referred_physician_id}
                              )
                         AND pe.is_active = 1;

                        SELECT
                            pe.patient_enrollment_id,
                            pe.enrollment_status,
                            pe.is_active,
                            pe.referred_physician_id AS doctor_id,
                            um.user_name AS doctor_name,
                            pe.actual_hookup,
                            pe.actual_removal,
                            pe.patient_id,
                            p.patient_name
                        FROM tbl_patient_enrollment AS pe
                        INNER JOIN tbl_patient AS p
                            ON pe.patient_id = p.patient_id
                        INNER JOIN tbl_user_mast AS um
                            ON um.user_id = pe.referred_physician_id
                         WHERE pe.enrollment_status = 'monitoring'
                          AND (
                                (SELECT user_type_id
                                 FROM tbl_user_mast
                                 WHERE user_id = {obj.referred_physician_id} ) = 1
                                OR pe.referred_physician_id = {obj.referred_physician_id}
                              )
                          AND pe.is_active = 1
                        ORDER BY " + sortColumn + " " + sortOrder + @",
                            pe.patient_enrollment_id DESC
                        OFFSET @offSet ROWS
                        FETCH NEXT @pageSize ROWS ONLY;";
        }
        #endregion
    }


    public class DoctorLivePatientMonitoringResDTO
    {
        public long patient_enrollment_id { get; set; }
        public long? patient_report_id { get; set; }
        public long patient_id { get; set; }
        public string patient_name { get; set; }

        public DateTime? actual_hookup { get; set; }
        public DateTime? actual_removal { get; set; }

        public string enrollment_status { get; set; }
        public bool is_active { get; set; }

        public long doctor_id { get; set; }
        public string doctor_name { get; set; }

    }


    public class DoctorLivePatientMonitoringDTO
    {
        public long patient_enrollment_id { get; set; }
        public string event_processed_path { get; set; }
        public long patient_id { get; set; }
        public string patient_name { get; set; }
        public long device_id { get; set; }
        public string device_name { get; set; }

        internal static string getBy_patient_live_monitoring_details()
        {
            return $@" select top 1 pl.patient_enrollment_id,pl.event_processed_path,p.patient_id,p.patient_name,dm.device_id,dm.device_name
                        from tbl_patient_base_line as pl
                        inner join tbl_patient_enrollment as pe
                        on pl.patient_enrollment_id = pe.patient_enrollment_id
                        inner join tbl_patient as p
                        on p.patient_id = pe.patient_id
                        inner join tbl_device_mast as dm
                        on dm.device_id = pe.device_id
                        where pl.patient_enrollment_id = @patient_enrollment_id and pe.patient_id = @patient_id and  pe.is_active =1 
                        order by pl.patient_base_line_id desc ";
        }

    }


    public class DoctorLivePatientDisclosureReqDTO
    {
        public long patient_id { get; set; }
        public long patient_enrollment_id { get; set; }
        public DateTime? from_datetime { get; set; }

        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public int offSet => (pageNumber - 1) * pageSize;
        public string sortBy { get; set; }
        public string orderBy { get; set; }
    }


    public class DoctorLivePatientDisclosureResDTO
    {
        public long patient_base_line_id { get; set; }

        public long patient_enrollment_id { get; set; }

        public long patient_id { get; set; }

        public DateTime? actual_hookup { get; set; }

        public DateTime? record_st { get; set; }

        public DateTime? record_end { get; set; }

        public string? event_processed_path { get; set; }

        internal static string select_doctor_live_ecg_files(DoctorLivePatientDisclosureReqDTO obj)
        {
            string sortBy = string.IsNullOrWhiteSpace(obj.sortBy) ? "record_st" : obj.sortBy;

            string orderBy = string.Equals(obj.orderBy, "DESC", StringComparison.OrdinalIgnoreCase) ? "DESC" : "ASC";

            if (sortBy != "record_st" && sortBy != "record_end" && sortBy != "patient_base_line_id")
            {
                sortBy = "record_st";
            }

            return $@"
                        SELECT COUNT(*) AS totalCount
                        FROM tbl_patient_base_line AS pl
                        INNER JOIN tbl_patient_enrollment AS pe
                            ON pe.patient_enrollment_id = pl.patient_enrollment_id
                        WHERE pe.patient_id = @patient_id
                            AND pe.patient_enrollment_id = @patient_enrollment_id
                            AND pe.enrollment_status = 'monitoring'
                            AND pl.is_active = 1
                            AND pl.record_st >= @from_datetime
                            AND pl.record_st <= GETDATE()
                            AND pl.record_end > pe.actual_hookup;


                        SELECT
                            pl.patient_base_line_id,
                            pl.patient_enrollment_id,
                            pe.patient_id,
                            pe.actual_hookup,
                            pl.record_st,
                            pl.record_end,
                            pl.event_processed_path
                        FROM tbl_patient_base_line AS pl
                        INNER JOIN tbl_patient_enrollment AS pe
                            ON pe.patient_enrollment_id = pl.patient_enrollment_id
                        WHERE pe.patient_id = @patient_id
                            AND pe.patient_enrollment_id = @patient_enrollment_id
                            AND pe.enrollment_status = 'monitoring'
                            AND pl.is_active = 1
                            AND pl.record_st >= @from_datetime
                            AND pl.record_st <= GETDATE()
                            AND pl.record_end > pe.actual_hookup
                        ORDER BY pl.{sortBy} {orderBy}
                        OFFSET @offSet ROWS
                        FETCH NEXT @pageSize ROWS ONLY;";
        }
    }

}
