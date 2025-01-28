using MagicVilla_VillaAPI.DB;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MagicVilla_VillaAPI.Repository
{
    public class VillaRepository : Repository<Villa>,IVillaRepository
    {
        private readonly ApplicationDBContext _db;
        public VillaRepository(ApplicationDBContext db):base(db)
        {
            _db = db;
        }
        
        public async Task<Villa> UpdateAsync(Villa villa)
        {
            villa.UpdatedDate = DateTime.Now;
            _db.Villas.Update(villa);
          await _db.SaveChangesAsync();
            return villa;
        }
    }
}
