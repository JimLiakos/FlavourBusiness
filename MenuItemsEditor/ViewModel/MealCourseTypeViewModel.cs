using MenuModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFUIElementObjectBind;

namespace MenuItemsEditor.ViewModel
{
    /// <MetaDataID>{1ce07638-b786-4103-ba67-5fba90068d5c}</MetaDataID>
    public class MealCourseTypeViewModel : MarshalByRefObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        public readonly IMealCourseType MealCourseType;
        public MealCourseTypeViewModel(IMealCourseType mealCourseType)
        {
            MealCourseType = mealCourseType;
        }

        public string Name
        {
            get
            {
                return MealCourseType.Name;
            }
            set
            {
                MealCourseType.Name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UnTranslatedName)));
            }
        }


        public double DurationInMinutes
        {
            get => MealCourseType.DurationInMinutes;
            set => MealCourseType.DurationInMinutes = value;
        }

        /// <exclude>Excluded</exclude>
        ITranslator _Translator;
        public ITranslator Translator
        {
            get
            {
                if (_Translator == null)
                    _Translator = new Translator();
                return _Translator;
            }
        }


        /// <MetaDataID>{cabfae52-13cc-4bf2-acf6-027199ca8331}</MetaDataID>
        public bool UnTranslatedName
        {
            get
            {
                string name = Name;
                using (OOAdvantech.CultureContext cultureContext = new OOAdvantech.CultureContext(OOAdvantech.CultureContext.CurrentCultureInfo, false))
                {
                    return string.IsNullOrWhiteSpace( Name);
                }
            }
        }

        /// <exclude>Excluded</exclude>
        bool _Edit;

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
    }
}
