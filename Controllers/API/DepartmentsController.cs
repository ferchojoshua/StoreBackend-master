using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Entities;
using Store.Helpers.ClientService;
using Store.Helpers.User;
using Store.Models.Responses;
using Store.Models.ViewModels;

namespace Store.Controllers.API
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly ILocationsHelper _locationsHelper;
        private readonly IUserHelper _userHelper;

        public DepartmentController(ILocationsHelper locationsHelper, IUserHelper userHelper)
        {
            _locationsHelper = locationsHelper;
            _userHelper = userHelper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Department>>> GetDepartments()
        {
            string email = User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
                .Value;
            User user = await _userHelper.GetUserByEmailAsync(email);
            if (user.IsDefaultPass)
            {
                return Ok(user);
            }
            if (!await _userHelper.IsAutorized(user.Rol, "COMMUNITIES VER"))
            {
                return Unauthorized();
            }

            string token = HttpContext.Request.Headers["Authorization"];
            token = token["Bearer ".Length..].Trim();
            if (user.UserSession.UserToken != token)
            {
                await _userHelper.LogoutAsync(user);
                return Ok("eX01");
            }
            try
            {
                var deptoList = await _locationsHelper.GetDepartmentListAsync();
                return Ok(deptoList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetDeptosWithMunCount")]
        public async Task<ActionResult<IEnumerable<Department>>> GetDeptosWithMunCount()
        {
            string email = User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
                .Value;
            User user = await _userHelper.GetUserByEmailAsync(email);
            if (user.IsDefaultPass)
            {
                return Ok(user);
            }
            if (!await _userHelper.IsAutorized(user.Rol, "COMMUNITIES VER"))
            {
                return Unauthorized();
            }

            string token = HttpContext.Request.Headers["Authorization"];
            token = token["Bearer ".Length..].Trim();
            if (user.UserSession.UserToken != token)
            {
                await _userHelper.LogoutAsync(user);
                return Ok("eX01");
            }
            try
            {
                List<DeptoResponse> dR = new();
                var deptoList = await _locationsHelper.GetDepartmentListAsync();

                foreach (var item in deptoList)
                {
                    var depto = await _locationsHelper.GetMunicipalitiesByDeptoAsync(item.Id);
                    dR.Add(
                        new DeptoResponse { Department = item, MunicipalitiesCount = depto.Count }
                    );
                }
                return Ok(dR);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetMunsWithCommsCountCount/{id}")]
        public async Task<ActionResult<IEnumerable<Municipality>>> GetMunsWithCommsCountCount(
            int id
        )
        {
            string email = User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
                .Value;
            User user = await _userHelper.GetUserByEmailAsync(email);
            if (user.IsDefaultPass)
            {
                return Ok(user);
            }
            if (!await _userHelper.IsAutorized(user.Rol, "COMMUNITIES VER"))
            {
                return Unauthorized();
            }

            string token = HttpContext.Request.Headers["Authorization"];
            token = token["Bearer ".Length..].Trim();
            if (user.UserSession.UserToken != token)
            {
                await _userHelper.LogoutAsync(user);
                return Ok("eX01");
            }
            try
            {
                List<MunResponse> mR = new();
                var municipalityList = await _locationsHelper.GetMunicipalitiesByDeptoAsync(id);

                foreach (var item in municipalityList)
                {
                    var mun = await _locationsHelper.GetCommunitiesByMunAsync(item.Id);
                    mR.Add(new MunResponse { Municipality = item, CommunitiesCount = mun.Count });
                }
                return Ok(mR);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Department>> GetDepartment(int id)
        {
            string email = User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
                .Value;
            User user = await _userHelper.GetUserByEmailAsync(email);
            if (user.IsDefaultPass)
            {
                return Ok(user);
            }
            if (!await _userHelper.IsAutorized(user.Rol, "COMMUNITIES VER"))
            {
                return Unauthorized();
            }

            string token = HttpContext.Request.Headers["Authorization"];
            token = token["Bearer ".Length..].Trim();
            if (user.UserSession.UserToken != token)
            {
                await _userHelper.LogoutAsync(user);
                return Ok("eX01");
            }
            try
            {
                var depto = await _locationsHelper.GetDepartmentAsync(id);
                return Ok(depto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetMunicipality/{id}")]
        public async Task<ActionResult<Municipality>> GetMunicipality(int id)
        {
            string email = User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
                .Value;
            User user = await _userHelper.GetUserByEmailAsync(email);
            if (user.IsDefaultPass)
            {
                return Ok(user);
            }
            if (!await _userHelper.IsAutorized(user.Rol, "COMMUNITIES VER"))
            {
                return Unauthorized();
            }

            string token = HttpContext.Request.Headers["Authorization"];
            token = token["Bearer ".Length..].Trim();
            if (user.UserSession.UserToken != token)
            {
                await _userHelper.LogoutAsync(user);
                return Ok("eX01");
            }
            try
            {
                var municipality = await _locationsHelper.GetMunicipalityAsync(id);
                return Ok(municipality);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetMunsByDepto/{id}")]
        public async Task<ActionResult<IEnumerable<Municipality>>> GetMunsByDepto(int id)
        {
            string email = User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
                .Value;
            User user = await _userHelper.GetUserByEmailAsync(email);
            if (user.IsDefaultPass)
            {
                return Ok(user);
            }
            if (!await _userHelper.IsAutorized(user.Rol, "COMMUNITIES VER"))
            {
                return Unauthorized();
            }

            string token = HttpContext.Request.Headers["Authorization"];
            token = token["Bearer ".Length..].Trim();
            if (user.UserSession.UserToken != token)
            {
                await _userHelper.LogoutAsync(user);
                return Ok("eX01");
            }
            try
            {
                var municipalityList = await _locationsHelper.GetMunicipalitiesByDeptoAsync(id);
                return Ok(municipalityList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetCommsByMun/{id}")]
        public async Task<ActionResult<IEnumerable<Municipality>>> GetCommsByMun(int id)
        {
            string email = User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
                .Value;
            User user = await _userHelper.GetUserByEmailAsync(email);
            if (user.IsDefaultPass)
            {
                return Ok(user);
            }
            if (!await _userHelper.IsAutorized(user.Rol, "COMMUNITIES VER"))
            {
                return Unauthorized();
            }

            string token = HttpContext.Request.Headers["Authorization"];
            token = token["Bearer ".Length..].Trim();
            if (user.UserSession.UserToken != token)
            {
                await _userHelper.LogoutAsync(user);
                return Ok("eX01");
            }
            try
            {
                var communityList = await _locationsHelper.GetCommunitiesByMunAsync(id);
                return Ok(communityList.OrderBy(c => c.Name));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Community>> AddCommunity(
            [FromBody] AddCommunityViewModel model
        )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            string email = User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
                .Value;
            User user = await _userHelper.GetUserByEmailAsync(email);
            if (user.IsDefaultPass)
            {
                return Ok(user);
            }
            if (!await _userHelper.IsAutorized(user.Rol, "COMMUNITIES CREATE"))
            {
                return Unauthorized();
            }

            string token = HttpContext.Request.Headers["Authorization"];
            token = token["Bearer ".Length..].Trim();
            if (user.UserSession.UserToken != token)
            {
                await _userHelper.LogoutAsync(user);
                return Ok("eX01");
            }

            try
            {
                var comm = await _locationsHelper.AddCommunityAsync(model);
                return Ok(comm);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("UpdateCommunity")]
        public async Task<ActionResult<Community>> UpdateCommunity(
            [FromBody] UpdateCommunityViewModel model
        )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            string email = User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
                .Value;
            User user = await _userHelper.GetUserByEmailAsync(email);
            if (user.IsDefaultPass)
            {
                return Ok(user);
            }
            if (!await _userHelper.IsAutorized(user.Rol, "COMMUNITIES UPDATE"))
            {
                return Unauthorized();
            }

            string token = HttpContext.Request.Headers["Authorization"];
            token = token["Bearer ".Length..].Trim();
            if (user.UserSession.UserToken != token)
            {
                await _userHelper.LogoutAsync(user);
                return Ok("eX01");
            }

            try
            {
                var comm = await _locationsHelper.UpdateCommunityAsync(model);
                if (comm == null)
                {
                    return NoContent();
                }
                return Ok(comm);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("UpdateMunicipality")]
        public async Task<ActionResult<Municipality>> UpdateMunicipality(
            [FromBody] UpdateMunicipalityViewModel model
        )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            string email = User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
                .Value;
            User user = await _userHelper.GetUserByEmailAsync(email);
            if (user.IsDefaultPass)
            {
                return Ok(user);
            }
            if (!await _userHelper.IsAutorized(user.Rol, "COMMUNITIES UPDATE"))
            {
                return Unauthorized();
            }

            string token = HttpContext.Request.Headers["Authorization"];
            token = token["Bearer ".Length..].Trim();
            if (user.UserSession.UserToken != token)
            {
                await _userHelper.LogoutAsync(user);
                return Ok("eX01");
            }

            try
            {
                var comm = await _locationsHelper.UpdateMunicipalityAsync(model);
                if (comm == null)
                {
                    return NoContent();
                }
                return Ok(comm);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("DeleteCommunity/{id}")]
        public async Task<ActionResult<Community>> DeleteCommunity(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            string email = User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
                .Value;
            User user = await _userHelper.GetUserByEmailAsync(email);
            if (user.IsDefaultPass)
            {
                return Ok(user);
            }
            if (!await _userHelper.IsAutorized(user.Rol, "COMMUNITIES DELETE"))
            {
                return Unauthorized();
            }

            string token = HttpContext.Request.Headers["Authorization"];
            token = token["Bearer ".Length..].Trim();
            if (user.UserSession.UserToken != token)
            {
                await _userHelper.LogoutAsync(user);
                return Ok("eX01");
            }

            try
            {
                var comm = await _locationsHelper.DeleteCommunityAsync(id);
                if (comm == null)
                {
                    return NoContent();
                }
                return Ok(comm);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
