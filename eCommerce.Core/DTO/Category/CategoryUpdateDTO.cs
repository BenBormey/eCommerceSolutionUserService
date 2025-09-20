using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.DTO.Category
{
    public class CategoryUpdateDTO
    {
        public Guid CategoryId { get; set; }   // identify which category to update
        public string Name { get; set; } = default!;
        public string? Description { get; set; }

    }
}
