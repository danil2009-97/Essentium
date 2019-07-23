using Microsoft.AspNetCore.Mvc;
using Project.Data.Dto;
using Project.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserRepository _repo;

        public UserController(IUserRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<List<UserDto>>> Get()
        {
            try
            {
                return Ok(await _repo.GetAllAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> Get(int id)
        {
            try
            {
                UserDto user = await _repo.GetByIdAsync(id);
                if (user == null)
                    return NotFound();
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpGet("login/{login}")]
        public async Task<ActionResult<UserDto>> Get(string login)
        {
            try
            {
                UserDto user = await _repo.GetByLoginAsync(login);
                if (user == null)
                    return NotFound();
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> Post([FromBody]UserDto item)
        {
            try
            {
                UserDto user = await _repo.CreateAsync(item);
                if (user == null)
                    return BadRequest();
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpPut]
        public async Task<ActionResult<bool>> Put([FromBody]UserDto item)
        {
            try
            {
                return await _repo.UpdateAsync(item);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            try
            {
                return await _repo.DeleteByIdAsync(id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpDelete("login/{login}")]
        public async Task<ActionResult<bool>> Delete(string login)
        {
            try
            {
                return await _repo.DeleteByLoginAsync(login);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}
