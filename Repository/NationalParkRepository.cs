using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ParkyAPI.Models;
using ParkyAPI.Repository.IRepository;

namespace ParkyAPI.Repository
{
    public class NationalParkRepository : INationalParkRepository
    {
        public bool CreateNationalPark(NationalPark nationalPark)
        {
            throw new NotImplementedException();
        }

        public bool Delete(NationalPark nationalPark)
        {
            throw new NotImplementedException();
        }

        public NationalPark GetNationalPark(int nationalParkId)
        {
            throw new NotImplementedException();
        }

        public ICollection<NationalPark> GetNationalParks()
        {
            throw new NotImplementedException();
        }

        public bool NationalParkExists(string name)
        {
            throw new NotImplementedException();
        }

        public bool NationalParkExists(int id)
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            throw new NotImplementedException();
        }

        public bool Update(NationalPark nationalPark)
        {
            throw new NotImplementedException();
        }
    }
}