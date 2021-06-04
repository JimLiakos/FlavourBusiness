using FlavourBusinessFacade;
using FlavourBusinessManager;
using OOAdvantech.Remoting.RestApi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Net.Http;
using System.Xml.Linq;
using FlavourBusinessToolKit;
using OOAdvantech.Transactions;
using System.Threading;
using WPFUIElementObjectBind;

namespace FLBAuthentication.ViewModel
{



    public delegate void NewMessmateHandler(SignInUserPopupViewModel flavoursOrderServer, string messmateName);

    public delegate void AuthenticationChangeHandle(SignInUserPopupViewModel authedication, IUser user);


    /// <MetaDataID>{796fc987-8b06-477a-976e-e9ff817ae3c8}</MetaDataID>
    public class SignInUserPopupViewModel : MarshalByRefObject, INotifyPropertyChanged, FlavourBusinessFacade.ViewModel.IUser, OOAdvantech.Remoting.IExtMarshalByRefObject
    {

        public RelayCommand ClickPseudoCommand { get; protected set; }

        //FrameClicked.UserInterfaceObjectConnection.ContainerControl

        [OOAdvantech.MetaDataRepository.HttpVisible]
        public event NewMessmateHandler MessmateAdded;

        [OOAdvantech.MetaDataRepository.HttpVisible]
        public event OOAdvantech.ObjectChangeStateHandle ObjectChanged;

        public event EventHandler SwitchOnOffPopupView;

        UserData.RoleType RoleType;

        public SignInUserPopupViewModel(UserData.RoleType roleType)
        {
            RoleType = roleType;
            _PopupWitdh = 510;
            _PopupHeight = 540;
            MessmateAdded += SignInUserPopupViewModel_MessmateAdded;
            ClickPseudoCommand = new RelayCommand((object sender) => { });
        }

        private void SignInUserPopupViewModel_MessmateAdded(SignInUserPopupViewModel flavoursOrderServer, string messmateName)
        {

        }

        string _Path;
        public string PathPoints
        {
            get
            {
                //PathGeometry path = new PathGeometry();
                //System.Windows.Size size = new System.Windows.Size(PopupHeight, PopupWitdh);
                //int arrowPos = (int)(size.Width * 0.82);


                //PathGeometry pathGeometry = new PathGeometry();
                //pathGeometry.FillRule = FillRule.Nonzero;
                //PathFigure pathFigure = new PathFigure();
                //pathFigure.StartPoint = new Point(0, 10);
                //pathFigure.IsClosed = true;
                //pathGeometry.Figures.Add(pathFigure);
                //LineSegment lineSegment = new LineSegment();
                //lineSegment.Point = new Point(arrowPos, 10);
                //pathFigure.Segments.Add(lineSegment);
                //lineSegment = new LineSegment();
                //lineSegment.Point = new Point(arrowPos + 10, 0);
                //pathFigure.Segments.Add(lineSegment);
                //lineSegment = new LineSegment();
                //lineSegment.Point = new Point(arrowPos + 20, 10);
                //pathFigure.Segments.Add(lineSegment);
                //lineSegment = new LineSegment();
                //lineSegment.Point = new Point(size.Width, 10);
                //pathFigure.Segments.Add(lineSegment);
                //lineSegment = new LineSegment();
                //lineSegment.Point = new Point(size.Width, size.Height);
                //pathFigure.Segments.Add(lineSegment);
                //lineSegment = new LineSegment();
                //lineSegment.Point = new Point(0, size.Height);
                //pathFigure.Segments.Add(lineSegment);

                ////return pathGeometry;


                //if (_Path == null)
                //    _Path = string.Format(new System.Globalization.CultureInfo(1033), "M0,10 {2},10 {3},0 {4},10 L{0},10 {0}, {1}  0, {1} z", size.Width, size.Height, arrowPos, arrowPos + 10, arrowPos + 20);
                return _Path;
            }
        }


        internal void SetHostWindowSize(Size size)
        {
            int arrowPos = (int)(size.Width * 0.82);
            _Path = string.Format(new System.Globalization.CultureInfo(1033), "M0,10 {2},10 {3},0 {4},10 L{0},10 {0}, {1}  0, {1} z", size.Width, size.Height, arrowPos, arrowPos + 10, arrowPos + 20);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PathPoints)));
        }

        public double HorizontalOffset
        {
            get
            {
                int arrowPos = -(int)(PopupWitdh * 0.82) - 30;
                return arrowPos;

            }
        }
        /// <exclude>Excluded</exclude>
        double _PopupWitdh;
        public double PopupWitdh
        {
            get
            {

                return _PopupWitdh;
            }
            set
            {
                if (_PopupWitdh != value)
                {
                    //_PopupWitdh = value;
                    //_Path = null;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PathPoints)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PopupWitdh)));
                }
            }
        }

        /// <exclude>Excluded</exclude>
        double _PopupHeight;
        public double PopupHeight
        {
            get
            {
                return _PopupHeight;
            }
            set
            {
                if (_PopupHeight != value)
                {
                    //_PopupHeight = value;
                    //_Path = null;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PathPoints)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PopupHeight)));
                }
            }
        }


        //private StateEnum _State;
        //public StateEnum State
        //{
        //    get
        //    {
        //        return _State;
        //    }
        //    set
        //    {
        //        var oldValue = State;
        //        _State = value;
        //        if (oldValue != value)
        //        {

        //            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(State)));
        //        }
        //    }
        //}

        public delegate void PageSizeChangedHandle(object sender, Size newSize);
        public event EventHandler PageLoaded;
        public event PropertyChangedEventHandler PropertyChanged;

        public event PageSizeChangedHandle PageSizeChanged;

        public event AuthenticationChangeHandle SignedIn;
        public event AuthenticationChangeHandle SignedOut;

        [OOAdvantech.MetaDataRepository.HttpVisible]
        public void OnPageLoaded()
        {
            PageLoaded?.Invoke(this, EventArgs.Empty);
        }

        bool cancelResize = false;

        [OOAdvantech.MetaDataRepository.HttpVisible]
        public void OnPageSizeChanged(double width, double height)
        {
            if (cancelResize)
                return;
            Task.Run(() =>
            {

                Size newSize = new Size(width, height);
                _PopupWitdh = width;
                _PopupHeight = height;
                //_Path=null;
                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PathPoints)));
                if (!OnSignIn)
                    PageSizeChanged?.Invoke(this, newSize);
            });
        }

        class MessageData
        {
            public string Message;
        }

        [OOAdvantech.MetaDataRepository.HttpVisible]
        public async void TogglePopupView()
        {
            SwitchOnOffPopupView?.Invoke(this, EventArgs.Empty);

            OnPageLoaded();



            MessmateAdded?.Invoke(this, "Liakos");


        }

        [OOAdvantech.MetaDataRepository.HttpVisible]
        public string GetValue(int age)
        {
            return "232K " + age.ToString();
        }

        [OOAdvantech.MetaDataRepository.HttpVisible]
        public void SetMessage(string message)
        {

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



        string _PhotoUrl;
        public string PhotoUrl
        {
            get
            {
                return _PhotoUrl;
            }

            set
            {
                _PhotoUrl = value;
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


        [OOAdvantech.MetaDataRepository.HttpVisible]
        public void SignOut()
        {

            SignedOut?.Invoke(this, CurrentUser);
        }

        //static string _AzureServerUrl = "http://localhost:8090/api/";
        //static string _AzureServerUrl = "http://192.168.2.8:8090/api/";


        //static string _AzureServerUrl = "http://192.168.2.8:8090/api/";//org
        //static string _AzureServerUrl = "http://192.168.2.4:8090/api/";//Braxati
        //static string _AzureServerUrl = "http://10.0.0.13:8090/api/";//work
        static string _AzureServerUrl = string.Format("http://{0}:8090/api/", FlavourBusinessFacade.ComputingResources.EndPoint.Server);


        public static string AzureServerUrl
        {
            get
            {
                string azureStorageUrl = OOAdvantech.Remoting.RestApi.RemotingServices.GetLocalIPAddress();
                if (azureStorageUrl == null)
                    azureStorageUrl = _AzureServerUrl;
                else
                    azureStorageUrl = "http://" + azureStorageUrl + ":8090/api/";

                return azureStorageUrl;
            }
        }


        [OOAdvantech.MetaDataRepository.HttpVisible]
        public FlavourBusinessFacade.ViewModel.IUser GetObj()
        {
            return this;
        }


        [OOAdvantech.MetaDataRepository.HttpVisible]
        public void SetObj(FlavourBusinessFacade.ViewModel.IUser user, FlavourBusinessFacade.ViewModel.IUser user2)
        {

        }
        public IUser CurrentUser;

        public bool OnSignIn;

        Task<bool> SignInTask;
        [OOAdvantech.MetaDataRepository.HttpVisible]
        public async Task<bool> SignIn()
        {
            System.Diagnostics.Debug.WriteLine("public async Task< bool> SignIn()");
            AuthUser authUser = System.Runtime.Remoting.Messaging.CallContext.GetData("AutUser") as AuthUser;


            if (authUser == null)
            {


            }
            if (DeviceAuthentication.AuthUser == null)
            {

            }
            if(DeviceAuthentication.AuthUser!=null)
                this.PhotoUrl = DeviceAuthentication.AuthUser.Picture;

            if(authUser != null)
                this.PhotoUrl = authUser.Picture;


            if (OnSignIn && SignInTask != null)
                return await SignInTask;
            else
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
                         IAuthFlavourBusiness pAuthFlavourBusines = null;

                         try
                         {
                             pAuthFlavourBusines = OOAdvantech.Remoting.RestApi.RemotingServices.CreateRemoteInstance(serverUrl, type, assemblyData) as IAuthFlavourBusiness;
                         }
                         catch (System.Net.WebException error)
                         {
                             throw;
                         }
                         catch (Exception error)
                         {
                             throw;
                         }


                         var user = pAuthFlavourBusines.SignIn(RoleType);
                         CurrentUser = user;

                         if (user != null)
                             _PhoneNumber = user.PhoneNumber;
                         if (user is Organization)
                             _Address = (user as Organization).Address;
                         OnSignIn = false;

                         if (user == null)
                         {
                             var task = Task.Run(() =>
                             {
                                 var dispatcher = Application.Current != null ? Application.Current.Dispatcher : null;
                                 if (dispatcher == null || dispatcher.CheckAccess())
                                     TogglePopupView();
                                 else
                                     dispatcher.Invoke(() => TogglePopupView());
                             });
                             return false;
                         }

                         SignedIn?.Invoke(this, user);
                         OnPageSizeChanged(_PopupWitdh, _PopupHeight);
                         return true;

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
                return await SignInTask;
            }






        }

        private void Organization_ObjectChangeState(object _object, string member)
        {
            if (_object is IOrganization)
            {
                var adr = (_object as IOrganization).Address;
            }

        }

        [OOAdvantech.MetaDataRepository.HttpVisible]
        public bool SignUp()
        {
            string assemblyData = "FlavourBusinessManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
            string type = "FlavourBusinessManager.AuthFlavourBusiness";// typeof(FlavourBusinessManager.AuthFlavourBusiness).FullName;
            AuthUser authUser = System.Runtime.Remoting.Messaging.CallContext.GetData("AutUser") as AuthUser;
            string serverUrl = "http://localhost/FlavourBusinessWebApiRole/api/";
            serverUrl = "http://localhost:8090/api/";
            serverUrl = AzureServerUrl;
            IAuthFlavourBusiness pAuthFlavourBusines = OOAdvantech.Remoting.RestApi.RemotingServices.CreateRemoteInstance(serverUrl, type, assemblyData) as IAuthFlavourBusiness;
            var user = pAuthFlavourBusines.SignUp(new UserData() { Email = this.Email, FullName = this.FullName, Address = this.Address, PhoneNumber = this.PhoneNumber,PhotoUrl=this.PhotoUrl }, RoleType);

            SignedIn?.Invoke(this, user);
            return user != null;

        }
        [OOAdvantech.MetaDataRepository.HttpVisible]
        public void SaveUserProfile()
        {
            Task<bool>.Run(() =>
            {
                string assemblyData = "FlavourBusinessManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
                string type = "FlavourBusinessManager.AuthFlavourBusiness";// typeof(FlavourBusinessManager.AuthFlavourBusiness).FullName;
                AuthUser authUser = System.Runtime.Remoting.Messaging.CallContext.GetData("AutUser") as AuthUser;
                string serverUrl = "http://localhost/FlavourBusinessWebApiRole/api/";
                serverUrl = "http://localhost:8090/api/";
                serverUrl = AzureServerUrl;
                IAuthFlavourBusiness pAuthFlavourBusines = OOAdvantech.Remoting.RestApi.RemotingServices.CreateRemoteInstance(serverUrl, type, assemblyData) as IAuthFlavourBusiness;
                pAuthFlavourBusines.UpdateUserProfile(new UserData() { Email = this.Email, FullName = this.FullName, Address = this.Address, PhoneNumber = this.PhoneNumber,PhotoUrl=this.PhotoUrl }, RoleType);
            });
            //SwitchOnOffPopupView?.Invoke(this, EventArgs.Empty);

        }




        public string UserName { get; set; }
        [OOAdvantech.MetaDataRepository.HttpVisible]
        public string UserIdentity { get; set; }


        [OOAdvantech.MetaDataRepository.HttpVisible]
        public string SignInProvider { get; set; }

        public Point ArrowOffset
        {
            get
            {
                System.Windows.Size size = new System.Windows.Size(PopupWitdh, PopupHeight);
                int xPos = (int)(size.Width * 0.82) + 10;

                return new Point(xPos, 0);
            }
        }

    }
}
