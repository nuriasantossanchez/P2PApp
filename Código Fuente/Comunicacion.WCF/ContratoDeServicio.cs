using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.PeerResolvers;
using System.Windows;
using System.Windows.Threading;


namespace Peer2PeerApp.Comunicacion
{
    public class ContratoDeServicio : IContratoDeServicio, IDisposable
    {
        string mNombreNodo;                                
        string mPassword;                                  
        string mNombreRed;                     
        InstanceContext mContextoDeInstancia;                 
        DuplexChannelFactory<IContratoDeServicioCliente> mFabricaDeCanalesDuplex; //proxy
        IContratoDeServicioCliente mParticipanteP2P;                 

        public ContratoDeServicio()
        {
            mNombreNodo = mPassword = mNombreRed = null;
            mFabricaDeCanalesDuplex = null;
            mParticipanteP2P = null;
            mDispose = false;
        }

        #region Conexión

        public bool IniciarCanalP2P(String nombreNodo, String nombreRed, String password)
        {
            CheckDispose();
                      
            mNombreNodo = nombreNodo;
            mNombreRed = nombreRed;
            mPassword = password;

            if (string.IsNullOrEmpty(password.Trim()))
            {
                mPassword = "pass";
            }
            else
            {
                mPassword = password.Trim();
            }
           
            mContextoDeInstancia = new InstanceContext(this);
  
            NetPeerTcpBinding p2pBinding = new NetPeerTcpBinding();
            p2pBinding.Name = "BindingDefault";
            p2pBinding.Port = 0; //puerto dinámico
            p2pBinding.MaxReceivedMessageSize = 70000000; //(70mb)          
            p2pBinding.Resolver.Mode = PeerResolverMode.Pnrp;
  
            String bindingInfo = "net.p2p://";  
            EndpointAddress epa = new EndpointAddress(String.Concat(bindingInfo, mNombreRed));
            ServiceEndpoint serviceEndpoint = new ServiceEndpoint(ContractDescription.GetContract(typeof(IContratoDeServicio)), p2pBinding, epa);
        
            mFabricaDeCanalesDuplex = new DuplexChannelFactory<IContratoDeServicioCliente>(mContextoDeInstancia, serviceEndpoint);
            mFabricaDeCanalesDuplex.Credentials.Peer.MeshPassword = mPassword;
            mParticipanteP2P = mFabricaDeCanalesDuplex.CreateChannel();

            mAyudanteImagen = new AyudanteImagen(mParticipanteP2P.ServicioGestionarImagen, mNombreNodo);
            mAyudanteChat = new AyudanteChat(mParticipanteP2P.ServicioGestionarChat);
            mAyudanteFichero = new AyudanteFichero(mParticipanteP2P.ServicioGestionarFichero);
            mAyudanteAudio = new AyudanteStream(mParticipanteP2P, mNombreNodo, mParticipanteP2P.ServicioGestionarAudio);
            mAyudanteVideo = new AyudanteStream(mParticipanteP2P, mNombreNodo, mParticipanteP2P.ServicioGestionarVideo);
            mAyudanteDescarga = new AyudanteDescarga(mParticipanteP2P.ServicioGestionarDescarga);

            try
            {
                AsyncCallback callBackAsync = new AsyncCallback(AbrirProceso);
                TimeSpan ts = new TimeSpan(0, 1, 0); 
                mParticipanteP2P.BeginOpen(ts, callBackAsync, this);
            }
            catch (CommunicationException e)
            {
                System.Windows.MessageBox.Show(String.Format("Error al acceder a la red: {0}", e.Message));
                mParticipanteP2P = null;
                return false;
            }
            return true;
        }

        void AbrirProceso(IAsyncResult resultAsync)
        {
            if (false == mDispose)
            {
                try
                {
                    mParticipanteP2P.EndOpen(resultAsync);
                }
                catch (TimeoutException)
                {
                    MessageBox.Show("Ha sido imposible inicializar la red. Excedido el tiempo máximo de espera permitido.\n" + 
                                    "\nLo sentimos, la aplicación se cerrará.",
                                    "Error crítico.",
                                    MessageBoxButton.OK, MessageBoxImage.Error);

                    Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new System.Action(() =>
                    {
                        Application.Current.Shutdown();
                    }));
                }
                catch (Exception e)
                {
                    if (null != e.InnerException)
                        MessageBox.Show(String.Format("Detectada conexión a Internet a través de Proxy: {0}\n" +
                                                        "\nLo sentimos, la aplicación se cerrará.",
                                                        e.InnerException.Message),
                                                        "Error crítico.",
                                                        MessageBoxButton.OK, MessageBoxImage.Error);
                    mParticipanteP2P = null;
                    Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new System.Action(() =>
                    {
                        Application.Current.Shutdown();
                    }));               
                }

                if (mFabricaDeCanalesDuplex.State != CommunicationState.Faulted)
                    if (mParticipanteP2P != null)
                        mParticipanteP2P.ServicioUnirseARed(mNombreNodo);      
            }
        }

        ~ContratoDeServicio()
        {
            Dispose(false);
        }
       
        public void CerrarProceso()
        {
            (this as IDisposable).Dispose();
        }

        #endregion Conexión

        #region Ayudantes IContratoDeServicio

        private AyudanteStream mAyudanteVideo;
        public AyudanteStream AyudanteVideo
        {
            get { CheckDispose(); return mAyudanteVideo; }
            set { CheckDispose(); mAyudanteVideo = value; }
        }

        public void ServicioGestionarVideo(StreamPacket packet)
        {
            CheckDispose();
            mAyudanteVideo.CompartirStream(packet);
        }
       
        private AyudanteStream mAyudanteAudio;
        public AyudanteStream AyudanteAudio
        {
            get { CheckDispose(); return mAyudanteAudio; }
            set { CheckDispose(); mAyudanteAudio = value; }
        }

        public void ServicioGestionarAudio(StreamPacket packet)
        {
            CheckDispose();
            mAyudanteAudio.CompartirStream(packet);
        }

        private AyudanteChat mAyudanteChat;
        public AyudanteChat AyudanteChat
        {
            get { CheckDispose(); return mAyudanteChat; }
            set { CheckDispose(); mAyudanteChat = value; }
        }
        public void ServicioGestionarChat(string mienbro, string mensaje)
        {
            CheckDispose();
            mAyudanteChat.EnviarMensajeAParticipante(mienbro, mensaje);
        }

        private AyudanteImagen mAyudanteImagen;
        public AyudanteImagen AyudanteImagen
        {
            get { CheckDispose(); return mAyudanteImagen; }
            set { CheckDispose(); mAyudanteImagen = value; }
        }
        public void ServicioGestionarImagen(ImagenPacket packet)
        {
            CheckDispose();
            mAyudanteImagen.EnviarImagenAParticipante(packet);
        }
      
        private AyudanteFichero mAyudanteFichero;
        public AyudanteFichero AyudanteFichero
        {
            get { CheckDispose(); return mAyudanteFichero; }
            set { CheckDispose(); mAyudanteFichero = value; }
        }
        public void ServicioGestionarFichero(Packet packet)
        {
            CheckDispose();
            if (packet.nombreNodoEmisor != mNombreNodo)
                mAyudanteFichero.EnviarFicheroAParticipante(packet);
        }

        private AyudanteDescarga mAyudanteDescarga;
        public AyudanteDescarga AyudanteDescarga
        {
            get { CheckDispose(); return mAyudanteDescarga; }
            set { CheckDispose(); mAyudanteDescarga = value; }
        }

        public void ServicioGestionarDescarga(DownloadPacket packet)
        {
            CheckDispose();
            if (packet.nombreNodoEmisor != mNombreNodo)
                if (packet.nombreNodoDestino == mNombreNodo || packet.nombreNodoDestino == "*")
                    mAyudanteDescarga.BuscarDescargarRecurso(packet);
        }

        private event EventHandler<EstadoRedChangedEventArgs> mEstadoRedChanged;
        public event EventHandler<EstadoRedChangedEventArgs> EstadoRedChanged
        {
            add { CheckDispose(); mEstadoRedChanged += value; }
            remove { CheckDispose(); mEstadoRedChanged -= value; }
        }

        public void ServicioUnirseARed(string mienbro)
        {
            CheckDispose();
           
            if (null != mEstadoRedChanged)
            {
                mEstadoRedChanged(this, new EstadoRedChangedEventArgs(mienbro, true, false));
            }
        }

        public void ServicioAbandonarRed(string mienbro)
        {
            CheckDispose();
           
            if (null != mEstadoRedChanged)
            {
                mEstadoRedChanged(this, new EstadoRedChangedEventArgs(mienbro, false, true));
            } 
        }

        #endregion Ayudantes IContratoDeServicio

        #region Dispose

        private bool mDispose;
        protected void CheckDispose()
        {
            try
            {
                if (mDispose)
                {
                    throw new ObjectDisposedException(this.GetType().FullName);
                }
            }
            catch
            { 

            }
        }

        public void Dispose()
        {
            if (mDispose)
            {
                return;
            }

            try
            {
                if (mParticipanteP2P != null)
                {
                    mParticipanteP2P.ServicioAbandonarRed(mNombreNodo);
                }

                if (mFabricaDeCanalesDuplex != null)
                {
                    mFabricaDeCanalesDuplex.Close();
                }
            }
            catch (InvalidOperationException)
            {

            }
            catch (CommunicationObjectFaultedException)
            {
                //canal de comuniación en estado Faulted
                mFabricaDeCanalesDuplex.Abort();
            }
            finally
            {
                if (mParticipanteP2P != null)
                {
                    mParticipanteP2P.Abort();
                    mParticipanteP2P.Dispose();
                }
                mParticipanteP2P = null;
                mDispose = true;
            }
        }

        virtual protected void Dispose(bool disposeAll)
        {
            (this as IDisposable).Dispose();
        }

        #endregion Dispose
    }
}
