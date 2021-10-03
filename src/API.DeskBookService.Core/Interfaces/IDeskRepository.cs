using API.DeskBookService.Core.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.DeskBookService.Core.Interfaces
{
    public interface IDeskRepository
    {
        Task<Desk> Save(Desk desk);
        Task<Desk> Get(string id);
        Task<List<Desk>> GetAll();
        Task<List<Desk>> GetAvailableDesks(DateTime date);
        Task<bool> Update(string id, Desk deskIn);
        Task<bool> Remove(string id);
    }
}
