using System.Collections;

namespace Monocle.File
{
    public interface IScanReader : IEnumerable
    {
        void Open(string path);
    }
}
