﻿using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xamarin.Essentials;
using YourVitebskApp.Models;

namespace YourVitebskApp.Services
{
    public class AuthService
    {
        private const string _url = "http://yourvitebsk.somee.com/api/auth/";
        private readonly JsonSerializerOptions _options;
        private readonly HttpClient _client;

        public AuthService()
        {
            _options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        public async void SaveUserCreds(string token)
        {
            await SecureStorage.SetAsync("Token", token);
            var handler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtSecurityToken = handler.ReadJwtToken(token);
            await SecureStorage.SetAsync("UserId", jwtSecurityToken.Claims.First(x => x.Type == "UserId").Value);
            await SecureStorage.SetAsync("Email", jwtSecurityToken.Claims.First(x => x.Type == "Email").Value);
            await SecureStorage.SetAsync("FirstName", jwtSecurityToken.Claims.First(x => x.Type == "FirstName").Value);
            await SecureStorage.SetAsync("SecondName", jwtSecurityToken.Claims.First(x => x.Type == "SecondName").Value);
            await SecureStorage.SetAsync("LastName", jwtSecurityToken.Claims.First(x => x.Type == "LastName").Value);
            await SecureStorage.SetAsync("PhoneNumber", jwtSecurityToken.Claims.First(x => x.Type == "PhoneNumber").Value);
            await SecureStorage.SetAsync("Expires", jwtSecurityToken.Claims.First(x => x.Type == "exp").Value);
        }

        // Авторизация пользователя и получение токена
        public async Task<string> Login(UserLoginDTO loginCreds)
        {
            var response = await _client.PostAsync(_url + "login/", new StringContent(JsonSerializer.Serialize(loginCreds), Encoding.UTF8, "application/json"));
            if (response.IsSuccessStatusCode)
            {
                return JsonSerializer.Deserialize<ResponseModel>(await response.Content.ReadAsStringAsync(), _options).Content.ToString();
            }
            else
            {
                throw new ArgumentException(JsonSerializer.Deserialize<ResponseModel>(await response.Content.ReadAsStringAsync(), _options).ErrorMessage);
            }
        }

        // Авторизация пользователя и получение токена
        public async Task<string> Register(UserRegisterDTO registerCreds)
        {
            var response = await _client.PostAsync(_url + "register/", new StringContent(JsonSerializer.Serialize(registerCreds), Encoding.UTF8, "application/json"));
            if (response.IsSuccessStatusCode)
            {
                return JsonSerializer.Deserialize<ResponseModel>(await response.Content.ReadAsStringAsync(), _options).Content.ToString();
            }
            else
            {
                throw new ArgumentException(JsonSerializer.Deserialize<ResponseModel>(await response.Content.ReadAsStringAsync(), _options).ErrorMessage);
            }
        }
    }
}