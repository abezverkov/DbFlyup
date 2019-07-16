using System.Threading.Tasks;

namespace DbFlyup
{
    public interface IFlywayUpdater
    {
        Task<int> Baseline();
        Task<int> Clean();
        Task<int> Info();
        Task<int> Migrate();
        Task<int> Repair();
        Task<int> Test();
        Task<int> Undo();
        Task<int> Validate();
    }
}