using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KENDO_PRACTICE.Models;
using Npgsql;

namespace KENDO_PRACTICE.Repository
{
    public class AdminRepository :IAdminRepository
    {
        private readonly NpgsqlConnection conn;
        private readonly IHttpContextAccessor accessor;
        public AdminRepository(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            conn = new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection"));
            this.accessor = httpContextAccessor;
        }


        public void AddAlbum(AlbumModel albumModel)
        {
            try
            {
                conn.Open();
                string query = "INSERT INTO t_album (c_album, c_genre, c_artist, c_title, c_price) VALUES (@c_album, @c_genre, @c_artist, @c_title, @c_price)";
                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@c_album", albumModel.c_album);
                cmd.Parameters.AddWithValue("@c_genre", albumModel.c_genre);
                cmd.Parameters.AddWithValue("@c_artist", albumModel.c_artist);
                cmd.Parameters.AddWithValue("@c_title", albumModel.c_title);
                cmd.Parameters.AddWithValue("@c_price", albumModel.c_price);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }


        public void UpdateAlbum(AlbumModel albumModel)
        {
            try
            {
                conn.Open();
                string query = "UPDATE t_album SET c_album = @c_album, c_genre = @c_genre, c_artist = @c_artist, c_title = @c_title, c_price = @c_price WHERE c_id = @c_id";
                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@c_album", albumModel.c_album);
                cmd.Parameters.AddWithValue("@c_genre", albumModel.c_genre);
                cmd.Parameters.AddWithValue("@c_artist", albumModel.c_artist);
                cmd.Parameters.AddWithValue("@c_title", albumModel.c_title);
                cmd.Parameters.AddWithValue("@c_price", albumModel.c_price);
                cmd.Parameters.AddWithValue("@c_id", albumModel.c_id);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        public List<AlbumModel> GetAlbums()
        {
            List<AlbumModel> albums = new List<AlbumModel>();
            try
            {
                conn.Open();
                string query = "SELECT * FROM t_album";
                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                NpgsqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    AlbumModel album = new AlbumModel();
                    album.c_id = reader.GetInt32(0);
                    album.c_album = reader.GetString(1);
                    album.c_genre = reader.GetString(2);
                    album.c_artist = reader.GetString(3);
                    album.c_title = reader.GetString(4);
                    album.c_price = reader.GetString(5);
                    albums.Add(album);
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return albums;

        }

        public void DeleteAlbum(int id)
        {
            try
            {
                conn.Open();
                string query = "DELETE FROM t_album WHERE c_id = @c_id";
                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@c_id", id);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        public AlbumModel GetAlbumById(int id)
        {
            AlbumModel album = new AlbumModel();
            try
            {
                conn.Open();
                string query = "SELECT * FROM t_album WHERE c_id = @c_id";
                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@c_id", id);
                NpgsqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    album.c_id = reader.GetInt32(0);
                    album.c_album = reader.GetString(1);
                    album.c_genre = reader.GetString(2);
                    album.c_artist = reader.GetString(3);
                    album.c_title = reader.GetString(4);
                    album.c_price = reader.GetString(5);
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return album;
        }

        public List<GraphModel> GetGenre()
        {
            List<GraphModel> graph = new List<GraphModel>();
            try
            {
                conn.Open();
                string query = "SELECT a.c_genre AS album_genre, SUM(ci.c_quantity) AS total_quantity FROM t_cart ci INNER JOIN t_album a ON ci.c_album_id = a.c_id where ci.c_checkedout = true GROUP BY a.c_genre ";
                var command = new NpgsqlCommand(query, conn);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    GraphModel graphModel = new GraphModel();
                    graphModel.name = reader.GetString(0);
                    graphModel.value = reader.GetInt32(1);
                    graph.Add(graphModel);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                conn.Close();
            }
            return graph;
        }

        public List<RevenueModel> GetWeeklyRevenue()
        {
            List<RevenueModel> revenue = new List<RevenueModel>();
            try
            {
                conn.Open();
                string query = "SELECT DATE_TRUNC ('week',c_shipping_date),COUNT(*),SUM(CAST(c_shipping_total AS DECIMAL)) FROM t_shipping GROUP BY DATE_TRUNC('week',c_shipping_date)ORDER BY DATE_TRUNC('week',c_shipping_date);";
                var command = new NpgsqlCommand(query, conn);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    RevenueModel revenueModel = new RevenueModel();
                    revenueModel.c_date = reader.GetDateTime(0);
                    revenueModel.c_sales = reader.GetInt32(1);
                    revenueModel.c_revenue = reader.GetInt32(2);
                    revenue.Add(revenueModel);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                conn.Close();
            }
            return revenue;
        }
        public List<RevenueModel> GetMonthRevenue()
        {
            List<RevenueModel> revenue = new List<RevenueModel>();
            try
            {
                conn.Open();
                string query = "SELECT DATE_TRUNC ('week',c_shipping_date),COUNT(*),SUM(CAST(c_shipping_total AS DECIMAL)) FROM t_shipping GROUP BY DATE_TRUNC('month',c_shipping_date)ORDER BY DATE_TRUNC('month',c_shipping_date);";
                var command = new NpgsqlCommand(query, conn);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    RevenueModel revenueModel = new RevenueModel();
                    revenueModel.c_date = reader.GetDateTime(0);
                    revenueModel.c_sales = reader.GetInt32(1);
                    revenueModel.c_revenue = reader.GetInt32(2);
                    revenue.Add(revenueModel);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                conn.Close();
            }
            return revenue;
        }

        public List<RevenueModel> GetRevenue()
        {
            List<RevenueModel> revenue = new List<RevenueModel>();
            try
            {
                conn.Open();
                string query = "SELECT c_shipping_date, COUNT(*) , SUM(CAST(c_shipping_total AS DECIMAL)) FROM t_shipping GROUP BY c_shipping_date ORDER BY c_shipping_date ";
                var command = new NpgsqlCommand(query, conn);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    RevenueModel revenueModel = new RevenueModel();
                    revenueModel.c_date = reader.GetDateTime(0);
                    revenueModel.c_sales = reader.GetInt32(1);
                    revenueModel.c_revenue = reader.GetInt32(2);
                    revenue.Add(revenueModel);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                conn.Close();
            }
            return revenue;
        }


         public bool DeleteMultipleStudents(List<int> studentIds)
        {
            try
            {
                conn.Open();

                string sql = "DELETE FROM public.tblstudent WHERE id = ANY(@studentIds)";
                using var command = new NpgsqlCommand(sql, conn);
                command.Parameters.AddWithValue("@studentIds", studentIds.ToArray());
                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                conn.Close();
            }
        }


        public void CheckoutDetails(CheckoutModel checkoutModel)
        {
            try
            {
                conn.Open();
                string query = "insert into public.t_shipping(c_firstname, c_lastname, c_address, c_city, c_state, c_postal_code, c_country, c_phone, c_email_address, c_shipping_priority, c_shipping_date, c_user_id, c_shipping_total) values(@c_firstname, @c_lastname, @c_address, @c_city, @c_state, @c_postal_code, @c_country, @c_phone, @c_email_address, @c_shipping_priority, @c_shipping_date, @c_user_id, @c_total)";
                var command = new NpgsqlCommand(query, conn);
                command.Parameters.AddWithValue("@c_firstname", checkoutModel.c_firstname);
                command.Parameters.AddWithValue("@c_lastname", checkoutModel.c_lastname);
                command.Parameters.AddWithValue("@c_address", checkoutModel.c_address);
                command.Parameters.AddWithValue("@c_city", checkoutModel.c_city);
                command.Parameters.AddWithValue("@c_state", checkoutModel.c_state);
                command.Parameters.AddWithValue("@c_postal_code", checkoutModel.c_postal_code);
                command.Parameters.AddWithValue("@c_country", checkoutModel.c_country);
                command.Parameters.AddWithValue("@c_phone", checkoutModel.c_phone);
                command.Parameters.AddWithValue("@c_email_address", checkoutModel.c_email_address);
                command.Parameters.AddWithValue("@c_shipping_priority", checkoutModel.c_shipping_priority);
                command.Parameters.AddWithValue("@c_shipping_date", checkoutModel.c_shipping_date);
                var userid = accessor.HttpContext.Session.GetInt32("userid");
                command.Parameters.AddWithValue("@c_user_id", userid);
                command.Parameters.AddWithValue("@c_total", accessor.HttpContext.Session.GetString("total"));

                command.ExecuteNonQuery();

                string query2 = "update public.t_cart set c_checkedout = true where c_user_id = @c_user_id";
                var command2 = new NpgsqlCommand(query2, conn);
                command2.Parameters.AddWithValue("@c_user_id", userid);
                command2.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                conn.Close();
            }
        }
        public void RemoveFromCart(int id)
        {
            try
            {
                conn.Open();
                string query = "update public.t_cart set c_checkedout = true where c_id = @c_id";
                var command = new NpgsqlCommand(query, conn);
                command.Parameters.AddWithValue("@c_id", id);
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        public List<CartModel> GetAllCart()
        {
            List<CartModel> cart = new List<CartModel>();
            var userid = accessor.HttpContext.Session.GetInt32("userid");
            try
            {
                conn.Open();
                string query = "SELECT c.c_id ,a.c_title,  a.c_album , a.c_price , c.c_quantity , c.c_total ,c.c_user_id FROM t_album a JOIN t_cart c ON a.c_id = c.c_album_id where c.c_user_id = @c_user_id and c.c_checkedout = false";
                var command = new NpgsqlCommand(query, conn);
                command.Parameters.AddWithValue("@c_user_id", userid);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    CartModel cartModel = new CartModel();
                    cartModel.c_id = reader.GetInt32(0);
                    cartModel.c_title = reader.GetString(1);
                    cartModel.c_album_art = reader.GetString(2);
                    cartModel.c_price = reader.GetString(3);
                    cartModel.c_quantity = reader.GetInt32(4);
                    cartModel.c_total = reader.GetString(5);
                    cartModel.c_user_id = reader.GetInt32(6);
                    cart.Add(cartModel);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                conn.Close();
            }
            return cart;
        }

        public void AddToCart(CartModel cartModel)
        {
            try
            {
                conn.Open();
                string query = "insert into public.t_cart(c_album_id, c_price, c_quantity, c_total, c_user_id,c_checkedout) values(@c_album_id, @c_price, @c_quantity, @c_total, @c_user_id,false)";
                var command = new NpgsqlCommand(query, conn);
                command.Parameters.AddWithValue("@c_album_id", cartModel.c_id);
                command.Parameters.AddWithValue("@c_price", cartModel.c_price);
                command.Parameters.AddWithValue("@c_quantity", cartModel.c_quantity);
                command.Parameters.AddWithValue("@c_total", cartModel.c_total);
                var userid = accessor.HttpContext.Session.GetInt32("userid");
                command.Parameters.AddWithValue("@c_user_id", userid);
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                conn.Close();
            }
        }

    }
}