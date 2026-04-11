using AutoMapper;
using Azure.Core;
using Limak.Application.Abstractions.Repositories;
using Limak.Application.Abstractions.Services;
using Limak.Application.Dtos.AccountDtos;
using Limak.Application.Dtos.ResultDtos;
using Limak.Application.Dtos.TokenDtos;
using Limak.Application.Exceptions;
using Limak.Application.Helpers;
using Limak.Domain.Entities;
using Limak.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace Limak.Persistence.Implementations.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IMapper _mapper;
    private readonly IEmailService _emailService;
    private readonly IConfiguration _configuration;
    private readonly IJWTService _jwtService;
    private readonly ICustomerRepository _userRepository;
    private readonly SignInManager<AppUser> _signInManager;

    public AuthService(UserManager<AppUser> userManager, IMapper mapper, IEmailService emailService, IConfiguration configuration, IJWTService jwtService, ICustomerRepository userRepository, SignInManager<AppUser> signInManager)
    {
        _userManager = userManager;
        _mapper = mapper;
        _emailService = emailService;
        _configuration = configuration;
        _jwtService = jwtService;
        _userRepository = userRepository;
        _signInManager = signInManager;
    }

    public async Task<ResultDto<AccessTokenDto>> Login(LoginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null)
        {
            throw new LoginFailException();
        }

        var checkPassword = await _userManager.CheckPasswordAsync(user, dto.Password);

        if (!checkPassword)
        {
            throw new LoginFailException();
        }

        if (!user.EmailConfirmed)
        {
            if (user.LastConfirmationEmailSent == null ||
                user.LastConfirmationEmailSent < DateTime.UtcNow.AddMinutes(-5))
            {
                await SendConfirmationEmail(user);
                user.LastConfirmationEmailSent = DateTime.UtcNow;
                await _userManager.UpdateAsync(user);
            }

            throw new LoginFailException("Please confirm your email first!");
        }



        AccessTokenDto tokenResult = await GetAccessToken(user);

        return new ResultDto<AccessTokenDto>(tokenResult);
    }

    private async Task<AccessTokenDto> GetAccessToken(AppUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);

        List<Claim> claims = [
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new("FirstName",user.FirstName),
            new("LastName",user.LastName),
            new("Username",user.UserName!),
            new("Email",user.Email!),
            new("Role",roles.FirstOrDefault() ?? "")
            ];

        var tokenResult = _jwtService.CreateAccessToken(claims);

        user.RefreshToken = tokenResult.RefreshToken;
        user.RefreshTokenExpiredDate = tokenResult.RefreshTokenExpiredDate;

        await _userManager.UpdateAsync(user);
        return tokenResult;
    }

    public async Task<ResultDto<AccessTokenDto>> RefreshToken(string token)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.RefreshToken == token && x.RefreshTokenExpiredDate > DateTime.UtcNow);

        if (user == null)
        {
            throw new LoginFailException();
        }

        var tokenResult = await GetAccessToken(user);

        return new(tokenResult);
    }

    public async Task<ResultDto> Register(RegisterDto dto)
    {
        var isExistUsername = await _userManager.Users.AnyAsync(x => x.UserName!.ToLower() == dto.UserName.ToLower());
        if (isExistUsername)
        {
            throw new AlreadyExistException("This username is already exist");
        }

        var isExistEmail = await _userManager.Users.AnyAsync(x => x.Email!.ToLower() == dto.Email.ToLower());
        if (isExistEmail)
        {
            throw new AlreadyExistException("This email is already exist");
        }

        var user = _mapper.Map<AppUser>(dto);

        var result = await _userManager.CreateAsync(user, dto.Password);

        if (!result.Succeeded)
        {
            string errorMessage = string.Join(",", result.Errors.Select(e => e.Description));
            throw new RegisterFailException(errorMessage);
        }

        await _userManager.AddToRoleAsync(user, IdentityRoles.Member.ToString());

        string customerCode;

        do
        {
            customerCode = CustomerCodeGenerator.Generate();
        }
        while (await _userRepository.AnyAsync(x => x.CustomerCode == customerCode));

        var newUser = new Customer
        {
            AppUserId = user.Id,
            OfficeId = dto.OfficeId,
            CustomerCode = customerCode
        };

        await _userRepository.AddAsync(newUser);
        await _userRepository.SaveChangesAsync();

        await SendConfirmationEmail(user);

        return new ResultDto("Register successful. Please confirm your email.");
    }

    private async Task SendConfirmationEmail(AppUser user)
    {
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        var encodedToken = WebEncoders.Base64UrlEncode(
            System.Text.Encoding.UTF8.GetBytes(token)
        );

        var confirmationLink =
            $"https://localhost:7001/Account/ConfirmEmail?userId={user.Id}&token={Uri.EscapeDataString(encodedToken)}";

        string body = $@"
                         <!DOCTYPE html>
                            <html>
                            <body>
                                <h2>Confirm your email</h2>
                            
                                <a href='{confirmationLink}'
                                   style='background:#2563eb;color:white;
                                          padding:10px 20px;border-radius:6px;
                                          text-decoration:none;font-weight:bold;'>
                                    Confirm Email
                                </a>                              
                            </body>
                            </html>

    ";

        await _emailService.SendEmailAsync(user.Email!, "Confirm your email", body);
    }

    public async Task<ResultDto> ConfirmEmailAsync(string userId, string token)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            throw new NotFoundException("User not found");

        var decodedBytes = WebEncoders.Base64UrlDecode(token);
        var decodedToken = Encoding.UTF8.GetString(decodedBytes);

        var result = await _userManager.ConfirmEmailAsync(user, decodedToken);

        if (!result.Succeeded)
        {
            string errorMessage = string.Join(",", result.Errors.Select(x => x.Description));
            throw new EmailConfirmationFailException(errorMessage);
        }

        return new ResultDto("Email confirmed successfully");
    }

    public async Task<ResultDto> RevokeRefreshToken(string refreshToken)
    {
        var user = _userManager.Users.FirstOrDefault(x => x.RefreshToken == refreshToken);
        if (user == null)
        {
            throw new NotFoundException("User not found");
        }
        user.RefreshToken = null;
        user.RefreshTokenExpiredDate = null;
        await _userManager.UpdateAsync(user);
        return new("Logout successful");
    }
}
