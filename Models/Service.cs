using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;

namespace u21635618HW05.Models
{
    public class Service
    { 
        private String ConnectionString;

        public Service()
        {
            
            ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

       
        public List<Students> SearchStudent(string name, string _class)
        {
            List<Students> students = new List<Students>();
            using (SqlConnection myConnection = new SqlConnection(ConnectionString))
            {
                myConnection.Open();
                string query = "";

                //if the search bar for the class is empty
                if (_class != "none" || _class != "" || _class != null)
                {
                    query = "Select * from students " +
                    "Where class Like '%" + _class + "%'";
                }

               //if the search bar for the name of the student and the class are not empty
                if (name != null && _class != null && name != "" && name != "none" && _class != "" && _class != "none")
                {
                    query = "Select * from students" + " Where class Like '%" + _class + "%' AND name Like '%" + name + "%'";
                }

                //if the class is empty, it will use the name of the student to search
                if (_class == "none")
                {
                    query = "Select * from students "
                    + "Where name Like '%" + name + "%'";
                }
                using (SqlCommand cmd = new SqlCommand(query, myConnection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            Students student = new Students
                            {
                                Id = Convert.ToInt32(reader["studentId"]),
                                Name = reader["name"].ToString(),
                                Surname = reader["surname"].ToString(),
                                Class = reader["class"].ToString(),
                                Points = Convert.ToInt32(reader["point"])

                            };
                            students.Add(student);
                        }

                    }
                }
                myConnection.Close();
            }

            return students;
        }

       
        public List<Books> Search(string name, int type, int author)
        {
            string innerJoin = "";
           //uses author to conduct search
            if (name == null && type == 0 && author > 0)
            {
                innerJoin =
                " select books.bookId as ID, books.pagecount as PageCount, books.point as Points, books.name as Name, types.name as Type, authors.surname as Author  from Books " +
                " inner join authors on books.authorId = authors.authorId " +
                " inner join types on books.typeId = types.typeId " +
                " where books.authorId = " + author + "";
            }

            //uses bookname to conduct search
            if (name != null && type == 0 && author == 0)
            {
                innerJoin =
                " select books.bookId as ID, books.pagecount as PageCount, books.point as Points, books.name as Name, types.name as Type, authors.surname as Author  from Books " +
                " inner join authors on books.authorId = authors.authorId " +
                " inner join types on books.typeId = types.typeId " +
                " where books.name LIKE '%" + name + "%'";
            }
            //uses bookname and auhor to conduct search
            if (name != null && author > 0)
            {
                innerJoin =
                    " select books.bookId as ID, books.pagecount as PageCount, books.point as Points, books.name as Name, types.name as Type, authors.surname as Author  from Books " +
                " inner join authors on books.authorId = authors.authorId " +
                " inner join types on books.typeId = types.typeId " +
                " where books.authorId = " + author + " AND books.name LIKE '%" + name + "%'";
            }

           //uses the booktype and author to conduct search
            if (type > 0 && author > 0)
            {
                innerJoin =
                " select books.bookId as ID, books.pagecount as PageCount, books.point as Points, books.name as Name, types.name as Type, authors.surname as Author  from Books " +
                " inner join authors on books.authorId = authors.authorId " +
                " inner join types on books.typeId = types.typeId " +
                " where books.typeId = " + type + " AND books.authorId = " + author;
            }

           //uses the booktype and bookname to conduct the search
            if (type > 0 && name != null)
            {
                innerJoin =
                " select books.bookId as ID, books.pagecount as PageCount, books.point as Points, books.name as Name, types.name as Type, authors.surname as Author  from Books " +
                " inner join authors on books.authorId = authors.authorId " +
                " inner join types on books.typeId = types.typeId " +
                " where books.typeId = " + type + " AND books.name LIKE '%" + name + "%'";
            }
            //uses the booktype,bookname and the author to conduct the search
            if (type > 0 && author > 0 && name != null)
            {
                innerJoin =
                " select books.bookId as ID, books.pagecount as PageCount, books.point as Points, books.name as Name, types.name as Type, authors.surname as Author  from Books " +
                " inner join authors on books.authorId = authors.authorId " +
                " inner join types on books.typeId = types.typeId " +
                " where books.typeId = " + type + " AND books.name LIKE '%" + name + "%'" + " AND  books.authorId = " + author;
            }

       //read data from database
            List<Books> books = new List<Books>();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand(innerJoin, con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Books book = new Books
                            {
                                ID = Convert.ToInt32(reader["ID"]),
                                Name = reader["Name"].ToString(),
                                Author = reader["Author"].ToString(),
                                PageCount = Convert.ToInt32(reader["PageCount"]),
                                Points = Convert.ToInt32(reader["Points"]),
                                Types = reader["Type"].ToString()
                            };
                            books.Add(book);
                        }
                    }
                }
                con.Close();
            }

            return books;
        }
       
        public List<BorrowedBook> GetAllBorrowedBooks(int id = 0)
        {
            
            List<BorrowedBook> borrowedBooks = new List<BorrowedBook>();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
               
                string innerJoin =
                    " Select CONCAT( students.name,' ',students.surname) as Student, takenDate, broughtDate, borrows.bookId ,  borrows.borrowId from students " +
                    " inner join borrows on students.studentId = borrows.studentId " +
                    " inner join books on books.bookId = borrows.bookId " +
                    "where borrows.bookId = " + id;
                if (id == 0)
                {
                    innerJoin =
                    " Select CONCAT( students.name,' ',students.surname) as Student, takenDate, broughtDate, borrows.bookId ,  borrows.borrowId from students " +
                    " inner join borrows on students.studentId = borrows.studentId " +
                    " inner join books on books.bookId = borrows.bookId ";
                }

                using (SqlCommand cmd = new SqlCommand(innerJoin, con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            BorrowedBook book = new BorrowedBook
                            {
                                BookID = Convert.ToInt32(reader["bookId"]),
                                BorrowID = Convert.ToInt32(reader["borrowId"]),
                                BroughtDate = reader["broughtDate"].ToString(),
                                TakenDate = reader["takenDate"].ToString(),
                                StudentName = reader["student"].ToString(),
                            };
                            borrowedBooks.Add(book);
                        }
                    }
                }
                con.Close();
            }


            return borrowedBooks;
        }

        //
        public void BorrowBook(int bookId, int studentId)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                string query = "insert into borrows( studentId, bookId, takenDate) " +
                    "values(@studentId,@bookId,@takenDate) ";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {

                    cmd.Parameters.Add(new SqlParameter("@studentId", studentId));
                    cmd.Parameters.Add(new SqlParameter("@bookId", bookId));
                    cmd.Parameters.Add(new SqlParameter("@takenDate", DateTime.Now));
                    cmd.ExecuteNonQuery();
                }
                con.Close();
            }

            GetAllStudents().Where(s => s.Id == studentId).FirstOrDefault().Book = true;

        }

        public void ReturnBook(int bookId, int studentId)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                string query = "update borrows set broughtDate = @broughtDate where borrows.studentId = @studentId  AND borrows.bookId = @bookId and broughtDate IS NULL";
                ;
                using (SqlCommand cmd = new SqlCommand(query, con))
                {

                    cmd.Parameters.Add(new SqlParameter("@studentId", studentId));
                    cmd.Parameters.Add(new SqlParameter("@bookId", bookId));
                    cmd.Parameters.Add(new SqlParameter("@broughtDate", DateTime.Now));
                    cmd.ExecuteNonQuery();
                }
                con.Close();
            }

        }
 
        public List<Authors> GetAllAuthors()
        {
            List<Authors> authors = new List<Authors>();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("select * from Authors", con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Authors author = new Authors
                            {
                                AuthorID = Convert.ToInt32(reader["authorId"]),
                                Name = reader["name"].ToString(),
                                Surname = reader["surname"].ToString()
                            };
                            authors.Add(author);
                        }
                    }
                }
                con.Close();
            }
            return authors;

        }

        public List<Types> GetAllTypes()
        {
            List<Types> types = new List<Types>();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("select * from types", con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Types type = new Types
                            {
                                ID = Convert.ToInt32(reader["typeId"]),
                                Name = reader["name"].ToString(),

                            };
                            types.Add(type);
                        }
                    }
                }
                con.Close();
            }
            return types;

        }
        
        public List<Students> GetAllStudents()
        {
            List<Students> students = new List<Students>();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("select * from students", con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Students student = new Students
                            {
                                Id = Convert.ToInt32(reader["studentId"]),
                                Name = reader["name"].ToString(),
                                Surname = reader["surname"].ToString(),
                                Class = reader["class"].ToString(),
                                Points = Convert.ToInt32(reader["point"])

                            };
                            students.Add(student);
                        }
                    }
                }
                con.Close();
            }
            return students;

        }

      
        public List<Options> GetAllClasses()
        {
            List<Options> classes = new List<Options>();
            foreach (Students student in GetAllStudents())
            {
                Options cl = new Options
                {
                    Name = student.Class
                };
                if (classes.Where(n => n.Name == student.Class).Count() == 0)
                {
                    classes.Add(cl);
                }
            }
            return classes;
        }

      
        public List<Books> GetAllBooks()
        {
          //read the books from the database
            List<Books> books = new List<Books>();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                
                string innerJoin =
                    " select books.bookId as ID, books.pagecount as PageCount, books.point as Points, books.name as Name, types.name as Type, authors.surname as Author  from Books " +
                    " inner join authors on books.authorId = authors.authorId " +
                    " inner join types on books.typeId = types.typeId ";

                using (SqlCommand cmd = new SqlCommand(innerJoin, con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Books book = new Books
                            {
                                ID = Convert.ToInt32(reader["ID"]),
                                Name = reader["Name"].ToString(),
                                Author = reader["Author"].ToString(),
                                PageCount = Convert.ToInt32(reader["PageCount"]),
                                Points = Convert.ToInt32(reader["Points"]),
                                Types = reader["Type"].ToString(),
                            };
                            books.Add(book);
                        }
                    }
                }
                con.Close();
            }

            //to check whether the books is available or its unavailable
            foreach (var book in books)
            {
              
                List<BorrowedBook> borrowedBooks = GetAllBorrowedBooks(book.ID);
              
                if (borrowedBooks.Where(b => b.BroughtDate == "").Count() == 1)
                {
                    book.Status = "Book Out";
                }
                else
                {
                    book.Status = "Available";
                }
            }
            return books;
        }
    }
}