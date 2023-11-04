using System.Diagnostics;
using VolumeControl.Core.Attributes;
using VolumeControl.Core.Input;

namespace VolumeControl.TestAddon
{
    [HotkeyActionGroup("File", GroupColor = "#F0F")]
    public class FileActions
    {
        [HotkeyAction(Description = "Opens a file with the default application.")]
        [HotkeyActionSetting("Path", typeof(string), Description = "The location of the file to open.")]
        public void OpenFile(object? sender, HotkeyPressedEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.GetValue<string>("Path"))
            {
                UseShellExecute = true
            })?.Dispose();
        }
    }
}