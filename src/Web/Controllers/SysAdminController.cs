using Application.Interfaces;
using Application.Models;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SysAdminController : ControllerBase
    {
        private readonly ISysAdminService _sysAdminService;
        public SysAdminController(ISysAdminService sysAdminService) 
        { 
            _sysAdminService = sysAdminService;
        }
       
        [HttpPost("[action]")]
        public ActionResult CreateUser([FromBody] User user)
        {
            try 
            {
                _sysAdminService.CreateUser(user);
               return NoContent();
            }catch (Exception ex) 
            {
               return NotFound(ex.Message);
            }
        
        }

        [HttpPut("{id}")]
        public ActionResult UpadateUser([FromRoute] int id)
        {
            try
            {
                _sysAdminService.UpdateUser(id);
                return NoContent();
            }catch (Exception ex) 
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteUser([FromRoute] int id)
        {
            try
            {
                _sysAdminService.DeleteUser(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<UserDto> GetUserById(int id) 
        {
            try
            {
               var user=  _sysAdminService.GetUserById(id);
                return Ok(user);    
            }
            catch(Exception ex) 
            { 
                return NotFound(ex.Message);
            }
        }

        [HttpGet("[action]")]
        public ActionResult<IEnumerable<UserDto>> GetUsersAll() 
        {
            var userAll = _sysAdminService.GetAllUsers();
            return Ok(userAll);
        }
    }
}
