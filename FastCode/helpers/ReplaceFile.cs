using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastCode.Helpers
{
    public class ReplaceFile
    {
        public static void run(string file, string folder) {
            Log.Text("ReplaceFile");
            Log.Text("file:"+file);
            Log.Text("folder:"+folder);

            if (!System.IO.File.Exists(file))
            {
                Log.Error("arquivo:"+file+" não existe");
            }

            if (!System.IO.Directory.Exists(folder))
            {
                Log.Error("pasta:"+folder + " não existe");
            }

            if (System.IO.File.Exists(file) && System.IO.Directory.Exists(folder)) {
                Log.Text("start replace");
                string fileContent=System.IO.File.ReadAllText(file);
                string[] filesByFolder= System.IO.Directory.GetFiles(folder);
                foreach (var item in filesByFolder)
                {
                    string itemContent = System.IO.File.ReadAllText(item);
                    string itemName = System.IO.Path.GetFileName(item);
                    Log.Text("item:"+ itemName);
                    string search = "{{"+ itemName + "}}";
                    fileContent = fileContent.Replace(search, itemContent);
                }
                Log.Text("replace concluído com sucesso!", ConsoleColor.Green);
                System.IO.File.WriteAllText(file, fileContent);
            }
        }
    }
}
