using System.Threading.Tasks;
using AutoMapper;
using BMS.BLL.DTOs;
using BMS.BLL.Services.PostService;
using BMS.WebAPI.Models;
using BMS.WebAPI.Utility.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BMS.WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Post")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly IMapper _mapper;

        public PostController(IPostService postService, IMapper mapper)
        {
            _postService = postService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get a post by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(PostDto), 200)]
        [ProducesResponseType(204)]
        public async Task<ActionResult<PostDto>> Get([FromRoute] int id)
        {
            var post = await _postService.GetAsync(id);

            return post == null
                ? (ActionResult)NoContent()
                : Ok(post);
        }

        /// <summary>
        /// Create a new post
        /// </summary>
        /// <param name="postModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(typeof(ResponseModel), 200)]
        [ProducesResponseType(typeof(ResponseModel), 400)]
        public async Task<ActionResult> Create([FromBody] PostCreateModel postModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseModel(400, "Invalid value was entered! Please, redisplay form."));
            }

            var postDto = _mapper.Map<PostDto>(postModel);

            postDto.AuthorId = this.GetCurrentUserId();

            var result = await _postService.CreateAsync(postDto);

            return result.Succeeded
                ? Ok(new ResponseModel(200, "Completed.", "Post created."))
                : (ActionResult)BadRequest(new ResponseModel(400, "Failed.", result.Error));
        }

        /// <summary>
        /// Update a post
        /// </summary>
        /// <param name="postModel"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(typeof(ResponseModel), 200)]
        [ProducesResponseType(typeof(ResponseModel), 400)]
        public async Task<ActionResult> Update([FromBody] PostUpdateModel postModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseModel(400, "Invalid value was entered! Please, redisplay form."));
            }

            var postDto = _mapper.Map<PostDto>(postModel);

            postDto.AuthorId = this.GetCurrentUserId();

            var result = await _postService.UpdateAsync(postDto);

            return result.Succeeded
                ? Ok(new ResponseModel(200, "Completed.", "Post updated."))
                : (ActionResult)BadRequest(new ResponseModel(400, "Failed.", result.Error));
        }

        /// <summary>
        /// Delete a post
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(typeof(ResponseModel), 200)]
        [ProducesResponseType(typeof(ResponseModel), 400)]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var postDto = new PostDto
            {
                Id = id,
                AuthorId = this.GetCurrentUserId()
            };

            var result = await _postService.DeleteAsync(postDto);

            return result.Succeeded
                ? Ok(new ResponseModel(200, "Completed.", "Post deleted."))
                : (ActionResult)BadRequest(new ResponseModel(400, "Failed.", result.Error));
        }
    }
}