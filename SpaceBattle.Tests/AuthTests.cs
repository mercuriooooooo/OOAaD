using SpaceBattle.Lib;
using Xunit;

public class AuthTests
{
    private readonly AuthService _auth = new();

    [Fact]
    public void Register_And_Login_IssuesToken()
    {
        _auth.Register("Александра", "pass123");
        var token = _auth.Login("Александра", "pass123");

        Assert.NotEqual(Guid.Empty, token);
        Assert.Equal("Александра", _auth.ValidateToken(token));
        Assert.True(_auth.IsOnline("Александра"));
    }

    [Fact]
    public void Register_Duplicate_Throws()
    {
        _auth.Register("Иван", "pass1");
        Assert.Throws<AuthException>(() => _auth.Register("Иван", "другой"));
    }

    [Theory]
    [InlineData("", "pass")]
    [InlineData("Иван", "")]
    public void Register_InvalidInput_Throws(string login, string password) =>
        Assert.ThrowsAny<AuthException>(() => _auth.Register(login, password));

    [Fact]
    public void Login_WrongPassword_Throws()
    {
        _auth.Register("Иван", "pass1");
        Assert.Throws<AuthException>(() => _auth.Login("Иван", "неверно"));
    }

    [Fact]
    public void ValidateToken_Unknown_Throws() =>
        Assert.Throws<AuthException>(() => _auth.ValidateToken(Guid.NewGuid()));

    [Fact]
    public void Logout_SetsOffline_And_InvalidatesToken()
    {
        _auth.Register("Мария", "p");
        var token = _auth.Login("Мария", "p");
        _auth.Logout(token);

        Assert.False(_auth.IsOnline("Мария"));
        Assert.Throws<AuthException>(() => _auth.ValidateToken(token));
    }

    [Fact]
    public void HostRights_OnlyForGranted()
    {
        _auth.Register("ХостА", "p");
        _auth.Register("ИгрокБ", "p");
        var hostToken = _auth.Login("ХостА", "p");
        var plainToken = _auth.Login("ИгрокБ", "p");

        _auth.GrantHost(hostToken);
        _auth.RequireHost(hostToken);
        Assert.Throws<AuthException>(() => _auth.RequireHost(plainToken));
    }
}
