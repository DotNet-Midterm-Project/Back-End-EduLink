using EduLink.Data;
using EduLink.Repositories.Interfaces;

namespace EduLink.Repositories.Services
{
    
    public class TestService : ITest //Implement all nethod in ITest
    {
        //Inject EduLinkDbContext
        private readonly EduLinkDbContext _context;

        public TestService(EduLinkDbContext context)
        {
            _context = context;
        }
    }
}
