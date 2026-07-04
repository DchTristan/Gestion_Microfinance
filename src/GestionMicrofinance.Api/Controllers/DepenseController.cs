using GestionMicrofinance.Application.Common.Interfaces;
using GestionMicrofinance.Application.Operations;
using GestionMicrofinance.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionMicrofinance.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class DepenseController : ControllerBase
{
    private readonly IOperationService _operationService;
    private readonly ICurrentUserService _currentUserService;

    public DepenseController(IOperationService operationService, ICurrentUserService currentUserService)
    {
        _operationService = operationService;
        _currentUserService = currentUserService;
    }

    [HttpPost("declare")]
    public async Task<ActionResult<OperationResponse>> DeclareDepense(DeclareOperationRequest request)
    {
        var result = await _operationService.DeclareAsync(_currentUserService.PersonneId, request, TypeCategorie.Depense);
        return Ok(result);
    }

    [HttpGet("operations-recentes")]
    public async Task<ActionResult<List<OperationResponse>>> GetRecentOperations()
    {
        var result = await _operationService.GetRecentAsync(_currentUserService.PersonneId, TypeCategorie.Depense);
        return Ok(result);
    }
}
