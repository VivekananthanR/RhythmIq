using elr_API.Common;

namespace elr_API.DTO.ReqDTO.EnrolmentReq
{
    public class EnrollmentReqDTO
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

        internal static string check_valid_otp()
        {
            return $@"select otp,expiry_date from {Tbl.patient} where phone_no = @phone_no ";
        }
        internal static string Clear_otp()
        {
            return $@"UPDATE {Tbl.patient} SET otp = NULL, expiry_date = NULL 
              WHERE phone_no = @phone_no";
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

        public long state_id { get; set; }
        public string state_name { get; set; }
        public long country_id { get; set; }
        public string country_name { get; set; }

        public long created_by { get; set; }
        public DateTime? created_datetime { get; set; }
        public long modified_by { get; set; }
        public DateTime? modified_datetime { get; set; }
        public bool is_active { get; set; }


    }


    public class EnrollmentRawDataDTO
    {
        public long enrolment_raw_data_id { get; set; }
        public long patient_enrolment_id { get; set; }
        public long symptoms_id { get; set; }
        public DateTime? data_start { get; set; }
        public DateTime? data_end { get; set; }
        public IFormFile file_name { get; set; }
        public string file_path { get; set; }
        public int battery_percentage { get; set; }
        public string auto_check_status { get; set; } = "waiting";
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
                  (patient_enrollment_id,symptoms_id, data_start,data_end,file_path,battery_percentage,auto_check_status, auto_check_datetime, created_by) 
               VALUES(@patient_enrolment_id,@symptoms_id, @data_start,@data_end,@file_path,@battery_percentage,@auto_check_status, getdate(), @created_by)";

        }

        internal static string get_raw_data()
        {
            return $@"select top 1 erd.enrolment_raw_data_id, erd.patient_enrolment_id, erd.symptoms_id, erd.data_start, erd.data_end, erd.file_path, erd.battery_percentage,
                    erd.auto_check_status, erd.waiting_datetime, erd.checking_datetime, erd.completed_datetime, erd.created_by, erd.created_datetime, erd.is_active, 
                    from {Tbl.enrollment_raw_data} as erd 
                    left join {Tbl.patient_enrolment} as pe
                    on erd.patient_enrollment_id = pe.patient_enrollment_id
                    left join {Tbl.symptoms} as sy
                    erd.symptoms_id = sy.symptoms_id
                    where auto_check_status = 'waiting'  and is_active = 1 
                    order by enrolment_raw_data_id asc;";
        }
        internal static string update_auto_check_status()
        {
            return $@"update {Tbl.enrollment_raw_data}
                       set auto_check_status = @auto_check_status , auto_check_datetime = @auto_check_datetime
                        where enrolment_raw_data_id = @enrolment_raw_data_id";
        }

        #endregion

    }
    public class AddUpdateQueuedEventDTO
    {
        public long queued_events_records_id { get; set; }
        public long? patient_enrolment_id { get; set; }
        public long enrolment_raw_data_id { get; set; }
        public int priority_level { get; set; }
        public string trigger_type { get; set; }
        public int hr_min { get; set; }
        public int hr_max { get; set; }
        public int hr_avg { get; set; }
        public int event_duration_seconds { get; set; }
        public string event_status { get; set; }
        public DateTime? entry_datetime { get; set; }
        public IFormFile file_name { get; set; }
        public string file_path { get; set; }
        public long created_by { get; set; }
        public DateTime? created_datetime { get; set; }
        public long modified_by { get; set; }
        public DateTime? modified_datetime { get; set; }
        public bool is_active { get; set; }

        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public int offset => (pageNumber - 1) * pageSize;
        public string sortColumn { get; set; }
        public string orderBy { get; set; }

        #region Query
        internal static string insert_queued_event()
        {
            return $@"INSERT INTO {Tbl.queued_events_records}
                        (enrolment_raw_data_id, priority_level,trigger_type,hr_min, hr_max,hr_avg, event_duration_seconds,event_status,entry_datetime,file_path,created_by)
                     values ( @enrolment_raw_data_id, @priority_level, @trigger_type, @hr_min, @hr_max,@hr_avg, @event_duration_seconds, @event_status, @entry_datetime, @file_path, @created_by)";
        }

        internal static string get_queued_event(AddUpdateQueuedEventDTO obj)
        {
            return $@" select count(*) 
                       from {Tbl.queued_events_records}
                        where is_active =1 
                        and (@priority_level = '' or @priority_level= null or @priority_level = 0 or priority_level = @priority_level)
                        and (@trigger_type ='' or @trigger_type = null or trigger_type = @trigger_type);

                        select * 
                        from {Tbl.queued_events_records}
                        where is_active = 1 
                        and (@priority_level = '' or @priority_level= null or @priority_level = 0 or priority_level = @priority_level)
                        and (@trigger_type ='' or @trigger_type = null or trigger_type = @trigger_type)
                        ORDER BY {obj.sortColumn} {obj.orderBy}
                        OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY ";
        }
        #endregion

    }
}
