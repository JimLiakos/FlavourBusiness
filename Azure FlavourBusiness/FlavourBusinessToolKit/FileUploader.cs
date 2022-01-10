using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlavourBusinessFacade;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;

namespace FlavourBusinessToolKit
{

    /// <MetaDataID>{b032723b-81ca-4473-bb26-beafcdb5ec60}</MetaDataID>
    public interface IFileManager
    {
        string RootUri { get; }

        void Upload(string serverStoragePath, string fileName, string contentType = "");
        void Copy(string aboluteImageUri, string newImageUri);

        void Upload(string filePath, MemoryStream memoryStream, string contentType = "");

        bool Exist(string filePath);

        FileInfo GetBlobInfo(string filePath);

        List<FileInfo> ListBlobs(string serverStorageFolder);
    }

    /// <MetaDataID>{c3074225-faa3-46ff-80f2-11feedefbec1}</MetaDataID>
    public abstract class FileManager
    {

        static IFileManager _CurrentFileManager;
        public static IFileManager CurrentFileManager
        {
            get
            {
                if (_CurrentFileManager == null)
                {
                    //_CurrentFileManager = new BlobFileManager();
                    _CurrentFileManager = new NativeFileManager();
                }
                return _CurrentFileManager;
            }
        }
    }

    /// <MetaDataID>{eba2f16b-f08a-40d5-b8a0-b53ae4aa2540}</MetaDataID>
    public abstract class FileInfo
    {
        public abstract String Uri { get; }

        public abstract DateTime? LastModifiedUtc { get; }

        public abstract void DeleteIfExists();
    }

    /// <MetaDataID>{c544a747-2012-4f8d-98ee-21c91666f568}</MetaDataID>
    internal class BlobInfo : FileInfo
    {


        CloudBlob CloudBlob;
        public BlobInfo(CloudBlob cloudBlob)
        {
            CloudBlob = cloudBlob;
        }
        public override void DeleteIfExists()
        {
            if (CloudBlob != null)
                CloudBlob.DeleteIfExists();
        }

        public override DateTime? LastModifiedUtc
        {
            get
            {
                if (CloudBlob == null)
                    return default(DateTime?);

                if (CloudBlob.Properties.LastModified.HasValue)
                    return CloudBlob.Properties.LastModified.Value.UtcDateTime;
                else return default(DateTime?);
            }
        }

        public override string Uri
        {
            get
            {
                if (CloudBlob == null)
                    return "";
                return CloudBlob.Uri.AbsoluteUri;
            }
        }
    }


    /// <MetaDataID>{517e6854-7b5b-43fb-aa0a-31dbf8d66bbc}</MetaDataID>
    internal class NativeFileInfo : FileInfo
    {
        System.IO.FileInfo CloudBlob;
        public NativeFileInfo(System.IO.FileInfo cloudBlob)
        {
            CloudBlob = cloudBlob;
        }
        public override void DeleteIfExists()
        {
            if (CloudBlob.Exists)
                CloudBlob.Delete();
        }

        public override DateTime? LastModifiedUtc
        {
            get
            {
                if (CloudBlob.Exists)
                    return CloudBlob.LastWriteTimeUtc;
                else
                    return default(DateTime?);
                //if (CloudBlob.LastWriteTimeUtc.Properties.LastModified.HasValue)
                //    return CloudBlob.Properties.LastModified.Value.UtcDateTime;
                //else return default(DateTime?);
            }
        }

        public override string Uri
        {
            get
            {
                return "file:///" + CloudBlob.FullName.Replace(@"\", "/");
            }
        }
    }
    /// <MetaDataID>{868cd7de-47be-46bf-9c15-113b1b81ec5e}</MetaDataID>
    class FileUploadEntry
    {
        public string FileUploadIdentity;
        public string RelativePath;
        public string FileName;
        public int FileSize;
        public int Offset;
        public string ContentType;
        public MemoryStream Stream;
        public string jsonData;
        public CloudAppendBlob AppendBlob;
        public System.DateTime TimeStamp;
    }

    /// <MetaDataID>{9e226fb7-ec24-4583-863f-c6b88cb67320}</MetaDataID>
    public class BlobFileManager : FileManager, IFileManager
    {
        string Container;
        public BlobFileManager(CloudStorageAccount cloudStorageAccount, string container)
        {
            CloudStorageAccount = cloudStorageAccount;
            Container = container;

        }
        //public static string AzureStorageBaseUrl
        //{
        //    get
        //    {
        //        CloudStorageAccount.
        //    }
        //}
        const int kB = 1024;
        const int MB = kB * 1024;
        const long GB = MB * 1024;
        public static long NumBytesPerChunk = 4 * MB; // A block may be up to 4 MB in size. 



        CloudStorageAccount CloudStorageAccount;
        //CloudStorageAccount.DevelopmentStorageAccount



        public string RootUri
        {
            get
            {
                return CloudStorageAccount.BlobStorageUri.PrimaryUri.AbsoluteUri;
            }
        }

        static Dictionary<string, FileUploadEntry> FileUploads = new Dictionary<string, FileUploadEntry>();

        string IFileManager.RootUri
        {
            get
            {
                return RootUri;
            }
        }

        public string CreateFileUploadSession(string path, int size, string contentType)
        {

            try
            {
                string userFolder = "27B45100-AF26-4E33-8846-4FA08FA6A586";
                path = path.Replace(@"\", "/");
                FileUploadEntry fileUploadEntry = new FileUploadEntry();
                fileUploadEntry.RelativePath = path;
                fileUploadEntry.FileName = userFolder + "/" + path;
                fileUploadEntry.FileSize = size;
                fileUploadEntry.TimeStamp = DateTime.UtcNow;
                fileUploadEntry.Stream = new MemoryStream();
                fileUploadEntry.ContentType = contentType;
                fileUploadEntry.FileUploadIdentity = Guid.NewGuid().ToString().Replace("-", "");

                //CloudStorageAccount = new CloudStorageAccount(new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials("asfameazure", @"pJL6v5+z9tRTOxpg/tzuh71j19s/16rKMiPSlTyLmJdqkIrdms/EV5ZO/ptz8ZCQYNaOC7Kba+gtQl8X1qVZ7g=="), true);
                CloudStorageAccount = CloudStorageAccount.DevelopmentStorageAccount;
                FileUploads[fileUploadEntry.FileUploadIdentity] = fileUploadEntry;
                return fileUploadEntry.FileUploadIdentity;
            }
            catch (Microsoft.Azure.Storage.StorageException error)
            {

                throw;
            }
        }


        //public void Upload(string credentialKey, string filePath, MemoryStream memoryStream, string contentType = "")
        //{

        //    string serverStorageFileName = filePath;
        //    //the file that we want to upload
        //    //string fileName = @"C:\ProgramData\Microneme\DontWaitWater\PageBackgrounds\Woodsy.jpg";
        //    int Offset = 0; // starting offset.

        //    //define the chunk size
        //    int ChunkSize = 65536; // 64 * 1024 kb

        //    //define the buffer array according to the chunksize.
        //    byte[] Buffer = new byte[ChunkSize];
        //    //opening the file for read.
        //    Stream fs = memoryStream;



        //    //creating the ServiceSoapClient which will allow to connect to the service.
        //    FlavourBusinessToolKit.BlobFileManager fileUploader = new FlavourBusinessToolKit.BlobFileManager(CloudStorageAccount.DevelopmentStorageAccount);
        //    string fileUploadIdentity = fileUploader.CreateFileUploadSession(credentialKey, serverStorageFileName, (int)fs.Length, contentType);

        //    try
        //    {
        //        long FileSize = memoryStream.Length;// File size of file being uploaded.
        //                                            // reading the file.
        //        fs.Position = Offset;
        //        int BytesRead = 0;
        //        while (Offset != FileSize) // continue uploading the file chunks until offset = file size.
        //        {
        //            BytesRead = fs.Read(Buffer, 0, ChunkSize); // read the next chunk 
        //                                                       // (if it exists) into the buffer. 
        //                                                       // the while loop will terminate if there is nothing left to read
        //                                                       // check if this is the last chunk and resize the buffer as needed 
        //                                                       // to avoid sending a mostly empty buffer 
        //                                                       // (could be 10Mb of 000000000000s in a large chunk)
        //            if (BytesRead != Buffer.Length)
        //            {
        //                ChunkSize = BytesRead;
        //                byte[] TrimmedBuffer = new byte[BytesRead];
        //                Array.Copy(Buffer, TrimmedBuffer, BytesRead);
        //                Buffer = TrimmedBuffer; // the trimmed buffer should become the new 'buffer'
        //            }
        //            // send this chunk to the server. it is sent as a byte[] parameter, 
        //            // but the client and server have been configured to encode byte[] using MTOM. 
        //            bool ChunkAppened = fileUploader.UploadFile(fileUploadIdentity, Buffer, Offset);
        //            if (!ChunkAppened)
        //            {
        //                break;
        //            }

        //            // Offset is only updated AFTER a successful send of the bytes. 
        //            Offset += BytesRead; // save the offset position for resume
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    finally
        //    {
        //        fs.Close();
        //    }

        //}

        /// <summary>
        /// This function is used to append chunk of bytes to a file name. 
        /// If the offset starts from 0 means file name should be created.
        /// </summary>
        /// <param name="FileName">File Name</param>
        /// <param name="buffer">Buffer array</param>
        /// <param name="Offset">Offset</param>
        /// <returns>boolean: true means append is successfully</returns>
        //  [WebMethod]
        public bool UploadFile(string fileUploadIdentity, byte[] buffer, long Offset)
        {
            bool retVal = false;
            try
            {
                FileUploadEntry fileUploadEntry = FileUploads[fileUploadIdentity];
                long availableNumBytes = NumBytesPerChunk - fileUploadEntry.Stream.Length;
                if (availableNumBytes > buffer.Length)
                    fileUploadEntry.Stream.Write(buffer, 0, buffer.Length);
                else
                {
                    int bufferOffset = 0;
                    if (fileUploadEntry.AppendBlob == null)
                    {
                        string containerName = Container;
                        if (string.IsNullOrWhiteSpace(containerName))
                            containerName = "usersfolder";
                        var blobClient = CloudStorageAccount.CreateCloudBlobClient();
                        var container = blobClient.GetContainerReference(containerName);
                        container.CreateIfNotExists();
                        CloudAppendBlob blob = container.GetAppendBlobReference(fileUploadEntry.FileName);
                        blob.Properties.ContentType = fileUploadEntry.ContentType;
                        blob.DeleteIfExists();
                        blob.CreateOrReplace(AccessCondition.GenerateIfNotExistsCondition(), null, null);
                        fileUploadEntry.AppendBlob = blob;
                    }
                    int remainingBytes = 0;
                    do
                    {
                        fileUploadEntry.Stream.Write(buffer, bufferOffset, (int)availableNumBytes);
                        fileUploadEntry.Stream.Position = 0;
                        fileUploadEntry.AppendBlob.AppendBlock(fileUploadEntry.Stream);//Chunk buffer completed


                        bufferOffset += (int)availableNumBytes;
                        fileUploadEntry.Stream.SetLength(0); //evacuates Chunk buffer 
                        availableNumBytes = NumBytesPerChunk; //  Set availableNumBytes max
                        if (buffer.Length - bufferOffset < availableNumBytes)
                        {
                            //remaining bytes fits on chunk buffer
                            fileUploadEntry.Stream.Write(buffer, bufferOffset, buffer.Length - bufferOffset);
                            remainingBytes = 0;
                        }
                        else
                        {
                            remainingBytes = buffer.Length - bufferOffset;

                            //go to append Chunk buffer to blob and refill with remaining bytes
                        }
                    }
                    while (remainingBytes > 0);
                }


                if (buffer.Length + Offset == fileUploadEntry.FileSize)
                {
                    // end of file upload
                    if (fileUploadEntry.AppendBlob != null)
                    {
                        fileUploadEntry.Stream.Position = 0;
                        fileUploadEntry.AppendBlob.AppendBlock(fileUploadEntry.Stream);
                        // append last Chunk buffer; 
                    }
                    else
                    {
                        // file size is less than Chunk size
                        string containerName = Container;
                        if (string.IsNullOrWhiteSpace(containerName))
                            containerName = "usersfolder";

                        var blobClient = CloudStorageAccount.CreateCloudBlobClient();
                        var container = blobClient.GetContainerReference(containerName);
                        container.CreateIfNotExists();

                        var blob = container.GetBlockBlobReference(fileUploadEntry.FileName);
                        blob.DeleteIfExists();
                        blob.Properties.ContentType = fileUploadEntry.ContentType;
                        fileUploadEntry.Stream.Position = 0;
                        blob.UploadFromStream(fileUploadEntry.Stream);
                    }
                    int save = 0;
                    save++;
                }
                retVal = true;
            }		//Message	"Unable to cast object of type 'SharpVectors.Net.ExtendedHttpWebRequest' to type 'System.Net.HttpWebRequest'."	string

            catch (Exception ex)
            {
                //sending error to an email id
                //common.SendError(ex);
            }
            return retVal;
        }


        public void Upload(string serverStoragePath, string fileName, string contentType = "")
        {

            if (System.IO.File.Exists(fileName))
            {
                System.IO.FileInfo fInfo = new System.IO.FileInfo(fileName);
                string serverStorageFileName = serverStoragePath + "/" + fInfo.Name;
                //the file that we want to upload
                //string fileName = @"C:\ProgramData\Microneme\DontWaitWater\PageBackgrounds\Woodsy.jpg";
                int Offset = 0; // starting offset.

                //define the chunk size
                int ChunkSize = 65536; // 64 * 1024 kb

                //define the buffer array according to the chunksize.
                byte[] Buffer = new byte[ChunkSize];
                //opening the file for read.
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                //creating the ServiceSoapClient which will allow to connect to the service.
                FlavourBusinessToolKit.BlobFileManager fileUploader = new FlavourBusinessToolKit.BlobFileManager(CloudStorageAccount.DevelopmentStorageAccount, Container);
                string fileUploadIdentity = fileUploader.CreateFileUploadSession(serverStorageFileName, (int)fs.Length, contentType);


                try
                {
                    long FileSize = new System.IO.FileInfo(fileName).Length; // File size of file being uploaded.
                                                                             // reading the file.
                    fs.Position = Offset;
                    int BytesRead = 0;
                    while (Offset != FileSize) // continue uploading the file chunks until offset = file size.
                    {
                        BytesRead = fs.Read(Buffer, 0, ChunkSize); // read the next chunk 
                                                                   // (if it exists) into the buffer. 
                                                                   // the while loop will terminate if there is nothing left to read
                                                                   // check if this is the last chunk and resize the buffer as needed 
                                                                   // to avoid sending a mostly empty buffer 
                                                                   // (could be 10Mb of 000000000000s in a large chunk)
                        if (BytesRead != Buffer.Length)
                        {
                            ChunkSize = BytesRead;
                            byte[] TrimmedBuffer = new byte[BytesRead];
                            Array.Copy(Buffer, TrimmedBuffer, BytesRead);
                            Buffer = TrimmedBuffer; // the trimmed buffer should become the new 'buffer'
                        }
                        // send this chunk to the server. it is sent as a byte[] parameter, 
                        // but the client and server have been configured to encode byte[] using MTOM. 
                        bool ChunkAppened = fileUploader.UploadFile(fileUploadIdentity, Buffer, Offset);
                        if (!ChunkAppened)
                        {
                            break;
                        }

                        // Offset is only updated AFTER a successful send of the bytes. 
                        Offset += BytesRead; // save the offset position for resume
                    }
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    fs.Close();
                }
            }
        }

        //void IFileManager.Upload(string credentialKey, string serverStoragePath, string fileName, string contentType)
        //{
        //    throw new NotImplementedException();
        //}

        void IFileManager.Copy(string sourceUri, string targetUri)
        {

            string root = RootUri.ToLower() + "/";
            var blobClient = CloudStorageAccount.CreateCloudBlobClient();

            if (sourceUri.ToLower().IndexOf(root) == 0)
                sourceUri = sourceUri.Substring(root.Length);

            string sourceContainerName = sourceUri.Substring(0, sourceUri.IndexOf("/"));
            string sourceBlobUrl = sourceUri.Substring(sourceUri.IndexOf("/") + 1);
            var sourceContainer = blobClient.GetContainerReference(sourceContainerName);

            var sourceBlob = sourceContainer.GetBlockBlobReference(sourceBlobUrl);

            if (targetUri.ToLower().IndexOf(root) == 0)
                targetUri = targetUri.Substring(root.Length);

            string targetContainerName = targetUri.Substring(0, targetUri.IndexOf("/"));
            string targetBlobUrl = targetUri.Substring(targetUri.IndexOf("/") + 1);
            var targetContainer = blobClient.GetContainerReference(targetContainerName);

            var targetBlob = targetContainer.GetBlockBlobReference(targetBlobUrl);

            var task = targetBlob.StartCopyAsync(sourceBlob);
            task.Wait();

            //var newBlob = container.GetBlockBlobReference(blobUrl);
            ////create a new blob
            //var task = newBlob.StartCopyAsync(existBlob);
            //task.Wait();

        }
        FileInfo IFileManager.GetBlobInfo(string blobUrl)
        {

            var blobClient = CloudStorageAccount.CreateCloudBlobClient();
            string containerName = blobUrl.Substring(0, blobUrl.IndexOf("/"));
            blobUrl = blobUrl.Substring(blobUrl.IndexOf("/") + 1);
            var container = blobClient.GetContainerReference(containerName);
            if (container.Exists())
            {
                CloudBlockBlob blob = container.GetBlockBlobReference(blobUrl);
                return new BlobInfo(blob);
            }
            else
                return new BlobInfo(null);

        }

        bool IFileManager.Exist(string blobUrl)
        {
            var blobClient = CloudStorageAccount.CreateCloudBlobClient();
            string containerName = blobUrl.Substring(0, blobUrl.IndexOf("/"));
            blobUrl = blobUrl.Substring(blobUrl.IndexOf("/") + 1);
            var container = blobClient.GetContainerReference(containerName);
            if (container.Exists())
            {
                CloudBlockBlob blob = container.GetBlockBlobReference(blobUrl);
                return blob.Exists();
            }
            else
                return false;

        }
        void IFileManager.Upload(string blobUrl, MemoryStream memoryStream, string contentType)
        {
            var blobClient = CloudStorageAccount.CreateCloudBlobClient();
            CloudBlobContainer cloudBlobContainer;
            if (string.IsNullOrWhiteSpace(Container))
            {
                string containerName = blobUrl.Substring(0, blobUrl.IndexOf("/"));

                blobUrl = blobUrl.Substring(blobUrl.IndexOf("/") + 1);

                cloudBlobContainer = blobClient.GetContainerReference(containerName);
            }
            else
                cloudBlobContainer = blobClient.GetContainerReference(Container);



            CloudBlockBlob blob = cloudBlobContainer.GetBlockBlobReference(blobUrl);
            blob.Properties.ContentType = contentType;
            blob.DeleteIfExists();
            blob.UploadFromStream(memoryStream);
        }

        List<FileInfo> IFileManager.ListBlobs(string serverStorageFolder)
        {

            var blobClient = CloudStorageAccount.CreateCloudBlobClient();
            string containerName = serverStorageFolder.Substring(0, serverStorageFolder.IndexOf("/"));
            serverStorageFolder = serverStorageFolder.Substring(serverStorageFolder.IndexOf("/") + 1);
            var container = blobClient.GetContainerReference(containerName);
            var folder = container.ListBlobs(serverStorageFolder).OfType<CloudBlobDirectory>().FirstOrDefault();

            if (folder != null)
                return (from cloudBlob in folder.ListBlobs().OfType<CloudBlob>() select new BlobInfo(cloudBlob)).OfType<FileInfo>().ToList();
            return new List<FileInfo>();



        }
    }




    /// <MetaDataID>{9de363b7-43cc-4002-820a-cf553e618e15}</MetaDataID>
    public class RestApiBlobFileManager
    {
        const int kB = 1024;
        const int MB = kB * 1024;
        const long GB = MB * 1024;
        public static long NumBytesPerChunk = 40 * kB; // A block may be up to 40 KB in size. 

        public static void Upload(MemoryStream memoryStream, IUploadSlot uploadSlot, string contentType = "")
        {

            //define the chunk size
            int ChunkSize = (int)NumBytesPerChunk;
            memoryStream.Position = 0;
            string base64 = System.Convert.ToBase64String(Compress(memoryStream));

            //creating the ServiceSoapClient which will allow to connect to the service.
            string fileUploadIdentity = uploadSlot.CreateFileUploadSession((int)base64.Length, contentType);

            bool chunkAppened = false;
            while (base64.Length > ChunkSize)
            {
                string chunk = base64.Substring(0, ChunkSize);
                chunkAppened = uploadSlot.UploadFile(fileUploadIdentity, chunk);
                base64 = base64.Substring(chunk.Length);
                if (!chunkAppened)
                {
                    break;
                }
            }
            if (base64.Length > 0)
                chunkAppened = uploadSlot.UploadFile(fileUploadIdentity, base64);

            uploadSlot.EndOfUploadFile(fileUploadIdentity);

        }






        internal static byte[] Compress(Stream input)
        {

            using (var compressStream = new MemoryStream())
            using (var compressor = new DeflateStream(compressStream, CompressionMode.Compress))
            {
                input.Position = 0;
                input.CopyTo(compressor);
                compressor.Close();
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




    /// <MetaDataID>{d12739dd-7463-458b-90eb-daf49b2a31f1}</MetaDataID>
    public class NativeFileManager : FileManager, IFileManager
    {
        public string RootUri
        {
            get
            {
                return "file:///C:/ProgramData/Microneme/DontWaitWater/wwwroot/";
            }
        }

        public void Copy(string aboluteImageUri, string newImageUri)
        {

            string userFolder = "27B45100-AF26-4E33-8846-4FA08FA6A586";

            string targetUri = RootUri + "usersfolder/" + userFolder + "/" + newImageUri;
            string targetFileName = targetUri.Replace("file:///", "").Replace("/", @"\");
            string sourceFileName = aboluteImageUri.Replace("file:///", "").Replace("/", @"\");
            MemoryStream memoryStream = new MemoryStream();
            using (FileStream file = new FileStream(sourceFileName, FileMode.Open, FileAccess.Read))
                file.CopyTo(memoryStream);
            memoryStream.Position = 0;

            Directory.CreateDirectory(new System.IO.FileInfo(targetFileName).Directory.FullName);
            if (System.IO.File.Exists(targetFileName))
                System.IO.File.Delete(targetFileName);
            memoryStream.Position = 0;
            using (FileStream file = new FileStream(targetFileName, FileMode.Create, System.IO.FileAccess.Write))
            {
                memoryStream.CopyTo(file);
            }



        }

        public bool Exist(string filePath)
        {
            string userFolder = "27B45100-AF26-4E33-8846-4FA08FA6A586";
            string targetUri = RootUri + "usersfolder/" + userFolder + "/" + filePath;
            string targetFileName = targetUri.Replace("file:///", "").Replace("/", @"\");
            return System.IO.File.Exists(targetFileName);
        }

        public List<FileInfo> ListBlobs(string serverStorageFolder)
        {
            string userFolder = "27B45100-AF26-4E33-8846-4FA08FA6A586";
            string targetUri = RootUri + "usersfolder/" + userFolder + "/" + serverStorageFolder;
            DirectoryInfo dir = new DirectoryInfo(targetUri.Replace("file:///", "").Replace("/", @"\"));
            if (dir.Exists)
                return (from fInfo in dir.GetFiles() select new NativeFileInfo(fInfo)).OfType<FileInfo>().ToList();
            return new List<FileInfo>();
        }

        public FileInfo GetBlobInfo(string filePath)
        {
            string userFolder = "27B45100-AF26-4E33-8846-4FA08FA6A586";
            string targetUri = RootUri + "usersfolder/" + userFolder + "/" + filePath;
            return new NativeFileInfo(new System.IO.FileInfo(targetUri));
        }


        public void Upload(string filePath, MemoryStream memoryStream, string contentType = "")
        {
            string userFolder = "27B45100-AF26-4E33-8846-4FA08FA6A586";
            string targetUri = RootUri + "usersfolder/" + userFolder + "/" + filePath;
            string targetFileName = targetUri.Replace("file:///", "").Replace("/", @"\");
            if (System.IO.File.Exists(targetFileName))
                System.IO.File.Delete(targetFileName);
            memoryStream.Position = 0;
            using (FileStream file = new FileStream(targetFileName, FileMode.Create, System.IO.FileAccess.Write))
            {
                memoryStream.CopyTo(file);
            }
        }

        public void Upload(string serverStoragePath, string fileName, string contentType = "")
        {
            if (System.IO.File.Exists(fileName))
            {
                System.IO.FileInfo fInfo = new System.IO.FileInfo(fileName);

                string userFolder = "27B45100-AF26-4E33-8846-4FA08FA6A586";

                string targetUri = RootUri + "usersfolder/" + userFolder + "/" + serverStoragePath + "/" + fInfo.Name;
                string targetFileName = targetUri.Replace("file:///", "").Replace("/", @"\");
                Directory.CreateDirectory(new System.IO.FileInfo(targetFileName).Directory.FullName);

                if (System.IO.File.Exists(targetFileName))
                    System.IO.File.Delete(targetFileName);

                MemoryStream memoryStream = new MemoryStream();
                using (FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                    file.CopyTo(memoryStream);
                memoryStream.Position = 0;
                using (FileStream file = new FileStream(targetFileName, FileMode.Create, System.IO.FileAccess.Write))
                {
                    memoryStream.CopyTo(file);
                }



            }
        }
    }
}
