using AutoMapper;
using Limak.Application.Abstractions.Repositories;
using Limak.Application.Abstractions.Services;
using Limak.Application.Dtos.PaymentDtos;
using Limak.Application.Dtos.PurchaseDtos;
using Limak.Application.Dtos.ResultDtos;
using Limak.Application.Exceptions;
using Limak.Domain.Entities;
using Limak.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Limak.Persistence.Implementations.Services;

public class PaymentService : IPaymentService
{
    private readonly HttpClient _httpClient;
    private readonly PaymentOptionsDto _paymentOptionsDto;
    private readonly IPaymentRepository _repository;
    private readonly IMapper _mapper;
    public PaymentService(HttpClient httpClient, IConfiguration configuration, IPaymentRepository repository, IMapper mapper)
    {
        _httpClient = httpClient;
        _paymentOptionsDto = configuration.GetSection("KapitalBankOptions").Get<PaymentOptionsDto>() ?? new();
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ResultDto> CreateAsync(PaymentCreateDto dto)
    {
        var payment = _mapper.Map<Payment>(dto);

        await _repository.AddAsync(payment);
        await _repository.SaveChangesAsync();
        return new("Payment created successfully");
    }

    public async Task<ResultDto<PurchaseGetDto>> CreatePaymentRequest(PurchaseCreateDto dto)
    {
        var payload = new
        {
            order = new
            {
                typeRid = "Order_SMS",
                amount = dto.Amount.ToString().Replace(",", "."),
                currency = dto.Currency,
                language = "az",
                description = dto.Description,
                hppRedirectUrl = dto.RedirectUrl,
                hppCofCapturePurposes = new[] { "Cit" }
            }
        };

        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        var json = JsonSerializer.Serialize(payload, options);

        using var request = new HttpRequestMessage(HttpMethod.Post, "https://txpgtst.kapitalbank.az/api/order/");
        request.Content = new StringContent(json, Encoding.UTF8, "application/json");

        var username = _paymentOptionsDto.Username;
        var password = _paymentOptionsDto.Password;
        var basicValue = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"));
        request.Headers.Authorization = new AuthenticationHeaderValue("Basic", basicValue);

        using var response = await _httpClient.SendAsync(request);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException($"Payment request failed: {response.StatusCode} - {content}");
        }

        var result = Newtonsoft.Json.JsonConvert.DeserializeObject<PurchaseGetDto>(content) ?? new();

        return new(result);
    }

    public async Task<ResultDto<PaymentGetDto>> GetAsync(int id)
    {
        var payment = await _repository.GetAsync(x => x.PurchaseId == id);
        if (payment == null)
        {
            throw new NotFoundException("Payment not found");
        }

        var dto = _mapper.Map<PaymentGetDto>(payment);
        return new(dto);
    }

    public async Task<ResultDto<PaymentGetDto>> GetByIdAsync(Guid id)
    {
        var payment = await _repository.GetByIdAsync(id);
        if (payment == null)
        {
            throw new NotFoundException("Payment not found");
        }

        var dto = _mapper.Map<PaymentGetDto>(payment);
        return new(dto);
    }

    public async Task<ResultDto<PaymentGetDto>> GetPaymentByAppUserId(string appUserId)
    {
        var payment = await _repository.GetAll().Where(x => x.AppUserId == appUserId && x.PaymentStatus == PaymentStatuses.Fullypaid).OrderByDescending(x => x.CreatedDate).FirstOrDefaultAsync();
        if (payment == null)
        {
            throw new NotFoundException("Payment not found");
        }
        var dto = _mapper.Map<PaymentGetDto>(payment);
        return new(dto);
    }

    public async Task<ResultDto<PurchaseDetailDto>> GetPurchaseInfoAsync(int id)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, $"https://txpgtst.kapitalbank.az/api/order/{id}");

        var username = _paymentOptionsDto.Username;
        var password = _paymentOptionsDto.Password;
        var basicValue = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"));
        request.Headers.Authorization = new AuthenticationHeaderValue("Basic", basicValue);

        using var response = await _httpClient.SendAsync(request);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException($"Payment request failed: {response.StatusCode} - {content}");
        }

        var result = Newtonsoft.Json.JsonConvert.DeserializeObject<PurchaseDetailDto>(content) ?? new();

        return new(result);

    }

    public async Task<ResultDto> UpdateAsync(PaymentUpdateDto dto)
    {
        var payment = await _repository.GetByIdAsync(dto.Id);
        if (payment == null)
        {
            throw new NotFoundException("Payment is not found");
        }

        payment = _mapper.Map(dto, payment);
        _repository.Update(payment);
        await _repository.SaveChangesAsync();

        return new("Payment updated successfully");
    }
}
