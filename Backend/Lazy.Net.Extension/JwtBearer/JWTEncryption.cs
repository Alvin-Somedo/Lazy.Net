using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Lazy.Net.Extension.JwtBearer
{
    /// <summary>
    /// JWT 加解密
    /// </summary>
    public class JWTEncryption
    {
        /// <summary>
        /// 生成Token
        /// </summary>
        /// <param name="payload"></param>
        /// <param name="expiredTime">过期时间（分钟）</param>
        /// <returns></returns>
        public static string Encrypt(IDictionary<string,object> payload,long? expiredTime = null)
        {
            var (Payload, JWTSettings) = CombinePayload(payload, expiredTime);
            return Encrypt(JWTSettings.IssuerSigningKey, Payload, JWTSettings.Algorithm);
        }

        public static string Encrypt(string issureSigninKey, IDictionary<string, object> payload, string algorithm = SecurityAlgorithms.HmacSha256)
        {
            // 处理 JwtPayload 序列化不一致问题
            var stringPayload = payload is JwtPayload jwtPayload ? jwtPayload.SerializeToJson() : JsonSerializer.Serialize(payload, new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });
            return Encrypt(issureSigninKey, stringPayload, algorithm);
        }

        public static string Encrypt(string issuerSigningKey, string payload, string algorithm = SecurityAlgorithms.HmacSha256)
        {
            SigningCredentials credentials = null;

            if (!string.IsNullOrWhiteSpace(issuerSigningKey))
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(issuerSigningKey));
                credentials = new SigningCredentials(securityKey, algorithm);
            }

            var tokenHandler = new JsonWebTokenHandler();
            return credentials == null ? tokenHandler.CreateToken(payload) : tokenHandler.CreateToken(payload, credentials);
        }

        /// <summary>
        /// 组合 Claims 负荷
        /// </summary>
        /// <param name="payload"></param>
        /// <param name="expiredTime">过期时间，单位：分钟</param>
        /// <returns></returns>
        private static (IDictionary<string, object> Payload, JWTSettingsOptions JWTSettings) CombinePayload(IDictionary<string, object> payload, long? expiredTime = null)
        {
            var jwtSettings = GetJWTSettings();
            var datetimeOffset = DateTimeOffset.UtcNow;

            if (!payload.ContainsKey(JwtRegisteredClaimNames.Iat))
            {
                payload.Add(JwtRegisteredClaimNames.Iat, datetimeOffset.ToUnixTimeSeconds());
            }

            if (!payload.ContainsKey(JwtRegisteredClaimNames.Nbf))
            {
                payload.Add(JwtRegisteredClaimNames.Nbf, datetimeOffset.ToUnixTimeSeconds());
            }

            if (!payload.ContainsKey(JwtRegisteredClaimNames.Exp))
            {
                var minute = expiredTime ?? jwtSettings?.ExpiredTime ?? 20;
                payload.Add(JwtRegisteredClaimNames.Exp, DateTimeOffset.UtcNow.AddMinutes(minute).ToUnixTimeSeconds());
            }

            if (!payload.ContainsKey(JwtRegisteredClaimNames.Iss))
            {
                payload.Add(JwtRegisteredClaimNames.Iss, jwtSettings?.ValidIssuer);
            }

            if (!payload.ContainsKey(JwtRegisteredClaimNames.Aud))
            {
                payload.Add(JwtRegisteredClaimNames.Aud, jwtSettings?.ValidAudience);
            }

            return (payload, jwtSettings);

        }

        /// <summary>
        /// 获取 JWT 配置
        /// </summary>
        /// <returns></returns>
        public static JWTSettingsOptions GetJWTSettings()
        {


            return new JWTSettingsOptions()
            {
                IssuerSigningKey = "asdasdasd1111111111111111111111111111111111111111111111111111111111111",
                ValidIssuer = "asdasd1111111111111111111111",
                ValidAudience = "asdasd11111111111111111111111",
                ExpiredTime = 180,
                Algorithm = SecurityAlgorithms.HmacSha256
            };
        }



    }
}
