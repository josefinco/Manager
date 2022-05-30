using AutoMapper;
using EscNet.Cryptography.Interfaces;
using Manager.Core.Exceptions;
using Manager.Domain.Entities;
using Manager.Infra.Interfaces;
using Manager.Services.DTO;
using Manager.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager.Services.Services
{
  public class UserService : IUserService
  {
    private readonly IMapper _mapper;

    private readonly IUserRepository _userRepository;

    private readonly IRijndaelCryptography _rijandelCryptography;

    public UserService(IMapper mapper,
    IUserRepository userRepository,
    IRijndaelCryptography rijndaelCryptography)
    {
      _mapper = mapper;
      _userRepository = userRepository;
      _rijandelCryptography = rijndaelCryptography;
    }


    public async Task<UserDTO> Create(UserDTO? userDTO)
    {
      var userExists = await _userRepository.GetByEmail(userDTO.Email);
      if (userExists != null)
        throw new DomainException("Já Existe um usuário com este email");

      var user = _mapper.Map<User>(userDTO);
      user.Validate();
      user.ChangePassword(_rijandelCryptography.Encrypt(user.Password));

      var userCreated = await _userRepository.Create(user);
      return _mapper.Map<UserDTO>(userCreated);
    }

    public async Task<UserDTO> Update(UserDTO userDTO)
    {
      var userExists = await _userRepository.GetByEmail(userDTO.Email);
      if (userExists == null)
        throw new DomainException("O Usuário não existe");

      var user = _mapper.Map<User>(userDTO);
      user.Validate();
      user.ChangePassword(_rijandelCryptography.Encrypt(user.Password));
      //   System.Console.WriteLine(user.Password);

      var userUpdated = await _userRepository.Update(user);
      return _mapper.Map<UserDTO>(userUpdated);
    }
    public async Task Remove(long id)
    {
      var user = await _userRepository.Get(id);
      if (user == null)
        throw new DomainException("O Usuário não existe");

      await _userRepository.Remove(id);

    }
    public async Task<UserDTO> Get(long id)
    {
      var user = await _userRepository.Get(id);
      if (user == null)
        throw new DomainException("O Usuário não existe");

      return _mapper.Map<UserDTO>(user);
    }
    public async Task<List<UserDTO>> Get()
    {
      var allUsers = await _userRepository.Get();

      return _mapper.Map<List<UserDTO>>(allUsers);
    }
    public async Task<List<UserDTO>> SearchByEmail(string email)
    {

      var users = await _userRepository.SearchByEmail(email);

      return _mapper.Map<List<UserDTO>>(users);
    }
    public async Task<UserDTO> GetByEmail(string emaill)
    {
      var user = await _userRepository.GetByEmail(emaill);
      return _mapper.Map<UserDTO>(user);
    }
    public async Task<List<UserDTO>> SearchByName(string name)
    {

      var users = await _userRepository.SearchByName(name);

      return _mapper.Map<List<UserDTO>>(users);
    }

  }
}
