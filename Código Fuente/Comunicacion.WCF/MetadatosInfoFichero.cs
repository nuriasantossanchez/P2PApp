using System;
using System.IO;

namespace Peer2PeerApp.Comunicacion
{
    public class MetadatosInfoFichero
    {
        private string mNombreFichero;
        private string mNombreArtista;
        private string mNombreAlbum;
        private string mTitulo;

        public string NombreFichero
        {
            get { return mNombreFichero; }
            set { mNombreFichero = value; }
        }
        public string Titulo
        {
            get { return mTitulo; }
            set { mTitulo = value; }
        }
        public string NombreAlbum
        {
            get { return mNombreAlbum; }
            set { mNombreAlbum = value; }
        }
        public string NombreArtista
        {
            get { return mNombreArtista; }
            set { mNombreArtista = value; }
        }
       
        public void LeerMetadatos(String pathFichero)
        {
            Shell32.Shell shell = new Shell32.Shell();
            Shell32.Folder folder = shell.NameSpace(Path.GetDirectoryName(pathFichero));
            Shell32.FolderItem folderItem = folder.ParseName(Path.GetFileName(pathFichero));

            if (folderItem != null)
            {
                NombreFichero = Path.GetFileName(pathFichero);
                NombreAlbum = folder.GetDetailsOf(folderItem, (int)InfoFichero.Album);
                NombreArtista = folder.GetDetailsOf(folderItem, (int)InfoFichero.Artista);
                Titulo = folder.GetDetailsOf(folderItem, (int)InfoFichero.Titulo);
            }
            folderItem = null;
            folder = null;
            shell = null;
        }
    }
}
