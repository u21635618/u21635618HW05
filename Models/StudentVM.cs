using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace u21635618HW05.Models
{
    public class StudentVM
    {

        public List<Students> Students { get; set; }
        public Books Book { get; set; }
        public List<Options> Options { get; set; }
    }
}