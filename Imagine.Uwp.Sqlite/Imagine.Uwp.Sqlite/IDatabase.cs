using System.Threading.Tasks;

namespace Imagine.Uwp.Sqlite
{
    public interface IDatabase
    {
        void Setup();

        Task Clear();
    }
}
