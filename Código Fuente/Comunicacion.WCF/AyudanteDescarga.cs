using System;
using System.IO;
using System.Threading;

namespace Peer2PeerApp.Comunicacion
{
    public class AyudanteDescarga
    {
        public delegate void ControladorDeEventoOperacionDeContrato(DownloadPacket packet);
        ControladorDeEventoOperacionDeContrato mOperacionDeContrato;

        public ControladorDeEventoOperacionDeContrato OperacionDeContrato
        {
            get { return mOperacionDeContrato; }
            set { mOperacionDeContrato = value; }
        }

        public AyudanteDescarga(ControladorDeEventoOperacionDeContrato op)
        {
            mOperacionDeContrato = op;
        }

        public void BuscarRecurso(DownloadPacket packet)
        {
            mOperacionDeContrato(packet);
        }

        public void DescargarRecurso(DownloadPacket packet)
        {
            mOperacionDeContrato(packet);
        }

        public void BuscarDescargarRecurso(DownloadPacket packet)
        {
            if (null != mDescargaChanged)
            {
                mDescargaChanged(this, new DescargaChangedEventArgs(packet));
            }
        }

        private event EventHandler<DescargaChangedEventArgs> mDescargaChanged;
        public event EventHandler<DescargaChangedEventArgs> DescargaChanged
        {
            add { mDescargaChanged += value; }
            remove { mDescargaChanged -= value; }
        }
    }
}
