using eCommerce.Core.DTO.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.ServiceContracts
{
    public interface ICategoryService
    {
        Task<CategoryDTO> CreateCategoryAsync(CategoryCreateDTO dto);

        Task<IReadOnlyList<CategoryDTO>> GetAllCategoriesAsync();


        Task<CategoryDTO?> GetCategoryByIdAsync(Guid categoryId);

 
        Task<CategoryDTO?> UpdateCategoryAsync(CategoryUpdateDTO dto);


        Task<bool> DeleteCategoryAsync(Guid categoryId);

    }
}
