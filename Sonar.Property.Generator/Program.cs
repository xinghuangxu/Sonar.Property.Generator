using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Sonar.Property.Generator
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, List<Folder>> dic = new Dictionary<string, List<Folder>>();
            Folder folder = new Folder("Application", dic);

            List<Property> properties = new List<Property>();
            using (StreamReader sr = new StreamReader("config.csv"))
            {
                string line = sr.ReadLine();
                Property p = new Property("");
                while ((line = sr.ReadLine()) != null)
                {
                    string[] vals = line.Split(',');
                    if (p.Name != vals[0])
                    {
                        p = new Property(vals[0]);
                        properties.Add(p);
                    }
                    p.AddComponent(vals[1]);
                }
            }
            foreach (Property p in properties)
            {
                p.writeFile(dic);
            }
            //string[] files;
            //files = Directory.GetFiles("config");
            //foreach (string f in files)
            //{
            //    Property p = new Property(f);
            //    p.writeFile(dic);
            //    properties.Add(p);
            //}
            createRootPropertyFile(properties);
            return;
        }

        static void createRootPropertyFile(List<Property> pl)
        {
            using (StreamWriter sw = new StreamWriter("sonar-project.properties"))
            {
                sw.WriteLine("sonar.projectKey=epg.netapp.com:RAIDCore_" + "Kingston");
                sw.WriteLine("sonar.projectName=RAIDCore_Kingston");
                sw.WriteLine("sonar.projectVersion=3281");
                sw.WriteLine("sonar.language=c++");
                sw.WriteLine("sonar.sourceEncoding=UTF-8");
                List<string> effectiveModels = new List<string>();
                StringBuilder sb = new StringBuilder();
                foreach (Property p in pl)
                {
                    if (!p.IsEmpty)
                    {
                        sb.Append(p.Name + ".sonar.projectBaseDir=modules/" + p.Name + "\n");
                        effectiveModels.Add(p.Name);
                    }
                }
                if (effectiveModels.Count > 0)
                {
                    sw.WriteLine("sonar.modules=" + String.Join(",", effectiveModels.ToArray()));
                    sw.WriteLine(sb.ToString());
                }
            }
        }
    }
}
