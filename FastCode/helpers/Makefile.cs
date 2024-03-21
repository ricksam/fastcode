using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastCode.helpers
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class MakefileRole
    {
        public string template { get; set; }
        public string command { get; set; }
        public override string ToString()
        {
            return template;
        }
    }

    public class Makefile
    {
        public string name { get; set; }
        public List<MakefileRole> roles { get; set; }
        public string project { get; set; }
        public string templates { get; set; }
        public string target { get; set; }
    }
}
