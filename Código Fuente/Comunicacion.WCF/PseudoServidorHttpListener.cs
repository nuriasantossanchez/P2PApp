using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Threading;


namespace Peer2PeerApp.Comunicacion
{
    public class PseudoServidorHttpListener
    {
        Stream mPseudoStream;
        public Stream mSyncStream;
        long mSyncStreamUltimaPosicionEscrita;
        long mSyncStreamUltimaPosicionLeida;
        bool mStreamEntrante;
        String mNombreNodoReceptor, mNombreArchivoGuardado;
        private BackgroundWorker mPseudoservidorHttpListenerThreadBGWorker;
        System.Collections.Hashtable mHashTable;
        int mTrozoEsperado;
        Guid mGuidReceptor;
        Timer mTimer;
        private static readonly object Lock = new object();
        bool mEnvio;
        bool mPseudoServidorPreparado;
        String mNombreUnico;
       
        public delegate void WorkerThreadTerminado(object sender, RunWorkerCompletedEventArgs e);

        public delegate void MensajeInfo(String s);
        MensajeInfo mMensajeInfo;
       
        public delegate void IniciarStream(String nombreArchivo);
        IniciarStream mIniciarStream;
      
        public delegate bool TrozoStreamEnviado();
        TrozoStreamEnviado mStreamEnviado;

        public PseudoServidorHttpListener(String nombreUnico, WorkerThreadTerminado threadTerminadoDelegate,
                                        MensajeInfo mensaje, IniciarStream iniciarStream, TrozoStreamEnviado streamEnviado)
        {
            mMensajeInfo = mensaje;
            mNombreUnico = nombreUnico;
            mIniciarStream = iniciarStream;
            mStreamEnviado = streamEnviado;
          
            mPseudoservidorHttpListenerThreadBGWorker = new System.ComponentModel.BackgroundWorker();
            mPseudoservidorHttpListenerThreadBGWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(PseudoServidorHttp);
            mPseudoservidorHttpListenerThreadBGWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(threadTerminadoDelegate);
            mPseudoservidorHttpListenerThreadBGWorker.WorkerSupportsCancellation = true;
            mPseudoservidorHttpListenerThreadBGWorker.RunWorkerAsync(null);

            mPseudoServidorPreparado = false;
            mStreamEntrante = false;
            mSyncStreamUltimaPosicionEscrita = 0;
            mPseudoStream = new MemoryStream();
            mSyncStream = Stream.Synchronized(mPseudoStream);
            mHashTable = new System.Collections.Hashtable();

            mTimer = new Timer(new TimerCallback(TimerCallback));
            mTimer.Change(0, 3500);
        }

        public void Salir()
        {
            mPseudoservidorHttpListenerThreadBGWorker.CancelAsync();
        }

        public void EnviarDatoStream(String rutaYNombreArchivo)
        {
            BackgroundWorker worker;
            worker = new System.ComponentModel.BackgroundWorker();
            worker.DoWork += new System.ComponentModel.DoWorkEventHandler(EnviarBGWorker);
            worker.RunWorkerAsync(rutaYNombreArchivo);
            mEnvio = true;
        }

        public void EnviarBGWorker(object sender, DoWorkEventArgs e)
        {
            if (mMensajeInfo != null)
                mMensajeInfo(String.Format("Procesando archivo multimedia: {0}\n", (String)e.Argument));

            mIniciarStream((String)e.Argument);

            while (!mStreamEnviado());

            if (mMensajeInfo != null)
                mMensajeInfo("Procesamiento completado.\n");

            mEnvio = false;
        }

        public void StreamChanged(StreamChangedEventArgs e)
        {
            if (e.StreamPacket.guid != mGuidReceptor)
            {
                lock (Lock)
                {
                    mSyncStreamUltimaPosicionEscrita = 0;

                    mSyncStream.Close();
                    mSyncStream.Flush();
                    mPseudoStream.Close();
                    mPseudoStream = new MemoryStream();
                    mSyncStream = Stream.Synchronized(mPseudoStream);
                }
                mGuidReceptor = e.StreamPacket.guid;
                mNombreNodoReceptor = e.StreamPacket.nombreArchivo;
                mHashTable.Clear();
                mTrozoEsperado = 0;
            }
            if (e.StreamPacket.numPacket != mTrozoEsperado)
            {
                mHashTable.Add(e.StreamPacket.numPacket, e.StreamPacket);
                return;
            }
            else
            {  
                ActualizarStream(e.StreamPacket);
                mTrozoEsperado++;

                while (mHashTable.Contains(mTrozoEsperado))
                {
                    ActualizarStream((StreamPacket)mHashTable[mTrozoEsperado]);
                    mTrozoEsperado++;
                }
            }

            if (e.StreamPacket.numPacket == 0)
            {
                mNombreArchivoGuardado = StringHttpGuardado(e.StreamPacket.nombreArchivo);
            }
        }

        protected void ActualizarStream(StreamPacket packet)
        {
            lock (Lock)
            {
                mSyncStream.Position = mSyncStreamUltimaPosicionEscrita;
                AyudanteStream.CopiarStream(packet.stream, mSyncStream);
                mSyncStreamUltimaPosicionEscrita = mSyncStream.Position;
                mStreamEntrante = !packet.esFinalDeStream;

                if (mStreamEntrante == false)
                {
                    mMensajeInfo("Procesamiento multimedia finalizado con éxito.\n");
                }
            }
        }

        public bool EstaPseudoServidorPreparado
        {
            get { return mPseudoServidorPreparado; }
        }

        public void PseudoServidorHttp(object sender, DoWorkEventArgs e)
        {
            HttpListener listenerHttp = new HttpListener();
            IAsyncResult resultAsync = null;
            try
            {
                if (!HttpListener.IsSupported)
                {
                    MessageBox.Show("Se requiere Windows Vista, XP SP2, Server 2003 o superior para usar esta aplicación.\n" +
                                    "\nLo sentimos, la aplicación se cerrará.",
                                    "Error crítico.",
                                    MessageBoxButton.OK, MessageBoxImage.Error);

                    Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new System.Action(() =>
                    {
                        Application.Current.Shutdown();
                    }));
                }

                listenerHttp.Prefixes.Add("http://localhost:8088/" + mNombreUnico);
                listenerHttp.Start();
                mPseudoServidorPreparado = true;

                resultAsync = listenerHttp.BeginGetContext(new AsyncCallback(ListenerHttpCallback), listenerHttp);
                
                while (true)
                {
                    System.Threading.Thread.Sleep(500);
                    if (mPseudoservidorHttpListenerThreadBGWorker.CancellationPending)
                    {
                        break;
                    }
                }

                listenerHttp.Prefixes.Clear();
                listenerHttp.Stop();
                listenerHttp.Close();
            }
            catch (System.Net.HttpListenerException)
            {
                //nombre de usuario ya existe en esa malla
                listenerHttp.Close();
                mPseudoServidorPreparado = false;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Ocurrió una excepción en 'PseudoServidorHttp': {0}", ex.Message);
                mPseudoServidorPreparado = false;
            }
        }

        public void ListenerHttpCallback(IAsyncResult resultAsync)
        {
            HttpListener listenerHttp = (HttpListener)resultAsync.AsyncState;

            if (listenerHttp.IsListening)
            {
                try
                {
                    HttpListenerContext context = listenerHttp.EndGetContext(resultAsync);
                    HttpListenerResponse response = context.Response;

                    response.KeepAlive = true;
                    response.SendChunked = true;
                    response.ContentType = "audio/x-ms-wma";
                    response.AddHeader("Content-Disposition", "attachment;nombreArchivo=" + mNombreArchivoGuardado);
                    response.SendChunked = true;
                    System.IO.Stream streamSaliente = response.OutputStream;

                    mMensajeInfo(String.Format("{0} - Servicio Arrancado.\n", DateTime.Now));

                    byte[] buffer = new byte[12000];//12K
                    int longitud = 1;
                    mSyncStreamUltimaPosicionLeida = 0;

                    while (longitud > 0 || mStreamEntrante == true)
                    {
                        if (longitud == 0)
                        {
                            System.Threading.Thread.Sleep(200);
                        }
                        lock (Lock)
                        {
                            mSyncStream.Position = mSyncStreamUltimaPosicionLeida;
                            longitud = mSyncStream.Read(buffer, 0, 12000);
                            streamSaliente.Write(buffer, 0, longitud);
                            mSyncStreamUltimaPosicionLeida += longitud;
                        }                        
                    }
                    streamSaliente.Flush();
                    streamSaliente.Close();
                    response.Close();
                    mMensajeInfo(String.Format("{0} - Servicio Terminado.\n", DateTime.Now));
                  
                    if (listenerHttp.IsListening)
                        listenerHttp.BeginGetContext(new AsyncCallback(ListenerHttpCallback), listenerHttp);
                }
                catch (HttpListenerException e)
                {
                    if ((e.ErrorCode == 1229) || (e.ErrorCode == 64))
                    {
                        mMensajeInfo("Conexión perdida durante el proceso.\n");

                        listenerHttp = (HttpListener)resultAsync.AsyncState;

                        if (listenerHttp.IsListening)
                        {
                            listenerHttp.BeginGetContext(new AsyncCallback(ListenerHttpCallback), listenerHttp);
                        }

                        System.Threading.Thread.Sleep(0);
                        mMensajeInfo(String.Format("{0} - Conexión recuperada.\n", DateTime.Now));
                    }
                    else
                    {
                        mMensajeInfo(String.Format("Error: '{0}.'\n", e));
                    }
                }
                catch (Exception)
                {

                }
            }
        }

        public void RecibirTimeout()
        {
            mStreamEntrante = false;
            if (mMensajeInfo != null)
                mMensajeInfo("Stream Timed out...");
        }

        delegate void StopHandler();
        public void Stop()
        {
          
        }

        delegate void TimerHandler(object state);
        public void TimerCallback(object state)
        {

        }

        public bool Enviando
        {
            get { return mEnvio; }
        }

        public static String StringHttpGuardado(String strEntrante)
        {
            if (null == strEntrante)
                return null;

            String guardado = strEntrante.Trim();
            guardado = guardado.Replace(" ", "").ToLower();
          
            guardado = Regex.Replace(guardado, "[^a-z0-9\\.]", "");
            return guardado;
        }
    }
}
