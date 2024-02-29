using System.Threading.Tasks;

namespace Henry.Scheduling.Api.Infrastructure.Jobs.Contracts
{
    public interface IAmARecurringJob
    {
        Task ExecuteAsync();
    }
}
