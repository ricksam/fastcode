using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastCode.Models
{
    public class EntityField
    {
        public string title { get; set; }
        public string name { get; set; }
        public string placeholder { get; set; }
        public string type { get; set; }
        public string size { get; set; }
        public string role { get; set; }
        public override string ToString()
        {
            return name;
        }
    }

    public class Entity
    {
        public string title { get; set; }
        public string name { get; set; }
        public List<EntityField> fields { get; set; } = new List<EntityField>();

        public override string ToString()
        {
            return name;
        }
    }



    public class ProjectScheme
    {
        public string name { get; set; }
        public string database { get; set; }
        public string language { get; set; }
        public List<Entity> entities { get; set; } = new List<Entity>();
    }
}
