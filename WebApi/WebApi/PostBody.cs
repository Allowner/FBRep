using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi
{
    public class PostBody
    {
        public string FilePath { get; set; }
        public string Caption { get; set; }
        public string DescriptionAndHashtags { get; set; }
    }
}
