using GestionMicrofinance.Application.Common.Exceptions;
using GestionMicrofinance.Application.Common.Interfaces;
using GestionMicrofinance.Application.Operations;
using GestionMicrofinance.Domain.Entities;
using GestionMicrofinance.Domain.Enums;
using Moq;
using Xunit;

namespace GestionMicrofinance.Application.UnitTests;

public class OperationServiceTests
{
    private readonly Mock<IOperationRepository> _operationRepository = new();
    private readonly Mock<ICategorieRepository> _categorieRepository = new();
    private readonly Mock<IPersonneRepository> _personneRepository = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly OperationService _sut;

    public OperationServiceTests()
    {
        _sut = new OperationService(
            _operationRepository.Object,
            _categorieRepository.Object,
            _personneRepository.Object,
            _unitOfWork.Object);
    }

    [Fact]
    public async Task DeclareAsync_WithMatchingCategorieType_CreatesOperationWithDateSet()
    {
        var personne = new Personne { Id = 1 };
        var categorie = new Categorie { Id = 2, Libelle = "Loyer", Type = TypeCategorie.Depense };

        _personneRepository.Setup(r => r.GetByIdAsync(personne.Id)).ReturnsAsync(personne);
        _categorieRepository.Setup(r => r.GetByIdAsync(categorie.Id)).ReturnsAsync(categorie);

        var request = new DeclareOperationRequest { Montant = 100m, CategorieId = categorie.Id };

        var result = await _sut.DeclareAsync(personne.Id, request, TypeCategorie.Depense);

        Assert.Equal(100m, result.Montant);
        Assert.NotEqual(default, result.DateOperation);
        _operationRepository.Verify(r => r.AddAsync(It.Is<Operation>(o => o.DateOperation != default)), Times.Once);
        _unitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeclareAsync_WithUnknownCategorie_ThrowsNotFoundException()
    {
        var personne = new Personne { Id = 1 };
        _personneRepository.Setup(r => r.GetByIdAsync(personne.Id)).ReturnsAsync(personne);
        _categorieRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Categorie?)null);

        var request = new DeclareOperationRequest { Montant = 100m, CategorieId = 999 };

        await Assert.ThrowsAsync<NotFoundException>(() => _sut.DeclareAsync(personne.Id, request, TypeCategorie.Depense));
    }

    [Fact]
    public async Task DeclareAsync_WithMismatchedCategorieType_ThrowsValidationAppException()
    {
        var personne = new Personne { Id = 1 };
        var categorie = new Categorie { Id = 2, Libelle = "Salaire", Type = TypeCategorie.Revenu };

        _personneRepository.Setup(r => r.GetByIdAsync(personne.Id)).ReturnsAsync(personne);
        _categorieRepository.Setup(r => r.GetByIdAsync(categorie.Id)).ReturnsAsync(categorie);

        var request = new DeclareOperationRequest { Montant = 100m, CategorieId = categorie.Id };

        await Assert.ThrowsAsync<ValidationAppException>(() => _sut.DeclareAsync(personne.Id, request, TypeCategorie.Depense));
    }

    [Fact]
    public async Task GetRecentAsync_UsesThreeMonthsWindowByDefault()
    {
        _operationRepository
            .Setup(r => r.GetRecentByPersonneAsync(1, It.IsAny<DateTime>()))
            .ReturnsAsync(new List<Operation>());

        await _sut.GetRecentAsync(1, TypeCategorie.Depense);

        _operationRepository.Verify(r => r.GetRecentByPersonneAsync(
            1,
            It.Is<DateTime>(d => d <= DateTime.UtcNow.AddMonths(-3).AddMinutes(1))), Times.Once);
    }

    [Fact]
    public async Task GetRecentAsync_FiltersOutOperationsOfTheOtherType()
    {
        var depenseCategorie = new Categorie { Id = 1, Libelle = "Loyer", Type = TypeCategorie.Depense };
        var revenuCategorie = new Categorie { Id = 2, Libelle = "Salaire", Type = TypeCategorie.Revenu };

        _operationRepository
            .Setup(r => r.GetRecentByPersonneAsync(1, It.IsAny<DateTime>()))
            .ReturnsAsync(new List<Operation>
            {
                new() { Id = 1, Montant = 100m, CategorieId = 1, Categorie = depenseCategorie },
                new() { Id = 2, Montant = 200m, CategorieId = 2, Categorie = revenuCategorie }
            });

        var result = await _sut.GetRecentAsync(1, TypeCategorie.Depense);

        Assert.Single(result);
        Assert.Equal(1, result[0].Id);
    }
}
