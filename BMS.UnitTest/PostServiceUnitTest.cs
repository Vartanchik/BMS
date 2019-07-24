using AutoMapper;
using BMS.BLL.DTOs;
using BMS.BLL.Services;
using BMS.BLL.Services.PostService;
using BMS.DAL;
using BMS.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Xunit;

namespace BMS.UnitTest
{
    public class PostServiceUnitTest
    {
        private IPostService _postService;

        [Fact]
        public async Task CreatePost_IsSuccessful()
        {
            _postService = GetPostService();

            var newPost = new PostDto
            {
                Id = 1,
                Title = "Test Post Title",
                Content = "Test Post Content",
                AuthorId = 1
            };

            var result = await _postService.CreateAsync(newPost);

            var actualPost = await _postService.GetAsync(1);

            Assert.True(result.Succeeded);
            Assert.Equal(newPost.Id, actualPost.Id);
            Assert.Equal(newPost.Title, actualPost.Title);
            Assert.Equal(newPost.Content, actualPost.Content);
            Assert.Equal(newPost.AuthorId, actualPost.AuthorId);
        }

        private IPostService GetPostService()
        {
            var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            var context = new ApplicationDbContext(contextOptions);

            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile<DtoMapProfile>());

            var mapper = new Mapper(mapperConfig);

            var logger = new Logger<PostService>(new LoggerFactory());

            return new PostService(new UnitOfWork(context, null), mapper, logger);
        }
    }
}
