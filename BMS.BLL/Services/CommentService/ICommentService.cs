using BMS.BLL.DTOs;
using System.Threading.Tasks;

namespace BMS.BLL.Services.CommentService
{
    public interface ICommentService
    {
        Task<CommentDto> GetAsync(int commentId);
        Task<ServiceResult> CreateAsync(CommentDto commentDto);
        Task<ServiceResult> UpdateAsync(CommentDto commentDto);
        Task<ServiceResult> DeleteAsync(CommentDto commentDto);
    }
}
