using _305.Application.IUOW;
using DataLayer;

namespace _305.WebApi.Assistants.Tasks;

public class TokenCleanupTask
{
    public async Task ExecuteAsync(IUnitOfWork unitOfWork)
    {
        var tokens = unitOfWork.TokenBlacklistRepository.FindList(t => t.expiry_date <= DateTime.UtcNow);
		if (tokens != null)
        {
            unitOfWork.TokenBlacklistRepository.RemoveRange(tokens);

            await unitOfWork.CommitAsync(new CancellationToken());
        }
    }
}
