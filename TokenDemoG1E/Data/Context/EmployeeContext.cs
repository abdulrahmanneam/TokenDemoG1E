using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TokenDemoG1E.Data.Models;

namespace TokenDemoG1E.Data.Context
{
    public class EmployeeContext : IdentityDbContext<Employee>
    {
        public EmployeeContext(DbContextOptions<EmployeeContext> options) : base(options)
        {
        }

    }
}
