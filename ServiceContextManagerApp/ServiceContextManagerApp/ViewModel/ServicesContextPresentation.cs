using FlavourBusinessFacade;

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using FlavourBusinessFacade.HumanResources;





#if DeviceDotNet
using MarshalByRefObject = OOAdvantech.Remoting.MarshalByRefObject;
using ZXing;
#else
using MarshalByRefObject = System.MarshalByRefObject;
using System.Drawing.Imaging;
using QRCoder;
using System.IO;
using System;
#endif


namespace ServiceContextManagerApp
{
    /// <MetaDataID>{43d3b2e4-4576-47ec-bf5f-6ad3bb87aa57}</MetaDataID>
    public class ServicesContextPresentation : MarshalByRefObject, INotifyPropertyChanged, IServicesContextPresentation, OOAdvantech.Remoting.IExtMarshalByRefObject
    {
        public event PropertyChangedEventHandler PropertyChanged;

        internal IFlavoursServicesContext ServicesContext;

        public string ServicesContextName { get => ServicesContext.Description; set { } }

        List<IWaiterPresentation> _Waiters;
        public List<IWaiterPresentation> Waiters
        {
            get
            {
                if (_Waiters == null && ServicesContext != null)
                {
                    _Waiters = ServicesContext.ServiceContextHumanResources.Waiters.Select(x => new WaiterPresentation(x,ServicesContextRuntime)).OfType<IWaiterPresentation>().ToList();

                    //var administrator = _Waiters.Where(x => x.SupervisorIdentity == AdministratorIdentity).FirstOrDefault();
                    //if (administrator != null)
                    //{
                    //    _Waiters.Remove(administrator);
                    //    _Waiters.Insert(0, administrator);
                    //}
                    return _Waiters;
                }
                else if (_Waiters != null)
                    return _Waiters;
                else
                    return new List<IWaiterPresentation>();
            }
        }


        List<ISupervisorPresentation> _Supervisors;
        public List<ISupervisorPresentation> Supervisors
        {
            get
            {
                if (_Supervisors == null && ServicesContext != null)
                {
                    _Supervisors = ServicesContext.ServiceContextHumanResources.Supervisors.Select(x => new SupervisorPresentation(x, ServicesContextRuntime)).OfType<ISupervisorPresentation>().ToList();

                    var administrator = _Supervisors.Where(x => x.SupervisorIdentity == AdministratorIdentity).FirstOrDefault();
                    if (administrator != null)
                    {
                        _Supervisors.Remove(administrator);
                        _Supervisors.Insert(0, administrator);
                    }
                    return _Supervisors;
                }
                else if (_Supervisors != null)
                    return _Supervisors;
                else
                    return new List<ISupervisorPresentation>();
            }
        }




        IFlavoursServicesContextRuntime _FlavoursServicesContextRuntime;
        IFlavoursServicesContextRuntime FlavoursServicesContextRuntime
        {
            get
            {
                if (_FlavoursServicesContextRuntime == null)
                    _FlavoursServicesContextRuntime = ServicesContext.GetRunTime();

                return _FlavoursServicesContextRuntime;
            }
        }
        public bool RemoveSupervisor(ISupervisorPresentation supervisorPresentation)
        {
            
            var administrator = _Supervisors.Where(x => x.SupervisorIdentity == AdministratorIdentity).FirstOrDefault();
            if (supervisorPresentation == administrator)

                return false;

            return FlavoursServicesContextRuntime.RemoveSupervisor((supervisorPresentation as SupervisorPresentation).Supervisor);


        }



        public void MakeSupervisorActive(ISupervisorPresentation supervisorPresentation)
        {
            var administrator = _Supervisors.Where(x => x.SupervisorIdentity == AdministratorIdentity).FirstOrDefault();
            if (supervisorPresentation != administrator)
                FlavoursServicesContextRuntime.MakeSupervisorActive((supervisorPresentation as SupervisorPresentation).Supervisor);
        }

        IServiceContextSupervisor Administrator;

        public IFlavoursServicesContextRuntime ServicesContextRuntime { get; }

        string AdministratorIdentity;
        public ServicesContextPresentation(IFlavoursServicesContext servicesContext, IServiceContextSupervisor administrator)
        {

            AdministratorIdentity = "";
            Administrator = administrator;
            if (Administrator != null)
                AdministratorIdentity = Administrator.Identity;

            ServicesContext = servicesContext;
            ServicesContext.ObjectChangeState += ServicesContext_ObjectChangeState;
            

            this.ServicesContextRuntime = ServicesContext.GetRunTime();


        }

        private void ServicesContext_ObjectChangeState(object _object, string member)
        {

            _Supervisors = ServicesContext.ServiceContextHumanResources.Supervisors.Select(x => new SupervisorPresentation(x, ServicesContextRuntime)).OfType<ISupervisorPresentation>().ToList();
            _ObjectChangeState?.Invoke(this, nameof(Supervisors));

            _Waiters = ServicesContext.ServiceContextHumanResources.Waiters.Select(x => new WaiterPresentation(x, ServicesContextRuntime)).OfType<IWaiterPresentation>().ToList();
            _ObjectChangeState?.Invoke(this, nameof(Waiters));

        }

        [OOAdvantech.MetaDataRepository.HttpInVisible]
        event OOAdvantech.ObjectChangeStateHandle _ObjectChangeState;

        [OOAdvantech.MetaDataRepository.HttpInVisible]
        public event OOAdvantech.ObjectChangeStateHandle ObjectChangeState
        {
            add
            {
                _ObjectChangeState += value;
            }
            remove
            {
                _ObjectChangeState -= value;
            }
        }


        public NewSupervisorCode GetNewWaiterQRCode(string color)
        {


            string codeValue = FlavoursServicesContextRuntime.NewWaiter();
            string SigBase64 = "";
#if DeviceDotNet
            var barcodeWriter = new BarcodeWriterGeneric()
            {
                Format = ZXing.BarcodeFormat.QR_CODE,
                Options = new ZXing.Common.EncodingOptions
                {
                    Height = 400,
                    Width = 400
                }
            };


            var bitmapMatrix = barcodeWriter.Encode(codeValue);
            var width = bitmapMatrix.Width;
            var height = bitmapMatrix.Height;
            int[] pixelsImage = new int[width * height];
            SkiaSharp.SKBitmap qrCodeImage = new SkiaSharp.SKBitmap(width, height);

            SkiaSharp.SKColor fgColor = SkiaSharp.SKColors.Black;
            if (!SkiaSharp.SKColor.TryParse(color, out fgColor))
                fgColor = SkiaSharp.SKColors.Black;

            var pixels = qrCodeImage.Pixels;
            int k = 0;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (bitmapMatrix[j, i])
                        pixels[k++] = fgColor;
                    else
                        pixels[k++] = SkiaSharp.SKColors.White;
                }
            }
            qrCodeImage.Pixels = pixels;

            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                SkiaSharp.SKData d = SkiaSharp.SKImage.FromBitmap(qrCodeImage).Encode(SkiaSharp.SKEncodedImageFormat.Png, 100);
                d.SaveTo(ms);
                byte[] byteImage = ms.ToArray();
                SigBase64 = @"data:image/png;base64," + System.Convert.ToBase64String(byteImage);
            }
#else
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(codeValue, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            var qrCodeImage = qrCode.GetGraphic(20, color, "#FFFFFF", true);

            using (System.IO.MemoryStream ms = new MemoryStream())
            {
                qrCodeImage.Save(ms, ImageFormat.Png);
                byte[] byteImage = ms.ToArray();
                SigBase64 = @"data:image/png;base64," + Convert.ToBase64String(byteImage);
            }
#endif


            return new NewSupervisorCode() { QRCode = SigBase64, Code = codeValue };
        }


    }
}
