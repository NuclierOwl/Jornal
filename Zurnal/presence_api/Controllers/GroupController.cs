using domain.Models;
using domain.UseCase;
using Microsoft.AspNetCore.Mvc;

namespace presence_api.Controllers;
[ApiController]
[Route ("api/[controller]")]

public class GroupController: ControllerBase 
{
    private readonly GroupUseCase _groupUseCase;

    public GroupController(GroupUseCase groupUseCase)
    {
        _groupUseCase = groupUseCase;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Group>> getGroups() 
    { 
        return Ok(_groupUseCase.GetAllGroups());
    }
}



