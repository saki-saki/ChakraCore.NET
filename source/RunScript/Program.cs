﻿using System;
using System.IO;
using System.Text;
using ChakraCore.NET;
using ChakraCore.NET.Plugin;
namespace RunScript
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                showUsage();
            }
            else
            {
                ScriptConfig config = null;
                try
                {
                    config = ScriptConfig.Parse(args);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("invalid parameter");
                    Console.WriteLine(ex.Message);
                    showUsage();
                    Console.WriteLine("Press any key to exit");
                    Console.Read();
                    return;
                }
                var context = prepareContext();
                if (!string.IsNullOrEmpty(config.RootFolder))
                {
                    FileSystemLoader loader = new FileSystemLoader(Path.Combine(@"D:\MyGitHub\My Repro\ChakraCore.NET\source\RunScript\bin\Debug\netcoreapp2.0", config.PluginRootFolder));
                    PluginManager manager = new PluginManager(context);
                    manager.Loaders.Add(loader);
                    manager.Init(context);
                    //PluginInstaller.InstallPlugins(config.PluginRootFolder, context);
                    //PluginInstaller installer = new PluginInstaller(Path.Combine(@"D:\MyGitHub\My Repro\ChakraCore.NET\source\RunScript\bin\Debug\netcoreapp2.0", config.PluginRootFolder),context);
                    //installer.RequireNative("ImageSharpProvider");
                    
                }
                string script = System.IO.File.ReadAllText(config.File);
                Console.WriteLine("---Script Start---");
                if (config.IsModule)
                {
                    var jsClass = context.ProjectModuleClass(config.FileName, config.ModuleClass, config.RootFolder);
                    jsClass.CallMethod(config.ModuleEntryPoint);
                }
                else
                {
                    context.RunScript(script);
                }
            }

            Console.WriteLine("Press any key to exit");
            Console.Read();
        }
        

        static ChakraContext prepareContext()
        {
            ChakraRuntime runtime = ChakraRuntime.Create();
            ChakraContext result = runtime.CreateContext(false);
            return result;
        }

        static void showUsage()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendJoin(
                Environment.NewLine
                , "RunScript useage:"
                , "/file:FileName                    run a javascript file"
                , "/module                           run a javascript as module"
                , "/class:ClassName                  the entrypoint class name of module, default is \"app\""
                , "/entrypoint:FunctionName          the entrypoint function name of module, default is \"main\""
                );
            Console.WriteLine(sb);
        }
    }
}
