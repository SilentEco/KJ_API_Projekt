using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KJ_API_Projekt.model;
using KJ_API_Projekt.data;
using Microsoft.EntityFrameworkCore;

namespace KJ_API_Projekt.ApiKey
{
    public class ApiTokenManager {

        private readonly ApplicationDbContext _context;
        public ApiTokenManager(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<string> GenerateTokenAsync(MyUser user)
        {
            // Försök att hitta om det redan finns en token
            var token = await _context.ApiTokens
                .FirstOrDefaultAsync(t => t.User.Id == user.Id);

            token ??= new ApiToken();

            token.Value = Guid.NewGuid().ToString();
            token.User = user;

            _context.ApiTokens.Update(token);
            await _context.SaveChangesAsync();

            return token.Value;
        }
    }
}
