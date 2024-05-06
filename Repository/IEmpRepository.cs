using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KENDO_PRACTICE.Models;

namespace KENDO_PRACTICE.Repository
{
    public interface IEmpRepository
    {
        List<tblemp> GetAll();
        List<tbldept> GetDept();
         

        void Insert (tblemp stud);


        void Update(tblemp stud);


        void  Delete (int id);
        List<tblcourse> Getcor();

        tblemp GetOne(int id);
        List<tblemp> GetEmployeeFromUserName(string user);
    }
}