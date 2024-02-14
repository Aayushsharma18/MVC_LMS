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
    public class BooksController : Controller
    {
        private readonly LibrarysContext _context;
        private readonly ILogger<BooksController> _logger;

        public BooksController(ILogger<BooksController> logger, LibrarysContext context)
        {
            _logger = logger;
            _context = context;
        }

        // [Route("/Index")]
        public IActionResult Index()
        {
            List<Book> GetAllBooks = _context.Books.FromSqlRaw("getBookDetails").ToList();

            return View(GetAllBooks);
        }

        public IActionResult GetBookById(int? id)
        {
            Book? userById = _context.Books.FromSqlRaw($"getBookDetailsById {id}").AsEnumerable().FirstOrDefault();
            return View("GetBookById", userById);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateBooks(int id, string title, int isbn)
        {
            SqlParameter[] param =
            [
                new(){ ParameterName="@id",SqlDbType=System.Data.SqlDbType.VarChar,Value=id},
                new(){ ParameterName="@title",SqlDbType=System.Data.SqlDbType.VarChar,Value=title},
                new(){ ParameterName="@isbn",SqlDbType=System.Data.SqlDbType.Int,Value=isbn}
            ];

            int updateLib = await _context.Database.ExecuteSqlRawAsync($"exec updateBook @id,@title,@isbn", param);

            return View("UpdateBooks", updateLib);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrEdit(int id, string title, int isbn)
        {
            SqlParameter[] param =
            [
                new(){ ParameterName="@id", SqlDbType=System.Data.SqlDbType.VarChar, Value=id},
                new(){ ParameterName="@title", SqlDbType=System.Data.SqlDbType.VarChar, Value=title},
                new(){ ParameterName="@isbn", SqlDbType=System.Data.SqlDbType.VarChar, Value=isbn}
            ];

            if (id == 0)
            {
                _ = await _context.Database.ExecuteSqlRawAsync("exec insertBooks @title, @isbn", param);
            }
            else
            {
                _ = await _context.Database.ExecuteSqlRawAsync("exec updateBook @id, @title, @isbn", param);
            }

            return RedirectToAction(nameof(Index));
        }


        [HttpDelete, ActionName("DeleteBookById")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteBookById(int id)
        {
            try
            {
                Book? Book = await _context.Books.FindAsync(id);
                if (Book == null)
                {
                    return View("NotFound");
                }

                _ = _context.Books.Remove(Book);
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
            return View("Error!");
        }
    }
}