using System;
using System.IO;
using System.Threading;

namespace Peer2PeerApp.Comunicacion
{
    public class AyudanteStream
    {
        EstadoStream estadoStream;    
        Timer mTimer;                    
        Guid mActualGuid;                                                                                  
        String mActualNombreArchivo;
        Stream mActualStream;
        int mPacketNum;               
        bool mStreamParado;                  
        string mNombreNodo;

        public delegate void CompartirStreamHandler(StreamPacket packet);
        CompartirStreamHandler compartirStream;

        public AyudanteStream(IContratoDeServicioCliente participante, string nombreNodo, CompartirStreamHandler stream)
        {
            mNombreNodo = nombreNodo;
            estadoStream = EstadoStream.Inicial;
            mTrozo = 0;
            mUltimoTrozo = -1;
            compartirStream = stream;
      
            mTimer = new Timer(new TimerCallback(Vigilante));
            mTimer.Change(0, 20000);
        }

        public event EventHandler<StreamChangedEventArgs> StreamChanged;

        private event EventHandler mTimeoutRecibidoChanged;
        public event EventHandler TimeoutRecibidoChanged
        {
            add { mTimeoutRecibidoChanged += value; }
            remove { mTimeoutRecibidoChanged -= value; }
        }

        public EstadoStream EstadoStream
        {
            get
            {
                return estadoStream;
            }
        }

        public void CompartirStream(StreamPacket sp)
        {
            switch (estadoStream)
            {
                case EstadoStream.Inicial:
                    estadoStream = EstadoStream.Procesando;
                    mUltimoTrozo = -1;
                    break;

                case EstadoStream.Procesando:
                    mTrozo = sp.numPacket;

                    if (sp.esFinalDeStream)
                    {
                        estadoStream = EstadoStream.Inicial;
                    }
                    break;
            }
            if (null != StreamChanged)
            {
                StreamChanged(this, new StreamChangedEventArgs(sp));
            }
        }

        public void IniciarStream(String nombreArchivo)
        {
            mActualGuid = System.Guid.NewGuid();
            mActualNombreArchivo = nombreArchivo;
            mActualStream = System.IO.File.OpenRead(nombreArchivo);
            mPacketNum = 0;
            mStreamParado = false;
        }
        
        public void CancelarEnvioDeStream()
        {
            mStreamParado = true;
        }

        public bool TrozoStreamEnviado()
        {
            StreamPacket packet = new StreamPacket();
            Byte[] bytes = new Byte[12000]; //12K
            int longitud = 0;
            bool terminado = false;
          
            if (mPacketNum == 0)
            {
                packet.nombreArchivo = System.IO.Path.GetFileName(mActualNombreArchivo);
                MetadatosInfoFichero metaData = new MetadatosInfoFichero();
                metaData.LeerMetadatos(mActualNombreArchivo);
                packet.album = metaData.NombreAlbum;
                packet.artista = metaData.NombreArtista;
                packet.titulo = metaData.Titulo;
            }

            try
            {

                longitud = mActualStream.Read(bytes, 0, 12000);
                packet.stream.Write(bytes, 0, longitud);
                packet.stream.Position = 0;
                packet.guid = mActualGuid;

                if (mActualStream.Length != mActualStream.Position && !mStreamParado)
                {
                    terminado = false;
                    packet.esFinalDeStream = false;
                }
                else
                {
                    terminado = true;
                    packet.esFinalDeStream = true;
                    mActualStream.Close();
                }

                packet.nombreNodoEmisor = mNombreNodo;
                packet.numPacket = mPacketNum++;

                if (compartirStream != null)
                    compartirStream(packet);
            }
            catch (Exception)
            {
                terminado = true;
            }
         
            return terminado;
        }

        int mUltimoTrozo, mTrozo; 
        public void Vigilante(object state)
        {
            if (estadoStream == EstadoStream.Procesando)
            {
                if (mTrozo == mUltimoTrozo)
                {              
                    estadoStream = EstadoStream.Inicial;
                    
                    if (null != mTimeoutRecibidoChanged)
                    {
                        mTimeoutRecibidoChanged(this, new EventArgs());
                    }
                }
                else
                {
                    mUltimoTrozo = mTrozo;
                }
            }
        }

        public static void CopiarStream(Stream streamOrigen, Stream streamDestino)
        {
            const int bufferTam = 4096;//4K
            byte[] buffer = new byte[bufferTam];
            int tam = 0;
            int contBytes = 0;
            int contTrozos = 0;

            try
            {
                while ((tam = streamOrigen.Read(buffer, 0, bufferTam)) > 0)
                {
                    contTrozos++;
                    streamDestino.Write(buffer, 0, tam);
                    contBytes += tam;
                }
            }
            catch
            { 
            
            }
        }
    }
}
