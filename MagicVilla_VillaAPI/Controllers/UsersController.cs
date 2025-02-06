using Asp.Versioning;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/v{version:apiVersion}/UsersAuth")]
    [ApiVersionNeutral]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepo;
        private APIResponse _response;
        public UsersController(IUserRepository userRepository)
        {
            _userRepo = userRepository;
            this._response = new APIResponse();
        }
        //public IActionResult Index()
        //{
        //    return View();
        //}

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
        {
            var TokenDTO = await _userRepo.Login(model);
            if (TokenDTO == null || string.IsNullOrEmpty(TokenDTO.AccessToken))
            {
                _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("UserName or Password is incorrect");
                return BadRequest(_response);
            }
            _response.StatusCode = System.Net.HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = TokenDTO;
            return Ok(_response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDTO model)
        {
            bool ifUserNameUnique = _userRepo.IsUniqueUser(model.UserName);
            if (!ifUserNameUnique)
            {
                _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("UserName already exists");
                return BadRequest(_response);
            }
            var user = await _userRepo.Register(model);
            if (user == null )
            {
                _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Error While Registering");
                return BadRequest(_response);
            }
            _response.StatusCode = System.Net.HttpStatusCode.OK;
            _response.IsSuccess = true;
            return Ok(_response);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> GetNewTokenFromRefreshToken([FromBody] TokenDTO tokenDTO)
        {
            if (ModelState.IsValid) { 
            
                var tokenDTOResponse=await _userRepo.RefreshAccessToken(tokenDTO);

                if (string.IsNullOrEmpty(tokenDTOResponse.AccessToken)||tokenDTOResponse==null)
                {
                    _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Invalid Token");
                    return BadRequest(_response);
                }
                _response.IsSuccess=true;
                _response.StatusCode=System.Net.HttpStatusCode.OK;
                _response.Result = tokenDTOResponse;
                return Ok(_response);

            }
            else
            {
                _response.IsSuccess=false;
                _response.Result = "Invalid Token";
                return BadRequest(_response);
            }
        }

        [HttpPost("revoke")]
        public async Task<IActionResult> RevokeRefreshToken([FromBody] TokenDTO tokenDTO)
        {
            if (ModelState.IsValid)
            {
                await _userRepo.RevokeRefreshToken(tokenDTO);
                _response.StatusCode = System.Net.HttpStatusCode.OK;
                _response.IsSuccess = true;
                return BadRequest(_response);
            }
            
            _response.Result = "Invalid Input";
            _response.IsSuccess = false;
            return BadRequest(_response);
        }

    }
}
