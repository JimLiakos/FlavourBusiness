using MenuModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIBaseEx;
using WPFUIElementObjectBind;

namespace MenuItemsEditor.ViewModel
{
    /// <MetaDataID>{e00bc8a7-cfff-4332-9cf3-5e84417b8282}</MetaDataID>
    public class MealTypeViewModel : MarshalByRefObject, System.ComponentModel.INotifyPropertyChanged
    {



        /// <MetaDataID>{14cba6ab-f2ee-4e47-90d5-a6813c732e41}</MetaDataID>
        public RelayCommand DeleteSelectedMealCourseTypeCommand { get; protected set; }

        /// <MetaDataID>{826dfd5d-b938-446b-83d5-a84d6f5b80cc}</MetaDataID>
        public RelayCommand AddMealCourseTypeCommand { get; protected set; }



        /// <MetaDataID>{0eab4458-3979-48b8-95f1-2d85c78cdefb}</MetaDataID>
        public RelayCommand MoveUpSelectedMealCourseTypeCommand { get; protected set; }

        public RelayCommand MakeSelectedSelectedMealCourseTypeDefaultCommand { get; protected set; }




        /// <MetaDataID>{0636cadc-a418-479f-bd1a-dcc2799eb723}</MetaDataID>
        public RelayCommand MoveDownSelectedMealCourseTypeCommand { get; protected set; }

        /// <MetaDataID>{63adcc6d-3f4b-4588-89eb-52dff2251bfb}</MetaDataID>
        public RelayCommand RenameSelectedMealCourseTypeCommand { get; protected set; }


        public event PropertyChangedEventHandler PropertyChanged;

        /// <MetaDataID>{ce000b22-2309-4259-b893-b3f6fc4dd07e}</MetaDataID>
        public readonly MealType MealType;

        /// <MetaDataID>{4d05e7e3-2ac5-4411-9448-d5dd5093b529}</MetaDataID>
        public MealTypeViewModel(MealType mealType)
        {
            MealType = mealType;

            MakeSelectedSelectedMealCourseTypeDefaultCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
             {
                 if (MealType != null)
                 {
                     foreach (MealCourseTypeViewModel mealCourseTypeViewModel in Courses)
                         mealCourseTypeViewModel.IsDefault = false;
                 }
                 SelectedCourse.IsDefault = true;


                 //int pos = MealType.Courses.IndexOf(SelectedCourse.MealCourseType);
                 //if (pos > 0)
                 //    MealType.MoveMealCourseType(SelectedCourse.MealCourseType, pos - 1);

                 //_Courses = null;
                 //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Courses)));

             }, (object sender) => IsSelectedMealCourseDefault);


            MoveUpSelectedMealCourseTypeCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
             {
                 int pos = MealType.Courses.IndexOf(SelectedCourse.MealCourseType);
                 if (pos > 0)
                     MealType.MoveMealCourseType(SelectedCourse.MealCourseType, pos - 1);

                 _Courses = null;
                 PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Courses)));

             }, (object sender) => CanBeSelectedMealCourseTypeMoveUp);

            MoveDownSelectedMealCourseTypeCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {
                int pos = MealType.Courses.IndexOf(SelectedCourse.MealCourseType);
                if (pos < MealType.Courses.Count - 2)
                    MealType.MoveMealCourseType(SelectedCourse.MealCourseType, pos + 1);

                _Courses = null;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Courses)));

            }, (object sender) => CanBeSelectedMealCourseTypeMoveDown);

            DeleteSelectedMealCourseTypeCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
             {
                 if (mealType is FixedMealType)
                     return;
                 if (SelectedCourse != null)
                 {
                     _Courses.Remove(SelectedCourse);
                     mealType.RemoveMealCourseType(SelectedCourse.MealCourseType);
                     foreach (var mealCourse in Courses)
                         mealCourse.IsDefault = mealCourse.MealCourseType.IsDefault;

                     PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Courses)));
                 }

             }, (object sender) => !(MealType is MenuModel.FixedMealType));

            AddMealCourseTypeCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {
                var newMealCourseType = mealType.NewMealCourseType();

                if (_Courses == null)
                    _Courses = (from mealCourseType in MealType.Courses select MealCourseTypeDictionary.GetViewModelFor(mealCourseType, mealCourseType)).ToList();
                else
                    _Courses.Add(MealCourseTypeDictionary.GetViewModelFor(newMealCourseType, newMealCourseType));

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Courses)));

            }, (object sender) => !(MealType is MenuModel.FixedMealType));

            RenameSelectedMealCourseTypeCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {
                SelectedCourse.Edit = true;
            }, (object sender) => SelectedCourse != null);

        }

        /// <MetaDataID>{5d1097be-2ec1-4acb-8434-c389fd7a394c}</MetaDataID>
        public bool CanBeSelectedMealCourseTypeMoveUp
        {
            get
            {
                if (SelectedCourse == null)
                    return false;
                if (this.MealType == null || MealType is MenuModel.FixedMealType)
                    return false;
                int pos = this.MealType.Courses.IndexOf(SelectedCourse.MealCourseType);
                if (pos == -1 || pos == 0)
                    return false;
                return true;
            }
        }


        public bool IsSelectedMealCourseDefault
        {
            get
            {
                if (SelectedCourse == null)
                    return false;
                if (this.MealType == null)
                    return false;
                int pos = this.MealType.Courses.IndexOf(SelectedCourse.MealCourseType);
                if (pos == -1)
                    return false;

                if (SelectedCourse.MealCourseType.IsDefault)
                    return false;

                return true;
            }
        }

        /// <MetaDataID>{58b2afd3-66a9-44bf-bf68-62054eef1434}</MetaDataID>
        bool CanBeSelectedMealCourseTypeMoveDown
        {
            get
            {
                if (SelectedCourse == null)
                    return false;
                if (this.MealType == null || MealType is MenuModel.FixedMealType)
                    return false;
                int pos = this.MealType.Courses.IndexOf(SelectedCourse.MealCourseType);
                if (pos == -1 || pos + 1 == this.MealType.Courses.Count)
                    return false;
                return true;

            }
        }

        /// <MetaDataID>{72400700-2fd0-4555-ae3e-3dea6a1337d9}</MetaDataID>
        public string Name
        {
            get
            {
                return MealType.Name;
            }
            set
            {
                MealType.Name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UnTranslatedName)));
            }
        }
        /// <MetaDataID>{14dfb665-fbea-4072-aca2-69e919b65b50}</MetaDataID>
        public bool UnTranslatedName
        {
            get
            {
                string name = Name;
                using (OOAdvantech.CultureContext cultureContext = new OOAdvantech.CultureContext(OOAdvantech.CultureContext.CurrentCultureInfo, false))
                {
                    return Name == null;
                }
            }
        }

        /// <exclude>Excluded</exclude>
        ITranslator _Translator;
        /// <MetaDataID>{5c69f72d-81a6-405c-b90f-11c6da8385da}</MetaDataID>
        public ITranslator Translator
        {
            get
            {
                if (_Translator == null)
                    _Translator = new Translator();
                return _Translator;
            }
        }


        /// <exclude>Excluded</exclude>
        bool _Edit;

        /// <MetaDataID>{a1a7becb-4c3f-4fd5-9f2c-e9c65c6d3bcc}</MetaDataID>
        public bool Edit
        {
            get
            {
                return _Edit;
            }
            set
            {
                _Edit = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Edit)));
            }
        }

        /// <MetaDataID>{cd287526-e784-4063-b4c1-3e2949293508}</MetaDataID>
        MealCourseTypeViewModel _SelectedCourse;
        /// <MetaDataID>{b49e8ca0-3471-49d8-a8b9-d8a041bd4b34}</MetaDataID>
        public MealCourseTypeViewModel SelectedCourse
        {
            get
            {
                return _SelectedCourse;
            }
            set
            {
                _SelectedCourse = value;
            }
        }
        /// <MetaDataID>{9ef45140-6899-411c-9f41-cf6f56f3ba93}</MetaDataID>
        List<MealCourseTypeViewModel> _Courses = null;
        /// <MetaDataID>{237eab09-7953-419f-883d-de2080bbf14e}</MetaDataID>
        readonly ViewModelWrappers<MenuModel.IMealCourseType, MealCourseTypeViewModel> MealCourseTypeDictionary = new ViewModelWrappers<IMealCourseType, MealCourseTypeViewModel>();

        /// <MetaDataID>{814a7d45-f2c3-43a4-b3ea-c4a1375630b3}</MetaDataID>
        public List<MealCourseTypeViewModel> Courses
        {
            get
            {
                if (_Courses == null)
                    _Courses = (from mealCourseType in MealType.Courses select MealCourseTypeDictionary.GetViewModelFor(mealCourseType, mealCourseType)).ToList();
                return _Courses;
            }
        }


    }
}
