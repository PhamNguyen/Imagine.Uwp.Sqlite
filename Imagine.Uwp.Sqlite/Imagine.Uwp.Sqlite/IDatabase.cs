using System.Threading.Tasks;

namespace Zinio.SDK.Data
{
    public interface IDatabase
    {
        void Setup();

        Task Clear();
    }
}
