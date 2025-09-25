using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.Entities
{
    public class Category
    {
        public Guid CategoryId { get;set; }
        public string CategoryName { get;set; } 
        public string CategoryDescription { get;set; }
        public DateTime CreateAt { get;set; }   
        public DateTime UpdateAt { get;set; }
        public IEnumerable<Services> services { get; set; }
    }
}
