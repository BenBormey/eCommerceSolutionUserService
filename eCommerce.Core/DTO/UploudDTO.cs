using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.DTO
{
    public class UploudDTO
    {
   public IFormFile image { get; set; } = default!;
    }
}
