using System;
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
using FlavourBusinessFacade.RoomService;
using System.Linq.Expressions;




#if DeviceDotNet
using Xamarin.Forms;
using ZXing;
using MarshalByRefObject = OOAdvantech.Remoting.MarshalByRefObject;
/// <MetaDataID>{31ffc233-6769-4a2c-bb2f-451aeab898e7}</MetaDataID>
public interface IFontsResolver
{

}
#else
using QRCoder;
using MenuPresentationModel.MenuCanvas;
using System.Drawing.Imaging;
using MarshalByRefObject = System.MarshalByRefObject;
using FlavourBusinessManager.HumanResources;

#endif


namespace ServiceContextManagerApp
{



    /// <MetaDataID>{57c59ee6-4bcb-45a9-9635-06d1d922efea}</MetaDataID>
    public class ManagerPresentation : MarshalByRefObject, INotifyPropertyChanged, IManagerPresentation, FlavourBusinessFacade.ViewModel.ISecureUser, IFontsResolver, OOAdvantech.Remoting.IExtMarshalByRefObject
    {



        /// <MetaDataID>{336c5eed-616e-4844-b938-ea8acbeb0e1b}</MetaDataID>
        public ManagerPresentation()
        {


            Expression<System.Func<IMealsController, dynamic>> expression = t => t.MealCoursesInProgress.Select(x => new
            {
                x.Name,
                x.Meal,
                FoodItemsInProgress = x.FoodItemsInProgress.Select(y => new
                {
                    y.Description,
                    y.SessionType
                })
            });


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
        /// <MetaDataID>{306f6042-7558-4084-8104-01b5a2f3ff31}</MetaDataID>
        private void Auth_AuthStateChange(object sender, OOAdvantech.Authentication.AuthStateEventArgs e)
        {
            var ssds = e.Auth.CurrentUser;

        }


        /// <MetaDataID>{e1874240-bd62-4eed-9a55-59f15dbb1e7f}</MetaDataID>
        public DeviceUtilities.NetStandard.ScanCode ScanCode = new DeviceUtilities.NetStandard.ScanCode();
#endif
        /// <MetaDataID>{75663c23-6955-4a5d-b3fb-bde8b1bd38d7}</MetaDataID>
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
        /// <MetaDataID>{83fa7a7b-086c-474b-9add-b9776f8276ea}</MetaDataID>
        private void DeviceAuthentication_AuthStateChanged(object sender, AuthUser user)
        {
            if (user == null)
                SignOut();
        }





        //static string _AzureServerUrl = "http://192.168.2.8:8090/api/";//org
        //static string _AzureServerUrl = "http://192.168.2.4:8090/api/";//Braxati
        //static string _AzureServerUrl = "http://10.0.0.13:8090/api/";//work
        /// <exclude>Excluded</exclude>
        static string _AzureServerUrl = string.Format("http://{0}:8090/api/", FlavourBusinessFacade.ComputingResources.EndPoint.Server);


        /// <MetaDataID>{fd05c84a-026a-4737-8109-c2cc8c54b037}</MetaDataID>
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

        /// <exclude>Excluded</exclude>
        string _ConfirmPassword;
        /// <MetaDataID>{1c54861c-2d1b-4e27-b56c-3967c760d306}</MetaDataID>
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


        /// <exclude>Excluded</exclude>
        string _SignInProvider;
        /// <MetaDataID>{e7973798-b089-4821-962e-06c217adcf47}</MetaDataID>
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
        /// <exclude>Excluded</exclude>
        string _Email;
        /// <MetaDataID>{c0e0f36e-8bde-4897-bc4a-bccc62f90f94}</MetaDataID>
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

        /// <exclude>Excluded</exclude>
        string _FullName;
        /// <MetaDataID>{69c99fdd-f17f-4c08-8e54-48fbd8fdf42d}</MetaDataID>
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

        /// <exclude>Excluded</exclude>
        string _Password;
        /// <MetaDataID>{18350b7e-b963-40cc-b053-a5ffac9efefe}</MetaDataID>
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

        /// <exclude>Excluded</exclude>
        string _UserName;
        /// <MetaDataID>{e849ba56-e073-4248-a791-24c82e8ee214}</MetaDataID>
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





        /// <MetaDataID>{17ba4f14-257f-4b3a-9393-75998cef416f}</MetaDataID>
        IShiftWork ShiftWork;


        /// <MetaDataID>{c109b363-534b-4f4f-9743-652b9c30d16e}</MetaDataID>
        public DateTime ActiveShiftWorkStartedAt
        {
            get
            {
                if (InActiveShiftWork)
                    return ShiftWork.StartsAt;
                else
                    return DateTime.MinValue;
            }
        }


        /// <MetaDataID>{0f1c3a01-3251-4acf-befa-c015e66a53bc}</MetaDataID>
        public DateTime ActiveShiftWorkEndsAt
        {
            get
            {
                if (InActiveShiftWork)
                    return ShiftWork.StartsAt + TimeSpan.FromHours(ShiftWork.PeriodInHours);
                else
                    return DateTime.MinValue;
            }
        }


        /// <MetaDataID>{98a2d3fb-2f90-4cde-9bf2-a400f08f29c0}</MetaDataID>
        public bool InActiveShiftWork
        {
            get
            {
                if (ShiftWork?.IsActive() == true)
                    return true;
                else
                    return false;
            }
        }



        /// <MetaDataID>{2f005569-6308-4b93-8986-c6e26fddfeca}</MetaDataID>
        public async void ShiftWorkStart(DateTime startedAt, double timespanInHours)
        {
            if (ServiceContextSupervisor != null)
            {
                ShiftWork = ServiceContextSupervisor.NewShiftWork(startedAt, timespanInHours);

                if (ShiftWork != null)
                    _ObjectChangeState?.Invoke(this, nameof(ShiftWork));
            }
        }






        public event PropertyChangedEventHandler PropertyChanged;
        /// <exclude>Excluded</exclude>
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

        /// <exclude>Excluded</exclude>
        string _PhoneNumber;
        /// <MetaDataID>{7f73bb48-37ca-4b22-b761-f5b9aa0948a5}</MetaDataID>
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

        /// <exclude>Excluded</exclude>
        string _Address;
        /// <MetaDataID>{f45b84ec-f70e-426f-a780-a143d458ec55}</MetaDataID>
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




        /// <MetaDataID>{0c7b86e7-e55e-414a-9658-248fd799bb65}</MetaDataID>
        [OOAdvantech.MetaDataRepository.HttpVisible]
        public void SignOut()
        {
            UserData = new UserData();
            AuthUser = null;
            Organization = null;
            ServiceContextSupervisor = null;
            _ServicesContexts.Clear();
        }
        /// <MetaDataID>{d0e5e87c-585d-40cb-82a1-944937be16c6}</MetaDataID>
        [OOAdvantech.MetaDataRepository.HttpVisible]
        public void OnPageLoaded()
        {

        }
        /// <MetaDataID>{9677066b-9054-4613-beaf-97c1127c4b2c}</MetaDataID>
        [OOAdvantech.MetaDataRepository.HttpVisible]
        public void OnPageSizeChanged(double width, double height)
        {
        }

        /// <MetaDataID>{4fbee07a-7587-46ff-9dd0-8a5ac3c83a62}</MetaDataID>
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
                        var role = UserData.Roles.Where(x => x.RoleType == RoleType.ServiceContextSupervisor).FirstOrDefault();
                        if (role.RoleType == RoleType.ServiceContextSupervisor)
                        {
                            ServiceContextSupervisor = RemotingServices.CastTransparentProxy<IServiceContextSupervisor>(role.User);
                            _OAuthUserIdentity = ServiceContextSupervisor.OAuthUserIdentity;
                        }

                        role = UserData.Roles.Where(x => x.RoleType == RoleType.Organization).FirstOrDefault();
                        if (role.RoleType == RoleType.Organization)
                        {
                            string administratorIdentity = "";
                            if (ServiceContextSupervisor != null)
                                administratorIdentity = ServiceContextSupervisor.Identity;

                            Organization = RemotingServices.CastTransparentProxy<IOrganization>(role.User);
                            _OAuthUserIdentity = Organization.OAuthUserIdentity;
                            _ServicesContexts = Organization.ServicesContexts.Select(x => new ServicesContextPresentation(x, ServiceContextSupervisor)).OfType<IServicesContextPresentation>().ToList();
                        }
                        else
                            _ServicesContexts = new List<IServicesContextPresentation>();

                        _FullName = UserData.FullName;
                        _UserName = UserData.UserName;
                        _Email = UserData.Email;
                        _PhoneNumber = UserData.PhoneNumber;
                        _Address = UserData.Address;

                        AuthUser = authUser;
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

        /// <exclude>Excluded</exclude>
        string _OAuthUserIdentity;
        /// <MetaDataID>{e36af3eb-2875-4bda-94b6-73ed287df617}</MetaDataID>
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

        /// <MetaDataID>{c13a180e-e632-4759-af6a-c68728530923}</MetaDataID>
        public bool IsOrganizationManager
        {
            get
            {
                return UserData != null && UserData.Roles != null && UserData.Roles.Where(x => x.RoleType == RoleType.Organization).FirstOrDefault().User != null;
            }
        }

        /// <MetaDataID>{a8d03ca4-910c-4e8c-b010-9c469ebc7f4a}</MetaDataID>
        public bool IsServiceContextSupervisor
        {
            get
            {
                return UserData.Roles != null && UserData.Roles.Where(x => x.RoleType == RoleType.ServiceContextSupervisor).FirstOrDefault().User != null;
            }
        }
#if DeviceDotNet
        //ScanPage ScanPage = new ScanPage();//  FormsSample.CustomScanPage()
#endif





        /// <MetaDataID>{f8606fc4-2fff-4c04-8193-d7b04b833ae4}</MetaDataID>
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



        /// <MetaDataID>{a33fe5c3-fea7-4637-8e50-c1a3c4ec585d}</MetaDataID>
        object SignInTaskLock = new object();


        /// <MetaDataID>{0a183e2a-369d-492a-ae23-4c06a3445445}</MetaDataID>
        UserData UserData;
        /// <MetaDataID>{7e769640-40f7-430b-a86c-2fb26408c5b9}</MetaDataID>
        AuthUser AuthUser;
        /// <MetaDataID>{d88fc483-0d3e-4809-be40-c4bfef668a20}</MetaDataID>
        public bool OnSignIn;
        /// <MetaDataID>{2471d38b-4b03-44ae-bac1-ad4faf05e0ce}</MetaDataID>
        [OOAdvantech.MetaDataRepository.HttpVisible]
        public async Task<bool> SignIn()
        {
#if DeviceDotNet
            OOAdvantech.DeviceApplication.Current.Log(new List<string>() { "SignIn()" });
#endif

            //System.IO.File.AppendAllLines(App.storage_path, new string[] { "SignIn " });
            System.Diagnostics.Debug.WriteLine(" SignIn()");
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

            lock (SignInTaskLock)
            {

                if (SignInTask == null || !OnSignIn)
                {
                    OnSignIn = true;
                    //int count = 5;
                    //while (count-->=0)
                    {
                        SignInTask = Task<bool>.Run(async () =>
                        {
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
                                _OAuthUserIdentity = null;
                                UserData = pAuthFlavourBusiness.SignIn();
                                if (UserData != null)
                                {
                                    _FullName = UserData.FullName;
                                    _UserName = UserData.UserName;
                                    _Email = UserData.Email;
                                    _PhoneNumber = UserData.PhoneNumber;
                                    _Address = UserData.Address;
#if DeviceDotNet
                                OOAdvantech.DeviceApplication.Current.Log(new List<string>() { "ServiceContextSupervisor sign in" });
#endif
                                    AuthUser = authUser;
                                    string administratorIdentity = "";
                                    var roles = UserData.Roles.Where(x => x.RoleType == RoleType.ServiceContextSupervisor);

                                    foreach (var supervisorRole in roles)
                                    {
                                        ServiceContextSupervisor = RemotingServices.CastTransparentProxy<IServiceContextSupervisor>(supervisorRole.User);
                                        var servicesContextIdentity = ServiceContextSupervisor.ServicesContextIdentity;
                                        if (!UserServiceContextSupervisorRoles.ContainsKey(servicesContextIdentity))
                                        {
                                            _OAuthUserIdentity = ServiceContextSupervisor.OAuthUserIdentity;

                                            administratorIdentity = ServiceContextSupervisor.Identity;
                                            var flavoursServicesContext = ServiceContextSupervisor.ServicesContext; ;
                                            UserServiceContextSupervisorRoles[servicesContextIdentity] = flavoursServicesContext.ServiceContextHumanResources.Supervisors.Where(x => x.OAuthUserIdentity != ServiceContextSupervisor.OAuthUserIdentity).ToList();

                                            ServicesContextPresentation servicesContextPresentation = new ServicesContextPresentation(flavoursServicesContext, ServiceContextSupervisor);

#if DeviceDotNet
                                        IDeviceOOAdvantechCore device = DependencyService.Get<IDeviceInstantiator>().GetDeviceSpecific(typeof(OOAdvantech.IDeviceOOAdvantechCore)) as OOAdvantech.IDeviceOOAdvantechCore;
                                        ServiceContextSupervisor.DeviceFirebaseToken = device.FirebaseToken;
                                        OOAdvantech.DeviceApplication.Current.Log(new List<string>() { "servicesContextPresentation added" });
#endif
                                            _ServicesContexts.Add(servicesContextPresentation);

                                            if (_ServicesContexts.Count > 1)
                                            {

                                            }
                                        }


                                    }



                                    var role = UserData.Roles.Where(x => x.RoleType == RoleType.Organization).FirstOrDefault();
                                    if (role.RoleType == RoleType.Organization && ServiceContextSupervisor == null)
                                    {

                                        var servicesContextIdentity = ServiceContextSupervisor.ServicesContextIdentity;
                                        Organization = RemotingServices.CastTransparentProxy<IOrganization>(role.User);
                                        _ServicesContexts = Organization.ServicesContexts.Select(x => new ServicesContextPresentation(x, ServiceContextSupervisor)).OfType<IServicesContextPresentation>().ToList();
#if DeviceDotNet
                                    OOAdvantech.DeviceApplication.Current.Log(new List<string>() { "Organization servicesContextPresentations" });
#endif
                                        if (_ServicesContexts.Count > 1)
                                        {

                                        }

                                        //_ObjectChangeState?.Invoke(this, null);
                                    }
                                    //else
                                    //{
                                    //    _ServicesContexts = new List<IServicesContextPresentation>();
                                    //}


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



            }
            return await SignInTask;
        }


        /// <MetaDataID>{6e691b92-7f64-489f-8927-d3ee52e41de7}</MetaDataID>
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

        /// <MetaDataID>{cf374d65-19a1-40bf-b6ce-98ccdc65e22c}</MetaDataID>
        public Task<IList<UserData>> GetNativeUsers()
        {
            return Task<IList<UserData>>.FromResult(new List<UserData>() as IList<UserData> );
        }

        /// <MetaDataID>{fff52657-d165-4775-af70-255c5ce4b7eb}</MetaDataID>
        public UserData SignInNativeUser(string userName, string password)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{14cd392b-59c1-4cef-96da-9ee961d180df}</MetaDataID>
        private static IAuthFlavourBusiness GetFlavourBusinessAuth()
        {
            IAuthFlavourBusiness pAuthFlavourBusiness;
            AuthUser authUser = System.Runtime.Remoting.Messaging.CallContext.GetData("AutUser") as AuthUser;
            string assemblyData = "FlavourBusinessManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
            string type = "FlavourBusinessManager.AuthFlavourBusiness";// typeof(FlavourBusinessManager.AuthFlavourBusiness).FullName;
            System.Runtime.Remoting.Messaging.CallContext.SetData("AutUser", authUser);
            string serverUrl = AzureServerUrl;
            var remoteObject = RemotingServices.CreateRemoteInstance(serverUrl, type, assemblyData);
            pAuthFlavourBusiness = RemotingServices.CastTransparentProxy<IAuthFlavourBusiness>(remoteObject);
            return pAuthFlavourBusiness;
        }

        /// <MetaDataID>{fedf4e72-9fe2-4730-9d53-f73b2bd9010c}</MetaDataID>
        public void CreateUserWithEmailAndPassword(string emailVerificationCode)
        {
            IAuthFlavourBusiness pAuthFlavourBusiness = null;


            try
            {


                UserData userData = new UserData() { UserName = UserName, Email = Email, FullName = FullName, Address = Address, PhoneNumber = PhoneNumber };
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

        /// <MetaDataID>{fd445d7b-89d2-4383-9908-5d578190b89b}</MetaDataID>
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





        /// <MetaDataID>{e0cf560b-8218-45bf-9e8e-1ab82bb8bb96}</MetaDataID>
        List<IServicesContextPresentation> _ServicesContexts = new List<IServicesContextPresentation>();
        /// <MetaDataID>{569be9e0-84cc-49d1-bb9b-665d12b49059}</MetaDataID>
        public List<IServicesContextPresentation> ServicesContexts
        {
            get
            {
                return _ServicesContexts.ToList();
            }
        }
        /// <MetaDataID>{77b6b4da-7c36-48e3-8351-565ad752e7a9}</MetaDataID>
        private Dictionary<string, List<IServiceContextSupervisor>> UserServiceContextSupervisorRoles = new Dictionary<string, List<IServiceContextSupervisor>>();

        /// <MetaDataID>{889910a6-0a3e-4f1e-bd94-7bbd5bb89e92}</MetaDataID>
        [OOAdvantech.MetaDataRepository.HttpVisible]
        public void SaveUserProfile()
        {
            try
            {
                IAuthFlavourBusiness pAuthFlavourBusiness = GetFlavourBusinessAuth();// OOAdvantech.Remoting.RestApi.RemotingServices.CreateRemoteInstance(serverUrl, type, assemblyData) as IAuthFlavourBusiness;
                UserData = new UserData() { Email = this.Email, FullName = this.FullName, PhoneNumber = this.PhoneNumber, Address = this.Address };
                pAuthFlavourBusiness.UpdateUserProfile(UserData, RoleType.Waiter);

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

        /// <MetaDataID>{be5f5d18-17bb-4b96-8d73-00487b2d0f37}</MetaDataID>
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
        /// <MetaDataID>{1b8248d1-e683-4862-8cbd-7834de81cc4e}</MetaDataID>
        public async Task<bool> AssignSupervisor()
        {
            return await Assign();
        }

        /// <MetaDataID>{326542fc-2b82-4ecb-b079-9c913c3f034b}</MetaDataID>
        Dictionary<string, FontData> Fonts = new Dictionary<string, FontData>();

        /// <MetaDataID>{e7417b43-263c-4523-8a09-ce6fc440d180}</MetaDataID>
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
                    fontData = OOAdvantech.Json.JsonConvert.DeserializeObject<FontData>(json);
                    Fonts[fontUri] = fontData;
                }
            }
            return fontData;
        }

        /// <MetaDataID>{6d9638e3-723b-4fd2-b8c7-e42e76a99381}</MetaDataID>
        IServiceContextSupervisor ServiceContextSupervisor;

        /// <MetaDataID>{3f24c775-eaad-46a7-8a09-1d5e68498610}</MetaDataID>
        IOrganization Organization;
        /// <MetaDataID>{1da67023-cb0e-4988-a14f-54ecfa90ddd3}</MetaDataID>
        private Task<bool> SignInTask;
    }



}
