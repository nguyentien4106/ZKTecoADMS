﻿namespace ZKTecoADMS.Application.DTOs.Auth;

public record RegisterRequest(string Email, string Password, string FirstName, string LastName, string PhoneNumber);