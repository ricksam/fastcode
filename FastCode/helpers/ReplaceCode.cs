using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastCode.helpers
{
    public class ReplaceCode
    {
        private static string GetExistingitem(string line, string[] fields)
        {
            foreach (var item in fields)
            {
                string search = "[" + item + "]";
                if (line.Contains(search))
                {
                    Log.Process(search, ConsoleColor.Yellow);
                    return search;
                }
            }

            return "";
        }

        private static bool HasField(string line, string[] fields)
        {
            return !string.IsNullOrEmpty(GetExistingitem(line, fields));
        }

        

        private static string ReplaceByProperty(string line, string property, string value)
        {
            if (line.Contains("[" + property + "](lower)")) {
                line = line.Replace("["+property+"](lower)", value.ToLower());
            }
            if (line.Contains("[" + property + "](upper)"))
            {
                line = line.Replace("[" + property + "](upper)", value.ToLower());
            }
            if (line.Contains("[" + property + "](camel)"))
            {
                line = line.Replace("[" + property + "](camel)", value.ToLower());
            }
            return line.Replace("["+property+"]", value).Replace("["+property+"]", value);
        }

        public static string ByEntity(Entity entity, ProjectScheme project, string line)
        {
            string[] SearchFields = new string[] { "namespace", "entity", "entity.name", "entity.title", "entity.index", "entity.count", "pk", "field", "field.name" };
            while (HasField(line, SearchFields))
            {
                //,  "field", "field.index", "field.size", "field.count", "pk"
                string existing = GetExistingitem(line, SearchFields);
                if (existing == "[namespace]")
                {
                    line = line.Replace("[namespace]", project.name);
                }
                else if (existing == "[entity]" || existing == "[entity.name]")
                {
                    line = ReplaceByProperty(line, "entity", entity.name);
                    line = ReplaceByProperty(line, "entity.name", entity.name);
                }
                else if (existing == "[entity.title]")
                {
                    line = line.Replace("[entity.title]", entity.title);
                }
                else if (existing == "[entity.index]")
                {
                    line = line.Replace("[entity.index]", project.entities.IndexOf(entity).ToString());
                }
                else if (existing == "[entity.count]")
                {
                    line = line.Replace("[entity.count]", project.entities.Count.ToString());
                }
                else if (existing == "[pk]")
                {
                    line = ReplaceByProperty(line, "pk", entity.fields.FirstOrDefault(q => q.type == "pk")?.name);
                }
                else if (existing == "[field]" || existing == "[field.name]")
                {
                    var fields = entity.fields.Where(q => q.type != "pk").ToList();
                    if (fields.Count >= 1)
                    {
                        line = ReplaceByProperty(line, "field", fields[0].name);
                        line = ReplaceByProperty(line, "field.name", fields[0].name);
                    }
                }
                else
                {
                    Log.Text("field invalid:" + existing, ConsoleColor.Red);
                }
            }
            return line;
        }



        private static string ReplaceByType(TemplateField t_field, string new_line, bool isFirst, bool isLast, string text_replace)
        {
            if ((isFirst && !t_field.role.first) || (isLast && !t_field.role.last) || (!isFirst && !isLast && !t_field.role.outhers))
            {
                return new_line.Replace("[" + t_field.name + "]", "");
            }
            return new_line.Replace("[" + t_field.name + "]", text_replace);
        }

        public static string ByFieldList(Template template, Entity entity, ProjectScheme project, string line)
        {
            string[] SearchFields = new string[] { "field", "field.name", "field.title", "field.size", "field.type", "field.role", "field.placeholder", "field.index", "field.count" };
            string new_lines = "";
            var fields = entity.fields.Where(q => q.type != "pk").ToList();
            for (int i = 0; i < fields.Count; i++)
            {
                EntityField field = fields[i];
                bool FieldIsFirst = i == 0;
                bool FieldIsLast = i == fields.Count - 1;

                string new_line = line;

                string[] template_fields = template.fields.Select(q => q.name).ToArray();

                do
                {
                    while (HasField(new_line, SearchFields))
                    {
                        string existing = GetExistingitem(new_line, SearchFields);
                        if (existing == "[field]" || existing == "[field.name]")
                        {
                            new_line = ReplaceByProperty(new_line, "field", field.name);
                            new_line = ReplaceByProperty(new_line, "field.name", field.name);
                        }
                        else if (existing == "[field.title]")
                        {
                            new_line = new_line.Replace("[field.title]", field.title);
                        }
                        else if (existing == "[field.size]")
                        {
                            new_line = new_line.Replace("[field.size]", field.size);
                        }
                        else if (existing == "[field.type]")
                        {
                            new_line = ReplaceByProperty(new_line, "field.type", field.type);
                        }
                        else if (existing == "[field.role]")
                        {
                            new_line = new_line.Replace("[field.role]", field.role);
                        }
                        else if (existing == "[field.placeholder]")
                        {
                            new_line = new_line.Replace("[field.placeholder]", field.placeholder);
                        }
                        else if (existing == "[field.index]")
                        {
                            new_line = new_line.Replace("[field.index]", entity.fields.IndexOf(field).ToString());
                        }
                        else if (existing == "[field.count]")
                        {
                            new_line = new_line.Replace("[field.count]", entity.fields.Count.ToString());
                        }
                        else
                        {
                            Log.Text("field invalid:" + existing, ConsoleColor.Red);
                        }
                    }

                    while (HasField(new_line, template_fields))
                    {
                        string existing = GetExistingitem(new_line, template_fields);
                        foreach (var t_field in template.fields)
                        {
                            if (existing == "[" + t_field.name + "]")
                            {
                                if (field.type == "number")
                                {
                                    new_line = ReplaceByType(t_field, new_line, FieldIsFirst, FieldIsLast, t_field.type.number);
                                }
                                else if (field.type == "value")
                                {
                                    new_line = ReplaceByType(t_field, new_line, FieldIsFirst, FieldIsLast, t_field.type.value);
                                }
                                else if (field.type == "text")
                                {
                                    new_line = ReplaceByType(t_field, new_line, FieldIsFirst, FieldIsLast, t_field.type.text);
                                }
                                else if (field.type == "bool")
                                {
                                    new_line = ReplaceByType(t_field, new_line, FieldIsFirst, FieldIsLast, t_field.type.@bool);
                                }
                                else if (field.type == "date")
                                {
                                    new_line = ReplaceByType(t_field, new_line, FieldIsFirst, FieldIsLast, t_field.type.date);
                                }
                                else if (field.type == "time")
                                {
                                    new_line = ReplaceByType(t_field, new_line, FieldIsFirst, FieldIsLast, t_field.type.time);
                                }
                                else if (field.type == "file")
                                {
                                    new_line = ReplaceByType(t_field, new_line, FieldIsFirst, FieldIsLast, t_field.type.file);
                                }
                                else if (field.type == "password")
                                {
                                    new_line = ReplaceByType(t_field, new_line, FieldIsFirst, FieldIsLast, t_field.type.password);
                                }
                                else if (project.entities.FirstOrDefault(q => q.name == field.type) != null)
                                {
                                    new_line = ReplaceByType(t_field, new_line, FieldIsFirst, FieldIsLast, t_field.type.fk);
                                }
                                else
                                {
                                    Log.Text("field.type not exist: " + field.type, ConsoleColor.Red);
                                }
                            }
                        }
                    }
                } while (HasField(new_line, SearchFields) || HasField(new_line, template_fields));

                if (!string.IsNullOrEmpty(new_line.Trim()))
                {
                    new_lines += new_line + "\n";
                }
            }


            new_lines = ByEntity(entity, project, new_lines);
            return new_lines;
        }

        public static string ByEntityList(ProjectScheme project, string line)
        {
            string[] SearchFields = new string[] { "namespace", "entity", "entity.name", "entity.title", "entity.index", "entity.count", "pk", "field", "field.name" };
            string new_lines = "";

            foreach (var entity in project.entities)
            {
                string new_line = line;
                while (HasField(new_line, SearchFields))
                {
                    string existing = GetExistingitem(new_line, SearchFields);
                    if (existing == "[namespace]")
                    {
                        new_line = new_line.Replace("[namespace]", project.name);
                    }
                    else if (existing == "[entity]" || existing == "[entity.name]")
                    {
                        new_line = ReplaceByProperty(new_line, "entity", entity.name);
                        new_line = ReplaceByProperty(new_line, "entity.name", entity.name);
                    }
                    else if (existing == "[entity.title]")
                    {
                        new_line = new_line.Replace("[entity.title]", entity.title);
                    }
                    else if (existing == "[entity.index]")
                    {
                        new_line = new_line.Replace("[entity.index]", project.entities.IndexOf(entity).ToString());
                    }
                    else if (existing == "[entity.count]")
                    {
                        new_line = new_line.Replace("[entity.count]", project.entities.Count.ToString());
                    }
                    else if (existing == "[pk]")
                    {
                        new_line = ReplaceByProperty(new_line, "pk", entity.fields.FirstOrDefault(q => q.type == "pk")?.name);
                    }
                    else if (existing == "[field]" || existing == "[field.name]")
                    {
                        var fields = entity.fields.Where(q => q.type != "pk").ToList();
                        if (fields.Count >= 1)
                        {
                            new_line = ReplaceByProperty(new_line, "field", fields[0].name);
                            new_line = ReplaceByProperty(new_line, "field.name", fields[0].name);
                        }
                    }
                    else
                    {
                        Log.Text("field invalid:" + existing, ConsoleColor.Red);
                    }
                }

                if (!string.IsNullOrEmpty(new_line.Trim()))
                {
                    new_lines += new_line + "\n";
                }
            }

            return new_lines;
        }
    }
}
