using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ParkyMVC.Models;

namespace ParkyMVC.Repository.IRepository
{
    public interface IAccountRepository : IRepository<User>
    {
        Task<User> LoginAsync(string url, User objToCreate);
        Task<bool> RegisterAsync(string url, User objToCreate);
    }
}