using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuDesigner.ViewModel.MenuCanvas
{

    /// <summary>
    /// Define heading presentation in a ListView
    /// </summary>
    /// <MetaDataID>{1c7526f8-3da8-4f56-9903-74ceee495a8c}</MetaDataID>
    public class ListMenuHeadingPresentation : MarshalByRefObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public readonly MenuPresentationModel.RestaurantMenu RestaurantMenu;
        public readonly MenuPresentationModel.MenuCanvas.IMenuCanvasHeading Heading;
        public ListMenuHeadingPresentation(MenuPresentationModel.RestaurantMenu restaurantMenu, MenuPresentationModel.MenuCanvas.IMenuCanvasHeading heading)
        {
            RestaurantMenu = restaurantMenu;
            Heading = heading;
            Heading.ObjectChangeState += Heading_ObjectChangeState;
        }

        private void Heading_ObjectChangeState(object _object, string member)
        {
            if(member==nameof(MenuPresentationModel.MenuCanvas.IMenuCanvasHeading.Page))
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ImageUri)));

            if (member == nameof(MenuPresentationModel.MenuCanvas.IMenuCanvasHeading.Description))
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Description)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UnTranslated)));
            }

        }



        public string Description
        {
            get
            {
                return Heading.Description;
            }
            set
            {
                Heading.Description = value;
            }
        }
        WPFUIElementObjectBind.ITranslator _Translator;
        public WPFUIElementObjectBind.ITranslator Translator
        {
            get
            {
                if (_Translator == null)
                    _Translator = new MenuItemsEditor.Translator();

                return _Translator;
            }
        }
        public bool UnTranslated
        {
            get
            {
                string name = Description;

                using (OOAdvantech.CultureContext cultureContext = new OOAdvantech.CultureContext(OOAdvantech.CultureContext.CurrentCultureInfo, false))
                {
                    return name != Description;
                }

                //return true;
            }
        }

        ///// <exclude>Excluded</exclude>
        //bool _IsEditing = false;
        //public bool IsEditing
        //{
        //    get
        //    {
        //        return _IsEditing;
        //    }
        //    set
        //    {
        //        _IsEditing = value;
        //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsEditing)));
        //    }
        //}
        public bool IsEditable
        {
            get
            {
                return true;
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

        public void EditMode()
        {
            Edit = true;
            return;
            if (_Edit == true)
            {
                _Edit = !_Edit;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Edit)));
            }
            _Edit = true;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Edit)));
        }

        internal void Remove()
        {
            Heading.ObjectChangeState -= Heading_ObjectChangeState;
        }

        public Uri ImageUri
        {
            get
            {
                if(Heading.Page!=null)
                    return new Uri(@"pack://application:,,,/MenuDesignerLib;Component/Resources/Images/Metro/DocumentHeader.png");
                else
                    return new Uri(@"pack://application:,,,/MenuDesignerLib;Component/Resources/Images/Metro/DocumentHeaderAdd.png");

            }
        }
    }
}

