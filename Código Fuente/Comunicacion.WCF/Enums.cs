using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Peer2PeerApp.Comunicacion
{
    public enum EstadoRed
    {
        Desconectado,
        UsuarioFallido,
        Conectando,
        Conectado,
    }

    public enum EstadoStream
    {
        Inicial,
        Procesando,
    }

    public enum InfoFichero
    {
        Nombre = 0,
        Artista = 13,
        Album = 14,
        Titulo = 21,
    }
}
