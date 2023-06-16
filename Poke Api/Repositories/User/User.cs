using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Poke_Api.Context;
using Poke_Api.Models.Rules;
using Poke_Api.Models.User;
using Poke_Api.Services;

namespace Poke_Api.Repositories.User
{
    public class User : IUser
    {
        private readonly DataContext? _context;
        private readonly AuthenticationService? _auth;

        public User(DataContext context, AuthenticationService auth)
        {
            _context = context;
            _auth = auth;
        }
        public async Task<UserModel> DeleteUserAsync(int id)
        {
            try
            {
                UserModel? userSelected = await _context.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Id == id);
                if (userSelected == null) { throw new Exception("User not found with id " + id); }
                _context.Users.Remove(userSelected);
                await _context.SaveChangesAsync();
                return userSelected;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<UserModel> GetUserAsync(int id)
        {
            try
            {
                UserModel? userSelected = await _context.Users
                    .AsNoTracking()   
                    .Include(u => u.Rules)
                    .FirstOrDefaultAsync(u => u.Id == id);
                return userSelected;
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        public async Task<IEnumerable<UserModel>> GetUsersAsync()
        {
            try
            {
                IEnumerable<UserModel>? usersSelecteds = await _context.Users
                    .Include(u => u.Rules)
                    .ToListAsync();
                return usersSelecteds;
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        public async Task<string> LoginAsync(UserLogin user)
        {
            try
            {
                UserModel? userLogged = await _context.Users
                    .Include(u => u.Rules)
                    .FirstOrDefaultAsync(u => u.UserName == user.UserName && u.Password == user.Password); 
                if(userLogged == null)
                {
                    return null;
                }
                return _auth.Authenticate(userLogged);
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        public async Task<UserModel> PostUserAsync(UserModel user)
        {
            try
            {
                List<RuleModel> rulesUser = new List<RuleModel>();
                foreach (RuleModel rule in user.Rules)
                {
                    rulesUser.Add(rule);
                }

                user.Rules = null;

                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                user.Rules = new List<RuleModel>();

                Dictionary<string, RuleModel> ruleDict = await _context.Rules.ToDictionaryAsync(r => r.Name); ;

                foreach (RuleModel userRule in rulesUser)
                {
                    if (ruleDict.TryGetValue(userRule.Name, out RuleModel existingRule))
                    {
                        ((List<RuleModel>)user.Rules).Add(existingRule);
                        string sqlQuery = "INSERT INTO UserRule (UserId, RuleId) VALUES (@UserId, @RuleId)";

                        await _context.Database.ExecuteSqlRawAsync(sqlQuery, new[] {
                            new SqlParameter("@UserId", user.Id),
                            new SqlParameter("@RuleId", existingRule.Id)
                        });
                    }
                }

                return user;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }


        public async Task<UserModel> PutUserAsync(UserModel user, int id)
        {
            try
            {
                UserModel? userUpdate = await GetUserAsync(id);
                if(userUpdate == null) { throw new Exception("User not found with id " + id); }
                userUpdate.Name = user.Name;    
                userUpdate.Password = user.Password;    
                _context.Users.Update(userUpdate);
                await _context.SaveChangesAsync();
                return userUpdate;
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }
        public async Task<UserModel> Me(string token)
        {
            try
            {
                int id = _auth.IdByToken(token);
                return await GetUserAsync(id);

            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

    }
}
