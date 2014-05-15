using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Sonar.Property.Generator
{
    class Property
    {
        static string DIR = "modules";
        static string SONAR_PROPERTY_FILENAME = "sonar-project.properties";
        public bool IsEmpty { get; private set; }
        public string Name { get; private set; }

        private List<string> components=new List<string>();

        public Property(string name)
        {
            this.Name = name;
        }

        //public Property(string filename)
        //{
        //    this.IsEmpty = false;
        //    this.Name = filename.Substring(filename.LastIndexOf('\\')+1);
        //    this.Name = this.Name.Substring(0, this.Name.LastIndexOf('.'));
        //    using (StreamReader sr = new StreamReader(filename))
        //    {
        //        string line;
        //        while ((line = sr.ReadLine())!= null)
        //        {
        //            this.components.Add(line);
        //        }
        //    }

        //}

        public void AddComponent(string c)
        {
            this.components.Add(c);
        }

        public void writeFile(Dictionary<string, List<Folder>> dic)
        {
            string path=DIR+'\\'+this.Name;
            Directory.CreateDirectory(path);
            using (StreamWriter sw = new StreamWriter(path + '\\'+SONAR_PROPERTY_FILENAME))
            {
                sw.WriteLine("sonar.projectKey=epg.netapp.com:RAIDCore_"+this.Name);
                sw.WriteLine("sonar.projectName="+this.Name);
                sw.WriteLine("sonar.projectVersion=3281");
                sw.WriteLine("sonar.language=c++");
                sw.WriteLine("sonar.sourceEncoding=UTF-8");
                List<string> effectiveModels = new List<string>();
                StringBuilder sb = new StringBuilder();
                foreach (string c in this.components)
                {
                    if (dic.ContainsKey(c))
                    {
                        effectiveModels.Add(c);
                        List<Folder> fls = dic[c];
                        sb.Append(c + ".sonar.projectBaseDir=../../Application\n");
                        //sw.WriteLine(c + ".sonar.projectBaseDir=../../Application");
                        string sources;
                        sources = fls[0].getSource();
                        if (fls.Count > 1)
                        {
                            sources += "," + fls[1].getSource();
                        }
                        sb.Append(c + ".sonar.sources=" + sources.Replace("\\", "/")+"\n");
                        //sw.WriteLine(c + ".sonar.sources=" + sources.Replace("\\","/"));
                    }
                }
                if (effectiveModels.Count > 0)
                {
                    sw.WriteLine("sonar.modules=" + String.Join(",", effectiveModels.ToArray()));
                    sw.WriteLine(sb.ToString());
                }
                else
                {
                    this.IsEmpty = true;
                }
            }
        }
    }
}
