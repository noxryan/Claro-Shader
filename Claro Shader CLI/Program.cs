/*Copyright 2011 Ryan Schlesinger. All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are
permitted provided that the following conditions are met:

1. Redistributions of source code must retain the above copyright notice, this list of
conditions and the following disclaimer.

2. Redistributions in binary form must reproduce the above copyright notice, this list
of conditions and the following disclaimer in the documentation and/or other materials
provided with the distribution.

THIS SOFTWARE IS PROVIDED BY Ryan Schlesinger ``AS IS'' AND ANY EXPRESS OR IMPLIED
WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL Ryan Schlesinger OR
CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON
ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF
ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

The views and conclusions contained in the software and documentation are those of the
authors and should not be interpreted as representing official policies, either expressed
or implied, of Ryan Schlesinger.*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
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
                    if (arg.StartsWith("-") && arg.Contains("="))
                        cliArgs.Add(arg.Split('=')[0].Substring(1).ToLower(), arg.Split('=')[1]);
                    else if (arg.StartsWith("-"))
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


            string outputPath;
            if (cliArgs.TryGetValue("o", out outputPath))
                Shader.OutputPath = outputPath;

            try { Shader.H = Int32.Parse(cliArgs["h"]); } catch (Exception e) { }
            try { Shader.S = Int32.Parse(cliArgs["s"]); } catch (Exception e) { }
            try { Shader.L = Int32.Parse(cliArgs["l"]); } catch (Exception e) { }
            try { Shader.Invert = Boolean.Parse(cliArgs["i"]); } catch (Exception e) { }
            try { Shader.KeepBlacks = Boolean.Parse(cliArgs["kb"]); } catch (Exception e) { }
            try { Shader.KeepGrays = Boolean.Parse(cliArgs["kg"]); } catch (Exception e) { }
            try { Shader.GrayTolerance = Int32.Parse(cliArgs["gt"]); } catch (Exception e) { }

            Shader.Start();

            string pathToLess;
            if (cliArgs.TryGetValue("pl", out pathToLess))
            {
                string outPath = Path.Combine(Shader.ClaroPath, "compile.js");
                if (Shader.OutputPath != "")
                    outPath = Path.Combine(Shader.OutputPath, "compile.js");
                File.Copy(Path.Combine("Resources", "compile.js"), outPath, true);
                ProcessStartInfo psi = new ProcessStartInfo("node", "compile.js \"" + pathToLess + "\"");
                Shader.BuildClaro(psi);
            }
            else
                Shader.BuildClaro();
        }

        static void Shader_Log(string msg)
        {
            Console.WriteLine(msg);
        }
    }
}
