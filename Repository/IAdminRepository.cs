using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KENDO_PRACTICE.Models;

namespace KENDO_PRACTICE.Repository
{
    public interface IAdminRepository
    {
        List<AlbumModel> GetAlbums();
        void AddAlbum(AlbumModel albumModel);
        void UpdateAlbum(AlbumModel albumModel);
        void DeleteAlbum(int id);
        AlbumModel GetAlbumById(int id);
        bool DeleteMultipleStudents(List<int> studentIds);
        List<GraphModel> GetGenre();
        List<RevenueModel> GetRevenue();
        List<RevenueModel> GetWeeklyRevenue();
        List<RevenueModel> GetMonthRevenue();


        void CheckoutDetails(CheckoutModel checkoutModel);


        void RemoveFromCart(int id);

        List<CartModel> GetAllCart();

        void AddToCart(CartModel cartModel);


    }
}