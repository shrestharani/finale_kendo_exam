using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KENDO_PRACTICE.Models;
using Npgsql;

namespace KENDO_PRACTICE.Repository
{
    public class CustomerRepository:ICustomerRepository
    {
         private readonly NpgsqlConnection conn;
        private readonly IHttpContextAccessor accessor;
        public CustomerRepository(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            conn = new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection"));
            this.accessor = httpContextAccessor;
        }

        public void AddUser(CustomerModel customerModel)
        {
            try
            {
                conn.Open();
                string query = "INSERT INTO t_users(c_username,c_email,c_password,c_role) VALUES (@c_username,@c_email,@c_password,'customer')";
                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@c_username", customerModel.c_username);
                cmd.Parameters.AddWithValue("@c_email", customerModel.c_email);
                cmd.Parameters.AddWithValue("@c_password", customerModel.c_password);
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                conn.Close();
            }
        }

        public bool CheckEmail(string email)
        {
            try
            {
                conn.Open();
                string query = "SELECT 1 FROM t_users WHERE c_email=@c_email LIMIT 1";
                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@c_email", email);
                var reader = cmd.ExecuteReader();
                return reader.Read();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
            }
            finally
            {
                conn.Close();
            }
            return false;
        }

        public bool CheckUsername(string username)
        {
            try
            {
                conn.Open();
                string query = "SELECT 1 FROM t_users WHERE c_username=@c_username LIMIT 1";
                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@c_username", username);
                var reader = cmd.ExecuteReader();
                return reader.Read();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
            }
            finally
            {
                conn.Close();
            }
            return false;
        }

        public bool Login(CustomerModel customerModel)
        {
            try
            {
                conn.Open();
                string query = "SELECT * FROM t_users WHERE c_username=@c_username AND c_password=@c_password";
                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@c_username", customerModel.c_username);
                cmd.Parameters.AddWithValue("@c_password", customerModel.c_password);
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    accessor.HttpContext.Session.SetString("username", reader["c_username"].ToString());
                    accessor.HttpContext.Session.SetString("role", reader["c_role"].ToString());
                    accessor.HttpContext.Session.SetInt32("userid", (int)reader["c_id"]);
                    accessor.HttpContext.Session.SetString("email", reader["c_email"].ToString());
                    return true;
                }
            }
            catch(Exception ex)
            {
                System.Console.WriteLine(ex);
            }
            finally
            {
                conn.Close();
            }
            return false;
            
        }

    }
}