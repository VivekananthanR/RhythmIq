using elr_API.Common;
using System.Text.Json;

namespace elr_API.DTO.ReqDTO
{
    public class HomeReqDTO
    {
    }

    public class MenuDTO
    {
        public long menu_id { get; set; }
        public string menu_name { get; set; }
        public string router_link { get; set; }
        public int priority { get; set; }
        public string permission_menu { get; set; }
        public long created_by { get; set; }
        public DateTime? created_datetime { get; set; }
        public long modified_by { get; set; }
        public DateTime? modified_datetime { get; set; }
        public bool is_active { get; set; }

        #region Query

        internal static string select_menu()
        {
            return $@"select * from {Tbl.menu_mast} where is_active = 1 ";
        }
        internal static string insert_menu()
        {
            return $@"insert into {Tbl.menu_mast} (menu_name, router_link, priority, permission_menu, created_by)
                        values( @menu_name, @router_link ,@priority, @permission_menu, @created_by)";
        }
        internal static string update_menu()
        {
            return $@"update {Tbl.menu_mast}
                        set menu_name = @menu_name, router_link = @router_link, priority = @priority, permission_menu = @permission_menu, 
                            modified_by = @modified_by, modified_datetime = getdate(), is_active = @is_active
                          where menu_id =@menu_id";
        }
        #endregion
    }

    public class CountryDTO
    {
        public long country_id { get; set; }
        public string country_code { get; set; }
        public string country_name { get; set; }
        public bool is_active { get; set; }

        // Search field
        public string search_text { get; set; }

        #region Query

        internal static string insert_country()
        {
            return $@"INSERT INTO tbl_country
                    (country_code, country_name, is_active)
                  VALUES
                    (@country_code, @country_name, @is_active)";
        }

        internal static string update_country()
        {
            return $@"UPDATE tbl_country SET
                    country_code = @country_code,
                    country_name = @country_name,
                    is_active    = @is_active
                  WHERE country_id = @country_id";
        }

        internal static string select_country()
        {
            return $@"SELECT
                    country_id,
                    country_code,
                    country_name,
                    is_active
                  FROM tbl_country
                  WHERE is_active = 1
                  AND (@country_id  = 0   OR country_id   = @country_id)
                  AND (@search_text IS NULL OR country_name LIKE '%' + @search_text + '%')";
        }

        #endregion
    }

    public class StateDTO
    {
        public long state_id { get; set; }
        public string state_code { get; set; }
        public long country_id { get; set; }
        public string state_name { get; set; }
        public bool is_active { get; set; }

        // Join field
        public string country_name { get; set; }

        // Search field
        public string search_text { get; set; }

        #region Query

        internal static string insert_state()
        {
            return $@"INSERT INTO tbl_state
                    (state_code, country_id, state_name, is_active)
                  VALUES
                    (@state_code, @country_id, @state_name, @is_active)";
        }

        internal static string update_state()
        {
            return $@"UPDATE tbl_state SET
                    state_code  = @state_code,
                    country_id  = @country_id,
                    state_name  = @state_name,
                    is_active   = @is_active
                  WHERE state_id = @state_id";
        }

        internal static string select_state()
        {
            return $@"SELECT
                    s.state_id,
                    s.state_code,
                    s.country_id,
                    s.state_name,
                    s.is_active,
                    c.country_name
                  FROM tbl_state s
                  LEFT JOIN tbl_country c ON c.country_id = s.country_id
                  WHERE s.is_active = 1
                  AND (@state_id   = 0    OR s.state_id   = @state_id)
                  AND (@country_id = 0    OR s.country_id = @country_id)
                  AND (@search_text IS NULL OR s.state_name LIKE '%' + @search_text + '%')";
        }

        #endregion
    }

    public class DistrictDTO
    {
        public long district_id { get; set; }
        public string district_code { get; set; }
        public long state_id { get; set; }
        public string district_name { get; set; }
        public bool is_active { get; set; }

        // Join fields
        public string state_name { get; set; }
        public string country_name { get; set; }
        public long country_id { get; set; }

        // Search field
        public string search_text { get; set; }

        #region Query

        internal static string insert_district()
        {
            return $@"INSERT INTO tbl_district
                    (district_code, state_id, district_name, is_active)
                  VALUES
                    (@district_code, @state_id, @district_name, @is_active)";
        }

        internal static string update_district()
        {
            return $@"UPDATE tbl_district SET
                    district_code = @district_code,
                    state_id      = @state_id,
                    district_name = @district_name,
                    is_active     = @is_active
                  WHERE district_id = @district_id";
        }

        internal static string select_district()
        {
            return $@"SELECT
                    d.district_id,
                    d.district_code,
                    d.state_id,
                    d.district_name,
                    d.is_active,
                    s.state_name,
                    s.country_id,
                    c.country_name
                  FROM tbl_district d
                  LEFT JOIN tbl_state   s ON s.state_id   = d.state_id
                  LEFT JOIN tbl_country c ON c.country_id = s.country_id
                  WHERE d.is_active = 1
                  AND (@district_id = 0    OR d.district_id = @district_id)
                  AND (@state_id    = 0    OR d.state_id    = @state_id)
                  AND (@country_id  = 0    OR s.country_id  = @country_id)
                  AND (@search_text IS NULL OR d.district_name LIKE '%' + @search_text + '%')";
        }

        #endregion
    }
    public class UserTypeDTO
    {
        public long user_type_id { get; set; }
        public string user_type { get; set; }
        public long created_by { get; set; }
        public DateTime? created_datetime { get; set; }
        public long modified_by { get; set; }
        public DateTime? modified_datetime { get; set; }
        public bool is_active { get; set; }

        #region Query
        internal static string select_user_type()
        {
            return $@"select * from {Tbl.user_type} where is_active = 1 and (user_type_id = 0 OR user_type_id = @user_type_id)";
        }

        internal static string insert_user_type()
        {
            return $@" Insert Into {Tbl.user_type} (user_type,created_by)
                       values (@user_type, @created_by)";
        }
        internal static string update_user_type()
        {
            return $@"Update {Tbl.user_type}
                        Set user_type = @user_type, modified_by = @modified_by, modified_datetime = getdate(), is_active = @is_active
                        where user_type_id =user_type_id";
        }
        #endregion
    }


    public class AddUpdateHospitalReqDTO
    {
        public long hospital_id { get; set; }
        public string hospital_name { get; set; }
        public string full_address { get; set; }
        public string phone_no { get; set; }
        public string city { get; set; }
        public long? district_id { get; set; }
        public long? state_id { get; set; }
        public long? country_id { get; set; }
        public string pincode { get; set; }
        public string location_latitude { get; set; }
        public string location_longitude { get; set; }

        public long created_by { get; set; }
        public DateTime? created_datetime { get; set; }
        public long modified_by { get; set; }
        public DateTime? modified_datetime { get; set; }
        public bool is_active { get; set; }

        #region Query

        internal static string select_hospital()
        {
            return $@"
        SELECT 
            hospital_id,
            hospital_name,
            full_address,
            phone_no,
            city,
            district_id,
            state_id,
            country_id,
            pincode,
            location_latitude,
            location_longitude,
            is_active
        FROM {Tbl.hospital_mast}
        WHERE is_active = 1
          AND (@hospital_name IS NULL OR hospital_name LIKE '%' + @hospital_name + '%')
        ORDER BY hospital_name ASC";
        }

        internal static string insert_hospital()
        {
            return $@"insert into {Tbl.hospital_mast}
                        (hospital_name, full_address, phone_no, city, district_id, state_id,country_id, pincode, location_latitude, location_longitude, created_by)
                        values(@hospital_name, @full_address, @phone_no, @city, @district_id, @state_id, @country_id, @pincode, @location_latitude, @location_longitude, @created_by)";
        }

        internal static string update_hospital()
        {
            return $@"update {Tbl.hospital_mast}
                       set hospital_name =@hospital_name, full_address =@full_address, phone_no =@phone_no, city =@city, district_id =@district_id, state_id =@state_id,country_id =@country_id, pincode = @pincode, 
                            location_latitude =@location_latitude, location_longitude =@location_longitude , modified_by= @modified_by,modified_datetime =getdate(),is_active = @is_active
                        where hospital_id = @hospital_id";
        }
        #endregion
    }

    public class AddUpdateTestDTO
    {
        public long test_id { get; set; }
        public int duration_in_day { get; set; }
        public string test_name { get; set; }

        public long created_by { get; set; }
        public DateTime? created_datetime { get; set; }
        public long modified_by { get; set; }
        public DateTime? modified_datetime { get; set; }
        public bool is_active { get; set; }

        #region Query

        internal static string insert_test()
        {
            return $@"INSERT INTO {Tbl.test_mast}  (duration_in_day, test_name, created_by)
                         VALUES (@duration_in_day, @test_name, @created_by)";
        }

        internal static string update_test()
        {
            return $@"UPDATE  {Tbl.test_mast} SET
                    duration_in_day     = @duration_in_day,
                    test_name           = @test_name,
                    modified_by         = @modified_by,
                    modified_datetime   = GETDATE(),
                    is_active           = @is_active
                  WHERE test_id = @test_id";
        }

        internal static string select_test()
        {
            return $@"SELECT 
                    test_id,
                    duration_in_day,
                    test_name,
                    created_by,
                    created_datetime,
                    modified_by,
                    modified_datetime,
                    is_active
                  FROM  {Tbl.test_mast}
                  WHERE is_active = 1";
        }
        #endregion
    }

    public class AddUpdateTestPricesDTO
    {
        public long test_prices_id { get; set; }
        public long location_id { get; set; }
        public long test_id { get; set; }
        public decimal test_price { get; set; }

        public long created_by { get; set; }
        public DateTime? created_datetime { get; set; }
        public long modified_by { get; set; }
        public DateTime? modified_datetime { get; set; }
        public bool is_active { get; set; }

        public string test_name { get; set; }
        public int duration_in_day { get; set; }
        public string branch_name { get; set; }

        #region Query

        internal static string insert_test_prices()
        {
            return $@"INSERT INTO {Tbl.test_prices} (location_id, test_id, test_price, created_by)
                  VALUES (@location_id, @test_id, @test_price, @created_by)";
        }

        internal static string update_test_prices()
        {
            return $@"UPDATE {Tbl.test_prices} SET
                    location_id       = @location_id,
                    test_id           = @test_id,
                    test_price        = @test_price,
                    modified_by       = @modified_by,
                    modified_datetime = GETDATE(),
                    is_active         = @is_active
                  WHERE test_prices_id = @test_prices_id";
        }

        internal static string select_test_prices()
        {
            return $@"SELECT 
                    tp.test_prices_id,
                    tp.location_id,
                    tp.test_id,
                    tp.test_price,
                    tp.is_active,
                    tp.created_by,
                    tp.created_datetime,
                    tp.modified_by,
                    tp.modified_datetime,
                    tm.test_name,
                    tm.duration_in_day,
                    lm.branch_name
                  FROM {Tbl.test_prices} tp
                  LEFT JOIN {Tbl.test_mast} tm  ON tm.test_id       = tp.test_id
                  LEFT JOIN {Tbl.location_mast} lm ON lm.location_id = tp.location_id
                  WHERE tp.is_active = 1
                  AND   (@test_id      = 0 OR tp.test_id      = @test_id)
                  AND   (@location_id  = 0 OR tp.location_id  = @location_id)";
        }

        #endregion
    }

    #region 
    //public class HospitalDTO
    //{
    //    public long hospital_id { get; set; }
    //    public string hospital_name { get; set; }
    //    public string full_address { get; set; }
    //    public string phone_no { get; set; }
    //    public string city { get; set; }
    //    public long district_id { get; set; }
    //    public long state_id { get; set; }
    //    public long country_id { get; set; }
    //    public string pincode { get; set; }
    //    public string location_latitude { get; set; }
    //    public string location_longitude { get; set; }

    //    public long created_by { get; set; }
    //    public DateTime? created_datetime { get; set; }
    //    public long modified_by { get; set; }
    //    public DateTime? modified_datetime { get; set; }
    //    public bool is_active { get; set; }

    //    // Search field
    //    public string search_text { get; set; }

    //    #region Query

    //    internal static string insert_hospital()
    //    {
    //        return $@"INSERT INTO tbl_hospital_mast
    //                (hospital_name, full_address, phone_no, city,
    //                 district_id, state_id, country_id, pincode,
    //                 location_latitude, location_longitude,
    //                 created_by)
    //              VALUES
    //                (@hospital_name, @full_address, @phone_no, @city,
    //                 @district_id, @state_id, @country_id, @pincode,
    //                 @location_latitude, @location_longitude,
    //                 @created_by)";
    //    }

    //    internal static string update_hospital()
    //    {
    //        return $@"UPDATE tbl_hospital_mast SET
    //                hospital_name      = @hospital_name,
    //                full_address       = @full_address,
    //                phone_no           = @phone_no,
    //                city               = @city,
    //                district_id        = @district_id,
    //                state_id           = @state_id,
    //                country_id         = @country_id,
    //                pincode            = @pincode,
    //                location_latitude  = @location_latitude,
    //                location_longitude = @location_longitude,
    //                modified_by        = @modified_by,
    //                modified_datetime  = GETDATE(),
    //                is_active          = @is_active
    //              WHERE hospital_id = @hospital_id";
    //    }

    //    internal static string select_hospital()
    //    {
    //        return $@"SELECT
    //                hospital_id,
    //                hospital_name,
    //                full_address,
    //                phone_no,
    //                city,
    //                district_id,
    //                state_id,
    //                country_id,
    //                pincode,
    //                location_latitude,
    //                location_longitude,
    //                is_active,
    //                created_by,
    //                created_datetime
    //              FROM tbl_hospital_mast
    //              WHERE is_active = 1
    //              AND (@hospital_id  = 0 OR hospital_id = @hospital_id)
    //              AND (@search_text  IS NULL OR hospital_name LIKE '%' + @search_text + '%')";
    //    }

    //    #endregion
    //}
    #endregion

    public class SymtomsTypeDTO
    {
        public long symptoms_type_id { get; set; }
        public string symptoms_type { get; set; }

        public long created_by { get; set; }
        public DateTime? created_datetime { get; set; }
        public long modified_by { get; set; }
        public DateTime? modified_datetime { get; set; }
        public bool is_active { get; set; }
        #region Query
        internal static string select_symptoms_List()
        {
            return $@"select * from {Tbl.symptoms_type}";
        }
        #endregion
    }
    public class LocationDTO
    {
        public long location_id { get; set; }
        public string branch_name { get; set; }
        public string phone_no { get; set; }
        public string landline_no { get; set; }
        public string email_id { get; set; }
        public string house_no_and_name { get; set; }
        public string street_name { get; set; }
        public string area_name { get; set; }
        public string city { get; set; }
        public long district_id { get; set; }
        public long state_id { get; set; }
        public long country_id { get; set; }
        public string pincode { get; set; }
        public string direction_link { get; set; }
        public string location_latitude { get; set; }
        public string location_longitude { get; set; }

        public long created_by { get; set; }
        public DateTime? created_datetime { get; set; }
        public long modified_by { get; set; }
        public DateTime? modified_datetime { get; set; }
        public bool is_active { get; set; }

        // Search field
        public string search_text { get; set; }

        #region Query

        internal static string insert_location()
        {
            return $@"INSERT INTO tbl_location_mast
                    (branch_name, phone_no, landline_no, email_id,
                     house_no_and_name, street_name, area_name, city,
                     district_id, state_id, country_id, pincode,
                     direction_link, location_latitude, location_longitude,
                     created_by)
                  VALUES
                    (@branch_name, @phone_no, @landline_no, @email_id,
                     @house_no_and_name, @street_name, @area_name, @city,
                     @district_id, @state_id, @country_id, @pincode,
                     @direction_link, @location_latitude, @location_longitude,
                     @created_by)";
        }

        internal static string update_location()
        {
            return $@"UPDATE tbl_location_mast SET
                    branch_name        = @branch_name,
                    phone_no           = @phone_no,
                    landline_no        = @landline_no,
                    email_id           = @email_id,
                    house_no_and_name  = @house_no_and_name,
                    street_name        = @street_name,
                    area_name          = @area_name,
                    city               = @city,
                    district_id        = @district_id,
                    state_id           = @state_id,
                    country_id         = @country_id,
                    pincode            = @pincode,
                    direction_link     = @direction_link,
                    location_latitude  = @location_latitude,
                    location_longitude = @location_longitude,
                    modified_by        = @modified_by,
                    modified_datetime  = GETDATE(),
                    is_active          = @is_active
                  WHERE location_id = @location_id";
        }

        internal static string select_location()
        {
            return $@"SELECT
                    location_id,
                    branch_name,
                    phone_no,
                    landline_no,
                    email_id,
                    house_no_and_name,
                    street_name,
                    area_name,
                    city,
                    district_id,
                    state_id,
                    country_id,
                    pincode,
                    direction_link,
                    location_latitude,
                    location_longitude,
                    is_active
                  FROM tbl_location_mast
                  WHERE is_active = 1
                  AND (@location_id = 0 OR location_id = @location_id)
                  AND (@search_text IS NULL OR branch_name LIKE '%' + @search_text + '%')";
        }

        #endregion
    }



    public class DeviceDTO
    {
        public long device_id { get; set; }
        public string device_no { get; set; }
        public string device_name { get; set; }


        #region Query

        internal static string check_valid_device()
        {
            return $@"select * from {Tbl.device_mast} where device_no =@device_no and is_active = 1";
        }
        #endregion
    }

    public class PatchDTO
    {
        public long patch_id { get; set; }
        public string patch_no { get; set; }


        #region Query

        internal static string check_valid_patch()
        {
            return $@"select * from {Tbl.patch_mast} where patch_no =@patch_no and is_active = 1";
        }
        #endregion
    }

    public class QueuedAnomalyReqDTO
    {
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public int offset => (pageNumber - 1) * pageSize;
        public string shortColumn { get; set; }
        public string orderBy { get; set; }

        #region Query   
        internal static string queue_anomaly_list(QueuedAnomalyReqDTO obj)
        {
            return $@"select count(1) from {Tbl.queued_events_records} as a where a.is_active = 1 and a.event_status <> 'partial-completed'

                        SELECT
                            a.queued_events_records_id,
                            a.patient_enrollment_id,
                            a.enrollment_raw_data_id,
                            a.priority_level,
                            a.trigger_type,
                            a.bpm,
                            a.event_status,
                            a.event_start_datetime,
                            a.event_end_datetime,
                            a.event_file_path,
                            a.created_datetime,

                            -- Patient Details
                            p.patient_id,
                            p.patient_name,
                            p.age,
                            p.gender,
                            p.phone_no,
                            p.email_id,

                            -- Technician Details
                            pe.actual_technician_id,
                            u.user_name AS technician_name,
                            u.emp_code AS technician_emp_code,
                            u.phone_no AS technician_phone_no,
                            u.email_id AS technician_email_id,

                            -- Device Details
                            d.device_id,
                            d.device_no,      
                            d.device_name   
    

                            FROM {Tbl.queued_events_records} a

                            LEFT JOIN {Tbl.patient_enrollment} pe
                                ON a.patient_enrollment_id = pe.patient_enrollment_id

                            LEFT JOIN {Tbl.patient} p
                                ON pe.patient_id = p.patient_id

                            LEFT JOIN {Tbl.user_master} u
                                ON pe.actual_technician_id = u.user_id

                            LEFT JOIN {Tbl.device_mast} d
                                ON pe.device_id = d.device_id

                                where a.is_active = 1
                                        and a.event_status <> 'partial-completed'

                                order by {obj.shortColumn} {obj.orderBy}
                                offset @offset rows fetch next @pageSize rows only";
        }

        #endregion
    }

    public class QueuedAnomalyDTO
    {
        public long queued_events_records_id { get; set; }
        public long patient_enrollment_id { get; set; }
        public long enrollment_raw_data_id { get; set; }
        public string priority_level { get; set; }
        public string trigger_type { get; set; }
        public int bpm { get; set; }
        public string event_status { get; set; }
        public DateTime? event_start_datetime { get; set; }
        public DateTime? event_end_datetime { get; set; }
        public IFormFile? file_name { get; set; }
        public string event_file_path { get; set; }


        // Patient
        public long patient_id { get; set; }
        public string patient_name { get; set; }
        public int? age { get; set; }
        public string gender { get; set; }
        public string patient_phone_no { get; set; }


        // Technician
        public long? actual_technician_id { get; set; }
        public string technician_name { get; set; }
        public string technician_emp_code { get; set; }
        public string technician_phone_no { get; set; }

        public long device_id { get; set; }
        public string device_no { get; set; }
        public string device_name { get; set; }

        public long created_by { get; set; }
        public DateTime? created_datetime { get; set; }
        public DateTime? processing_datetime { get; set; }
        public long modified_by { get; set; }
        public DateTime? modified_datetime { get; set; }
        public bool is_active { get; set; }

        #region Query
        internal static string insert_queued_event()
        {
            return $@"INSERT INTO {Tbl.queued_events_records}
                        (patient_enrollment_id,enrollment_raw_data_id, priority_level,trigger_type,bpm,event_status,event_start_datetime,event_end_datetime,event_file_path,created_by)
                     values (@patient_enrollment_id, @enrollment_raw_data_id, @priority_level, @trigger_type, @bpm, @event_status, @event_start_datetime,@event_end_datetime, @event_file_path, @created_by)";
        }
        internal static string reset_event_status()
        {
            return $@"
                    UPDATE {Tbl.queued_events_records}
                    SET
                        event_status = 'pending',
                        modified_datetime = GETDATE()
                    WHERE queued_events_records_id = @queued_events_records_id";
        }
        internal static string update_event_processing()
        {
            return @$"
                    UPDATE {Tbl.queued_events_records}
                    SET event_status = 'processing',
                        processing_datetime = GETDATE()
                    WHERE queued_events_records_id = @queued_events_records_id ";
        }

        internal static string update_event_status()
        {
            return @$"
                    UPDATE {Tbl.queued_events_records}
                    SET event_status = @event_status,
                        modified_by = @modified_by,
                        last_assigned_datetime = GETDATE()
                    WHERE queued_events_records_id = @queued_events_records_id";
        }
        #endregion

    }

    public class GetNextEventResDTO
    {
        public long queued_events_records_id { get; set; }
        public long patient_enrollment_id { get; set; }
        public long enrollment_raw_data_id { get; set; }
        public string priority_level { get; set; }
        public string trigger_type { get; set; }
        public int bpm { get; set; }
        public string event_status { get; set; }
        public string event_file_path { get; set; }
        public long created_by { get; set; }
        public DateTime? created_datetime { get; set; }
        public DateTime? processing_datetime { get; set; }


        public long patient_id { get; set; }
        public string patient_name { get; set; }

        public long device_id { get; set; }
        public string device_no { get; set; }
        public string device_name { get; set; }

        internal static string get_next_event()
        {
            return $@"
                ;WITH cte AS
                (
                    SELECT TOP 1
                        q.queued_events_records_id
                    FROM {Tbl.queued_events_records} q
                        WITH (UPDLOCK, READPAST, ROWLOCK)
                    WHERE q.event_status = 'pending'
                    AND NOT EXISTS
                    (
                        SELECT 1
                        FROM {Tbl.event_assign} ea
                        WHERE ea.queued_events_records_id = q.queued_events_records_id
                        AND ea.work_status = 'inprogress'
                    )
                    ORDER BY
                        CASE q.priority_level
                            WHEN 'high' THEN 1
                            WHEN 'medium' THEN 2
                            WHEN 'low' THEN 3
                            ELSE 4
                        END,
                        CASE q.trigger_type
                            WHEN 'AF' THEN 1
                            WHEN 'VT' THEN 2
                            WHEN 'Pause' THEN 3
                            WHEN 'Brady' THEN 4
                            WHEN 'Tachy' THEN 5
                            ELSE 6
                        END,
                        q.created_datetime ASC
                )
                UPDATE q
                SET
                    q.event_status = 'processing',
                    q.processing_datetime = GETDATE()
                OUTPUT INSERTED.queued_events_records_id
                FROM {Tbl.queued_events_records} q
                INNER JOIN cte
                    ON cte.queued_events_records_id = q.queued_events_records_id;";
        }

        internal static string get_event_details()
        {
            return $@"
        SELECT
            q.queued_events_records_id,
            q.patient_enrollment_id,
            q.enrollment_raw_data_id,
            q.priority_level,
            q.trigger_type,
            q.bpm,
            q.event_status,
            q.event_file_path,
            q.created_by,
            q.created_datetime,
            q.processing_datetime,
            p.patient_id,
            p.patient_name,
            d.device_id,
            d.device_no,
            d.device_name
        FROM {Tbl.queued_events_records} q
        INNER JOIN {Tbl.patient_enrollment} pe
            ON pe.patient_enrollment_id = q.patient_enrollment_id
        INNER JOIN {Tbl.patient} p
            ON p.patient_id = pe.patient_id
        LEFT JOIN {Tbl.device_mast} d
            ON d.device_id = pe.device_id
        WHERE q.queued_events_records_id = @queued_events_records_id";
        }

    }
    public class EventAssignDTO
    {
        public long event_assign_id { get; set; }
        public long patient_enrollment_id { get; set; }
        public long queued_events_records_id { get; set; }
        public long assigned_by { get; set; }
        public DateTime? assigned_datetime { get; set; }
        public DateTime? analysis_start_datetime { get; set; }
        public DateTime? last_activity_datetime { get; set; }
        public string work_status { get; set; }
        public string remarks { get; set; }
        public DateTime? analysis_end_datetime { get; set; }
        public long created_by { get; set; }
        public DateTime? created_datetime { get; set; }
        public long modified_by { get; set; }
        public DateTime? modified_datetime { get; set; }
        public bool is_active { get; set; }
        #region Query
        internal static string event_assign_insert()
        {
            return $@" insert into {Tbl.event_assign}(patient_enrollment_id,queued_events_records_id,assigned_by,assigned_datetime,analysis_start_datetime,work_status,created_by)
                        values(@patient_enrollment_id, @queued_events_records_id, @assigned_by, getdate(),@analysis_start_datetime, @work_status, @created_by)";
        }
        internal static string reassign_expired_events()
        {
            return $@"
                    UPDATE {Tbl.event_assign}
                    SET
                        work_status = 'reassigned',
                        modified_by = assigned_by,
                        modified_datetime = GETDATE(),
                        last_activity_datetime = getdate(),
                        remarks = '10 Minutes Reached'
                    WHERE work_status = 'inprogress'
                    AND DATEDIFF(MINUTE,
                                 analysis_start_datetime,
                                 GETDATE()) >= 10";
        }


        internal static string reset_reassigned_events()
        {
            return $@"
                    UPDATE q
                    SET
                        q.event_status = 'pending',
                        q.modified_datetime = GETDATE()
                    FROM {Tbl.queued_events_records} q
                    WHERE EXISTS
                    (
                        SELECT 1
                        FROM {Tbl.event_assign} ea
                        WHERE ea.queued_events_records_id = q.queued_events_records_id
                          AND ea.work_status = 'reassigned'
                    )
                    AND NOT EXISTS
                    (
                        SELECT 1
                        FROM {Tbl.event_assign} ea
                        WHERE ea.queued_events_records_id = q.queued_events_records_id
                          AND ea.work_status = 'partial-completed'
                    )
                    AND q.event_status = 'processing';";
        }

        internal static string time_out_reassign_event()
        {
            return $@"
                    UPDATE {Tbl.event_assign}
                    SET
                        work_status = 'reassigned',
                        remarks = 'No activity for 3 minutes',
                        modified_by = @modified_by,
                        modified_datetime = GETDATE(),
                        last_activity_datetime = GETDATE()
                    WHERE queued_events_records_id = @queued_events_records_id
                    AND work_status = 'inprogress'";
        }
        internal static string get_active_event()
        {
            return $@"
                    SELECT COUNT(1)
                    FROM {Tbl.event_assign}
                    WHERE assigned_by = @emp_id
                    AND work_status = 'inprogress'";
        }
        internal static string update_work_status()
        {
            return $@"
                    UPDATE {Tbl.event_assign}
                    SET work_status = @work_status,
                        modified_by = @modified_by,
                        modified_datetime = GETDATE(),
                        analysis_end_datetime = GETDATE()
                    WHERE queued_events_records_id = @queued_events_records_id";
        }
        #endregion
    }


    public class ProcessedRawDataDTO
    {
        public long patient_base_line_id { get; set; }
        public long enrollment_raw_data_id { get; set; }
        public long patient_enrollment_id { get; set; }
        public DateTime? record_st { get; set; }
        public DateTime? record_end { get; set; }
        public int duration_seconds { get; set; }
        public IFormFile? event_parent_file { get; set; }
        public string? event_parent_path { get; set; }
        public IFormFile? event_processed_file { get; set; }
        public string? event_processed_path { get; set; }
        public long created_by { get; set; }
        public DateTime? created_datetime { get; set; }
        public long modified_by { get; set; }
        public DateTime? modified_datetime { get; set; }
        public bool is_active { get; set; }

        #region Query
        internal static string insert_processed_data()
        {
            return $@"insert into {Tbl.patient_base_line} (enrollment_raw_data_id,patient_enrollment_id,record_st,record_end,event_parent_path,event_processed_path)
                       values(@enrollment_raw_data_id, @patient_enrollment_id, @record_st, @record_end, @event_parent_path, @event_processed_path)";
        }
        #endregion
    }




    public class CurrentStripDTO
    {
        public DateTime? event_date_time { get; set; }
        public string event_file_path { get; set; }
        public string trigger_type { get; set; }
        public int bpm { get; set; }
        public long event_assign_id { get; set; }
        public long queued_events_records_id { get; set; }

        public List<SubStripEventResDTO> sub_strip { get; set; }

        #region Query
        internal static string get_current_pateint_strip()
        {
            return $@"
                    SELECT TOP 1
                        qer.bpm,
                        qer.trigger_type,
                        qer.created_datetime AS event_date_time,
                        qer.event_file_path,
                        ea.event_assign_id,
                        qer.queued_events_records_id
                    FROM {Tbl.patient_enrollment} pe
                    INNER JOIN {Tbl.event_assign} ea
                        ON pe.patient_enrollment_id = ea.patient_enrollment_id
                    INNER JOIN {Tbl.queued_events_records} qer
                        ON ea.queued_events_records_id = qer.queued_events_records_id
                    WHERE ea.queued_events_records_id = @queued_events_records_id
                      AND ea.is_active = 1
                      AND qer.is_active = 1 ";

        }

        #endregion
    }
    public class SubStripReqDTO
    {
        public long patient_id { get; set; }
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public int offset => (pageNumber - 1) * pageSize;
        public string? shortColumn { get; set; }
        public string? orderBy { get; set; }

        #region Query
        internal static string get_sub_strip_report(SubStripReqDTO obj)
        {
            return $@"
                    SELECT COUNT(1)
                    FROM {Tbl.sub_strip_event} sse
                    INNER JOIN {Tbl.queued_events_records} qer
                        ON sse.queued_events_records_id = qer.queued_events_records_id
                    INNER JOIN {Tbl.patient_enrollment} pe
                        ON qer.patient_enrollment_id = pe.patient_enrollment_id
                    WHERE pe.patient_id = @patient_id
                      AND sse.is_active = 1;

                    SELECT
                        sse.sub_strip_event_id,
                        sse.queued_events_records_id,
                        sse.event_assign_id,
                        sse.sub_strip_start_datetime,
                        sse.sub_strip_end_datetime,
                        sse.created_by,
                        sse.created_datetime,
                        sse.is_active
                    FROM {Tbl.sub_strip_event} sse
                    INNER JOIN {Tbl.queued_events_records} qer
                        ON sse.queued_events_records_id = qer.queued_events_records_id
                    INNER JOIN {Tbl.patient_enrollment} pe
                        ON qer.patient_enrollment_id = pe.patient_enrollment_id
                    WHERE pe.patient_id = @patient_id
                      AND sse.is_active = 1
                    ORDER BY sse.created_datetime DESC
                    OFFSET @offset ROWS
                    FETCH NEXT @pageSize ROWS ONLY;";
        }
        #endregion
    }

    public class SubStripEventDTO
    {
        public long sub_strip_event_id { get; set; }
        public long queued_events_records_id { get; set; }
        public long event_assign_id { get; set; }
        public string sub_strip_file_path { get; set; }
        public string? sub_strip_start_datetime { get; set; }
        public string? sub_strip_end_datetime { get; set; }
        public string remarks { get; set; }
        public JsonElement json_data { get; set; }

        public long created_by { get; set; }
        public DateTime? created_datetime { get; set; }
        public long modified_by { get; set; }
        public DateTime? modified_datetime { get; set; }
        public bool is_active { get; set; }

        #region Query
        internal static string insert_sub_strip()
        {
            return $@" insert into {Tbl.sub_strip_event}(queued_events_records_id,event_assign_id,sub_strip_file_path,sub_strip_start_datetime,sub_strip_end_datetime,remarks,created_by)
                       values(@queued_events_records_id, @event_assign_id, @sub_strip_file_path, @sub_strip_start_datetime, @sub_strip_end_datetime, @remarks, @created_by)";
        }


        internal static string get_sub_strip()
        {
            return $@"
                    SELECT
                        sub_strip_event_id,
                        queued_events_records_id,
                        event_assign_id,
                        sub_strip_file_path,
                        sub_strip_start_datetime,
                        sub_strip_end_datetime
                    FROM {Tbl.sub_strip_event}
                    WHERE queued_events_records_id = @queued_events_records_id
                      AND is_active = 1
                       Order by created_datetime desc";
        }

        internal static string in_active_sub_strip()
        {
            return $@"Update {Tbl.sub_strip_event}
                        set is_active = 0,
                        modified_by = @modified_by,
                        modified_datetime = getdate()
                        where sub_strip_event_id = @sub_strip_event_id";
        }


        #endregion
    }
    public class SubStripEventResDTO
    {
        public long sub_strip_event_id { get; set; }
        public long queued_events_records_id { get; set; }
        public long event_assign_id { get; set; }
        public string sub_strip_file_path { get; set; }
        public string? sub_strip_start_datetime { get; set; }
        public string? sub_strip_end_datetime { get; set; }
        public string remarks { get; set; }
        public long created_by { get; set; }
        public DateTime? created_datetime { get; set; }
        public long modified_by { get; set; }
        public DateTime? modified_datetime { get; set; }
        public bool is_active { get; set; }
    }
    public class FindingEventDTO
    {
        public long event_finding_id { get; set; }
        public string finding_name { get; set; }
        public long? parent_finding_id { get; set; }
        public long created_by { get; set; }
        public DateTime? created_datetime { get; set; }
        public long modified_by { get; set; }
        public DateTime? modified_datetime { get; set; }
        public bool is_active { get; set; }
        public List<FindingEventDTO> child_list { get; set; }

        #region Query
        internal static string insert_finding_mast()
        {
            return $@"insert into {Tbl.event_finding_mast}(finding_name,parent_finding_id,created_by)
                        values(@finding_name,@parent_finding_id,@created_by)";
        }
        internal static string update_finding_mast()
        {
            return $@"UPDATE {Tbl.event_finding_mast}
              SET finding_name = @finding_name,
                  parent_finding_id = @parent_finding_id,
                  modified_by = @modified_by,
                  modified_datetime = GETDATE(),
                  is_active = @is_active
              WHERE event_finding_id = @event_finding_id";
        }
        internal static string get_finding_mast()
        {
            return $@"
                    SELECT
                        event_finding_id,
                        finding_name,
                        parent_finding_id,
                        is_active
                    FROM {Tbl.event_finding_mast}
                    WHERE is_active = 1
                      AND parent_finding_id IS NULL";
        }
        internal static string get_finding_parent_id()
        {
            return $@"
                    SELECT
                        event_finding_id,
                        finding_name,
                        parent_finding_id,
                        is_active
                    FROM {Tbl.event_finding_mast}
                    WHERE is_active = 1
                      AND parent_finding_id = @parent_finding_id";
        }
        #endregion


    }

    public class ParentStripDTO
    {
        public long? patient_enrollment_id { get; set; }
        public DateTime? created_datetime { get; set; }
        public long? queued_events_records_id { get; set; }
        public string trigger_type { get; set; }
        public long? sub_strip_event_id { get; set; }
        public string sub_strip_file_path { get; set; }

        #region Query
        internal static string select_parent_strip()
        {
            return $@"
                    SELECT
                        pe.patient_enrollment_id,
                        pe.created_datetime,
                        qer.queued_events_records_id,
                        qer.trigger_type,
                        sse.sub_strip_event_id,
                        sse.sub_strip_file_path
                    FROM {Tbl.sub_strip_event} sse
                    INNER JOIN {Tbl.queued_events_records} qer
                        ON sse.queued_events_records_id = qer.queued_events_records_id
                    INNER JOIN {Tbl.patient_enrollment} pe
                        ON qer.patient_enrollment_id = pe.patient_enrollment_id
                    WHERE sse.sub_strip_event_id = @sub_strip_event_id";
        }
        #endregion
    }



    public class PushToQCDTO
    {
        public long sub_strip_event_finding_id { get; set; }
        public long? sub_strip_event_id { get; set; }
        public long? queued_events_records_id { get; set; }

        public List<FindingDTO> finding_list { get; set; }

        public long created_by { get; set; }
        public DateTime? created_datetime { get; set; }
        public long? modified_by { get; set; }
        public DateTime? modified_datetime { get; set; }
        public bool is_active { get; set; }

        public string remarks { get; set; }

        public bool is_experienced_user { get; set; }


        #region Query
        internal static string insert_sub_strip_finding()
        {
            return $@"
                    INSERT INTO {Tbl.sub_strip_event_finding}
                    (
                        detection_id,
                        queued_events_records_id,
                        sub_strip_event_id,
                        event_finding_id,
                        value1,
                        value2,
                        created_by
                    )
                    VALUES
                    (
                        @detection_id,
                        @queued_events_records_id,
                        @sub_strip_event_id,
                        @event_finding_id,
                        @value1,
                        @value2,
                        @created_by
                    )";
        }

        internal static string insert_detection()
        {
            return $@"
                    INSERT INTO {Tbl.detection}
                    (
                        sub_strip_event_id,
                        queued_events_records_id,
                        detection_status,
                        created_by
                    )
                    OUTPUT INSERTED.detection_id
                    VALUES
                    (
                        @sub_strip_event_id,
                        @queued_events_records_id,
                        @detection_status,
                        @created_by
                    )";
        }
        #endregion
    }

    public class FindingDTO
    {
        public long event_finding_id { get; set; }
        public string? finding_name { get; set; }
        public int? value1 { get; set; }
        public int? value2 { get; set; }

        #region Query

        internal static string select_complete_qc_report_findings()
        {
            return $@"
        SELECT
            ef.event_finding_id,
            ef.finding_name,
            ssf.value1,
            ssf.value2
        FROM {Tbl.sub_strip_event_finding} ssf
        INNER JOIN {Tbl.event_finding_mast} ef
            ON ef.event_finding_id = ssf.event_finding_id
        WHERE ssf.detection_id = @detection_id
        ORDER BY ssf.sub_strip_event_finding_id";
        }
        #endregion
    }


    public class CompletedFindingListReqDTO
    {
        public string search { get; set; }
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public int offSet => (pageNumber - 1) * pageSize;
        public string shortColumn { get; set; }
        public string orderBy { get; set; }

        #region Query
        internal static string get_completed_findings(CompletedFindingListReqDTO obj)
        {
            return $@"
                SELECT COUNT(1)
                FROM {Tbl.detection} d
                LEFT JOIN {Tbl.queued_events_records} qu
                    ON d.queued_events_records_id = qu.queued_events_records_id
                LEFT JOIN {Tbl.patient_enrollment} p
                    ON qu.patient_enrollment_id = p.patient_enrollment_id
                LEFT JOIN {Tbl.patient} pt
                    ON pt.patient_id = p.patient_id
                LEFT JOIN {Tbl.user_master} u
                    ON u.user_id = d.created_by
                WHERE qu.event_status = 'partial-completed'
                  AND (
                        ISNULL(@search, '') = ''
                        OR pt.patient_name LIKE '%' + @search + '%'
                        OR u.user_name LIKE '%' + @search + '%'
                      );

                SELECT 
                    d.detection_id,
                    d.sub_strip_event_id,
                    d.queued_events_records_id,
                    d.detection_status,
                    d.created_by,
                    d.created_datetime,

                    qu.queued_events_records_id,
                    qu.trigger_type,
                    qu.last_assigned_datetime,
                    qu.event_status,
                    qu.modified_by,

                    u.user_id,
                    u.user_name,

                    pt.patient_id,
                    pt.patient_name

                FROM {Tbl.detection} d
                LEFT JOIN {Tbl.queued_events_records} qu
                    ON d.queued_events_records_id = qu.queued_events_records_id

                LEFT JOIN {Tbl.patient_enrollment} p
                    ON qu.patient_enrollment_id = p.patient_enrollment_id

                LEFT JOIN {Tbl.patient} pt
                    ON pt.patient_id = p.patient_id

                LEFT JOIN {Tbl.user_master} u
                    ON u.user_id = d.created_by


                WHERE qu.event_status = 'partial-completed'
                  AND (
                        ISNULL(@search, '') = ''
                        OR pt.patient_name LIKE '%' + @search + '%'
                        OR u.user_name LIKE '%' + @search + '%'
                      )

                ORDER BY {obj.shortColumn} {obj.orderBy}
                OFFSET @offSet ROWS
                FETCH NEXT @pageSize ROWS ONLY;
                ";
        }
        #endregion
    }


    public class CompletedFindingListDTO
    {
        public long detection_id { get; set; }
        public long sub_strip_event_id { get; set; }
        public long queued_events_records_id { get; set; }

        public string detection_status { get; set; }

        public long created_by { get; set; }
        public DateTime created_datetime { get; set; }

        public string trigger_type { get; set; }
        public DateTime last_assigned_datetime { get; set; }
        public string event_status { get; set; }

        public long user_id { get; set; }
        public string user_name { get; set; }

        public long patient_id { get; set; }
        public string patient_name { get; set; }
    }

    public class CompletedFindingListResDTO
    {
        public long queued_events_records_id { get; set; }
        public long patient_enrollment_id { get; set; }
        public long enrollment_raw_data_id { get; set; }
        public string priority_level { get; set; }
        public string trigger_type { get; set; }
        public int bpm { get; set; }
        public string event_status { get; set; }
        public string event_file_path { get; set; }
        public DateTime? created_datetime { get; set; }
        public DateTime? last_assigned_datetime { get; set; }


        public long patient_id { get; set; }
        public string patient_name { get; set; }

        public long device_id { get; set; }
        public string device_no { get; set; }
        public string device_name { get; set; }

        public long user_id { get; set; }
        public string user_name { get; set; }

        public long detection_id { get; set; }
        public long? sub_strip_event_id { get; set; }
        public string sub_strip_file_path { get; set; }
        public string detection_status { get; set; }


        public List<FindingDTO> findings { get; set; }

        public long? modified_by { get; set; }
        public DateTime? modified_datetime { get; set; }

        #region Query

        internal static string is_active_old_sub_strip()
        {
            return $@"UPDATE {Tbl.sub_strip_event}
                  SET is_active = 0,
                      modified_by = @modified_by,
                      modified_date = GETDATE()
                  WHERE sub_strip_event_id = @sub_strip_event_id";
        }
        internal static string insert_new_strip()
        {
            return $@"insert into{Tbl.sub_strip_event}(detection_id,created_by)
                    values(@detection_id,@created_by)";
        }
        internal static string insert_submit_qc()
        {
            return $@" INSERT INTO {Tbl.sub_strip_event_finding}
                            (
                                detection_id,
                                sub_strip_event_id,
                                event_finding_id,
                                value1,
                                value2,
                                modified_by,
                                modified_datetime
                            )
                            VALUES
                            (
                                @detection_id,
                                @sub_strip_event_id,
                                @event_finding_id,
                                @value1,
                                @value2,
                                @modified_by,
                                GETDATE()
                            );";
        }
        internal static string update_submit_status()
        {
            return $@"UPDATE {Tbl.detection}
                SET
                    detection_status = 'submit',
                    modified_by = @modified_by,
                    modified_datetime = GETDATE()
                WHERE detection_id = @detection_id";
        }
        internal static string update_event_completed()
        {
            return @$"
                    UPDATE {Tbl.queued_events_records}
                    SET event_status = 'completed',
                        modified_by = @modified_by,
                    last_assigned_datetime = GETDATE()
                    WHERE queued_events_records_id = @queued_events_records_id ";
        }
        internal static string update_status_completed()
        {
            return $@"
                    UPDATE {Tbl.event_assign}
                    SET
                        work_status = 'completed',
                        modified_by = @modified_by,
                        modified_datetime = GETDATE(),
                        analysis_end_datetime = getdate()
                    WHERE queued_events_records_id = @queued_events_records_id";
        }
        internal static string select_complete_qc_report()
        {
            return $@"
                    SELECT
                    d.detection_id,
                    d.sub_strip_event_id,
                    d.queued_events_records_id,
                    d.detection_status,
                    d.created_datetime,

                    qr.patient_enrollment_id,
                    qr.enrollment_raw_data_id,
                    qr.priority_level,
                    qr.trigger_type,
                    qr.bpm,
                    qr.event_status,
                    qr.event_file_path,
                    qr.last_assigned_datetime,

                    p.patient_id,
                    p.patient_name,

                    dc.device_id,
                    dc.device_no,
                    dc.device_name,

                    u.user_id,
                    u.user_name,

                    sse.sub_strip_file_path,

                    MAX(ssf.value1) AS value1,
                    MAX(ssf.value2) AS value2,

                    STRING_AGG(
                        CAST(ef.event_finding_id AS VARCHAR(20)),
                        ', '
                    ) AS event_finding_ids,

                    STRING_AGG(
                        ef.finding_name,
                        ', '
                    ) AS finding_name

                    FROM {Tbl.detection} d

                    INNER JOIN {Tbl.queued_events_records} qr
                        ON qr.queued_events_records_id = d.queued_events_records_id

                    LEFT JOIN {Tbl.patient_enrollment} pe
                        ON pe.patient_enrollment_id = qr.patient_enrollment_id

                    LEFT JOIN {Tbl.patient} p
                        ON p.patient_id = pe.patient_id

                    LEFT JOIN {Tbl.device_mast} dc
                        ON dc.device_id = pe.device_id

                    LEFT JOIN {Tbl.user_master} u
                        ON u.user_id = d.created_by

                    LEFT JOIN {Tbl.sub_strip_event_finding} ssf
                        ON ssf.detection_id = d.detection_id

                    LEFT JOIN {Tbl.event_finding_mast} ef
                        ON ef.event_finding_id = ssf.event_finding_id

                    LEFT JOIN {Tbl.sub_strip_event} sse
                        ON sse.sub_strip_event_id = d.sub_strip_event_id

                    WHERE d.detection_id = @detection_id

                   GROUP BY
                        d.detection_id,
                        d.sub_strip_event_id,
                        d.queued_events_records_id,
                        d.detection_status,
                        d.created_datetime,
                        qr.patient_enrollment_id,
                        qr.enrollment_raw_data_id,
                        qr.priority_level,
                        qr.trigger_type,
                        qr.bpm,
                        qr.event_status,
                        qr.event_file_path,
                        qr.last_assigned_datetime,
                        p.patient_id,
                        p.patient_name,
                        dc.device_id,
                        dc.device_no,
                        dc.device_name,
                        u.user_id,
                        u.user_name,
                        sse.sub_strip_file_path";
        }
        #endregion
    }
}
