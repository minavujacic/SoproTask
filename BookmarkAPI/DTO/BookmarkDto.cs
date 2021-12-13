using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dto
{
    public class BookmarkDto
    {
        public string URL { get; set; }

        public string ShortDescription { get; set; }

        public int? CategoryId { get; set; }
    }
}
