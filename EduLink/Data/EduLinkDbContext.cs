using Microsoft.EntityFrameworkCore;

namespace EduLink.Data
{
    public class EduLinkDbContext : DbContext
    {
        public EduLinkDbContext(DbContextOptions options) : base(options)
        {

        }
    }
}
