﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace u21635618HW05.Models
{
    public class Books
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int PageCount { get; set; }
        public int Points { get; set; }
        public string Author { get; set; }
        public string Status { get; set; }
        public string Types { get; set; }
    }
}