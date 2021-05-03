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
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FLBManager.ViewModel.HumanResources
{
    /// <MetaDataID>{54bceb17-98f7-472b-b018-b84583976b7c}</MetaDataID>
    public class TranslatorViewModel : MarshalByRefObject, INotifyPropertyChanged
    {

        /// <MetaDataID>{f8f05580-9e48-4adf-b0ca-58a0fd258cf6}</MetaDataID>
        FlavourBusinessFacade.HumanResources.ITranslator Translator;

        CompanyPresentation CompanyPresentation;

        public System.Windows.Visibility TranslatorProfileVisibility { get; set; } = System.Windows.Visibility.Collapsed;
        public System.Windows.Visibility ActivitiesViewVisibility { get; set; } = System.Windows.Visibility.Collapsed;



        internal List<TranslationSubject> MenusTranslationSubjects;


        internal List<TranslationSubject> AppsTranslationSubjects;


        /// <MetaDataID>{dfbded82-a08f-4265-b221-c79fdad72907}</MetaDataID>
        public bool UserNameIsReadonly
        {
            get
            {
                return Translator != null;
            }
        }
        /// <MetaDataID>{39dc3897-e03a-4750-8bc9-0d56ef0216e0}</MetaDataID>
        public UserData UserData;
        /// <MetaDataID>{4d59dd2c-0e58-41b8-9212-89f9ac4102aa}</MetaDataID>
        public TranslatorViewModel(CompanyPresentation companyPresentation, FlavourBusinessFacade.HumanResources.ITranslator translator = null)
        {
            CompanyPresentation = companyPresentation;

            MenusTranslationSubjects = (from menu in companyPresentation.Menus.Members.OfType<MenuDesigner.ViewModel.GraphicMenuTreeNode>()
                                        select new TranslationSubject() { TranslationType = TranslationType.Menu, Description = menu.Name, SubjcectIdentity = menu.GraphicMenuStorageRef.StorageIdentity }).ToList();
            AppsTranslationSubjects = new List<TranslationSubject>() {
                new TranslationSubject() { TranslationType = TranslationType.Application, Description = "Don't wait manager app", SubjcectIdentity = "com.microneme.dontwaitmanager" },
                new TranslationSubject() { TranslationType = TranslationType.Application, Description = "Don't wait waiter app", SubjcectIdentity = "com.microneme.dontwaitwaiterapp" },
                new TranslationSubject() { TranslationType = TranslationType.Application, Description = "Don't wait app", SubjcectIdentity = "com.microneme.dontwait" }
            };

            Translator = translator;
            if (translator != null)
            {
                UserName = (translator as IUser).Email;
                FullName = translator.Name;
                Stream userImageStream = null;
                try
                {
                    string photoUrl= (translator as IUser).PhotoUrl;
                    if (!string.IsNullOrWhiteSpace((translator as IUser).PhotoUrl))
                    {
                        WebClient client = new WebClient();
                        userImageStream = client.OpenRead((translator as IUser).PhotoUrl);
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
                TranslatorProfileVisibility = System.Windows.Visibility.Visible;
                ActivitiesViewVisibility = System.Windows.Visibility.Visible;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ActivitiesViewVisibility)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UserImage)));


                //_Activities = (from translationActivity in translator.Activities.OfType<ITranslationActivity>()
                //               select new TranslationActivityPresentation(this, translationActivity)).ToList();



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
                if (translator != null)
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
                    if (UserData != null)
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
                    TranslatorProfileVisibility = System.Windows.Visibility.Visible;
                else
                    TranslatorProfileVisibility = System.Windows.Visibility.Collapsed;



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




                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TranslatorProfileVisibility)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FullName)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UserImage)));

                System.Diagnostics.Debug.WriteLine(UserName);

            });
        }

        internal List<TranslationSubject> GetAppsTranslationSubjects(TranslationSubject selectedTranslationSubject)
        {
            var appsTranslationSubjects = AppsTranslationSubjects.ToList();
            foreach (var activityPresentation in Activities)
            {
                if (appsTranslationSubjects.Contains(activityPresentation.SelectedTranslationSubject))
                    appsTranslationSubjects.Remove(activityPresentation.SelectedTranslationSubject);
            }
            if (selectedTranslationSubject != null && !appsTranslationSubjects.Contains(selectedTranslationSubject))
                appsTranslationSubjects.Add(selectedTranslationSubject);

            return appsTranslationSubjects;

        }

        internal List<TranslationSubject> GetMenusTranslationSubjects(TranslationSubject selectedTranslationSubject)
        {
            var menusTranslationSubjects = MenusTranslationSubjects.ToList();
            foreach (var activityPresentation in Activities)
            {
                if (menusTranslationSubjects.Contains(activityPresentation.SelectedTranslationSubject))
                    menusTranslationSubjects.Remove(activityPresentation.SelectedTranslationSubject);
            }
            if (selectedTranslationSubject!=null&&!menusTranslationSubjects.Contains(selectedTranslationSubject))
                menusTranslationSubjects.Add(selectedTranslationSubject);

            return menusTranslationSubjects;

        }

        internal void Save()
        {

            //var activities = (from translationActivity in Translator.Activities.OfType<ITranslationActivity>()
            //                  select translationActivity).ToList();

            //foreach (var activity in activities)
            //{
            //    if (Activities.Where(x => x.TranslationActivity == activity).FirstOrDefault() == null)
            //        Translator.RemoveTranslationActivity(activity);
            //}

            //foreach (var activity in Activities)
            //{
            //    if (activity.TranslationActivity != null)
            //        activity.Save();
            //    else
            //        this.Translator.NewTranslationActivity(activity.SubjectDescription, activity.SelectedTranslationType, activity.SelectedTranslationSubject.SubjcectIdentity);
            //}
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
                if (GetMenusTranslationSubjects(null).Count > 0 || GetAppsTranslationSubjects(null).Count > 0)
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

            _Activities.Add(new TranslationActivityPresentation(this, null));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Activities)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CanAddActivity)));

        }

        List<TranslationActivityPresentation> _Activities = new List<TranslationActivityPresentation>();

        public List<TranslationActivityPresentation> Activities
        {
            get
            {
                return _Activities.ToList();
            }
        }

        TranslationActivityPresentation _SelectedActivity;
        public TranslationActivityPresentation SelectedActivity
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



    }
}
