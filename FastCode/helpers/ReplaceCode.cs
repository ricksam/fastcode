using FastCode.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastCode.Helpers
{
    public class ReplaceCode
    {
        public static string[] DefaultFieldTypes = new string[] { "pk", "number", "value", "text", "bool", "date", "time", "file", "password" };

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

        public static string ToCamelCase(string input)
        {
            // Converte o primeiro caractere para minúsculo
            string firstCharLower = input.Substring(0, 1).ToLower();

            // Concatena o primeiro caractere em minúsculo com o restante da string
            string camelCase = firstCharLower + input.Substring(1);

            return camelCase;
        }


        public static string ReplaceByProperty(string line, string property, string value)
        {
            if (value == null) {
                value = "";
            }

            if (line.Contains("[" + property + "](lower)"))
            {
                line = line.Replace("[" + property + "](lower)", value.ToLower());
            }
            if (line.Contains("[" + property + "](upper)"))
            {
                line = line.Replace("[" + property + "](upper)", value.ToUpper());
            }
            if (line.Contains("[" + property + "](camel)"))
            {
                line = line.Replace("[" + property + "](camel)", ToCamelCase(value));
            }
            return line.Replace("[" + property + "]", value);
        }

        private static string ReplaceByType(TemplateField t_field, string new_line, bool isFirst, bool isLast, string text_replace)
        {
            if ((isFirst && !t_field.role.first) || (isLast && !t_field.role.last) || (!isFirst && !isLast && !t_field.role.outhers))
            {
                return new_line.Replace("[" + t_field.name + "]", "");
            }
            return new_line.Replace("[" + t_field.name + "]", text_replace);
        }

        public static string ByEntity(Entity entity, ProjectScheme project, string line)
        {
            string[] SearchFields = new string[] { "namespace", "entity", "entity.name", "entity.title", "entity.index", "entity.count", "pk", "field", "field.name" };
            while (HasField(line, SearchFields))
            {
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
                    //line = line.Replace(existing, existing.Replace("[", "").Replace("]", "")); // evita loop
                    //Log.Text("field invalid:" + existing, ConsoleColor.Red);
                    throw new Exception("entity invalid:" + existing);
                }
            }
            return line;
        }

        public static string ByEntityList(ProjectScheme project, string line, Template template)
        {
            string new_lines = "";

            for (int i = 0; i < project.entities.Count; i++)
            {
                bool IsFirst = i == 0;
                bool IsLast = i == project.entities.Count - 1;

                string new_line = ByTemplateField(line, project, template,new EntityField(), IsFirst, IsLast);

                new_line = ByEntity(project.entities[i], project, new_line);

                if (!string.IsNullOrEmpty(new_line.Trim()))
                {
                    new_lines += new_line + "\n";
                }
            }

            return new_lines;
        }

        public static string ByTemplateField(string line, ProjectScheme project, Template template, EntityField field, bool FieldIsFirst, bool FieldIsLast) {
            string[] template_fields_name = template.fields.Select(q => q.name).ToArray();
            while (HasField(line, template_fields_name))
            {
                string existing = GetExistingitem(line, template_fields_name);
                foreach (var t_field in template.fields)
                {
                    if (existing == "[" + t_field.name + "]")
                    {
                        if (field.type == "text"|| string.IsNullOrEmpty(field.type))
                        {
                            line = ReplaceByType(t_field, line, FieldIsFirst, FieldIsLast, t_field.type.text);
                        }
                        else if (field.type == "number")
                        {
                            line = ReplaceByType(t_field, line, FieldIsFirst, FieldIsLast, t_field.type.number);
                        }
                        else if (field.type == "value")
                        {
                            line = ReplaceByType(t_field, line, FieldIsFirst, FieldIsLast, t_field.type.value);
                        }
                         
                        else if (field.type == "bool")
                        {
                            line = ReplaceByType(t_field, line, FieldIsFirst, FieldIsLast, t_field.type.@bool);
                        }
                        else if (field.type == "date")
                        {
                            line = ReplaceByType(t_field, line, FieldIsFirst, FieldIsLast, t_field.type.date);
                        }
                        else if (field.type == "time")
                        {
                            line = ReplaceByType(t_field, line, FieldIsFirst, FieldIsLast, t_field.type.time);
                        }
                        else if (field.type == "file")
                        {
                            line = ReplaceByType(t_field, line, FieldIsFirst, FieldIsLast, t_field.type.file);
                        }
                        else if (field.type == "password")
                        {
                            line = ReplaceByType(t_field, line, FieldIsFirst, FieldIsLast, t_field.type.password);
                        }
                        else if (project.entities.FirstOrDefault(q => q.name == field.type) != null)
                        {
                            line = ReplaceByType(t_field, line, FieldIsFirst, FieldIsLast, t_field.type.fk);
                        }
                        else
                        {
                            //line = line.Replace(existing, existing.Replace("[", "").Replace("]", "")); // evita loop
                            //Log.Text("field.type not exist: " + field.type, ConsoleColor.Red);
                            throw new Exception("field.type not exist: " + field.type);
                        }
                    }
                }
            }
            return line;
        }

        public static string ByFieldListFilter(Template template, Entity entity, ProjectScheme project, string line, List<EntityField> fields)
        {
            string[] SearchFields = new string[] { "field", "field.name", "field.title", "field.size", "field.type", "field.role", "field.placeholder", "field.index", "field.count" };
            string new_lines = "";
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
                            new_line = ReplaceByProperty(new_line, "field.role", field.role);
                        }
                        else if (existing == "[field.placeholder]")
                        {
                            new_line = ReplaceByProperty(new_line, "field.placeholder", field.placeholder);
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
                            new_line = new_line.Replace(existing, existing.Replace("[", "").Replace("]", "")); // evita loop
                            Log.Text("field invalid:" + existing, ConsoleColor.Red);
                        }
                    }

                    new_line = ByTemplateField(new_line, project, template, field, FieldIsFirst, FieldIsLast);
                } while (HasField(new_line, SearchFields) || HasField(new_line, template_fields));

                if (!string.IsNullOrEmpty(new_line.Trim()))
                {
                    new_lines += new_line + "\n";
                }
            }


            new_lines = ByEntity(entity, project, new_lines);
            return new_lines;
        }

        // Todos campos exceto pk
        public static string ByFieldList(Template template, Entity entity, ProjectScheme project, string line)
        {
            return ByFieldListFilter(template, entity, project, line, entity.fields.Where(q => q.type != "pk").ToList());
        }

        // Todos campos exceto pk e fk
        public static string ByFieldNotKeys(Template template, Entity entity, ProjectScheme project, string line)
        {
            return ByFieldListFilter(template, entity, project, line, entity.fields.Where(q => q.type != "pk" && DefaultFieldTypes.Contains(q.type)).ToList());
        }

        // Todas as pks
        public static string ByPrimaryKeys(Template template, Entity entity, ProjectScheme project, string line)
        {
            return ByFieldListFilter(template, entity, project, line, entity.fields.Where(q => q.type == "pk").ToList());
        }

        // Todas as fks
        public static string ByForeignKeys(Template template, Entity entity, ProjectScheme project, string line)
        {
            return ByFieldListFilter(template, entity, project, line, entity.fields.Where(q => !DefaultFieldTypes.Contains(q.type)).ToList());
        }
    }
}
