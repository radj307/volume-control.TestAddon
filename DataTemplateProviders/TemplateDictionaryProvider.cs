using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using VolumeControl.Core;
using VolumeControl.Core.Extensions;

namespace VolumeControl.TestAddon.DataTemplateProviders
{
    /// <summary>
    /// This is an example of a ITemplateDictionaryProvider implementation.
    /// </summary>
    public partial class TemplateDictionaryProvider : ResourceDictionary, ITemplateDictionaryProvider
    {
        #region Constructor
        public TemplateDictionaryProvider()
        {
            InitializeComponent(); //< this line is VERY important, and sometimes shows up as missing in Visual Studio. The project still compiles successfully.
        }
        #endregion Constructor

        #region ITemplateDictionaryProvider
        public ActionSettingDataTemplate? ProvideDataTemplate(string key)
        {
            if (base[key] is ActionSettingDataTemplate actionSettingDataTemplate)
            {
                return actionSettingDataTemplate;
            }
            else return null;
        }
        public ActionSettingDataTemplate? ProvideDataTemplate(Type valueType)
        {
            // cast this to an enumerate list of DictionaryEntry instances.
            //  This works because this class inherits from ResourceDictionary, which implements IEnumerable.
            foreach (var (key, value) in this.Cast<DictionaryEntry>())
            {
                // check if this entry is an ActionSettingDataTemplate instance that supports the requested value type.
                if (value is ActionSettingDataTemplate actionSettingDataTemplate
                    && actionSettingDataTemplate.SupportsValueType(valueType))
                {
                    return actionSettingDataTemplate;
                }
            }
            return null;
        }
        #endregion ITemplateDictionaryProvider

        public void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (!e.KeyboardDevice.IsModifierKeyDown(Core.Input.Enums.EModifierKey.Ctrl))
                return;

            switch (e.Key)
            {
            case Key.Enter:
                { // update the binding source when the user presses Ctrl+Enter
                    var textBox = (TextBox)sender;
                    var bindingExpression = textBox.GetBindingExpression(TextBox.TextProperty);
                    bindingExpression.UpdateSource();
                    break;
                }
            }
        }
    }
}
