using System.ComponentModel;
using VolumeControl.API;
using VolumeControl.Hotkeys.Attributes;
using VolumeControl.Log;

namespace VolumeControl.TestAddon
{
    [ActionAddon(nameof(TestActionAddon))]
    public class TestActionAddon
    {
        private static VCAPI API => VCAPI.Default;
        private static LogWriter Log => VCAPI.Log;


        [HotkeyAction]
        public void CustomAction(object? sender, HandledEventArgs e)
        {
            Log.Debug($"Successfully triggered addon method {nameof(CustomAction)}!", $"Currently selected session name: '{API.AudioAPI.SelectedSession?.ProcessName}'");
        }
    }
}