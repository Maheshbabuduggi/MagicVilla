using MagicVilla_VillaAPI.DB;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MagicVilla_VillaAPI.Repository
{
    public class VillaNumberRepository : Repository<VillaNumber>, IVillaNumberRepository
    {
        private readonly ApplicationDBContext _db;
        public VillaNumberRepository(ApplicationDBContext db) : base(db)
        {
            _db = db;
        }

        public async Task<VillaNumber> UpdateAsync(VillaNumber villaNumber)
        {
            villaNumber.UpdatedDate = DateTime.Now;
            _db.VillaNumbers.Update(villaNumber);
            await _db.SaveChangesAsync();
            return villaNumber;
        }
    }
}
