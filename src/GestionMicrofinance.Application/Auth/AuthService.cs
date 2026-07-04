using GestionMicrofinance.Application.Common.Exceptions;
using GestionMicrofinance.Application.Common.Interfaces;
using GestionMicrofinance.Domain.Entities;

namespace GestionMicrofinance.Application.Auth;

public class AuthService : IAuthService
{
    private readonly IPersonneRepository _personneRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AuthService(
        IPersonneRepository personneRepository,
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _personneRepository = personneRepository;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<PersonneResponse> RegisterAsync(RegisterRequest request)
    {
        if (await _personneRepository.EmailExistsAsync(request.Email))
        {
            throw new ConflictException($"Un compte existe déjà avec l'email '{request.Email}'.");
        }

        _passwordHasher.CreateHash(request.Password, out var passwordHash, out var passwordSalt);

        var personne = new Personne
        {
            Nom = request.Nom,
            Prenom = request.Prenom,
            Email = request.Email,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            DateCreation = DateTime.UtcNow,
            IsActive = true
        };

        await _personneRepository.AddAsync(personne);
        await _unitOfWork.SaveChangesAsync();

        return new PersonneResponse
        {
            Id = personne.Id,
            Nom = personne.Nom,
            Prenom = personne.Prenom,
            Email = personne.Email,
            DateCreation = personne.DateCreation
        };
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var personne = await _personneRepository.GetByEmailAsync(request.Email);

        if (personne is null || !_passwordHasher.Verify(request.Password, personne.PasswordHash, personne.PasswordSalt))
        {
            throw new UnauthorizedAppException("Email ou mot de passe incorrect.");
        }

        return new AuthResponse
        {
            Token = _jwtTokenGenerator.GenerateToken(personne)
        };
    }
}
