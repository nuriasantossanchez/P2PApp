using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows.Threading;
using System.Threading;
using Peer2PeerApp.Comunicacion;
using Peer2PeerApp.WPF.Chat;
using Peer2PeerApp.WPF.Fichero;
using Peer2PeerApp.WPF.Multimedia;
using Peer2PeerApp.WPF.Imagen;
using Peer2PeerApp.WPF.Descarga;

namespace Peer2PeerApp.WPF.Red
{
    
    public partial class GestorRedUserControl : UserControl
    {
       
        private BackgroundWorker mCanalP2PThreadBGWorker;
        private ContratoDeServicio mContratoDeServicio;
        PseudoServidorHttpListener mPseudoServidorAudioHttpListener;
        PseudoServidorHttpListener mPseudoServidorVideoHttpListener;
  
        public GestorRedUserControl()
        {
            InitializeComponent();
        }

        public void GestorRedUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (TextBoxNombreRed == null)
            {
                TextBoxNombreRed = txtNombreRed;
            }

            if (TextBoxNombreUsuario == null)
            {
                TextBoxNombreUsuario = txtNombreUsuario;
            }

            if (TextBoxPassword == null)
            {
                TextBoxPassword = txtPassword;
            }

            if (BotonConectar == null)
            {
                BotonConectar = btnConectar;
            }

            if (LabelValorEstado == null)
            {
                LabelValorEstado = lblValorEstado;
            }
            Utilidades.SetTextoLabel(LabelValorEstado, "Desconectado");
            Utilidades.SetEstadoActivadoContentControl(lblTitulo, false);


            if (Audio != null)
            {
                Utilidades.SetTextoLabel(Audio.LabelTitulo, "AUDIO");
            }

            if (Video != null)
            {
                Utilidades.SetTextoLabel(Video.LabelTitulo, "VIDEO");
            }
        }

        #region Propiedades de dependencia
        
        public static DependencyProperty TextBoxNombreUsuarioProperty = DependencyProperty.Register(
            "TextBoxNombreUsuario", typeof(TextBox), typeof(GestorRedUserControl));

        public TextBox TextBoxNombreUsuario
        {
            get
            {
                try
                {
                    return (TextBox)this.Dispatcher.Invoke(
                       System.Windows.Threading.DispatcherPriority.Normal,
                       (DispatcherOperationCallback)delegate { return GetValue(TextBoxNombreUsuarioProperty); },
                       TextBoxNombreUsuarioProperty);
                }
                catch
                {
                    return (TextBox)TextBoxNombreUsuarioProperty.DefaultMetadata.DefaultValue;
                }
            }
            set
            {
                Dispatcher.Invoke(DispatcherPriority.Background,
                  (SendOrPostCallback)delegate { SetValue(TextBoxNombreUsuarioProperty, value); },
                  value);
            }
        }

        public static DependencyProperty TextBoxNombreRedProperty = DependencyProperty.Register(
            "TextBoxNombreRed", typeof(TextBox), typeof(GestorRedUserControl));

        public TextBox TextBoxNombreRed
        {
            get
            {
                try
                {
                    return (TextBox)this.Dispatcher.Invoke(
                       System.Windows.Threading.DispatcherPriority.Normal,
                       (DispatcherOperationCallback)delegate { return GetValue(TextBoxNombreRedProperty); },
                       TextBoxNombreRedProperty);
                }
                catch
                {
                    return (TextBox)TextBoxNombreRedProperty.DefaultMetadata.DefaultValue;
                }
            }
            set
            {
                Dispatcher.Invoke(DispatcherPriority.Background,
                  (SendOrPostCallback)delegate { SetValue(TextBoxNombreRedProperty, value); },
                  value);
            }
        }

        public static DependencyProperty TextBoxPasswordProperty = DependencyProperty.Register(
            "TextBoxPassword", typeof(TextBox), typeof(GestorRedUserControl));

        public TextBox TextBoxPassword
        {
            get
            {
                try
                {
                    return (TextBox)this.Dispatcher.Invoke(
                       System.Windows.Threading.DispatcherPriority.Normal,
                       (DispatcherOperationCallback)delegate { return GetValue(TextBoxPasswordProperty); },
                       TextBoxPasswordProperty);
                }
                catch
                {
                    return (TextBox)TextBoxPasswordProperty.DefaultMetadata.DefaultValue;
                }
            }
            set
            {
                Dispatcher.Invoke(DispatcherPriority.Background,
                  (SendOrPostCallback)delegate { SetValue(TextBoxPasswordProperty, value); },
                  value);
            }
        }

        public static DependencyProperty BotonRegistrarProperty = DependencyProperty.Register(
            "BotonConectar", typeof(Button), typeof(GestorRedUserControl), new PropertyMetadata(null, new PropertyChangedCallback(OnBotonConectarChanged)));

        public Button BotonConectar
        {
            get
            {
                try
                {
                    return (Button)this.Dispatcher.Invoke(
                       System.Windows.Threading.DispatcherPriority.Normal,
                       (DispatcherOperationCallback)delegate { return GetValue(BotonRegistrarProperty); },
                       BotonRegistrarProperty);
                }
                catch
                {
                    return (Button)TextBoxNombreRedProperty.DefaultMetadata.DefaultValue;
                }
            }
            set
            {
                Dispatcher.Invoke(DispatcherPriority.Background,
                  (SendOrPostCallback)delegate { SetValue(BotonRegistrarProperty, value); },
                  value);
            }
        }

        private static void OnBotonConectarChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            GestorRedUserControl controlRed = (GestorRedUserControl)d;
            if (e.OldValue != null)
            {
                Button oldButton = (Button)e.NewValue;
                oldButton.Click -= new RoutedEventHandler(controlRed.BotonConectar_Click);
            }
            Button newButton = (Button)e.NewValue;
            newButton.Click += new RoutedEventHandler(controlRed.BotonConectar_Click);
        }

        public static DependencyProperty LabelValorEstadoProperty = DependencyProperty.Register(
           "LabelValorEstado", typeof(Label), typeof(GestorRedUserControl));

        public Label LabelValorEstado
        {
            get
            {
                try
                {
                    return (Label)this.Dispatcher.Invoke(
                       System.Windows.Threading.DispatcherPriority.Normal,
                       (DispatcherOperationCallback)delegate { return GetValue(LabelValorEstadoProperty); },
                       LabelValorEstadoProperty);
                }
                catch
                {

                    return (Label)LabelValorEstadoProperty.DefaultMetadata.DefaultValue;
                }
            }
            set
            {
                Dispatcher.Invoke(DispatcherPriority.Background,
                  (SendOrPostCallback)delegate { SetValue(LabelValorEstadoProperty, value); },
                  value);
            }
        }

        #endregion Propiedades de dependencia

        #region Métodos privados

        private EstadoRed mEstado = EstadoRed.Desconectado;

        private void BotonConectar_Click(object sender, RoutedEventArgs e)
        {
            if (mEstado == EstadoRed.Desconectado)
            {
                AbrirConexion();
            }
            else
            {
                CerrarConexion();
            }
        }

        #endregion Métodos privados

        #region Métodos públicos

        public enum EstadoRed
        {
            Desconectado,
            UsuarioFallido,
            Conectando,
            Conectado,
        }

        #endregion Métodos públicos

        #region Conexión

        protected void AbrirConexion()
        {
            Regex regex = new Regex("[a-zA-Z0-9]");
            if (!string.IsNullOrEmpty(TextBoxNombreUsuario.Text) && !string.IsNullOrEmpty(TextBoxNombreRed.Text) &&
                regex.IsMatch(TextBoxNombreUsuario.Text) && regex.IsMatch(TextBoxNombreRed.Text)
                && !TextBoxNombreRed.Text.Trim().Contains(' ') && !TextBoxNombreUsuario.Text.Trim().Contains(' '))
            {
                if (!(null == mContratoDeServicio && mCanalP2PThreadBGWorker == null))
                {
                    MessageBox.Show("Ocurrió un error crítico al inicializar la red.\n" +
                                    "\nLo sentimos, la aplicación se cerrará.",
                                    "Error crítico.",
                                    MessageBoxButton.OK, MessageBoxImage.Error);
                    Application.Current.Shutdown();
                }

                mEstado = EstadoRed.Conectando;
                Utilidades.SetTextoLabel(this.LabelValorEstado, "Conectando...");

                mCanalP2PThreadBGWorker = new System.ComponentModel.BackgroundWorker();
                mCanalP2PThreadBGWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(mServiceContractThread);
                mCanalP2PThreadBGWorker.WorkerReportsProgress = true;
                mCanalP2PThreadBGWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(mContratoDeServicioThreadCompleted);
                mCanalP2PThreadBGWorker.WorkerSupportsCancellation = true;
                mCanalP2PThreadBGWorker.RunWorkerAsync();

                Utilidades.SetEstadoActivadoContentControl(TextBoxNombreUsuario, false);
                Utilidades.SetEstadoActivadoContentControl(TextBoxNombreRed, false);
                Utilidades.SetEstadoActivadoContentControl(TextBoxPassword, false);
                Utilidades.SetEstadoActivadoContentControl(BotonConectar, false);
            }
            else
            {
                MessageBox.Show("Por favor introduzca un Nombre de Usuario y un Nombre de Red que contengan solamente caracteres alfanuméricos y sin espacios.",
                                 "Login inválido.", MessageBoxButton.OK, MessageBoxImage.Error);
                Utilidades.SetEstadoActivadoContentControl(TextBoxNombreUsuario, true);
                Utilidades.SetEstadoActivadoContentControl(TextBoxNombreRed, true);
                Utilidades.SetEstadoActivadoContentControl(TextBoxPassword, true);
            }
         }
      
        private void mServiceContractThread(object sender, DoWorkEventArgs doWorkArgs)
        {
            mContratoDeServicio = new ContratoDeServicio();
            if (false == mContratoDeServicio.IniciarCanalP2P(Utilidades.GetTextoTextbox(TextBoxNombreUsuario), Utilidades.GetTextoTextbox(TextBoxNombreRed), Utilidades.GetTextoTextbox(TextBoxPassword)))
            {
                CerrarConexion();
            }

            mContratoDeServicio.EstadoRedChanged += new EventHandler<EstadoRedChangedEventArgs>(MiEstadoRedChanged);
            mContratoDeServicio.AyudanteChat.ChatChanged += new EventHandler<ChatChangedEventArgs>(Chat_ChatChanged);
            mContratoDeServicio.AyudanteImagen.ImagenChanged += new EventHandler<ImagenChangedEventArgs>(Imagen_ImagenChanged);
            mContratoDeServicio.AyudanteFichero.FicheroChanged += new EventHandler<FicheroChangedEventArgs>(Fichero_FicheroChanged);
            mContratoDeServicio.AyudanteDescarga.DescargaChanged += new EventHandler<DescargaChangedEventArgs>(Descarga_DescargaChanged);

            if (Audio != null)
            {
                mContratoDeServicio.AyudanteAudio.StreamChanged += new EventHandler<StreamChangedEventArgs>(mContratoDeServicio_StreamAudioListener);

                mPseudoServidorAudioHttpListener = new PseudoServidorHttpListener(Utilidades.GetTextoTextbox(TextBoxNombreUsuario) + Utilidades.GetTextoTextbox(TextBoxNombreRed) 
                            + "/audio/",
                            AudioBGWorkerThreadFinished, MensajeDeEstadoAudio,
                            mContratoDeServicio.AyudanteAudio.IniciarStream, mContratoDeServicio.AyudanteAudio.TrozoStreamEnviado);
            }

            if (Video != null)
            {
                mContratoDeServicio.AyudanteVideo.StreamChanged += new EventHandler<StreamChangedEventArgs>(mContratoDeServicio_StreamVideoListener);

                mPseudoServidorVideoHttpListener = new PseudoServidorHttpListener(Utilidades.GetTextoTextbox(TextBoxNombreUsuario) + Utilidades.GetTextoTextbox(TextBoxNombreRed) 
                            + "/video/",
                            VideoBGWorkerThreadFinished, MensajeDeEstadoVideo,
                            mContratoDeServicio.AyudanteVideo.IniciarStream, mContratoDeServicio.AyudanteVideo.TrozoStreamEnviado);
            }

            while ((mPseudoServidorAudioHttpListener != null && mPseudoServidorVideoHttpListener != null) &&
                    (!(mPseudoServidorAudioHttpListener.EstaPseudoServidorPreparado || mPseudoServidorVideoHttpListener.EstaPseudoServidorPreparado)))
            {
                System.Threading.Thread.Sleep(0);
            }

            if (mPseudoServidorAudioHttpListener != null && mPseudoServidorVideoHttpListener != null)
            {
                while (mEstado == EstadoRed.Conectando)
                {
                    System.Threading.Thread.Sleep(100);
                    progressBarEstadoRed.Dispatcher.Invoke(DispatcherPriority.Normal,
                        (ThreadStart)delegate
                        {
                            progressBarEstadoRed.Value = ((progressBarEstadoRed.Value >= 100) ? 0 : progressBarEstadoRed.Value + 1);
                        });

                    if (mCanalP2PThreadBGWorker.CancellationPending)
                    {
                        progressBarEstadoRed.Dispatcher.Invoke(DispatcherPriority.Normal,
                        (ThreadStart)delegate
                        {
                            progressBarEstadoRed.Value = 0;
                        });
                        mCanalP2PThreadBGWorker = null;
                        break;
                    }
                }

                progressBarEstadoRed.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate { progressBarEstadoRed.Value = 100; });

                while (true)
                {
                    System.Threading.Thread.Sleep(1000);
                    if (mCanalP2PThreadBGWorker.CancellationPending)
                    {
                        progressBarEstadoRed.Dispatcher.Invoke(DispatcherPriority.Normal,
                        (ThreadStart)delegate
                        {
                            progressBarEstadoRed.Value = 0;
                        });
                        mCanalP2PThreadBGWorker = null;
                        break;
                    }
                }   
            }
            else
            {
                mEstado = EstadoRed.UsuarioFallido;
                Utilidades.SetTextoLabel(lblValorEstado, "Desconectando...");
                Utilidades.SetTextoTextBox(TextBoxNombreUsuario, "USUARIO FALLIDO - Por favor, espere unos segundos...");

                CerrarConexion();

                mContratoDeServicio = null;
                mCanalP2PThreadBGWorker = null;

                MessageBox.Show("Lo sentimos en este momento su Nombre de Usuario ya existe en la red.\n" +
                               "\nPor favor, vuelva a intentarlo con un Nombre de Usuario distinto.",
                               "Nombre de Usuario actualmente en uso.",
                               MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        protected void mContratoDeServicioThreadCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(String.Format("Ocurrió un error en el hilo de ServiceContract: {0}", e.Error.Message));
            }
            else
            {
                if (mEstado == EstadoRed.Conectado)
                {
                    mCanalP2PThreadBGWorker = null;
                    mContratoDeServicio = null;
                    if (mPseudoServidorAudioHttpListener == null && mPseudoServidorVideoHttpListener == null)
                    {
                        Utilidades.SetTextoTextBox(TextBoxNombreUsuario,"");
                        Utilidades.SetTextoTextBox(TextBoxNombreRed, "");
                        Utilidades.SetTextoTextBox(TextBoxPassword, "");
                    }
                }
                else if (mEstado == EstadoRed.UsuarioFallido)
                {
                    Utilidades.SetTextoTextBox(TextBoxNombreUsuario, "");
                }
                mEstado = EstadoRed.Desconectado;

                Utilidades.SetTextoLabel(LabelValorEstado, "Desconectado");
                Utilidades.SetEstadoActivadoContentControl(lblTitulo, false);
                Utilidades.SetEstadoActivadoContentControl(TextBoxNombreUsuario, true);
                Utilidades.SetEstadoActivadoContentControl(TextBoxNombreRed, true);
                Utilidades.SetEstadoActivadoContentControl(TextBoxPassword, true);
                Utilidades.SetEstadoActivadoContentControl(BotonConectar, true);
            }
        }

        protected void CerrarConexion()
        {
            if (null != mContratoDeServicio)
            {
                mContratoDeServicio.CerrarProceso();
                mContratoDeServicio = null;
            }
            
            if (null != mPseudoServidorVideoHttpListener)
                mPseudoServidorVideoHttpListener.Stop();

            if (null != mPseudoServidorAudioHttpListener)
                mPseudoServidorAudioHttpListener.Stop();

            if (null != mPseudoServidorVideoHttpListener)
            {
                mPseudoServidorVideoHttpListener.Salir();
            }

            if (null != mPseudoServidorAudioHttpListener)
            {
                mPseudoServidorAudioHttpListener.Salir();
            }

            if (null != mCanalP2PThreadBGWorker)
            {
                mCanalP2PThreadBGWorker.CancelAsync();
            }

            if (mEstado == EstadoRed.UsuarioFallido)
            {
                Utilidades.SetTextoTextBox(TextBoxNombreUsuario, "");
            }
            else
            {
                Utilidades.SetTextoTextBox(TextBoxNombreUsuario, "");
                Utilidades.SetTextoTextBox(TextBoxNombreRed, "");
                Utilidades.SetTextoTextBox(TextBoxPassword, "");
            }

            Utilidades.SetEstadoActivadoContentControl(TextBoxNombreUsuario, true);
            Utilidades.SetEstadoActivadoContentControl(TextBoxNombreRed, true);
            Utilidades.SetEstadoActivadoContentControl(TextBoxPassword, true);              
        }

        protected void MiEstadoRedChanged(object sender, Peer2PeerApp.Comunicacion.EstadoRedChangedEventArgs e)
        {
            if (Chat != null)
            {
                Chat.SetEstadoRedCambiado(Utilidades.GetTextoTextbox(TextBoxNombreUsuario), e);
            }

            if (Imagen != null)
            {
                Imagen.SetEstadoRedCambiado(Utilidades.GetTextoTextbox(TextBoxNombreUsuario), e);
            }

            if (Fichero != null)
            {
                Fichero.SetEstadoRedCambiado(Utilidades.GetTextoTextbox(TextBoxNombreUsuario), e);
            }

            if (Video != null)
            {
                Video.SetEstadoRedCambiado(Utilidades.GetTextoTextbox(TextBoxNombreUsuario), e);
            }

            if (Audio != null)
            {
                Audio.SetEstadoRedCambiado(Utilidades.GetTextoTextbox(TextBoxNombreUsuario), e);
            }

            if (Descarga != null)
            {
                Descarga.SetEstadoRedCambiado(Utilidades.GetTextoTextbox(TextBoxNombreUsuario), e);
            }

            if (e.Miembro == Utilidades.GetTextoTextbox(TextBoxNombreUsuario))
            {
                if (e.EsNuevoNodoUnidoARed)
                {
                    Utilidades.SetTextoLabel(LabelValorEstado, "¡Conectado!");
                    Utilidades.SetEstadoActivadoContentControl(lblTitulo, true);
                    Utilidades.SetEstadoActivadoContentControl(BotonConectar, true);
                    mEstado = EstadoRed.Conectado;
                }

                if (e.EsNodoQueAbandonaRed)
                {
                    Utilidades.SetTextoLabel(LabelValorEstado, "Desconectado");
                    Utilidades.SetEstadoActivadoContentControl(lblTitulo, false);
                    Utilidades.SetEstadoActivadoContentControl(BotonConectar, true);
                    mEstado = EstadoRed.Desconectado;
                }
            }
            else
            {
                if (e.EsNuevoNodoUnidoARed)
                {
                    Chat_ChatChanged(this, new ChatChangedEventArgs(string.Format("{0} se unió a la red P2P.\n", e.Miembro)));
                }

                if (e.EsNodoQueAbandonaRed)
                {
                    Chat_ChatChanged(this, new ChatChangedEventArgs(string.Format("{0} abandonó la red P2P.\n", e.Miembro)));
                }
            }   
        }

        #endregion Conexión

        #region Imagen

        public static DependencyProperty ImagenProperty = DependencyProperty.Register(
          "Imagen", typeof(ImagenUserControl), typeof(GestorRedUserControl), new PropertyMetadata(null, new PropertyChangedCallback(OnImagenChanged)));

      
        public ImagenUserControl Imagen
        {
            get
            {
                try
                {
                    return (ImagenUserControl)this.Dispatcher.Invoke(
                       System.Windows.Threading.DispatcherPriority.Normal,
                       (DispatcherOperationCallback)delegate { return GetValue(ImagenProperty); },
                       ImagenProperty);
                }
                catch
                {
                    return (ImagenUserControl)ImagenProperty.DefaultMetadata.DefaultValue;
                }
            }
            set
            {
                Dispatcher.Invoke(DispatcherPriority.Background,
                  (SendOrPostCallback)delegate { SetValue(ImagenProperty, value); },
                  value);
            }
        }

        #region Métodos privados

        private static void OnImagenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            GestorRedUserControl controlRed = (GestorRedUserControl)d;
            if (e.OldValue != null)
            {
                ImagenUserControl controlImagenOld = (ImagenUserControl)e.NewValue;
                controlImagenOld.EnviandoImagen -= new EventHandler<ImagenChangedEventArgs>(controlRed.Imagen_EnviarImagen);
            }
           
            ImagenUserControl controlImagenNew = (ImagenUserControl)e.NewValue;
            controlImagenNew.EnviandoImagen += new EventHandler<ImagenChangedEventArgs>(controlRed.Imagen_EnviarImagen);
        }

        private void Imagen_EnviarImagen(object sender, ImagenChangedEventArgs e)
        {
            e.Packet.nombreNodoEmisor = Utilidades.GetTextoTextbox(TextBoxNombreUsuario);
            mContratoDeServicio.AyudanteImagen.EnviarImagen(e.Packet);
        }

        #endregion Métodos privados

        #region Métodos públicos

        public void Imagen_ImagenChanged(object sender, ImagenChangedEventArgs e)
        {
            if (Imagen != null)
            {
                Imagen.SetNuevaImagen(e);
            }
        }

        #endregion Métodos públicos

        #endregion Imagen

        #region Chat

        public static DependencyProperty ChatProperty = DependencyProperty.Register(
         "Chat", typeof(ChatUserControl), typeof(GestorRedUserControl), new PropertyMetadata(null, new PropertyChangedCallback(OnChatChanged)));

        public ChatUserControl Chat
        {
            get
            {
                try
                {
                    return (ChatUserControl)this.Dispatcher.Invoke(
                       System.Windows.Threading.DispatcherPriority.Normal,
                       (DispatcherOperationCallback)delegate { return GetValue(ChatProperty); },
                       ChatProperty);
                }
                catch
                {
                    return (ChatUserControl)ChatProperty.DefaultMetadata.DefaultValue;
                }
            }
            set
            {
                Dispatcher.Invoke(DispatcherPriority.Background,
                  (SendOrPostCallback)delegate { SetValue(ChatProperty, value); },
                  value);
            }
        }

        #region Métodos privados

        private static void OnChatChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            GestorRedUserControl controlRed = (GestorRedUserControl)d;
            if (e.OldValue != null)
            {
                ChatUserControl controlChatOld = (ChatUserControl)e.NewValue;
                controlChatOld.EnviandoTexto -= new EventHandler<ChatChangedEventArgs>(controlRed.Chat_EnviarMensajeTexto);
            }
            ChatUserControl controlChatNew = (ChatUserControl)e.NewValue;
            controlChatNew.EnviandoTexto += new EventHandler<ChatChangedEventArgs>(controlRed.Chat_EnviarMensajeTexto);
        }

        private void Chat_EnviarMensajeTexto(object sender, ChatChangedEventArgs e)
        {
            if (null != mContratoDeServicio)
            {
                mContratoDeServicio.AyudanteChat.EnviarMensajeTexto(TextBoxNombreUsuario.Text, e.Mensaje);
            }
        }

        #endregion Métodos privados

        #region Métodos públicos

        public void Chat_ChatChanged(object sender, ChatChangedEventArgs e)
        {
            if (Chat != null)
            {
                Chat.SetMensajeRecibido(e.Mensaje);
            }
        }

        #endregion Métodos públicos

        #endregion Chat

        #region Descarga

        public static DependencyProperty DescargaProperty = DependencyProperty.Register(
                    "Descarga", typeof(DescargaUserControl), typeof(GestorRedUserControl), new PropertyMetadata(
                    null, new PropertyChangedCallback(OnDescargaChanged)));

        public DescargaUserControl Descarga
        {
            get
            {
                try
                {
                    return (DescargaUserControl)this.Dispatcher.Invoke(
                       System.Windows.Threading.DispatcherPriority.Normal,
                       (DispatcherOperationCallback)delegate { return GetValue(DescargaProperty); },
                       DescargaProperty);
                }
                catch
                {
                    return (DescargaUserControl)DescargaProperty.DefaultMetadata.DefaultValue;
                }
            }
            set
            {
                Dispatcher.Invoke(DispatcherPriority.Background,
                  (SendOrPostCallback)delegate { SetValue(DescargaProperty, value); },
                  value);
            }
        }

        #region Métodos privados

        private static void OnDescargaChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            GestorRedUserControl controlRed = (GestorRedUserControl)d;
            if (e.OldValue != null)
            {
                DescargaUserControl controlDescargaOld = (DescargaUserControl)e.NewValue;
                controlDescargaOld.EnviandoBusqueda -= new EventHandler<DescargaChangedEventArgs>(controlRed.Descarga_BuscarRecurso);
                controlDescargaOld.EnviandoDescarga -= new EventHandler<DescargaChangedEventArgs>(controlRed.Descarga_DescargarRecurso);
            }
            DescargaUserControl controlDescargaNew = (DescargaUserControl)e.NewValue;
            controlDescargaNew.EnviandoBusqueda += new EventHandler<DescargaChangedEventArgs>(controlRed.Descarga_BuscarRecurso);
            controlDescargaNew.EnviandoDescarga += new EventHandler<DescargaChangedEventArgs>(controlRed.Descarga_DescargarRecurso);
        }

        private void Descarga_BuscarRecurso(object sender, DescargaChangedEventArgs e)
        {
            if (null != mContratoDeServicio)
            {
                e.Packet.nombreNodoEmisor = Utilidades.GetTextoTextbox(TextBoxNombreUsuario);
                mContratoDeServicio.AyudanteDescarga.BuscarRecurso(e.Packet);
            }
        }

        private void Descarga_DescargarRecurso(object sender, DescargaChangedEventArgs e)
        {
            if (null != mContratoDeServicio)
            {
                e.Packet.nombreNodoEmisor = Utilidades.GetTextoTextbox(TextBoxNombreUsuario);
                mContratoDeServicio.AyudanteDescarga.DescargarRecurso(e.Packet);
            }
        }

        #endregion Métodos privados

        #region Métodos públicos

        public void Descarga_DescargaChanged(object sender, DescargaChangedEventArgs e)
        {
            if (Descarga != null)
            {
                if (e.Packet.comando == "_BUSQUEDA_")
                {
                    e.Packet.nombreNodoDestino = e.Packet.nombreNodoEmisor;
                    Descarga.SetMensajeDeBusquedaRecibido(e);
                }
                else if (e.Packet.comando == "_RESPUESTA_BUSQUEDA_")
                {
                    Descarga.SetRespuestaABusqueda(e);
                }
                else if (e.Packet.comando == "_PETICION_DESCARGA_")
                {
                    e.Packet.nombreNodoDestino = e.Packet.nombreNodoEmisor;
                    Descarga.SetPeticionDeDescargaRecibida(e);
                }
                else if (e.Packet.comando == "_DESCARGA_")
                {
                    Descarga.SetDescarga(e);
                }
            }
        }

        #endregion Métodos públicos

        #endregion Descarga

        #region Fichero

        public static DependencyProperty FicheroProperty = DependencyProperty.Register(
       "Fichero", typeof(FicheroUserControl), typeof(GestorRedUserControl), new PropertyMetadata(null, new PropertyChangedCallback(OnFicheroChanged)));

        public FicheroUserControl Fichero
        {
            get
            {
                try
                {
                    return (FicheroUserControl)this.Dispatcher.Invoke(
                       System.Windows.Threading.DispatcherPriority.Normal,
                       (DispatcherOperationCallback)delegate { return GetValue(FicheroProperty); },
                       ChatProperty);
                }
                catch
                {
                    return (FicheroUserControl)FicheroProperty.DefaultMetadata.DefaultValue;
                }
            }
            set
            {
                Dispatcher.Invoke(DispatcherPriority.Background,
                  (SendOrPostCallback)delegate { SetValue(FicheroProperty, value); },
                  value);
            }
        }

        #region Métodos privados

        private static void OnFicheroChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            GestorRedUserControl controlRed = (GestorRedUserControl)d;
            if (e.OldValue != null)
            {
               
                FicheroUserControl controlFicheroOld = (FicheroUserControl)e.NewValue;
                controlFicheroOld.EnviandoFichero -= new EventHandler<FicheroChangedEventArgs>(controlRed.Fichero_EnviarFichero);
            }
            
            FicheroUserControl controlFicheroNew = (FicheroUserControl)e.NewValue;
            controlFicheroNew.EnviandoFichero += new EventHandler<FicheroChangedEventArgs>(controlRed.Fichero_EnviarFichero);
        }

        private void Fichero_EnviarFichero(object sender, FicheroChangedEventArgs e)
        {
            if (null != mContratoDeServicio)
            {
                e.Packet.nombreNodoEmisor = Utilidades.GetTextoTextbox(TextBoxNombreUsuario);
                mContratoDeServicio.AyudanteFichero.EnviarFichero(e.Packet);
            }
        }

        #endregion Métodos privados

        #region Métodos públicos

        public void Fichero_FicheroChanged(object sender, FicheroChangedEventArgs e)
        {
            if (Fichero != null)
            {
                Fichero.SetFicheroRecibido(e);
            }
        }

        #endregion Métodos públicos

        #endregion Fichero

        #region Audio

        public static DependencyProperty AudioProperty = DependencyProperty.Register(
            "Audio", typeof(MultimediaUserControl), typeof(GestorRedUserControl), new PropertyMetadata(null, new PropertyChangedCallback(OnAudioChanged)));

        public MultimediaUserControl Audio
        {
            get
            {
                try
                {
                    return (MultimediaUserControl)this.Dispatcher.Invoke(
                       System.Windows.Threading.DispatcherPriority.Normal,
                       (DispatcherOperationCallback)delegate { return GetValue(AudioProperty); },
                       AudioProperty);
                }
                catch
                {
                    return (MultimediaUserControl)AudioProperty.DefaultMetadata.DefaultValue;
                }
            }
            set
            {
                Dispatcher.Invoke(DispatcherPriority.Background,
                  (SendOrPostCallback)delegate { SetValue(AudioProperty, value); },
                  value);
            }
        }

        #region Métodos privados

        private static void OnAudioChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            GestorRedUserControl controlRed = (GestorRedUserControl)d;
            if (e.OldValue != null)
            {
               
                MultimediaUserControl controlAudioOld = (MultimediaUserControl)e.NewValue;
                controlAudioOld.EnviandoStream -= new EventHandler<EnviarStreamEventArgs>(controlRed.Audio_EnviarMultimedia);
                controlAudioOld.CancelarStream -= new EventHandler(controlRed.Audio_CancelarStream);
            }
            
            MultimediaUserControl controlAudioNew = (MultimediaUserControl)e.NewValue;
            controlAudioNew.EnviandoStream += new EventHandler<EnviarStreamEventArgs>(controlRed.Audio_EnviarMultimedia);
            controlAudioNew.CancelarStream += new EventHandler(controlRed.Audio_CancelarStream);
            controlAudioNew.EsMultimediaVideo(false); 
        }

        private void Audio_EnviarMultimedia(object sender, EnviarStreamEventArgs e)
        {
            if (mPseudoServidorAudioHttpListener != null)
            {
                mPseudoServidorAudioHttpListener.EnviarDatoStream(e.NombreArchivo);
            }
        }

        private void Audio_CancelarStream(object sender, EventArgs e)
        {
            if (null != mContratoDeServicio)
            {
                mContratoDeServicio.AyudanteAudio.CancelarEnvioDeStream();
            }
        }

        #endregion Métodos privados

        #region Métodos públicos

        public void MensajeDeEstadoAudio(string mensaje)
        {
            if (Audio != null)
            {
                Audio.SetMensajeDeEstado(mensaje);
            }
        }

        delegate void _mContratoDeServicio_StreamAudioListenerHandler(object sender, StreamChangedEventArgs e);
        public void mContratoDeServicio_StreamAudioListener(object sender, StreamChangedEventArgs e)
        {
            MultimediaUserControl audio = Audio;
            if (audio != null)
            {
                if (audio.CheckAccess())
                {

                    bool play = false;
                    play = e.StreamPacket.nombreNodoEmisor == TextBoxNombreUsuario.Text;

                    MessageBoxResult mbr = MessageBoxResult.Cancel;
                    if (false == play)
                    {
                        mbr = MessageBox.Show(e.StreamPacket.nombreNodoEmisor + " le invita a escuchar la canción '" + e.StreamPacket.titulo + 
                                                                                "'. ¿Acepta la invitación?",
                                                                                "Invitación de audio.", MessageBoxButton.OKCancel);
                    }

                    if (play || mbr == MessageBoxResult.OK)
                    {
                        Utilidades.SetTextoLabel(audio.LabelMetadatosLineaTitulo, "Título:");
                        Utilidades.SetTextoLabel(audio.LabelMetadatosLineaArtista, "Artista:");
                        Utilidades.SetTextoLabel(audio.LabelMetadatosLineaAlbum, "Álbum:");

                        Utilidades.SetTextoTextBlock(audio.TextBlockValorMetadatosLineaTitulo, e.StreamPacket.titulo);
                        Utilidades.SetTextoTextBlock(audio.TextBlockValorMetadatosLineaArtista, e.StreamPacket.artista);
                        Utilidades.SetTextoTextBlock(audio.TextBlockValorMetadatosLineaAlbum, e.StreamPacket.album);

                        audio.StartPlayElementoMultimedia("http://localhost:8088/" + TextBoxNombreUsuario.Text + TextBoxNombreRed.Text + "/audio/" + 
                                        PseudoServidorHttpListener.StringHttpGuardado(e.StreamPacket.nombreArchivo));
                    }
                }
                else
                {
                    if (mPseudoServidorAudioHttpListener != null)
                    {
                        mPseudoServidorAudioHttpListener.StreamChanged(e);
                        if (e.StreamPacket.numPacket == 0)
                        {
                            audio.Dispatcher.Invoke(DispatcherPriority.Normal, new _mContratoDeServicio_StreamAudioListenerHandler(mContratoDeServicio_StreamAudioListener), sender, e);
                        }
                    }
                }     
            }
        }

        protected void AudioBGWorkerThreadFinished(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(String.Format("Ocurrió un error en el hilo que procesa el audio: {0}", e.Error.Message));
            }
            else
            {
                mPseudoServidorAudioHttpListener = null;

                if (mCanalP2PThreadBGWorker == null && mPseudoServidorVideoHttpListener == null)
                {
                    Utilidades.SetTextoLabel(LabelValorEstado, "Desconectado");
                    mEstado = EstadoRed.Desconectado;
                    Utilidades.SetEstadoActivadoContentControl(TextBoxNombreUsuario, true);
                    Utilidades.SetEstadoActivadoContentControl(TextBoxNombreRed, true);
                    Utilidades.SetEstadoActivadoContentControl(TextBoxPassword, true);
                    Utilidades.SetEstadoActivadoContentControl(BotonConectar, true);
                }
            }
        }

        #endregion Métodos públicos

        #endregion Audio

        #region Video

        public static DependencyProperty VideoProperty = DependencyProperty.Register(
            "Video", typeof(MultimediaUserControl), typeof(GestorRedUserControl), new PropertyMetadata(null, new PropertyChangedCallback(OnVideoChanged)));

        public MultimediaUserControl Video
        {
            get
            {
                try
                {
                    return (MultimediaUserControl)this.Dispatcher.Invoke(
                       System.Windows.Threading.DispatcherPriority.Normal,
                       (DispatcherOperationCallback)delegate { return GetValue(VideoProperty); },
                       VideoProperty);
                }
                catch
                {
                    return (MultimediaUserControl)VideoProperty.DefaultMetadata.DefaultValue;
                }
            }
            set
            {
                Dispatcher.Invoke(DispatcherPriority.Background,
                  (SendOrPostCallback)delegate { SetValue(VideoProperty, value); },
                  value);
            }
        }

        #region Métodos privados

        private static void OnVideoChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            GestorRedUserControl controlRed = (GestorRedUserControl)d;
            if (e.OldValue != null)
            {
                MultimediaUserControl controlVideoOld = (MultimediaUserControl)e.NewValue;
                controlVideoOld.EnviandoStream -= new EventHandler<EnviarStreamEventArgs>(controlRed.Video_EnviarMultimedia);
                controlVideoOld.CancelarStream -= new EventHandler(controlRed.Video_CancelarStream);
            }

            MultimediaUserControl controlVideoNew = (MultimediaUserControl)e.NewValue;
            controlVideoNew.EnviandoStream += new EventHandler<EnviarStreamEventArgs>(controlRed.Video_EnviarMultimedia);
            controlVideoNew.CancelarStream += new EventHandler(controlRed.Video_CancelarStream);
            controlVideoNew.EsMultimediaVideo(true);
        }
       
        private void Video_EnviarMultimedia(object sender, EnviarStreamEventArgs e)
        {
            if (mPseudoServidorVideoHttpListener != null)
            {
                mPseudoServidorVideoHttpListener.EnviarDatoStream(e.NombreArchivo);
            }
        }

        private void Video_CancelarStream(object sender, EventArgs e)
        {
            if (null != mContratoDeServicio)
            {
                mContratoDeServicio.AyudanteVideo.CancelarEnvioDeStream();
            }
        }

        #endregion Métodos privados

        #region Métodos públicos

        public void MensajeDeEstadoVideo(string mensaje)
        {
            if (Video != null)
            {
                Video.SetMensajeDeEstado(mensaje);
            }
        }

        delegate void _mContratoDeServicio_StreamVideoListenerHandler(object sender, StreamChangedEventArgs e);
        public void mContratoDeServicio_StreamVideoListener(object sender, StreamChangedEventArgs e)
        {
            MultimediaUserControl video = Video; 
            if (video != null)
            {
                if (video.CheckAccess())
                {
                    bool play = false;
                    play = e.StreamPacket.nombreNodoEmisor == TextBoxNombreUsuario.Text;


                    MessageBoxResult mbr = MessageBoxResult.Cancel;
                    if (false == play)
                    {
                        mbr = MessageBox.Show(e.StreamPacket.nombreNodoEmisor + " le invita a visualizar el vídeo '" + e.StreamPacket.nombreArchivo +
                                                                                "'. ¿Acepta la invitación?",
                                                                                "Invitación de vídeo.", MessageBoxButton.OKCancel);
                    }
    
                    if (play || mbr == MessageBoxResult.OK)
                    {
                        Utilidades.SetTextoLabel(video.LabelMetadatosLineaTitulo, "Título:");
                        Utilidades.SetTextoLabel(video.LabelMetadatosLineaArtista, "Artista:");
                        Utilidades.SetTextoLabel(video.LabelMetadatosLineaAlbum, "");

                        Utilidades.SetTextoTextBlock(video.TextBlockValorMetadatosLineaTitulo, e.StreamPacket.nombreArchivo);
                        Utilidades.SetTextoTextBlock(video.TextBlockValorMetadatosLineaArtista, e.StreamPacket.artista);

                        video.StartPlayElementoMultimedia("http://localhost:8088/" + TextBoxNombreUsuario.Text + TextBoxNombreRed.Text + "/video/" + 
                                                                   PseudoServidorHttpListener.StringHttpGuardado(e.StreamPacket.nombreArchivo));
                    }
                }
                else
                {
                    if (mPseudoServidorVideoHttpListener != null)
                    {
                        mPseudoServidorVideoHttpListener.StreamChanged(e);
                        if (e.StreamPacket.numPacket == 0)
                        {
                            video.Dispatcher.Invoke(DispatcherPriority.Normal, new _mContratoDeServicio_StreamVideoListenerHandler(mContratoDeServicio_StreamVideoListener), sender, e);
                        }
                    }
                }
            }
        }

        protected void VideoBGWorkerThreadFinished(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(String.Format("Ocurrió un error en el hilo que procesa el video:{0}", e.Error.Message));
            }
            else
            {
                mPseudoServidorVideoHttpListener = null;
                if (mCanalP2PThreadBGWorker == null && mPseudoServidorAudioHttpListener == null)
                {
                    Utilidades.SetTextoLabel(LabelValorEstado, "Desconectado");
                    mEstado = EstadoRed.Desconectado;
                    Utilidades.SetEstadoActivadoContentControl(TextBoxNombreUsuario, true);
                    Utilidades.SetEstadoActivadoContentControl(TextBoxNombreRed, true);
                    Utilidades.SetEstadoActivadoContentControl(TextBoxPassword, true);
                    Utilidades.SetEstadoActivadoContentControl(BotonConectar, true);
                }
            }
        }

        #endregion Métodos públicos

        #endregion Video
    }
}
