using System.IO;
using System.IO.Compression;
using System.Text.Json;
using System.Diagnostics;
using Sharprompt;

class Config
{
    public string? UltraPath { get; set; }
    public string? ModDirectory { get; set; }
    public static string[] Dependencies { get; set; } = new[] { "0Harmony.dll", "UnityEngine.dll", "UnityEngine.CoreModule.dll", "Assembly-CSharp.dll" };
    public string? HarmonyPath { get; set; }
}


internal class Builder
{
    static string MOD_NAME = "ExampleMod"; // Change this to the name of your mod
    public static string GetHarmonyPath()
    {
        var ShouldDownloadHarmony = Prompt.Confirm("Should the program automatically download 0Harmony for you?", defaultValue: true);
        if (ShouldDownloadHarmony)
        {
            Console.WriteLine("Downloading 0Harmony.dll");
            var client = new HttpClient();
            var responseTask = client.GetAsync("https://github.com/pardeike/Harmony/releases/download/v2.3.3.0/Harmony-Fat.2.3.3.0.zip");
            responseTask.Wait();
            Directory.CreateDirectory("_Harmony");
            var contentTask = responseTask.Result.Content.ReadAsByteArrayAsync();
            contentTask.Wait();
            File.WriteAllBytes("_Harmony.zip", contentTask.Result);
            ZipFile.OpenRead("_Harmony.zip").ExtractToDirectory("_Harmony", true);
            Console.WriteLine("Successfully downloaded 0Harmony!");
            return Path.Combine(Directory.GetCurrentDirectory(), "_Harmony");
        }
        var HarmonyPath = Prompt.Input<string>("Enter your 0harmony build directory");
        if (HarmonyPath == null || !Directory.Exists(HarmonyPath) || !File.Exists(Path.Combine(HarmonyPath, "net48", "0Harmony.dll")) || !File.Exists(Path.Combine(HarmonyPath, ".doorstop-version")))
        {
            Console.WriteLine("0Harmony build path cannot be found or is invalid"); Environment.Exit(1);
        }
        return HarmonyPath;
    }

    public static Config PromptForConfig()
    {
        Console.WriteLine("Config not found. creating it now...");
        var config = new Config();
        var UltraPath = Prompt.Input<string>("Enter your Marble it Up! Ultra installation directory", "C:\\Program Files (x86)\\Steam\\steamapps\\common\\Marble It Up!");
        if (UltraPath == "" && !Directory.Exists(UltraPath)) { Console.WriteLine("Marble it Up! Ultra installation path cannot be null"); Environment.Exit(1); }
        config.UltraPath = UltraPath;
        config.HarmonyPath = GetHarmonyPath();
        if (!Path.Exists(Path.Combine(UltraPath, "Mods", MOD_NAME))) Directory.CreateDirectory(Path.Combine(UltraPath, "Mods", MOD_NAME));
        config.ModDirectory = Path.Combine(UltraPath, "Mods", MOD_NAME);
        return config;
    }

    public static Config ParseConfig()
    {
        if (File.Exists("config.json"))
        {
            Console.WriteLine("Using existing config...");
            return JsonSerializer.Deserialize<Config>(File.ReadAllText("config.json"));
        }
        var config = PromptForConfig();
        File.WriteAllText("config.json", JsonSerializer.Serialize(config));
        return config;
    }

    public static Config config { get; set; }

    public static void CopyDeps()
    {
        var LibsPath = Path.Combine(new[] { Directory.GetCurrentDirectory(), "..", "..", "..", "..", MOD_NAME, "Libraries" });
        Console.WriteLine($"Copying {Config.Dependencies.Length} dependencies...");
        foreach (var dependency in Config.Dependencies)
        {
            Console.WriteLine("Copying " + dependency);
            if (dependency == "0Harmony.dll")
            {
                File.Copy(Path.Combine(config.HarmonyPath, "net48", "0Harmony.dll"), Path.Combine(LibsPath, dependency), true);
                continue;
            }
            File.Copy(Path.Combine(config.UltraPath, "Marble it Up_Data", "Managed", dependency), Path.Combine(LibsPath, dependency), true);
        }
    }

    public static string Build()
    {

        CopyDeps();
        Directory.SetCurrentDirectory($"../../../../{MOD_NAME}");
        Console.WriteLine($"Building {MOD_NAME}...");
        ProcessStartInfo psi = new ProcessStartInfo();
        psi.RedirectStandardError = true;
        psi.RedirectStandardOutput = true;
        psi.FileName = "dotnet.exe";
        psi.Arguments = "build";
        var proc = Process.Start(psi);
        proc.WaitForExit();
        if (proc.ExitCode != 0)
        {
            Console.WriteLine($"Build failed with exit code {proc.ExitCode}");
            Console.WriteLine(proc.StandardOutput.ReadToEnd());
            Console.WriteLine(proc.StandardError.ReadToEnd());
            Environment.Exit(1);
        }
        var ModPath = Path.Combine(Directory.GetCurrentDirectory(), "bin", "Debug", $"{MOD_NAME}.dll");
        Console.WriteLine($"{MOD_NAME} is available at {ModPath}");
        return ModPath;
    }



    private static void Main(string[] args)
    {
        config = ParseConfig();
        var LoaderPath = Build();
        Console.WriteLine($"Installing {MOD_NAME} to Marble it Up! Ultra...");
        File.Copy(LoaderPath, Path.Combine(config.ModDirectory, $"{MOD_NAME}.dll"), true);
        Console.WriteLine($"Successfully installed {MOD_NAME}.dll to Marble it Up! Ultra");
    }

}