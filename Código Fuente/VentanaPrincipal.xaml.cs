using System.Windows;
using System.Windows.Input;

namespace Peer2PeerApp
{
    public partial class VentanaPrincipal : Window
    {
        public VentanaPrincipal()
        {
            InitializeComponent();
        }

        private void VentanaPrincipal_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
                this.DragMove();
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnMinimizar_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        
        private void btnRed_Click(object sender, RoutedEventArgs e)
        {
            if (controlGestorRed.Visibility != System.Windows.Visibility.Visible)
            {
                controlGestorRed.Visibility = System.Windows.Visibility.Visible;
                controlGestorRed.TextBoxNombreUsuario.Focus();
            }
            else
                controlGestorRed.Visibility = System.Windows.Visibility.Hidden;
        }
        private void btnChat_Click(object sender, RoutedEventArgs e)
        {
            if (controlChat.Visibility != System.Windows.Visibility.Visible)
            {
                controlChat.Visibility = System.Windows.Visibility.Visible;
                controlChat.TextBoxNuevoMensajeChat.Focus();
            }
            else
                controlChat.Visibility = System.Windows.Visibility.Hidden;
        }
        private void btnFichero_Click(object sender, RoutedEventArgs e)
        {
            if (controlFichero.Visibility != System.Windows.Visibility.Visible)
                controlFichero.Visibility = System.Windows.Visibility.Visible;
            else
                controlFichero.Visibility = System.Windows.Visibility.Hidden;
        }
        private void btnAudio_Click(object sender, RoutedEventArgs e)
        {
            if (controlAudio.Visibility != System.Windows.Visibility.Visible)
                controlAudio.Visibility = System.Windows.Visibility.Visible;
            else
                controlAudio.Visibility = System.Windows.Visibility.Hidden;
        }
        private void btnVideo_Click(object sender, RoutedEventArgs e)
        {
            if (controlVideo.Visibility != System.Windows.Visibility.Visible)
                controlVideo.Visibility = System.Windows.Visibility.Visible;
            else
                controlVideo.Visibility = System.Windows.Visibility.Hidden;
        }
        private void btnDownload_Click(object sender, RoutedEventArgs e)
        {
            if (controlDescarga.Visibility != System.Windows.Visibility.Visible)
            {
                controlDescarga.Visibility = System.Windows.Visibility.Visible;
                controlDescarga.TextBoxBusqueda.Focus();
            }
            else
                controlDescarga.Visibility = System.Windows.Visibility.Hidden;
        }
        
        private void btnImagen_Click(object sender, RoutedEventArgs e)
        {
            if (controlImagen.Visibility != System.Windows.Visibility.Visible)
                controlImagen.Visibility = System.Windows.Visibility.Visible;
            else
                controlImagen.Visibility = System.Windows.Visibility.Hidden;
        }
    }
}
