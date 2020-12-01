﻿using System;
using System.Threading.Tasks;
using Books_Inventory_System.Models;

namespace Books_Inventory_System.Data
{
    public interface IAuthRepository
    {
        Task<ServiceResponse<int>> Register(User user, string password);
        Task<ServiceResponse<string>> Login(string username, string password);
        Task<bool> UserExists(string username);
    }
}