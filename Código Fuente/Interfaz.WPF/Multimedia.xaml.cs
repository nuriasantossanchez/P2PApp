using System;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Threading;
using System.Threading;
using Peer2PeerApp.Comunicacion;

namespace Peer2PeerApp.WPF.Multimedia
{
    public partial class MultimediaUserControl : UserControl
    {
        public MultimediaUserControl()
        {
            InitializeComponent();
        }

        public void MultimediaUserControl_Loaded(object sender, EventArgs e)
        {
            if (LabelTitulo == null)
            {
                LabelTitulo = lblTitulo;
            }

            if (BotonEnviarMultimedia == null)
            {
                BotonEnviarMultimedia = btnEnviarMultimedia;
            }

            if (BotonCancelarMultimedia == null)
            {
                BotonCancelarMultimedia = btnPararMultimedia;
            }

            if (LabelMetadatosLineaTitulo == null)
            {
                LabelMetadatosLineaTitulo = lblLineaTitulo;
            }
            if (LabelMetadatosLineaArtista == null)
            {
                LabelMetadatosLineaArtista = lblLineaArtista;
            }

            if (LabelMetadatosLineaAlbum == null)
            {
                LabelMetadatosLineaAlbum = lblLineaAlbum;
            }

            if (TextBlockValorMetadatosLineaTitulo == null)
            {
                TextBlockValorMetadatosLineaTitulo = txtblckValorLineaTitulo;
            }
            if (TextBlockValorMetadatosLineaArtista == null)
            {
                TextBlockValorMetadatosLineaArtista = txtblckValorLineaArtista;
            }
            if (TextBlockValorMetadatosLineaAlbum == null)
            {
                TextBlockValorMetadatosLineaAlbum = txtblckValorLineaAlbum;
            }

            if (ElementoMultimedia == null)
            {
                ElementoMultimedia = mElementoMultimedia;
            }

            BotonEnviarMultimedia.IsEnabled = false;
            BotonCancelarMultimedia.IsEnabled = false;
            lblTitulo.IsEnabled = false;
        }

        #region Propiedades de dependencia

        public static DependencyProperty LabelTituloProperty = DependencyProperty.Register(
           "LabelTitulo", typeof(Label), typeof(MultimediaUserControl));

        public Label LabelTitulo
        {
            get
            {
                try
                {
                    return (Label)this.Dispatcher.Invoke(
                       System.Windows.Threading.DispatcherPriority.Normal,
                       (DispatcherOperationCallback)delegate { return GetValue(LabelTituloProperty); },
                       LabelTituloProperty);
                }
                catch
                {

                    return (Label)LabelTituloProperty.DefaultMetadata.DefaultValue;
                }
            }
            set
            {
                Dispatcher.Invoke(DispatcherPriority.Normal,
                  (SendOrPostCallback)delegate { SetValue(LabelTituloProperty, value); },
                  value);
            }
        }

        public static DependencyProperty BotonEnviarMultimediaProperty = DependencyProperty.Register(
          "BotonEnviarMultimedia", typeof(Button), typeof(MultimediaUserControl), new PropertyMetadata(
              null, new PropertyChangedCallback(OnBotonEnviarMultimediaChanged)));

        public Button BotonEnviarMultimedia
        {
            get
            {
                try
                {
                    return (Button)this.Dispatcher.Invoke(
                       System.Windows.Threading.DispatcherPriority.Normal,
                       (DispatcherOperationCallback)delegate { return GetValue(BotonEnviarMultimediaProperty); },
                       BotonEnviarMultimediaProperty);
                }
                catch
                {

                    return (Button)BotonEnviarMultimediaProperty.DefaultMetadata.DefaultValue;
                }
            }
            set
            {
                Dispatcher.Invoke(DispatcherPriority.Normal,
                  (SendOrPostCallback)delegate { SetValue(BotonEnviarMultimediaProperty, value); },
                  value);
            }
        }
      
        private static void OnBotonEnviarMultimediaChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MultimediaUserControl controlMultimedia = (MultimediaUserControl)d;
            if (e.OldValue != null)
            {
               
                Button oldButton = (Button)e.NewValue;
                oldButton.Click -= new RoutedEventHandler(controlMultimedia.BotonEnviarMultimedia_Click);
            }
           
            Button newButton = (Button)e.NewValue;
            newButton.Click += new RoutedEventHandler(controlMultimedia.BotonEnviarMultimedia_Click);
        }
      
        public static DependencyProperty BotonCancelarMultimediaProperty = DependencyProperty.Register(
          "BotonCancelarMultimedia", typeof(Button), typeof(MultimediaUserControl), new PropertyMetadata(null, 
                                                                                new PropertyChangedCallback(OnBotonCancelarMultimediaChanged)));

        public Button BotonCancelarMultimedia
        {
            get
            {
                try
                {
                    return (Button)this.Dispatcher.Invoke(
                       System.Windows.Threading.DispatcherPriority.Normal,
                       (DispatcherOperationCallback)delegate { return GetValue(BotonCancelarMultimediaProperty); },
                       BotonCancelarMultimediaProperty);
                }
                catch
                {

                    return (Button)BotonCancelarMultimediaProperty.DefaultMetadata.DefaultValue;
                }
            }
            set
            {
                Dispatcher.Invoke(DispatcherPriority.Normal,
                  (SendOrPostCallback)delegate { SetValue(BotonCancelarMultimediaProperty, value); },
                  value);
            }
        }

        private static void OnBotonCancelarMultimediaChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MultimediaUserControl controlMultimedia = (MultimediaUserControl)d;
            if (e.OldValue != null)
            {
               
                Button oldButton = (Button)e.NewValue;
                oldButton.Click -= new RoutedEventHandler(controlMultimedia.BotonCancelarMultimedia_Click);
            }
          
            Button newButton = (Button)e.NewValue;
            newButton.Click += new RoutedEventHandler(controlMultimedia.BotonCancelarMultimedia_Click);
        }
     
        public static DependencyProperty ElementoMultimediaProperty = DependencyProperty.Register(
            "ElementoMultimedia", typeof(MediaElement), typeof(MultimediaUserControl));

        public MediaElement ElementoMultimedia
        {
            get
            {
                try
                {
                    return (MediaElement)this.Dispatcher.Invoke(
                       System.Windows.Threading.DispatcherPriority.Normal,
                       (DispatcherOperationCallback)delegate { return GetValue(ElementoMultimediaProperty); },
                       ElementoMultimediaProperty);
                }
                catch
                {
                    return (MediaElement)ElementoMultimediaProperty.DefaultMetadata.DefaultValue;
                }
            }
            set
            {
                Dispatcher.Invoke(DispatcherPriority.Normal,
                  (SendOrPostCallback)delegate { SetValue(ElementoMultimediaProperty, value); },
                  value);
            }
        }

     
        public static DependencyProperty LabelMetadatosLineaTituloProperty = DependencyProperty.Register(
           "LabelMetadatosLineaTitulo", typeof(Label), typeof(MultimediaUserControl));

        public Label LabelMetadatosLineaTitulo
        {
            get
            {
                try
                {
                    return (Label)this.Dispatcher.Invoke(
                       System.Windows.Threading.DispatcherPriority.Normal,
                       (DispatcherOperationCallback)delegate { return GetValue(LabelMetadatosLineaTituloProperty); },
                       LabelMetadatosLineaTituloProperty);
                }
                catch
                {

                    return (Label)LabelMetadatosLineaTituloProperty.DefaultMetadata.DefaultValue;
                }
            }
            set
            {
                Dispatcher.Invoke(DispatcherPriority.Normal,
                  (SendOrPostCallback)delegate { SetValue(LabelMetadatosLineaTituloProperty, value); },
                  value);
            }
        }

        public static DependencyProperty LabelMetadatosLineaArtistaProperty = DependencyProperty.Register(
           "LabelMetadatosLineaArtista", typeof(Label), typeof(MultimediaUserControl));

        public Label LabelMetadatosLineaArtista
        {
            get
            {
                try
                {
                    return (Label)this.Dispatcher.Invoke(
                       System.Windows.Threading.DispatcherPriority.Normal,
                       (DispatcherOperationCallback)delegate { return GetValue(LabelMetadatosLineaArtistaProperty); },
                       LabelMetadatosLineaArtistaProperty);
                }
                catch
                {

                    return (Label)LabelMetadatosLineaArtistaProperty.DefaultMetadata.DefaultValue;
                }
            }
            set
            {
                Dispatcher.Invoke(DispatcherPriority.Normal,
                  (SendOrPostCallback)delegate { SetValue(LabelMetadatosLineaArtistaProperty, value); },
                  value);
            }
        }
     
        public static DependencyProperty LabelMetadatosLineaAlbumProperty = DependencyProperty.Register(
        "LabelMetadatosLineaAlbum", typeof(Label), typeof(MultimediaUserControl));
       
        public Label LabelMetadatosLineaAlbum
        {
            get
            {
                try
                {
                    return (Label)this.Dispatcher.Invoke(
                       System.Windows.Threading.DispatcherPriority.Normal,
                       (DispatcherOperationCallback)delegate { return GetValue(LabelMetadatosLineaAlbumProperty); },
                       LabelMetadatosLineaAlbumProperty);
                }
                catch
                {

                    return (Label)LabelMetadatosLineaAlbumProperty.DefaultMetadata.DefaultValue;
                }
            }
            set
            {
                Dispatcher.Invoke(DispatcherPriority.Normal,
                  (SendOrPostCallback)delegate { SetValue(LabelMetadatosLineaAlbumProperty, value); },
                  value);
            }
        }

        public static DependencyProperty TextBlockValorMetadatosLineaTituloProperty = DependencyProperty.Register(
          "TextBlockValorMetadatosLineaTitulo", typeof(TextBlock), typeof(MultimediaUserControl));

        public TextBlock TextBlockValorMetadatosLineaTitulo
        {
            get
            {
                try
                {
                    return (TextBlock)this.Dispatcher.Invoke(
                       System.Windows.Threading.DispatcherPriority.Normal,
                       (DispatcherOperationCallback)delegate { return GetValue(TextBlockValorMetadatosLineaTituloProperty); },
                       TextBlockValorMetadatosLineaTituloProperty);
                }
                catch
                {

                    return (TextBlock)TextBlockValorMetadatosLineaTituloProperty.DefaultMetadata.DefaultValue;
                }
            }
            set
            {
                Dispatcher.Invoke(DispatcherPriority.Normal,
                  (SendOrPostCallback)delegate { SetValue(TextBlockValorMetadatosLineaTituloProperty, value); },
                  value);
            }
        }

        public static DependencyProperty TextBlockValorMetadatosLineaArtistaProperty = DependencyProperty.Register(
            "TextBlockValorMetadatosLineaArtista", typeof(TextBlock), typeof(MultimediaUserControl));

        public TextBlock TextBlockValorMetadatosLineaArtista
        {
            get
            {
                try
                {
                    return (TextBlock)this.Dispatcher.Invoke(
                       System.Windows.Threading.DispatcherPriority.Normal,
                       (DispatcherOperationCallback)delegate { return GetValue(TextBlockValorMetadatosLineaArtistaProperty); },
                       TextBlockValorMetadatosLineaArtistaProperty);
                }
                catch
                {

                    return (TextBlock)TextBlockValorMetadatosLineaArtistaProperty.DefaultMetadata.DefaultValue;
                }
            }
            set
            {
                Dispatcher.Invoke(DispatcherPriority.Normal,
                  (SendOrPostCallback)delegate { SetValue(TextBlockValorMetadatosLineaArtistaProperty, value); },
                  value);
            }
        }

        public static DependencyProperty TextBlockValorMetadatosLineaAlbumProperty = DependencyProperty.Register(
            "TextBlockValorMetadatosLineaAlbum", typeof(TextBlock), typeof(MultimediaUserControl));

        public TextBlock TextBlockValorMetadatosLineaAlbum
        {
            get
            {
                try
                {
                    return (TextBlock)this.Dispatcher.Invoke(
                       System.Windows.Threading.DispatcherPriority.Normal,
                       (DispatcherOperationCallback)delegate { return GetValue(TextBlockValorMetadatosLineaAlbumProperty); },
                       TextBlockValorMetadatosLineaAlbumProperty);
                }
                catch
                {

                    return (TextBlock)TextBlockValorMetadatosLineaAlbumProperty.DefaultMetadata.DefaultValue;
                }
            }
            set
            {
                Dispatcher.Invoke(DispatcherPriority.Normal,
                  (SendOrPostCallback)delegate { SetValue(TextBlockValorMetadatosLineaAlbumProperty, value); },
                  value);
            }
        }

        #endregion Propiedades de dependencia

        #region Métodos públicos

        public event EventHandler<EstadoRedChangedEventArgs> EstadoRedChanged;
      
        public event EventHandler StartPlayChanged;
       

        public void StartPlayElementoMultimedia(string play)
        {
            ElementoMultimedia.Source = new Uri(play);
            ElementoMultimedia.Play();
            if (StartPlayChanged != null)
            {
                StartPlayChanged(this, new EventArgs());
            }
        }

        #endregion Métodos públicos

        #region Métodos privados

        private bool mEsMultimediaVideo;
        internal void EsMultimediaVideo(bool value)
        {
            mEsMultimediaVideo = value;
            if (mEsMultimediaVideo)
            {
                txtBlockTitulo.Text = "VIDEO";
            }
            else
            {
                txtBlockTitulo.Text = "AUDIO";
            }
        }

        internal event EventHandler<EnviarStreamEventArgs> EnviandoStream;
        private void BotonEnviarMultimedia_Click(object sender, EventArgs e)
        {            
            if (EnviandoStream != null)
            {                
                Microsoft.Win32.OpenFileDialog ofd;
                ofd = new Microsoft.Win32.OpenFileDialog();
                ofd.FileName = "NombreDeAchivo";
                if (mEsMultimediaVideo)
                {
                    ofd.Filter = "(*.wmv)|*.wmv|All Files (*.*)|*.*";
                    ofd.Title = "Por favor, seleccione el vídeo que desea reproducir.";
                    ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);
                }
                else
                {
                    ofd.Filter = "(*.mp3)|*.mp3|(*.wma)|*.wma|All Files (*.*)|*.*";
                    ofd.Title = "Por favor, seleccione la canción que desea escuchar.";
                    ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
                }
                ofd.RestoreDirectory = true;
               
                bool ok = (bool)ofd.ShowDialog();
                if (ok)
                {
                    EnviandoStream(this, new EnviarStreamEventArgs(ofd.FileName));
                }
            }
        }

        internal event EventHandler CancelarStream;
        private void BotonCancelarMultimedia_Click(object sender, EventArgs e)
        {
            ElementoMultimedia.Stop();
            if (CancelarStream != null)
            {
                CancelarStream(this, new EventArgs());
            }
        }

        private delegate void _SetMensajeDeEstadoHandler(string mensaje);
        internal void SetMensajeDeEstado(string mensaje)
        {
            if (txtInfoEstadoMultimedia.CheckAccess())
            {
                this.txtInfoEstadoMultimedia.Text += mensaje + "\n";
                txtInfoEstadoMultimedia.ScrollToEnd();
            }
            else
            {
                txtInfoEstadoMultimedia.Dispatcher.Invoke(DispatcherPriority.Normal, new _SetMensajeDeEstadoHandler(SetMensajeDeEstado), mensaje);
            }            
        }

        private delegate void _SetEstadoRedCambiadoHandler(string nombreUsuario, EstadoRedChangedEventArgs e);
        internal void SetEstadoRedCambiado(string nombreUsuario, EstadoRedChangedEventArgs e)
        {
            if (BotonEnviarMultimedia.CheckAccess())
            {
                if (nombreUsuario == e.Miembro && e.EsNuevoNodoUnidoARed)
                {
                    BotonEnviarMultimedia.IsEnabled = true;
                    BotonCancelarMultimedia.IsEnabled = true;
                    lblTitulo.IsEnabled = true;
                }

                if (nombreUsuario == e.Miembro && e.EsNodoQueAbandonaRed)
                {
                    BotonEnviarMultimedia.IsEnabled = false;
                    BotonCancelarMultimedia.IsEnabled = false;
                    lblTitulo.IsEnabled = false;
                }
                if (null != EstadoRedChanged)
                {
                    EstadoRedChanged(nombreUsuario, e);
                }
            }
            else
            {
                BotonEnviarMultimedia.Dispatcher.Invoke(DispatcherPriority.Normal, new _SetEstadoRedCambiadoHandler(SetEstadoRedCambiado), nombreUsuario, e);
            }
        }

        #endregion Métodos privados
    }
}
