using GestionMicrofinance.Application.Categories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionMicrofinance.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CategorieController : ControllerBase
{
    private readonly ICategorieService _categorieService;

    public CategorieController(ICategorieService categorieService)
    {
        _categorieService = categorieService;
    }

    [HttpPost("create")]
    public async Task<ActionResult<CategorieResponse>> CreateCategorie(CreateCategorieRequest request)
    {
        var result = await _categorieService.CreateAsync(request);
        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<List<CategorieResponse>>> GetAll()
    {
        var result = await _categorieService.GetAllAsync();
        return Ok(result);
    }
}
