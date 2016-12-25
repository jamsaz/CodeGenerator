using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Jamsaz.CodeGenerator.Tool.Global;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Jamsaz.CodeGenerator.Tool.Engine.Generator
{
    public class RuntimeObjectGenerator
    {
        protected IConfigurationManager ConfigurationManager;

        public RuntimeObjectGenerator(IConfigurationManager configurationManager)
        {
            ConfigurationManager = configurationManager;
        }
        public object Generate(string name)
        {
            var configurationPath = ConfigurationManager.ConfigurationFilePath;
            var directory = configurationPath + Constants.MyMethodsDirectory;
            object response = null;
            if (!Directory.Exists(directory)) return null;
            var codes = Directory.GetFiles(directory)
                .Aggregate("", (current, file) => current + ("\n" + File.ReadAllText(file) + "\n"));
            var syntaxTree = CSharpSyntaxTree.ParseText(codes);
            var assemblyName = Path.GetRandomFileName();
            var references = new MetadataReference[]
            {
                MetadataReference.CreateFromFile(typeof (object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof (Enumerable).Assembly.Location)
            };

            var compilation = CSharpCompilation.Create(
                assemblyName, new[] { syntaxTree }, references,
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
            using (var ms = new MemoryStream())
            {
                var result = compilation.Emit(ms);

                if (!result.Success)
                {
                    var failures = result.Diagnostics.Where(diagnostic =>
                        diagnostic.IsWarningAsError ||
                        diagnostic.Severity == DiagnosticSeverity.Error);

                    foreach (var diagnostic in failures)
                    {
                        Console.Error.WriteLine("{0}: {1}", diagnostic.Id, diagnostic.GetMessage());
                    }
                }
                else
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    var assembly = Assembly.Load(ms.ToArray());
                    var namespaceDotClass = name.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                    var type = assembly.GetType($"{namespaceDotClass[0]}.{namespaceDotClass[1]}");
                    var obj = Activator.CreateInstance(type);
                    response = type.InvokeMember(namespaceDotClass[3], BindingFlags.Default | BindingFlags.InvokeMethod, null,
                        obj, null);
                }
            }
            return response;
        }
    }
}