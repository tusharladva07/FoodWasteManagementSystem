namespace Hospitality.API.Repositories;

using Hospitality.API.Models;

public interface IHospitalityRepository
{
    void AddWasteRecord(BuffetWasteRecord record);
    IEnumerable<BuffetWasteRecord> GetWasteRecords();

    void AddCancellation(CanceledOrderRecord record);
    IEnumerable<CanceledOrderRecord> GetCancellations();
}
