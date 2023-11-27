using System.Diagnostics;

namespace GroceryList.WebApi.Helpers;

public static class CreateTestDataHelper
{
    internal static void CreateTestData()
    {
        try
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string relativePath = Path.Combine("..", "GroceryList.Console", "bin", "Debug", "net7.0", "GroceryList.Console.dll");
            string consoleAppPath = Path.GetFullPath(Path.Combine(currentDirectory, relativePath));

            //string consoleAppPath = @"C:\SRC\GroceryList\GroceryList.Console\bin\Debug\net7.0\GroceryList.Console.dll";

            ProcessStartInfo psi = new()
            {
                FileName = "dotnet",
                Arguments = $"exec {consoleAppPath}",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using Process process = new() { StartInfo = psi };
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            Console.WriteLine(output);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error executing console application: {ex.Message}");
        }
    }
}
