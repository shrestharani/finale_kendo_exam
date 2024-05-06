using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using KENDO_PRACTICE.Models;
using Npgsql;

namespace KENDO_PRACTICE.Repository
{
    public class EmpRepository :IEmpRepository
    {
         private readonly NpgsqlConnection _conn;
        // private readonly IHttpContextAccessor _httpContextAccessor;

        public EmpRepository(NpgsqlConnection connection)
        {
            _conn = connection;
            // _httpContextAccessor = httpContextAccessor;
        }
        public List<tblemp> GetAll()
        {
            List<tblemp> students = new List<tblemp>();

            try
            {
                _conn.Open();
                using var command = new NpgsqlCommand("SELECT * FROM t_emcrud", _conn);

                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    tblemp student = new tblemp
                    {
                        c_empid = Convert.ToInt32(reader["c_empid"]),
                        c_empname = reader["c_empname"].ToString(),
                        c_gender = reader["c_gender"].ToString(),
                        c_shift = reader["c_shift"].ToString(),
                        c_depid = Convert.ToInt32(reader["c_depid"]),
                        // c_dob = reader.GetFieldValue<DateOnly>("c_dob"),
                        c_corid=Convert.ToInt32(reader["c_course_id"]),
                          c_dob = reader.GetFieldValue<DateTime>("c_dob"),
                        c_empimage = reader["c_empimage"].ToString()
                    };

                    students.Add(student);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                _conn.Close();
            }

            return students;


        }

        public List<tbldept> GetDept()
        {
            List<tbldept> departments = new List<tbldept>();

            try
            {
                _conn.Open();
                using var command = new NpgsqlCommand("SELECT * FROM t_department", _conn);

                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    tbldept department = new tbldept
                    {
                        c_depid = Convert.ToInt32(reader["c_depid"]),
                        c_dename = reader["c_dename"].ToString()
                    };

                    departments.Add(department);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
              finally
            {
                _conn.Close();
            }

            return departments;


        }
        
        public List<tblcourse> Getcor()
        {
            List<tblcourse> employees = new List<tblcourse>();

            try
            {
                _conn.Open();
                string query = "SELECT * FROM t_course";
                using (NpgsqlCommand command = new NpgsqlCommand(query, _conn))
                {
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tblcourse course = new tblcourse
                            {
                                c_corid = Convert.ToInt32(reader["c_course_id"]),
                                c_corname = reader["c_course_name"].ToString()
                            };
                            employees.Add(course);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // Handle or log the exception
            }
            finally
            {
                if (_conn.State == ConnectionState.Open)
                {
                    _conn.Close();
                }
            }

            return employees;
        }

        // Implement other repository methods similarly...

        public void Dispose()
        {
            _conn.Dispose();
        }
        public tblemp GetOne(int id)
        {
            try
            {
                _conn.Open();
                using var command = new NpgsqlCommand("SELECT * FROM t_mcrud WHERE c_empid = @Id", _conn);
                command.Parameters.AddWithValue("@Id", id);

                using var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return new tblemp
                    {
                        c_empid = Convert.ToInt32(reader["c_empid"]),
                        c_empname = reader["c_empname"].ToString(),
                        c_gender = reader["c_gender"].ToString(),
                        c_shift = reader["c_shift"].ToString(),
                        c_depid = Convert.ToInt32(reader["c_depid"]),
                        // c_dob = reader.GetFieldValue<DateOnly>("c_dob"),
                                                 c_dob = reader.GetDateTime(reader.GetOrdinal("c_dob")),

                        c_empimage = reader["c_empimage"].ToString()
                    };
                }
                else
                {
                    // Return null or handle case where student with the given ID is not found
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // Handle exception (log, display error message, etc.)
                return null;
            }
            finally
            {
                _conn.Close();
            }
        }
        public List<tblemp> GetEmployeeFromUserName(string user)
        {
            Console.WriteLine("USER DATA :: : : : : " + user);
            List<tblemp> empList = new List<tblemp>();
            NpgsqlCommand cmd = new NpgsqlCommand();
            try
            {
                _conn.Open();
                cmd.Connection = _conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT * FROM t_mcrud where c_username = @username  ORDER BY c_empid ";

                cmd.Parameters.AddWithValue("@username", user);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var employee = new tblemp
                        {
                        c_empid = Convert.ToInt32(reader["c_empid"]),
                        c_empname = reader["c_empname"].ToString(),
                        c_gender = reader["c_gender"].ToString(),
                        c_shift = reader["c_shift"].ToString(),
                        c_depid = Convert.ToInt32(reader["c_depid"]),
                        // c_dob = reader.GetFieldValue<DateOnly>("c_dob"),
                          c_dob = reader.GetFieldValue<DateTime>("c_dob"),
                        c_empimage = reader["c_empimage"].ToString()
                        };
                        empList.Add(employee);

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            finally
            {
                _conn.Close();
            }
            return empList;
        }


        public void Insert(tblemp stud)
        {
            // var ename = _httpContextAccessor.HttpContext.Session.GetString("username");
            _conn.Open();
            using var command = new NpgsqlCommand("INSERT INTO t_emcrud(c_empname, c_gender, c_shift, c_depid, c_dob, c_empimage,c_username,c_course_id) VALUES (@empname, @gender, @shift, @depid, @dob, @image,@una,@c_corid)", _conn);
            command.CommandType = CommandType.Text;

            // Set parameter values with explicit NpgsqlDbType
            command.Parameters.AddWithValue("@empname", stud.c_empname);
            command.Parameters.AddWithValue("@gender", stud.c_gender);
            command.Parameters.AddWithValue("@shift", stud.c_shift);
            command.Parameters.AddWithValue("@depid", stud.c_depid);
            command.Parameters.AddWithValue("@dob", stud.c_dob);
            command.Parameters.AddWithValue("@image", stud.c_empimage);
            command.Parameters.AddWithValue("@una", stud.c_empname);
            command.Parameters.AddWithValue("@c_corid", stud.c_corid);
            
            

            command.ExecuteNonQuery();
            _conn.Close();

        }

        public void Update(tblemp stud)
        {
            _conn.Open();
            using var command = new NpgsqlCommand("UPDATE t_mcrud SET c_empname = @Name, c_gender = @Gender,c_shift=@shift, c_depid = @DeptId, c_dob = @Dob, c_empimage = @EmpImage WHERE c_empid = @Id", _conn);
            command.CommandType = CommandType.Text;

            // Set parameter values with explicit NpgsqlDbType
            command.Parameters.AddWithValue("@Id", stud.c_empid);
            // command.Parameters.AddWithValue("@DocumentPath", stud.c_empname);
            command.Parameters.AddWithValue("@Name", stud.c_empname);
            command.Parameters.AddWithValue("@Gender", stud.c_gender);
            command.Parameters.AddWithValue("@shift", stud.c_shift);
            command.Parameters.AddWithValue("@DeptId", stud.c_depid);
            command.Parameters.AddWithValue("@Dob", stud.c_dob);
            command.Parameters.AddWithValue("@EmpImage", stud.c_empimage);

            command.ExecuteNonQuery();
            _conn.Close();
        }
        public void Delete(int id)
        {
            using var command = new NpgsqlCommand("DELETE FROM t_mcrud WHERE c_empid =@Id", _conn);
            command.CommandType = CommandType.Text;
            command.Parameters.AddWithValue("@Id", id);

            _conn.Open();
            command.ExecuteNonQuery();

            _conn.Close();

        }

    }
}