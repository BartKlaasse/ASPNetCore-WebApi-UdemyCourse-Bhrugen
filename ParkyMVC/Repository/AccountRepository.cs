using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ParkyMVC.Models;
using ParkyMVC.Repository.IRepository;

namespace ParkyMVC.Repository
{
    public class AccountRepository : Repository<User>, IAccountRepository
    {
        private readonly IHttpClientFactory _clientFactory;
        public AccountRepository(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public Task<User> LoginAsync(string url, User objToCreate)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RegisterAsync(string url, User objToCreate)
        {
            throw new NotImplementedException();
        }
    }
}