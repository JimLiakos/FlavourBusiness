using FlavourBusinessFacade;
using FlavourBusinessFacade.HumanResources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FLBManager.ViewModel.HumanResources
{
    /// <MetaDataID>{d916fc81-3e27-448b-b6ef-902b7faf83cd}</MetaDataID>
    public class MenuMakerViewModel : MarshalByRefObject, INotifyPropertyChanged
    {

        /// <MetaDataID>{f8f05580-9e48-4adf-b0ca-58a0fd258cf6}</MetaDataID>
        FlavourBusinessFacade.HumanResources.IMenuMaker MenuMaker;

        CompanyPresentation CompanyPresentation;

        public System.Windows.Visibility GraphicDesignerProfileVisibility { get; set; } = System.Windows.Visibility.Collapsed;
        public System.Windows.Visibility ActivitiesViewVisibility { get; set; } = System.Windows.Visibility.Collapsed;

        internal List<DesignSubject> MenusDesignSubjects;
        internal List<DesignSubject> StyleSheetSubjects;

        public MenuMakerViewModel()
        {

        }


        /// <MetaDataID>{dfbded82-a08f-4265-b221-c79fdad72907}</MetaDataID>
        public bool UserNameIsReadonly
        {
            get
            {
                return MenuMaker != null;
            }
        }

        public readonly IAccountability MenuMakingAccountability;

        /// <MetaDataID>{39dc3897-e03a-4750-8bc9-0d56ef0216e0}</MetaDataID>
        public UserData UserData;
        /// <MetaDataID>{4d59dd2c-0e58-41b8-9212-89f9ac4102aa}</MetaDataID>
        public MenuMakerViewModel(CompanyPresentation companyPresentation, IAccountability menuMakingAccountability = null)
        {
            MenuMakingAccountability = menuMakingAccountability;

            if (MenuMakingAccountability != null)
            {
                MenuMaker = MenuMakingAccountability.Responsible as IMenuMaker;
                if (MenuMaker == null)
                    throw new ArgumentException("menuMakingAccountability has not menu maker as responsible");
            }

            CompanyPresentation = companyPresentation;
            MenusDesignSubjects = (from menu in companyPresentation.Menus.Members.OfType<MenuDesigner.ViewModel.GraphicMenuTreeNode>()
                                   select new DesignSubject() { DesignSubjectType = DesignSubjectType.Menu, Description = menu.Name, SubjcectIdentity = menu.GraphicMenuStorageRef.StorageIdentity }).ToList();

            StyleSheetSubjects = MenuPresentationModel.MenuStyles.StyleSheet.StyleSheetRefs.Select(x => new DesignSubject() { DesignSubjectType = DesignSubjectType.Stylesheet, Description = x.Name, SubjcectIdentity = x.Uri }).ToList();
            //StyleSheetSubjects = new List<DesignSubject>() {
            //    new DesignSubject() { DesignSubjectType = DesignSubjectType.Stylesheet, Description = "Stylesheet Don't wait manager app", SubjcectIdentity = "com.microneme.dontwaitmanager" },
            //    new DesignSubject() { DesignSubjectType = DesignSubjectType.Stylesheet, Description = "Stylesheet Don't wait waiter app", SubjcectIdentity = "com.microneme.dontwaitwaiterapp" },
            //    new DesignSubject() { DesignSubjectType = DesignSubjectType.Stylesheet, Description = "Stylesheet Don't wait app", SubjcectIdentity = "com.microneme.dontwait" }
            //};

            if (MenuMaker != null)
            {
                UserName = (MenuMaker as IUser).Email;
                FullName = MenuMaker.Name;
                Stream userImageStream = null;
                try
                {
                    string photoUrl = (MenuMaker as IUser).PhotoUrl;
                    if (!string.IsNullOrWhiteSpace((MenuMaker as IUser).PhotoUrl))
                    {
                        WebClient client = new WebClient();
                        userImageStream = client.OpenRead((MenuMaker as IUser).PhotoUrl);
                    }
                }
                catch (Exception error)
                {
                }
                if (userImageStream != null)
                {
                    var imageSource = new BitmapImage();
                    imageSource.BeginInit();
                    imageSource.StreamSource = userImageStream;
                    imageSource.EndInit();
                    UserImage = imageSource;
                }
                else
                    UserImage = new BitmapImage(new Uri(@"pack://application:,,,/FLBManager;Component/Resources/Images/Metro/user.png"));
                GraphicDesignerProfileVisibility = System.Windows.Visibility.Visible;
                ActivitiesViewVisibility = System.Windows.Visibility.Visible;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ActivitiesViewVisibility)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UserImage)));


                _Activities = (from menuDesignActivity in MenuMakingAccountability.Activities.OfType<IMenuDesignActivity>()
                               select new GraphicDesignActivityPresentation(this, menuDesignActivity)).ToList();



            }


            DeleteSelectedActivityCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {
                DeleteSelectedActivity();
            });

            AddActivityCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {
                NewActivity();
            });


            SearchUserCommand = new WPFUIElementObjectBind.RelayCommand(async (object sender) =>
            {
                if (MenuMaker != null)
                    return;
                Stream userImageStream = null;
                await Task<UserData>.Run(() =>
                {
                    string assemblyData = "FlavourBusinessManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
                    string type = "FlavourBusinessManager.AuthFlavourBusiness";// typeof(FlavourBusinessManager.AuthFlavourBusiness).FullName;

                    string serverUrl = "http://localhost/FlavourBusinessWebApiRole/api/";
                    serverUrl = "http://localhost:8090/api/";
                    serverUrl = FLBAuthentication.ViewModel.SignInUserPopupViewModel.AzureServerUrl;
                    IAuthFlavourBusiness pAuthFlavourBusines = OOAdvantech.Remoting.RestApi.RemotingServices.CreateRemoteInstance(serverUrl, type, assemblyData) as IAuthFlavourBusiness;

                    UserData = pAuthFlavourBusines.GetUser(UserName);
                    if (UserData != null && UserData.GetRoleObject(RoleType.MenuMaker) != null)
                    {
                        FullName = UserData.FullName;
                        try
                        {
                            if (!string.IsNullOrWhiteSpace(UserData.PhotoUrl))
                            {
                                WebClient client = new WebClient();
                                userImageStream = client.OpenRead(UserData.PhotoUrl);
                            }
                        }
                        catch (Exception error)
                        {
                        }
                    }
                });
                if (UserData != null)
                    GraphicDesignerProfileVisibility = System.Windows.Visibility.Visible;
                else
                    GraphicDesignerProfileVisibility = System.Windows.Visibility.Collapsed;



                if (userImageStream != null)
                {
                    var imageSource = new BitmapImage();
                    imageSource.BeginInit();
                    imageSource.StreamSource = userImageStream;
                    imageSource.EndInit();
                    UserImage = imageSource;
                }
                else
                    UserImage = new BitmapImage(new Uri(@"pack://application:,,,/FLBManager;Component/Resources/Images/Metro/user.png"));




                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(GraphicDesignerProfileVisibility)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FullName)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UserImage)));

                System.Diagnostics.Debug.WriteLine(UserName);

            });
        }

        internal List<DesignSubject> GetStyleSheetSubjects(DesignSubject selectedTranslationSubject)
        {
            var styleSheetSubjects = StyleSheetSubjects.ToList();
            foreach (var activityPresentation in Activities)
            {
                if (styleSheetSubjects.Contains(activityPresentation.SelectedDesignSubject))
                    styleSheetSubjects.Remove(activityPresentation.SelectedDesignSubject);
            }
            if (selectedTranslationSubject != null && !styleSheetSubjects.Contains(selectedTranslationSubject))
                styleSheetSubjects.Add(selectedTranslationSubject);

            return styleSheetSubjects;

        }

        internal List<DesignSubject> GetMenusDesignSubjects(DesignSubject selectedDesignSubject)
        {
            var menusDesignSubjects = MenusDesignSubjects.ToList();
            foreach (var activityPresentation in Activities)
            {
                if (menusDesignSubjects.Contains(activityPresentation.SelectedDesignSubject))
                    menusDesignSubjects.Remove(activityPresentation.SelectedDesignSubject);
            }
            if (selectedDesignSubject != null && !menusDesignSubjects.Contains(selectedDesignSubject))
                menusDesignSubjects.Add(selectedDesignSubject);

            return menusDesignSubjects;

        }

        internal void Save()
        {
            var activities = (from menuDesignActivity in this.MenuMakingAccountability.Activities.OfType<IMenuDesignActivity>()
                              select menuDesignActivity).ToList();

            foreach (var activity in activities)
            {
                if (Activities.Where(x => x.MenuDesignActivity == activity).FirstOrDefault() == null)
                    this.MenuMakingAccountability.RemoveActivity(activity);
            }

            foreach (var activity in Activities)
            {
                if (activity.MenuDesignActivity != null)
                    activity.Save();
                else
                    this.MenuMaker.NewMenuDesignActivity(MenuMakingAccountability, activity.SubjectDescription, activity.SelectedDesignSubjectType, activity.SelectedDesignSubject.SubjcectIdentity);
            }
        }

        public bool CanDelete
        {
            get
            {
                return SelectedActivity != null;
            }
        }
        public bool CanAddActivity
        {
            get
            {
                if (GetMenusDesignSubjects(null).Count > 0 || GetStyleSheetSubjects(null).Count > 0)
                    return true;
                else
                    return false;
            }
        }
        private void DeleteSelectedActivity()
        {
            if (_SelectedActivity != null)
            {
                _Activities.Remove(_SelectedActivity);
                _SelectedActivity = null;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedActivity)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Activities)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CanAddActivity)));
            }
        }

        private void NewActivity()
        {

            _Activities.Add(new GraphicDesignActivityPresentation(this, null));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Activities)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CanAddActivity)));

        }

        List<GraphicDesignActivityPresentation> _Activities = new List<GraphicDesignActivityPresentation>();

        public List<GraphicDesignActivityPresentation> Activities
        {
            get
            {
                return _Activities.ToList();
            }
        }

        GraphicDesignActivityPresentation _SelectedActivity;
        public GraphicDesignActivityPresentation SelectedActivity
        {
            get => _SelectedActivity; set
            {
                _SelectedActivity = value;
                foreach (var activity in Activities)
                    activity.Focused = false;
                if (_SelectedActivity != null)
                    _SelectedActivity.Focused = true;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CanDelete)));
            }
        }

        /// <MetaDataID>{d33a7da0-94a0-4dc5-9566-c5e654424a68}</MetaDataID>
        public string UserName { get; set; }
        /// <MetaDataID>{01f259fb-f486-4d8a-a8e0-60bf21518ab3}</MetaDataID>
        public string FullName { get; set; }
        /// <MetaDataID>{970ecbaa-bee7-4e6b-b04e-bbd72a68f70e}</MetaDataID>
        public ImageSource UserImage { get; set; }



        public event PropertyChangedEventHandler PropertyChanged;

        /// <MetaDataID>{c5fbc828-06e5-463e-bb40-a1de4f78c655}</MetaDataID>
        public WPFUIElementObjectBind.RelayCommand SearchUserCommand { get; protected set; }

        public WPFUIElementObjectBind.RelayCommand DeleteSelectedActivityCommand { get; protected set; }

        public WPFUIElementObjectBind.RelayCommand AddActivityCommand { get; protected set; }

        public WPFUIElementObjectBind.RelayCommand RenameSelectedActivityCommand { get; protected set; }

        public WPFUIElementObjectBind.RelayCommand EditSelectedOptionCommand { get; protected set; }


        public bool IsEditable
        {
            get
            {
                return true;
            }
        }
    }
}
