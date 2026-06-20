using elr_API.DTO.ReqDTO;
using elr_API.DTO.ResDTO;

namespace elr_API.IService.IHome_manager
{
    public interface IHomeService
    {
        Task<ResponseDTO> GetMenu();
        Task<ResponseDTO> AddUpdateMenu(MenuDTO obj);


        Task<ResponseDTO> GetCountry(CountryDTO obj);
        Task<List<CountryDTO>> CountryList(CountryDTO obj);
        Task<ResponseDTO> AddUpdateCountry(CountryDTO obj);

        Task<ResponseDTO> GetState(StateDTO obj);
        Task<List<StateDTO>> StateList(StateDTO obj);
        Task<ResponseDTO> AddUpdateState(StateDTO obj);

        Task<ResponseDTO> GetDistrict(DistrictDTO obj);
        Task<List<DistrictDTO>> DistrictList(DistrictDTO obj);
        Task<ResponseDTO> AddUpdateDistrict(DistrictDTO obj);

        Task<ResponseDTO> GetUserType(UserTypeDTO obj);
        Task<List<UserTypeDTO>> UserTypeList(UserTypeDTO obj);
        Task<ResponseDTO> AddUpdateUserType(UserTypeDTO obj);

        Task<ResponseDTO> ElrHospitalList(string? hospital_name);
        Task<ResponseDTO> AddUpdateHospital(AddUpdateHospitalReqDTO obj);
        //Task<ResponseDTO> GetHospital(HospitalDTO obj);
        //Task<List<HospitalDTO>> HospitalList(HospitalDTO obj);

        Task<ResponseDTO> ElrGetTest();
        //Task<List<AddUpdateTestDTO>> TestList(AddUpdateTestDTO obj);
        Task<ResponseDTO> AddUpdateTest(AddUpdateTestDTO obj);

        Task<ResponseDTO> ElrGetTestPrices(AddUpdateTestPricesDTO obj);
        Task<ResponseDTO> ElrGetTestPriceByDays(AddUpdateTestPricesDTO obj);
        Task<List<AddUpdateTestPricesDTO>> TestPricesList(AddUpdateTestPricesDTO obj);
        Task<ResponseDTO> AddUpdateTestPrices(AddUpdateTestPricesDTO obj);

        Task<ResponseDTO> ElrSymtomsTypeList();

        Task<ResponseDTO> GetLocation(LocationDTO obj);
        Task<List<LocationDTO>> LocationList(LocationDTO obj);
        Task<ResponseDTO> AddUpdateLocation(LocationDTO obj);

        Task<ResponseDTO> ElrUploadProcessedData(ProcessedRawDataDTO obj);

        Task<ResponseDTO> ElrQueueAnomalyList(QueuedAnomalyReqDTO obj);
        Task<ResponseDTO> ElrUploadQueueAnomaly(QueuedAnomalyDTO obj);

        Task<ResponseDTO> ElrGetNextEvent(long emp_id);
        Task<ResponseDTO> ElrTimeOutEvent(long queued_events_records_id, long emp_id);
        Task<ResponseDTO> ElrPatientCurrentStrip(long queued_events_records_id);

        Task<ResponseDTO> ElrSubStripEvent(SubStripEventDTO obj);
        Task<ResponseDTO> ElrInActiveSubStrip(long sub_strip_event_id, long emp_id);
        Task<ResponseDTO> ElrSubStripReport(SubStripReqDTO obj);
        Task<ResponseDTO> ElrAddUpdateFindingEvent(FindingEventDTO obj);
    }
}
