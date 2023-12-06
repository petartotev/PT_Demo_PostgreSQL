using System.Diagnostics;

namespace DemoPostgreSql;

public class CommandManager
{
    public static void Execute(string command, int delayInSeconds = 5)
    {
        Process process = new();
        process.StartInfo.FileName = "cmd.exe"; // Use "cmd.exe" on Windows
        process.StartInfo.Arguments = "/c " + command; // /c carries out the command specified by string and then terminates
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.CreateNoWindow = true;

        process.Start();

        // Read the output (if needed)
        string output = process.StandardOutput.ReadToEnd();
        Console.WriteLine("Output: " + output);

        process.WaitForExit();
        process.Close();

        Thread.Sleep(delayInSeconds * 1000);
    }
}
