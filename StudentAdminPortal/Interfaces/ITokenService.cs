using System.Collections.Generic;
using StudentAdminPortal.Models;

namespace StudentAdminPortal.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(ApplicationUser user, IList<string> roles);
    }
}
