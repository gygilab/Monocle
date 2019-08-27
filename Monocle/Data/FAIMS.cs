using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monocle.Data
{
    public class FAIMS
    {
        /// <summary>
        /// Compensation voltage
        /// </summary>
        public int CV { get; set; } = -55;
    }

    public enum FAIMS_State
    {
        On,
        Off
    }
}
