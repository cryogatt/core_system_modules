using System;
using System.Linq;
using System.Reflection;

namespace CryogattServerAPI.Models
{
    public class Version
    {
        public int Version_Major { get; set; }
        public int Version_Minor { get; set; }

        public Version()
        {
            int vmaj = 0, vmin = 5;

            string assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            var vers = assemblyVersion.Split('.');
            if (vers.Count() > 2)
            {
                if (Int32.TryParse(vers[0], out vmaj))
                {
                    Version_Major = vmaj;
                }
                if (Int32.TryParse(vers[1], out vmin))
                {
                    Version_Minor = vmin;
                }
            }
        }
    }
}