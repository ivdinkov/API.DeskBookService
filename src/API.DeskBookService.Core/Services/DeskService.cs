using API.DeskBookService.Core.Contracts.Requests;
using API.DeskBookService.Core.DataInterfaces;
using API.DeskBookService.Core.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.DeskBookService.Core.Services
{
    public class DeskService : IDeskService
    {
        private readonly IDeskRepository _deskRepo;
        private readonly IBookingRepository _bookingRepo;

        public DeskService(IDeskRepository deskRepository, IBookingRepository bookingRepository)
        {
            _deskRepo = deskRepository;
            _bookingRepo = bookingRepository;
        }
        public async Task<Desk> Get(string id)
        {
            return await _deskRepo.Get(id);
        }

        public async Task<IEnumerable<Desk>> GetAll()
        {
            return await _deskRepo.Get();
        }

        public async Task<bool> Remove(string id)
        {
            var result = Get(id);
            if (result == null)
                return false;

            return await _deskRepo.Remove(id);
        }

        public async Task<Desk> Save(Desk desk)
        {
            return await _deskRepo.Save(desk);
        }

        public async Task<bool> Update(string id, DeskUpdateRequest deskIn)
        {
            var desk = await Get(id);
            if (desk == null)
                return false;

            desk.Description = deskIn.Description;
            desk.Name = deskIn.Name;

            return await _deskRepo.Update(id,desk);
        }
    }
}
