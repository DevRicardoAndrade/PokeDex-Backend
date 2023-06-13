using Poke_Api.Models.User;

namespace Poke_Api.Repositories.User
{
    public interface IUser
    {
        public Task<UserModel> GetUserAsync(int id);
        public Task<IEnumerable<UserModel>> GetUsersAsync();
        public Task<UserModel> PostUserAsync(UserModel user);
        public Task<UserModel> PutUserAsync(UserModel user, int id);
        public Task<UserModel> DeleteUserAsync(int id);

        public Task<string> LoginAsync(UserLogin user);
        public Task<UserModel> Me(string Token);
    }
}
