﻿using System;
using System.Collections.Generic;
using System.ComponentModel;

using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlavourBusinessFacade;
using FlavourBusinessFacade.HumanResources;
using OOAdvantech;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Remoting.RestApi;

using UIBaseEx;
using System.Net.Mail;
using FlavourBusinessManager;



#if DeviceDotNet
using Xamarin.Forms;
using ZXing;
using MarshalByRefObject = OOAdvantech.Remoting.MarshalByRefObject;
public interface IFontsResolver
{

}
#else
using QRCoder;
using MenuPresentationModel.MenuCanvas;
using System.Drawing.Imaging;
using MarshalByRefObject = System.MarshalByRefObject;


#endif


namespace ServiceContextManagerApp
{



    /// <MetaDataID>{57c59ee6-4bcb-45a9-9635-06d1d922efea}</MetaDataID>
    public class ManagerPresentation : MarshalByRefObject, INotifyPropertyChanged, IManagerPresentation, FlavourBusinessFacade.ViewModel.ISecureUser, IFontsResolver, OOAdvantech.Remoting.IExtMarshalByRefObject
    {



        public ManagerPresentation()
        {
            
            DeviceAuthentication.AuthStateChanged += DeviceAuthentication_AuthStateChanged;
#if DeviceDotNet

            OOAdvantech.Authentication.OOAdvantechAuth.Auth.AuthStateChange += Auth_AuthStateChange;



            //this.ScanPage.HeaderText = "Hold your phone up to the place Identity";
            //this.ScanPage.FooterText = "Scanning will happen automatically";
            //ScanPage.OnScan += async (result) =>
            //{
            //    if (OnScan)
            //    {
            //        TaskCompletionSource<bool> connectToServicePointTask = null;
            //        lock (this)
            //        {
            //            connectToServicePointTask = ConnectToServicePointTask;
            //            ConnectToServicePointTask = null;
            //            OnScan = false;
            //        }

            //        string supervisorAssignKey = result.Text;
            //        string assemblyData = "FlavourBusinessManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
            //        string type = "FlavourBusinessManager.FlavoursServicesContextManagment";
            //        string serverUrl = AzureServerUrl;

            //        AuthUser authUser = System.Runtime.Remoting.Messaging.CallContext.GetData("AutUser") as AuthUser;
            //        if (authUser == null)
            //            authUser = DeviceAuthentication.AuthUser;
            //        try
            //        {
            //            IFlavoursServicesContextManagment servicesContextManagment = RemotingServices.CastTransparentProxy<IFlavoursServicesContextManagment>(OOAdvantech.Remoting.RestApi.RemotingServices.CreateRemoteInstance(serverUrl, type, assemblyData));
            //            this.ServiceContextSupervisor = servicesContextManagment.AssignSupervisorUser(supervisorAssignKey);

            //        }
            //        catch (Exception error)
            //        {
            //        }
            //        // Navigate away
            //        try
            //        {
            //            await ScanPage.Navigation.PopAsync();
            //            connectToServicePointTask.SetResult(true);
            //        }
            //        catch (Exception error)
            //        {
            //            connectToServicePointTask.SetResult(false);
            //        }
            //    }
            //};
            //ScanPage.Disappearing += (object sender, EventArgs e) =>
            //{
            //    OnScan = false;
            //};
#endif
        }

#if DeviceDotNet
        private void Auth_AuthStateChange(object sender, OOAdvantech.Authentication.AuthStateEventArgs e)
        {
            var ssds = e.Auth.CurrentUser;

        }


        public DeviceUtilities.NetStandard.ScanCode ScanCode = new DeviceUtilities.NetStandard.ScanCode();
#endif
        public async Task<bool> Assign()
        {
            //this.ScanPage.HeaderText = "Hold your phone up to the place Identity";
            //this.ScanPage.FooterText = "Scanning will happen automatically";
#if DeviceDotNet
            string supervisorAssignKey = null;
            try
            {
                var result = await ScanCode.Scan("Hold your phone up to the place Identity", "Scanning will happen automatically");

                if (result == null || string.IsNullOrWhiteSpace(result.Text))
                    return false;
                supervisorAssignKey = result.Text;
            }
            catch (Exception error)
            {
                return false;
            }

            string assemblyData = "FlavourBusinessManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
            string type = "FlavourBusinessManager.FlavoursServicesContextManagment";
            string serverUrl = AzureServerUrl;

            AuthUser authUser = System.Runtime.Remoting.Messaging.CallContext.GetData("AutUser") as AuthUser;
            if (authUser == null)
                authUser = DeviceAuthentication.AuthUser;

            try
            {
               var pAuthFlavourBusiness = GetFlavourBusinessAuth();
                UserData = pAuthFlavourBusiness.SignIn();
                return true;
            }
            catch (System.Net.WebException error)
            {
                throw;
            }
            catch (Exception error)
            {
                throw;
            }

       



            //lock (this)
            //{
            //    if (OnScan && ConnectToServicePointTask != null)
            //        return ConnectToServicePointTask.Task;

            //    OnScan = true;
            //    ConnectToServicePointTask = new TaskCompletionSource<bool>();
            //}
            //Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
            //{
            //    await (App.Current.MainPage as NavigationPage).CurrentPage.Navigation.PushAsync(ScanPage);
            //});


            //return ConnectToServicePointTask.Task;
#else
            return true;
#endif

        }

        //TaskCompletionSource<bool> ConnectToServicePointTask;


        //bool OnScan = false;
        private void DeviceAuthentication_AuthStateChanged(object sender, AuthUser user)
        {
            if (user == null)
                SignOut();
        }





        //static string _AzureServerUrl = "http://192.168.2.8:8090/api/";//org
        //static string _AzureServerUrl = "http://192.168.2.4:8090/api/";//Braxati
        //static string _AzureServerUrl = "http://10.0.0.13:8090/api/";//work
        static string _AzureServerUrl = string.Format("http://{0}:8090/api/", FlavourBusinessFacade.ComputingResources.EndPoint.Server);


        static string AzureServerUrl
        {
            get
            {
                string azureStorageUrl = null;// OOAdvantech.Remoting.RestApi.RemotingServices.GetLocalIPAddress();
                if (azureStorageUrl == null)
                    azureStorageUrl = _AzureServerUrl;
                else
                    azureStorageUrl = "http://" + azureStorageUrl + ":8090/api/";

                return azureStorageUrl;
            }
        }

        string _ConfirmPassword;
        public string ConfirmPassword
        {
            get
            {
                return _ConfirmPassword;
            }

            set
            {
                _ConfirmPassword = value;
            }
        }


        string _SignInProvider;
        public string SignInProvider
        {
            get
            {
                return _SignInProvider;// ApplicationSettings.Current.SignInProvider;
            }
            set
            {
                _SignInProvider = value;// ApplicationSettings.Current.SignInProvider = value;
            }
        }
        string _Email;
        public string Email
        {
            get
            {
                return _Email;
            }

            set
            {
                _Email = value;
            }
        }

        string _FullName;
        public string FullName
        {
            get
            {
                return _FullName;
            }

            set
            {
                _FullName = value;
            }
        }

        string _Password;
        public string Password
        {
            get
            {
                return _Password;
            }

            set
            {
                _Password = value;
            }
        }

        string _UserName;
        public string UserName
        {
            get
            {
                return _UserName;
            }
            set
            {
                _UserName = value;

            }
        }



        public event PropertyChangedEventHandler PropertyChanged;
        public event ObjectChangeStateHandle _ObjectChangeState;
        public event ObjectChangeStateHandle ObjectChangeState
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

        string _PhoneNumber;
        [OOAdvantech.MetaDataRepository.HttpVisible]
        public string PhoneNumber
        {
            get
            {
                return _PhoneNumber;
            }
            set
            {
                _PhoneNumber = value;
            }
        }

        string _Address;
        [OOAdvantech.MetaDataRepository.HttpVisible]
        public string Address
        {
            get
            {
                return _Address;
            }

            set
            {
                _Address = value;
            }
        }




        [OOAdvantech.MetaDataRepository.HttpVisible]
        public void SignOut()
        {
            UserData = new UserData();
            AuthUser = null;
            Organization = null;
            ServiceContextSupervisor = null;
            _ServicesContexts.Clear();
        }
        [OOAdvantech.MetaDataRepository.HttpVisible]
        public void OnPageLoaded()
        {

        }
        [OOAdvantech.MetaDataRepository.HttpVisible]
        public void OnPageSizeChanged(double width, double height)
        {
        }

        [OOAdvantech.MetaDataRepository.HttpVisible]
        public async Task<bool> SignUp()
        {
            System.Diagnostics.Debug.WriteLine("public async Task< bool> SignIn()");
            AuthUser authUser = System.Runtime.Remoting.Messaging.CallContext.GetData("AutUser") as AuthUser;
            if (authUser == null)
            {
                authUser = DeviceAuthentication.AuthUser;
            }
            if (DeviceAuthentication.AuthUser == null)
            {

            }
            return await Task<bool>.Run(async () =>
            {

                OnSignIn = true;
                try
                {

                    IAuthFlavourBusiness pAuthFlavourBusiness = null;

                    try
                    {
                     pAuthFlavourBusiness = GetFlavourBusinessAuth();

                    }
                    catch (System.Net.WebException error)
                    {
                        throw;
                    }
                    catch (Exception error)
                    {
                        throw;
                    }

                    if (authUser == null)
                    {
                    }
                    UserData = new UserData() { Email = this.Email, FullName = this.FullName, PhoneNumber = this.PhoneNumber, Address = this.Address };
                    UserData = pAuthFlavourBusiness.SignUp(UserData);

                    if (UserData != null)
                    {
                        var role = UserData.Roles.Where(x => x.RoleType == UserData.RoleType.ServiceContextSupervisor).FirstOrDefault();
                        if (role.RoleType == UserData.RoleType.ServiceContextSupervisor)
                        {
                            ServiceContextSupervisor = RemotingServices.CastTransparentProxy<IServiceContextSupervisor>(role.User);
                            _OAuthUserIdentity=UserData.OAuthUserIdentity;
                        }

                        role = UserData.Roles.Where(x => x.RoleType == UserData.RoleType.Organization).FirstOrDefault();
                        if (role.RoleType == UserData.RoleType.Organization)
                        {
                            string administratorIdentity = "";
                            if (ServiceContextSupervisor != null)
                                administratorIdentity = ServiceContextSupervisor.Identity;

                            Organization = RemotingServices.CastTransparentProxy<IOrganization>(role.User);
                            _OAuthUserIdentity=UserData.OAuthUserIdentity;
                            _ServicesContexts = Organization.ServicesContexts.Select(x => new ServicesContextPresentation(x, ServiceContextSupervisor)).OfType<IServicesContextPresentation>().ToList();
                        }
                        else
                            _ServicesContexts = new List<IServicesContextPresentation>();

                        _FullName = UserData.FullName;
                        _UserName = UserData.UserName;
                        _Email=UserData.Email;
                        _PhoneNumber = UserData.PhoneNumber;
                        _Address = UserData.Address;

                        AuthUser=authUser;
                        _ObjectChangeState?.Invoke(this, null);

                        //if(Organization!=null&& ServiceContextSupervisor!=null)
                        //{
                        //    var serviceContex= Organization.GetFlavoursServicesContext(ServiceContextSupervisor.ServicesContextIdentity);
                        //    serviceContex.ObjectChangeState
                        //}
                        return true;
                    }
                    else
                        return false;

                }
                catch (Exception error)
                {

                    throw;
                }
                finally
                {
                    OnSignIn = false;
                }
            });
        }

        string _OAuthUserIdentity;
        public string OAuthUserIdentity
        {
            get
            {
                return _OAuthUserIdentity;// ApplicationSettings.Current.SignInUserIdentity;
            }
            set
            {
                _OAuthUserIdentity = value;// ApplicationSettings.Current.SignInUserIdentity = value;
            }
        }

        public bool IsOrganizationManager
        {
            get
            {
                return UserData != null && UserData.Roles != null && UserData.Roles.Where(x => x.RoleType == UserData.RoleType.Organization).FirstOrDefault().User != null;
            }
        }

        public bool IsServiceContextSupervisor
        {
            get
            {
                return UserData.Roles != null && UserData.Roles.Where(x => x.RoleType == UserData.RoleType.ServiceContextSupervisor).FirstOrDefault().User != null;
            }
        }
#if DeviceDotNet
        //ScanPage ScanPage = new ScanPage();//  FormsSample.CustomScanPage()
#endif





        public NewUserCode GetNewSupervisorQRCode(IServicesContextPresentation servicesContext, string color)
        {


            string codeValue = Organization.NewSupervisor((servicesContext as ServicesContextPresentation).ServicesContext.ServicesContextIdentity);
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
                SigBase64 = @"data:image/png;base64," + Convert.ToBase64String(byteImage);
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


            return new NewUserCode() { QRCode = SigBase64, Code = codeValue };
        }



        object SignInTaskLock = new object();


        UserData UserData;
        AuthUser AuthUser;
        public bool OnSignIn;
        [OOAdvantech.MetaDataRepository.HttpVisible]
        public async Task<bool> SignIn()
        {

            //System.IO.File.AppendAllLines(App.storage_path, new string[] { "SignIn " });
            System.Diagnostics.Debug.WriteLine("public async Task< bool> SignIn()");
            AuthUser authUser = System.Runtime.Remoting.Messaging.CallContext.GetData("AutUser") as AuthUser;
            if (authUser == null)
            {
                authUser = DeviceAuthentication.AuthUser;
            }
            if (DeviceAuthentication.AuthUser == null)
            {

            }
            if (authUser == null)
                return false;
            if (AuthUser != null && authUser.User_ID == AuthUser.User_ID)
                return true;


            if (SignInTask == null||!OnSignIn)
            {

                lock (SignInTaskLock)
                {
                    SignInTask = Task<bool>.Run(async () =>
            {

                

                OnSignIn = true;
                try
                {
                    string assemblyData = "FlavourBusinessManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
                    string type = "FlavourBusinessManager.AuthFlavourBusiness";// typeof(FlavourBusinessManager.AuthFlavourBusiness).FullName;
                    System.Runtime.Remoting.Messaging.CallContext.SetData("AutUser", authUser);
                    string serverUrl = "http://localhost/FlavourBusinessWebApiRole/api/";
                    serverUrl = "http://localhost:8090/api/";
                    serverUrl = AzureServerUrl;
                    IAuthFlavourBusiness pAuthFlavourBusiness = null;

                    try
                    {
                        pAuthFlavourBusiness = GetFlavourBusinessAuth();
                    }
                    catch (System.Net.WebException error)
                    {
                        throw;
                    }
                    catch (Exception error)
                    {
                        throw;
                    }


                    //sds.SendTimeout
                    //OOAdvantech.Remoting.RestApi.RemotingServices.T
                    authUser = DeviceAuthentication.AuthUser;
                    if (DeviceAuthentication.AuthUser == null)
                    {

                    }
                    if (authUser == null)
                    {

                    }
                    authUser = DeviceAuthentication.AuthUser;
                    _OAuthUserIdentity=null;
                    UserData = pAuthFlavourBusiness.SignIn();
                    if (UserData != null)
                    {
                        _FullName = UserData.FullName;
                        _UserName = UserData.UserName;
                        _Email=UserData.Email;
                        _PhoneNumber = UserData.PhoneNumber;
                        _Address = UserData.Address;

                        AuthUser=authUser;

                        var role = UserData.Roles.Where(x => x.RoleType == UserData.RoleType.ServiceContextSupervisor).FirstOrDefault();
                        if (role.RoleType == UserData.RoleType.ServiceContextSupervisor)
                        {
                            ServiceContextSupervisor = RemotingServices.CastTransparentProxy<IServiceContextSupervisor>(role.User);
                            _OAuthUserIdentity=UserData.OAuthUserIdentity;
                        }

                        role = UserData.Roles.Where(x => x.RoleType == UserData.RoleType.Organization).FirstOrDefault();
                        if (role.RoleType == UserData.RoleType.Organization)
                        {
                            Organization = RemotingServices.CastTransparentProxy<IOrganization>(role.User);
                            string administratorIdentity = "";
                            _OAuthUserIdentity=UserData.OAuthUserIdentity;

                            if (ServiceContextSupervisor != null)
                            {
                                administratorIdentity = ServiceContextSupervisor.Identity;
                                var flavoursServicesContext = Organization.GetFlavoursServicesContext(ServiceContextSupervisor.ServicesContextIdentity);
                                ServiceContextSupervisors[ServiceContextSupervisor.ServicesContextIdentity] = flavoursServicesContext.ServiceContextHumanResources.Supervisors.Where(x => x.OAuthUserIdentity != ServiceContextSupervisor.OAuthUserIdentity).ToList();
                            }
                            else
                            {

                            }
                            _ServicesContexts = Organization.ServicesContexts.Select(x => new ServicesContextPresentation(x, ServiceContextSupervisor)).OfType<IServicesContextPresentation>().ToList();
                        }
                        else
                            _ServicesContexts = new List<IServicesContextPresentation>();


                        _ObjectChangeState?.Invoke(this, null);
                        return true;
                    }
                    else
                        return false;


                }
                catch (Exception error)
                {
                     
                    throw;
                }
                finally
                {
                    OnSignIn = false;
                }
            }); 
                }

                

            }
            return await SignInTask;
        }


        public bool IsUsernameInUse(string username, OOAdvantech.Authentication.SignInProvider signInProvider)
        {
            IAuthFlavourBusiness pAuthFlavourBusiness = null;


            try
            {
                pAuthFlavourBusiness = GetFlavourBusinessAuth();
                return pAuthFlavourBusiness.IsUsernameInUse(username, signInProvider);
            }
            catch (System.Net.WebException error)
            {
                throw;
            }
            catch (Exception error)
            {
                throw;
            }
        }

        private static IAuthFlavourBusiness GetFlavourBusinessAuth()
        {
            IAuthFlavourBusiness pAuthFlavourBusiness;
            AuthUser authUser = System.Runtime.Remoting.Messaging.CallContext.GetData("AutUser") as AuthUser;
            string assemblyData = "FlavourBusinessManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
            string type = "FlavourBusinessManager.AuthFlavourBusiness";// typeof(FlavourBusinessManager.AuthFlavourBusiness).FullName;
            System.Runtime.Remoting.Messaging.CallContext.SetData("AutUser", authUser);
            string serverUrl = AzureServerUrl;
            var remoteObject = RemotingServices.CreateRemoteInstance(serverUrl, type, assemblyData);
            pAuthFlavourBusiness =RemotingServices.CastTransparentProxy<IAuthFlavourBusiness>(remoteObject);
            return pAuthFlavourBusiness;
        }

        public void CreateUserWithEmailAndPassword(string emailVerificationCode)
        {
            IAuthFlavourBusiness pAuthFlavourBusiness = null;


            try
            {
                
                
                UserData userData = new UserData() { UserName=UserName, Email=Email, FullName=FullName, Address=Address, PhoneNumber=PhoneNumber };
                pAuthFlavourBusiness = GetFlavourBusinessAuth();
                pAuthFlavourBusiness.SignUpUserWithEmailAndPassword(Email, Password, userData, emailVerificationCode);
            }
            catch (System.Net.WebException error)
            {
                throw;
            }
            catch (Exception error)
            {
                throw;
            }

        }

        public void SendVerificationEmail(string emailAddress)
        {
            IAuthFlavourBusiness pAuthFlavourBusiness = null;


            try
            {
                pAuthFlavourBusiness = GetFlavourBusinessAuth();
                pAuthFlavourBusiness.SendVerificationEmail(emailAddress);
            }
            catch (System.Net.WebException error)
            {
                throw;
            }
            catch (Exception error)
            {
                throw;
            }
        }


      


        List<IServicesContextPresentation> _ServicesContexts = new List<IServicesContextPresentation>();
        public List<IServicesContextPresentation> ServicesContexts
        {
            get
            {
                return _ServicesContexts.ToList();
            }
        }

        Dictionary<string, List<IServiceContextSupervisor>> ServiceContextSupervisors = new Dictionary<string, List<IServiceContextSupervisor>>();

        [OOAdvantech.MetaDataRepository.HttpVisible]
        public void SaveUserProfile()
        {
            try
            {
                IAuthFlavourBusiness pAuthFlavourBusiness = GetFlavourBusinessAuth();// OOAdvantech.Remoting.RestApi.RemotingServices.CreateRemoteInstance(serverUrl, type, assemblyData) as IAuthFlavourBusiness;
                UserData = new UserData() { Email = this.Email, FullName = this.FullName, PhoneNumber = this.PhoneNumber, Address = this.Address };
                pAuthFlavourBusiness.UpdateUserProfile(UserData, UserData.RoleType.Waiter);

            }
            catch (System.Net.WebException error)
            {
                throw;
            }
            catch (Exception error)
            {
                throw;
            }
        }

        public string FlavoursServiceContextDescription
        {
            get
            {
                if (this.ServiceContextSupervisor != null)
                    return this.ServiceContextSupervisor.FlavoursServiceContextDescription;
                else
                    return "";
            }
        }
        public async Task<bool> AssignSupervisor()
        {
            return await Assign();
        }

        Dictionary<string, FontData> Fonts = new Dictionary<string, FontData>();

        public FontData GetFont(string fontUri)
        {
            FontData fontData;
            if (string.IsNullOrEmpty(fontUri))
                return default(FontData);

            if (!Fonts.TryGetValue(fontUri, out fontData))
            {
                string fontUrl = string.Format("http://{0}:8090/api/MenuModel/Font/{1}", FlavourBusinessFacade.ComputingResources.EndPoint.Server, fontUri);
                using (System.Net.WebClient wc = new System.Net.WebClient())
                {
                    var json = wc.DownloadString(fontUrl);
                    fontData= OOAdvantech.Json.JsonConvert.DeserializeObject<FontData>(json);
                    Fonts[fontUri] = fontData;
                }
            }
            return fontData;
        }

        IServiceContextSupervisor ServiceContextSupervisor;

        IOrganization Organization;
        private Task<bool> SignInTask;
    }



}
