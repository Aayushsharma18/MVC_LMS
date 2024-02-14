using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using firstmvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace firstmvc.Controllers
{
    [Route("[controller]/[action]")]
    public class BookingController(LibrarysContext context, ILogger<BookingController> logger) : Controller
    {
        private readonly LibrarysContext _context = context;
        private readonly ILogger<BookingController> _logger = logger;


        // [Route("/Index")]
        public IActionResult Index()
        {
            List<BorrowedBook> getBorrowedBooks = _context.BorrowedBooks.FromSqlRaw("getBorrowedBooksDetails").ToList();

            return View(getBorrowedBooks);
        }

        public IActionResult GetBorrowedBookById(int? id)
        {
            BorrowedBook? userById = _context.BorrowedBooks.FromSqlRaw($"getBorrowedBooksById {id}").AsEnumerable().FirstOrDefault();
            return View("GetBorrowedBookById", userById);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateBorrowedBooks(int id, string userid, string bookid, DateTime bdate, DateTime rdate)
        {
            SqlParameter[] param =
            [
                new(){ ParameterName="@id",SqlDbType=System.Data.SqlDbType.VarChar,Value=id},
                new(){ ParameterName="@userid",SqlDbType=System.Data.SqlDbType.VarChar,Value=userid},
                new(){ ParameterName="@bookid",SqlDbType=System.Data.SqlDbType.VarChar,Value=bookid},
                new(){ ParameterName="@bdate",SqlDbType=System.Data.SqlDbType.DateTime,Value=bdate},
                new(){ ParameterName="@rdate",SqlDbType=System.Data.SqlDbType.DateTime,Value=rdate}
            ];

            int updateLib = await _context.Database.ExecuteSqlRawAsync($"exec updateBorrowedBook @id,@userid,@bookid,@bdate,@rdate", param);

            return View("UpdateBorrowedBooks", updateLib);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrEdit(int id, string userid, string bookid, DateTime bdate, DateTime rdate)
        {
            SqlParameter[] param =
            [
                new(){ ParameterName="@id", SqlDbType=System.Data.SqlDbType.VarChar, Value=id},
                new(){ ParameterName="@userid", SqlDbType=System.Data.SqlDbType.VarChar, Value=userid},
                new(){ ParameterName="@bookid", SqlDbType=System.Data.SqlDbType.VarChar, Value=bookid},
                new(){ ParameterName="@bdate", SqlDbType=System.Data.SqlDbType.DateTime, Value=bdate},
                new(){ ParameterName="@rdate", SqlDbType=System.Data.SqlDbType.DateTime, Value=rdate}
            ];

            if (id == 0)
            {
                _ = await _context.Database.ExecuteSqlRawAsync("exec insertBorrowedBook @userid, @bookid, @bdate, @rdate", param);
            }
            else
            {
                _ = await _context.Database.ExecuteSqlRawAsync("exec updateBorrowedBook @id, @userid, @bookid, @bdate, @rdate", param);
            }

            return RedirectToAction(nameof(Index));
        }


        [HttpDelete, ActionName("DeleteBorrowedBookById")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteBorrowedBookById(int id)
        {
            try
            {
                BorrowedBook? borrowedBook = await _context.BorrowedBooks.FindAsync(id);
                if (borrowedBook == null)
                {
                    return View("NotFound");
                }

                _ = _context.BorrowedBooks.Remove(borrowedBook);
                _ = await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return View("Error");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error");
        }
    }
}