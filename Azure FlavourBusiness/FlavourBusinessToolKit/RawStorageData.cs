using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using FlavourBusinessFacade;
using System.Net.Http;

namespace FlavourBusinessToolKit
{
    /// <MetaDataID>{96c28c00-0489-4476-9a52-af9addb3aab7}</MetaDataID>
    public class RawStorageData : OOAdvantech.PersistenceLayer.IRawStorageData
    {
        string _FileName;

        public string FileName
        {
            get
            {
                return _FileName;
            }
        }
        public readonly XDocument StorageDoc;
        public readonly OrganizationStorageRef StorageRef;
        IUploadService UploadService;
        public RawStorageData(XDocument storageDoc, string fileName, OrganizationStorageRef storageRef, IUploadService uploadService)
        {
            StorageDoc = storageDoc;
            _FileName = fileName;
            StorageRef = storageRef;
            UploadService = uploadService;
        }

        public RawStorageData( string localFileName, OrganizationStorageRef storageRef, IUploadService uploadService)
        {
            string path = null;
            var foldersSequence = localFileName.Split('\\');
            foldersSequence = foldersSequence.Take(foldersSequence.Length - 1).ToArray();
            foreach (var dir in foldersSequence)
            {
                if (path != null)
                    path += "\\" + dir;
                else
                    path = dir;

                if (!System.IO.Directory.Exists(path))
                    System.IO.Directory.CreateDirectory(path);
            }
            HttpClient httpClient = new HttpClient();
            var dataStreamTask = httpClient.GetStreamAsync(storageRef.StorageUrl);
            dataStreamTask.Wait();
            using (var dataStream = dataStreamTask.Result)
            {
                StorageDoc = XDocument.Load(dataStream);
                _FileName = localFileName;
                StorageRef = storageRef;
                UploadService = uploadService;
            }
        }

        public RawStorageData(OrganizationStorageRef storageRef, IUploadService uploadService)
        {
            HttpClient httpClient = new HttpClient();
            var getStreamTask = httpClient.GetStreamAsync(storageRef.StorageUrl);
            getStreamTask.Wait();
            var dataStream = getStreamTask.Result;
            StorageDoc = XDocument.Load(dataStream);
            StorageRef = storageRef;
            UploadService = uploadService;
        }
        //public RawStorageData(string fileName, OrganizationStorageRef storageRef, IUploadService uploadService)
        //{
        //    _FileName = fileName;
        //    HttpClient httpClient = new HttpClient();
        //    var getStreamTask =  httpClient.GetStreamAsync(storageRef.StorageUrl);
        //    getStreamTask.Wait();
        //    var dataStream = getStreamTask.Result;
        //    StorageDoc = XDocument.Load(dataStream);
        //    StorageRef = storageRef;
        //    UploadService = uploadService;
        //}

        public object RawData
        {
            get
            {
                return StorageDoc;

            }
            set
            {
                throw new NotImplementedException();
            }
        }


        public bool IsReadonly => UploadService == null;

        public string StorageLocation => StorageRef.StorageUrl;

        public string StorageName => StorageRef.Name;

        public void SaveRawData()
        {
            if (UploadService == null)
                throw new System.Exception("Object storage is read only.  StorageLocation : " + StorageLocation);
            if (!string.IsNullOrWhiteSpace(FileName))
            {
                StorageDoc.Root.Save(FileName);
                MemoryStream memoryStream = new MemoryStream();

                StorageDoc.Save(memoryStream, SaveOptions.None);
                byte[] Buffer = new byte[memoryStream.Length];
                memoryStream.Position = 0;
                //memoryStream.Read(Buffer, 0,(int) memoryStream.Length);

                var uploadSlot = UploadService.GetUploadSlotFor(StorageRef);
                FlavourBusinessToolKit.RestApiBlobFileManager.Upload(memoryStream, uploadSlot, "application/xml");
                return;
            }
            else
            {
                //SaveFileDialog saveFile = new SaveFileDialog();
                //saveFile.Filter = "Files (*.xml)|*.xml|All Files (*.*)|*.*";
                //if (saveFile.ShowDialog() == true)
                //{
                //    try
                //    {
                //        StorageDoc.Root.Save(saveFile.FileName);
                //        _FileName = saveFile.FileName;
                //    }
                //    catch (Exception ex)
                //    {
                //        MessageBox.Show(ex.StackTrace, ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                //    }
                //}
            }

        }
    }
}
