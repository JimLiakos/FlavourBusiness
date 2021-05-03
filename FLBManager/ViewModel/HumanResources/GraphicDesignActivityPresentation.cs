using FlavourBusinessFacade.HumanResources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLBManager.ViewModel.HumanResources
{
    /// <MetaDataID>{9cbb5fa7-a2e0-4b18-b04a-bcec1a256af9}</MetaDataID>
    public class GraphicDesignActivityPresentation: MarshalByRefObject, INotifyPropertyChanged
    {
        public readonly IMenuDesignActivity MenuDesignActivity;

        MenuMakerViewModel MenuMakerViewModel;
        public GraphicDesignActivityPresentation(MenuMakerViewModel menuMakerViewModel, IMenuDesignActivity menuDesignActivity)
        {

            MenuMakerViewModel = menuMakerViewModel;

            MenuDesignActivity = menuDesignActivity;

            if (menuDesignActivity != null)
            {
                _SelectedDesignSubjectType = menuDesignActivity.DesignActivityType;
                if (_SelectedDesignSubjectType == DesignSubjectType.Menu)
                    this.SelectedDesignSubject = MenuMakerViewModel.MenusDesignSubjects.Where(x => x.SubjcectIdentity == menuDesignActivity.DesigneSubjectIdentity).FirstOrDefault();
                else
                    this.SelectedDesignSubject = MenuMakerViewModel.StyleSheetSubjects.Where(x => x.SubjcectIdentity == menuDesignActivity.DesigneSubjectIdentity).FirstOrDefault();
                SubjectDescription = menuDesignActivity.Name;
            }
            else
            {
                if (MenuMakerViewModel.GetMenusDesignSubjects(null).Count != 0)
                {
                    this._SelectedDesignSubjectType = DesignSubjectType.Menu;
                    this.SelectedDesignSubject = MenuMakerViewModel.GetMenusDesignSubjects(null).FirstOrDefault();
                }
                else
                {
                    this._SelectedDesignSubjectType = DesignSubjectType.Stylesheet;
                    this.SelectedDesignSubject = MenuMakerViewModel.GetStyleSheetSubjects(null).FirstOrDefault();
                }
                SubjectDescription = "Design";
            }


        }

        public bool IsEditable
        {
            get
            {
                return true;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        DesignSubjectType _SelectedDesignSubjectType;
        public DesignSubjectType SelectedDesignSubjectType
        {
            get => _SelectedDesignSubjectType;
            set
            {
                if (_SelectedDesignSubjectType != value)
                {
                    DesignSubject selectedTranslationSubject = null;
                    if (value == DesignSubjectType.Menu)
                        selectedTranslationSubject = MenuMakerViewModel.GetMenusDesignSubjects(null).FirstOrDefault();
                    else if (value == DesignSubjectType.Stylesheet && MenuMakerViewModel.StyleSheetSubjects.Count > 0)
                        selectedTranslationSubject = MenuMakerViewModel.GetStyleSheetSubjects(null).FirstOrDefault();

                    if (selectedTranslationSubject != null)
                    {
                        _SelectedDesignSubjectType = value;
                        SelectedDesignSubject = selectedTranslationSubject;
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DesignSubjects)));
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedDesignSubject)));
                    }
                    else
                    {
                        var taskc = Task.Run(() =>
                        {

                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedDesignSubjectType)));

                        });
                    }


                }
            }
        }

        internal void Save()
        {
            if (MenuDesignActivity != null)
            {
                if (MenuDesignActivity.Name != SubjectDescription)
                    MenuDesignActivity.Name = SubjectDescription;
                if (MenuDesignActivity.DesignActivityType != SelectedDesignSubjectType)
                    MenuDesignActivity.DesignActivityType = SelectedDesignSubjectType;
                if (MenuDesignActivity.DesigneSubjectIdentity != SelectedDesignSubject.SubjcectIdentity)
                    MenuDesignActivity.DesigneSubjectIdentity = SelectedDesignSubject.SubjcectIdentity;
            }
        }

        public DesignSubject SelectedDesignSubject { get; set; }

        string SelectedTranslationSubjectIdentity { get; set; }

        public string SubjectDescription { get; set; }


        public List<DesignSubject> DesignSubjects
        {
            get
            {
                if (SelectedDesignSubjectType == DesignSubjectType.Menu)
                {
                    return MenuMakerViewModel.GetMenusDesignSubjects(SelectedDesignSubject);
                }
                else
                    return MenuMakerViewModel.GetStyleSheetSubjects(SelectedDesignSubject);
            }
        }

        public System.Windows.Visibility EditControlVisible { get; set; } = System.Windows.Visibility.Hidden;

        public System.Windows.Visibility LabelControlVisible { get; set; } = System.Windows.Visibility.Visible;

        public List<DesignSubjectType> DesignSubjectTypes { get; set; } = new List<DesignSubjectType>() { DesignSubjectType.Menu, DesignSubjectType.Stylesheet };

        bool _Focused;
        public bool Focused
        {
            get => _Focused; internal set
            {
                _Focused = value;
                if (_Focused)
                {
                    LabelControlVisible = System.Windows.Visibility.Hidden;
                    EditControlVisible = System.Windows.Visibility.Visible;
                }
                else
                {
                    LabelControlVisible = System.Windows.Visibility.Visible;
                    EditControlVisible = System.Windows.Visibility.Hidden;
                }

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LabelControlVisible)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EditControlVisible)));

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedDesignSubject)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedDesignSubjectType)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LineThickness)));
            }
        }


        public System.Windows.Thickness LineThickness
        {
            get
            {
                if (_Focused)
                    return new System.Windows.Thickness(0);
                else
                    return new System.Windows.Thickness(0, 0, 0, 1);
            }
        }
    }
}
