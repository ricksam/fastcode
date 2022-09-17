using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FastCode.helpers
{
    public class Compiler
    {
        private Makefile Makefile { get; set; }
        private ProjectScheme Project { get; set; }
        private List<Template> Templates { get; set; } = new List<Template>();
        public void LoadMakefile(string path)
        {
            Log.Text("Load MakeFile: " + path, ConsoleColor.DarkMagenta);
            if (!File.Exists(path))
            {
                throw new Exception("Makefile não encontrato:" + path);
            }

            string makefile_json = File.ReadAllText(path);
            this.Makefile = Newtonsoft.Json.JsonConvert.DeserializeObject<Makefile>(makefile_json);

            if (!File.Exists(this.Makefile.project))
            {
                throw new Exception("Project não encontrato:" + this.Makefile.project);
            }
            if (!Directory.Exists(this.Makefile.templates))
            {
                throw new Exception("Pasta de templates não encontrata:" + this.Makefile.templates);
            }

            if (!Directory.Exists(this.Makefile.target))
            {
                Directory.CreateDirectory(this.Makefile.target);
            }
        }

        public void LoadProject()
        {
            Log.Text("Load Project: " + this.Makefile.project, ConsoleColor.DarkMagenta);
            string project_json = File.ReadAllText(this.Makefile.project);
            this.Project = Newtonsoft.Json.JsonConvert.DeserializeObject<ProjectScheme>(project_json);
        }

        public void LoadTemplates()
        {
            string[] files = Directory.GetFiles(this.Makefile.templates);
            foreach (var item in files)
            {
                Log.Text("Load Template: " + item, ConsoleColor.DarkMagenta);
                string template_json = File.ReadAllText(item);
                this.Templates.Add(Newtonsoft.Json.JsonConvert.DeserializeObject<Template>(template_json));

            }
        }

        public void Build(bool force)
        {
            foreach (var role in this.Makefile.roles)
            {
                Template template = this.Templates.FirstOrDefault(q => q.name == role.template);

                if (role.command.Contains("["))
                {
                    foreach (var entity in this.Project.entities)
                    {
                        string concat = "";
                        string new_code = "";

                        foreach (var line in template.code.Split("\n"))
                        {
                            if (string.IsNullOrEmpty(line))
                            {
                                continue;
                            }

                            Log.Process(line, ConsoleColor.Yellow);

                            string type = line.Substring(0, 1);
                            string code = line.Remove(0, 1);
                            

                            if (type == " ")
                            {
                                new_code += ReplaceCode.ByEntity(entity, this.Project, code) + "\n";
                            }
                            else if (type == "+")
                            {
                                concat += code+ "\n";
                                continue;
                            }
                            else if (type == "*")
                            {
                                concat += code;
                                new_code += ReplaceCode.ByFieldList(template, entity, this.Project, concat);
                                concat = "";
                            }
                            else if (type == "^")
                            {
                                concat += code;
                                new_code += ReplaceCode.ByEntityList(this.Project, concat);
                                concat = "";
                            }
                        }

                        SaveFile(this.Makefile.target + "/" + role.command.Replace("[entity]", entity.name), new_code, force);
                    }
                }
                else {
                    string concat = "";
                    string new_code = "";
                    foreach (var line in template.code.Split("\n"))
                    {
                        if (string.IsNullOrEmpty(line))
                        {
                            continue;
                        }

                        string type = line.Substring(0, 1);
                        string code = line.Remove(0, 1);

                        if (type == " ")
                        {
                            new_code += ReplaceCode.ByEntity(null, this.Project, code) + "\n";
                        }
                        else if (type == "+")
                        {
                            concat += code;
                            continue;
                        }
                        else if (type == "^")
                        {
                            concat += code;
                            new_code += ReplaceCode.ByEntityList(this.Project, concat);
                            concat = "";
                        }
                    }

                    SaveFile(this.Makefile.target + "/" + role.command, new_code, force);
                }

            }
        }

        public void SaveFile(string path, string code, bool force) {
            string code_dir = Path.GetDirectoryName(path);
            if (!Directory.Exists(code_dir))
            {
                Directory.CreateDirectory(code_dir);
            }

            if (force) {
                File.WriteAllBytes(path, Encoding.Default.GetBytes(code));
                Log.Text(path, ConsoleColor.Green);
            }
            else
            {
                if (!File.Exists(path))
                {
                    File.WriteAllBytes(path, Encoding.Default.GetBytes(code));
                    Log.Text(path, ConsoleColor.DarkGreen);
                }
                else
                {
                    Log.Text("File exists :" + path, ConsoleColor.DarkRed);
                }
            }
            
        }
    }
}
