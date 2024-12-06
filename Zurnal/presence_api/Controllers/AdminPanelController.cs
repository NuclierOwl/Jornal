using domain.UseCase;
using Microsoft.AspNetCore.Mvc;


namespace presence_api.Controllers;

[ApiController]
[Route("api/admin")]

public class AdminPanelController : ControllerBase
{
    private readonly GroupUseCase _groupUseCase;
    private readonly UserUseCase _userUseCase;
    private readonly UseCaseGeneratePresence _presenceUseCase;

    public AdminPanelController(GroupUseCase groupUseCase, UserUseCase userUseCase, UseCaseGeneratePresence presenceUseCase)
    {
        _groupUseCase = groupUseCase;
        _userUseCase = userUseCase;
        _presenceUseCase = presenceUseCase;
    }

    

}