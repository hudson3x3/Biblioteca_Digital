using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using GrupoLTM.WebSmart.Services.Model;
namespace GrupoLTM.WebSmart.Services.JwtAuth
{
    public class JwtService
    {
        public JwtService() { }
        public string GenerateJwt(JwtOptions _jwtOptions)
        {

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _jwtOptions.ValidIssuer,// URL base da aplicação que executa o sso, por exemplo https://hml.meumundoavon.com.br
                audience: _jwtOptions.Audience,// URL do redirect, por exemplo a url do catálogo https://sandbox.webpremios.digital
                claims: new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, _jwtOptions.Sub), // login do participante no catálogo
                    new Claim("returnUrl", String.IsNullOrEmpty(_jwtOptions.redirectUrlMktplace)?"":_jwtOptions.redirectUrlMktplace), // URL de redirecionamneto Verdor Links
                    new Claim("additional.info", _jwtOptions.additionalinfo == "COORD"?"true":"false") 

                },
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: _jwtOptions.SigningCredentials //credenciais de autenticação
            );
            // url do middleware que fará a autenticação.. https://sso-sandbox.azurewebsites.net
            var jwt = $"{_jwtOptions.SsoEndpoint}/session?jwt={new JwtSecurityTokenHandler().WriteToken(token)}";
            return jwt;
        }
    }
}
