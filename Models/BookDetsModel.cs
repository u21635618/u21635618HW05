using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace u21635618HW05.Models
{
    public class BookDetsModel
    {
        public Books Book { get; set; }
        public List<BorrowedBook> BorrowedBooks { get; set; }
    }
}