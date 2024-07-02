
using Microsoft.AspNetCore.Mvc;
using QuokkaMesh.Models.DataModel.OTPModel;
using QuokkaMesh.Models.DataModel.TokenDataModel;
using QuokkaMesh.Models.DataModels.UserModel;

namespace QuokkaMesh.Services.Users
{

    public interface IAuthService
    {
       
        Task<AuthModel> RegistrationAsync(RegisterModel model);

         Task<AuthModel> GetTokenAsync(TokenRequestModel model);


        public  Task<Messages> UpdateSubProfile([FromRoute] string id, [FromBody] RegisterModel patch);


        Task<AuthModel> RefreshTokenAsync(string token);
        Task<bool> RevokeTokenAsync(string token);


    }




}
