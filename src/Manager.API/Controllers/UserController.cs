using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Manager.Core.Exceptions;
using Manager.API.ViewModels;
using Manager.Services.Interfaces;
using AutoMapper;
using Manager.Services.DTO;
using Manager.API.Utilities;

namespace Manager.API.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("api/v1/create")]
        public async Task<IActionResult> Create([FromBody] CreateUserViewModel userViewModel)
        {
            try
            {
                var userDTO = _mapper.Map<UserDTO>(userViewModel);
                var userCreated = await _userService.Create(userDTO);
                return Ok(new ResultViewModel
                {
                    Message = "Usuário criado com sucesso",
                    Success = true,
                    Data = userCreated
                });
            }
            catch (DomainException ex)
            {
                return BadRequest(Responses.DomainErrorMessage(ex.Message, ex.Errors));
            }
            catch (Exception)
            {
                return StatusCode(500, Responses.ApplicationErrorMessage());
            }
        }


        [HttpGet]
        [Route("/api/v1/users/get-all")]
        public async Task<IActionResult> GetAsync()
        {
            var allUsers = await _userService.Get();


            return Ok(new ResultViewModel
            {
                Message = "Usuários encontrados com sucesso!",
                Success = true,
                Data = allUsers
            });
        }
    }
}
