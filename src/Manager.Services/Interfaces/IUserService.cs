using Manager.Services.DTO;

namespace Manager.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserDTO> Create(UserDTO userDTO);
        Task<UserDTO> Update(UserDTO userDTO);
        Task Remove(int id);
        Task<UserDTO> Get(int id);
        Task<List<UserDTO>> Get();
        Task<List<UserDTO>> SearchByName(string name);
        Task<UserDTO> GetByName(string emaill);
        Task<List<UserDTO>> SearchByEmail(string email);
        Task<UserDTO> GetByEmail(string email);

    }
}
