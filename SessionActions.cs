using VolumeControl.Core.Attributes;
using VolumeControl.Core.Input;
using VolumeControl.SDK;

namespace VolumeControl.TestAddon
{
    [HotkeyActionGroup("Session Extras", GroupColor = "#DAD")]
    public class SessionActions
    {
        private static VCAPI VCAPI => VCAPI.Default;

        [HotkeyAction]
        public void UnhideAllSessions(object? sender, HotkeyPressedEventArgs e)
        {
            var hiddenSessions = VCAPI.AudioSessionManager.HiddenSessions;

            // enumerate backwards through the list using a for loop to avoid exceptions when the list is changed
            for (int i = hiddenSessions.Count - 1; i >= 0; --i)
            {
                VCAPI.AudioSessionManager.UnhideSession(hiddenSessions[i]);
            }
        }
        [HotkeyAction(Description = "Permanently unhides all audio sessions.")]
        public void PermanentlyUnhideAllSessions(object? sender, HotkeyPressedEventArgs e)
        {
            VCAPI.Settings.HiddenSessionProcessNames.Clear();
        }
    }
}