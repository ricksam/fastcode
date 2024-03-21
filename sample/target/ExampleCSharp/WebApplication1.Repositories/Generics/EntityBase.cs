using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace WebApplication1.Repositories.Generics
{
    public class EntityBase
    {
        public void SetFieldValue(string Name, object Value)
        {
            FieldInfo info = this.GetType().GetField(Name);
            if (info != null && !info.IsStatic)
            {
                info.SetValue(this, Value);
            }
        }

        public void SetPropertyValue(string Name, object Value)
        {
            PropertyInfo info = this.GetType().GetProperty(Name);
            if (info != null && info.CanWrite)
            {
                info.SetValue(this, Value, null);
            }
        }

        public void Assign(object o)
        {
            if (o == null)
            {
                return;
            }

            FieldInfo[] Fields = o.GetType().GetFields();
            PropertyInfo[] Properties = o.GetType().GetProperties();

            for (int i = 0; i < Fields.Length; i++)
            { SetFieldValue(Fields[i].Name, Fields[i].GetValue(o)); }

            for (int i = 0; i < Properties.Length; i++)
            {
                string name = Properties[i].Name;
                object value = Properties[i].GetValue(o, null);
                SetPropertyValue(name, value);
            }
        }
    }
}
