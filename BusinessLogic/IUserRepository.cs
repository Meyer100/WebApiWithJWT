using CLModels;

namespace BusinessLogic
{
    public interface IUserRepository
    {
        Task<string> Login(UserDTO userDTO);
    }
}