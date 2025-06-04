using MediatR;
using System.ComponentModel.DataAnnotations;
using _305.Application.Base.Command;
using _305.Application.Base.Response;

namespace _305.Application.Features.RoleFeatures.Command;

public class CreateRoleCommand : CreateCommand<string>
{
}
