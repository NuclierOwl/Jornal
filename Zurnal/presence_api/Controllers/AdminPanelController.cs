using domain.UseCase;
using Microsoft.AspNetCore.Mvc;


namespace presence_api.Controllers;

[ApiController]
[Route("api/admin")]

public class AdminPanelController : ControllerBase
{
    public readonly GroupUseCase _groupUseCase;
    public readonly UserUseCase _userUseCase;
    public readonly UseCaseGeneratePresence _presenceUseCase;

    public AdminPanelController(GroupUseCase groupUseCase, UserUseCase userUseCase, UseCaseGeneratePresence presenceUseCase)
    {
        _groupUseCase = groupUseCase;
        _userUseCase = userUseCase;
        _presenceUseCase = presenceUseCase;
    }

    

}