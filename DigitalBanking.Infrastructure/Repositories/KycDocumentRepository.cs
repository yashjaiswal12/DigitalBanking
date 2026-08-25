using DigitalBanking.Application.Interfaces.Persistence;
using DigitalBanking.Infrastructure.Persistence;

namespace DigitalBanking.Infrastructure.Repositories
{
    public class KycDocumentRepository : IKycDocumentRepository
    {
        private readonly ApplicationDbContext _context;

        public KycDocumentRepository(ApplicationDbContext context)
        {
            _context = context;
        }
    }
}
