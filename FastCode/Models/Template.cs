using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastCode.Models
{
    public class TemplateFieldRole
    {
        public bool first { get; set; }
        public bool last { get; set; }
        public bool outhers { get; set; }
    }

    public class TemplateFieldType
    {
        public string number { get; set; }
        public string value { get; set; }
        public string text { get; set; }
        public string @bool { get; set; }
        public string date { get; set; }
        public string time { get; set; }
        public string file { get; set; }
        public string password { get; set; }
        public string fk { get; set; }
    }

    public class TemplateField
    {
        public string name { get; set; }
        public TemplateFieldType type { get; set; }
        public TemplateFieldRole role { get; set; }

        public override string ToString()
        {
            return name;
        }
    }

    public class Template
    {
        public string name { get; set; }
        public List<TemplateField> fields { get; set; } = new List<TemplateField>();
        public string code { get; set; }

        public override string ToString()
        {
            return name;
        }
    }
}
