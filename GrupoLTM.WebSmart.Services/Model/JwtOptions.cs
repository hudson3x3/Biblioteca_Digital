using Microsoft.IdentityModel.Tokens;
using System;

namespace GrupoLTM.WebSmart.Services.Model
{
    public class JwtOptions
    {
        public string redirectUrlMktplace;

        public string SecurityKey { get; set; }
        public string ValidIssuer { get; set; }
        public string Audience { get; set; }
        public string SsoEndpoint { get; set; }
        public string Sub { get; set; }
        public string additionalinfo { get; set; }
        public SymmetricSecurityKey SymmetricSecurityKey => new SymmetricSecurityKey(Convert.FromBase64String(SecurityKey));
        public SigningCredentials SigningCredentials => new SigningCredentials(SymmetricSecurityKey, SecurityAlgorithms.HmacSha256);

    }

}
