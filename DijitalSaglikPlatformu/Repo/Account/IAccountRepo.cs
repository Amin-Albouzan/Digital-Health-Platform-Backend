using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DijitalSaglikPlatformu.Dto;

namespace DijitalSaglikPlatformu.Repo.Account
{
    public interface IAccountRepo
    {
        public Task<string> Register(RegisterNewUser userData,string role);

        public Task<string> Login(dtoLogin login);

        public Task<string> CreateAccessTokenFromRefreshToken(string refreshTokenStr);

        public Task<string> ResetPassword(dtoResetPassword dto);

       

    }
}
