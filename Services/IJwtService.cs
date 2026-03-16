namespace BankingPaymentsApiDemo.Services;

public interface IJwtService
{
    string GenerateToken(string username);
}