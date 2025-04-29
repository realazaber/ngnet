using backend.DTOs.User;
using backend.Models;

namespace Backend.Models.Mappers
{
    public static class UserMapper
    {
        public static RetriveUserDTO ConvertToRetrieveDTO(User user, IList<string> roles)
        {
            return new RetriveUserDTO(new Guid(user.Id), user.FirstName, user.LastName,
                                                              user.ProfileImg, user.Email, user.UserName,                                                              user.PhoneNumber, user.CreatorId, roles);
        }

    }
}
