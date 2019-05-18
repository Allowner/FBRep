using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostApp.Models
{
    public class PostBody
    {
        public string PageNameOrId { get; set; }
        public string PageAccessToken { get; set; }
        public string FilePath { get; set; }
        public string DescriptionAndHashtags { get; set; }
    }
}
