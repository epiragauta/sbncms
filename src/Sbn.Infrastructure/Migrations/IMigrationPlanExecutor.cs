using System.Threading.Tasks;
using Sbn.Cms.Infrastructure.Migrations;

namespace Sbn.Cms.Core.Migrations
{
    public interface IMigrationPlanExecutor
    {
        string Execute(MigrationPlan plan, string fromState);
    }
}
