using System.Diagnostics;
using System.IO;
using VolumeControl.Core.Attributes;
using VolumeControl.Core.Input;
using VolumeControl.Log;
using VolumeControl.TestAddon.DataTemplateProviders;

namespace VolumeControl.TestAddon
{
    [HotkeyActionGroup("Script")]
    public class ScriptActions
    {
        private readonly string _powershellPath = FindPowershellExecutable();
        private readonly string _userHomePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

        private static string FindPowershellExecutable()
        {
            // first check for powershell 7
            string path = Path.GetFullPath("pwsh.exe");
            if (File.Exists(path))
                return path;

            // check for regular powershell
            path = Path.GetFullPath("powershell.exe");
            if (File.Exists(path))
                return path;

            // use built-in powershell
            path = @"C:\Windows\System32\WindowsPowershell\v1.0\powershell.exe";
            if (File.Exists(path))
                return path;

            // nothing exists!
            return string.Empty;
        }

        [HotkeyAction("Execute PowerShell Script", Description = "Executes a predefined script using PowerShell.")]
        [HotkeyActionSetting(
            name: "Script",
            valueType: typeof(string),
            dataTemplateProviderType: typeof(TemplateDictionaryProvider),
            dataTemplateKey: "MultiLineStringDataTemplate",
            DefaultValue = "# Write your powershell script here:")]
        [HotkeyActionSetting("Working Directory", typeof(string))]
        [HotkeyActionSetting("Redirect STDOUT", typeof(bool), DefaultValue = true)]
        [HotkeyActionSetting("Redirect STDERR", typeof(bool), DefaultValue = true)]
        public void ExecutePowerShellScript(object? sender, HotkeyPressedEventArgs e)
        {
            if (_powershellPath.Length == 0)
            {
                FLog.Error($"[{nameof(ExecutePowerShellScript)}] Cannot execute script because no PowerShell executable was found!");
                return;
            }

            var hk = (Hotkey)sender!;
            if (hk!.NoRepeat == false)
                hk.NoRepeat = true; //< enable NoRepeat modifier to prevent this action from being easily spammed

            string script = e.GetValue<string>("Script");
            if (string.IsNullOrWhiteSpace(script)) return;

            try
            {
                bool redirectSTDOUT = e.GetValue<bool>("Redirect STDOUT");
                bool redirectSTDERR = e.GetValue<bool>("Redirect STDERR");
                using var process = Process.Start(new ProcessStartInfo(_powershellPath)
                {
                    UseShellExecute = false,
                    WorkingDirectory = e.GetValueOrDefault<string>("Working Directory", _userHomePath),
                    RedirectStandardInput = true,
                    RedirectStandardOutput = redirectSTDOUT,
                    RedirectStandardError = redirectSTDERR,
                });

                if (process == null)
                {
                    FLog.Error($"[{nameof(ExecutePowerShellScript)}] Failed to start a powershell process.");
                    return;
                }

                process.StandardInput.Write(script);
                process.StandardInput.Flush();

                if (redirectSTDOUT)
                {
                    Console.Out.Write(process.StandardOutput.ReadToEnd());
                }
                if (redirectSTDERR)
                {
                    Console.Error.WriteLine(process.StandardError.ReadToEnd());
                }

                process.WaitForExit();
            }
            catch (Exception ex)
            {
                FLog.Error($"[{nameof(ExecutePowerShellScript)}] An exception occurred while executing the script:", ex);
            }
        }
    }
}
