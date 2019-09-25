using System;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Peer2PeerApp.Comunicacion
{
    [ServiceContract(CallbackContract = typeof(IContratoDeServicio))]
    public interface IContratoDeServicio
    {
        [OperationContract(IsOneWay = true)]
        void ServicioUnirseARed(string mienbro);

        [OperationContract(IsOneWay = true)]
        void ServicioAbandonarRed(string mienbro);

        [OperationContract(IsOneWay = true)]
        void ServicioGestionarChat(string mienbro, string mensaje);
       
        [OperationContract(IsOneWay = true)]
        void ServicioGestionarImagen(ImagenPacket packet);

        [OperationContract(IsOneWay = true)]
        void ServicioGestionarAudio(StreamPacket packet);
       
        [OperationContract(IsOneWay = true)]
        void ServicioGestionarVideo(StreamPacket packet);
      
        [OperationContract(IsOneWay = true)]
        void ServicioGestionarDescarga(DownloadPacket packet);

        [OperationContract(IsOneWay = true)]
        void ServicioGestionarFichero(Packet packet);
    }

   
    [MessageContract]
    [Serializable]
    public class Packet : IDisposable
    {
        [MessageHeader]
        public String nombreNodoEmisor;
       
        [MessageHeader]
        public int numPacket;

        [MessageHeader]
        public bool esFinalDeStream;

        [MessageHeader]
        public Guid guid;
       
        [MessageHeader]
        public String nombreArchivo;
       
        [MessageBodyMember]
        public Stream stream;

        public Packet()
        {
            stream = new MemoryStream();
        }

        public void Dispose()
        {
            if (stream != null)
            {
                stream.Dispose();
            }
        }
    }
   
    [MessageContract]
    [Serializable]
    public class ImagenPacket
    {
        [MessageBodyMember]
        public Stream stream;

        [MessageHeader]
        public String nombreArchivo;

        [MessageHeader]
        public String titulo;

        [MessageHeader]
        public String nombreNodoEmisor;
    }
   
    [MessageContract]
    [Serializable]
    public class StreamPacket : Packet
    {
        [MessageHeader]
        public String titulo; 

        [MessageHeader]
        public String album;

        [MessageHeader]
        public String artista;
    }

    [MessageContract]
    [Serializable]
    public class DownloadPacket : Packet
    {
        [MessageHeader]
        public String comando;

        [MessageHeader]
        public String nombreNodoDestino;

        [MessageHeader]
        public long tamanio;
    }
   
    public interface IContratoDeServicioCliente : IContratoDeServicio, IClientChannel
    {
    }

}
