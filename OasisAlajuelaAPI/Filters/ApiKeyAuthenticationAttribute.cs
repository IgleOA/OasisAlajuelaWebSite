﻿using Microsoft.IdentityModel.Tokens;
using System;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using ET;
using BL;

namespace OasisAlajuelaAPI.Filters
{
    public class ApiKeyAuthenticationAttribute : AuthorizationFilterAttribute
    {

        private static string SecretKey = ConfigurationManager.AppSettings["JWT_SECRET_KEY"].ToString();
        private TokensBL TBL = new TokensBL();

        private static void HandleUnathorized(HttpActionContext actionContext)
        {
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);            
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var authHeader = actionContext.Request.Headers.GetValues("Authorization").FirstOrDefault();
            var token = authHeader.Substring("Bearer ".Length);

            SecurityToken validatedToken;
            var SecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));

            TokenValidationParameters validparams = new TokenValidationParameters()
            {
                IssuerSigningKey = SecurityKey,
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true
            };

            ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(token, validparams, out validatedToken);

            var r = TBL.ValidateToken(token);

            if(r.ExpiresDate > DateTime.Now)
            {
                return;
            }

            HandleUnathorized(actionContext);
        }
    }
}