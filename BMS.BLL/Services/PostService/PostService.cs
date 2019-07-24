using System;
using System.Threading.Tasks;
using AutoMapper;
using BMS.BLL.DTOs;
using BMS.DAL.Entities;
using BMS.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BMS.BLL.Services.PostService
{
    public class PostService : IPostService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        private readonly ILogger<PostService> _logger;

        public PostService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<PostService> logger)
        {
            _uow = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PostDto> GetAsync(int postId)
        {
            try
            {
                var post = await _uow.Posts.GetByIdAsync(postId);

                return _mapper.Map<PostDto>(post);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{this.ToString()} - error message:{ex.Message}");

                return null;
            }
        }

        public async Task<ServiceResult> CreateAsync(PostDto postDto)
        {
            try
            {
                var post = _mapper.Map<Post>(postDto);

                await _uow.Posts.CreateAsync(post);

                await _uow.CommitAsync();

                return new ServiceResult();
            }
            catch (Exception ex)
            {
                _logger.LogError($"{this.ToString()} - error message:{ex.Message}");

                return new ServiceResult(ex.Message);
            }
        }

        public async Task<ServiceResult> UpdateAsync(PostDto postDto)
        {
            try
            {
                // Checking if post exist
                var postExist = await _uow.Posts.AsQueryable()
                                                .AnyAsync(p => p.Id == postDto.Id &&
                                                          p.AuthorId == postDto.AuthorId);

                if (!postExist) return new ServiceResult($"Post with id: {postDto.Id} - not found.");

                // Post updating
                var post = _mapper.Map<Post>(postDto);

                await _uow.Posts.UpdateAsync(post);

                await _uow.CommitAsync();

                return new ServiceResult();
            }
            catch (Exception ex)
            {
                _logger.LogError($"{this.ToString()} - error message:{ex.Message}");

                return new ServiceResult(ex.Message);
            }
        }

        public async Task<ServiceResult> DeleteAsync(PostDto postDto)
        {
            try
            {
                // Checking if post exist
                var postExist = await _uow.Posts.AsQueryable()
                                                .AnyAsync(p => p.Id == postDto.Id &&
                                                               p.AuthorId == postDto.AuthorId);

                if (!postExist) return new ServiceResult($"Post with id: {postDto.Id} - not found.");

                // Post deleting
                await _uow.Posts.DeleteAsync(postDto.Id);

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
