﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Core.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Firsname { get; set; }
        public string Lastname { get; set; }
        public string Email{ get; set; }
        public string Password { get; set; }
    }
}
