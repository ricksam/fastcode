using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using FastCode.Models;
using System.Diagnostics;

namespace FastCode.Helpers
{
    public class Compiler
    {
        private Makefile Makefile { get; set; }
        private ProjectScheme Project { get; set; }
        private List<Template> Templates { get; set; } = new List<Template>();
        public void LoadMakefile(string path, string DefinedProject, string DefinedTemplates, string DefinedTarget)
        {
            Log.Text("Load MakeFile: " + path, ConsoleColor.DarkMagenta);
            if (!File.Exists(path))
            {
                throw new Exception("Makefile não encontrato:" + path);
            }

            string makefile_json = File.ReadAllText(path);
            this.Makefile = Newtonsoft.Json.JsonConvert.DeserializeObject<Makefile>(makefile_json);

            if (!string.IsNullOrEmpty(DefinedProject))
            {
                this.Makefile.project = DefinedProject;
            }

            if (!string.IsNullOrEmpty(DefinedTemplates)) {
                this.Makefile.templates = DefinedTemplates;
            }

            if (!string.IsNullOrEmpty(DefinedTarget)) {
                this.Makefile.target = DefinedTarget;
            }

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

                if (template.name == "dashboard.repository")
                {
                    Debug.WriteLine(template.name);
                }

                if (template == null)
                {
                    throw new Exception(role.template + " não encontrado");
                }

                if (role.command.Contains("["))
                {
                    foreach (var entity in this.Project.entities)
                    {
                        string concat = "";
                        string new_code = "";
                        int lineCounter = 0;
                        foreach (var line in template.code.Split("\n"))
                        {
                            lineCounter++;
                            try {
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
                                    concat += code + "\n";
                                    continue;
                                }

                                else if (type == "^") // todas as entidades
                                {
                                    concat += code;
                                    new_code += ReplaceCode.ByEntityList(this.Project, concat, template);
                                    concat = "";
                                }
                                else if (type == "*") // todos campos exceto primary keys
                                {
                                    concat += code;
                                    new_code += ReplaceCode.ByFieldList(template, entity, this.Project, concat);
                                    concat = "";
                                }
                                else if (type == "$") // todos campos exceto primary keys e foreign keys
                                {
                                    concat += code;
                                    new_code += ReplaceCode.ByFieldNotKeys(template, entity, this.Project, concat);
                                    concat = "";
                                }
                                else if (type == "#") // todas as primary keys
                                {
                                    concat += code;
                                    new_code += ReplaceCode.ByPrimaryKeys(template, entity, this.Project, concat);
                                    concat = "";
                                }
                                else if (type == "@") // todas as foreign keys
                                {
                                    concat += code;
                                    new_code += ReplaceCode.ByForeignKeys(template, entity, this.Project, concat);
                                    concat = "";
                                }
                                else
                                {
                                    throw new Exception("linha inválida: " + line + ". As linhas devem começar com ' ', '+', '*', '^', '#' ou '@'");
                                }
                            } catch (Exception ex) {
                                Log.Line(role.template + " " + lineCounter + ":" + line);
                                Log.Error(ex.Message);
                                throw ex;
                            }
                            
                        }

                        string nameFile = ReplaceCode.ReplaceByProperty(role.command, "entity", entity.name);
                        nameFile = nameFile.Replace("[namespace]", this.Project.name);
                        SaveFile(this.Makefile.target + "/" + nameFile, new_code, role.overwrite || force);
                    }
                }
                else
                {
                    string concat = "";
                    string new_code = "";
                    int lineCounter = 0;
                    foreach (var line in template.code.Split("\n"))
                    {
                        lineCounter++;
                        try {
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
                                concat += code + "\n";
                                continue;
                            }
                            else if (type == "^")
                            {
                                concat += code;
                                new_code += ReplaceCode.ByEntityList(this.Project, concat, template);
                                concat = "";
                            }
                        } catch (Exception ex) {
                            Log.Line(role.template+" "+ lineCounter + ":"+ line);
                            Log.Error(ex.Message);
                            throw ex;
                        }
                        
                    }

                    SaveFile(this.Makefile.target + "/" + role.command, new_code, role.overwrite || force);
                }

            }
        }

        public void SaveFile(string path, string code, bool force)
        {
            string code_dir = Path.GetDirectoryName(path);
            if (!Directory.Exists(code_dir))
            {
                Directory.CreateDirectory(code_dir);
            }

            if (force)
            {
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
                    Log.Text("File already exists :" + path, ConsoleColor.DarkGray);
                }
            }

        }
    }
}
