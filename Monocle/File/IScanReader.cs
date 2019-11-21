using System.Collections;

namespace Monocle.File
{
    public interface IScanReader : IEnumerable
    {
        /// <summary>
        /// Open data file and hold new scan information.
        /// </summary>
        /// <param name="path">Path to the data file.</param>
        void Open(string path);
    }
}
