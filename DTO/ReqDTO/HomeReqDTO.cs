using elr_API.Common;

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

    public class QueuedEventReqDTO
    {
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public int offset => (pageNumber - 1) * pageSize;
        public string shortColumn { get; set; }
        public string orderBy { get; set; }

        #region Query
        internal static string queue_event_list(QueuedEventReqDTO obj)
        {
            return $@"select count(1) from {Tbl.queued_events_records}

                        select * from {Tbl.queued_events_records}
                        where is_active = 1
                        order by {obj.shortColumn} {obj.orderBy}
                        offset @offset rows fetch next @pageSize rows only";
        }

        #endregion
    }

    public class QueuedEventDTO
    {
        public long queued_events_records_id { get; set; }
        public long patient_enrollment_id { get; set; }
        public long enrollment_raw_data_id { get; set; }
        public string priority_level { get; set; }
        public string trigger_type { get; set; }
        public int? hr_min { get; set; }
        public int? hr_max { get; set; }
        public int? hr_avg { get; set; }
        public int? event_duration_seconds { get; set; }
        public string event_status { get; set; }
        public DateTime? event_start_datetime { get; set; }
        public DateTime? event_end_datetime { get; set; }
        public IFormFile? file_name { get; set; }
        public string event_file_path { get; set; }

        public long created_by { get; set; }
        public DateTime? created_datetime { get; set; }
        public long modified_by { get; set; }
        public DateTime? modified_datetime { get; set; }
        public bool is_active { get; set; }

        #region Query
        internal static string insert_queued_event()
        {
            return $@"INSERT INTO {Tbl.queued_events_records}
                        (patient_enrollment_id,enrollment_raw_data_id, priority_level,trigger_type,hr_min, hr_max,hr_avg, event_duration_seconds,event_status,event_start_datetime,event_end_datetime,event_file_path,created_by)
                     values (@patient_enrollment_id, @enrollment_raw_data_id, @priority_level, @trigger_type, @hr_min, @hr_max, @hr_avg, @event_duration_seconds, @event_status, @event_start_datetime,@event_end_datetime, @event_file_path, @created_by)";
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
            return $@"insert into {Tbl.patient_base_line} (enrollment_raw_data_id,patient_enrollment_id,record_st,record_end,duration_seconds,event_parent_path,event_processed_path)
                       values(@enrollment_raw_data_id, @patient_enrollment_id, @record_st, @record_end, @duration_seconds, @event_parent_path, @event_processed_path)";
        }
        #endregion
    }
}
