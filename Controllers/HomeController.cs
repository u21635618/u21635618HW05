using System;
using u21635618HW05.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace u21635618HW05.Controllers
{
    public class HomeController : Controller
    {
        
        private Service DataService = new Service();
        public ActionResult Index()
        {
           
            BooksVM booksVM = new BooksVM();
            booksVM.Books = DataService.GetAllBooks();
            booksVM.Authors = DataService.GetAllAuthors();
            booksVM.Types = DataService.GetAllTypes();
            return View(booksVM);
        }

        public ActionResult Books(int id)
        {
          
            BookDetsModel bookDetailsVM = new BookDetsModel();
            bookDetailsVM.BorrowedBooks = DataService.GetAllBorrowedBooks(id);
            bookDetailsVM.Book = DataService.GetAllBooks().Where(b => b.ID == id).FirstOrDefault();
            return View(bookDetailsVM);
        }
        public ActionResult Search(int type = 0, int author = 0, string name = null)
        {
         
            BooksVM booksVM = new BooksVM();
            booksVM.Books = DataService.Search(name, type, author);
            booksVM.Authors = DataService.GetAllAuthors();
            booksVM.Types = DataService.GetAllTypes();
            return View("Index", booksVM);
        }

        public ActionResult Students(int bookId)
        {
            
            StudentVM studentVM = new StudentVM();
            List<Students> students = DataService.GetAllStudents();
            List<BorrowedBook> books = DataService.GetAllBorrowedBooks(bookId);
            foreach (var student in students)
            {
                for (int i = 0; i < books.Count(); i++)
                {
                    string name = student.Name + " " + student.Surname;
                    if (books[i].StudentName == name && (books[i].BroughtDate == "" || books[i].BroughtDate == null))
                    {
                        student.Book = true;
                    }
                    else
                    {
                        student.Book = false;

                    }
                }
            }
            studentVM.Students = students;
            studentVM.Book = DataService.GetAllBooks().Where(b => b.ID == bookId).FirstOrDefault();
            studentVM.Options = DataService.GetAllClasses();
            return View(studentVM);
        }


        public ActionResult ReturnBook(int bookId, int studentId)
        {
            DataService.ReturnBook(bookId, studentId);

            BookDetsModel bookDetailsVM = new BookDetsModel();
            bookDetailsVM.BorrowedBooks = DataService.GetAllBorrowedBooks(bookId);
            bookDetailsVM.Book = DataService.GetAllBooks().Where(b => b.ID == bookId).FirstOrDefault();
            return View("Books", bookDetailsVM);

        }


        public ActionResult BorrowBook(int bookId, int studentId)
        {
            DataService.BorrowBook(bookId, studentId);
            BookDetsModel bookDetailsVM = new BookDetsModel();
            bookDetailsVM.BorrowedBooks = DataService.GetAllBorrowedBooks(bookId);
            bookDetailsVM.Book = DataService.GetAllBooks().Where(b => b.ID == bookId).FirstOrDefault();
            return View("Books", bookDetailsVM);
        }

        public ActionResult StudentSearch(int bookId, string name = "none", string _class = "none")
        {
            
            StudentVM studentVM = new StudentVM
            {
                Students = DataService.SearchStudent(name, _class),
                Book = DataService.GetAllBooks().Where(b => b.ID == bookId).FirstOrDefault(),
                Options = DataService.GetAllClasses()

            };
            return View("Students", studentVM);
        }
    }
}