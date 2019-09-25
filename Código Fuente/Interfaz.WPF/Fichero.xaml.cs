using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Peer2PeerApp.Comunicacion;

namespace Peer2PeerApp.WPF.Fichero
{
    public partial class FicheroUserControl : UserControl
    {
        public FicheroUserControl()
        {
            InitializeComponent();
        }

        private void FicheroUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (BotonEnviarFichero == null)
            {
                BotonEnviarFichero = btnEnviarFichero;
            }

            if (TextBoxInfoEstadoFichero == null)
            {
                TextBoxInfoEstadoFichero = txtInfoEstadoFichero;
            }

            BotonEnviarFichero.IsEnabled = false;
            lblTitulo.IsEnabled = false;
        }

        #region Propiedades de dependencia
      
        public static DependencyProperty BotonEnviarFicheroProperty = DependencyProperty.Register(
           "BotonEnviarFichero", typeof(Button), typeof(FicheroUserControl), new PropertyMetadata(
               null, new PropertyChangedCallback(OnBotonEnviarFicheroChanged)));

        public Button BotonEnviarFichero
        {
            get
            {
                try
                {
                    return (Button)this.Dispatcher.Invoke(
                       System.Windows.Threading.DispatcherPriority.Normal,
                       (DispatcherOperationCallback)delegate { return GetValue(BotonEnviarFicheroProperty); },
                       BotonEnviarFicheroProperty);
                }
                catch
                {

                    return (Button)BotonEnviarFicheroProperty.DefaultMetadata.DefaultValue;
                }
            }
            set
            {
                Dispatcher.Invoke(DispatcherPriority.Normal,
                  (SendOrPostCallback)delegate { SetValue(BotonEnviarFicheroProperty, value); },
                  value);
            }
        }

        private static void OnBotonEnviarFicheroChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FicheroUserControl controlFichero = (FicheroUserControl)d;
            if (e.OldValue != null)
            {
               
                Button oldButton = (Button)e.NewValue;
                oldButton.Click -= new RoutedEventHandler(controlFichero.BotonEnviarFichero_Click);
            }
           
            Button newButton = (Button)e.NewValue;
            newButton.Click += new RoutedEventHandler(controlFichero.BotonEnviarFichero_Click);
        }

        public static DependencyProperty TextBoxInfoEstadoFicheroProperty = DependencyProperty.Register(
           "TextBoxInfoEstadoFichero", typeof(TextBox), typeof(FicheroUserControl));

        public TextBox TextBoxInfoEstadoFichero
        {
            get
            {
                try
                {
                    return (TextBox)this.Dispatcher.Invoke(
                       System.Windows.Threading.DispatcherPriority.Normal,
                       (DispatcherOperationCallback)delegate { return GetValue(TextBoxInfoEstadoFicheroProperty); },
                       TextBoxInfoEstadoFicheroProperty);
                }
                catch
                {

                    return (TextBox)TextBoxInfoEstadoFicheroProperty.DefaultMetadata.DefaultValue;
                }
            }
            set
            {
                Dispatcher.Invoke(DispatcherPriority.Normal,
                  (SendOrPostCallback)delegate { SetValue(TextBoxInfoEstadoFicheroProperty, value); },
                  value);
            }
        }

        #endregion Propiedades de dependencia

        #region Métodos públicos

        public void EnviarFichero(string rutaYNombreFichero)
        {  
            if (rutaYNombreFichero != null && System.IO.File.Exists(rutaYNombreFichero))
            {
                BackgroundWorker worker;
                worker = new System.ComponentModel.BackgroundWorker();
                worker.DoWork += new System.ComponentModel.DoWorkEventHandler(EnviarFicheroBGWorker);
                worker.RunWorkerAsync(rutaYNombreFichero);
            }
            else
            {
                throw new ApplicationException(string.Format("Fichero no encontrado: {0}", rutaYNombreFichero));
            }
        }

        public event EventHandler<FicheroChangedEventArgs> FicheroRecibido;
        public event EventHandler<EstadoRedChangedEventArgs> EstadoRedChanged;

        #endregion Métodos públicos

        #region Métodos privados

        private void BotonEnviarFichero_Click(object sender, EventArgs e)
        {
            Microsoft.Win32.OpenFileDialog ofd;
            ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.FileName = "NombreDeArchivo";
            ofd.Filter = "All Files (*.*)|*.*";
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            ofd.RestoreDirectory = true;
            ofd.Title = "Por favor, seleccione el archivo que desea enviar.";

            bool ok = (bool)ofd.ShowDialog();
            if (ok)
            {
                EnviarFichero(ofd.FileName);
            }            
        }

        internal event EventHandler<FicheroChangedEventArgs> EnviandoFichero;
        private void EnviarFicheroBGWorker(object sender, DoWorkEventArgs e)
        {
            ActualizarTextboxInfoEnvio(String.Format("Enviando archivo: {0}", (String)e.Argument));
            System.IO.Stream stream = System.IO.File.OpenRead((String)e.Argument);
            if (stream != null)
            {
                Packet packet = new Packet();
                packet.stream = stream;
                packet.nombreArchivo = System.IO.Path.GetFileName((String)e.Argument);
                MetadatosInfoFichero md = new MetadatosInfoFichero();
                md.LeerMetadatos((String)e.Argument);

                if (EnviandoFichero != null)
                {
                    EnviandoFichero(this, new FicheroChangedEventArgs(packet));
                }
            }
            ActualizarTextboxInfoEnvio("Archivo enviado con éxito.\n"); 
        }

        private delegate void _FicheroRecibidoHandler(FicheroChangedEventArgs e);
        internal void SetFicheroRecibido(FicheroChangedEventArgs e)
        {            
            if (TextBoxInfoEstadoFichero.CheckAccess())
            {
                if (FicheroRecibido != null)
                {
                    FicheroRecibido(this, e);
                }
                else
                {
                    Stream stream = null;
                    try
                    {
                        System.Windows.Forms.DialogResult dr = System.Windows.Forms.MessageBox.Show(e.Packet.nombreNodoEmisor + 
                                                                " le envía el archivo '" + e.Packet.nombreArchivo + "'. ¿Desea Aceptarlo?",
                                                                "Envío de archivo.", System.Windows.Forms.MessageBoxButtons.OKCancel);
                        if (dr == System.Windows.Forms.DialogResult.OK)
                        {
                            Microsoft.Win32.SaveFileDialog sfd;
                            sfd = new Microsoft.Win32.SaveFileDialog();
                            sfd.FileName = e.Packet.nombreArchivo;
                            sfd.Filter = "All Files (*.*)|*.*";
                            sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                            sfd.RestoreDirectory = true;
                            sfd.Title = "Ha recibido un archivo de " + e.Packet.nombreNodoEmisor + ". Por favor, seleccione el directorio donde desea guardarlo.";

                            bool ok = (bool)sfd.ShowDialog();
                            if (ok)
                            {
                                stream = new FileStream(sfd.FileName, FileMode.Create, FileAccess.Write);
                                AyudanteStream.CopiarStream(e.Packet.stream, stream);
                            }
                            if (ok)
                            {
                                ActualizarTextboxInfoEnvio(String.Format("Archivo recibido con éxito: {0}\n", (String)e.Packet.nombreArchivo));
                            }
                        }
                        else
                        {
                            ActualizarTextboxInfoEnvio(String.Format(" Archivo rechazado: {0}\n", (String)e.Packet.nombreArchivo));
                        }
                    }
                    catch (Exception)
                    {
                     //Excepcion transparente al usuario. Salta cuando se sobreescribe el archivo (si el archivo ya existe)
                    }
                    finally
                    {
                        if (stream != null)
                            stream.Close();
                    }
                }
            }
            else
            {
                TextBoxInfoEstadoFichero.Dispatcher.Invoke(DispatcherPriority.Normal, new _FicheroRecibidoHandler(SetFicheroRecibido), e);
            }
        }

        private delegate void _SetEstadoRedCambiadoHandler(string nombreUsuario, EstadoRedChangedEventArgs e);
        internal void SetEstadoRedCambiado(string nombreUsuario, EstadoRedChangedEventArgs e)
        {
            
            if (BotonEnviarFichero.CheckAccess())
            {
                if (nombreUsuario == e.Miembro && e.EsNuevoNodoUnidoARed)
                {
                    BotonEnviarFichero.IsEnabled = true;
                    lblTitulo.IsEnabled = true;
                }

                if (nombreUsuario == e.Miembro && e.EsNodoQueAbandonaRed)
                {
                    BotonEnviarFichero.IsEnabled = false;
                    lblTitulo.IsEnabled = false;
                }
               
                if (null != EstadoRedChanged)
                {
                    SetEstadoRedCambiado(nombreUsuario, e);
                }
            }
            else
            {
                BotonEnviarFichero.Dispatcher.Invoke(DispatcherPriority.Normal, new _SetEstadoRedCambiadoHandler(SetEstadoRedCambiado),  nombreUsuario, e);                
            }
        }

        private delegate void _ActualizarTextBoxInfoEstadoFicheroHandler(string mensaje);
        internal void ActualizarTextboxInfoEnvio(string mensaje)
        {
            if (mensaje != null)
            {
                if (TextBoxInfoEstadoFichero.CheckAccess())
                {
                    TextBoxInfoEstadoFichero.Text += mensaje + "\n";
                    TextBoxInfoEstadoFichero.ScrollToEnd();
                }
                else
                {
                    TextBoxInfoEstadoFichero.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, 
                                                    new _ActualizarTextBoxInfoEstadoFicheroHandler(ActualizarTextboxInfoEnvio), mensaje);
                }
            }
        }

        #endregion Métodos privados
    }
}
