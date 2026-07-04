using GestionMicrofinance.Application.Auth;
using GestionMicrofinance.Application.Common.Exceptions;
using GestionMicrofinance.Application.Common.Interfaces;
using GestionMicrofinance.Domain.Entities;
using Moq;
using Xunit;

namespace GestionMicrofinance.Application.UnitTests;

public class AuthServiceTests
{
    private readonly Mock<IPersonneRepository> _personneRepository = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<IPasswordHasher> _passwordHasher = new();
    private readonly Mock<IJwtTokenGenerator> _jwtTokenGenerator = new();
    private readonly AuthService _sut;

    public AuthServiceTests()
    {
        _sut = new AuthService(
            _personneRepository.Object,
            _unitOfWork.Object,
            _passwordHasher.Object,
            _jwtTokenGenerator.Object);
    }

    [Fact]
    public async Task RegisterAsync_WithNewEmail_CreatesPersonne()
    {
        _personneRepository.Setup(r => r.EmailExistsAsync(It.IsAny<string>())).ReturnsAsync(false);

        var request = new RegisterRequest
        {
            Nom = "Domche",
            Prenom = "Tristan",
            Email = "tristan@example.com",
            Password = "password123"
        };

        var result = await _sut.RegisterAsync(request);

        Assert.Equal(request.Email, result.Email);
        _personneRepository.Verify(r => r.AddAsync(It.IsAny<Personne>()), Times.Once);
        _unitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task RegisterAsync_WithExistingEmail_ThrowsConflictException()
    {
        _personneRepository.Setup(r => r.EmailExistsAsync(It.IsAny<string>())).ReturnsAsync(true);

        var request = new RegisterRequest
        {
            Nom = "Domche",
            Prenom = "Tristan",
            Email = "tristan@example.com",
            Password = "password123"
        };

        await Assert.ThrowsAsync<ConflictException>(() => _sut.RegisterAsync(request));
        _personneRepository.Verify(r => r.AddAsync(It.IsAny<Personne>()), Times.Never);
    }

    [Fact]
    public async Task LoginAsync_WithUnknownEmail_ThrowsUnauthorizedAppException()
    {
        _personneRepository.Setup(r => r.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync((Personne?)null);

        var request = new LoginRequest { Email = "inconnu@example.com", Password = "password123" };

        await Assert.ThrowsAsync<UnauthorizedAppException>(() => _sut.LoginAsync(request));
    }

    [Fact]
    public async Task LoginAsync_WithInvalidPassword_ThrowsUnauthorizedAppException()
    {
        var personne = new Personne { Id = 1, Email = "tristan@example.com" };
        _personneRepository.Setup(r => r.GetByEmailAsync(personne.Email)).ReturnsAsync(personne);
        _passwordHasher.Setup(p => p.Verify(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<byte[]>())).Returns(false);

        var request = new LoginRequest { Email = personne.Email, Password = "mauvais-mot-de-passe" };

        await Assert.ThrowsAsync<UnauthorizedAppException>(() => _sut.LoginAsync(request));
    }

    [Fact]
    public async Task LoginAsync_WithValidCredentials_ReturnsToken()
    {
        var personne = new Personne { Id = 1, Email = "tristan@example.com" };
        _personneRepository.Setup(r => r.GetByEmailAsync(personne.Email)).ReturnsAsync(personne);
        _passwordHasher.Setup(p => p.Verify(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<byte[]>())).Returns(true);
        _jwtTokenGenerator.Setup(j => j.GenerateToken(personne)).Returns("fake-jwt-token");

        var request = new LoginRequest { Email = personne.Email, Password = "password123" };

        var result = await _sut.LoginAsync(request);

        Assert.Equal("fake-jwt-token", result.Token);
    }
}
