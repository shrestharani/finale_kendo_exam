using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KENDO_PRACTICE.Models;

namespace KENDO_PRACTICE.Repository
{
    public interface ICustomerRepository
    {
         void AddUser(CustomerModel customerModel);
        bool CheckEmail(string email);
        bool CheckUsername(string username);
        bool Login(CustomerModel customerModel);
    }
}