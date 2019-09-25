using System.Windows.Controls;
using System.Windows.Media;

namespace Peer2PeerApp.WPF
{
    public class Utilidades
    {
        delegate string GetTextoTxtboxHandler(TextBox t);
        public static string GetTextoTextbox(TextBox t)
        {
            if (t == null)
            {
                return null;
            }
            if (t.CheckAccess())
            {
                return t.Text;
            }
            else
            {
                return (string)t.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new GetTextoTxtboxHandler(GetTextoTextbox), t);
            }
        }

        delegate void SetTextoLabelHandler(Label l, string texto);
        internal static void SetTextoLabel(Label l, string s)
        {
            if (l != null)
            {
                if (l.CheckAccess())
                {
                    l.Content = s;
                }
                else
                {
                    l.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new SetTextoLabelHandler(SetTextoLabel), l, s);
                }
            }
        }

        delegate void SetTextoTextBoxHandler(TextBox txt, string texto);
        internal static void SetTextoTextBox(TextBox txt, string s)
        {
            if (txt != null)
            {
                if (txt.CheckAccess())
                {
                    txt.Text = s;
                }
                else
                {
                    txt.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new SetTextoTextBoxHandler(SetTextoTextBox), txt, s);
                }
            }
        }

        delegate void SetTextoTextBlockHandler(TextBlock t, string texto);
        internal static void SetTextoTextBlock(TextBlock t, string s)
        {
            if (t != null)
            {
                if (t.CheckAccess())
                {
                    t.Text = s;
                }
                else
                {
                    t.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new SetTextoTextBlockHandler(SetTextoTextBlock), t, s);
                }
            }
        }

        delegate void SetEstadoContentControlHandler(Control c, bool isEnabled);
        internal static void SetEstadoActivadoContentControl(Control c, bool isEnabled)
        {
            if (c != null)
            {
                if (c.CheckAccess())
                {  
                    c.IsEnabled = isEnabled;
                }
                else
                {
                    c.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new SetEstadoContentControlHandler(SetEstadoActivadoContentControl), c, isEnabled);
                }
            }
        }
    }
}
