using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Peer2PeerApp.Comunicacion;
using System.Windows.Input;

namespace Peer2PeerApp.WPF.Descarga
{
   
    public partial class DescargaUserControl : UserControl
    {

        public DescargaUserControl()
        {
            InitializeComponent();
        }

        private void DescargaUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (TextBoxBusqueda == null)
            {
                TextBoxBusqueda = txtBusqueda;
            }
            if (BotonDescargar == null)
            {
                BotonDescargar = btnDescargar;
            }
            if (BotonEnviarBusqueda == null)
            {
                BotonEnviarBusqueda = btnEnviarBusqueda;
            }
            if (BotonLimpiar == null)
            {
                BotonLimpiar = btnLimpiar;
            }
            if (TextBoxInfoEstadoDescarga == null)
            {
                TextBoxInfoEstadoDescarga = txtInfoEstadoDescarga;
            }
            if (ListBoxDescargasEncontradas == null)
            {
                ListBoxDescargasEncontradas = lstboxDescargasEncontradas;
            }
            if (ListBoxDescargasActivas == null)
            {
                ListBoxDescargasActivas = lstboxDescargasActivas;
            }

            BotonEnviarBusqueda.IsEnabled = false;
            BotonDescargar.IsEnabled = false;
            BotonLimpiar.IsEnabled = false;
            lblTitulo.IsEnabled = false;
        }

        #region Propiedades de dependencia

         public static DependencyProperty TextBoxBusquedaProperty = DependencyProperty.Register(
            "TextBoxBusqueda", typeof(TextBox), typeof(DescargaUserControl));
       
        public TextBox TextBoxBusqueda
        {
            get
            {
                try
                {
                    return (TextBox)this.Dispatcher.Invoke(
                       System.Windows.Threading.DispatcherPriority.Normal,
                       (DispatcherOperationCallback)delegate { return GetValue(TextBoxBusquedaProperty); },
                       TextBoxBusquedaProperty);
                }
                catch
                {

                    return (TextBox)TextBoxBusquedaProperty.DefaultMetadata.DefaultValue;
                }
            }
            set {
                    Dispatcher.Invoke(DispatcherPriority.Background,
                      (SendOrPostCallback)delegate { SetValue(TextBoxBusquedaProperty, value); },
                      value);
                }
        }

        public static DependencyProperty BotonEnviarBusquedaProperty = DependencyProperty.Register(
                    "BotonEnviarBusqueda", typeof(Button), typeof(DescargaUserControl), new PropertyMetadata(
                     null, new PropertyChangedCallback(OnBotonEnviarBusquedaChanged)));

        public Button BotonEnviarBusqueda
        {
            get
            {
                try
                {
                    return (Button)this.Dispatcher.Invoke(
                       System.Windows.Threading.DispatcherPriority.Normal,
                       (DispatcherOperationCallback)delegate { return GetValue(BotonEnviarBusquedaProperty); },
                       BotonEnviarBusquedaProperty);
                }
                catch
                {

                    return (Button)BotonEnviarBusquedaProperty.DefaultMetadata.DefaultValue;
                }
            }
            set
            {
                Dispatcher.Invoke(DispatcherPriority.Normal,
                  (SendOrPostCallback)delegate { SetValue(BotonEnviarBusquedaProperty, value); },
                  value);
            }
        }

        private static void OnBotonEnviarBusquedaChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DescargaUserControl controlDescarga = (DescargaUserControl)d;
            if (e.OldValue != null)
            {
                
                Button oldButton = (Button)e.NewValue;
                oldButton.Click -= new RoutedEventHandler(controlDescarga.BotonEnviarBusquedaRecurso_Click);
            }
            
            Button newButton = (Button)e.NewValue;
            newButton.Click += new RoutedEventHandler(controlDescarga.BotonEnviarBusquedaRecurso_Click);
        }

        public static DependencyProperty BotonDescargarProperty = DependencyProperty.Register(
                      "BotonDescargar", typeof(Button), typeof(DescargaUserControl), new PropertyMetadata(
                       null, new PropertyChangedCallback(OnBotonDescargarChanged)));

        public Button BotonDescargar
        {
            get
            {
                try
                {
                    return (Button)this.Dispatcher.Invoke(
                       System.Windows.Threading.DispatcherPriority.Normal,
                       (DispatcherOperationCallback)delegate { return GetValue(BotonDescargarProperty); },
                       BotonDescargarProperty);
                }
                catch
                {
                    return (Button)BotonDescargarProperty.DefaultMetadata.DefaultValue;
                }
            }
            set
            {
                Dispatcher.Invoke(DispatcherPriority.Normal,
                  (SendOrPostCallback)delegate { SetValue(BotonDescargarProperty, value); },
                  value);
            }
        }

        private static void OnBotonDescargarChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DescargaUserControl controlDescarga = (DescargaUserControl)d;
            if (e.OldValue != null)
            {
               
                Button oldButton = (Button)e.NewValue;
                oldButton.Click -= new RoutedEventHandler(controlDescarga.BotonDescargar_Click);
            }
            
            Button newButton = (Button)e.NewValue;
            newButton.Click += new RoutedEventHandler(controlDescarga.BotonDescargar_Click);
        }

        public static DependencyProperty BotonLimpiarProperty = DependencyProperty.Register(
             "BotonLimpiar", typeof(Button), typeof(DescargaUserControl), new PropertyMetadata(
              null, new PropertyChangedCallback(OnBotonLimpiarChanged)));

        public Button BotonLimpiar
        {
            get
            {
                try
                {
                    return (Button)this.Dispatcher.Invoke(
                       System.Windows.Threading.DispatcherPriority.Normal,
                       (DispatcherOperationCallback)delegate { return GetValue(BotonLimpiarProperty); },
                       BotonLimpiarProperty);
                }
                catch
                {

                    return (Button)BotonLimpiarProperty.DefaultMetadata.DefaultValue;
                }
            }
            set
            {
                Dispatcher.Invoke(DispatcherPriority.Normal,
                  (SendOrPostCallback)delegate { SetValue(BotonLimpiarProperty, value); },
                  value);
            }
        }

        private static void OnBotonLimpiarChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DescargaUserControl controlDescarga = (DescargaUserControl)d;
            if (e.OldValue != null)
            {
                Button oldButton = (Button)e.NewValue;
                oldButton.Click -= new RoutedEventHandler(controlDescarga.BotonLimpiar_Click);
            }
           
            Button newButton = (Button)e.NewValue;
            newButton.Click += new RoutedEventHandler(controlDescarga.BotonLimpiar_Click);
        }
        
        public static DependencyProperty TextBoxInfoEstadoDescargaProperty = DependencyProperty.Register(
        "TextBoxInfoEstadoDescarga", typeof(TextBox), typeof(DescargaUserControl));

        public TextBox TextBoxInfoEstadoDescarga
        {
            get
            {
                try
                {
                    return (TextBox)this.Dispatcher.Invoke(
                       System.Windows.Threading.DispatcherPriority.Normal,
                       (DispatcherOperationCallback)delegate { return GetValue(TextBoxInfoEstadoDescargaProperty); },
                       TextBoxInfoEstadoDescargaProperty);
                }
                catch
                {

                    return (TextBox)TextBoxInfoEstadoDescargaProperty.DefaultMetadata.DefaultValue;
                }
            }
            set
            {
                Dispatcher.Invoke(DispatcherPriority.Normal,
                  (SendOrPostCallback)delegate { SetValue(TextBoxInfoEstadoDescargaProperty, value); },
                  value);
            }
        }

        public static DependencyProperty ListBoxDescargasEncontradasProperty = DependencyProperty.Register(
       "ListBoxDescargasEncontradas", typeof(ListBox), typeof(DescargaUserControl));

        public ListBox ListBoxDescargasEncontradas
        {
            get
            {
                try
                {
                    return (ListBox)this.Dispatcher.Invoke(
                       System.Windows.Threading.DispatcherPriority.Normal,
                       (DispatcherOperationCallback)delegate { return GetValue(ListBoxDescargasEncontradasProperty); },
                       ListBoxDescargasEncontradasProperty);
                }
                catch
                {
                    return (ListBox)ListBoxDescargasEncontradasProperty.DefaultMetadata.DefaultValue;
                }
            }
            set
            {
                Dispatcher.Invoke(DispatcherPriority.Normal,
                  (SendOrPostCallback)delegate { SetValue(ListBoxDescargasEncontradasProperty, value); },
                  value);
            }
        }
       
        public static DependencyProperty ListBoxDescargasActivasProperty = DependencyProperty.Register(
        "ListBoxDescargasActivas", typeof(ListBox), typeof(DescargaUserControl));

        public ListBox ListBoxDescargasActivas
        {
            get
            {
                try
                {
                    return (ListBox)this.Dispatcher.Invoke(
                       System.Windows.Threading.DispatcherPriority.Normal,
                       (DispatcherOperationCallback)delegate { return GetValue(ListBoxDescargasActivasProperty); },
                       ListBoxDescargasActivasProperty);
                }
                catch
                {

                    return (ListBox)ListBoxDescargasActivasProperty.DefaultMetadata.DefaultValue;
                }
            }
            set
            {
                Dispatcher.Invoke(DispatcherPriority.Normal,
                  (SendOrPostCallback)delegate { SetValue(ListBoxDescargasActivasProperty, value); },
                  value);
            }
        }

        #endregion Propiedades de dependencia

        #region Métodos públicos

        public void BuscarRecurso(string nombreArchivo)
        {
            if (nombreArchivo != null)
            {
                BackgroundWorker worker;
                worker = new System.ComponentModel.BackgroundWorker();
                worker.DoWork += new System.ComponentModel.DoWorkEventHandler(EnviarBusquedaRecursoBGWorker);
                worker.RunWorkerAsync(nombreArchivo);
            }
            else
            {
                throw new ApplicationException(string.Format("Error en el fichero: {0}", nombreArchivo));
            }
        }

        public void EnviarPeticionDescarga(string nombreArchivoYNodoDestino)
        {
            if (nombreArchivoYNodoDestino != null)
            {
                BackgroundWorker worker;
                worker = new System.ComponentModel.BackgroundWorker();
                worker.DoWork += new System.ComponentModel.DoWorkEventHandler(EnviarPeticionDescargaBGWorker);
                worker.RunWorkerAsync(nombreArchivoYNodoDestino);
            }
            else
            {
                throw new ApplicationException(string.Format("Error la peticíon de descarga: {0}", nombreArchivoYNodoDestino));
            }
        }

        public event EventHandler<EstadoRedChangedEventArgs> EstadoRedChanged;

        #endregion Métodos públicos

        #region Métodos privados

        internal event EventHandler<DescargaChangedEventArgs> EnviandoBusqueda;
        private void BotonEnviarBusquedaRecurso_Click(object sender, EventArgs e)
        {
            if (EnviandoBusqueda != null)
            {
                if (!string.IsNullOrEmpty(TextBoxBusqueda.Text.Trim()))
                {
                    ListBoxDescargasEncontradas.Items.Clear();
                    string nombreArchivo = TextBoxBusqueda.Text.Trim();
                    BuscarRecurso(nombreArchivo);
                }
                else
                {
                    MessageBox.Show("No ha introduccido ningún criterio de búsqueda. Por favor, introduzca un criterio de búsqueda.", 
                                    "Acción requerida.", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            TextBoxBusqueda.Clear();
        }

        private void EnviarBusquedaRecursoBGWorker(object sender, DoWorkEventArgs e)
        {
            ActualizarTextboxInfoEstadoDescarga(String.Format("Enviando busqueda: {0}.", (String)e.Argument));

            if (EnviandoBusqueda != null)
            {
                DownloadPacket packet = new DownloadPacket();
                packet.nombreArchivo = (String)e.Argument;
                packet.nombreNodoDestino = "*"; //busqueda enviada a todos los nodos conectados (*)
                packet.comando = "_BUSQUEDA_";

                if (EnviandoBusqueda != null)
                {
                    EnviandoBusqueda(this, new DescargaChangedEventArgs(packet));
                    ActualizarTextboxInfoEstadoDescarga(String.Format("Buscando...\n"));
                }
                else
                {
                    ActualizarTextboxInfoEstadoDescarga("Error de envío. Por favor, inténtelo de nuevo. Si el error persiste, reinicie la aplicación.");
                }
            }
        }

        internal event EventHandler<DescargaChangedEventArgs> EnviandoDescarga;
        private void BotonDescargar_Click(object sender, EventArgs e)
        {
            if (EnviandoDescarga != null)
            {
                if ((ListBoxDescargasEncontradas.SelectedItem != null))
                {
                    string text = ListBoxDescargasEncontradas.SelectedItem.ToString();

                    string[] palabras = text.Split(new[] { '~' }, StringSplitOptions.RemoveEmptyEntries);

                    string nombreArchivo = palabras[0];
                    string para = palabras[2];

                    ActualizarListBoxDescargas(nombreArchivo);

                    ListBoxDescargasEncontradas.Items.Remove(ListBoxDescargasEncontradas.SelectedItem);
                    ListBoxDescargasEncontradas.SelectedIndex = -1;

                    EnviarPeticionDescarga(nombreArchivo + ":" + para);
                }
                else
                {
                    MessageBox.Show("No ha seleccionado ningún archivo. Por favor, seleccione un archivo de la lista Archivos Encontrados.",
                                    "Acción requerida.", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            } 
        }

        private void EnviarPeticionDescargaBGWorker(object sender, DoWorkEventArgs e)
        {
            ActualizarTextboxInfoEstadoDescarga(String.Format("Enviando petición de descarga: {0}", (String)e.Argument));

            string text = (String)e.Argument;

            string[] palabras = text.Split(':');

            string nombreArchivo = palabras[0];
            string para = palabras[1];

            DownloadPacket packet = new DownloadPacket();
            packet.nombreArchivo = nombreArchivo;
            packet.nombreNodoDestino = para;
            packet.comando = "_PETICION_DESCARGA_";
            if (EnviandoDescarga != null)
            {
                EnviandoDescarga(this, new DescargaChangedEventArgs(packet));
                ActualizarTextboxInfoEstadoDescarga(String.Format("Petición de descarga enviada con éxito.",(String)e.Argument));
            }
        }

        private void BotonLimpiar_Click(object sender, EventArgs e)
        {
            if ((ListBoxDescargasEncontradas.SelectedIndex != -1) || (ListBoxDescargasActivas.SelectedIndex != -1))
            {
                while (ListBoxDescargasEncontradas.SelectedIndex != -1)
                    ListBoxDescargasEncontradas.Items.Remove(ListBoxDescargasEncontradas.SelectedItem);

                while (ListBoxDescargasActivas.SelectedIndex != -1)
                    ListBoxDescargasActivas.Items.Remove(ListBoxDescargasActivas.SelectedItem); 
            }
            else
            {
                MessageBox.Show("Por favor, seleccione al menos un archivo de las listas Archivos Encontrados y/o Descargas Activas.", 
                                "Acción requerida.", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private delegate void _SetMensajeDeBusquedaRecibidoHandler(DescargaChangedEventArgs e);
        internal void SetMensajeDeBusquedaRecibido(DescargaChangedEventArgs e)
        {
            if (TextBoxInfoEstadoDescarga.CheckAccess())
            {
                try
                {
                    DirectoryInfo directorio = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\MisArchivosCompartidos_P2P");
                    string path = null;

                    if (directorio.Exists)
                    {
                        path = directorio.ToString();
                    }
                    else
                    {
                        string directorioNuevo = System.IO.Path.Combine(directorio.ToString().Replace(@"\MisArchivosCompartidos_P2P", ""), "MisArchivosCompartidos_P2P");
                        System.IO.Directory.CreateDirectory(directorioNuevo);
                        path = directorioNuevo;
                    }

                    DirectoryInfo directorioBusqueda = new DirectoryInfo(path);
                    System.IO.FileInfo[] ficheros = directorioBusqueda.GetFiles("*" + e.Packet.nombreArchivo + "*.*");

                    foreach (System.IO.FileInfo f in ficheros)
                    {
                        e.Packet.comando = "_RESPUESTA_BUSQUEDA_";
                        e.Packet.nombreArchivo = f.Name;
                        e.Packet.tamanio = f.Length;

                        if (EnviandoBusqueda != null)
                        {
                            EnviandoBusqueda(this, new DescargaChangedEventArgs(e.Packet));
                        }
                    }
                }
                catch (Exception)
                {

                }
            }
            else
            {
                TextBoxInfoEstadoDescarga.Dispatcher.Invoke(DispatcherPriority.Normal, new _SetMensajeDeBusquedaRecibidoHandler(SetMensajeDeBusquedaRecibido), e);
            }
        }

        static double ConvertirBytesAKiloMegaOGigaBytes(long bytes)
        {
            return bytes / 1024f;
        }

        private delegate void _SetRespuestaAlMensajeDeBusquedaRecibidoHandler(DescargaChangedEventArgs e);
        internal void SetRespuestaABusqueda(DescargaChangedEventArgs e)
        {
            if (TextBoxInfoEstadoDescarga.CheckAccess())
            {
                try
                {
                    double tamFich = e.Packet.tamanio;
                    string strTam = " Bytes";

                    if (tamFich >= 1024)
                    {
                        tamFich = ConvertirBytesAKiloMegaOGigaBytes((long)tamFich);
                        strTam = " KB";
                    }
                    if (tamFich >= 1024)
                    {
                        tamFich = ConvertirBytesAKiloMegaOGigaBytes((long)tamFich);
                        strTam = " MB";
                    }
                    if (tamFich >= 1024)
                    {
                        tamFich = ConvertirBytesAKiloMegaOGigaBytes((long)tamFich);
                        strTam = " GB";
                    }

                    strTam = tamFich.ToString("0.00") + strTam;

                    ActualizarListBoxBusquedas(String.Format(e.Packet.nombreArchivo + strTam.PadLeft(15, '~') + e.Packet.nombreNodoEmisor.PadLeft(15, '~')));
                }
                catch (Exception)
                {

                }
            }
            else
            {
                TextBoxInfoEstadoDescarga.Dispatcher.Invoke(DispatcherPriority.Normal, new _SetRespuestaAlMensajeDeBusquedaRecibidoHandler(SetRespuestaABusqueda), e);
            }
        }

        private delegate void _SetPeticionDeDescargaRecibidaHandler(DescargaChangedEventArgs e);
        internal void SetPeticionDeDescargaRecibida(DescargaChangedEventArgs e)
        {
            if (TextBoxInfoEstadoDescarga.CheckAccess())
            {
                 try
                 {
                     DirectoryInfo directorio = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\MisArchivosCompartidos_P2P");
                     System.IO.FileInfo[] ficheros = directorio.GetFiles(e.Packet.nombreArchivo);
                     System.IO.Stream stream = ficheros[0].OpenRead();

                     if (stream != null)
                     {
                         e.Packet.stream = stream;
                         e.Packet.comando = "_DESCARGA_";

                         if (EnviandoDescarga != null)
                         {
                             EnviandoDescarga(this, new DescargaChangedEventArgs(e.Packet));
                         }
                     }
                 }
                 catch (Exception)
                 {

                 }
            }
            else
            {
                TextBoxInfoEstadoDescarga.Dispatcher.Invoke(DispatcherPriority.Normal, new _SetPeticionDeDescargaRecibidaHandler(SetPeticionDeDescargaRecibida), e);
            }
        }

        private delegate void _SetDescargaRecibidaHandler(DescargaChangedEventArgs e);
        internal void SetDescarga(DescargaChangedEventArgs e)
        {
            if (TextBoxInfoEstadoDescarga.CheckAccess())
            {
                Stream stream = null;
                DirectoryInfo directorio = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\MisArchivosCompartidos_P2P");
                string path = null;

                try
                {
                    if (directorio.Exists)
                    {
                        path = directorio.ToString() + @"\" + e.Packet.nombreArchivo;

                        int cont = 0;
                        while (File.Exists(path))
                        {
                            cont++;
                            path = directorio.ToString() + @"\" + "copia (" + cont + ") - " + e.Packet.nombreArchivo;
                        }
                    }
                    else
                    {
                        string directorioNuevo = System.IO.Path.Combine(directorio.ToString().Replace(@"\MisArchivosCompartidos_P2P", ""), "MisArchivosCompartidos_P2P");

                        System.IO.Directory.CreateDirectory(directorioNuevo);

                        path = directorioNuevo + @"\" + e.Packet.nombreArchivo;

                        int cont = 0;
                        while (File.Exists(e.Packet.nombreArchivo))
                        {
                            cont++;
                            path = directorioNuevo + @"\" + "copia (" + cont + ") - " + e.Packet.nombreArchivo;
                        }
                    }

                    stream = new FileStream(path, FileMode.Create, FileAccess.Write);
                    AyudanteStream.CopiarStream(e.Packet.stream, stream);

                    ActualizarTextboxInfoEstadoDescarga(String.Format("Archivo '{0}' guardado en MisArchivosCompartidos_P2P", e.Packet.nombreArchivo));
                    ActualizarTextboxInfoEstadoDescarga("Descarga completada con éxito.\n");
                }
                catch (Exception)
                {

                }
                finally
                {
                    if (stream != null)
                        stream.Close();
                }
            }
            else
            {
                TextBoxInfoEstadoDescarga.Dispatcher.Invoke(DispatcherPriority.Normal, new _SetDescargaRecibidaHandler(SetDescarga), e);
            }
        }

        private delegate void _ActualizarListboxBusquedasHandler(String mensaje);
        internal void ActualizarListBoxBusquedas(string mensaje)
        {
            if (ListBoxDescargasEncontradas.CheckAccess())
            {
               
                ListBoxDescargasEncontradas.Items.Add(mensaje);
            }
            else
            {
               
                ListBoxDescargasEncontradas.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new _ActualizarListboxBusquedasHandler(ActualizarListBoxBusquedas), mensaje);
            }
        }

        private delegate void _ActualizarListboxDescargasHandler(String mensaje);
        internal void ActualizarListBoxDescargas(string mensaje)
        {
            if (ListBoxDescargasActivas.CheckAccess())
            {
               
                ListBoxDescargasActivas.Items.Add(mensaje);
            }
            else
            {
               
                ListBoxDescargasActivas.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new _ActualizarListboxDescargasHandler(ActualizarListBoxDescargas), mensaje);
            }
        }

        private delegate void _ActualizarTextboxInfoEstadoDescargaHandler(String mensaje);
        internal void ActualizarTextboxInfoEstadoDescarga(string mensaje)
        {
            if (TextBoxInfoEstadoDescarga.CheckAccess())
            {
               
                TextBoxInfoEstadoDescarga.Text += mensaje + "\n";
                TextBoxInfoEstadoDescarga.ScrollToEnd();
            }
            else
            {
               
                TextBoxInfoEstadoDescarga.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new _ActualizarTextboxInfoEstadoDescargaHandler(ActualizarTextboxInfoEstadoDescarga), mensaje);
            }
        }

        private delegate void _SetEstadoRedCambiadoHandler(string userName, EstadoRedChangedEventArgs e);
        internal void SetEstadoRedCambiado(string userName, EstadoRedChangedEventArgs e)
        {

            if (BotonDescargar.CheckAccess())
            {
                if (userName == e.Miembro && e.EsNuevoNodoUnidoARed)
                {
                    BotonDescargar.IsEnabled = true;
                    BotonEnviarBusqueda.IsEnabled = true;
                    BotonLimpiar.IsEnabled = true;
                    lblTitulo.IsEnabled = true;
                }

                if (userName == e.Miembro && e.EsNodoQueAbandonaRed)
                {
                    BotonDescargar.IsEnabled = false;
                    BotonEnviarBusqueda.IsEnabled = false;
                    BotonLimpiar.IsEnabled = false;
                    lblTitulo.IsEnabled = false;
                }

               
                if (null != EstadoRedChanged)
                {
                    SetEstadoRedCambiado(userName, e);
                }
            }
            else
            {
                BotonDescargar.Dispatcher.Invoke(DispatcherPriority.Normal, new _SetEstadoRedCambiadoHandler(SetEstadoRedCambiado), userName, e);
            }
        }

        #endregion Métodos privados
    }
}
