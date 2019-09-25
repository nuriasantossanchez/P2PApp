using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Peer2PeerApp.Comunicacion
{
    public class AyudanteFichero
    {
        public delegate void ControladorDeEventoOperacionDeContrato(Packet packet);
        ControladorDeEventoOperacionDeContrato mOperacionDeContrato;

        public ControladorDeEventoOperacionDeContrato OperacionDeContrato
        {
            get { return mOperacionDeContrato; }
            set { mOperacionDeContrato = value; }
        }

        public AyudanteFichero(ControladorDeEventoOperacionDeContrato op)
        {
            mOperacionDeContrato = op;
        }

        public void EnviarFichero(Packet packet)
        {
            mOperacionDeContrato(packet);
        }

        public void EnviarFicheroAParticipante(Packet packet)
        {
            if (mFicheroChanged != null)
            {
                mFicheroChanged(this, new FicheroChangedEventArgs(packet)); 
            }
        }
 
        private event EventHandler<FicheroChangedEventArgs> mFicheroChanged;
        public event EventHandler<FicheroChangedEventArgs> FicheroChanged
        {
            add { mFicheroChanged += value; }
            remove { mFicheroChanged -= value; }
        }
    }
}
