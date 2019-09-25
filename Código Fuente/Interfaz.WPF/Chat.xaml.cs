using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Threading;
using Peer2PeerApp.Comunicacion;

namespace Peer2PeerApp.WPF.Chat
{
  
    public partial class ChatUserControl : UserControl
    {
       
        public ChatUserControl()
        {
            InitializeComponent();
        }

        private void ChatUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (TextBoxNuevoMensajeChat == null)
            {
                TextBoxNuevoMensajeChat = txtNuevoMensajeChat;
            }

            if (BotonEnviarTextoChat == null)
            {
                BotonEnviarTextoChat = btnEnviarTextoChat;
            }

            if (TextBoxConversacionChat == null)
            {
                TextBoxConversacionChat = txtConversacionChat;
            }

            BotonEnviarTextoChat.IsEnabled = false;
            TextBoxNuevoMensajeChat.IsEnabled = false;
            lblTitulo.IsEnabled = false;
        }

        #region Propiedades de dependencia
        
        public static  DependencyProperty TextBoxNuevoMensajeChatProperty = DependencyProperty.Register(
           "TextBoxNuevoMensajeChat", typeof(TextBox), typeof(ChatUserControl), new PropertyMetadata(null, 
                                                    new PropertyChangedCallback(OnTextBoxNuevoMensajeChatChanged)));
             
       
        public TextBox TextBoxNuevoMensajeChat
        {
            get
            {
                try
                {
                    return (TextBox)this.Dispatcher.Invoke(
                       System.Windows.Threading.DispatcherPriority.Normal,
                       (DispatcherOperationCallback)delegate { return GetValue(TextBoxNuevoMensajeChatProperty); },
                       TextBoxNuevoMensajeChatProperty);
                }
                catch
                {

                    return (TextBox)TextBoxNuevoMensajeChatProperty.DefaultMetadata.DefaultValue;
                }
            }
            set
            {
                Dispatcher.Invoke(DispatcherPriority.Normal,
                  (SendOrPostCallback)delegate { SetValue(TextBoxNuevoMensajeChatProperty, value); },
                  value);
            }
        }

        private static void OnTextBoxNuevoMensajeChatChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ChatUserControl controlChat = (ChatUserControl)d;
            if (e.OldValue != null)
            {
               
                TextBox oldText = (TextBox)e.NewValue;
                oldText.TextChanged -= new TextChangedEventHandler(controlChat.TextBoxNuevoMensaje_TextChanged);
            }
            
            TextBox newText = (TextBox)e.NewValue;
            newText.TextChanged += new TextChangedEventHandler(controlChat.TextBoxNuevoMensaje_TextChanged);

        }

        public static  DependencyProperty TextBoxConversacionChatProperty = DependencyProperty.Register(
           "TextBoxConversacionChat", typeof(TextBox), typeof(ChatUserControl));
        
      
        public TextBox TextBoxConversacionChat
        {
            get
            {
                try
                {
                    return (TextBox)this.Dispatcher.Invoke(
                       System.Windows.Threading.DispatcherPriority.Normal,
                       (DispatcherOperationCallback)delegate { return GetValue(TextBoxConversacionChatProperty); },
                       TextBoxConversacionChatProperty);
                }
                catch
                {

                    return (TextBox)TextBoxConversacionChatProperty.DefaultMetadata.DefaultValue;
                }
            }
            set
            {
                Dispatcher.Invoke(DispatcherPriority.Normal,
                  (SendOrPostCallback)delegate { SetValue(TextBoxConversacionChatProperty, value); },
                  value);
            }
        }


        public static  DependencyProperty BotonEnviarTextoChatProperty = DependencyProperty.Register(
          "BotonEnviarTextoChat", typeof(Button), typeof(ChatUserControl), new PropertyMetadata(null, 
                                        new PropertyChangedCallback(OnBotonEnviarTextoChatChanged)));

        public Button BotonEnviarTextoChat
        {
            get
            {
                try
                {
                    return (Button)this.Dispatcher.Invoke(
                       System.Windows.Threading.DispatcherPriority.Normal,
                       (DispatcherOperationCallback)delegate { return GetValue(BotonEnviarTextoChatProperty); },
                       BotonEnviarTextoChatProperty);
                }
                catch
                {

                    return (Button)BotonEnviarTextoChatProperty.DefaultMetadata.DefaultValue;
                }
            }
            set
            {                
                Dispatcher.Invoke(DispatcherPriority.Normal,
                  (SendOrPostCallback)delegate { SetValue(BotonEnviarTextoChatProperty, value); },
                  value);
            }
        }
     
        private static void OnBotonEnviarTextoChatChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ChatUserControl controlChat = (ChatUserControl)d;
            if (e.OldValue != null)
            {
               
                Button oldButton = (Button)e.NewValue;
                oldButton.Click -= new RoutedEventHandler(controlChat.BotonEnviarTexto_Click);
            }
            
            Button newButton = (Button)e.NewValue;
            newButton.Click += new RoutedEventHandler(controlChat.BotonEnviarTexto_Click);
        }

        #endregion Propiedades de dependencia

        #region Métodos privados

        private void TextBoxNuevoMensaje_TextChanged(object sender, RoutedEventArgs e)
        {
            TextBox txtbox = (TextBox)sender;
            if (txtbox.Text.Contains("\r\n"))
            {
                BotonEnviarTexto_Click(sender, e);
            }
            TextBoxNuevoMensajeChat.Focus();
        }

        internal event EventHandler<ChatChangedEventArgs> EnviandoTexto;
        private void BotonEnviarTexto_Click(object sender, RoutedEventArgs e)
        {
            if (EnviandoTexto != null)
            {
                EnviandoTexto(sender, new ChatChangedEventArgs(TextBoxNuevoMensajeChat.Text));
            }
            TextBoxNuevoMensajeChat.Clear();
        }

        private delegate void _HabilitarControlesHandler(bool valor);
        private void HabilitarControles(bool valor)
        {
            if (TextBoxNuevoMensajeChat.CheckAccess())
            {
                BotonEnviarTextoChat.IsEnabled = valor;
                TextBoxNuevoMensajeChat.IsEnabled = valor;
                lblTitulo.IsEnabled = valor;
            }
            else
            {
                TextBoxNuevoMensajeChat.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new _HabilitarControlesHandler(HabilitarControles), valor);
            }
        }
     
        private delegate void _ActualizarTextBoxHandler(String mensaje);
        internal void ActualizarTextBoxConversacionChat(string mensaje)
        {
            if (TextBoxConversacionChat.CheckAccess())
            {
                mensaje = mensaje.Replace("\r\n","").Replace("\n","").Replace("\r","");
                TextBoxConversacionChat.Text += mensaje + "\n\n";
                TextBoxConversacionChat.ScrollToEnd();
            }
            else
            { 
                TextBoxConversacionChat.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, 
                                                    new _ActualizarTextBoxHandler(ActualizarTextBoxConversacionChat), mensaje);
            }
        }

        internal void SetEstadoRedCambiado(string nombreUsusario, EstadoRedChangedEventArgs e)
        {            
            if (nombreUsusario == e.Miembro && e.EsNuevoNodoUnidoARed)
            {
             
                Utilidades.SetTextoLabel(lblValorUsuarioChat, nombreUsusario);             
                HabilitarControles(true);
            }

            if (nombreUsusario == e.Miembro && e.EsNodoQueAbandonaRed)
            {
                Utilidades.SetTextoLabel(lblValorUsuarioChat, "");
                HabilitarControles(false);
            }
        }

        internal void SetMensajeRecibido(string s)
        {
            ActualizarTextBoxConversacionChat(s);
        }

        #endregion Métodos privados
    }
}
