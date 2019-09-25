using System;

namespace Peer2PeerApp.Comunicacion
{
    public class AyudanteChat
    {
        public delegate void ControladorDeEventoOperacionDeContrato(string miembro, string mensaje);
        ControladorDeEventoOperacionDeContrato mOperacionDeContrato;

        public AyudanteChat(ControladorDeEventoOperacionDeContrato op)
        {
            mOperacionDeContrato = op;
        }

        public void EnviarMensajeTexto(string nombreNodo, string mensaje)
        {
            mOperacionDeContrato(nombreNodo, mensaje);
        }

        public void EnviarMensajeAParticipante(string miembro, string mensaje)
        {
            if (null != mChatChanged)
            {
                mChatChanged(this, new ChatChangedEventArgs(String.Format("{0} dice: {1}", miembro, mensaje)));
            }
        }

        private event EventHandler<ChatChangedEventArgs> mChatChanged;
        public event EventHandler<ChatChangedEventArgs> ChatChanged
        {
            add { mChatChanged += value; }
            remove { mChatChanged -= value; }
        }
    }

}
