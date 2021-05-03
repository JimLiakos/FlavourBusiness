using FlavourBusinessFacade.HumanResources;
using FlavourBusinessManager.HumanResources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLBManager.ViewModel.HumanResources
{
    /// <MetaDataID>{8a3e850c-9e4a-4faf-affe-189304652f6d}</MetaDataID>
    public class TranslationActivityPresentation : MarshalByRefObject, INotifyPropertyChanged
    {

        public readonly ITranslationActivity TranslationActivity;

        TranslatorViewModel TranslatorViewModel;
        public TranslationActivityPresentation(TranslatorViewModel translatorViewModel, ITranslationActivity translationActivity)
        {
            
            TranslatorViewModel = translatorViewModel;

            TranslationActivity = translationActivity;

            if (translationActivity != null)
            {
                _SelectedTranslationType = translationActivity.TranslationType;
                if (_SelectedTranslationType == TranslationType.Menu)
                    this.SelectedTranslationSubject = TranslatorViewModel.MenusTranslationSubjects.Where(x => x.SubjcectIdentity == translationActivity.TranslationIdentity).FirstOrDefault();
                else
                    this.SelectedTranslationSubject = TranslatorViewModel.AppsTranslationSubjects.Where(x => x.SubjcectIdentity == translationActivity.TranslationIdentity).FirstOrDefault();


                SubjectDescription = translationActivity.Name;
            }
            else
            {
                if (TranslatorViewModel.GetMenusTranslationSubjects(null).Count != 0)
                {
                    this._SelectedTranslationType = TranslationType.Menu;
                    this.SelectedTranslationSubject = TranslatorViewModel.GetMenusTranslationSubjects(null).FirstOrDefault();
                }
                else
                {
                    this._SelectedTranslationType = TranslationType.Application;
                    this.SelectedTranslationSubject = TranslatorViewModel.GetAppsTranslationSubjects(null).FirstOrDefault();
                }
                SubjectDescription = "Translation";
            }

            
        }

        public event PropertyChangedEventHandler PropertyChanged;

        TranslationType _SelectedTranslationType;
        public TranslationType SelectedTranslationType
        {
            get => _SelectedTranslationType;
            set
            {
                if (_SelectedTranslationType != value)
                {
                    TranslationSubject selectedTranslationSubject = null;
                    if (value == TranslationType.Menu)
                        selectedTranslationSubject = TranslatorViewModel.GetMenusTranslationSubjects(null).FirstOrDefault();
                    else if (value == TranslationType.Application && TranslatorViewModel.AppsTranslationSubjects.Count > 0)
                        selectedTranslationSubject = TranslatorViewModel.GetAppsTranslationSubjects(null).FirstOrDefault();

                    if (selectedTranslationSubject != null)
                    {
                        _SelectedTranslationType = value;
                        SelectedTranslationSubject = selectedTranslationSubject;
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TranslationSubjects)));
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedTranslationSubject)));
                    }
                    else
                    {
                        var taskc = Task.Run(() =>
                        {

                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedTranslationType)));

                        });
                    }


                }
            }
        }

        internal void Save()
        {
            if (TranslationActivity != null)
            {
                if (TranslationActivity.Name != SubjectDescription)
                    TranslationActivity.Name = SubjectDescription;
                if (TranslationActivity.TranslationType != SelectedTranslationType)
                    TranslationActivity.TranslationType = SelectedTranslationType;
                if (TranslationActivity.TranslationIdentity != SelectedTranslationSubject.SubjcectIdentity)
                    TranslationActivity.TranslationIdentity = SelectedTranslationSubject.SubjcectIdentity;
            }
        }

        public TranslationSubject SelectedTranslationSubject { get; set; }

        string SelectedTranslationSubjectIdentity { get; set; }

        public string SubjectDescription { get; set; }


        public List<TranslationSubject> TranslationSubjects
        {
            get
            {
                if (SelectedTranslationType == TranslationType.Menu)
                {
                    return TranslatorViewModel.GetMenusTranslationSubjects(SelectedTranslationSubject);
                }
                else
                    return TranslatorViewModel.GetAppsTranslationSubjects(SelectedTranslationSubject);
            }
        }

        public System.Windows.Visibility EditControlVisible { get; set; } = System.Windows.Visibility.Hidden;

        public System.Windows.Visibility LabelControlVisible { get; set; } = System.Windows.Visibility.Visible;

        public List<TranslationType> TranslationTypes { get; set; } = new List<TranslationType>() { TranslationType.Menu, TranslationType.Application };

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

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedTranslationSubject)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedTranslationType)));
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
                    return new System.Windows.Thickness(0,0,0,1);
            }
        }


        public  bool IsEditable
        {
            get
            {
                return true;
            }
        }

    }
}
