using System.IO;

namespace DbFlyup
{
    public interface IFlywayTestRunner
    {
        void RunTests();
        void RunTests(StreamWriter writer);
    }
}