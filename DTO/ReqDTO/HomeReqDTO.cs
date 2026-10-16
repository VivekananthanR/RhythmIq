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
            return $@"
                    select count(1) as totalCount from {Tbl.country} where is_active = 1;
                  SELECT
                    country_id,
                    country_code,
                    country_name,
                    is_active
                  FROM tbl_country
                  WHERE is_active = 1";
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
            return $@"
                   select count(1) as totalCount from {Tbl.state} where is_active = 1;
                   SELECT
                    s.state_id,
                    s.state_code,
                    s.country_id,
                    s.state_name,
                    s.is_active
                  FROM tbl_state s
                  WHERE s.is_active = 1 ";
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
            return $@"
                    select count(1) as totalCount from {Tbl.district} where is_active = 1;
            
                    SELECT
                    d.district_id,
                    d.district_code,
                    d.state_id,
                    d.district_name,
                    d.is_active
                  FROM tbl_district d
                 where d.is_active = 1 ";
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
                  WHERE tp.is_active = 1";
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

    public class DeviceReqDTO
    {
        public long device_id { get; set; }
        public string device_no { get; set; }
        public string device_name { get; set; }
        public long current_location_id { get; set; }
        public long current_emp_id { get; set; }
        public string device_status { get; set; }

    }

    public class EmployeeDeviceListDTO
    {
        public long device_id { get; set; }
        public string device_no { get; set; }
        public string device_status { get; set; }
        public long current_emp_id { get; set; }
        public long patient_enrollment_id { get; set; }

        #region Query
        internal static string select_device()
        {
            return $@"
                    SELECT COUNT(*)
                    FROM {Tbl.device_mast} dm
                    WHERE dm.current_emp_id = @current_emp_id;

                    SELECT 
                        dm.device_id,
                        dm.device_no,
                        dm.device_status,
                        dm.current_emp_id,
                        da.patient_enrollment_id
                    FROM {Tbl.device_mast} dm
                    LEFT JOIN {Tbl.patient_enrollment} da
                        ON da.device_id = dm.device_id
                        AND da.enrollment_status IN ('hookuped', 'monitoring')
                    WHERE dm.current_emp_id = @current_emp_id;
                ";
        }
        #endregion
    }



    public class DeviceResDTO
    {
        public long device_id { get; set; }
        public string device_no { get; set; }
        public string device_name { get; set; }
        public string manufacturer_name { get; set; }
        public string model_no { get; set; }
        public string serial_no { get; set; }
        public long current_location_id { get; set; }
        public long current_emp_id { get; set; }
        public string device_status { get; set; }
        public long created_by { get; set; }
        public DateTime? created_datetime { get; set; }
        public long modified_by { get; set; }
        public DateTime? modified_datetime { get; set; }
        public bool is_active { get; set; }


        public long? from_location_id { get; set; }
        public long? to_location_id { get; set; }
        public long? from_emp_id { get; set; }
        public long? to_emp_id { get; set; }
        public string transfer_status { get; set; }


        #region Query

        internal static string insert_device()
        {
            return $@"insert into {Tbl.device_mast} (device_no,device_name,manufacturer_name,model_no,serial_no,current_emp_id,current_location_id,device_status,created_by)
                        values (@device_no, @device_name, @manufacturer_name, @model_no, @serial_no, @current_emp_id, @current_location_id, @device_status, @created_by  )";
        }
        internal static string update_device()
        {
            return $@"Update {Tbl.device_mast}
                        set device_no = @device_no ,
                        device_name = @device_name,
                        manufacturer_name = @manufacturer_name,
                        model_no = @model_no,                   
                        serial_no = @serial_no,
                        current_emp_id =@current_emp_id,
                        current_location_id= @current_location_id,
                        device_status = @device_status,
                        modified_by = @modified_by,             
                        modified_datetime = getdate(),
                        is_active = @is_active
                        where device_id = @device_id";
        }

        internal static string select_device_list()
        {
            return $@"SELECT
                    d.device_id,
                    d.device_no,
                    d.device_name,
                    d.manufacturer_name,
                    d.model_no,
                    d.serial_no,
                    d.current_location_id,
                    d.current_emp_id,
                    d.device_status,
                    d.created_by,
                    d.created_datetime,
                    d.modified_by,
                    d.modified_datetime,
                    d.is_active
                FROM {Tbl.device_mast} AS d
                WHERE d.is_active = 1 and (d.device_id = @device_id or @device_id is null or @device_id = 0) 
                                      and ( d.device_status = @device_status or @device_status is null or @device_status = '' )
                                      and (d.device_name = @device_name or @device_name is null or @device_name = '')
                                      AND (@device_no IS NULL OR @device_no = '' OR d.device_no = @device_no)";
        }


        internal static string update_device_status()
        {
            return $@"Update {Tbl.device_mast}
                        set device_status= @device_status
                        WHERE device_id = @device_id  and device_status = 'active';";

        }
        internal static string get_device()
        {
            return $@"select device_id,device_no,device_status,current_emp_id from {Tbl.device_mast} where device_no =@device_no and device_status = 'active' and is_active= 1";
        }


        internal static string check_valid_device()
        {
            return $@"
                    SELECT
                        device_id,
                        device_no,
                        device_name,
                        device_status
                    FROM {Tbl.device_mast}
                    WHERE device_no = @device_no
                    AND is_active = 1 ";
        }

        internal static string update_device_status_active()
        {
            return $@"
                    UPDATE {Tbl.device_mast}
                    SET device_status = 'active'
                    WHERE device_id = @device_id and is_active = 1";
        }

        internal static string get_device_by_enrollment()
        {
            return $@"
                    SELECT TOP 1
                        da.device_id,
                        dm.current_emp_id,
                        dm.current_location_id,
                        dm.device_status
                    FROM {Tbl.device_assignment} da
                    INNER JOIN {Tbl.device_mast} dm
                        ON dm.device_id = da.device_id
                    WHERE da.patient_enrollment_id = @patient_enrollment_id
                      AND da.assignment_status in('assigned' , 'processing')
                      AND da.is_active = 1
                      AND dm.is_active = 1
                    ORDER BY da.device_assignment_id DESC";
        }
        internal static string get_assigned_device_by_enrollment()
        {
            return $@"
                SELECT TOP 1
                    da.device_id,
                    da.patient_enrollment_id,
                    da.emp_id,
                    dm.current_emp_id,
                    dm.current_location_id,
                    dm.device_status
                FROM {Tbl.device_assignment} da
                INNER JOIN {Tbl.device_mast} dm
                    ON dm.device_id = da.device_id
                WHERE da.patient_enrollment_id = @patient_enrollment_id
                  AND da.assignment_status = 'assigned'
                  AND da.is_active = 1
                  AND dm.is_active = 1
                ORDER BY da.device_assignment_id DESC";
        }
        #endregion
    }
    public class DeviceAssignmentReqDTO
    {
        public long device_assignment_id { get; set; }
        public long emp_id { get; set; }
        public long patient_enrollment_id { get; set; }
        public long created_by { get; set; }
        public string assignment_status { get; set; }
    }
    public class DeviceAssignmentResDTO
    {
        public long device_assignment_id { get; set; }
        public long device_id { get; set; }
        public long emp_id { get; set; }
        public long patient_enrollment_id { get; set; }
        public string assignment_status { get; set; }
        public string device_no { get; set; }
        public string remarks { get; set; }
        public long created_by { get; set; }
        public DateTime? created_datetime { get; set; }
        public long modified_by { get; set; }
        public DateTime? modified_datetime { get; set; }
        public bool is_active { get; set; }

        #region Query


        internal static string check_device_assignment()
        {
            return $@"
                    SELECT TOP 1 *
                    FROM {Tbl.device_assignment}
                    WHERE device_id = @device_id
                    AND assignment_status = 'assigned'
                    AND is_active = 1;";
        }
        public static string update_device_assign_pateint_enroll_id()
        {
            return $@"
                    UPDATE {Tbl.device_assignment}
                    SET patient_enrollment_id = @patient_enrollment_id,
                        assignment_status = 'processing',
                        modified_by = @modified_by,
                        modified_datetime = GETDATE()
                    WHERE device_id = @device_id
                      AND assignment_status in ( 'assigned','completed')
                      AND is_active = 1";
        }
        internal static string insert_device_assignment()
        {
            return $@"
                    INSERT INTO {Tbl.device_assignment}
                    (
                        emp_id,
                        device_id,
                        assignment_status,
                        created_by
                    )
                    VALUES
                    (
                        @emp_id,
                        @device_id,
                        'assigned',
                        @created_by
                    );";
        }

        internal static string update_device_assignment()
        {
            return $@"update {Tbl.device_assignment}
                        set emp_id = @emp_id,
                              device_id = @device_id,
                              modified_by = @modified_by,
                              modified_datetime = GETDATE(),
                              assignment_status = @assignment_status
                                remarks = @remarks,
                                is_active = @is_active
                                where device_assignment_id = @device_assignment_id and assignment_status = 'assignment_status'";
        }
        internal static string update_device_assign_status()
        {
            return $@" update {Tbl.device_assignment}
                        set assignment_status = 'processing'
                        where patient_enrollment_id = @patient_enrollment_id and assignment_status = 'assigned'";
        }
        internal static string insert_device_assignment_history()
        {
            return $@"
                    INSERT INTO {Tbl.device_assignment}
                    (
                        device_id,
                        emp_id,
                        patient_enrollment_id,
                        assignment_status,
                        remarks,
                        created_by,
                        created_datetime,
                        is_active
                    )
                    VALUES
                    (
                        @device_id,
                        @emp_id,
                    @patient_enrollment_id,
                    @assignment_status,
                    @remarks,
                    @created_by,
                    GETDATE(),
                    1
                )";
        }
        #endregion
    }
    public class PatchReqDTO
    {
        public long patch_id { get; set; }
        public string patch_no { get; set; }
        public long current_location_id { get; set; }
        public long current_emp_id { get; set; }
    }
    public class PatchResDTO
    {
        public long patch_id { get; set; }
        public string patch_no { get; set; }
        public long current_location_id { get; set; }
        public long current_emp_id { get; set; }
        public string patch_status { get; set; }
        public long created_by { get; set; }
        public DateTime? created_datetime { get; set; }
        public long modified_by { get; set; }
        public DateTime? modified_datetime { get; set; }
        public bool is_active { get; set; }


        #region Query
        internal static string insert_patch()
        {
            return $@"insert into {Tbl.patch_mast} (patch_no,patch_status,current_emp_id,current_location_id,created_by)
                        values (@patch_no,@patch_status,@current_emp_id,@current_location_id,@created_by)";
        }


        internal static string update_patch()
        {
            return $@"update {Tbl.patch_mast}
                        set patch_no = @patch_no,
                            patch_status = @patch_status,
                            current_emp_id =@current_emp_id,
                            current_location_id = @current_location_id,
                            modified_by = @modified_by,
                            modified_datetime = getdate(),
                            is_active = @is_active
                            where patch_id = @patch_id";
        }
        internal static string check_valid_patch()
        {
            return $@"select * from {Tbl.patch_mast} where patch_no =@patch_no and  is_active = 1";
        }


        internal static string update_patch_status()
        {
            return $@"update {Tbl.patch_mast}
                        set patch_status = @patch_status
                            where patch_id = @patch_id and patch_status = 'active'";
        }
        internal static string get_patch_by_enrollment()
        {
            return $@"
                    SELECT TOP 1 patch_id
                    FROM {Tbl.patch_assignment}
                    WHERE patient_enrollment_id = @patient_enrollment_id
                      AND assignment_status = 'assigned'
                      AND is_active = 1";
        }
        internal static string update_patch_status_active()
        {
            return $@"
                    UPDATE {Tbl.patch_mast}
                    SET patch_status = 'active'
                    WHERE patch_id = @patch_id";
        }
        internal static string get_patch_assigned()
        {
            return $@"select patch_id,patch_no,patch_status from {Tbl.patch_mast} where patch_no =@patch_no and patch_status = 'active' and is_active= 1";
        }
        #endregion
    }
    public class PatchAssignmentReqDTO
    {
        public string patch_no { get; set; }
        public long emp_id { get; set; }
        public long patient_enrollment_id { get; set; }
    }
    public class PatchAssignmentResDTO
    {
        public long patch_assignment_id { get; set; }
        public long patch_id { get; set; }
        public long emp_id { get; set; }
        public long patient_enrollment_id { get; set; }
        public string assignment_status { get; set; }
        public string remarks { get; set; }
        public long created_by { get; set; }
        public DateTime? created_datetime { get; set; }
        public long modified_by { get; set; }
        public DateTime? modified_datetime { get; set; }
        public bool is_active { get; set; }

        #region Query
        internal static string check_patch_assignment()
        {
            return $@"select top 1 *
                        from {Tbl.patch_assignment}
                            where patch_id = @patch_id and assignment_status = 'assigned' and is_active = 1";
        }
        internal static string insert_patch_assignment()
        {
            return $@"insert into {Tbl.patch_assignment} (patch_id,emp_id,patient_enrollment_id,assignment_status,created_by)
                        values (@patch_id,@emp_id,@patient_enrollment_id,@assignment_status,@created_by)";
        }
        internal static string complete_device_assignment()
        {
            return $@"
                UPDATE {Tbl.patch_assignment}
                SET assignment_status = 'completed',
                    modified_by = @modified_by,
                    modified_datetime = GETDATE()
                WHERE patient_enrollment_id = @patient_enrollment_id
                  AND assignment_status = 'assigned'
                  AND is_active = 1";
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
            return $@"
        SELECT COUNT(1)
        FROM {Tbl.queued_events_records} a

        LEFT JOIN {Tbl.patient_enrollment} pe
            ON a.patient_enrollment_id = pe.patient_enrollment_id

        LEFT JOIN {Tbl.patient} p
            ON pe.patient_id = p.patient_id

        LEFT JOIN {Tbl.user_master} u
            ON pe.actual_technician_id = u.user_id

        LEFT JOIN {Tbl.device_mast} d
            ON pe.device_id = d.device_id

        WHERE a.is_active = 1
          AND pe.is_active = 1
          AND p.is_active = 1
          AND u.is_active = 1
          AND d.is_active = 1
          AND a.event_status NOT IN ('partial-completed', 'completed');


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

            p.patient_id,
            p.patient_name,
            p.age,
            p.gender,
            p.phone_no,
            p.email_id,

            pe.actual_technician_id,
            u.user_name AS technician_name,

            CASE
                WHEN pe.is_patient_hookuped = 1 THEN 'Patient'
                ELSE 'Employee'
            END AS hookup_done_by,

            CASE
                WHEN pe.is_patient_hookuped = 1 THEN p.patient_name
                ELSE u.user_name
            END AS hookup_done_by_name,

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

        WHERE a.is_active = 1
          AND pe.is_active = 1
          AND p.is_active = 1
          AND u.is_active = 1
          AND d.is_active = 1
          AND a.event_status NOT IN ('partial-completed', 'completed')

        ORDER BY {obj.shortColumn} {obj.orderBy}
        OFFSET @offset ROWS
        FETCH NEXT @pageSize ROWS ONLY;";
        }
        //internal static string queue_anomaly_list(QueuedAnomalyReqDTO obj)
        //{
        //    return $@"select count(1) from {Tbl.queued_events_records} as a where a.is_active = 1 and a.event_status <> 'partial-completed' and a.event_status <> 'completed';

        //                SELECT
        //                    a.queued_events_records_id,
        //                    a.patient_enrollment_id,
        //                    a.enrollment_raw_data_id,
        //                    a.priority_level,
        //                    a.trigger_type,
        //                    a.bpm,
        //                    a.event_status,
        //                    a.event_start_datetime,
        //                    a.event_end_datetime,
        //                    a.event_file_path,
        //                    a.created_datetime,

        //                    -- Patient Details
        //                    p.patient_id,
        //                    p.patient_name,
        //                    p.age,
        //                    p.gender,
        //                    p.phone_no,
        //                    p.email_id,

        //                    -- Technician Details
        //                    pe.actual_technician_id,
        //                    u.user_name AS technician_name,

        //                    CASE
        //                        WHEN pe.is_patient_hookuped = 1 THEN 'Patient'
        //                        ELSE 'Employee'
        //                    END AS hookup_done_by,

        //                    CASE
        //                        WHEN pe.is_patient_hookuped = 1 THEN p.patient_name
        //                        ELSE u.user_name
        //                    END AS hookup_done_by_name,

        //                    -- Device Details
        //                    d.device_id,
        //                    d.device_no,      
        //                    d.device_name   


        //                    FROM {Tbl.queued_events_records} a

        //                    LEFT JOIN {Tbl.patient_enrollment} pe
        //                        ON a.patient_enrollment_id = pe.patient_enrollment_id

        //                    LEFT JOIN {Tbl.patient} p
        //                        ON pe.patient_id = p.patient_id

        //                    LEFT JOIN {Tbl.user_master} u
        //                        ON pe.actual_technician_id = u.user_id

        //                    LEFT JOIN {Tbl.device_mast} d
        //                        ON pe.device_id = d.device_id

        //                        where a.is_active = 1 and pe.is_active = 1 and p.is_active = 1 and u.is_active = 1 and d.is_active = 1
        //                                and a.event_status <> 'partial-completed' and a.event_status <> 'completed'

        //                        order by {obj.shortColumn} {obj.orderBy}
        //                        offset @offset rows fetch next @pageSize rows only";
        //}

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
        public string hookup_done_by { get; set; }
        public string hookup_done_by_name { get; set; }

        public long device_id { get; set; }
        public string device_no { get; set; }
        public string device_name { get; set; }

        public long created_by { get; set; }
        public DateTime? created_datetime { get; set; }
        public long modified_by { get; set; }
        public DateTime? modified_datetime { get; set; }
        public bool is_active { get; set; }

        #region Query

        public static string update_event_status_by_queue()
        {
            return $@"
                    UPDATE {Tbl.queued_events_records}
                    SET
                        event_status = @event_status,
                        completed_by =
                            CASE
                                WHEN @event_status='completed'
                                THEN @completed_by
                                ELSE completed_by
                            END,
                        completed_datetime =
                            CASE
                                WHEN @event_status='completed'
                                THEN GETDATE()
                                ELSE completed_datetime
                            END
                    WHERE queued_events_records_id=@queued_events_records_id";
        }
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
                        modified_by = @modified_by,
                        modified_datetime = GETDATE(),
                        remarks = 'No activity for 3 minutes'
                    WHERE queued_events_records_id = @queued_events_records_id
                    AND event_status = 'processing'";
        }
        internal static string update_event_processing()
        {
            return @$"
                    UPDATE {Tbl.queued_events_records}
                    SET event_status = 'processing',
                        modified_by = @modified_by,
                        modified_datetime = GETDATE()
                    WHERE queued_events_records_id = @queued_events_records_id ";
        }


        internal static string get_modified_datetime_query()
        {
            return $@"select modified_datetime from {Tbl.queued_events_records} where queued_events_records_id =@queued_events_records_id and is_active = 1";
        }

        internal static string update_event_status()
        {
            return @$"
                    UPDATE {Tbl.queued_events_records}
                    SET event_status = @event_status,
                        completed_by = @completed_by,
                        completed_datetime = GETDATE()
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
        public long modified_by { get; set; }
        public DateTime? created_datetime { get; set; }
        public DateTime? modified_datetime { get; set; }
        public List<FetchHistoryDTO>? fetch_history { get; set; }


        public List<SymptomDTO> symptoms { get; set; }

        public long patient_id { get; set; }
        public string patient_name { get; set; }

        public long device_id { get; set; }
        public string device_no { get; set; }
        public string device_name { get; set; }

        public long event_assign_id { get; set; }

        public bool active { get; set; }

        #region Query


        internal static string get_next_event()
        {
            return $@"
                    UPDATE q
                    SET
                        q.event_status = 'processing',
                        q.modified_by = @modified_by,
                        q.modified_datetime = GETDATE()
                    OUTPUT INSERTED.queued_events_records_id
                    FROM {Tbl.queued_events_records} q
                    WHERE q.queued_events_records_id =
                    (
                        SELECT TOP 1 q1.queued_events_records_id
                        FROM {Tbl.queued_events_records} q1 WITH (UPDLOCK, READPAST, ROWLOCK)
                        INNER JOIN {Tbl.patient_enrollment} pe
                            ON pe.patient_enrollment_id = q1.patient_enrollment_id
                        WHERE q1.event_status = 'pending'
                          AND q1.is_active = 1
                          AND pe.is_active = 1
                          AND NOT EXISTS
                          (
                              SELECT 1
                              FROM {Tbl.event_assign} ea
                              WHERE ea.queued_events_records_id = q1.queued_events_records_id
                                AND ea.work_status = 'inprogress'
                          )
                        ORDER BY
                            CASE q1.priority_level
                                WHEN 'high' THEN 1
                                WHEN 'medium' THEN 2
                                WHEN 'low' THEN 3
                                ELSE 4
                            END,
                            CASE q1.trigger_type
                                WHEN 'AF' THEN 1
                                WHEN 'VT' THEN 2
                                WHEN 'Pause' THEN 3
                                WHEN 'Brady' THEN 4
                                WHEN 'Tachy' THEN 5
                                ELSE 6
                            END,
                            q1.created_datetime ASC
                    );";
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
                        q.modified_by,
                        q.created_datetime,
                        q.modified_datetime,
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
                    WHERE q.queued_events_records_id = @queued_events_records_id AND q.is_active = 1 AND pe.is_active = 1 AND p.is_active = 1 AND d.is_active = 1;";
        }



        #endregion

    }


    public class SymptomDTO
    {
        public long symptoms_id { get; set; }
        public long patient_enrollment_id { get; set; }
        public string symptom_notes { get; set; }
        public string custom_symptom_text { get; set; }
        public DateTime? symptom_start_datetime { get; set; }
        public DateTime? symptom_end_datetime { get; set; }

        public long symptoms_type_id { get; set; }
        public string symptoms_type { get; set; }


        public long queued_events_records_id { get; set; }

        #region Query 
        internal static string get_event_symptoms()
        {
            return $@" SELECT
                            s.symptoms_id,
                            s.patient_enrollment_id,
                            s.symptom_notes,
                            s.custom_symptom_text,
                            s.symptom_start_datetime,
                            s.symptom_end_datetime,
                            s.symptoms_type_id,
                            st.symptoms_type
                        FROM {Tbl.queued_events_records} q
                        INNER JOIN {Tbl.symptoms} s
                            ON s.patient_enrollment_id = q.patient_enrollment_id
                        LEFT JOIN {Tbl.symptoms_type} st
                            ON st.symptoms_type_id = s.symptoms_type_id
                        WHERE q.queued_events_records_id = @queued_events_records_id
                          AND s.is_active = 1";
        }
        //internal static string get_event_symptoms()
        //{
        //    return $@" SELECT
        //                    s.symptoms_id,
        //                    s.patient_enrollment_id,
        //                    s.symptom_notes,
        //                    s.custom_symptom_text,
        //                    s.symptom_start_datetime,
        //                    s.symptom_end_datetime,
        //                    s.symptoms_type_id,
        //                    st.symptoms_type
        //                FROM {Tbl.queued_events_records} q
        //                INNER JOIN {Tbl.symptoms} s
        //                    ON s.patient_enrollment_id = q.patient_enrollment_id
        //                LEFT JOIN {Tbl.symptoms_type} st
        //                    ON st.symptoms_type_id = s.symptoms_type_id
        //                WHERE q.queued_events_records_id = @queued_events_records_id
        //                  AND s.is_active = 1
        //                  AND (
        //                        q.event_start_datetime BETWEEN s.symptom_start_datetime AND s.symptom_end_datetime
        //                     OR q.event_end_datetime BETWEEN s.symptom_start_datetime AND s.symptom_end_datetime
        //                     OR (
        //                            q.event_start_datetime <= s.symptom_start_datetime
        //                        AND q.event_end_datetime >= s.symptom_end_datetime
        //                     )
        //                  )
        //                ORDER BY s.symptom_start_datetime asc ;";
        //}
        internal static string check_patient_enrollment_id()
        {
            return $@"SELECT patient_enrollment_id FROM {Tbl.symptoms} WHERE patient_enrollment_id = @patient_enrollment_id";
        }
        #endregion

    }
    public class SymtomTypeDTO
    {
        public long symptoms_type_id { get; set; }
        public string symptoms_type { get; set; }
        public long created_by { get; set; }
        public DateTime? created_datetime { get; set; }
        public long modified_by { get; set; }
        public DateTime? modified_datetime { get; set; }
        public bool is_active { get; set; }
        #region Query
        internal static string insert_symptoms_type()
        {
            return $@"insert into {Tbl.symptoms_type} (symptoms_type,created_by)
                        values (@symptoms_type,@created_by)";
        }
        internal static string update_symptoms_type()
        {
            return $@"update {Tbl.symptoms_type} set symptoms_type = @symptoms_type, modified_by = @modified_by, modified_datetime = GETDATE() ,is_active = @is_active where symptoms_type_id = @symptoms_type_id";
        }
        #endregion
    }

    public class FetchHistoryDTO
    {
        public long queued_events_records_id { get; set; }
        public string event_source { get; set; }
        public int? fetch_duration_minutes { get; set; }
        public long? parent_queued_events_records_id { get; set; }

        public List<FetchHistoryDTO> fetch_list { get; set; }

        #region Query 

        internal static string get_fetch_history()
        {
            return $@"
                    SELECT
                        queued_events_records_id,
                        event_source,
                        fetch_duration_minutes,
                        parent_queued_events_records_id
                    FROM {Tbl.queued_events_records}
                    WHERE parent_queued_events_records_id = @queued_events_records_id
                      AND is_active = 1";
        }
        internal static string get_event()
        {
            return $@"
                    SELECT
                        queued_events_records_id,
                        parent_queued_events_records_id,
                        event_source,
                        fetch_duration_minutes
                    FROM {Tbl.queued_events_records}
                    WHERE queued_events_records_id = @queued_events_records_id
                      AND is_active = 1";
        }


        #endregion
    }
    public class EventAssignDTO
    {
        public long event_assign_id { get; set; }
        public long patient_enrollment_id { get; set; }
        public long queued_events_records_id { get; set; }
        public long assigned_by { get; set; }
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
        public bool is_final_completed { get; set; }


        #region Query

        internal static string select_is_final_assignId()
        {
            return $@"
                    SELECT TOP 1 event_assign_id
                    FROM {Tbl.event_assign}
                    WHERE assigned_by = @assigned_by
                      AND work_status IN('completed', 'partial-completed')
                      AND is_final_completed = 0
                    ORDER BY event_assign_id  DESC;";
        }

        internal static string update_is_final_completed()
        {
            return $@"
                    UPDATE {Tbl.event_assign}
                    SET
                        is_final_completed = 1,
                        modified_by = @assigned_by,
                        modified_datetime = GETDATE()
                    WHERE event_assign_id  = @event_assign_id 
                      AND work_status IN ('completed', 'partial-completed')";
        }
        //internal static string update_work_status_by_event_assign()
        //{
        //    return $@"
        //            UPDATE {Tbl.event_assign}
        //            SET
        //                work_status = @work_status,
        //                is_final_completed =
        //                    CASE
        //                        WHEN @work_status IN ('completed','partial-completed')
        //                        THEN 0
        //                        ELSE is_final_completed
        //                    END,
        //                analysis_end_datetime =
        //                    CASE
        //                        WHEN @work_status IN ('completed','partial-completed')
        //                        THEN GETDATE()
        //                        ELSE analysis_end_datetime
        //                    END,
        //                modified_by = @modified_by,
        //                modified_datetime = GETDATE()
        //            WHERE event_assign_id = @event_assign_id";
        //}
        internal static string update_work_status_by_event_assign()
        {
            return $@"
                    UPDATE {Tbl.event_assign}
                    SET
                        work_status = @work_status,

                        is_final_completed =
                            CASE
                                WHEN @is_final_completed = 1 THEN 1
                                ELSE is_final_completed
                            END,

                        analysis_end_datetime =
                            CASE
                                WHEN @work_status IN ('completed','partial-completed')
                                THEN GETDATE()
                                ELSE analysis_end_datetime
                            END,

                        modified_by = @modified_by,
                        modified_datetime = GETDATE()

                    WHERE event_assign_id = @event_assign_id;";
        }
        internal static string event_assign_insert()
        {
            return $@" insert into {Tbl.event_assign}(patient_enrollment_id,queued_events_records_id,assigned_by,analysis_start_datetime,work_status,created_by)
                        values(@patient_enrollment_id, @queued_events_records_id, @assigned_by,@analysis_start_datetime, @work_status, @created_by)";
        }

        #region Background service
        //internal static string reassign_expired_events()
        //{
        //    return $@"
        //            UPDATE {Tbl.event_assign}
        //            SET
        //                work_status = 'reassigned',
        //                modified_by = assigned_by,
        //                last_activity_datetime = GETDATE(),
        //                remarks = '10 Minutes Reached'
        //            OUTPUT INSERTED.event_assign_id
        //           WHERE
        //                (
        //                    work_status = 'inprogress'
        //                    AND DATEDIFF(MINUTE, analysis_start_datetime, GETDATE()) >= 10
        //                )
        //                OR
        //                (
        //                    work_status IN ('completed','partial-completed')
        //                    AND is_final_completed = 0
        //                    AND DATEDIFF(MINUTE, analysis_start_datetime, GETDATE()) >= 10
        //                )";
        //}
        internal static string reassign_expired_events()
        {
            return $@"
                    UPDATE {Tbl.event_assign}
                    SET
                        work_status = 'reassigned',
                        modified_by = assigned_by,
                        last_activity_datetime = GETDATE(),
                        remarks = '10 Minutes Reached'
                    OUTPUT INSERTED.event_assign_id
                    WHERE
                        is_active = 1
                        AND ISNULL(is_final_completed, 0) = 0
                        AND work_status IN ('inprogress', 'completed', 'partial-completed')
                        AND DATEDIFF(MINUTE, analysis_start_datetime, GETDATE()) >= 10;";
        }

        internal static string insert_reassigned_log()
        {
            return $@"
                    INSERT INTO {Tbl.event_assign_log}
                    (
                        event_assign_id,
                        patient_enrollment_id,
                        queued_events_records_id,
                        table_name,
                        record_id,
                        action_type,
                        old_work_status,
                        new_work_status,
                        old_value,
                        new_value,
                        remarks,
                        created_by,
                        created_datetime
                    )
                    SELECT
                        event_assign_id,
                        patient_enrollment_id,
                        queued_events_records_id,
                        '{Tbl.event_assign}',
                        event_assign_id,
                        'Reassigned10Min',
                        'inprogress',
                        'reassigned',
                        '{{""work_status"":""inprogress""}}',
                        '{{""work_status"":""reassigned""}}',
                        'No activity for 10 minutes. Event automatically reassigned.',
                        assigned_by,
                        GETDATE()
                    FROM {Tbl.event_assign}
                    WHERE event_assign_id = @event_assign_id;";
        }

        internal static string inactive_reassigned_event_data()
        {
            return $@"

                UPDATE {Tbl.sub_strip_event}
                SET
                    is_active = 0,
                    modified_by = created_by,
                    modified_datetime = GETDATE(),
                    remarks = 'Auto Inactive - Timeout'
                WHERE event_assign_id = @event_assign_id
                AND is_active = 1;

                UPDATE {Tbl.sub_strip_event_finding}
                SET
                    is_active = 0,
                    modified_by = created_by,
                    modified_datetime = GETDATE(),
                    remarks = 'Auto Inactive - Timeout'
                WHERE event_assign_id = @event_assign_id
                AND is_active = 1;

                UPDATE {Tbl.detection}
                SET
                    is_active = 0,
                    modified_by = created_by,
                    modified_datetime = GETDATE(),
                    remarks = 'Auto Inactive - Timeout'
                WHERE event_assign_id = @event_assign_id
                AND is_active = 1;";
        }

        //internal static string reset_reassigned_events()
        //{
        //    return $@"
        //            UPDATE q
        //            SET
        //                q.event_status = 'pending',
        //                q.modified_by = ea.created_by,
        //                q.modified_datetime = GETDATE(),
        //                q.completed_by = NULL,
        //                q.completed_datetime = NULL,
        //                q.remarks = '10 Minutes Reached'
        //            FROM {Tbl.queued_events_records} q
        //            INNER JOIN {Tbl.event_assign} ea
        //                ON ea.queued_events_records_id = q.queued_events_records_id
        //            WHERE ea.event_assign_id = @event_assign_id
        //              AND q.event_status = 'processing';";
        //}

        //internal static string reset_reassigned_events()
        //{
        //    return $@"
        //            UPDATE q
        //            SET
        //                q.event_status = 'pending',
        //                q.modified_by = ea.created_by,
        //                q.modified_datetime = GETDATE(),
        //                q.completed_by = NULL,
        //                q.completed_datetime = NULL,
        //                q.remarks = '10 Minutes Reached'
        //            FROM {Tbl.queued_events_records} q
        //            INNER JOIN {Tbl.event_assign} ea
        //                ON ea.queued_events_records_id = q.queued_events_records_id
        //            WHERE ea.event_assign_id = @event_assign_id
        //              AND q.event_status IN ('processing','completed');";
        //}
        internal static string reset_reassigned_events()
        {
            return $@"
                    UPDATE q
                    SET
                        q.event_status = 'pending',
                        q.modified_by = ea.created_by,
                        q.modified_datetime = GETDATE(),
                        q.completed_by = NULL,
                        q.completed_datetime = NULL,
                        q.remarks = '10 Minutes Reached'
                    FROM {Tbl.queued_events_records} q
                    INNER JOIN {Tbl.event_assign} ea
                        ON ea.queued_events_records_id = q.queued_events_records_id
                    WHERE ea.event_assign_id = @event_assign_id
                      AND ISNULL(ea.is_final_completed, 0) = 0
                      AND q.event_status IN ('processing', 'completed');";
        }
        #endregion



        internal static string three_minute_timeout()
        {
            return $@"

                    SELECT
                        event_assign_id,
                        patient_enrollment_id,
                        queued_events_records_id,
                        '{Tbl.event_assign}',
                        event_assign_id,
                        'Timeout3Min',
                        work_status,
                        'reassigned',
                        CONCAT('{{""work_status"":""', work_status, '""}}'),
                        '{{""work_status"":""reassigned""}}',
                        'No activity for 3 minutes. Event released automatically.',
                        @modified_by,
                        GETDATE()
                    FROM {Tbl.event_assign}
                    WHERE event_assign_id = @event_assign_id
                      AND work_status IN ('inprogress','completed','partial-completed');


                    -- Event Assign
                    UPDATE ea
                    SET
                        ea.work_status = 'reassigned',
                        ea.is_final_completed = 0,
                        ea.modified_by = @modified_by,
                        ea.modified_datetime = GETDATE(),
                        ea.last_activity_datetime = GETDATE(),
                        ea.remarks = 'No activity for 3 minutes'
                    FROM {Tbl.event_assign} ea
                    WHERE ea.event_assign_id = @event_assign_id
                      AND ea.work_status IN ('inprogress','completed','partial-completed');


                    -- Queue
                    UPDATE q
                    SET
                        q.event_status = 'pending',
                        q.modified_by = @modified_by,
                        q.modified_datetime = GETDATE(),
                        q.completed_by = NULL,
                        q.completed_datetime = NULL,
                        q.remarks = 'Released after 3 minutes inactivity'
                    FROM {Tbl.queued_events_records} q
                    INNER JOIN {Tbl.event_assign} ea
                        ON ea.queued_events_records_id = q.queued_events_records_id
                    WHERE ea.event_assign_id = @event_assign_id
                      AND q.event_status IN ('processing','completed');


                    -- Sub Strip Event
                    UPDATE {Tbl.sub_strip_event}
                    SET
                        is_active = 0,
                        modified_by = created_by,
                        modified_datetime = GETDATE(),
                        remarks = 'Auto inactive - 3 minutes timeout'
                    WHERE event_assign_id = @event_assign_id
                      AND is_active = 1;


                    -- Sub Strip Finding
                    UPDATE {Tbl.sub_strip_event_finding}
                    SET
                        is_active = 0,
                        modified_by = created_by,
                        modified_datetime = GETDATE(),
                        remarks = 'Auto inactive - 3 minutes timeout'
                    WHERE event_assign_id = @event_assign_id
                      AND is_active = 1;


                    -- Detection
                    UPDATE {Tbl.detection}
                    SET
                        is_active = 0,
                        modified_by = created_by,
                        modified_datetime = GETDATE(),
                        remarks = 'Auto inactive - 3 minutes timeout'
                    WHERE event_assign_id = @event_assign_id
                      AND is_active = 1;";
        }

        internal static string get_active_event()
        {
            return $@"
                    SELECT
                        ea.event_assign_id,
                        ea.queued_events_records_id,
                        qer.patient_enrollment_id,
                        qer.enrollment_raw_data_id,
                        qer.priority_level,
                        qer.trigger_type,
                        qer.bpm,
                        qer.event_status,
                        qer.event_file_path,
                        qer.created_by,
                        qer.modified_by,
                        qer.created_datetime,
                        qer.modified_datetime,
                        p.patient_id,
                        p.patient_name,
                        dm.device_id,
                        dm.device_name

                    FROM {Tbl.event_assign} ea
                    INNER JOIN {Tbl.queued_events_records} qer
                        ON qer.queued_events_records_id = ea.queued_events_records_id
                    inner join {Tbl.patient_enrollment} as pe
                        on pe.patient_enrollment_id = ea.patient_enrollment_id 
                    inner join {Tbl.device_mast} as dm
                        on dm.device_id = pe.device_id
                    inner join {Tbl.patient} as p
                        on p.patient_id = pe.patient_id
                    WHERE ea.assigned_by = @assigned_by
                      AND ea.work_status = 'inprogress'
                    ORDER BY ea.event_assign_id DESC";
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
            return $@"insert into {Tbl.patient_base_line} (enrollment_raw_data_id,patient_enrollment_id,record_st,record_end,event_processed_path)
                       values(@enrollment_raw_data_id, @patient_enrollment_id, @record_st, @record_end, @event_processed_path)";
        }
        #endregion
    }


    public class GetByIdEventAssignResDTO
    {
        public long event_assign_id { get; set; }
        public long queued_events_records_id { get; set; }



        internal static string select_by_event_assign_id()
        {
            return $@"SELECT TOP 1 event_assign_id, queued_events_records_id FROM {Tbl.event_assign} 
                WHERE work_status = 'inprogress'
              AND assigned_by = @assigned_by
            ORDER BY event_assign_id DESC";
        }
    }

    public class CurrentStripDTO
    {
        public DateTime? event_date_time { get; set; }
        public string event_file_path { get; set; }
        public string trigger_type { get; set; }
        public int bpm { get; set; }
        public long event_assign_id { get; set; }
        public bool is_final_completed { get; set; }
        public long queued_events_records_id { get; set; }
        public long? parent_detection_id { get; set; }
        public string event_status { get; set; }
        public List<SubStripEventResDTO> sub_strip { get; set; }

        public List<FindingDTO> finding_list { get; set; }

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
                        qer.queued_events_records_id,
                        qer.event_status,
                        d.detection_id AS parent_detection_id
                    FROM {Tbl.event_assign} ea
                    INNER JOIN {Tbl.queued_events_records} qer
                        ON ea.queued_events_records_id = qer.queued_events_records_id
                    LEFT JOIN {Tbl.detection} d
                        ON d.event_assign_id = ea.event_assign_id
                       AND d.sub_strip_event_id IS NULL
                       AND d.is_active = 1
                    WHERE ea.event_assign_id = @event_assign_id
                      AND ea.is_active = 1
                      AND qer.is_active = 1;";
        }
        #endregion
    }
    public class SubStripReqDTO
    {
        public DateTime? from_date { get; set; }
        public DateTime? to_date { get; set; }
        public long patient_id { get; set; }
        public int? pageNumber { get; set; }
        public int? pageSize { get; set; }
        public int? offset => (pageNumber - 1) * pageSize;
        public string? shortColumn { get; set; }
        public string? orderBy { get; set; }

        #region Query
        internal static string get_sub_strip_report(SubStripReqDTO obj)
        {
            string pagination = string.Empty;

            if (obj.pageNumber.HasValue && obj.pageSize.HasValue)
            {
                pagination = @" OFFSET @offset ROWS
                                FETCH NEXT @pageSize ROWS ONLY;";
            }

            return $@"
                        SELECT COUNT(1)
                        FROM {Tbl.sub_strip_event} sse

                        INNER JOIN {Tbl.queued_events_records} qer
                            ON sse.queued_events_records_id = qer.queued_events_records_id

                        INNER JOIN {Tbl.patient_enrollment} pe
                            ON qer.patient_enrollment_id = pe.patient_enrollment_id

                        WHERE pe.patient_id = @patient_id
                          AND sse.is_active = 1

                          AND (@from_date IS NULL
                               OR pe.actual_hookup >= @from_date)

                          AND (@to_date IS NULL
                               OR pe.actual_hookup < DATEADD(DAY, 1, @to_date));


                        SELECT
                            sse.sub_strip_event_id,
                            sse.queued_events_records_id,
                            sse.event_assign_id,
                            d.detection_id,
                            sse.sub_strip_file_path,
                            sse.sub_strip_start_datetime,
                            sse.sub_strip_end_datetime,
                            sse.created_by,
                            sse.created_datetime,
                            sse.is_active,
                            qer.event_start_datetime

                        FROM {Tbl.sub_strip_event} sse

                        INNER JOIN {Tbl.queued_events_records} qer
                            ON sse.queued_events_records_id = qer.queued_events_records_id

                        INNER JOIN {Tbl.patient_enrollment} pe
                            ON qer.patient_enrollment_id = pe.patient_enrollment_id

                        LEFT JOIN {Tbl.detection} d
                            ON d.sub_strip_event_id = sse.sub_strip_event_id
                           AND d.event_assign_id = sse.event_assign_id
                           AND d.is_active = 1

                        WHERE pe.patient_id = @patient_id
                          AND sse.is_active = 1

                          AND (@from_date IS NULL
                               OR pe.actual_hookup >= @from_date)

                          AND (@to_date IS NULL
                               OR pe.actual_hookup < DATEADD(DAY, 1, @to_date))

                        ORDER BY qer.event_start_datetime DESC

                       {pagination};";
        }

        //internal static string get_sub_strip_report(SubStripReqDTO obj)
        //{
        //    return $@"
        //            SELECT COUNT(1)
        //            FROM {Tbl.sub_strip_event} sse
        //            INNER JOIN {Tbl.queued_events_records} qer
        //                ON sse.queued_events_records_id = qer.queued_events_records_id
        //            INNER JOIN {Tbl.patient_enrollment} pe
        //                ON qer.patient_enrollment_id = pe.patient_enrollment_id
        //            WHERE pe.patient_id = @patient_id
        //              AND sse.is_active = 1;

        //           SELECT
        //                sse.sub_strip_event_id,
        //                sse.queued_events_records_id,
        //                sse.event_assign_id,
        //                d.detection_id,
        //                sse.sub_strip_file_path,
        //                sse.sub_strip_start_datetime,
        //                sse.sub_strip_end_datetime,
        //                sse.created_by,
        //                sse.created_datetime,
        //                sse.is_active,
        //                qer.event_start_datetime
        //            FROM {Tbl.sub_strip_event} sse
        //            INNER JOIN {Tbl.queued_events_records} qer
        //                ON sse.queued_events_records_id = qer.queued_events_records_id
        //            INNER JOIN {Tbl.patient_enrollment} pe
        //                ON qer.patient_enrollment_id = pe.patient_enrollment_id
        //            LEFT JOIN {Tbl.detection} d
        //                ON d.sub_strip_event_id = sse.sub_strip_event_id
        //               AND d.event_assign_id = sse.event_assign_id
        //               AND d.is_active = 1
        //            WHERE pe.patient_id = @patient_id
        //              AND sse.is_active = 1
        //            ORDER BY qer.event_start_datetime DESC
        //            OFFSET @offset ROWS
        //            FETCH NEXT @pageSize ROWS ONLY;";
        //}

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
        //public JsonElement json_data { get; set; }
        public IFormFile? edf_file { get; set; }
        public long created_by { get; set; }
        public DateTime? created_datetime { get; set; }
        public long modified_by { get; set; }
        public DateTime? modified_datetime { get; set; }
        public bool is_active { get; set; }

        #region Query   
        internal static string check_active_published_sub_strip()
        {
            return $@"   SELECT count(1)
                                FROM tbl_patient_report AS pr
                                left JOIN tbl_queued_events_records AS qe
                                    ON qe.patient_enrollment_id = pr.patient_enrollment_id
                                    left join tbl_sub_strip_event as ss 
                                    on ss.queued_events_records_id = qe.queued_events_records_id
                                WHERE pr.report_status = 'completed'
                                  AND pr.is_active = 1
                                  AND qe.completed_by IS NOT NULL
                                  AND qe.is_active = 1
                                  AND qe.event_status = 'completed'
                                  and ss.is_active = 1 
                                  and ss.sub_strip_event_id = @sub_strip_event_id";
        }

        internal static string check_published_sub_strip()
        {
            return $@"  SELECT count(1)
                            FROM tbl_patient_report AS pr
                            INNER JOIN tbl_queued_events_records AS qe
                                ON qe.patient_enrollment_id = pr.patient_enrollment_id
                            WHERE pr.report_status = 'completed'
                              AND pr.is_active = 1
                              AND qe.completed_by IS NOT NULL
                              AND qe.is_active = 1
                              AND qe.event_status = 'completed'
                              and qe.queued_events_records_id = @queued_events_records_id";
        }
        internal static string insert_sub_strip()
        {
            return $@" insert into {Tbl.sub_strip_event}(queued_events_records_id,event_assign_id,sub_strip_file_path,sub_strip_start_datetime,sub_strip_end_datetime,remarks,created_by)
                       values(@queued_events_records_id, @event_assign_id, @sub_strip_file_path, @sub_strip_start_datetime, @sub_strip_end_datetime, @remarks, @created_by)";
        }

        //internal static string insert_sub_strip_log()
        //{
        //    return $@"
        //      INSERT INTO {Tbl.event_assign_log}
        //      (
        //          event_assign_id,
        //          patient_enrollment_id,
        //          queued_events_records_id,
        //          table_name,
        //          record_id,
        //          action_type,
        //          old_value,
        //          new_value,
        //          remarks,
        //          created_by,
        //          created_datetime
        //      )
        //      SELECT
        //          s.event_assign_id,
        //          ea.patient_enrollment_id,
        //          s.queued_events_records_id,
        //          '{Tbl.sub_strip_event}',
        //          s.sub_strip_event_id,
        //          'Insert',
        //          NULL,
        //          CONCAT(
        //              '{{',
        //              '""sub_strip_file_path"":""', ISNULL(s.sub_strip_file_path,''), '"",',
        //              '""sub_strip_start_datetime"":""', ISNULL(s.sub_strip_start_datetime,''), '"",',
        //              '""sub_strip_end_datetime"":""', ISNULL(s.sub_strip_end_datetime,''), '""',
        //              '}}'
        //          ),
        //          'Sub Strip Created',
        //          s.created_by,
        //          GETDATE()
        //      FROM {Tbl.sub_strip_event} s
        //      INNER JOIN {Tbl.event_assign} ea
        //          ON ea.event_assign_id = s.event_assign_id
        //      WHERE s.sub_strip_event_id = @sub_strip_event_id;";
        //}
        internal static string get_sub_strip()
        {
            return $@"
                    SELECT
                        sse.sub_strip_event_id,
                        sse.queued_events_records_id,
                        sse.event_assign_id,
                        d.detection_id,
                        sse.sub_strip_file_path,
                        sse.sub_strip_start_datetime,
                        sse.sub_strip_end_datetime,
                        sse.remarks,
                        sse.created_by,
                        sse.created_datetime,
                        sse.modified_by,
                        sse.modified_datetime,
                        sse.is_active
                    FROM {Tbl.sub_strip_event} sse
                    LEFT JOIN {Tbl.detection} d
                        ON d.sub_strip_event_id = sse.sub_strip_event_id
                       AND d.event_assign_id = sse.event_assign_id
                       AND d.is_active = 1
                    WHERE sse.event_assign_id = @event_assign_id
                      AND sse.is_active = 1
                    ORDER BY sse.sub_strip_start_datetime;";
        }

        internal static string in_active_sub_strip()
        {
            return $@"Update {Tbl.sub_strip_event}
                        set is_active = 0,
                        modified_by = @modified_by,
                        modified_datetime = getdate(),
                        remarks = 'deleted'
                        where sub_strip_event_id = @sub_strip_event_id";
        }
        internal static string inactive_sub_strip_log()
        {
            return $@"
                    INSERT INTO {Tbl.event_assign_log}
                    (
                        event_assign_id,
                        patient_enrollment_id,
                        queued_events_records_id,
                        table_name,
                        record_id,
                        action_type,
                        old_value,
                        new_value,
                        remarks,
                        created_by,
                        created_datetime
                    )
                    SELECT
                        s.event_assign_id,
                        ea.patient_enrollment_id,
                        s.queued_events_records_id,
                        '{Tbl.sub_strip_event}',
                        s.sub_strip_event_id,
                        'in_active',
                        '{{""is_active"":1}}',
                        '{{""is_active"":0}}',
                        'Sub Strip Inactivated',
                        @modified_by,
                        GETDATE()
                    FROM {Tbl.sub_strip_event} s
                    INNER JOIN {Tbl.event_assign} ea
                        ON ea.event_assign_id = s.event_assign_id
                    WHERE s.sub_strip_event_id = @sub_strip_event_id;";
        }


        #endregion
    }
    public class SubStripEventResDTO
    {
        public long sub_strip_event_id { get; set; }
        public long queued_events_records_id { get; set; }
        public long detection_id { get; set; }
        public long event_assign_id { get; set; }
        public string sub_strip_file_path { get; set; }
        public string? sub_strip_start_datetime { get; set; }
        public string? event_start_datetime { get; set; }
        public string? sub_strip_end_datetime { get; set; }
        public string remarks { get; set; }
        public long created_by { get; set; }
        public DateTime? created_datetime { get; set; }
        public long modified_by { get; set; }
        public DateTime? modified_datetime { get; set; }
        public bool is_active { get; set; }
        public List<FindingDTO> finding_list { get; set; }


        #region Query
        //internal static string sub_strip_files_select()
        //{
        //    return $@"
        //            SELECT DISTINCT
        //                sse.sub_strip_event_id,
        //                sse.queued_events_records_id,
        //                sse.event_assign_id,
        //                sse.sub_strip_file_path,
        //                sse.sub_strip_start_datetime,
        //                sse.sub_strip_end_datetime,
        //                sse.remarks,
        //                sse.created_by,
        //                sse.created_datetime,
        //                sse.modified_by,
        //                sse.modified_datetime,
        //                sse.is_active
        //            FROM {Tbl.sub_strip_event} sse
        //            INNER JOIN {Tbl.sub_strip_event_finding} sef
        //                ON sef.sub_strip_event_id = sse.sub_strip_event_id
        //               AND sef.event_assign_id = sse.event_assign_id
        //               AND sef.is_active = 1
        //            WHERE sse.event_assign_id = @event_assign_id
        //              AND sse.is_active = 1
        //            ORDER BY sse.created_datetime;";
        //}
        internal static string sub_strip_files_select()
        {
            return $@"
            SELECT
                sse.sub_strip_event_id,
                sse.queued_events_records_id,
                sse.event_assign_id,
                sse.sub_strip_file_path,
                sse.sub_strip_start_datetime,
                sse.sub_strip_end_datetime,
                sse.remarks,
                sse.created_by,
                sse.created_datetime,
                sse.modified_by,
                sse.modified_datetime,
                sse.is_active
            FROM {Tbl.sub_strip_event} sse
            WHERE sse.event_assign_id = @event_assign_id
              AND sse.is_active = 1
            ORDER BY sse.created_datetime;";
        }

        internal static string time_out_substrip()
        {
            return $@"  
                    update {Tbl.sub_strip_event} 
                    set 
                        is_active = 0,
                        modified_by = @modified_by,
                        modified_datetime = getdate(),
                        remarks = 'No activity for 3 minutes'
                        WHERE event_assign_id = @event_assign_id and is_active = 1";
        }
        #endregion

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
                      AND parent_finding_id IS NULL order by priority asc";
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
        public long? detection_id { get; set; }
        public long event_assign_id { get; set; }
        public long? sub_strip_event_id { get; set; }
        public long? queued_events_records_id { get; set; }

        public List<FindingDTO> finding_list { get; set; }

        public long created_by { get; set; }
        public DateTime? created_datetime { get; set; }
        public long? modified_by { get; set; }
        public DateTime? modified_datetime { get; set; }
        public long? completed_by { get; set; }
        public DateTime? completed_datetime { get; set; }
        public bool is_active { get; set; }

        public string remarks { get; set; }

        public bool is_experienced_user { get; set; }


        #region Query

        internal static string get_total_active_sub_strips()
        {
            return $@"
                    SELECT COUNT(*)
                    FROM {Tbl.sub_strip_event}
                    WHERE event_assign_id = @event_assign_id
                      AND is_active = 1";
        }

        internal static string get_completed_sub_strips()
        {
            return $@"
                    SELECT COUNT(DISTINCT sub_strip_event_id)
                    FROM {Tbl.detection}
                    WHERE event_assign_id = @event_assign_id
                      AND is_active = 1";
        }

        internal static string get_detection()
        {
            return $@"
                    SELECT TOP 1 detection_id
                    FROM {Tbl.detection}
                    WHERE queued_events_records_id = @queued_events_records_id
                      AND (
                            (sub_strip_event_id = @sub_strip_event_id)
                            OR
                            (sub_strip_event_id IS NULL AND @sub_strip_event_id IS NULL)
                          )
                      AND event_assign_id = @event_assign_id
                      AND is_active = 1";
        }

        internal static string insert_sub_strip_finding()
        {
            return $@"
                    INSERT INTO {Tbl.sub_strip_event_finding}
                    (
                        detection_id,
                        queued_events_records_id,
                        sub_strip_event_id,
                        event_finding_id,
                        event_assign_id,
                        value1,
                        value2,
                        created_by,
                        last_updated_sequence
                    )
                    VALUES
                    (
                        @detection_id,
                        @queued_events_records_id,
                        @sub_strip_event_id,
                        @event_finding_id,
                        @event_assign_id,
                        @value1,
                        @value2,
                        @created_by,
                        NEXT VALUE FOR FindingUpdateSequence
                    )";
        }
        internal static string insert_sub_strip_finding_log()
        {
            return $@"
                    INSERT INTO {Tbl.event_assign_log}
                    (
                        event_assign_id,
                        patient_enrollment_id,
                        queued_events_records_id,
                        table_name,
                        record_id,
                        action_type,
                        old_value,
                        new_value,
                        remarks,
                        created_by,
                        created_datetime
                    )
                    SELECT
                        f.event_assign_id,
                        ea.patient_enrollment_id,
                        f.queued_events_records_id,
                        '{Tbl.sub_strip_event_finding}',
                        f.sub_strip_event_finding_id,
                        'update_sub_strip_insert',
                        NULL,
                        CONCAT(
                            '{{',
                            '""event_finding_id"":', f.event_finding_id, ',',
                            '""value1"":', ISNULL(f.value1,0), ',',
                            '""value2"":', ISNULL(f.value2,0),
                            '}}'
                        ),
                        'Finding Added',
                        f.created_by,
                        GETDATE()
                    FROM {Tbl.sub_strip_event_finding} f
                    INNER JOIN {Tbl.event_assign} ea
                        ON ea.event_assign_id = f.event_assign_id
                    WHERE f.sub_strip_event_finding_id = @sub_strip_event_finding_id;";
        }


        internal static string insert_detection()
        {
            return $@"
                    INSERT INTO {Tbl.detection}
                    (
                        sub_strip_event_id,
                        event_assign_id,
                        queued_events_records_id,
                        detection_status,
                        created_by
                    )
                    VALUES
                    (
                        @sub_strip_event_id,
                        @event_assign_id,
                        @queued_events_records_id,
                        @detection_status,
                        @created_by
                    )";
        }
        internal static string insert_detection_log()
        {
            return $@"
                    INSERT INTO {Tbl.event_assign_log}
                    (
                        event_assign_id,
                        patient_enrollment_id,
                        queued_events_records_id,
                        table_name,
                        record_id,
                        action_type,
                        old_value,
                        new_value,
                        remarks,
                        created_by,
                        created_datetime
                    )
                    SELECT
                        d.event_assign_id,
                        ea.patient_enrollment_id,
                        d.queued_events_records_id,
                        '{Tbl.detection}',
                        d.detection_id,
                        @action_type,
                        NULL,
                        CONCAT('{{""detection_status"":""', d.detection_status, '""}}'),
                        'Detection Created',
                        d.created_by,
                        GETDATE()
                    FROM {Tbl.detection} d
                    INNER JOIN {Tbl.event_assign} ea
                        ON ea.event_assign_id = d.event_assign_id
                    WHERE d.detection_id = @detection_id;";
        }
        internal static string update_detection()
        {
            return $@"Update {Tbl.detection}
                        Set  modified_datetime = getdate(), modified_by = @modified_by,remarks = 'update_detection'
                        Where detection_id = @detection_id";
        }
        internal static string update_detection_log()
        {
            return $@"

                    SELECT
                        d.event_assign_id,
                        ea.patient_enrollment_id,
                        d.queued_events_records_id,
                        '{Tbl.detection}',
                        d.detection_id,
                        'Update_detection',
                        CONCAT('{{""modified_by"":', ISNULL(d.modified_by,0), '}}'),
                        CONCAT('{{""modified_by"":', @modified_by, '}}'),
                        'Detection Updated',
                        @modified_by,
                        GETDATE()
                    FROM {Tbl.detection} d
                    INNER JOIN {Tbl.event_assign} ea
                        ON ea.event_assign_id = d.event_assign_id
                    WHERE d.detection_id = @detection_id;";
        }

        internal static string inactive_sub_strip_finding()
        {
            return $@"
                    UPDATE {Tbl.sub_strip_event_finding}
                    SET
                        is_active = 0,
                        modified_by = @modified_by,
                        modified_datetime = getdate(),
                        remarks = 'deleted_sub_strip'
                    WHERE detection_id = @detection_id
                      AND is_active = 1";
        }
        internal static string inactive_sub_strip_finding_log()
        {
            return $@"
                    INSERT INTO {Tbl.event_assign_log}
                    (
                        event_assign_id,
                        patient_enrollment_id,
                        queued_events_records_id,
                        table_name,
                        record_id,
                        action_type,
                        old_value,
                        new_value,
                        remarks,
                        created_by,
                        created_datetime
                    )
                    SELECT
                        f.event_assign_id,
                        ea.patient_enrollment_id,
                        f.queued_events_records_id,
                        '{Tbl.sub_strip_event_finding}',
                        f.sub_strip_event_finding_id,
                        'deleted_sub_strip',
                        '{{""is_active"":1}}',
                        '{{""is_active"":0}}',
                        'Finding Inactivated',
                        @modified_by,
                        GETDATE()
                    FROM {Tbl.sub_strip_event_finding} f
                    INNER JOIN {Tbl.event_assign} ea
                        ON ea.event_assign_id = f.event_assign_id
                    WHERE f.detection_id = @detection_id
                      AND f.is_active = 0;";
        }

        internal static string time_out_detection()
        {
            return $@"
                    UPDATE {Tbl.detection}
                    SET
                        is_active = 0,
                        remarks = 'No activity for 3 minutes',
                        modified_by = @modified_by,
                        modified_datetime = GETDATE()
                    WHERE event_assign_id = @event_assign_id
                      AND is_active = 1;";
        }

        internal static string time_out_substrip_finding()
        {
            return $@"
                    UPDATE {Tbl.sub_strip_event_finding}
                    SET
                        is_active = 0,
                        remarks = 'No activity for 3 minutes',
                        modified_by = @modified_by,
                        modified_datetime = GETDATE()
                    WHERE event_assign_id = @event_assign_id
                      AND is_active = 1;";
        }
        #endregion
    }

    public class FindingDTO
    {
        public long? sub_strip_event_id { get; set; }
        public long event_finding_id { get; set; }
        public string? finding_name { get; set; }
        public decimal? value1 { get; set; }
        public decimal? value2 { get; set; }
        public long? detection_id { get; set; }
        public long? queued_events_records_id { get; set; }

        #region Query

        internal static string get_event_findings()
        {
            return $@"
                    SELECT DISTINCT
                        sef.event_finding_id,
                        sef.queued_events_records_id,
                        fm.finding_name,
                        sef.value1,
                        sef.value2
                    FROM {Tbl.sub_strip_event_finding} AS sef
                    INNER JOIN {Tbl.event_finding_mast} AS fm
                        ON fm.event_finding_id = sef.event_finding_id
                    WHERE
                        sef.queued_events_records_id = @queued_events_records_id
                        AND sef.is_active = 1
                        AND fm.is_active = 1";
        }

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
        WHERE ssf.detection_id = @detection_id and ssf.is_active = 1
        ORDER BY ssf.sub_strip_event_finding_id";
        }

        internal static string get_fetch_history_by_id()
        {
            return $@"
                    SELECT
                        c.queued_events_records_id,
                        c.event_finding_id,
                        c.sub_strip_event_id,
                        d.finding_name,
                        c.value1,
                        c.value2,
                        c.detection_id
                    FROM {Tbl.sub_strip_event_finding} AS c
                    INNER JOIN {Tbl.event_finding_mast} AS d
                        ON d.event_finding_id = c.event_finding_id
                    WHERE c.queued_events_records_id = @queued_events_records_id
                        AND c.is_active = 1
                        AND d.is_active = 1
                    GROUP BY
                        c.queued_events_records_id,
                        c.event_finding_id,
                        c.sub_strip_event_id,
                        d.finding_name,
                        c.value1,
                        c.value2,
                        c.detection_id;";
        }
        internal static string get_parent_finding()
        {
            return $@"
                    SELECT
                        sef.sub_strip_event_id,
                        sef.event_finding_id,
                        efm.finding_name,
                        sef.value1,
                        sef.value2,
                        sef.detection_id
                    FROM {Tbl.sub_strip_event_finding} sef
                    INNER JOIN {Tbl.event_finding_mast} efm
                        ON sef.event_finding_id = efm.event_finding_id
                    WHERE sef.event_assign_id = @event_assign_id
                      AND sef.sub_strip_event_id IS NULL
                      AND sef.is_active = 1
                    ORDER BY sef.last_updated_sequence;";
        }
        internal static string get_parent_finding_by_detection()
        {
            return $@"
                    SELECT
                        sef.sub_strip_event_id,
                        sef.event_finding_id,
                        efm.finding_name,
                        sef.value1,
                        sef.value2,
                        sef.detection_id
                    FROM {Tbl.sub_strip_event_finding} sef
                    INNER JOIN {Tbl.event_finding_mast} efm
                        ON sef.event_finding_id = efm.event_finding_id
                    WHERE sef.detection_id = @detection_id
                      AND sef.sub_strip_event_id IS NULL
                      AND sef.is_active = 1
                    ORDER BY sef.last_updated_sequence;";
        }
        internal static string get_sub_strip_finding()
        {
            return $@"
                    SELECT
                        sef.event_finding_id,
                        efm.finding_name,
                        sef.value1,
                        sef.value2,
                        sef.detection_id
                    FROM {Tbl.sub_strip_event_finding} sef
                    INNER JOIN {Tbl.event_finding_mast} efm
                        ON sef.event_finding_id = efm.event_finding_id
                    WHERE sef.event_assign_id = @event_assign_id
                      AND sef.sub_strip_event_id = @sub_strip_event_id
                      AND sef.is_active = 1
                    ORDER BY sef.last_updated_sequence;";
        }
        internal static string get_sub_strip_finding_by_detection()
        {
            return $@"
                    SELECT
                        sef.event_finding_id,
                        efm.finding_name,
                        sef.value1,
                        sef.value2,
                        sef.detection_id
                    FROM {Tbl.sub_strip_event_finding} sef
                    INNER JOIN {Tbl.event_finding_mast} efm
                        ON sef.event_finding_id = efm.event_finding_id
                    WHERE sef.detection_id = @detection_id
                      AND sef.sub_strip_event_id = @sub_strip_event_id
                      AND sef.is_active = 1
                    ORDER BY sef.last_updated_sequence;";
        }
        internal static string get_parent_finding_list()
        {
            return $@"
                    SELECT
                        sef.detection_id,
                        sef.sub_strip_event_id,
                        sef.event_finding_id,
                        efm.finding_name,
                        sef.value1,
                        sef.value2
                    FROM {Tbl.sub_strip_event_finding} sef
                    INNER JOIN {Tbl.event_finding_mast} efm
                        ON efm.event_finding_id = sef.event_finding_id
                    WHERE sef.event_assign_id = @event_assign_id
                      AND sef.sub_strip_event_id IS NULL
                      AND sef.is_active = 1
                    ORDER BY sef.sub_strip_event_finding_id;";
        }

        //internal static string get_sub_strip_finding_list()
        //{
        //    return $@"
        //            SELECT
        //                sef.detection_id,
        //                sef.sub_strip_event_id,
        //                sef.event_finding_id,
        //                efm.finding_name,
        //                sef.value1,
        //                sef.value2
        //            FROM {Tbl.sub_strip_event_finding} sef
        //            INNER JOIN {Tbl.event_finding_mast} efm
        //                ON efm.event_finding_id = sef.event_finding_id
        //            WHERE sef.event_assign_id = @event_assign_id
        //              AND sef.sub_strip_event_id = @sub_strip_event_id
        //              AND sef.is_active = 1
        //            ORDER BY sef.sub_strip_event_finding_id;";
        //}

        internal static string get_finding_sub_strip_list()
        {
            return $@"
        SELECT
            sef.event_assign_id,
            sef.event_finding_id,
            efm.finding_name,
            sef.sub_strip_event_id,
            sef.detection_id,
            sef.value1,
            sef.value2
        FROM {Tbl.sub_strip_event_finding} sef
        INNER JOIN {Tbl.event_finding_mast} efm
            ON efm.event_finding_id = sef.event_finding_id
        WHERE sef.event_assign_id = @event_assign_id
          AND sef.sub_strip_event_id = @sub_strip_event_id
          AND sef.is_active = 1
        ORDER BY
            sef.event_finding_id,
            sef.sub_strip_event_id,
            sef.sub_strip_event_finding_id;";
        }
        //internal static string get_finding_sub_strip_list()
        //{
        //    return $@"
        //            SELECT
        //                sef.event_assign_id,
        //                sef.event_finding_id,
        //                efm.finding_name,
        //                sef.sub_strip_event_id,
        //                sef.detection_id,
        //                sef.value1,
        //                sef.value2
        //            FROM {Tbl.sub_strip_event_finding} sef
        //            INNER JOIN {Tbl.event_finding_mast} efm
        //                ON efm.event_finding_id = sef.event_finding_id
        //            WHERE sef.event_assign_id = @event_assign_id
        //              AND sef.sub_strip_event_id IS NOT NULL
        //              AND sef.is_active = 1
        //            ORDER BY
        //                sef.event_finding_id,
        //                sef.sub_strip_event_id,
        //                sef.sub_strip_event_finding_id;";
        //}

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
                    LEFT JOIN {Tbl.event_assign} ea
                        ON ea.event_assign_id = d.event_assign_id
                    LEFT JOIN {Tbl.patient_enrollment} p
                        ON qu.patient_enrollment_id = p.patient_enrollment_id
                    LEFT JOIN {Tbl.patient} pt
                        ON pt.patient_id = p.patient_id
                    LEFT JOIN {Tbl.user_master} u
                        ON u.user_id = d.created_by
                    WHERE ea.work_status = 'partial-completed'
                      AND d.is_active = 1
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
                        qu.completed_datetime,
                        qu.event_status,
                        qu.modified_by,

                        ea.event_assign_id,
                        ea.work_status,

                        u.user_id,
                        u.user_name,

                        pt.patient_id,
                        pt.patient_name

                    FROM {Tbl.detection} d
                    LEFT JOIN {Tbl.queued_events_records} qu
                        ON d.queued_events_records_id = qu.queued_events_records_id
                    LEFT JOIN {Tbl.event_assign} ea
                        ON ea.event_assign_id = d.event_assign_id
                    LEFT JOIN {Tbl.patient_enrollment} p
                        ON qu.patient_enrollment_id = p.patient_enrollment_id
                    LEFT JOIN {Tbl.patient} pt
                        ON pt.patient_id = p.patient_id
                    LEFT JOIN {Tbl.user_master} u
                        ON u.user_id = d.created_by

                    WHERE ea.work_status = 'partial-completed'
                      AND d.is_active = 1
                      AND (
                            ISNULL(@search, '') = ''
                            OR pt.patient_name LIKE '%' + @search + '%'
                            OR u.user_name LIKE '%' + @search + '%'
                          )

                    ORDER BY {obj.shortColumn} {obj.orderBy}
                    OFFSET @offSet ROWS
                    FETCH NEXT @pageSize ROWS ONLY;";
        }
        #endregion
    }


    public class CompletedFindingListDTO
    {
        public long detection_id { get; set; }
        public long sub_strip_event_id { get; set; }
        public long queued_events_records_id { get; set; }
        public long event_assign_id { get; set; }

        public string detection_status { get; set; }

        public long created_by { get; set; }
        public DateTime created_datetime { get; set; }

        public string trigger_type { get; set; }
        public DateTime completed_datetime { get; set; }
        public string event_status { get; set; }

        public long user_id { get; set; }
        public string user_name { get; set; }

        public long patient_id { get; set; }
        public string patient_name { get; set; }
    }

    public class CompletedFindingListResDTO
    {
        public long queued_events_records_id { get; set; }
        public long event_assign_id { get; set; }
        public long patient_enrollment_id { get; set; }
        public long enrollment_raw_data_id { get; set; }
        public string priority_level { get; set; }
        public string trigger_type { get; set; }
        public int bpm { get; set; }
        public string event_status { get; set; }
        public string event_file_path { get; set; }
        public DateTime? created_datetime { get; set; }
        public DateTime? completed_datetime { get; set; }
        public List<FetchHistoryDTO>? fetch_history { get; set; }

        public List<SymptomDTO> symptoms { get; set; }

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
        internal static string select_complete_qc_report()
        {
            return $@"
                    SELECT
                    d.detection_id,
                    d.event_assign_id,
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
                    qr.completed_datetime,

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

                    WHERE d.detection_id = @detection_id and d.is_active = 1

                   GROUP BY
                        d.detection_id,
                        d.event_assign_id,
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
                        qr.completed_datetime,
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

    public class ConfirmSubmitDTO
    {
        public bool qc_tech_detection { get; set; }
        public long? detection_id { get; set; }
        public long parent_detection_id { get; set; }
        public long picked_by { get; set; }
        public DateTime? picked_datetime { get; set; }

        public long? sub_strip_event_id { get; set; }
        public long? queued_events_records_id { get; set; }

        public List<FindingDTO> finding_list { get; set; }

        public long created_by { get; set; }
        public DateTime? created_datetime { get; set; }
        public long? modified_by { get; set; }
        public DateTime? modified_datetime { get; set; }


        #region Query
        internal static string insert_detection()
        {
            return $@"insert into {Tbl.detection} (parent_detection_id,sub_strip_event_id,queued_events_records_id,detection_status,created_by)
                    values(@parent_detection_id,@sub_strip_event_id, @queued_events_records_id, @detection_status, @created_by)";
        }
        internal static string update_detection()
        {
            return $@"Update {Tbl.detection}
                       set sub_strip_event_id =@sub_strip_event_id, queued_events_records_id =@queued_events_records_id, modified_by =@modified_by,  modified_datetime = @modified_datetime
                       Where detection_id = @detection_id and is_active =1 ";
        }
        internal static string insert_sub_strip_finding()
        {
            return $@"insert into {Tbl.sub_strip_event_finding} (sub_strip_event_id,queued_events_records_id,event_finding_id,detection_id, value1, value2,created_by)
                        values (@sub_strip_event_id, @queued_events_records_id, @event_finding_id, @detection_id, @value1, @value2, @created_by)";
        }
        internal static string update_sub_strip_finding()
        {
            return $@"Update{Tbl.sub_strip_event_finding}
";
        }

        internal static string pick_detection()
        {
            return $@"
                    UPDATE {Tbl.detection}
                    SET
                        picked_by = @picked_by,
                        picked_datetime = GETDATE()
                    WHERE detection_id = @detection_id
                    AND is_active = 1
                    AND picked_by IS NULL";
        }

        internal static string release_pick_detection()
        {
            return $@"update {Tbl.detection}
                     set picked_by = null,
                         picked_datetime = null,
                        modified_by = @modified_by,
                        modified_datetime = getdate()
                      WHERE detection_id = @detection_id ";
        }

        internal static string get_next_qc_detection()
        {
            return $@"
                    SELECT TOP 1
                        detection_id,
                        parent_detection_id,
                        sub_strip_event_id,
                        queued_events_records_id
                    FROM {Tbl.detection}
                    WHERE is_active = 1
                      AND detection_status NOT IN ('confirm-submit', 'submit')
                      AND picked_by IS NULL
                    ORDER BY detection_id asc";
        }
        #endregion
    }

    public class PriorityCompleteReportReqDTO
    {
        public DateTime? from_date { get; set; }
        public DateTime? to_date { get; set; }
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public int offSet => (pageNumber - 1) * pageSize;
        public string shortColumn { get; set; }
        public string orderBy { get; set; }
        public long patient_id { get; set; }

        #region Query
        internal static string detection_complete_report(PriorityCompleteReportReqDTO obj)
        {
            var shortColumn = string.IsNullOrEmpty(obj.shortColumn) ? "q.event_start_datetime" : obj.shortColumn;

            var orderBy = string.IsNullOrEmpty(obj.orderBy) ? "DESC" : obj.orderBy;

            return $@"
                    SELECT COUNT(*)
                    FROM
                    (
                        SELECT d.event_assign_id
                        FROM {Tbl.detection} d

                        INNER JOIN {Tbl.queued_events_records} q
                            ON q.queued_events_records_id = d.queued_events_records_id

                        INNER JOIN {Tbl.patient_enrollment} pe
                            ON pe.patient_enrollment_id = q.patient_enrollment_id

                        WHERE d.is_active = 1
                          AND d.detection_status IN ('submit','confirm-submit')
                            AND (@from_date IS NULL 
                                   OR pe.actual_hookup >= @from_date)

                              AND (@to_date IS NULL 
                                   OR pe.actual_hookup < DATEADD(DAY, 1, @to_date))
                          AND (@patient_id = 0 OR pe.patient_id = @patient_id)

                          -- Parent detection must exist
                          AND EXISTS
                          (
                              SELECT 1
                              FROM {Tbl.detection} pd
                              WHERE pd.event_assign_id = d.event_assign_id
                                AND pd.sub_strip_event_id IS NULL
                                AND pd.is_active = 1
                                AND pd.detection_status IN ('submit','confirm-submit')
                          )

                        GROUP BY d.event_assign_id
                    ) A;


                    SELECT
                        MAX(d.detection_id)              AS detection_id,
                        d.event_assign_id,
                        MAX(d.queued_events_records_id)  AS queued_events_records_id,
                        MAX(q.event_file_path)           AS event_file_path,
                        MAX(d.detection_status)          AS detection_status,
                        MAX(d.remarks)                   AS remarks,
                        MAX(d.created_by)                AS created_by,
                        MAX(d.created_datetime)           AS created_datetime,
                        MAX(d.modified_by)               AS modified_by,
                        MAX(d.modified_datetime)         AS modified_datetime,
                        MAX(d.picked_by)                 AS picked_by,
                        MAX(d.picked_datetime)           AS picked_datetime,
                        MAX(pe.patient_id)               AS patient_id,
                        MAX(p.patient_name)              AS patient_name,
                        MAX(q.event_start_datetime)      AS event_start_datetime,
                        MAX(q.event_end_datetime)        AS event_end_datetime

                    FROM {Tbl.detection} d

                    INNER JOIN {Tbl.queued_events_records} q
                        ON q.queued_events_records_id = d.queued_events_records_id

                    INNER JOIN {Tbl.patient_enrollment} pe
                        ON pe.patient_enrollment_id = q.patient_enrollment_id

                    INNER JOIN {Tbl.patient} p
                        ON p.patient_id = pe.patient_id

                    WHERE d.is_active = 1
                      AND d.detection_status IN ('submit','confirm-submit')
                          AND (@from_date IS NULL 
                                   OR pe.actual_hookup >= @from_date)

                              AND (@to_date IS NULL 
                                   OR pe.actual_hookup < DATEADD(DAY, 1, @to_date))
                      AND (@patient_id = 0 OR pe.patient_id = @patient_id)

                      -- Parent detection must exist
                      AND EXISTS
                      (
                          SELECT 1
                          FROM {Tbl.detection} pd
                          WHERE pd.event_assign_id = d.event_assign_id
                            AND pd.sub_strip_event_id IS NULL
                            AND pd.is_active = 1
                            AND pd.detection_status IN ('submit','confirm-submit')
                      )

                    GROUP BY d.event_assign_id

                    ORDER BY {shortColumn} {orderBy},
                             MAX(d.detection_id) DESC

                    OFFSET @offSet ROWS
                    FETCH NEXT @pageSize ROWS ONLY;";
        }
        //internal static string detection_complete_report(PriorityCompleteReportReqDTO obj)
        //{
        //    var shortColumn = string.IsNullOrEmpty(obj.shortColumn) ? "q.event_start_datetime" : obj.shortColumn;

        //    var orderBy = string.IsNullOrEmpty(obj.orderBy) ? "DESC" : obj.orderBy;

        //    return $@"
        //            SELECT COUNT(*)
        //            FROM
        //            (
        //                SELECT d.event_assign_id
        //                FROM {Tbl.detection} d
        //                INNER JOIN {Tbl.queued_events_records} q
        //                    ON q.queued_events_records_id = d.queued_events_records_id
        //                INNER JOIN {Tbl.patient_enrollment} pe
        //                    ON pe.patient_enrollment_id = q.patient_enrollment_id
        //                WHERE d.is_active = 1
        //                  AND d.detection_status IN ('submit','confirm-submit')
        //                  AND (@patient_id = 0 OR pe.patient_id = @patient_id)

        //                  -- Parent detection must exist
        //                  AND EXISTS
        //                  (
        //                      SELECT 1
        //                      FROM {Tbl.detection} pd
        //                      WHERE pd.event_assign_id = d.event_assign_id
        //                        AND pd.sub_strip_event_id IS NULL
        //                        AND pd.is_active = 1
        //                        AND pd.detection_status IN ('submit','confirm-submit')
        //                  )
        //                  -- All active sub strips must have detection
        //                  AND
        //                  (
        //                      SELECT COUNT(*)
        //                      FROM {Tbl.sub_strip_event} s
        //                      WHERE s.event_assign_id = d.event_assign_id
        //                        AND s.is_active = 1
        //                  )
        //                  =
        //                  (
        //                      SELECT COUNT(*)
        //                      FROM {Tbl.detection} sd
        //                      WHERE sd.event_assign_id = d.event_assign_id
        //                        AND sd.sub_strip_event_id IS NOT NULL
        //                        AND sd.is_active = 1
        //                        AND sd.detection_status IN ('submit','confirm-submit')
        //                  )
        //                GROUP BY d.event_assign_id
        //            ) A;


        //            SELECT
        //                MAX(d.detection_id)              AS detection_id,
        //                d.event_assign_id,
        //                MAX(d.queued_events_records_id)  AS queued_events_records_id,
        //                MAX(q.event_file_path)           AS event_file_path,
        //                MAX(d.detection_status)          AS detection_status,
        //                MAX(d.remarks)                   AS remarks,
        //                MAX(d.created_by)                AS created_by,
        //                MAX(d.created_datetime)          AS created_datetime,
        //                MAX(d.modified_by)               AS modified_by,
        //                MAX(d.modified_datetime)         AS modified_datetime,
        //                MAX(d.picked_by)                 AS picked_by,
        //                MAX(d.picked_datetime)           AS picked_datetime,
        //                MAX(pe.patient_id)               AS patient_id,
        //                MAX(p.patient_name)              AS patient_name,
        //                MAX(q.event_start_datetime)      AS event_start_datetime,
        //                MAX(q.event_end_datetime)        AS event_end_datetime
        //            FROM {Tbl.detection} d
        //            INNER JOIN {Tbl.queued_events_records} q
        //                ON q.queued_events_records_id = d.queued_events_records_id
        //            INNER JOIN {Tbl.patient_enrollment} pe
        //                ON pe.patient_enrollment_id = q.patient_enrollment_id
        //            INNER JOIN {Tbl.patient} p
        //                ON p.patient_id = pe.patient_id
        //            WHERE d.is_active = 1
        //              AND d.detection_status IN ('submit','confirm-submit')
        //              AND (@patient_id = 0 OR pe.patient_id = @patient_id)

        //              -- Parent detection must exist
        //              AND EXISTS
        //              (
        //                  SELECT 1
        //                  FROM {Tbl.detection} pd
        //                  WHERE pd.event_assign_id = d.event_assign_id
        //                    AND pd.sub_strip_event_id IS NULL
        //                    AND pd.is_active = 1
        //                    AND pd.detection_status IN ('submit','confirm-submit')
        //              )
        //              -- All active sub strips must have detection
        //              AND
        //              (
        //                  SELECT COUNT(*)
        //                  FROM {Tbl.sub_strip_event} s
        //                  WHERE s.event_assign_id = d.event_assign_id
        //                    AND s.is_active = 1
        //              )
        //              =
        //              (
        //                  SELECT COUNT(*)
        //                  FROM {Tbl.detection} sd
        //                  WHERE sd.event_assign_id = d.event_assign_id
        //                    AND sd.sub_strip_event_id IS NOT NULL
        //                    AND sd.is_active = 1
        //                    AND sd.detection_status IN ('submit','confirm-submit')
        //              )
        //            GROUP BY d.event_assign_id
        //            ORDER BY {shortColumn} {orderBy}
        //            OFFSET @offSet ROWS
        //            FETCH NEXT @pageSize ROWS ONLY;";
        //}

        #endregion

    }
    public class PriorityCompleteReportDTO
    {
        public long detection_id { get; set; }
        public long? sub_strip_event_id { get; set; }
        public long queued_events_records_id { get; set; }
        public DateTime? event_start_datetime { get; set; }
        public DateTime? event_end_datetime { get; set; }
        public long event_assign_id { get; set; }
        public string event_file_path { get; set; }
        public string detection_status { get; set; }
        public string remarks { get; set; }
        public long created_by { get; set; }
        public DateTime? created_datetime { get; set; }
        public long? modified_by { get; set; }
        public DateTime? modified_datetime { get; set; }
        public long? picked_by { get; set; }
        public DateTime? picked_datetime { get; set; }

        public List<SubStripEventResDTO> sub_strip_files { get; set; }

        public long patient_id { get; set; }
        public string patient_name { get; set; }

        public List<FindingDTO> finding_list { get; set; }

    }
    public class ActivityLogReqDTO
    {
        public long activity_log_id { get; set; }
        public long queued_events_records_id { get; set; }
        public long? sub_strip_event_id { get; set; }
        public long detection_id { get; set; }
        public string action_type { get; set; }
        public List<ActivityLogFindingDTO> finding_list { get; set; }
        public long created_by { get; set; }
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public int offSet => (pageNumber - 1) * pageSize;
        public string sortBy { get; set; }
        public string orderBy { get; set; }



    }
    public class ActivityLogResDTO
    {
        public long activity_log_id { get; set; }
        public long created_by { get; set; }
        public DateTime? created_datetime { get; set; }
        public long? modified_by { get; set; }
        public DateTime? modified_datetime { get; set; }
        public bool is_active { get; set; }

        public long queued_events_records_id { get; set; }
        public long detection_id { get; set; }
        public long? sub_strip_event_id { get; set; }
        public string action_type { get; set; }
        public List<ActivityLogFindingDTO> finding_list { get; set; }
        public long user_id { get; set; }
        public string user_name { get; set; }

        #region Query 
        internal static string select_activity_log(ActivityLogReqDTO obj)
        {
            return $@"
                    SELECT COUNT(*)
                    FROM {Tbl.activity_log} al
                    LEFT JOIN {Tbl.user_master} u
                        ON u.user_id = al.created_by
                    LEFT JOIN {Tbl.detection} d
                        ON d.detection_id = al.detection_id
                    WHERE al.is_active = 1
                          AND u.is_active = 1
                          AND al.queued_events_records_id = @queued_events_records_id
                            AND
                            (
                                (
                                    ISNULL(@sub_strip_event_id, 0) = 0
                                    AND d.sub_strip_event_id IS NULL
                                )
                                OR
                                (
                                    ISNULL(@sub_strip_event_id, 0) <> 0
                                    AND d.sub_strip_event_id = @sub_strip_event_id
                                )
                            );


                    SELECT
                        al.activity_log_id,
                        al.created_by,
                        al.created_datetime,
                        al.modified_by,
                        al.modified_datetime,
                        al.is_active,
                        al.queued_events_records_id,
                        al.detection_id,
                        al.action_type,
                        u.user_id,
                        u.user_name,
                        d.sub_strip_event_id

                    FROM {Tbl.activity_log} al
                    LEFT JOIN {Tbl.user_master} u
                        ON u.user_id = al.created_by
                    LEFT JOIN {Tbl.detection} d
                        ON d.detection_id = al.detection_id

                    WHERE
                        al.is_active = 1
                        AND u.is_active = 1

                        AND al.queued_events_records_id = @queued_events_records_id

                        AND
                        (
                            (
                                ISNULL(@sub_strip_event_id, 0) = 0
                                AND d.sub_strip_event_id IS NULL
                            )
                            OR
                            (
                                ISNULL(@sub_strip_event_id, 0) <> 0
                                AND d.sub_strip_event_id = @sub_strip_event_id
                            )
                        )
                    ORDER BY {obj.sortBy} {obj.orderBy}
                    OFFSET @offSet ROWS
                    FETCH NEXT @pageSize ROWS ONLY;
                ";
        }
        internal static string select_activity_log_finding()
        {
            return $@"
                    SELECT
                        alf.activity_log_finding_id,
                        alf.event_finding_id,
                        efm.finding_name,
                        alf.value1,
                        alf.value2,
                        alf.finding_status,
                        alf.created_by,
                        alf.created_datetime,
                        alf.modified_by,
                        alf.modified_datetime,
                        alf.is_active

                    FROM {Tbl.activity_log_finding} alf

                    INNER JOIN {Tbl.event_finding_mast} efm
                        ON efm.event_finding_id = alf.event_finding_id

                    WHERE alf.activity_log_id = @activity_log_id
                      AND alf.is_active = 1

                    ORDER BY alf.activity_log_finding_id ASC;
                    ";
        }


        internal static string insert_activity_log()
        {
            return $@"insert into {Tbl.activity_log} (queued_events_records_id,created_by,detection_id,action_type)
                      values (@queued_events_records_id , @created_by, @detection_id, @action_type)";
        }
        internal static string insert_activity_log_finding()
        {
            return $@"insert into {Tbl.activity_log_finding} (activity_log_id,event_finding_id,value1,value2,created_by,finding_status)
                      values (@activity_log_id ,@event_finding_id ,@value1 ,@value2, @created_by, @finding_status)";
        }
        #endregion

    }
    public class ActivityLogFindingDTO
    {
        public long activity_log_finding_id { get; set; }
        public long event_finding_id { get; set; }
        public string finding_name { get; set; }
        public decimal? value1 { get; set; }
        public decimal? value2 { get; set; }
        public string finding_status { get; set; }
    }
    public class FectchHistoryReqDTO
    {
        public long queued_events_records_id { get; set; }
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public int offSet => (pageNumber - 1) * pageSize;
        public string sortBy { get; set; }
        public string orderBy { get; set; }
        public string event_status { get; set; }
        public string patient_name { get; set; }
        public DateTime? from_date { get; set; }
        public DateTime? to_date { get; set; }
        public bool is_download { get; set; }

    }

    public class FectchHistoryRespDTO
    {
        public long queued_events_records_id { get; set; }
        public long patient_enrollment_id { get; set; }
        public long enrollment_raw_data_id { get; set; }
        public long parent_queued_events_records_id { get; set; }
        public string priority_level { get; set; }
        public string trigger_type { get; set; }
        public int bpm { get; set; }
        public string event_status { get; set; }
        public string event_file_path { get; set; }
        public long created_by { get; set; }
        public long modified_by { get; set; }
        public DateTime? created_datetime { get; set; }
        public DateTime? modified_datetime { get; set; }
        public DateTime? event_start_datetime { get; set; }
        public DateTime? event_end_datetime { get; set; }
        public string event_source { get; set; }
        public string fetch_duration_minutes { get; set; }
        public long completed_by { get; set; }
        public string completed_user_name { get; set; }
        public DateTime? completed_datetime { get; set; }
        public string remarks { get; set; }
        public long user_id { get; set; }
        public string user_name { get; set; }
        public long patient_id { get; set; }
        public string patient_name { get; set; }
        public bool is_active { get; set; }

        public List<FindingDTO>? finding_list { get; set; }

        public List<SymptomDTO> symptoms { get; set; }

        public long device_id { get; set; }
        public string device_no { get; set; }
        public string device_name { get; set; }

        public long event_assign_id { get; set; }

        #region Query 
        internal static string get_fetch_history_by_id()
        {
            return $@"
                    SELECT DISTINCT 
                        a.queued_events_records_id,
                        a.patient_enrollment_id,
                        a.enrollment_raw_data_id,
                        a.priority_level,
                        a.trigger_type, 
                        a.bpm,
                        a.event_status,
                        a.event_file_path,
                        a.created_by,
                        a.modified_by,
                        a.created_datetime,
                        a.modified_datetime,

                        c.patient_id,
                        d.patient_name,

                        da.device_id,
                        dm.device_no,
                        dm.device_name,
                        ea.event_assign_id,
                        a.is_active

                    FROM {Tbl.queued_events_records} AS a

                    LEFT JOIN {Tbl.patient_enrollment} AS c
                        ON c.patient_enrollment_id = a.patient_enrollment_id

                    LEFT JOIN {Tbl.patient} AS d
                        ON d.patient_id = c.patient_id

                    LEFT JOIN {Tbl.device_assignment} AS da
                        ON da.patient_enrollment_id = a.patient_enrollment_id
                        AND da.is_active = 1

                    LEFT JOIN {Tbl.device_mast} AS dm
                        ON dm.device_id = da.device_id
                        AND dm.is_active = 1

                      left join {Tbl.event_assign} as ea
                        on ea.queued_events_records_id = a.queued_events_records_id 

                    WHERE a.queued_events_records_id = @queued_events_records_id
                      AND a.is_active = 1
                      AND d.is_active = 1
                    and ea.is_final_completed = 1
                    and ea.work_status = 'completed' 
                    and ea.is_active = 1
                ";
        }
        internal static string select_fetch_history_list(FectchHistoryReqDTO obj)
        {
            return $@"
                    SELECT COUNT(*)
                    FROM
                    (
                        SELECT DISTINCT
                            a.queued_events_records_id
                        FROM {Tbl.queued_events_records} AS a

                        LEFT JOIN {Tbl.patient_enrollment} AS c
                            ON c.patient_enrollment_id = a.patient_enrollment_id

                        INNER JOIN {Tbl.patient} AS d
                            ON d.patient_id = c.patient_id

                        WHERE a.trigger_type = 'Fetch-ECG'
                          AND a.is_active = 1

                          AND
                          (
                              a.parent_queued_events_records_id =
                              (
                                  SELECT
                                      CASE
                                          WHEN parent_queued_events_records_id > 0
                                              THEN parent_queued_events_records_id
                                          ELSE queued_events_records_id
                                      END
                                  FROM {Tbl.queued_events_records}
                                  WHERE queued_events_records_id = @queued_events_records_id
                                    AND is_active = 1
                              )
                              OR @queued_events_records_id = 0
                          )

                          AND (@event_status IS NULL
                               OR @event_status = ''
                               OR a.event_status = @event_status)

                          AND (@from_date IS NULL
                               OR a.created_datetime >= @from_date)

                          AND (@to_date IS NULL
                               OR a.created_datetime < DATEADD(DAY, 1, @to_date))

                          AND (@patient_name IS NULL
                               OR @patient_name = ''
                               OR d.patient_name LIKE '%' + @patient_name + '%')
                    ) AS TotalData;


                    SELECT DISTINCT
                        a.queued_events_records_id,
                        a.is_active,
                        a.parent_queued_events_records_id,
                        a.trigger_type,
                        a.event_start_datetime,
                        a.event_end_datetime,
                        a.event_status,
                        a.created_by,
                        a.created_datetime,
                        a.event_source,
                        a.fetch_duration_minutes,
                        a.completed_by,
                        a.completed_datetime,
                        a.remarks,

                        b.user_id,
                        b.user_name,

                        c.patient_id,
                        d.patient_name,

                        e.user_name AS completed_user_name

                    FROM {Tbl.queued_events_records} AS a

                    LEFT JOIN {Tbl.user_master} AS b
                        ON b.user_id = a.created_by

                    LEFT JOIN {Tbl.user_master} AS e
                        ON e.user_id = a.completed_by

                    LEFT JOIN {Tbl.patient_enrollment} AS c
                        ON c.patient_enrollment_id = a.patient_enrollment_id

                    INNER JOIN {Tbl.patient} AS d
                        ON d.patient_id = c.patient_id

                    WHERE a.trigger_type = 'Fetch-ECG'
                      AND a.is_active = 1

                      AND
                      (
                          a.parent_queued_events_records_id =
                          (
                              SELECT
                                  CASE
                                      WHEN parent_queued_events_records_id > 0
                                          THEN parent_queued_events_records_id
                                      ELSE queued_events_records_id
                                  END
                              FROM {Tbl.queued_events_records}
                              WHERE queued_events_records_id = @queued_events_records_id
                                AND is_active = 1
                          )
                          OR @queued_events_records_id = 0
                      )

                      AND (@event_status IS NULL
                           OR @event_status = ''
                           OR a.event_status = @event_status)

                      AND (@patient_name IS NULL
                           OR @patient_name = ''
                           OR d.patient_name LIKE '%' + @patient_name + '%')

                      AND (@from_date IS NULL
                           OR a.created_datetime >= @from_date)

                      AND (@to_date IS NULL
                           OR a.created_datetime < DATEADD(DAY, 1, @to_date))

                    ORDER BY {obj.sortBy} {obj.orderBy}

                    OFFSET @offSet ROWS
                    FETCH NEXT {obj.pageSize} ROWS ONLY;
                ";
        }
        //internal static string select_fetch_history_list(FectchHistoryReqDTO obj)
        //{
        //    return $@"
        //            SELECT COUNT(*)
        //            FROM {Tbl.queued_events_records} AS a

        //                    LEFT JOIN {Tbl.patient_enrollment} AS c
        //                        ON c.patient_enrollment_id = a.patient_enrollment_id

        //                    INNER JOIN {Tbl.patient} AS d
        //                        ON d.patient_id = c.patient_id

        //            WHERE a.trigger_type = 'Fetch-ECG'
        //              AND a.is_active = 1

        //              AND
        //              (
        //                  a.parent_queued_events_records_id =
        //                  (
        //                      SELECT
        //                          CASE
        //                              WHEN parent_queued_events_records_id > 0
        //                                  THEN parent_queued_events_records_id
        //                              ELSE queued_events_records_id
        //                          END
        //                      FROM {Tbl.queued_events_records}
        //                      WHERE queued_events_records_id = @queued_events_records_id
        //                        AND is_active = 1
        //                  )
        //                  OR @queued_events_records_id = 0
        //              )

        //              AND (@event_status IS NULL OR @event_status = '' OR a.event_status = @event_status)

        //              AND (@from_date IS NULL OR a.created_datetime >= @from_date)

        //              AND (@to_date IS NULL OR a.created_datetime  < DATEADD(DAY, 1, @to_date))

        //              AND (@patient_name IS NULL OR @patient_name = '' OR d.patient_name LIKE '%' + @patient_name + '%');


        //                    SELECT
        //                        a.queued_events_records_id,
        //                        a.is_active,
        //                        a.parent_queued_events_records_id,
        //                        a.trigger_type,
        //                        a.event_start_datetime,
        //                        a.event_end_datetime,
        //                        a.event_status,
        //                        a.created_by,
        //                        a.created_datetime,
        //                        a.event_source,
        //                        a.fetch_duration_minutes,
        //                        a.completed_by,
        //                        a.completed_datetime,
        //                        a.remarks,

        //                        b.user_id,
        //                        b.user_name,

        //                        c.patient_id,
        //                        d.patient_name,

        //                        e.user_name AS completed_user_name

        //                    FROM {Tbl.queued_events_records} AS a

        //                    LEFT JOIN {Tbl.user_master} AS b
        //                        ON b.user_id = a.created_by

        //                    LEFT JOIN {Tbl.user_master} AS e
        //                        ON a.completed_by = e.user_id

        //                    LEFT JOIN {Tbl.patient_enrollment} AS c
        //                        ON c.patient_enrollment_id = a.patient_enrollment_id

        //                    INNER JOIN {Tbl.patient} AS d
        //                        ON d.patient_id = c.patient_id

        //                    WHERE a.trigger_type = 'Fetch-ECG'
        //                      AND a.is_active = 1

        //                      AND
        //                      (
        //                          a.parent_queued_events_records_id =
        //                          (
        //                              SELECT
        //                                  CASE
        //                                      WHEN parent_queued_events_records_id > 0
        //                                          THEN parent_queued_events_records_id
        //                                      ELSE queued_events_records_id
        //                                  END
        //                              FROM {Tbl.queued_events_records}
        //                              WHERE queued_events_records_id = @queued_events_records_id
        //                                AND is_active = 1
        //                          )
        //                          OR @queued_events_records_id = 0
        //                      )

        //                      AND (@event_status IS NULL OR @event_status = '' OR a.event_status = @event_status)

        //                      AND (@patient_name IS NULL OR @patient_name = '' OR d.patient_name LIKE '%' + @patient_name + '%')

        //                      AND (@from_date IS NULL OR a.created_datetime >= @from_date)

        //                      AND (@to_date IS NULL OR a.created_datetime  < DATEADD(DAY, 1, @to_date))

        //                    ORDER BY {obj.sortBy} {obj.orderBy}

        //                    OFFSET @offSet ROWS
        //                    FETCH NEXT {obj.pageSize} ROWS ONLY";
        //}
        internal static string select_fetch_history_excel(FectchHistoryReqDTO obj)
        {
            return $@"
                    SELECT
                        a.queued_events_records_id,
                        a.parent_queued_events_records_id,
                        a.trigger_type,
                        a.event_start_datetime,
                        a.event_end_datetime,
                        a.event_status,
                        a.created_by,
                        a.created_datetime,
                        a.event_source,
                        a.fetch_duration_minutes,
                        a.completed_by,
                        a.completed_datetime,
                        a.remarks,

                        b.user_id,
                        b.user_name,

                        c.patient_id,
                        d.patient_name,

                        e.user_name AS completed_user_name

                    FROM {Tbl.queued_events_records} AS a

                    LEFT JOIN {Tbl.user_master} AS b
                        ON b.user_id = a.created_by

                    LEFT JOIN {Tbl.user_master} AS e
                        ON e.user_id = a.completed_by

                    LEFT JOIN {Tbl.patient_enrollment} AS c
                        ON c.patient_enrollment_id = a.patient_enrollment_id

                    INNER JOIN {Tbl.patient} AS d
                        ON d.patient_id = c.patient_id

                    WHERE a.trigger_type = 'Fetch-ECG'
                      AND a.is_active = 1

                      AND
                      (
                          a.parent_queued_events_records_id =
                          (
                              SELECT
                                  CASE
                                      WHEN parent_queued_events_records_id > 0
                                          THEN parent_queued_events_records_id
                                      ELSE queued_events_records_id
                                  END
                              FROM {Tbl.queued_events_records}
                              WHERE queued_events_records_id = @queued_events_records_id
                                AND is_active = 1
                          )
                          OR @queued_events_records_id = 0
                      )

                      AND (
                          @event_status IS NULL
                          OR @event_status = ''
                          OR a.event_status = @event_status
                      )

                      AND (
                          @patient_name IS NULL
                          OR @patient_name = ''
                          OR d.patient_name LIKE '%' + @patient_name + '%'
                      )

                      AND (
                          @from_date IS NULL
                          OR a.created_datetime >= @from_date
                      )

                      AND (
                          @to_date IS NULL
                          OR a.created_datetime < DATEADD(DAY, 1, @to_date)
                      )

                    ORDER BY {obj.sortBy} {obj.orderBy}";
        }
        #endregion
    }

    public class FetchECGReqDTO
    {
        public long queued_events_records_id { get; set; }
        public int minutes { get; set; }
        public string fetch_direction { get; set; }
        public long created_by { get; set; }
        public DateTime? created_datetime { get; set; }

        #region Query
        internal static string get_current_event()
        {
            return $@"
            SELECT
                patient_enrollment_id,
                enrollment_raw_data_id,
                priority_level,
                trigger_type,
                bpm,
                event_start_datetime,
                event_end_datetime,
                event_file_path
            FROM {Tbl.queued_events_records}
            WHERE queued_events_records_id=@queued_events_records_id
            AND is_active=1";
        }

        internal static string get_baseline_files()
        {
            return $@"
            SELECT
                patient_base_line_id,
                event_processed_path,
                record_st,
                record_end
            FROM {Tbl.patient_base_line}
            WHERE patient_enrollment_id=@patient_enrollment_id
              AND record_end>@fromTime
              AND record_st<@toTime
              AND is_active=1
            ORDER BY record_st";
        }

        internal static string check_fetch_exists()
        {
            return $@"
            SELECT queued_events_records_id
            FROM {Tbl.queued_events_records}
            WHERE parent_queued_events_records_id=@parent_queued_events_records_id
            AND event_source=@event_source
            AND fetch_duration_minutes=@fetch_duration_minutes
            AND is_active=1";
        }

        internal static string insert_fetch_event()
        {
            return $@"insert into {Tbl.queued_events_records}
                      ( patient_enrollment_id, enrollment_raw_data_id, priority_level, trigger_type, bpm, event_start_datetime, event_end_datetime,
                                               event_status, event_file_path, parent_queued_events_records_id, event_source,fetch_duration_minutes, created_by,created_datetime)
                        values(@patient_enrollment_id, @enrollment_raw_data_id, @priority_level, @trigger_type, @bpm, @event_start_datetime, @event_end_datetime,
                                                @event_status, @event_file_path, @parent_queued_events_records_id, @event_source, @fetch_duration_minutes, @created_by, @created_datetime);";
        }


        #endregion
    }
    public class FetchECGResDTO
    {
        public long queued_events_records_id { get; set; }
        public DateTime event_start_datetime { get; set; }
        public DateTime event_end_datetime { get; set; }
        public DateTime from_datetime { get; set; }
        public DateTime to_datetime { get; set; }
        public string merged_file_path { get; set; }

        public List<PatientBaseLineDTO> baseline_files { get; set; }
    }
    public class PatientBaseLineDTO
    {
        public long patient_base_line_id { get; set; }
        public string event_processed_path { get; set; }
        public DateTime record_st { get; set; }
        public DateTime record_end { get; set; }
    }
    public class EcgJsonDTO
    {
        public string type { get; set; }
        public string timestamp { get; set; }
        public string end_timestamp { get; set; }
        public string severity { get; set; }
        public double confidence { get; set; }
        public double movement_g { get; set; }
        public WaveformSnippet waveform_snippet { get; set; }
    }

    public class WaveformSnippet
    {
        public List<long> timestamp_ms { get; set; }
        public List<double> voltage_mv { get; set; }
        public List<long> beat_positions { get; set; }
        public List<string> beat_labels { get; set; }
    }




    public class MyProgressDTO
    {
        public int EventsSubmitted { get; set; }

        public decimal Productivity { get; set; }

        //public string ProductivityUnit { get; set; }

        public List<MyProgressChartDTO> Chart { get; set; }

        #region Query
        //internal static string GetProgressSummary(string type)
        //{
        //    return $@"
        //        SELECT
        //            COUNT(*) EventsSubmitted,
        //            {GetProductivity(type)} Productivity
        //        FROM {Tbl.queued_events_records} q
        //        INNER JOIN {Tbl.event_assign} ea
        //            ON ea.queued_events_records_id=q.queued_events_records_id
        //        WHERE
        //            q.completed_by=@assigned_by
        //            AND q.completed_datetime IS NOT NULL
        //            AND ea.is_final_completed=1
        //            AND {GetDateFilter(type)}";
        //}



        internal static string GetProgressSummary(string type)
        {
            return $@"
        SELECT
            COUNT(DISTINCT q.queued_events_records_id) EventsSubmitted,
            {GetProductivity(type)} Productivity
        FROM {Tbl.queued_events_records} q
        WHERE
            q.completed_by = @assigned_by
            AND q.completed_datetime IS NOT NULL
            AND EXISTS
            (
                SELECT 1
                FROM {Tbl.event_assign} ea
                WHERE ea.queued_events_records_id = q.queued_events_records_id
                  AND ea.is_final_completed = 1
            )
            AND {GetDateFilter(type)}";
        }


        //internal static string GetDateFilter(string type)
        //{
        //    switch (type.ToLower())
        //    {
        //        default:
        //            return @"
        //        CAST(q.completed_datetime AS DATE) = CAST(GETDATE() AS DATE)";

        //        case "week":
        //            return @"
        //        q.completed_datetime >= DATEADD(DAY, -(DATEPART(WEEKDAY, GETDATE()) + 5) % 7, CAST(GETDATE() AS DATE))
        //        AND q.completed_datetime < DATEADD(DAY, 7 - ((DATEPART(WEEKDAY, GETDATE()) + 5) % 7), CAST(GETDATE() AS DATE))";

        //        case "month":
        //            return @"
        //        q.completed_datetime >= DATEADD(WEEK, -3,DATEADD(DAY, -(DATEPART(WEEKDAY, GETDATE()) + 5) % 7, CAST(GETDATE() AS DATE)))
        //        AND q.completed_datetime < DATEADD(DAY, 7 - ((DATEPART(WEEKDAY, GETDATE()) + 5) % 7), CAST(GETDATE() AS DATE))";
        //    }
        //}



        internal static string GetDateFilter(string type)
        {
            switch (type.ToLower())
            {
                default:
                    return @"
                    CAST(q.completed_datetime AS DATE)
                    = CAST(GETDATE() AS DATE)";

                case "week":
                    return @"
                    q.completed_datetime >=
                    DATEADD(
                        DAY,
                        -(DATEPART(WEEKDAY, GETDATE()) + 5) % 7,
                        CAST(GETDATE() AS DATE)
                    )
                    AND q.completed_datetime <
                    DATEADD(
                        DAY,
                        7 - ((DATEPART(WEEKDAY, GETDATE()) + 5) % 7),
                        CAST(GETDATE() AS DATE)
                    )";

                case "month":
                    return @"
                    q.completed_datetime >=
                    DATEADD(
                        WEEK,
                        -3,
                        DATEADD(
                            DAY,
                            -(DATEPART(WEEKDAY, GETDATE()) + 5) % 7,
                            CAST(GETDATE() AS DATE)
                        )
                    )
                    AND q.completed_datetime <
                    DATEADD(
                        DAY,
                        7 - ((DATEPART(WEEKDAY, GETDATE()) + 5) % 7),
                        CAST(GETDATE() AS DATE)
                    )";
            }
        }


        //internal static string GetProductivity(string type)
        //{
        //    switch (type.ToLower())
        //    {
        //        case "week":
        //            return "CAST(COUNT(*)/7.0 AS DECIMAL(10,2))";

        //        case "month":
        //            return "CAST(COUNT(*)/28.0 AS DECIMAL(10,2))";

        //        default:
        //            return "CAST(COUNT(*) AS DECIMAL(10,2))";
        //    }
        //}


        //internal static string GetProductivity(string type)
        //{
        //    switch (type.ToLower())
        //    {
        //        case "week":
        //            return "CAST(COUNT(*) / 7.0 AS DECIMAL(10,2))";

        //        case "month":
        //            return "CAST(COUNT(*) / 28.0 AS DECIMAL(10,2))";

        //        default:
        //            return "CAST(COUNT(*) AS DECIMAL(10,2))";
        //    }
        //}

        internal static string GetProductivity(string type)
        {
            switch (type.ToLower())
            {
                case "week":
                    return "CAST(COUNT(DISTINCT q.queued_events_records_id) / 7.0 AS DECIMAL(10,2))";

                case "month":
                    return "CAST(COUNT(DISTINCT q.queued_events_records_id) / 28.0 AS DECIMAL(10,2))";

                default:
                    return "CAST(COUNT(DISTINCT q.queued_events_records_id) AS DECIMAL(10,2))";
            }
        }



        internal static string GetProgressChart(string type)
        {
            switch (type.ToLower())
            {
                // =========================
                // WEEK
                // =========================
                case "week":

                    return $@"
                WITH Days AS
                (
                    SELECT 1 DayNo, 'Monday' Label
                    UNION ALL SELECT 2, 'Tuesday'
                    UNION ALL SELECT 3, 'Wednesday'
                    UNION ALL SELECT 4, 'Thursday'
                    UNION ALL SELECT 5, 'Friday'
                    UNION ALL SELECT 6, 'Saturday'
                    UNION ALL SELECT 7, 'Sunday'
                )

                SELECT
                    d.Label,
                    ISNULL(COUNT(DISTINCT q.queued_events_records_id), 0) Events

                FROM Days d

                LEFT JOIN {Tbl.queued_events_records} q
                    ON
                    (
                        CASE DATEPART(WEEKDAY, q.completed_datetime)
                            WHEN 2 THEN 1
                            WHEN 3 THEN 2
                            WHEN 4 THEN 3
                            WHEN 5 THEN 4
                            WHEN 6 THEN 5
                            WHEN 7 THEN 6
                            WHEN 1 THEN 7
                        END
                    ) = d.DayNo

                    AND q.completed_by = @assigned_by
                    AND q.completed_datetime IS NOT NULL
                    AND {GetDateFilter(type)}

                    AND EXISTS
                    (
                        SELECT 1
                        FROM {Tbl.event_assign} ea
                        WHERE ea.queued_events_records_id =
                              q.queued_events_records_id
                          AND ea.is_final_completed = 1
                    )

                GROUP BY
                    d.DayNo,
                    d.Label

                ORDER BY
                    d.DayNo";


                // =========================
                // MONTH
                // =========================
                case "month":

                    return $@"
                WITH Weeks AS
                (
                    SELECT 1 WeekNo, 'Week 1' Label
                    UNION ALL SELECT 2, 'Week 2'
                    UNION ALL SELECT 3, 'Week 3'
                    UNION ALL SELECT 4, 'Week 4'
                ),

                StartDate AS
                (
                    SELECT
                        DATEADD
                        (
                            WEEK,
                            -3,
                            DATEADD
                            (
                                DAY,
                                -(DATEPART(WEEKDAY, GETDATE()) + 5) % 7,
                                CAST(GETDATE() AS DATE)
                            )
                        ) AS StartDate
                )

                SELECT
                    w.Label,
                    ISNULL(COUNT(DISTINCT q.queued_events_records_id), 0) Events

                FROM Weeks w

                CROSS JOIN StartDate s

                LEFT JOIN {Tbl.queued_events_records} q
                    ON q.completed_by = @assigned_by
                    AND q.completed_datetime IS NOT NULL

                    AND q.completed_datetime >= s.StartDate

                    AND q.completed_datetime <
                        DATEADD(DAY, 28, s.StartDate)

                    AND DATEDIFF
                    (
                        DAY,
                        s.StartDate,
                        CAST(q.completed_datetime AS DATE)
                    ) / 7 = w.WeekNo - 1

                    AND EXISTS
                    (
                        SELECT 1
                        FROM {Tbl.event_assign} ea
                        WHERE ea.queued_events_records_id =
                              q.queued_events_records_id
                          AND ea.is_final_completed = 1
                    )

                GROUP BY
                    w.WeekNo,
                    w.Label

                ORDER BY
                    w.WeekNo";


                // =========================
                // DAY
                // =========================
                default:

                    return $@"
                WITH Hours AS
                (
                    SELECT 0 Hr

                    UNION ALL

                    SELECT Hr + 1
                    FROM Hours
                    WHERE Hr < 23
                )

                SELECT
                    RIGHT('00' + CAST(h.Hr AS VARCHAR), 2) + ':00' Label,
                    ISNULL(COUNT(DISTINCT q.queued_events_records_id), 0) Events

                FROM Hours h

                LEFT JOIN {Tbl.queued_events_records} q
                    ON DATEPART(HOUR, q.completed_datetime) = h.Hr

                    AND q.completed_by = @assigned_by
                    AND q.completed_datetime IS NOT NULL
                    AND {GetDateFilter(type)}

                    AND EXISTS
                    (
                        SELECT 1
                        FROM {Tbl.event_assign} ea
                        WHERE ea.queued_events_records_id =
                              q.queued_events_records_id
                          AND ea.is_final_completed = 1
                    )

                GROUP BY
                    h.Hr

                ORDER BY
                    h.Hr

                OPTION(MAXRECURSION 24)";
            }
        }


        //internal static string GetProgressChart(string type)
        //{
        //    switch (type.ToLower())
        //    {
        //        case "week":

        //            return $@"
        //        WITH Days AS
        //        (
        //            SELECT 1 DayNo,'Monday' Label
        //            UNION ALL SELECT 2,'Tuesday'
        //            UNION ALL SELECT 3,'Wednesday'
        //            UNION ALL SELECT 4,'Thursday'
        //            UNION ALL SELECT 5,'Friday'
        //            UNION ALL SELECT 6,'Saturday'
        //            UNION ALL SELECT 7,'Sunday'
        //        )

        //        SELECT
        //            d.Label,
        //            ISNULL(COUNT(q.queued_events_records_id),0) Events
        //        FROM Days d

        //        LEFT JOIN {Tbl.queued_events_records} q
        //            ON
        //            (
        //                CASE DATEPART(WEEKDAY,q.completed_datetime)
        //                    WHEN 2 THEN 1
        //                    WHEN 3 THEN 2
        //                    WHEN 4 THEN 3
        //                    WHEN 5 THEN 4
        //                    WHEN 6 THEN 5
        //                    WHEN 7 THEN 6
        //                    WHEN 1 THEN 7
        //                END
        //            )=d.DayNo
        //            AND q.completed_by=@assigned_by
        //            AND q.completed_datetime IS NOT NULL
        //            AND {GetDateFilter(type)}

        //        LEFT JOIN {Tbl.event_assign} ea
        //            ON ea.queued_events_records_id=q.queued_events_records_id
        //            AND ea.is_final_completed=1

        //        GROUP BY d.DayNo,d.Label
        //        ORDER BY d.DayNo";

        //        case "month":

        //            return $@"
        //        WITH Weeks AS
        //        (
        //            SELECT 1 WeekNo,'Week 1' Label
        //            UNION ALL SELECT 2,'Week 2'
        //            UNION ALL SELECT 3,'Week 3'
        //            UNION ALL SELECT 4,'Week 4'
        //        )

        //        SELECT
        //            w.Label,
        //            ISNULL(COUNT(q.queued_events_records_id),0) Events
        //        FROM Weeks w

        //        LEFT JOIN {Tbl.queued_events_records} q
        //            ON
        //            (
        //                CASE DATEDIFF
        //                (
        //                    WEEK,
        //                    q.completed_datetime,
        //                    DATEADD
        //                    (
        //                        DAY,
        //                        -(DATEPART(WEEKDAY, GETDATE()) + 5) % 7,
        //                        CAST(GETDATE() AS DATE)
        //                    )
        //                )
        //                    WHEN 0 THEN 1
        //                    WHEN 1 THEN 2
        //                    WHEN 2 THEN 3
        //                    WHEN 3 THEN 4
        //                END
        //            ) = w.WeekNo
        //            AND q.completed_by = @assigned_by
        //            AND q.completed_datetime IS NOT NULL
        //            AND {GetDateFilter(type)}

        //        LEFT JOIN {Tbl.event_assign} ea
        //            ON ea.queued_events_records_id=q.queued_events_records_id
        //            AND ea.is_final_completed=1

        //        GROUP BY w.WeekNo,w.Label
        //        ORDER BY w.WeekNo";

        //        default:

        //            return $@"
        //        WITH Hours AS
        //        (
        //            SELECT 0 Hr
        //            UNION ALL
        //            SELECT Hr+1
        //            FROM Hours
        //            WHERE Hr<23
        //        )

        //        SELECT
        //            RIGHT('00'+CAST(h.Hr AS VARCHAR),2)+':00' Label,
        //            ISNULL(COUNT(q.queued_events_records_id),0) Events
        //        FROM Hours h

        //        LEFT JOIN {Tbl.queued_events_records} q
        //            ON DATEPART(HOUR,q.completed_datetime)=h.Hr
        //            AND q.completed_by=@assigned_by
        //            AND q.completed_datetime IS NOT NULL
        //            AND {GetDateFilter(type)}

        //        LEFT JOIN {Tbl.event_assign} ea
        //            ON ea.queued_events_records_id=q.queued_events_records_id
        //            AND ea.is_final_completed=1

        //        GROUP BY h.Hr
        //        ORDER BY h.Hr
        //        OPTION(MAXRECURSION 24)";
        //    }
        //}


        internal static string GetReportProgressSummary(string type)
        {
            return $@"
        SELECT
            COUNT(*) EventsSubmitted,
            {GetReportProductivity(type)} Productivity
        FROM {Tbl.report_assigned} ra
        WHERE
            ra.completed_by = @assigned_by
            AND ra.completed_datetime IS NOT NULL
            AND ra.assign_status = 'completed'
            AND ra.is_active = 1
            AND {GetReportDateFilter(type)}";
        }

        internal static string GetReportDateFilter(string type)
        {
            switch (type.ToLower())
            {
                default:
                    return @"
                CAST(ra.completed_datetime AS DATE)
                = CAST(GETDATE() AS DATE)";

                case "week":
                    return @"
                ra.completed_datetime >=
                DATEADD(
                    DAY,
                    -(DATEPART(WEEKDAY, GETDATE()) + 5) % 7,
                    CAST(GETDATE() AS DATE)
                )
                AND ra.completed_datetime <
                DATEADD(
                    DAY,
                    7 - ((DATEPART(WEEKDAY, GETDATE()) + 5) % 7),
                    CAST(GETDATE() AS DATE)
                )";

                case "month":
                    return @"
                ra.completed_datetime >=
                DATEADD(
                    WEEK,
                    -3,
                    DATEADD(
                        DAY,
                        -(DATEPART(WEEKDAY, GETDATE()) + 5) % 7,
                        CAST(GETDATE() AS DATE)
                    )
                )
                AND ra.completed_datetime <
                DATEADD(
                    DAY,
                    7 - ((DATEPART(WEEKDAY, GETDATE()) + 5) % 7),
                    CAST(GETDATE() AS DATE)
                )";
            }
        }

        internal static string GetReportProductivity(string type)
        {
            switch (type.ToLower())
            {
                case "week":
                    return "CAST(COUNT(*) / 7.0 AS DECIMAL(10,2))";

                case "month":
                    return "CAST(COUNT(*) / 28.0 AS DECIMAL(10,2))";

                default:
                    return "CAST(COUNT(*) AS DECIMAL(10,2))";
            }
        }

        internal static string GetReportProgressChart(string type)
        {
            switch (type.ToLower())
            {
                // =========================
                // WEEK
                // =========================
                case "week":

                    return $@"
                WITH Days AS
                (
                    SELECT 1 DayNo,'Monday' Label
                    UNION ALL SELECT 2,'Tuesday'
                    UNION ALL SELECT 3,'Wednesday'
                    UNION ALL SELECT 4,'Thursday'
                    UNION ALL SELECT 5,'Friday'
                    UNION ALL SELECT 6,'Saturday'
                    UNION ALL SELECT 7,'Sunday'
                )

                SELECT
                    d.Label,
                    ISNULL(COUNT(ra.report_assigned_id),0) Events
                FROM Days d

                LEFT JOIN {Tbl.report_assigned} ra
                    ON
                    (
                        CASE DATEPART(WEEKDAY,ra.completed_datetime)
                            WHEN 2 THEN 1
                            WHEN 3 THEN 2
                            WHEN 4 THEN 3
                            WHEN 5 THEN 4
                            WHEN 6 THEN 5
                            WHEN 7 THEN 6
                            WHEN 1 THEN 7
                        END
                    ) = d.DayNo

                    AND ra.completed_by=@assigned_by
                    AND ra.completed_datetime IS NOT NULL
                    AND ra.assign_status='completed'
                    AND ra.is_active=1
                    AND {GetReportDateFilter(type)}

                GROUP BY
                    d.DayNo,
                    d.Label

                ORDER BY
                    d.DayNo";

                // =========================
                // MONTH
                // =========================
                case "month":

                    return $@"
                WITH Weeks AS
                (
                    SELECT 1 WeekNo,'Week 1' Label
                    UNION ALL SELECT 2,'Week 2'
                    UNION ALL SELECT 3,'Week 3'
                    UNION ALL SELECT 4,'Week 4'
                )

                SELECT
                    w.Label,
                    ISNULL(COUNT(ra.report_assigned_id),0) Events
                FROM Weeks w

                LEFT JOIN {Tbl.report_assigned} ra
                    ON
                    (
                        CASE DATEDIFF
                        (
                            WEEK,
                            ra.completed_datetime,
                            DATEADD
                            (
                                DAY,
                                -(DATEPART(WEEKDAY,GETDATE()) + 5) % 7,
                                CAST(GETDATE() AS DATE)
                            )
                        )
                            WHEN 0 THEN 1
                            WHEN 1 THEN 2
                            WHEN 2 THEN 3
                            WHEN 3 THEN 4
                        END
                    ) = w.WeekNo

                    AND ra.completed_by=@assigned_by
                    AND ra.completed_datetime IS NOT NULL
                    AND ra.assign_status='completed'
                    AND ra.is_active=1
                    AND {GetReportDateFilter(type)}

                GROUP BY
                    w.WeekNo,
                    w.Label

                ORDER BY
                    w.WeekNo";

                // =========================
                // DAY
                // =========================
                default:

                    return $@"
                WITH Hours AS
                (
                    SELECT 0 Hr

                    UNION ALL

                    SELECT Hr+1
                    FROM Hours
                    WHERE Hr<23
                )

                SELECT
                    RIGHT('00'+CAST(h.Hr AS VARCHAR),2)+':00' Label,
                    ISNULL(COUNT(ra.report_assigned_id),0) Events
                FROM Hours h

                LEFT JOIN {Tbl.report_assigned} ra
                    ON DATEPART(HOUR,ra.completed_datetime)=h.Hr
                    AND ra.completed_by=@assigned_by
                    AND ra.completed_datetime IS NOT NULL
                    AND ra.assign_status='completed'
                    AND ra.is_active=1
                    AND {GetReportDateFilter(type)}

                GROUP BY
                    h.Hr

                ORDER BY
                    h.Hr

                OPTION(MAXRECURSION 24)";
            }
        }

        #endregion

    }

    public class MyProgressChartDTO
    {
        public string Label { get; set; }

        public int Events { get; set; }
    }


    public class EventEcgSearchReqDTO
    {
        public DateTime? from_date { get; set; }
        public DateTime? to_date { get; set; }
        public string date_type { get; set; }//process_date.event_date
        public long queued_events_records_id { get; set; }
        public long user_id { get; set; }
        public List<long> finding_ids { get; set; }
        public int limit { get; set; }

        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public int offSet => (pageNumber - 1) * pageSize;
        public string sortBy { get; set; }
        public string orderBy { get; set; }
    }

    public class EventEcgSearchResDTO
    {
        public long queued_events_records_id { get; set; }
        public long patient_enrollment_id { get; set; }
        public DateTime? event_start_datetime { get; set; }
        public DateTime? completed_datetime { get; set; }
        public long patient_id { get; set; }
        public string patient_name { get; set; }
        public long user_id { get; set; }
        public string user_name { get; set; }
        public List<FindingDTO> findings_list { get; set; }


        #region Query
        internal static string get_ecg_search(EventEcgSearchReqDTO obj)
        {
            string sortBy = obj.sortBy;

            if (obj.sortBy == "queued_events_records_id")
            {
                sortBy = "qe.queued_events_records_id";
            }

            string condition = $@"
                                qe.completed_by IS NOT NULL
                                AND qe.is_active = 1

                                AND ev.is_final_completed = 1
                                AND ev.is_active = 1

                                AND (
                                    @date_type IS NULL
                                    OR @date_type = ''
                                    OR (@date_type = 'process_date' AND qe.completed_datetime IS NOT NULL)
                                    OR (@date_type = 'event_date' AND qe.event_start_datetime IS NOT NULL)
                                )

                                AND (
                                @from_date IS NULL
                                OR (@date_type = 'process_date' AND qe.completed_datetime >= @from_date)
                                OR (@date_type = 'event_date' AND qe.event_start_datetime >= @from_date)
                            )

                            AND (
                                @to_date IS NULL
                                OR (@date_type = 'process_date'
                                    AND qe.completed_datetime < DATEADD(DAY, 1, @to_date))
                                OR (@date_type = 'event_date'
                                    AND qe.event_start_datetime < DATEADD(DAY, 1, @to_date))
                            )

                            AND (
                                @queued_events_records_id = 0
                                OR qe.queued_events_records_id = @queued_events_records_id
                            )

                            AND (
                                @user_id = 0
                                OR um.user_id = @user_id
                            )

                            AND (
                                @has_findings = 0
                                OR EXISTS
                                (
                                    SELECT 1    
                                    FROM {Tbl.sub_strip_event_finding} AS sef
                                    WHERE sef.queued_events_records_id =
                                          qe.queued_events_records_id
                                      AND sef.event_finding_id IN @finding_ids
                                      AND sef.is_active = 1
                                )
                            )";

            return $@"
                    ;WITH FilteredData AS
                    (
                        SELECT
                            qe.queued_events_records_id,
                            qe.event_start_datetime,
                            qe.event_status,
                            qe.created_by,
                            qe.created_datetime,
                            qe.modified_by,
                            qe.is_active,
                            qe.patient_enrollment_id,
                            qe.completed_by,
                            qe.completed_datetime,

                            ev.event_assign_id,
                            ev.work_status,
                            ev.is_active AS event_is_active,
                            ev.is_final_completed,

                            p.patient_id,
                            p.patient_name,

                            um.user_id,
                            um.user_name,

                            ROW_NUMBER() OVER
                            (
                                ORDER BY {sortBy} {obj.orderBy}
                            ) AS RowNum

                        FROM {Tbl.queued_events_records} AS qe

                        INNER JOIN
                        (
                            SELECT *
                            FROM
                            (
                                SELECT
                                    ea.*,
                                    ROW_NUMBER() OVER
                                    (
                                        PARTITION BY ea.queued_events_records_id
                                        ORDER BY ea.event_assign_id DESC
                                    ) AS rn
                                FROM {Tbl.event_assign} AS ea
                                WHERE ea.is_active = 1
                                  AND ea.is_final_completed = 1
                            ) AS x
                            WHERE x.rn = 1
                        ) AS ev
                            ON ev.queued_events_records_id =
                               qe.queued_events_records_id

                        INNER JOIN {Tbl.patient_enrollment} AS pe
                            ON pe.patient_enrollment_id =
                               qe.patient_enrollment_id

                        INNER JOIN {Tbl.patient} AS p
                            ON p.patient_id =
                               pe.patient_id

                        INNER JOIN {Tbl.user_master} AS um
                            ON um.user_id =
                               qe.completed_by

                        WHERE {condition}
                    )

                    SELECT COUNT(*)
                    FROM FilteredData
                    WHERE RowNum <= @limit;


                    ;WITH FilteredData AS
                    (
                        SELECT
                            qe.queued_events_records_id,
                            qe.event_start_datetime,
                            qe.event_status,
                            qe.created_by,
                            qe.created_datetime,
                            qe.modified_by,
                            qe.is_active,
                            qe.patient_enrollment_id,
                            qe.completed_by,
                            qe.completed_datetime,

                            ev.event_assign_id,
                            ev.work_status,
                            ev.is_active AS event_is_active,
                            ev.is_final_completed,

                            p.patient_id,
                            p.patient_name,

                            um.user_id,
                            um.user_name,

                            ROW_NUMBER() OVER
                            (
                                ORDER BY {sortBy} {obj.orderBy}
                            ) AS RowNum

                        FROM {Tbl.queued_events_records} AS qe

                        INNER JOIN
                        (
                            SELECT *
                            FROM
                            (
                                SELECT
                                    ea.*,
                                    ROW_NUMBER() OVER
                                    (
                                        PARTITION BY ea.queued_events_records_id
                                        ORDER BY ea.event_assign_id DESC
                                    ) AS rn
                                FROM {Tbl.event_assign} AS ea
                                WHERE ea.is_active = 1
                                  AND ea.is_final_completed = 1
                            ) AS x
                            WHERE x.rn = 1
                        ) AS ev
                            ON ev.queued_events_records_id =
                               qe.queued_events_records_id

                        INNER JOIN {Tbl.patient_enrollment} AS pe
                            ON pe.patient_enrollment_id =
                               qe.patient_enrollment_id

                        INNER JOIN {Tbl.patient} AS p
                            ON p.patient_id =
                               pe.patient_id

                        INNER JOIN {Tbl.user_master} AS um
                            ON um.user_id =
                               qe.completed_by

                        WHERE {condition}
                    )

                    SELECT
                        queued_events_records_id,
                        event_start_datetime,
                        event_status,
                        created_by,
                        created_datetime,
                        modified_by,
                        is_active,
                        patient_enrollment_id,
                        completed_by,
                        completed_datetime,

                        event_assign_id,
                        work_status,
                        event_is_active,
                        is_final_completed,

                        patient_id,
                        patient_name,

                        user_id,
                        user_name

                    FROM FilteredData

                    WHERE RowNum > @offSet
                      AND RowNum <=
                        CASE
                            WHEN @limit < (@offSet + @pageSize)
                                THEN @limit
                            ELSE (@offSet + @pageSize)
                        END

                    ORDER BY RowNum;";
        }
        #endregion
    }


    public class ReportTypeDTO
    {
        public long report_type_id { get; set; }
        public string report_type_name { get; set; }
        public long created_by { get; set; }
        public DateTime? created_datetime { get; set; }
        public long modified_by { get; set; }
        public DateTime? modified_datetime { get; set; }
        public bool is_active { get; set; }

        #region Query

        internal static string insert_report_type()
        {
            return $@"
                    INSERT INTO {Tbl.report_type} (report_type_name, created_by)
                    VALUES (@report_type_name, @created_by)";
        }
        internal static string get_report_type()
        {
            return $@"
                    SELECT
                        report_type_id,
                        report_type_name,
                        created_by,
                        created_datetime,
                        modified_by,
                        modified_datetime,
                        is_active
                    FROM {Tbl.report_type}
                    WHERE is_active = 1";
        }
        internal static string update_report_type()
        {
            return $@"report_type_name = @report_type_name , modified_by = @modified_by, modified_datetime = getdate(), is_active = @is_active where report_type_id = @report_type_id";
        }
        #endregion
    }
    public class ReportGenerateListResDTO
    {
        public long patient_report_id { get; set; }
        public long patient_enrollment_id { get; set; }
        public long report_type_id { get; set; }
        public string report_type_name { get; set; }
        public string report_status { get; set; }
        public long? generate_to { get; set; }
        public DateTime? report_started_datetime { get; set; }
        public DateTime? report_due_datetime { get; set; }
        public DateTime? report_generated_datetime { get; set; }
        public DateTime? report_finalized_datetime { get; set; }
        public long? created_by { get; set; }
        public DateTime? created_datetime { get; set; }
        public long? modified_by { get; set; }
        public DateTime? modified_datetime { get; set; }
        public bool is_active { get; set; }
        public List<PatientReportGenerateStripDTO> stripList { get; set; }




    }

    public class PatientReportGenerateReqDTO
    {
        public long patient_report_id { get; set; }
        public long report_assigned_id { get; set; }
        public long created_by { get; set; }

        public List<PatientReportGenerateStripDTO> stripIds { get; set; }



        #region

        internal static string publish_report()
        {
            return $@"update {Tbl.patient_report}
                    set report_status = 'completed',
                        report_finalized_datetime = GETDATE(),
                        modified_by = @modified_by,
                        modified_datetime = GETDATE()
                        where patient_report_id = @patient_report_id and report_status = 'inprogress';";
        }
        internal static string update_assigned_status()
        {
            return $@"update {Tbl.report_assigned}
                    set assign_status = 'pending',
                        modified_by = @modified_by,
                        modified_datetime = GETDATE()
                        where report_assigned_id = @report_assigned_id and patient_report_id = @patient_report_id AND assign_status = 'generated';";
        }
        internal static string publish_assigned_status()
        {
            return $@"update {Tbl.report_assigned}
                    set assign_status = 'completed',
                        completed_by = @modified_by,
                        completed_datetime = GETDATE()
                        where report_assigned_id = @report_assigned_id and patient_report_id = @patient_report_id AND assign_status = 'generated';";
        }

        internal static string update_patient_report_status_pending()
        {
            return $@"
                    UPDATE {Tbl.patient_report}
                    SET
                        report_status = 'pending',
                        modified_datetime = GETDATE(),
                        modified_by = @modified_by
                    WHERE patient_report_id = @patient_report_id
                      AND is_active = 1
                      AND report_status = 'inprogress'";
        }

        internal static string update_patient_generate_report()
        {
            return $@"update {Tbl.patient_report_generate}
                        set is_active = 0 ,
                            modified_datetime = GETDATE(),
                            modified_by = @modified_by
                            where  patient_report_id = @patient_report_id and is_active = 1";
        }

        internal static string delete_all_patient_report_generate()
        {
            return $@"
                    UPDATE {Tbl.patient_report_generate}
                    SET
                        is_active = 0,
                        modified_datetime = GETDATE(),
                        modified_by = @modified_by
                    WHERE patient_report_id = @patient_report_id and report_assigned_id =@report_assigned_id
                      AND is_active = 1";
        }

        internal static string check_patient_report_status()
        {
            return $@"
        SELECT TOP 1
            a.report_assigned_id,
            a.patient_report_id,
            a.assign_status,
            a.created_by as report_assign_to,
            a.modified_by,
            a.is_active
        FROM {Tbl.report_assigned} AS a
        INNER JOIN {Tbl.patient_report} AS b
            ON a.patient_report_id = b.patient_report_id
        WHERE a.report_assigned_id = @report_assigned_id
          AND a.patient_report_id = @patient_report_id
          AND a.is_active = 1";
        }

        internal static string get_report_generate_list()
        {
            return $@"
        SELECT COUNT(*)
        from tbl_report_assigned as a
        inner join tbl_patient_report as b
        on a.patient_report_id = b.patient_report_id
        inner join tbl_patient_enrollment as c
        on c.patient_enrollment_id = a.patient_enrollment_id
        inner join tbl_report_type as d
        on d.report_type_id = b.report_type_id
        inner join tbl_patient as e
        on e.patient_id = c.patient_id
        where e.patient_id = @patient_id and b.report_generated_datetime is not null and b.report_status in ('inprogress','completed') and a.assign_status in ('generated','completed') ;
       


          
        select
        a.patient_report_id,
        a.patient_enrollment_id,
        d.report_type_id,
        d.report_type_name,
        b.report_status,
        b.generate_to,
        b.report_started_datetime,
        b.report_due_datetime,
        b.report_generated_datetime,
        b.report_finalized_datetime,
        b.created_by,
        b.created_datetime,
        b.modified_by,
        b.modified_datetime,
        a.report_assigned_id
        from tbl_report_assigned as a
        inner join tbl_patient_report as b
        on a.patient_report_id = b.patient_report_id
        inner join tbl_patient_enrollment as c
        on c.patient_enrollment_id = a.patient_enrollment_id
        inner join tbl_report_type as d
        on d.report_type_id = b.report_type_id
        inner join tbl_patient as e
        on e.patient_id = c.patient_id
        where e.patient_id = @patient_id and b.report_generated_datetime is not null and b.report_status in ('inprogress','completed') and a.assign_status in ('generated','completed') 
       
    ";
        }

        internal static string existing_patient_report_generate()
        {
            return $@"
                SELECT
                    queued_events_records_id,
                    sub_strip_event_id
                FROM {Tbl.patient_report_generate}
                WHERE patient_report_id = @patient_report_id 
                  AND is_active = 1 order by patient_report_generate_id";
        }

        internal static string update_patient_report_generate()
        {
            return $@"
                UPDATE {Tbl.patient_report_generate}
                SET
                    is_active = 0,
                    modified_by = @modified_by,
                    modified_datetime = GETDATE()
                WHERE patient_report_id = @patient_report_id
                  AND queued_events_records_id = @queued_events_records_id
                  AND sub_strip_event_id = @sub_strip_event_id
                  AND report_assigned_id = @report_assigned_id
                  AND is_active = 1";
        }
        internal static string insert_patient_report_generate()
        {
            return $@"
                INSERT INTO {Tbl.patient_report_generate}
                (
                    patient_report_id,
                    report_assigned_id,
                    queued_events_records_id,
                    sub_strip_event_id,
                    created_by
                )
                VALUES
                (
                    @patient_report_id,
                    @report_assigned_id,
                    @queued_events_records_id,
                    @sub_strip_event_id,
                    @created_by)";
        }
        #endregion
    }

    public class PatientReportGenerateStripDTO
    {
        public long queued_events_records_id { get; set; }
        public long sub_strip_event_id { get; set; }
    }



    public class PatientReportSearchReqDTO
    {
        public string type { get; set; }//all,inprogress,completed
        public DateTime? from_date { get; set; }
        public DateTime? to_date { get; set; }
        public long? report_type_id { get; set; }
        public long? patient_report_id { get; set; }
        public long? user_id { get; set; }
        public int limit { get; set; }

        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public int offSet => (pageNumber - 1) * pageSize;
        public string sortBy { get; set; }
        public string orderBy { get; set; }



        #region Query
        internal static string patient_report_search_list(PatientReportSearchReqDTO obj)
        {
            string condition = $@"
                                a.is_active = 1
                                AND f.is_active = 1

                                AND f.assign_status IN ('generated', 'completed')
                                AND a.report_status IN ('inprogress', 'completed')

                                AND (@from_date IS NULL
                                     OR a.created_datetime >= @from_date)

                                AND (@to_date IS NULL
                                     OR a.created_datetime < DATEADD(DAY, 1, @to_date))

                                AND (@report_type_id = 0
                                     OR a.report_type_id = @report_type_id)

                                AND (@patient_report_id = 0
                                     OR a.patient_report_id = @patient_report_id)

                                AND (@user_id = 0
                                     OR a.generate_to = @user_id)

                                AND
                                ( 
                                        ( @type = 'all' AND a.report_status in ( 'inprogress' , 'completed' ) )
                                       OR ( @type is null AND a.report_status in ( 'inprogress' , 'completed' ) )
                                       OR (@type = 'inprogress' AND a.report_status = 'inprogress')
                                       OR (@type = 'completed' AND a.report_status = 'completed')
                                )";

            return $@"

                                -- Total Count
                                SELECT COUNT(*) AS totalCount

                                FROM {Tbl.report_assigned} AS f

                                INNER JOIN {Tbl.patient_report} AS a
                                    ON f.patient_report_id = a.patient_report_id

                                INNER JOIN {Tbl.patient_enrollment} AS b
                                    ON a.patient_enrollment_id = b.patient_enrollment_id

                                INNER JOIN {Tbl.patient} AS c
                                    ON c.patient_id = b.patient_id

                                LEFT JOIN {Tbl.user_master} AS d
                                    ON d.user_id = a.generate_to

                                INNER JOIN {Tbl.report_type} AS e
                                    ON e.report_type_id = a.report_type_id

                                WHERE {condition};


                                -- Report List
                                SELECT
                                    a.patient_report_id,
                                    a.patient_enrollment_id,
                                    e.report_type_id,
                                    a.report_status,
                                    a.generate_to,
                                    a.report_due_datetime,
                                    a.report_generated_datetime,
                                    a.report_finalized_datetime,
                                    a.report_started_datetime,

                                    c.patient_id,
                                    c.patient_name,

                                    d.user_id,
                                    d.user_name,
                                    dm.device_id,
                                    dm.device_name,

                                    f.report_assigned_id,
                                    f.assign_status,

                                    e.report_type_name

                                FROM {Tbl.report_assigned} AS f

                                INNER JOIN {Tbl.patient_report} AS a
                                    ON f.patient_report_id = a.patient_report_id

                                INNER JOIN {Tbl.patient_enrollment} AS b
                                    ON a.patient_enrollment_id = b.patient_enrollment_id

                                INNER JOIN {Tbl.patient} AS c
                                    ON c.patient_id = b.patient_id

                                LEFT JOIN {Tbl.user_master} AS d
                                    ON d.user_id = a.generate_to

                                LEFT JOIN {Tbl.device_mast} AS dm
                                    ON dm.device_id = b.device_id

                                INNER JOIN {Tbl.report_type} AS e
                                    ON e.report_type_id = a.report_type_id

                                WHERE {condition}

                                ORDER BY {obj.sortBy} {obj.orderBy}

                                OFFSET @offSet ROWS

                                FETCH NEXT
                                    CASE
                                        WHEN @limit - @offSet <= 0
                                            THEN 0

                                        WHEN @limit - @offSet < @pageSize
                                            THEN @limit - @offSet

                                        ELSE @pageSize
                                    END
                                ROWS ONLY;";
        }
        //internal static string patient_report_search_list(PatientReportSearchReqDTO obj)
        //{
        //    string condition = $@"
        //                    a.is_active = 1
        //                    AND b.is_active = 1
        //                    AND (@from_date IS NULL OR a.created_datetime >= @from_date)
        //                    AND (@to_date IS NULL OR a.created_datetime < DATEADD(DAY, 1, @to_date))
        //                    AND (@report_type_id = 0 OR a.report_type_id = @report_type_id)
        //                    AND (@patient_report_id  = 0 OR a.patient_report_id = @patient_report_id)
        //                    AND (@user_id  = 0 OR a.generate_to = @user_id)
        //                    AND (
        //                            ( @type = 'all' AND a.report_status in ( 'inprogress' , 'completed' ) )
        //                            OR ( @type is null AND a.report_status in ( 'inprogress' , 'completed' ) )
        //                            OR (@type = 'inprogress' AND a.report_status = 'inprogress')
        //                            OR (@type = 'completed' AND a.report_status = 'completed')
        //                        )";


        //    return $@"

        //            SELECT COUNT(*) AS totalCount
        //            from {Tbl.patient_report} a
        //            inner join {Tbl.patient_enrollment} b on a.patient_enrollment_id = b.patient_enrollment_id
        //            inner join {Tbl.patient} c on c.patient_id = b.patient_id
        //            inner join {Tbl.user_master} d on d.user_id = a.generate_to
        //            inner join {Tbl.report_type} e on e.report_type_id = a.report_type_id
        //            where {condition};


        //                SELECT
        //                    a.patient_report_id,
        //                    a.patient_enrollment_id,
        //                    a.report_type_id,
        //                    a.report_status,
        //                    a.generate_to,
        //                    a.report_due_datetime,
        //                    a.report_generated_datetime,
        //                    a.report_finalized_datetime,
        //                    a.report_started_datetime,

        //                    c.patient_id,
        //                    c.patient_name,

        //                    d.user_id,
        //                    d.user_name,

        //                    e.report_type_name

        //                FROM {Tbl.patient_report} AS a

        //                INNER JOIN {Tbl.patient_enrollment} AS b
        //                    ON a.patient_enrollment_id = b.patient_enrollment_id

        //                INNER JOIN {Tbl.patient} AS c
        //                    ON c.patient_id = b.patient_id

        //                LEFT JOIN {Tbl.user_master} AS d
        //                    ON d.user_id = a.generate_to

        //                INNER JOIN {Tbl.report_type} AS e
        //                    ON e.report_type_id = a.report_type_id

        //                WHERE {condition}

        //                ORDER BY {obj.sortBy} {obj.orderBy}

        //                OFFSET @offSet ROWS

        //                FETCH NEXT
        //                    CASE
        //                        WHEN @limit - @offSet <= 0
        //                            THEN 0

        //                        WHEN @limit - @offSet < @pageSize
        //                            THEN @limit - @offSet

        //                        ELSE @pageSize
        //                    END
        //                ROWS ONLY;";
        //}

        #endregion


    }
    public class PatientReportSearchResDTO
    {
        public long patient_report_id { get; set; }
        public long patient_id { get; set; }
        public string patient_name { get; set; }
        public long user_id { get; set; }
        public string user_name { get; set; }
        public DateTime? report_started_datetime { get; set; }
        public DateTime? report_due_datetime { get; set; }
        public DateTime? report_generated_datetime { get; set; }
        public DateTime? report_finalized_datetime { get; set; }
        public long report_type_id { get; set; }
        public string report_type_name { get; set; }



    }



}
