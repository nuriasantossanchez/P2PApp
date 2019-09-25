using System;
using System.IO;

namespace Peer2PeerApp.Comunicacion
{
    public class AyudanteImagen
    {
        public static readonly int MAX_IMAGE_SIZE = 70000000;
       
        public delegate void ControladorDeEventoOperacionDeContrato(ImagenPacket packet);
        ControladorDeEventoOperacionDeContrato mOperacionDeContrato;

        public ControladorDeEventoOperacionDeContrato OperacionDeContrato
        {
            get { return mOperacionDeContrato; }
            set { mOperacionDeContrato = value; }
        }

        String mNombreNodo;

        public AyudanteImagen(ControladorDeEventoOperacionDeContrato op, string nombreNodo)
        {
            mNombreNodo = nombreNodo;
            mOperacionDeContrato = op;
        }    

        public void EnviarImagen(ImagenPacket packet)
        {
            if (packet != null)
            {
                mOperacionDeContrato(packet);
            }
        }

        public void EnviarImagenAParticipante(ImagenPacket packet)
        {
            if (mImagenChanged != null)
            {
                mImagenChanged(this, new ImagenChangedEventArgs(packet));
            }
        }

        private event EventHandler<ImagenChangedEventArgs> mImagenChanged;
        public event EventHandler<ImagenChangedEventArgs> ImagenChanged
        {
            add { mImagenChanged += value; }
            remove { mImagenChanged -= value; }
        }

    }
}
