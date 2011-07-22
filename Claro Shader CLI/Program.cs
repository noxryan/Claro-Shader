using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Claro_Shader_Core;

namespace Claro_Shader_CLI
{
    class Program
    {
        static string imgExcludeFile = Path.Combine("Resources", "img_excludes.txt");
        static string cssExcludeFile = Path.Combine("Resources", "css_excludes.txt");

        static void Main(string[] args)
        {
            Dictionary<string, string> cliArgs = new Dictionary<string, string>();
            try
            {
                string key = "";
                foreach (string arg in args)
                {
                    if (arg.StartsWith("-"))
                        key = arg.Substring(1).ToLower();
                    else
                        cliArgs.Add(key, arg);
                }
            }
            catch (Exception e)
            {
                Console.Write("Invalid Arguments" + Environment.NewLine + e);
                Environment.Exit(0);
            }

            Shader.Log += new Shader.LogEventHandler(Shader_Log);

            using (StreamReader sr = new StreamReader(imgExcludeFile))
            {
                String line;
                while ((line = sr.ReadLine()) != null)
                {
                    Shader.ImgExcludes.Add(line);
                }
            }
            using (StreamReader sr = new StreamReader(cssExcludeFile))
            {
                String line;
                while ((line = sr.ReadLine()) != null)
                {
                    Shader.CssExcludes.Add(line);
                }
            }

            if(Directory.Exists(cliArgs["p"]))
                Shader.ClaroPath = cliArgs["p"];
            else
                Console.WriteLine("Invalid path to Claro.");

            try
            {
                Shader.H = Int32.Parse(cliArgs["h"]);
                Shader.S = Int32.Parse(cliArgs["s"]);
                Shader.L = Int32.Parse(cliArgs["l"]);
                Shader.Invert = Boolean.Parse(cliArgs["i"]);
                Shader.KeepBlacks = Boolean.Parse(cliArgs["kb"]);
                Shader.KeepGrays = Boolean.Parse(cliArgs["kg"]);
                Shader.GrayTolerance = Int32.Parse(cliArgs["gt"]);
            }
            catch (Exception e) { }

            Shader.Start(true);
        }

        static void Shader_Log(string msg)
        {
            Console.WriteLine(msg);
        }
    }
}
