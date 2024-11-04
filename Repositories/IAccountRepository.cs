using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IAccountRepository
    {
        public List<Account> GetAccounts();
        public List<Role> GetRoles();
        public void AddAccount(Account account);
        public void UpdateAccount(Account account);
        public void DeleteAccount(int accountId);
        public Account GetAccountById(int accountId); 
        public Account GetAccountByUserName(string name);

        public Account GetAccountByEmployeeId(int employeeId);
        public Employee GetEmployeeByUsername(int accountId);
    }
}
