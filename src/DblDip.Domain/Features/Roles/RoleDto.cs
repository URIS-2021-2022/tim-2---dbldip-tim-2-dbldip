using DblDip.Core.Models;
using System;
using System.Collections.Generic;

namespace DblDip.Domain.Features
{
    public record RoleDto(Guid RoleId, string Name, IEnumerable<Privilege> Privileges);
}
