using Blog.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Core.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Tiitle { get; set; }
        public CatgStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime PublishDate { get; set; }

        public string Category { get; set; }

    }
}
