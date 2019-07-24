using BMS.BLL.DTOs;
using System.Threading.Tasks;

namespace BMS.BLL.Services.PostService
{
    public interface IPostService
    {
        Task<PostDto> GetAsync(int postId);
        Task<ServiceResult> CreateAsync(PostDto postDto);
        Task<ServiceResult> UpdateAsync(PostDto postDto);
        Task<ServiceResult> DeleteAsync(PostDto postDto);
    }
}
