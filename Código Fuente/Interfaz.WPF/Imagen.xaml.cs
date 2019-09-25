using System;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Peer2PeerApp.Comunicacion;

namespace Peer2PeerApp.WPF.Imagen
{
   
    public partial class ImagenUserControl : UserControl
    {
       
        public ImagenUserControl()
        {
            InitializeComponent();
        }
       
        private void ImagenUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (BotonEnviarImagen == null)
            {
                BotonEnviarImagen = btnEnviarImagen;
            }

            if (PictureBox == null)
            {
                PictureBox = imagen;
            }

            if (TextBlockNombreImagen == null)
            {
                TextBlockNombreImagen = txtblckNombreImagen;
            }

            BotonEnviarImagen.IsEnabled = false;
            lblTitulo.IsEnabled = false;
        }

        #region Propiedades de dependencia
       
        public static DependencyProperty BotonEnviarImagenProperty = DependencyProperty.Register(
           "BotonEnviarImagen", typeof(Button), typeof(ImagenUserControl), new PropertyMetadata(null, 
                                        new PropertyChangedCallback(OnBotonEnviarImagenChanged)));

        public Button BotonEnviarImagen
        {
            get
            {
                try
                {
                    return (Button)this.Dispatcher.Invoke(
                       System.Windows.Threading.DispatcherPriority.Send,
                       (DispatcherOperationCallback)delegate { return GetValue(BotonEnviarImagenProperty); },
                       BotonEnviarImagenProperty);
                }
                catch
                {

                    return (Button)BotonEnviarImagenProperty.DefaultMetadata.DefaultValue;
                }
            }
            set
            {
                Dispatcher.Invoke(DispatcherPriority.Normal,
                  (SendOrPostCallback)delegate { SetValue(BotonEnviarImagenProperty, value); },
                  value);
            }
        }

        private static void OnBotonEnviarImagenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ImagenUserControl controlImagen = (ImagenUserControl)d;
            if (e.OldValue != null)
            {
                
                Button oldButton = (Button)e.NewValue;
                oldButton.Click -= new RoutedEventHandler(controlImagen.BotonEnviarImagen_Click);
            }
            
            Button newButton = (Button)e.NewValue;
            newButton.Click += new RoutedEventHandler(controlImagen.BotonEnviarImagen_Click);
        }

        public static DependencyProperty PictureBoxProperty = DependencyProperty.Register(
          "PictureBox", typeof(System.Windows.Controls.Image), typeof(ImagenUserControl));
      
        public System.Windows.Controls.Image PictureBox
        {
            get
            {
                try
                {
                    return (System.Windows.Controls.Image)this.Dispatcher.Invoke(
                       System.Windows.Threading.DispatcherPriority.Normal,
                       (DispatcherOperationCallback)delegate { return GetValue(PictureBoxProperty); },
                       PictureBoxProperty);
                }
                catch
                {

                    return (System.Windows.Controls.Image)PictureBoxProperty.DefaultMetadata.DefaultValue;
                }
            }
            set
            {
                Dispatcher.Invoke(DispatcherPriority.Normal,
                  (SendOrPostCallback)delegate { SetValue(PictureBoxProperty, value); },
                  value);
            }
        }

        public static DependencyProperty TextBlockNombreImagenProperty = DependencyProperty.Register(
         "TextBlockNombreImagen", typeof(TextBlock), typeof(ImagenUserControl));
      
        public TextBlock TextBlockNombreImagen
        {
            get
            {
                try
                {
                    return (TextBlock)this.Dispatcher.Invoke(
                       System.Windows.Threading.DispatcherPriority.Normal,
                       (DispatcherOperationCallback)delegate { return GetValue(TextBlockNombreImagenProperty); },
                       TextBlockNombreImagenProperty);
                }
                catch
                {
                    return (TextBlock)TextBlockNombreImagenProperty.DefaultMetadata.DefaultValue;
                }
            }
            set
            {
                Dispatcher.Invoke(DispatcherPriority.Normal,
                  (SendOrPostCallback)delegate { SetValue(TextBlockNombreImagenProperty, value); },
                  value);
            }
        }

        #endregion Propiedades de dependencia

        #region Métodos públicos

        internal event EventHandler<ImagenChangedEventArgs> EnviandoImagen; 
        public void EnviarImagen(string rutaYNombreArchivo)
        {
            
            if (rutaYNombreArchivo != null && System.IO.File.Exists(rutaYNombreArchivo))
            {
                using (FileStream fs = new FileStream(rutaYNombreArchivo, FileMode.Open))
                {
                    ImagenPacket packet = new ImagenPacket();
                    packet.stream = fs;
                    packet.nombreArchivo = System.IO.Path.GetFileName(rutaYNombreArchivo);
                    MetadatosInfoFichero md = new MetadatosInfoFichero();
                    md.LeerMetadatos(rutaYNombreArchivo);
                    packet.titulo = md.Titulo;
                    
                    if (EnviandoImagen != null)
                    {
                        EnviandoImagen(this, new ImagenChangedEventArgs(packet));
                    }
                }
            }
            else
            {
                throw new ApplicationException(string.Format("Archivo no encontrado: {0}", rutaYNombreArchivo));
            }
        }

        public event EventHandler<ImagenChangedEventArgs> ImagenRecibida;     
        public event EventHandler<EstadoRedChangedEventArgs> EstadoRedChanged;

        #endregion Métodos públicos

        #region Métodos privados
     
        private void BotonEnviarImagen_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog ofd;
            ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.FileName = "NombreDeArchivo";
            ofd.Filter = "(*.JPG, *.GIF, *.PNG)|*.jpg;*.gif;*.png|All Files (*.*)|*.*";
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            ofd.RestoreDirectory = true;
            ofd.Title = "Por favor, seleccione la imagen que desea visualizar.";
            bool ok = (bool)ofd.ShowDialog();
            if (ok)
            {
                EnviarImagen(ofd.FileName);                                   
            }
        }

        private delegate void _SetEstadoRedCambiadoHandler(string nombreUsuario, EstadoRedChangedEventArgs e);
        internal void SetEstadoRedCambiado(string nombreUsuario, EstadoRedChangedEventArgs e)
        {
            if (BotonEnviarImagen.CheckAccess())
            {
                if (nombreUsuario == e.Miembro && e.EsNuevoNodoUnidoARed)
                {
                    BotonEnviarImagen.IsEnabled = true;
                    lblTitulo.IsEnabled = true;
                }

                if (nombreUsuario == e.Miembro && e.EsNodoQueAbandonaRed)
                {
                    BotonEnviarImagen.IsEnabled = false;
                    lblTitulo.IsEnabled = false;
                }

                if (null != EstadoRedChanged)
                {
                    EstadoRedChanged(nombreUsuario, e);
                }
            }
            else
            {
                BotonEnviarImagen.Dispatcher.Invoke(DispatcherPriority.Normal, new _SetEstadoRedCambiadoHandler(SetEstadoRedCambiado), nombreUsuario, e );
            }
        }

        delegate void _SetNuevaImagenHandler(ImagenChangedEventArgs packet);
        internal void SetNuevaImagen(ImagenChangedEventArgs e)
        {
            if (PictureBox.CheckAccess())
            {
                Byte[] bytes = new Byte[Peer2PeerApp.Comunicacion.AyudanteImagen.MAX_IMAGE_SIZE];
                int posicionCursor = 0;
                int valor = 0;
                do
                {
                    if (e.Packet.stream.CanRead)
                    {
                        valor = e.Packet.stream.Read(bytes, posicionCursor, Peer2PeerApp.Comunicacion.AyudanteImagen.MAX_IMAGE_SIZE - posicionCursor);
                        posicionCursor += valor;
                    }
                    else
                    {
                        throw new ApplicationException("Ha sido imposible cargar la imagen. Por favor, inténtelo de nuevo. Si el error persiste reinicie la aplicación.");
                    }
                } while (valor != 0);

                try
                {

                    MemoryStream ms = new MemoryStream();
                    ms.Write(bytes, 0, posicionCursor);
                    ms.Position = 0;

                    System.Windows.Media.Imaging.BitmapImage bim = new System.Windows.Media.Imaging.BitmapImage();
                    bim.BeginInit();
                    bim.StreamSource = ms;
                    bim.EndInit();
                    PictureBox.Source = bim;

                    Utilidades.SetTextoTextBlock(TextBlockNombreImagen, e.Packet.nombreArchivo);

                    if (ImagenRecibida != null)
                    {
                        ImagenRecibida(this, e);
                    }
                }
                catch(NotSupportedException)
                {
                    //archivo no es .jpg,.gif,.png
                }
            }
            else
            {
                PictureBox.Dispatcher.Invoke(DispatcherPriority.Normal, new _SetNuevaImagenHandler(SetNuevaImagen), e);
            }
        }

        #endregion Métodos privados
    } 
}
