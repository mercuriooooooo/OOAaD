using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text;

namespace SpaceBattle.Lib;

public class AuthException : CommandException
{
    public AuthException(string message) : base(message) { }
}


public class PlayerAccount
{
    public string Login { get; }
    public string PasswordHash { get; }
    public bool IsOnline { get; set; }
    public bool IsHost { get; set; }
    public Guid Token { get; set; } = Guid.Empty;

    public PlayerAccount(string login, string passwordHash)
    {
        Login = login;
        PasswordHash = passwordHash;
    }

    public string Status => IsOnline ? "онлайн" : "офлайн";
}


public class AuthService
{
    private readonly ConcurrentDictionary<string, PlayerAccount> _accounts = new(StringComparer.OrdinalIgnoreCase);
    private readonly ConcurrentDictionary<Guid, string> _sessions = new();

    public PlayerAccount Register(string login, string password)
    {
        if (string.IsNullOrWhiteSpace(login) || login.Length < 3)
            throw new AuthException("Логин должен быть не короче 3 символов");
        if (string.IsNullOrEmpty(password))
            throw new AuthException("Пароль не может быть пустым");

        var account = new PlayerAccount(login, PasswordHasher.Hash(password, login));
        if (!_accounts.TryAdd(login, account))
            throw new AuthException($"Игрок '{login}' уже зарегистрирован");
        return account;
    }


    public Guid Login(string login, string password)
    {
        if (!_accounts.TryGetValue(login, out var account))
            throw new AuthException($"Игрок '{login}' не зарегистрирован");
        if (account.PasswordHash != PasswordHasher.Hash(password, login))
            throw new AuthException("Неверный пароль");

        var token = Guid.NewGuid();
        account.Token = token;
        account.IsOnline = true;
        _sessions[token] = login;
        return token;
    }


    public string ValidateToken(Guid token)
    {
        if (!_sessions.TryGetValue(token, out var login))
            throw new AuthException("Токен авторизации недействителен");
        return login;
    }

    public void Logout(Guid token)
    {
        if (_sessions.TryRemove(token, out var login) && _accounts.TryGetValue(login, out var account))
        {
            account.IsOnline = false;
            account.Token = Guid.Empty;
        }
    }

    public bool IsOnline(string login) =>
        _accounts.TryGetValue(login, out var a) && a.IsOnline;

    public PlayerAccount Get(string login) =>
        _accounts.TryGetValue(login, out var a)
            ? a
            : throw new AuthException($"Игрок '{login}' не зарегистрирован");


    public void GrantHost(Guid token) => Get(ValidateToken(token)).IsHost = true;


    public void RequireHost(Guid token)
    {
        var account = Get(ValidateToken(token));
        if (!account.IsHost)
            throw new AuthException($"Игрок '{account.Login}' не имеет прав хоста");
    }
}

public static class PasswordHasher
{

    public static string Hash(string password, string salt)
    {
        using var sha = SHA256.Create();
        var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(salt + "::" + password));
        return Convert.ToHexString(bytes);
    }
}
