using System;


namespace Peer2PeerApp.Comunicacion
{
    public class EstadoRedChangedEventArgs : EventArgs
    {
        string mienbro;
        bool esNuevoNodoUnidoARed, esNodoQueAbandonaRed;

        public String Miembro
        {
            get { return mienbro; }
        }

        public bool EsNuevoNodoUnidoARed
        {
            get { return esNuevoNodoUnidoARed; }
        }

        public bool EsNodoQueAbandonaRed
        {
            get { return esNodoQueAbandonaRed; }
        }

        internal EstadoRedChangedEventArgs(String mienbro, bool esNuevoNodoUnidoARed, bool esNodoQueAbandonaRed)
        {
            this.mienbro = mienbro;
            this.esNuevoNodoUnidoARed = esNuevoNodoUnidoARed;
            this.esNodoQueAbandonaRed = esNodoQueAbandonaRed;
        }
    }

    public class ChatChangedEventArgs : EventArgs
    {
        private string mensaje;

        public ChatChangedEventArgs(string msj)
        {
            mensaje = msj;
        }
        public string Mensaje
        {
            get { return mensaje; }
            set { mensaje = value; }
        }
    }

    public class FicheroChangedEventArgs : EventArgs
    {
        Peer2PeerApp.Comunicacion.Packet mPacket;

        public Packet Packet
        {
            get
            {
                return mPacket;
            }
        }

        public FicheroChangedEventArgs(Packet packet)
        {
            mPacket = packet;
        }
    }

    public class DescargaChangedEventArgs : EventArgs
    {
        Peer2PeerApp.Comunicacion.DownloadPacket mPacket;

        public DownloadPacket Packet
        {
            get
            {
                return mPacket;
            }
        }
        public DescargaChangedEventArgs(DownloadPacket packet)
        {
            mPacket = packet;
        }
    }
   
    public class ImagenChangedEventArgs : EventArgs
    {
        ImagenPacket mTrozoPacket;

        public ImagenPacket Packet
        {
            get
            {
                return mTrozoPacket;
            }
        }

        public ImagenChangedEventArgs(ImagenPacket packet)
        {
            mTrozoPacket = packet;
        }
    }

    public class StreamChangedEventArgs : EventArgs
    {
        private StreamPacket mStreamPacket;

        public StreamPacket StreamPacket
        {
            get { return mStreamPacket; }
            set { mStreamPacket = value; }
        }

        internal StreamChangedEventArgs(StreamPacket packet)
        {
            this.mStreamPacket = packet;
        }
    }

    public class EnviarStreamEventArgs : EventArgs
    {
        private string mNombreArchivo;

        public string NombreArchivo
        {
            get
            {
                return mNombreArchivo;
            }
        }

        public EnviarStreamEventArgs(string NombreArchivo)
        {
            mNombreArchivo = NombreArchivo;
        }
    }
}
