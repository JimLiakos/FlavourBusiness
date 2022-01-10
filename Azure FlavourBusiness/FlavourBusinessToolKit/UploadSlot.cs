using FlavourBusinessFacade;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlavourBusinessToolKit
{


    /// <MetaDataID>{5af1762e-745c-49d9-9ffe-1558bf4679fc}</MetaDataID>
    public class UploadSlot : MarshalByRefObject, OOAdvantech.Remoting.IExtMarshalByRefObject, IUploadSlot
    {
        public Object Tag;
        Microsoft.Azure.Storage.CloudStorageAccount CloudStorageAccount;
        string BlobUrl;
        string RootContainer;
        public UploadSlot(string blobUrl, CloudStorageAccount cloudStorageAccount, string rootContainer)
        {
            BlobUrl = blobUrl;
            CloudStorageAccount = cloudStorageAccount;
            RootContainer = rootContainer;

        }
        public event EventHandler FileUploaded;

        static Dictionary<string, FileUploadEntry> FileUploads = new Dictionary<string, FileUploadEntry>();


        public string CreateFileUploadSession(int size, string contentType)
        {
            try
            {
                FileUploadEntry fileUploadEntry = new FileUploadEntry();
                fileUploadEntry.FileName = BlobUrl;// userFolder + "/" + path;
                fileUploadEntry.FileSize = size;
                fileUploadEntry.TimeStamp = DateTime.UtcNow;
                fileUploadEntry.ContentType = contentType;
                fileUploadEntry.FileUploadIdentity = Guid.NewGuid().ToString("N");


                FileUploads[fileUploadEntry.FileUploadIdentity] = fileUploadEntry;
                return fileUploadEntry.FileUploadIdentity;
            }
            catch (Microsoft.Azure.Storage.StorageException error)
            {

                throw;
            }
        }

        /// <summary>
        /// This function is used to append chunk of bytes to a file name. 
        /// If the offset starts from 0 means file name should be created.
        /// </summary>
        /// <param name="FileName">File Name</param>
        /// <param name="dataChunk">Buffer array</param>
        /// <param name="Offset">Offset</param>
        /// <returns>boolean: true means append is successfully</returns>
        //  [WebMethod]
        public bool UploadFile(string fileUploadIdentity, string dataChunk)
        {
            bool retVal = false;
            try
            {
                FileUploadEntry fileUploadEntry = FileUploads[fileUploadIdentity];
                fileUploadEntry.jsonData += dataChunk;

                if (fileUploadEntry.jsonData.Length == fileUploadEntry.FileSize)
                {
                    var stream = Decompress(System.Convert.FromBase64String(fileUploadEntry.jsonData));
                    stream.Position = 0;
                    if (fileUploadEntry.AppendBlob == null)
                    {

                        string blobUrl = fileUploadEntry.FileName;//.Substring(fileUploadEntry.FileName.IndexOf("/") + 1);
                        string containerName = RootContainer;
                        if (string.IsNullOrWhiteSpace(containerName))
                        {
                            containerName = fileUploadEntry.FileName.Substring(0, fileUploadEntry.FileName.IndexOf("/"));
                            blobUrl = fileUploadEntry.FileName.Substring(fileUploadEntry.FileName.IndexOf("/") + 1);
                        }
                        var blobClient = CloudStorageAccount.CreateCloudBlobClient();
                        var container = blobClient.GetContainerReference(containerName);
                        container.CreateIfNotExists();
                        CloudBlockBlob blob = container.GetBlockBlobReference(blobUrl);
                        blob.Properties.ContentType = fileUploadEntry.ContentType;
                        blob.DeleteIfExists();
                        blob.UploadFromStream(stream);
                    }
                }
                retVal = true;
            }

            catch (Exception ex)
            {
                //sending error to an email id
                //common.SendError(ex);
            }
            return retVal;
        }




        public void EndOfUploadFile(string fileUploadIdentity)
        {
            FileUploaded?.Invoke(this, EventArgs.Empty);
        }

        internal static byte[] Compress(Stream input)
        {
            using (var compressStream = new MemoryStream())
            using (var compressor = new DeflateStream(compressStream, CompressionMode.Compress))
            {
                input.CopyTo(compressor);
                return compressStream.ToArray();
            }
        }
        internal static Stream Decompress(byte[] input)
        {
            var output = new MemoryStream();

            using (var compressStream = new MemoryStream(input))
            using (var decompressor = new DeflateStream(compressStream, CompressionMode.Decompress))
                decompressor.CopyTo(output);

            output.Position = 0;
            return output;
        }
    }

}
