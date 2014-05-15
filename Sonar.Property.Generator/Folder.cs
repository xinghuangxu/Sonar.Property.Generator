using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Sonar.Property.Generator
{
    class Folder
    {
        private string path;
        private List<Folder> folders = new List<Folder>();

        public string getSource(){
            int start = path.IndexOf("Application");
            return path.Substring(start+12);
        }



        public Folder(string path, Dictionary<string, List<Folder>> dic)
        {
            this.path = path;
            string[] folders = Directory.GetDirectories(path);
            foreach (string fold in folders)
            {
                string name = fold.Substring(fold.LastIndexOf('\\') + 1);

                if (name[0] != '.')
                {
                    Folder nf=new Folder(fold,dic);
                    this.folders.Add(nf);
                    if (!dic.ContainsKey(name))
                    {
                        dic[name] = new List<Folder>(); 
                    }
                    dic[name].Add(nf);
                }
            }
        }

    }
}
