using FastCode.Helpers;
using System;
using System.IO;

namespace FastCode
{
    class Program
    {
        static void Main(string[] args)
        {            
            try {
                Log.Title("FastCode 1.0", ConsoleColor.DarkCyan);
                string makefile_path = "";
                bool force = false;

                string DefinedProject = "";
                string DefinedTemplates = "";
                string DefinedTarget = "";

                if (args.Length == 0)
                {
                    ShowHelp();
                    Log.Text("Informe o arquivo makefile!", ConsoleColor.Blue);
                    makefile_path=Console.ReadLine();
                }
                else if (args.Length >= 1)
                {
                    if (args[0] == "-replace" && args.Length==3) {
                        ReplaceFile.run(args[1], args[2]);
                        return;
                    }

                    makefile_path = args[0];

                    for (int i = 1; i < args.Length; i++)
                    {
                        if (args[i] == "-h"|| args[i] == "-help"|| args[i] == "--help")
                        {
                            ShowHelp();
                        }

                        if (args[i] == "-force") {
                            force = true;
                        }

                        if (args[i] == "-project") {
                            DefinedProject = args[i + 1];
                        }
                        if (args[i] == "-templates") {
                            DefinedTemplates = args[i + 1];
                        }
                        if (args[i] == "-target") {
                            DefinedTarget = args[i + 1];
                        }
                    }
                }

                if (!File.Exists(makefile_path))
                {
                    ShowHelp();
                    Log.Text("makefile não informado ou não existe!", ConsoleColor.Red);
                    return;
                }

                Helpers.Compiler compiler = new Helpers.Compiler();
                compiler.LoadMakefile(makefile_path, DefinedProject, DefinedTemplates, DefinedTarget);
                compiler.LoadProject();
                compiler.LoadTemplates();
                compiler.Build(force);

            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
            
        }

        static void ShowHelp() {
            Log.Text("Exemplo de comandos:", ConsoleColor.Yellow);
            Log.Text("   fastcode.exe <makefile.json> [-help -force -project <project> -templates <templates> - target <target>]", ConsoleColor.Yellow);
            Log.Text("   ", ConsoleColor.Yellow);
            Log.Text("Comandos opcionais:", ConsoleColor.Yellow);
            Log.Text(" - help: Exibe a lista de comandos.", ConsoleColor.Yellow);
            Log.Text(" - force: Ignora as instruções para \"Sobrescrever\" arquivos e sobrescreve todos arquivos.", ConsoleColor.Yellow);
            Log.Text(" - project: Define o caminho do project.json e ignora o caminho informado no makefile.", ConsoleColor.Yellow);
            Log.Text(" - templates: Define o caminho da pasta de templates e ignora o caminho informado no makefile.", ConsoleColor.Yellow);
            Log.Text(" - target: Define o caminho da pasta alvo e ignora o caminho informado no makefile.", ConsoleColor.Yellow);
        }
    }
}
