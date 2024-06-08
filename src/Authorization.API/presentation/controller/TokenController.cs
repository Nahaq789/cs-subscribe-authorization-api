using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace User.Authentication.presentation.controller;

[ApiController]
[Route("/api/v1/[controller]")]
public class TokenController : Controller
{
    private readonly IMediator _mediator;
    private readonly ILogger<TokenController> _logger;

    public TokenController(IMediator mediator, ILogger<TokenController> logger)
    {
        this._mediator = mediator;
        this._logger = logger;
    }
}