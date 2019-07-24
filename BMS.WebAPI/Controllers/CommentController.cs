using System.Threading.Tasks;
using AutoMapper;
using BMS.BLL.DTOs;
using BMS.BLL.Services.CommentService;
using BMS.WebAPI.Models;
using BMS.WebAPI.Utility.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BMS.WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly IMapper _mapper;

        public CommentController(ICommentService commentService, IMapper mapper)
        {
            _commentService = commentService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get a comment by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(CommentDto), 200)]
        [ProducesResponseType(204)]
        public async Task<ActionResult<CommentDto>> Get([FromRoute] int id)
        {
            var comment = await _commentService.GetAsync(id);

            return comment == null
                ? (ActionResult)NoContent()
                : Ok(comment);
        }

        /// <summary>
        /// Create a new comment
        /// </summary>
        /// <param name="commentModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(typeof(ResponseModel), 200)]
        [ProducesResponseType(typeof(ResponseModel), 400)]
        public async Task<ActionResult> Create([FromBody] CommentCreateModel commentModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseModel(400, "Invalid value was entered! Please, redisplay form."));
            }

            var commentDto = _mapper.Map<CommentDto>(commentModel);

            commentDto.AuthorId = this.GetCurrentUserId();

            var result = await _commentService.CreateAsync(commentDto);

            return result.Succeeded
                ? Ok(new ResponseModel(200, "Completed.", "Comment created."))
                : (ActionResult)BadRequest(new ResponseModel(400, "Failed.", result.Error));
        }

        /// <summary>
        /// Update a comment
        /// </summary>
        /// <param name="commentModel"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(typeof(ResponseModel), 200)]
        [ProducesResponseType(typeof(ResponseModel), 400)]
        public async Task<ActionResult> Update([FromBody] CommentUpdateModel commentModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseModel(400, "Invalid value was entered! Please, redisplay form."));
            }

            var commentDto = _mapper.Map<CommentDto>(commentModel);

            commentDto.AuthorId = this.GetCurrentUserId();

            var result = await _commentService.UpdateAsync(commentDto);

            return result.Succeeded
                ? Ok(new ResponseModel(200, "Completed.", "Comment updated."))
                : (ActionResult)BadRequest(new ResponseModel(400, "Failed.", result.Error));
        }

        /// <summary>
        /// Delete a comment
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(typeof(ResponseModel), 200)]
        [ProducesResponseType(typeof(ResponseModel), 400)]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var commentDto = new CommentDto
            {
                Id = id,
                AuthorId = this.GetCurrentUserId()
            };

            var result = await _commentService.DeleteAsync(commentDto);

            return result.Succeeded
                ? Ok(new ResponseModel(200, "Completed.", "Comment deleted."))
                : (ActionResult)BadRequest(new ResponseModel(400, "Failed.", result.Error));
        }
    }
}