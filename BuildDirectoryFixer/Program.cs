using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BuildDirectoryFixer
{
    internal static class Program
    {
        private const string SolutionDirectory = @"C:\Users\b0-0b\Work\Digitator";
        private const string SourceDirectory = @"src";
        private const string ProjectDirectoryPlaceholder = @"PROJECT_DIRECTORY_PLACEHOLDER";
        private const string BuildDirectoryRelativePath = @"..\..\build\";

        private static readonly Dictionary<string, string> ReplacementDictionary = new Dictionary<string, string>
        {
            [@"bin\Debug\"]       = $"{BuildDirectoryRelativePath}{ProjectDirectoryPlaceholder}\\Debug\\AnyCPU\\",
            [@"bin\Release\"]     = $"{BuildDirectoryRelativePath}{ProjectDirectoryPlaceholder}\\Release\\AnyCPU\\",
            [@"bin\x86\Debug\"]   = $"{BuildDirectoryRelativePath}{ProjectDirectoryPlaceholder}\\Debug\\x86\\",
            [@"bin\x86\Release\"] = $"{BuildDirectoryRelativePath}{ProjectDirectoryPlaceholder}\\Release\\x86\\",
            [@"bin\x64\Debug\"]   = $"{BuildDirectoryRelativePath}{ProjectDirectoryPlaceholder}\\Debug\\x64\\",
            [@"bin\x64\Release\"] = $"{BuildDirectoryRelativePath}{ProjectDirectoryPlaceholder}\\Release\\x64\\",
            [@"bin\ARM\Debug\"] =   $"{BuildDirectoryRelativePath}{ProjectDirectoryPlaceholder}\\Debug\\ARM\\",
            [@"bin\ARM\Release\"] = $"{BuildDirectoryRelativePath}{ProjectDirectoryPlaceholder}\\Release\\ARM\\"
        };

        private static void Main(string[] args)
        {
            var projectDirectories = GetProjectDirectories().ToList();
            Console.WriteLine($"Processing project directoies:\n\t{string.Join("\n\t", projectDirectories)}");

            foreach (var projectDirectory in projectDirectories)
            {
                ProcessProjectDirectory(projectDirectory);
            }
        }

        private static IEnumerable<string> GetProjectDirectories()
        {
            var sourcePath = Path.Combine(SolutionDirectory, SourceDirectory);
            return Directory.GetDirectories(sourcePath);
        }

        private static string GetProjectDirectoryName(string projectDirectory)
            => projectDirectory.Split(new []{Path.DirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries).Last();

        private static string GetProjectFilename(string projectDirectory)
            => GetProjectDirectoryName(projectDirectory) + ".csproj";

        private static Dictionary<string, string> BuildCurrectProjectReplacementDictionary(string projectName)
            => ReplacementDictionary
                .ToDictionary(keyValuePair => keyValuePair.Key,
                              keyValuePair => keyValuePair.Value.Replace(ProjectDirectoryPlaceholder, projectName));

        private static void ProcessProjectDirectory(string projectDirectory)
        {
            var projectFile = Path.Combine(projectDirectory, GetProjectFilename(projectDirectory));

            if (!File.Exists(projectFile))
            {
                WriteColoured($"WARN: file {projectFile} does not exist, skipping", ConsoleColor.Red);
                return;
            }

            var projectName = GetProjectDirectoryName(projectDirectory);
            var replacementDictionary = BuildCurrectProjectReplacementDictionary(projectName);

            var projectFileContents = File.ReadAllText(projectFile);

            projectFileContents = replacementDictionary
                .Aggregate(projectFileContents, (current, keyValuePair) => current.Replace(keyValuePair.Key, keyValuePair.Value));

            File.WriteAllText(projectFile, projectFileContents);
            WriteColoured($"Directory {projectDirectory} processed", ConsoleColor.Green);
        }

        private static void WriteColoured(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
