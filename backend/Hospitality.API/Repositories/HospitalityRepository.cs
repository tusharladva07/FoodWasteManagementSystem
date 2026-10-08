namespace Hospitality.API.Repositories;

using Hospitality.API.Data;
using Hospitality.API.Models;

public class HospitalityRepository : IHospitalityRepository
{
    private readonly HospitalityDbContext _context;

    public HospitalityRepository(HospitalityDbContext context)
    {
        _context = context;
    }

    public void AddWasteRecord(BuffetWasteRecord record)
    {
        _context.BuffetWasteRecords.Add(record);
        _context.SaveChanges();
    }

    public IEnumerable<BuffetWasteRecord> GetWasteRecords()
    {
        return _context.BuffetWasteRecords.ToList();
    }

    public void AddCancellation(CanceledOrderRecord record)
    {
        _context.CanceledOrderRecords.Add(record);
        _context.SaveChanges();
    }

    public IEnumerable<CanceledOrderRecord> GetCancellations()
    {
        return _context.CanceledOrderRecords.ToList();
    }
}
