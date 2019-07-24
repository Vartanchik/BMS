using AutoMapper;
using BMS.BLL.DTOs;
using BMS.DAL.Entities;
using BMS.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BMS.BLL.Services.CommentService
{
    public class CommentService : ICommentService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        private readonly ILogger<CommentService> _logger;

        public CommentService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CommentService> logger)
        {
            _uow = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CommentDto> GetAsync(int commentId)
        {
            try
            {
                var comment = await _uow.Comments.GetByIdAsync(commentId);

                return _mapper.Map<CommentDto>(comment);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{this.ToString()} - error message:{ex.Message}");

                return null;
            }
        }

        public async Task<ServiceResult> CreateAsync(CommentDto commentDto)
        {
            try
            {
                // Checking if post exist
                var postExist = await _uow.Posts.AsQueryable()
                                                .AnyAsync(p => p.Id == commentDto.PostId &&
                                                               p.AuthorId == commentDto.AuthorId);

                if (!postExist) return new ServiceResult($"Post with id: {commentDto.PostId} - not found.");

                // Comment creating
                var comment = _mapper.Map<Comment>(commentDto);

                await _uow.Comments.CreateAsync(comment);

                await _uow.CommitAsync();

                return new ServiceResult();
            }
            catch (Exception ex)
            {
                _logger.LogError($"{this.ToString()} - error message:{ex.Message}");

                return new ServiceResult(ex.Message);
            }
        }

        public async Task<ServiceResult> UpdateAsync(CommentDto commentDto)
        {
            try
            {
                // Checking if comment exist
                var commentExist = await _uow.Comments.AsQueryable()
                                                      .AnyAsync(c => c.Id == commentDto.Id &&
                                                                     c.AuthorId == commentDto.AuthorId);

                if (!commentExist) return new ServiceResult($"Comment with id: {commentDto.Id} - not found.");

                // Comment updating
                var comment = _mapper.Map<Comment>(commentDto);

                await _uow.Comments.UpdateAsync(comment);

                await _uow.CommitAsync();

                return new ServiceResult();
            }
            catch (Exception ex)
            {
                _logger.LogError($"{this.ToString()} - error message:{ex.Message}");

                return new ServiceResult(ex.Message);
            }
        }

        public async Task<ServiceResult> DeleteAsync(CommentDto commentDto)
        {
            try
            {
                // Checking if comment exist
                var commentExist = await _uow.Comments.AsQueryable()
                                                      .AnyAsync(c => c.Id == commentDto.Id &&
                                                                     c.AuthorId == commentDto.AuthorId);

                if (!commentExist) return new ServiceResult($"Comment with id: {commentDto.Id} - not found.");
                
                // Comment deleting
                await _uow.Comments.DeleteAsync(commentDto.Id);

                await _uow.CommitAsync();

                return new ServiceResult();
            }
            catch (Exception ex)
            {
                _logger.LogError($"{this.ToString()} - error message:{ex.Message}");

                return new ServiceResult(ex.Message);
            }
        }
    }
}
