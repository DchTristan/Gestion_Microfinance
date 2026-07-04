using GestionMicrofinance.Application.Categories;
using GestionMicrofinance.Application.Common.Interfaces;
using GestionMicrofinance.Domain.Entities;
using GestionMicrofinance.Domain.Enums;
using Moq;
using Xunit;

namespace GestionMicrofinance.Application.UnitTests;

public class CategorieServiceTests
{
    private readonly Mock<ICategorieRepository> _categorieRepository = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly CategorieService _sut;

    public CategorieServiceTests()
    {
        _sut = new CategorieService(_categorieRepository.Object, _unitOfWork.Object);
    }

    [Fact]
    public async Task CreateAsync_AddsCategorieAndSaves()
    {
        var request = new CreateCategorieRequest { Libelle = "Transport", Type = TypeCategorie.Depense };

        var result = await _sut.CreateAsync(request);

        Assert.Equal("Transport", result.Libelle);
        Assert.Equal(TypeCategorie.Depense, result.Type);
        _categorieRepository.Verify(r => r.AddAsync(It.IsAny<Categorie>()), Times.Once);
        _unitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllCategoriesMapped()
    {
        _categorieRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Categorie>
        {
            new() { Id = 1, Libelle = "Salaire", Type = TypeCategorie.Revenu },
            new() { Id = 2, Libelle = "Loyer", Type = TypeCategorie.Depense }
        });

        var result = await _sut.GetAllAsync();

        Assert.Equal(2, result.Count);
        Assert.Contains(result, c => c.Libelle == "Salaire");
        Assert.Contains(result, c => c.Libelle == "Loyer");
    }
}
