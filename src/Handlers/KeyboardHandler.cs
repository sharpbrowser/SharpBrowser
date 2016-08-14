using System.Windows.Forms;
using CefSharp;


namespace SharpBrowser
{
    public class KeyboardHandler : IKeyboardHandler
    {
        MainForm myForm;

        public KeyboardHandler(MainForm form)
        {
            myForm = form;
        }
        public bool OnPreKeyEvent(IWebBrowser browserControl, IBrowser browser, KeyType type, int windowsKeyCode, int nativeKeyCode, CefEventFlags modifiers, bool isSystemKey, ref bool isKeyboardShortcut)
        {
            return false;
        }

        /// <inheritdoc/>>
        public bool OnKeyEvent(IWebBrowser browserControl, IBrowser browser, KeyType type, int windowsKeyCode, int nativeKeyCode, CefEventFlags modifiers, bool isSystemKey)
        {
            if (type == KeyType.KeyUp && windowsKeyCode == (int)Keys.F4 && (modifiers == CefEventFlags.ControlDown))
            {
                //Debug.WriteLine("Ctrl-F4");
                myForm.CloseActiveTab();
            }

            //Debug.WriteLine(String.Format("OnKeyEvent: KeyType: {0} 0x{1:X} Modifiers: {2}", type, windowsKeyCode, modifiers));
            return false;
        }
    }
}
