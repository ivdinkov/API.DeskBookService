﻿using API.DeskBookService.Core.DataInterfaces;
using API.DeskBookService.Core.Domain;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.DeskBookService.Data.Repository
{
    public class DeskRepository : IDeskRepository
    {
        private IMongoCollection<Desk> _desks;
        private IMongoCollection<DeskBooking> _bookings;

        public DeskRepository(IDeskBookerDataContext deskBookerContext)
        {
            _desks = deskBookerContext.Desks;
            _bookings = deskBookerContext.DesksBooking;
        }

        public async Task<Desk> Save(Desk desk)
        {
            await _desks.InsertOneAsync(desk);
            return desk;
        }

        public async Task<Desk> Get(string id)
        {
            try
            {
                return await _desks.Find<Desk>(desk => desk.Id == id).FirstOrDefaultAsync();
            }
            catch (System.FormatException)
            {
                return null;
            }            
        }

        public async Task<IEnumerable<Desk>> Get()
        {
            return await _desks.Find(_ => true).ToListAsync();
        }

        public async Task<bool> Remove(string id)
        {
            var booking = await _bookings.Find(b => b.DeskId == id).AnyAsync();
            if (booking)
                return false;

            var result = await _desks.DeleteOneAsync(desk => desk.Id == id);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        public async Task<bool> Update(string id, Desk deskIn)
        {
            var desk = Get(id);
            if (desk == null)
                return false;

            var result = await _desks.ReplaceOneAsync(d => d.Id == id, deskIn);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }
    }
}
