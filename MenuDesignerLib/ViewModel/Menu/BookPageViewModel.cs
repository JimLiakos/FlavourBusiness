using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml.Linq;
using System.Windows.Markup;
using System.Windows.Controls;
using System.Xml;
using System.Globalization;
using System.IO;
using MenuPresentationModel;
using MenuPresentationModel.MenuCanvas;
using MenuPresentationModel.MenuStyles;
using System.Windows;
using System.Windows.Media;
using OOAdvantech.Transactions;
using SharpVectors.Renderers.Wpf;
using SharpVectors.Converters;

namespace MenuDesigner.ViewModel.MenuCanvas
{
    /// <MetaDataID>MenuDesigner.BookPageViewModel</MetaDataID>

    public class BookPageViewModel : MarshalByRefObject, INotifyPropertyChanged
    {
        public event OOAdvantech.ObjectChangeStateHandle ObjectChangeState;

        List<AccentImagePresentation> AccentImagesSlots = new List<AccentImagePresentation>();
        List<MenuCanvasItemTextViewModel> TextCanvasItemsSlots = new List<MenuCanvasItemTextViewModel>();
        List<CanvasFrameViewModel> CanvasFramesSlots = new List<CanvasFrameViewModel>();
        List<PriceHeadingViewModel> PriceHedingsSlots = new List<PriceHeadingViewModel>();
        readonly List<ICanvasItem> CanvasItemsSlots = new List<ICanvasItem>();

        List<MenuCanvasLinePresentation> Lines = new List<MenuCanvasLinePresentation>();

        List<MenuCanvasLinePresentation> TranslationLines = new List<MenuCanvasLinePresentation>();


        //List<ICanvasItem> CanvasItems = new List<ICanvasItem>();
        /// <MetaDataID>{58359c45-bb43-4cc6-888a-befd2673e38c}</MetaDataID>
        public List<ICanvasItem> ItemsToShowInCanvas
        {
            get
            {
                //if(CanvasItems==null|| CanvasItems.Count<10)
                //{

                //}

                return CanvasItemsSlots;

                //return (from canvasText in MenuPage.MenuCanvasItems
                // select new ViewModel.MenuCanvasItemViewModel(canvasText)).ToList();
            }
        }

        internal void UpdateCanvasItems(MenuPage menuPage)
        {
            this._MenuPage = menuPage;
            OOAdvantech.Transactions.Transaction transaction = OOAdvantech.Transactions.Transaction.Current;
            var culture = OOAdvantech.CultureContext.CurrentCultureInfo;
            var useDefaultCultureValue = OOAdvantech.CultureContext.UseDefaultCultureValue;

            Application.Current.Dispatcher.Invoke(new Action(() =>
            {

                using (var cultureConext = new OOAdvantech.CultureContext(culture, useDefaultCultureValue))
                {

                    if (transaction == null)
                        ReAssigCanvasItems();
                    else
                    {
                        using (SystemStateTransition suppressStateTransition = new SystemStateTransition(TransactionOption.Suppress))
                        {
                            transaction.TransactionCompleted += OnTransactionCompleted;
                            using (SystemStateTransition systemStateTransition = new SystemStateTransition(transaction))
                            {
                                ReAssigCanvasItems();
                                systemStateTransition.Consistent = true;
                            }
                            suppressStateTransition.Consistent = true;
                        }
                    }
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PageHeight)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PageWidth)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BackgroundUri)));
                }
            }));
        }

        bool ForceCanvasItemsUpdate;

        private void OnTransactionCompleted(Transaction transaction)
        {
            ForceCanvasItemsUpdate = true;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// </remarks>
        private void ReAssigCanvasItems()
        {

            ReAssignTextItems();

            ReAssignPricesHeadings();

            ReAssignAccent();

            ReAssignSeparationLines();
            ReAssignTranslationLines();

            ReAssignCanvasFrames();


            #region Removed code
            //foreach (var menuCanvasItem in MenuPage.MenuCanvasItems)
            //{
            //    if ((menuCanvasItem is IMenuCanvasHeading) && (menuCanvasItem as IMenuCanvasHeading).Accent != null)
            //    {
            //        int i = 0;
            //        foreach (var accentImage in (menuCanvasItem as IMenuCanvasHeading).Accent.Accent.AccentImages)
            //        {
            //            if (accentImagesDictionary.ContainsKey(new AccentImageKey((menuCanvasItem as IMenuCanvasHeading).Accent, i)))
            //            {
            //                AccentImagePresentation existingAccentImageVM = accentImagesDictionary[new AccentImageKey((menuCanvasItem as IMenuCanvasHeading).Accent, i)];
            //                existingAccentImageVM.Visibility = Visibility.Visible;
            //                existingAccentImageVM.ChangeAccent((menuCanvasItem as IMenuCanvasHeading).Accent, i);
            //            }
            //            else if (freeAccentImages.Count > 0)
            //            {
            //                AccentImagePresentation accentImageVM = freeAccentImages[0];
            //                freeAccentImages.RemoveAt(0);
            //                accentImageVM.Visibility = Visibility.Visible;
            //                accentImageVM.ChangeAccent((menuCanvasItem as IMenuCanvasHeading).Accent, i);
            //            }
            //            else
            //            {
            //                AccentImagePresentation newAccentImageVM = new AccentImagePresentation((menuCanvasItem as IMenuCanvasHeading).Accent, i);
            //                AccentImages.Add(newAccentImageVM);
            //            }
            //            i++;
            //        }
            //        if ((menuCanvasItem as IMenuCanvasHeading).Accent.Accent.AccentImages.Count == 0)
            //        {
            //            while ((menuCanvasItem as IMenuCanvasHeading).Accent == null || accentImagesDictionary.ContainsKey(new AccentImageKey((menuCanvasItem as IMenuCanvasHeading).Accent, i)))
            //            {
            //                accentImagesDictionary[new AccentImageKey((menuCanvasItem as IMenuCanvasHeading).Accent, i)].Visibility = Visibility.Collapsed;
            //                i++;
            //            }
            //        }
            //    }
            //}
            //foreach (var accentImageVM in freeAccentImages)
            //    accentImageVM.Visibility = Visibility.Hidden;
            #endregion


            #region Removed code

            //foreach (var menuCanvasItem in MenuPage.MenuCanvasItems)
            //{
            //    if (menuCanvasItem is IMenuCanvasFoodItem)
            //    {

            //        #region Remove Code
            //        //if ((menuCanvasItem as IMenuCanvasFoodItem).Prices.Count > 0)
            //        //{
            //        //    foreach (var menuCanvasItemPrice in (menuCanvasItem as IMenuCanvasFoodItem).Prices)
            //        //    {
            //        //        if (menuCanvasItemPrice.Visisble)
            //        //        {
            //        //            if (textCanvasItems.ContainsKey(menuCanvasItemPrice))
            //        //            {
            //        //                MenuCanvasItemTextViewModel textCanvasItem = textCanvasItems[menuCanvasItemPrice];

            //        //                textCanvasItem.Visibility = Visibility.Visible;
            //        //                textCanvasItem.Refresh();
            //        //            }
            //        //            else if (freeTextCanvasItems.Count > 0)
            //        //            {
            //        //                MenuCanvasItemTextViewModel textCanvasItem = freeTextCanvasItems[0];
            //        //                freeTextCanvasItems.RemoveAt(0);
            //        //                textCanvasItem.Visibility = Visibility.Visible;
            //        //                textCanvasItem.ChangeCanvasItem(menuCanvasItemPrice);
            //        //            }
            //        //            else
            //        //            {
            //        //                MenuCanvasItemTextViewModel textCanvasItem = new MenuCanvasItemTextViewModel(menuCanvasItemPrice);
            //        //                TextCanvasItems.Add(textCanvasItem);
            //        //            }
            //        //        }

            //        //    }

            //        //    if ((menuCanvasItem as IMenuCanvasFoodItem).MultiPriceHeading != null)
            //        //    {
            //        //        foreach (var menuCanvasItemPriceHeading in (menuCanvasItem as IMenuCanvasFoodItem).MultiPriceHeading.PriceHeadings)
            //        //        {

            //        //            if (priceHeadings.ContainsKey(menuCanvasItemPriceHeading))
            //        //            {
            //        //                PriceHeadingViewModel priceHeadingVM = priceHeadings[menuCanvasItemPriceHeading];

            //        //                priceHeadingVM.Visibility = Visibility.Visible;
            //        //                priceHeadingVM.Refresh();
            //        //            }
            //        //            else if (freePriceHeadings.Count > 0)
            //        //            {
            //        //                PriceHeadingViewModel priceHeadingVM = freePriceHeadings[0];
            //        //                freePriceHeadings.RemoveAt(0);
            //        //                priceHeadingVM.Visibility = Visibility.Visible;
            //        //                priceHeadingVM.ChangeCanvasItem(menuCanvasItemPriceHeading);
            //        //                priceHeadings[menuCanvasItemPriceHeading] = priceHeadingVM;
            //        //            }
            //        //            else
            //        //            {
            //        //                PriceHeadingViewModel priceHeadingVM = new PriceHeadingViewModel(menuCanvasItemPriceHeading);
            //        //                PriceHedings.Add(priceHeadingVM);
            //        //                priceHeadings[menuCanvasItemPriceHeading] = priceHeadingVM;
            //        //            }

            //        //        }

            //        //        #region Set canvas frame for MultiPriceHeading
            //        //        if (!priceHeadingsCanvasFrame.ContainsKey((menuCanvasItem as IMenuCanvasFoodItem).MultiPriceHeading))
            //        //        {
            //        //            if (canvasFrames.Count > 0)
            //        //            {
            //        //                CanvasFrameViewModel canvasFrameViewModel = canvasFrames[0];
            //        //                canvasFrames.RemoveAt(0);
            //        //                canvasFrameViewModel.Visibility = Visibility.Visible;
            //        //                canvasFrameViewModel.ChangeCanvasItem((menuCanvasItem as IMenuCanvasFoodItem).MultiPriceHeading);
            //        //                priceHeadingsCanvasFrame[(menuCanvasItem as IMenuCanvasFoodItem).MultiPriceHeading] = canvasFrameViewModel;
            //        //            }
            //        //            else if (freeCanvasFrames.Count > 0)
            //        //            {
            //        //                CanvasFrameViewModel canvasFrameViewModel = freeCanvasFrames[0];
            //        //                freeTextCanvasItems.RemoveAt(0);
            //        //                canvasFrameViewModel.Visibility = Visibility.Visible;
            //        //                canvasFrameViewModel.ChangeCanvasItem((menuCanvasItem as IMenuCanvasFoodItem).MultiPriceHeading);
            //        //                priceHeadingsCanvasFrame[(menuCanvasItem as IMenuCanvasFoodItem).MultiPriceHeading] = canvasFrameViewModel;
            //        //            }
            //        //            else
            //        //            {
            //        //                CanvasFrameViewModel canvasFrameViewModel = new CanvasFrameViewModel((menuCanvasItem as IMenuCanvasFoodItem).MultiPriceHeading, this);
            //        //                CanvasFrames.Add(canvasFrameViewModel);
            //        //                priceHeadingsCanvasFrame[(menuCanvasItem as IMenuCanvasFoodItem).MultiPriceHeading] = canvasFrameViewModel;
            //        //            }

            //        //        }
            //        //        #endregion
            //        //    }


            //        //    if ((menuCanvasItem as IMenuCanvasFoodItem).PriceLeader != null)
            //        //    {

            //        //        if (textCanvasItems.ContainsKey((menuCanvasItem as IMenuCanvasFoodItem).PriceLeader))
            //        //        {
            //        //            MenuCanvasItemTextViewModel textCanvasItem = textCanvasItems[(menuCanvasItem as IMenuCanvasFoodItem).PriceLeader];
            //        //            textCanvasItem.Visibility = Visibility.Visible;
            //        //            textCanvasItem.Refresh();
            //        //        }
            //        //        else if (freeTextCanvasItems.Count > 0)
            //        //        {
            //        //            MenuCanvasItemTextViewModel textCanvasItem = freeTextCanvasItems[0];
            //        //            freeTextCanvasItems.RemoveAt(0);
            //        //            textCanvasItem.Visibility = Visibility.Visible;
            //        //            textCanvasItem.ChangeCanvasItem((menuCanvasItem as IMenuCanvasFoodItem).PriceLeader);
            //        //        }
            //        //        else
            //        //        {
            //        //            MenuCanvasItemTextViewModel textCanvasItem = new MenuCanvasItemTextViewModel((menuCanvasItem as IMenuCanvasFoodItem).PriceLeader);
            //        //            TextCanvasItems.Add(textCanvasItem);
            //        //        }
            //        //        //CanvasItems.Add(new MenuCanvasItemViewModel((menuCanvasItem as IMenuCanvasFoodItem).PriceLeader));
            //        //    }
            //        //}

            //        //foreach (var menuCanvasFoodItemText in (menuCanvasItem as IMenuCanvasFoodItem).SubTexts)
            //        //{
            //        //    if (textCanvasItems.ContainsKey(menuCanvasFoodItemText))
            //        //    {
            //        //        MenuCanvasItemTextViewModel textCanvasItem = textCanvasItems[menuCanvasFoodItemText];
            //        //        textCanvasItem.Visibility = Visibility.Visible;
            //        //        textCanvasItem.Refresh();
            //        //    }
            //        //    else if (freeTextCanvasItems.Count > 0)
            //        //    {
            //        //        MenuCanvasItemTextViewModel textCanvasItem = freeTextCanvasItems[0];
            //        //        freeTextCanvasItems.RemoveAt(0);
            //        //        textCanvasItem.Visibility = Visibility.Visible;
            //        //        textCanvasItem.ChangeCanvasItem(menuCanvasFoodItemText);
            //        //    }
            //        //    else
            //        //    {
            //        //        MenuCanvasItemTextViewModel textCanvasItem = new MenuCanvasItemTextViewModel(menuCanvasFoodItemText);
            //        //        TextCanvasItems.Add(textCanvasItem);
            //        //    }

            //        //}
            //        #endregion

            //        if (canvasFrames.Count > 0)
            //        {
            //            CanvasFrameViewModel canvasFrameViewModel = canvasFrames[0];
            //            canvasFrames.RemoveAt(0);
            //            canvasFrameViewModel.Visibility = Visibility.Visible;
            //            canvasFrameViewModel.ChangeCanvasItem(menuCanvasItem as IMenuCanvasFoodItem);
            //        }
            //        else if (freeCanvasFrames.Count > 0)
            //        {
            //            CanvasFrameViewModel canvasFrameViewModel = freeCanvasFrames[0];
            //            freeCanvasFrames.RemoveAt(0);
            //            canvasFrameViewModel.Visibility = Visibility.Visible;
            //            canvasFrameViewModel.ChangeCanvasItem(menuCanvasItem as IMenuCanvasFoodItem);
            //        }
            //        else
            //        {
            //            CanvasFrameViewModel canvasFrameViewModel = new CanvasFrameViewModel(menuCanvasItem as IMenuCanvasFoodItem, this);
            //            CanvasFrames.Add(canvasFrameViewModel);
            //        }
            //    }
            //    else if (menuCanvasItem is IMenuCanvasHeading)
            //    {

            //        if (canvasFrames.Count > 0)
            //        {
            //            CanvasFrameViewModel canvasFrameViewModel = canvasFrames[0];
            //            canvasFrames.RemoveAt(0);
            //            canvasFrameViewModel.Visibility = Visibility.Visible;
            //            canvasFrameViewModel.ChangeCanvasItem(menuCanvasItem as IMenuCanvasHeading);
            //        }
            //        else if (freeCanvasFrames.Count > 0)
            //        {
            //            CanvasFrameViewModel canvasFrameViewModel = freeCanvasFrames[0];
            //            freeCanvasFrames.RemoveAt(0);
            //            canvasFrameViewModel.Visibility = Visibility.Visible;
            //            canvasFrameViewModel.ChangeCanvasItem(menuCanvasItem as IMenuCanvasHeading);
            //        }
            //        else
            //        {
            //            CanvasFrameViewModel canvasFrameViewModel = new CanvasFrameViewModel(menuCanvasItem as IMenuCanvasHeading, this);
            //            CanvasFrames.Add(canvasFrameViewModel);
            //        }
            //        //if (textCanvasItems.ContainsKey(menuCanvasItem))
            //        //{
            //        //    MenuCanvasItemTextViewModel textCanvasItem = textCanvasItems[menuCanvasItem];
            //        //    textCanvasItem.Visibility = Visibility.Visible;
            //        //    textCanvasItem.Refresh();
            //        //}
            //        //else if (freeTextCanvasItems.Count > 0)
            //        //{

            //        //    MenuCanvasItemTextViewModel textCanvasItem = freeTextCanvasItems[0];
            //        //    freeTextCanvasItems.RemoveAt(0);
            //        //    textCanvasItem.Visibility = Visibility.Visible;
            //        //    textCanvasItem.ChangeCanvasItem(menuCanvasItem);
            //        //}
            //        //else
            //        //{
            //        //    MenuCanvasItemTextViewModel textCanvasItem = new MenuCanvasItemTextViewModel(menuCanvasItem);
            //        //    TextCanvasItems.Add(textCanvasItem);
            //        //}
            //    }
            //    else
            //    {
            //        //ViewModel.MenuCanvasItemViewModel menuCanvasItemViewModel = new MenuCanvasItemViewModel(menuCanvasItem);
            //        //CanvasItems.Add(menuCanvasItemViewModel);
            //    }
            //}
            #endregion


            if (DragDropArea == null)
                DragDropArea = new CanvasFrameViewModel(this);


            //List<AccentImagePresentation> accentImages = AccentImages.ToList();
            //List<MenuCanvasItemViewModel> textCanvasItems = TextCanvasItems.ToList();
            //List<CanvasFrameViewModel> canvasFrames = CanvasFrames.ToList();

            foreach (var canvasFrame in CanvasFramesSlots.ToList())
            {
                if (canvasFrame.MenuCanvasItem is IItemMultiPriceHeading)
                {
                    int pos = CanvasFramesSlots.IndexOf(canvasFrame);
                    //CanvasFrames.Remove(canvasFrame);
                    //CanvasFrames.Add(canvasFrame);
                }
            }

            List<ICanvasItem> canvasItems = new List<ICanvasItem>();
            canvasItems.AddRange(AccentImagesSlots.OfType<ICanvasItem>());
            canvasItems.AddRange(TextCanvasItemsSlots);
            canvasItems.AddRange(CanvasFramesSlots);
            canvasItems.Add(DragDropArea);
            canvasItems.AddRange(Lines);
            canvasItems.AddRange(TranslationLines);
            canvasItems.AddRange(PriceHedingsSlots);
            bool update = false;
            for (int i = 0; i != canvasItems.Count; i++)
            {
                if (CanvasItemsSlots.Count == i)
                {
                    update = true;
                    CanvasItemsSlots.Add(canvasItems[i]);
                }
                else if (CanvasItemsSlots[i] != canvasItems[i])
                {
                    CanvasItemsSlots[i] = canvasItems[i];
                    update = true;
                }
            }
            if (update || ForceCanvasItemsUpdate)
            {
                ObjectChangeState?.Invoke(this, nameof(ItemsToShowInCanvas));
                ForceCanvasItemsUpdate = false;
            }
        }

        private void ReAssignCanvasFrames()
        {
            List<IMenuCanvasFoodItem> menuCanvasFoodItems = MenuPage.MenuCanvasItems.OfType<IMenuCanvasFoodItem>().ToList();
            List<IMenuCanvasHeading> menuCanvasHeadings = MenuPage.MenuCanvasItems.OfType<IMenuCanvasHeading>().ToList();

            //var itemsMultiplePriceHeadings = (from MenuCanvasFoodItem foodItem in MenuPage.MenuCanvasItems.OfType<MenuCanvasFoodItem>()
            //                                  where foodItem.MultiPriceHeading != null
            //                                  select foodItem.MultiPriceHeading).OfType<IMenuCanvasItem>().Distinct().ToList();

            Dictionary<IMenuCanvasItem, CanvasFrameViewModel> canvasFrames = CanvasFramesSlots.ToDictionary(x => x.MenuCanvasItem);

            List<CanvasFrameViewModel> freeCanvasFrames = (from canvasFrameViewModel in CanvasFramesSlots
                                                           where !menuCanvasFoodItems.Contains(canvasFrameViewModel.MenuCanvasItem) && !menuCanvasHeadings.Contains(canvasFrameViewModel.MenuCanvasItem)
                                                           select canvasFrameViewModel).ToList();

            foreach (var framedMenuCanvasItem in MenuPage.MenuCanvasItems.Where(x => (x is IMenuCanvasFoodItem) || (x is IMenuCanvasHeading)))
            {

                if (canvasFrames.ContainsKey(framedMenuCanvasItem))
                {
                    CanvasFrameViewModel canvasFrameViewModel = canvasFrames[framedMenuCanvasItem];
                    canvasFrameViewModel.Visibility = Visibility.Visible;
                    canvasFrameViewModel.Refresh();
                }
                else if (freeCanvasFrames.Count > 0)
                {
                    CanvasFrameViewModel canvasFrameViewModel = freeCanvasFrames[0];
                    freeCanvasFrames.RemoveAt(0);
                    canvasFrameViewModel.Visibility = Visibility.Visible;

                    if (framedMenuCanvasItem is IMenuCanvasFoodItem)
                        canvasFrameViewModel.ChangeCanvasItem(framedMenuCanvasItem as IMenuCanvasFoodItem);
                    if (framedMenuCanvasItem is IMenuCanvasHeading)
                        canvasFrameViewModel.ChangeCanvasItem(framedMenuCanvasItem as IMenuCanvasHeading);
                }
                else
                {
                    if (framedMenuCanvasItem is IMenuCanvasFoodItem)
                    {
                        if (!IsReadonly)
                        {
                            CanvasFrameViewModel canvasFrameViewModel = new CanvasFrameViewModel(framedMenuCanvasItem as IMenuCanvasFoodItem, this);
                            CanvasFramesSlots.Add(canvasFrameViewModel);
                        }
                    }
                    if (framedMenuCanvasItem is IMenuCanvasHeading)
                    {
                        if (!IsReadonly)
                        {
                            CanvasFrameViewModel canvasFrameViewModel = new CanvasFrameViewModel(framedMenuCanvasItem as IMenuCanvasHeading, this);
                            CanvasFramesSlots.Add(canvasFrameViewModel);
                        }
                    }
                }
            }

            foreach (var canvasFrame in freeCanvasFrames)
                canvasFrame.Visibility = Visibility.Collapsed;

        }

        private void ReAssignSeparationLines()
        {
            var menuCanvasLines = MenuPage.SeparationLines;
            List<MenuCanvasLinePresentation> freeCanvasLines = (from pageLine in Lines
                                                                where !menuCanvasLines.Contains(pageLine.MenuCanvasLine)
                                                                select pageLine).ToList();

            Dictionary<IMenuCanvasLine, MenuCanvasLinePresentation> linesDictionary = (from menuCanvasLine in Lines
                                                                                       where menuCanvasLines.Contains(menuCanvasLine.MenuCanvasLine)
                                                                                       select menuCanvasLine).ToDictionary(x => x.MenuCanvasLine);
            foreach (var separationLine in MenuPage.SeparationLines)
            {
                if (linesDictionary.ContainsKey(separationLine))
                {
                    MenuCanvasLinePresentation linePresenation = linesDictionary[separationLine];
                    linePresenation.Visibility = Visibility.Visible;
                    linePresenation.Refresh();
                }
                else if (freeCanvasLines.Count > 0)
                {
                    MenuCanvasLinePresentation linePresenation = freeCanvasLines[0];
                    freeCanvasLines.RemoveAt(0);
                    linePresenation.Visibility = Visibility.Visible;
                    linePresenation.ChangeCanvasLine(separationLine);
                }
                else
                {
                    MenuCanvasLinePresentation linePresenation = new MenuCanvasLinePresentation(separationLine);
                    Lines.Add(linePresenation);
                }
            }

            foreach (var line in freeCanvasLines)
                line.Visibility = Visibility.Collapsed;
        }


        private void ReAssignTranslationLines()
        {
            var menuCanvasLines = MenuPage.TranslationLines;
            List<MenuCanvasLinePresentation> freeCanvasLines = (from pageLine in TranslationLines
                                                                where !menuCanvasLines.Contains(pageLine.MenuCanvasLine)
                                                                select pageLine).ToList();

            Dictionary<IMenuCanvasLine, MenuCanvasLinePresentation> linesDictionary = (from menuCanvasLine in TranslationLines
                                                                                       where menuCanvasLines.Contains(menuCanvasLine.MenuCanvasLine)
                                                                                       select menuCanvasLine).ToDictionary(x => x.MenuCanvasLine);
            foreach (var translationLine in MenuPage.TranslationLines)
            {
                if (linesDictionary.ContainsKey(translationLine))
                {
                    MenuCanvasLinePresentation linePresenation = linesDictionary[translationLine];
                    linePresenation.Visibility = Visibility.Visible;
                    linePresenation.Refresh();
                }
                else if (freeCanvasLines.Count > 0)
                {
                    MenuCanvasLinePresentation linePresenation = freeCanvasLines[0];
                    freeCanvasLines.RemoveAt(0);
                    linePresenation.Visibility = Visibility.Visible;
                    linePresenation.ChangeCanvasLine(translationLine);
                }
                else
                {
                    MenuCanvasLinePresentation linePresenation = new TranslationLinePresentation(translationLine);
                    TranslationLines.Add(linePresenation);
                }
            }

            foreach (var line in freeCanvasLines)
                line.Visibility = Visibility.Collapsed;
        }

        private void ReAssignTextItems()
        {


            List<IMenuCanvasItem> menuCanvasTextItems = (from menuCanvasItem in MenuPage.MenuCanvasItems
                                                         where menuCanvasItem is IMenuCanvasHeading
                                                         select menuCanvasItem).OfType<IMenuCanvasItem>().ToList();


            List<IMenuCanvasFoodItem> menuCanvasFoodItems = (from menuCanvasItem in MenuPage.MenuCanvasItems
                                                             where menuCanvasItem is IMenuCanvasFoodItem
                                                             select menuCanvasItem).OfType<IMenuCanvasFoodItem>().ToList();

            menuCanvasTextItems.AddRange((from menuCanvasItem in MenuPage.MenuCanvasItems.OfType<IMenuCanvasFoodItem>()
                                          from menuCanvasFoodItemText in menuCanvasItem.SubTexts
                                          select menuCanvasFoodItemText).OfType<IMenuCanvasItem>().ToList());

            menuCanvasTextItems.AddRange((from menuCanvasItem in MenuPage.MenuCanvasItems.OfType<IMenuCanvasFoodItem>()
                                          from priceText in menuCanvasItem.Prices
                                          where priceText.Visisble
                                          select priceText).OfType<IMenuCanvasItem>().ToList());

            menuCanvasTextItems.AddRange((from menuCanvasItem in MenuPage.MenuCanvasItems.OfType<IMenuCanvasFoodItem>()
                                          select menuCanvasItem.PriceLeader).OfType<IMenuCanvasItem>().ToList());




            //menuCanvasTextItems.AddRange(tmppriceHeadings);


            Dictionary<IMenuCanvasItem, MenuCanvasItemTextViewModel> textCanvasItems = (from menuCanvasItem in TextCanvasItemsSlots
                                                                                        where menuCanvasTextItems.Contains(menuCanvasItem.MenuCanvasItem)
                                                                                        select menuCanvasItem).ToDictionary(x => x.MenuCanvasItem);

            List<MenuCanvasItemTextViewModel> freeTextCanvasItems = (from menuCanvasItem in TextCanvasItemsSlots
                                                                     where !menuCanvasTextItems.Contains(menuCanvasItem.MenuCanvasItem)
                                                                     select menuCanvasItem).ToList();


            foreach (var menuCanvasFoodItemText in menuCanvasTextItems)
            {
                if (menuCanvasFoodItemText.Page == null)
                {

                }
                if (textCanvasItems.ContainsKey(menuCanvasFoodItemText))
                {
                    MenuCanvasItemTextViewModel textCanvasItem = textCanvasItems[menuCanvasFoodItemText];
                    textCanvasItem.Visibility = Visibility.Visible;
                    textCanvasItem.Refresh();
                }
                else if (freeTextCanvasItems.Count > 0)
                {
                    MenuCanvasItemTextViewModel textCanvasItem = freeTextCanvasItems[0];
                    freeTextCanvasItems.RemoveAt(0);
                    textCanvasItem.Visibility = Visibility.Visible;
                    textCanvasItem.ChangeCanvasItem(menuCanvasFoodItemText);
                }
                else
                {
                    if ((from menuCanvasItem in TextCanvasItemsSlots
                         where menuCanvasItem.MenuCanvasItem == menuCanvasFoodItemText
                         select menuCanvasItem).ToList().Count > 0)
                    {

                    }

                    MenuCanvasItemTextViewModel textCanvasItem = new MenuCanvasItemTextViewModel(menuCanvasFoodItemText);


                    TextCanvasItemsSlots.Add(textCanvasItem);
                }
            }
            foreach (var textCanvasItem in freeTextCanvasItems)
                textCanvasItem.Visibility = Visibility.Collapsed;


        }

        private void ReAssignPricesHeadings()
        {
            var multiPriceHeadings = (from menuCanvasItem in MenuPage.MenuCanvasItems.OfType<IMenuCanvasFoodItem>()
                                      where menuCanvasItem.MultiPriceHeading != null
                                      select menuCanvasItem.MultiPriceHeading).Distinct().ToList();

            var menuCanvasPriceHeadings = (from multiPriceHeading in multiPriceHeadings
                                           from priceHeading in multiPriceHeading.PriceHeadings
                                           select priceHeading).ToList();



            Dictionary<IPriceHeading, PriceHeadingViewModel> priceHeadings = (from priceHeading in PriceHedingsSlots
                                                                              where menuCanvasPriceHeadings.Contains(priceHeading.PriceHeading)
                                                                              select priceHeading).ToDictionary(x => x.PriceHeading);


            List<PriceHeadingViewModel> freePriceHeadings = (from priceHeading in PriceHedingsSlots
                                                             where !menuCanvasPriceHeadings.Contains(priceHeading.PriceHeading)
                                                             select priceHeading).ToList();


            foreach (var multiPriceHeading in multiPriceHeadings)
            {
                foreach (var menuCanvasItemPriceHeading in multiPriceHeading.PriceHeadings)
                {

                    if (priceHeadings.ContainsKey(menuCanvasItemPriceHeading))
                    {
                        PriceHeadingViewModel priceHeadingVM = priceHeadings[menuCanvasItemPriceHeading];

                        priceHeadingVM.Visibility = Visibility.Visible;
                        priceHeadingVM.Refresh();
                    }
                    else if (freePriceHeadings.Count > 0)
                    {
                        PriceHeadingViewModel priceHeadingVM = freePriceHeadings[0];
                        freePriceHeadings.RemoveAt(0);
                        priceHeadingVM.Visibility = Visibility.Visible;
                        priceHeadingVM.ChangeCanvasItem(menuCanvasItemPriceHeading);
                        priceHeadings[menuCanvasItemPriceHeading] = priceHeadingVM;
                    }
                    else
                    {
                        PriceHeadingViewModel priceHeadingVM = new PriceHeadingViewModel(menuCanvasItemPriceHeading);
                        PriceHedingsSlots.Add(priceHeadingVM);
                        priceHeadings[menuCanvasItemPriceHeading] = priceHeadingVM;
                    }

                }
            }

            foreach (var priceHeading in freePriceHeadings)
                priceHeading.Visibility = Visibility.Collapsed;
        }

        private void ReAssignAccent()
        {
            List<AccentImageKey> menuCanvasAccentImages = new List<AccentImageKey>();
            foreach (var menuCanvasAccent in MenuPage.MenuCanvasItems.OfType<IHighlightedMenuCanvasItem>().Where(x => x.MenuCanvasAccent != null).Select(x => x.MenuCanvasAccent).Distinct())
            {
                int i = 0;
                foreach (var accentImage in menuCanvasAccent.Accent.AccentImages)
                    menuCanvasAccentImages.Add(new AccentImageKey(menuCanvasAccent, i++));
            }

            Dictionary<AccentImageKey, AccentImagePresentation> accentImagesDictionary = (from headingAccent in AccentImagesSlots
                                                                                          where menuCanvasAccentImages.Contains(new AccentImageKey(headingAccent.HeadingAccent, headingAccent.AccentIndex))
                                                                                          select headingAccent).ToDictionary(x => new AccentImageKey() { HeadingAccent = x.HeadingAccent, AccentIndex = x.AccentIndex });

            List<AccentImagePresentation> freeAccentImages = (from headingAccent in AccentImagesSlots
                                                              where !menuCanvasAccentImages.Contains(new AccentImageKey(headingAccent.HeadingAccent, headingAccent.AccentIndex))
                                                              select headingAccent).ToList();


            foreach (var menuCanvasAccentImage in menuCanvasAccentImages)
            {
                AccentImagePresentation accentImageVM = null;
                if (accentImagesDictionary.TryGetValue(menuCanvasAccentImage, out accentImageVM))
                {
                    accentImageVM.Visibility = Visibility.Visible;
                    accentImageVM.ChangeAccent(menuCanvasAccentImage.HeadingAccent, menuCanvasAccentImage.AccentIndex);
                }
                else
                {
                    if (freeAccentImages.Count > 0)
                    {
                        AccentImagePresentation freeAccentImageVM = freeAccentImages[0];
                        freeAccentImages.RemoveAt(0);
                        freeAccentImageVM.Visibility = Visibility.Visible;
                        freeAccentImageVM.ChangeAccent(menuCanvasAccentImage.HeadingAccent, menuCanvasAccentImage.AccentIndex);
                    }
                    else
                    {
                        AccentImagePresentation newAccentImageVM = new AccentImagePresentation(menuCanvasAccentImage.HeadingAccent, menuCanvasAccentImage.AccentIndex);
                        AccentImagesSlots.Add(newAccentImageVM);
                    }
                }
            }
            foreach (var freeAccentImageVM in freeAccentImages)
                freeAccentImageVM.Visibility = Visibility.Hidden;
        }

        internal MenuPresentationModel.MenuCanvas.Rect GetDropRectangle(Point point)
        {

            return MenuPage.GetDropRectangle(point);
        }

        internal CanvasFrameViewModel DragDropArea;

        //public TransformGroup BackgroundTransform
        //{
        //    get
        //    {
        //        var transform = new TransformGroup();
        //        transform.Children.Add(new ScaleTransform(1, 1));
        //        TranslateTransform translateTransform = new TranslateTransform(0, ActualHeight);

        //        //Binding myBinding = new Binding();
        //        ////myBinding.Source = ViewModel;
        //        //myBinding.RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(Grid), 1); 
        //        //myBinding.Path = new PropertyPath("ActualHeight");
        //        //myBinding.Mode = BindingMode.OneWay;

        //        //BindingOperations.SetBinding(translateTransform, TranslateTransform.YProperty, myBinding);
        //        transform.Children.Add(translateTransform);
        //        return transform;

        //    }

        //}
        public double BorderThickness
        {
            get
            {
                return 10;
            }
        }
        /// <MetaDataID>{515bce28-4163-4db0-bc43-5710ba2abaa5}</MetaDataID>
        public double PageWidth
        {
            get
            {
                if (BookViewModel != null && BookViewModel.RealObject.Style != null)
                    return (BookViewModel.RealObject.Style.Styles["page"] as MenuPresentationModel.MenuStyles.PageStyle).PageWidth + (2 * BorderThickness);
                return 600;
            }
        }
        /// <MetaDataID>{ab5c103b-03bf-4f01-893c-c5f259fcc6a0}</MetaDataID>
        public double PageHeight
        {
            get
            {
                if (BookViewModel != null && BookViewModel.RealObject.Style != null)
                    return (BookViewModel.RealObject.Style.Styles["page"] as MenuPresentationModel.MenuStyles.PageStyle).PageHeight + (2 * BorderThickness);

                return 800;
            }
        }
        /// <exclude>Excluded</exclude>
        double _Border = 0;
        /// <MetaDataID>{42d63f98-8f66-4302-b70d-a3ece90f84a6}</MetaDataID>
        public double Border
        {
            get
            {
                return _Border;
            }
            set
            {
                _Border = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Border"));

            }
        }
        /// <exclude>Excluded</exclude>
        static BookPageViewModel _SelectedBookPage;
        /// <MetaDataID>{57fb1cca-1151-433c-9a8b-4a8258b550bb}</MetaDataID>
        public static BookPageViewModel SelectedBookPage
        {
            set
            {
                if (_SelectedBookPage != value)
                {
                    if (_SelectedBookPage != null)
                        _SelectedBookPage.Border = 0;
                    _SelectedBookPage = value;
                    if (_SelectedBookPage != null)
                        _SelectedBookPage.Border = 1;
                }

            }
        }

        /// <MetaDataID>{da3fbc6d-d330-499b-9e70-827201635877}</MetaDataID>
        internal void Select()
        {
            SelectedBookPage = this;

        }
        MenuPage _MenuPage;
        public readonly BookViewModel BookViewModel;

        public MenuPage MenuPage
        {
            get
            {
                return _MenuPage;
            }
        }
        public System.Windows.Thickness BorderMargin
        {
            get
            {
                UIBaseEx.Margin margin = new UIBaseEx.Margin();
                if (BookViewModel != null && BookViewModel.RealObject.Style != null)
                    margin = (BookViewModel.RealObject.Style.Styles["page"] as MenuPresentationModel.MenuStyles.PageStyle).BorderMargin;
                return new System.Windows.Thickness(margin.MarginLeft, margin.MarginTop, margin.MarginRight, margin.MarginBottom);
            }
        }

        public System.Windows.Thickness BackgroundMargin
        {
            get
            {
                UIBaseEx.Margin margin = new UIBaseEx.Margin();
                if (BookViewModel != null && BookViewModel.RealObject.Style != null)
                    margin = (BookViewModel.RealObject.Style.Styles["page"] as MenuPresentationModel.MenuStyles.PageStyle).BackgroundMargin;
                return new System.Windows.Thickness(margin.MarginLeft, margin.MarginTop, margin.MarginRight, margin.MarginBottom);
            }
        }

        public Stretch BackgroundStretch
        {
            get
            {
                if (BookViewModel != null && BookViewModel.RealObject.Style != null)
                    return (Stretch)(int)(BookViewModel.RealObject.Style.Styles["page"] as MenuPresentationModel.MenuStyles.PageStyle).BackgroundStretch;

                return Stretch.None;

            }
        }
        public double BackgroundScaleY
        {
            get
            {
                if (BookViewModel != null &&
                  BookViewModel.RealObject.Style != null &&
                  (BookViewModel.RealObject.Style.Styles["page"] as MenuPresentationModel.MenuStyles.PageStyle).Background != null &&
                  (BookViewModel.RealObject.Style.Styles["page"] as MenuPresentationModel.MenuStyles.PageStyle).Background.Flip)
                {
                    return -1;
                }
                else
                    return 1;

            }
        }

        public double BackgroundOffsetY
        {
            get
            {
                if (BookViewModel != null &&
                 BookViewModel.RealObject.Style != null &&
                 (BookViewModel.RealObject.Style.Styles["page"] as PageStyle).Background != null &&
                 (BookViewModel.RealObject.Style.Styles["page"] as PageStyle).Background.Flip)
                {
                    return (BookViewModel.RealObject.Style.Styles["page"] as PageStyle).PageHeight - (BackgroundMargin.Bottom + BackgroundMargin.Top);
                }
                else
                    return 0;
            }
        }
        public double BackgroundScaleX
        {
            get
            {
                if (BookViewModel != null &&
                  BookViewModel.RealObject.Style != null &&
                  (BookViewModel.RealObject.Style.Styles["page"] as MenuPresentationModel.MenuStyles.PageStyle).Background != null &&
                  (BookViewModel.RealObject.Style.Styles["page"] as MenuPresentationModel.MenuStyles.PageStyle).Background.Mirror)
                {
                    return -1;
                }
                else
                    return 1;

            }
        }
        public double BackgroundOffsetX
        {
            get
            {
                if (BookViewModel != null &&
                 BookViewModel.RealObject.Style != null &&
                 (BookViewModel.RealObject.Style.Styles["page"] as PageStyle).Background != null &&
                 (BookViewModel.RealObject.Style.Styles["page"] as PageStyle).Background.Mirror)
                {
                    return (BookViewModel.RealObject.Style.Styles["page"] as PageStyle).PageWidth - (BackgroundMargin.Right + BackgroundMargin.Left);
                }
                else
                    return 0;
            }
        }
        public double BackgroundOpacity
        {
            get
            {
                if (BookViewModel != null &&
                BookViewModel.RealObject.Style != null &&
                (BookViewModel.RealObject.Style.Styles["page"] as PageStyle).Background != null)
                {
                    return (BookViewModel.RealObject.Style.Styles["page"] as PageStyle).Background.Opacity;
                }
                else
                    return 1;
            }
        }


        public Visibility BorderVisibility
        {
            get
            {
                if (BorderDrawing == null)
                    return Visibility.Hidden;
                else
                    return Visibility.Visible;

            }
        }
        public DrawingGroup BorderDrawing
        {
            get
            {
                if (BorderUri == null)
                    return null;

                PageStyle pageStyle = BookViewModel.RealObject.Style.Styles["page"] as PageStyle;

                DrawingGroup drawing = null;
                MemoryStream ms = new MemoryStream();
                using (FileStream file = new FileStream(BorderUri, FileMode.Open, System.IO.FileAccess.Read))
                {
                    byte[] bytes = new byte[file.Length];



                    file.Read(bytes, 0, (int)file.Length);
                    ms.Write(bytes, 0, (int)file.Length);
                    ms.Position = 0;


                    string color = pageStyle.Border.Color;
                    //   if (!string.IsNullOrWhiteSpace(color))
                    ms = TransformSvg(ms, color, pageStyle.Border.Flip, pageStyle.Border.Mirror);

                    bool _textAsGeometry = false;
                    bool _includeRuntime = true;
                    bool _optimizePath = true;

                    WpfDrawingSettings settings = new WpfDrawingSettings();
                    settings.IncludeRuntime = _includeRuntime;
                    settings.TextAsGeometry = _textAsGeometry;
                    settings.OptimizePath = _optimizePath;
                    ////  if (_culture != null)
                    //      settings.CultureInfo = _culture;

                    using (FileSvgReader reader =
                              new FileSvgReader(settings))
                    {
                        drawing = reader.Read(ms);
                    }
                    return drawing;
                }
            }
        }
        public System.IO.Stream BorderAsStream
        {
            get
            {
                if (BorderUri == null)
                    return null;

                PageStyle pageStyle = BookViewModel.RealObject.Style.Styles["page"] as PageStyle;


                MemoryStream ms = new MemoryStream();
                using (FileStream file = new FileStream(BorderUri, FileMode.Open, System.IO.FileAccess.Read))
                {
                    byte[] bytes = new byte[file.Length];



                    file.Read(bytes, 0, (int)file.Length);
                    ms.Write(bytes, 0, (int)file.Length);
                    ms.Position = 0;


                    string color = pageStyle.Border.Color;
                    //   if (!string.IsNullOrWhiteSpace(color))
                    ms = TransformSvg(ms, color, pageStyle.Border.Flip, pageStyle.Border.Mirror);

                    return ms;
                }
            }
        }
        private static MemoryStream TransformSvg(MemoryStream ms, string newColor, bool flip, bool mirror)
        {


            SharpVectors.Dom.Svg.SvgDocument doc = new SharpVectors.Dom.Svg.SvgDocument(null);
            doc.Load(ms);
            ms.Close();
            //doc.RootElement.SetAttribute("transform", "translate(576, 0) scale(-1, 1)");
            System.Xml.XmlElement gElement = (from node in doc.RootElement.ChildNodes.OfType<XmlNode>()
                                              where node.Name == "g"
                                              select node).FirstOrDefault() as XmlElement;
            int widthInpx = 0;
            int heigthInpx = 0;
            int scaleX = 1;
            int scaleY = 1;
            if (mirror)
            {
                widthInpx = (int)(double)new System.Windows.LengthConverter().ConvertFromString(null, new System.Globalization.CultureInfo(1033), (doc.RootElement as XmlElement).GetAttribute("width"));
                scaleX = -1;
            }
            if (flip)
            {
                heigthInpx = (int)(double)new System.Windows.LengthConverter().ConvertFromString(null, new System.Globalization.CultureInfo(1033), (doc.RootElement as XmlElement).GetAttribute("height"));
                scaleY = -1;
            }

            if (gElement != null)
                gElement.SetAttribute("transform", string.Format("translate({0},{1}) scale({2},{3})", widthInpx, heigthInpx, scaleX, scaleY));

            if (newColor != "none" && newColor != null)
            {

                List<Color> colors = new List<Color>();
                for (ulong i = 0; i != doc.StyleSheets.Length; i++)
                {

                    //doc.RootElement
                    //transform = "translate(576,0) scale(-1, 1)"
                    var staleSheet = doc.StyleSheets[i];
                    for (ulong k = 0; k != (staleSheet as SharpVectors.Dom.Css.CssStyleSheet).CssRules.Length; k++)
                    {
                        var cssStyleRule = (staleSheet as SharpVectors.Dom.Css.CssStyleSheet).CssRules[k] as SharpVectors.Dom.Css.CssStyleRule;
                        string sdf = cssStyleRule.SelectorText;

                        if (cssStyleRule.Style.GetPropertyCssValue("stroke") != null && cssStyleRule.Style.GetPropertyValue("stroke").ToLower() != "none")
                        {
                            string colorStr = cssStyleRule.Style.GetPropertyValue("stroke");
                            object color = ColorConverter.ConvertFromString(colorStr);
                            if (color is Color)
                                colors.Add((Color)color);
                        }
                        if ((cssStyleRule.Style.GetPropertyCssValue("fill") != null && cssStyleRule.Style.GetPropertyValue("fill").ToLower() != "none"))
                        {
                            string colorStr = cssStyleRule.Style.GetPropertyValue("fill");
                            object color = ColorConverter.ConvertFromString(colorStr);
                            if (color is Color)
                                colors.Add((Color)color);
                        }
                    }
                }

                colors = ConvertToGrayScaleImage(colors, (Color)ColorConverter.ConvertFromString(newColor));
                int colorIndex = 0;
                for (ulong i = 0; i != doc.StyleSheets.Length; i++)
                {
                    var staleSheet = doc.StyleSheets[i];
                    for (ulong k = 0; k != (staleSheet as SharpVectors.Dom.Css.CssStyleSheet).CssRules.Length; k++)
                    {
                        var cssStyleRule = (staleSheet as SharpVectors.Dom.Css.CssStyleSheet).CssRules[k] as SharpVectors.Dom.Css.CssStyleRule;
                        if (cssStyleRule.Style.GetPropertyCssValue("fill") != null && cssStyleRule.Style.GetPropertyValue("fill").ToLower() != "none")
                        {
                            string hexColor = new ColorConverter().ConvertToString(colors[colorIndex++]);
                            cssStyleRule.Style.SetProperty("fill", hexColor, cssStyleRule.Style.GetPropertyPriority("fill"));
                        }
                        if (cssStyleRule.Style.GetPropertyCssValue("stroke") != null && cssStyleRule.Style.GetPropertyValue("stroke").ToLower() != "none")
                        {
                            string hexColor = new ColorConverter().ConvertToString(colors[colorIndex++]);
                            cssStyleRule.Style.SetProperty("stroke", hexColor, cssStyleRule.Style.GetPropertyPriority("stroke"));
                        }
                        //    if (cssStyleRule.SelectorText == ".st0")
                        //{
                        //    if (cssStyleRule.Style.GetPropertyCssValue("stroke") != null)
                        //        cssStyleRule.Style.SetProperty("stroke", newColor, cssStyleRule.Style.GetPropertyPriority("stroke"));
                        //    else
                        //        cssStyleRule.Style.SetProperty("fill", newColor, cssStyleRule.Style.GetPropertyPriority("fill"));
                        //}
                    }
                    string newCssText = "\n\n";
                    for (ulong k = 0; k != (staleSheet as SharpVectors.Dom.Css.CssStyleSheet).CssRules.Length; k++)
                    {
                        var cssStyleRule = (staleSheet as SharpVectors.Dom.Css.CssStyleSheet).CssRules[k] as SharpVectors.Dom.Css.CssStyleRule;
                        newCssText += "\n" + cssStyleRule.CssText;
                    }
                    newCssText += "\n\n";
                    staleSheet.OwnerNode.InnerText = newCssText;
                }
            }
            ms = new MemoryStream();
            doc.Save(ms);
            ms.Position = 0;
            return ms;
        }



        private static List<Color> ConvertToGrayScaleImage(List<Color> orgColors, Color newColor)
        {
            System.Drawing.Bitmap originalBitmap = Properties.Resources.TransformColor;
            // A blank bitmap is created having same size as original bitmap image.
            System.Drawing.Bitmap colorScaleBitmap = new System.Drawing.Bitmap(originalBitmap.Width, originalBitmap.Height);

            List<Color> colors = new List<Color>();

            int i = 0;
            foreach (Color color in orgColors)
            {
                originalBitmap.SetPixel(i, 0, System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B));
                i++;
            }



            // Initializing a graphics object from the new image bitmap.
            System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(colorScaleBitmap);

            // Creating the Grayscale ColorMatrix whose values are determined by
            // calculating the luminosity of a color, which is a weighted average of the
            // RGB color components. The average is weighted according to the sensitivity
            // of the human eye to each color component. The weights used here are as
            // given by the NTSC (North America Television Standards Committee)
            // and are widely accepted.
            //ColorMatrix colorMatrix = new ColorMatrix(new float[][]
            //{
            //    new float[] { 0.299f, 0.299f, 0.299f, 0, 0 },
            //    new float[] { 0.587f, 0.587f, 0.587f, 0, 0 },
            //    new float[] { 0.114f, 0.114f, 0.114f, 0, 0 },
            //    new float[] { 0,      0,      0,      1, 0 },
            //    new float[] { 0,      0,      0,      0, 1 }
            //});

            //ColorMatrix colorMatrix = new ColorMatrix(new float[][]
            //{
            //    new float[] {1f, 0.349f, 0.272f, 0, 0},
            //    new float[] {1f, 0.686f, 0.534f, 0, 0},
            //    new float[] {1f, 0.168f, 0.131f, 0, 0},
            //    new float[] { 0, 0, 0, 1, 0},
            //    new float[] { 0, 0, 0, 0, 1}
            //});
            //#0b8565
            //a11d28
            float r = ((float)newColor.R) / ((float)0xff);
            float g = ((float)newColor.G) / ((float)0xff);
            float b = ((float)newColor.B) / ((float)0xff);
            if (r > 1)
                r = 1;
            if (g > 1)
                g = 1;
            if (b > 1)
                b = 1;


            int e = 0;
            System.Drawing.Imaging.ColorMatrix orgcolorMatrix = new System.Drawing.Imaging.ColorMatrix(new float[][]
           {
                             new float[] { r, 0, 0, 0, 0 },
                             new float[] { 0, g, 0, 0, 0 },
                             new float[] { 0, 0, b, 0, 0 },
                             new float[] { 0, 0, 0, 1, 0 },
                             new float[] { 0, 0, 0, 0, 1 }
           });
            //create the grayscale ColorMatrix
            System.Drawing.Imaging.ColorMatrix colorMatrix = new System.Drawing.Imaging.ColorMatrix(
               new float[][]
               {
                 new float[] {.3f, .3f, .3f, 0, 0},
                 new float[] {.59f, .59f, .59f, 0, 0},
                 new float[] {.11f, .11f, .11f, 0, 0},
                 new float[] {.6f, .6f, .6f, 1, 0},
                 new float[] {0, 0, 0, 0, 1}
               });

            // Creating image attributes.
            System.Drawing.Imaging.ImageAttributes attributes = new System.Drawing.Imaging.ImageAttributes();

            // Setting the color matrix attribute.
            attributes.SetColorMatrix(colorMatrix);

            // Drawing the original bitmap image on the new bitmap image using the
            // Grayscale color matrix.
            graphics.DrawImage(originalBitmap, new System.Drawing.Rectangle(0, 0, originalBitmap.Width,
                originalBitmap.Height), 0, 0, originalBitmap.Width,
                originalBitmap.Height, System.Drawing.GraphicsUnit.Pixel, attributes);

            graphics.Dispose();
            attributes.SetColorMatrix(orgcolorMatrix);
            originalBitmap = colorScaleBitmap;
            colorScaleBitmap = new System.Drawing.Bitmap(originalBitmap.Width, originalBitmap.Height);

            graphics = System.Drawing.Graphics.FromImage(colorScaleBitmap);
            // Drawing the original bitmap image on the new bitmap image using the
            // Grayscale color matrix.
            graphics.DrawImage(originalBitmap, new System.Drawing.Rectangle(0, 0, originalBitmap.Width,
                originalBitmap.Height), 0, 0, originalBitmap.Width,
                originalBitmap.Height, System.Drawing.GraphicsUnit.Pixel, attributes);


            // Disposing the Graphics object.
            graphics.Dispose();


            i = 0;
            foreach (Color color in orgColors)
            {
                var dColor = colorScaleBitmap.GetPixel(i, 0);
                colors.Add(Color.FromArgb(dColor.A, dColor.R, dColor.G, dColor.B));
                i++;
            }


            return colors;
        }


        public String BorderUri
        {
            get
            {
                if (BookViewModel != null && BookViewModel.RealObject.Style != null &&
                    (BookViewModel.RealObject.Style.Styles["page"] as PageStyle).Border != null)
                {

                    if ((BookViewModel.RealObject.Style.Styles["page"] as PageStyle).IsPortrait)
                        return MenuPresentationModel.MenuStyles.PageStyle.ResourcesRootPath + (BookViewModel.RealObject.Style.Styles["page"] as PageStyle).Border.PortraitUri;
                    else
                        return MenuPresentationModel.MenuStyles.PageStyle.ResourcesRootPath + (BookViewModel.RealObject.Style.Styles["page"] as PageStyle).Border.LandscapeUri;
                }
                return null;
            }
        }
        public String BackgroundUri
        {
            get
            {

                if (BookViewModel != null && BookViewModel.RealObject.Style != null &&
                (BookViewModel.RealObject.Style.Styles["page"] as PageStyle).Background != null)
                {
                    if ((BookViewModel.RealObject.Style.Styles["page"] as PageStyle).IsPortrait)
                        return MenuPresentationModel.MenuStyles.PageStyle.ResourcesRootPath + (BookViewModel.RealObject.Style.Styles["page"] as PageStyle).Background.PortraitUri;
                    else
                        return MenuPresentationModel.MenuStyles.PageStyle.ResourcesRootPath + (BookViewModel.RealObject.Style.Styles["page"] as PageStyle).Background.LandscapeUri;
                }
                return null;


            }
        }

        public IStyleSheet Style
        {
            get
            {
                return MenuPage.Style;
            }
            //internal set
            //{
            //    if(MenuPage.Style!=null&& MenuPage.Style.Styles.ContainsKey("page"))
            //        (MenuPage.Style.Styles["page"] as MenuPresentationModel.MenuStyles.PageStyle).PropertyChanged -= BookPageViewModel_PropertyChanged;
            //    //MenuPage.Style = value;
            //    MenuPage.RenderMenuCanvasItems();
            //    if (MenuPage.Style != null && MenuPage.Style.Styles.ContainsKey("page"))
            //        (MenuPage.Style.Styles["page"] as MenuPresentationModel.MenuStyles.PageStyle).PropertyChanged += BookPageViewModel_PropertyChanged;

            //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ItemsToShowInCanvas)));
            //}
        }

        public bool IsReadonly
        {
            get
            {
                return OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(MenuPage) == null || OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(MenuPage).IsReadonly;
            }
        }

        private void BookPageViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {

            if (e.PropertyName == nameof(IPageStyle.Border))
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BorderAsStream)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BorderDrawing)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BorderVisibility)));


                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BorderMargin)));
            }
            else if (e.PropertyName == nameof(IPageStyle.BorderMargin))
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BorderMargin)));
            }
            else if (e.PropertyName == nameof(IPageStyle.Background))
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BackgroundUri)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BackgroundMargin)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BackgroundScaleX)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BackgroundScaleY)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BackgroundOffsetX)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BackgroundOffsetY)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BackgroundOpacity)));
            }
            else if (e.PropertyName == nameof(IPageStyle.BackgroundMargin))
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BackgroundMargin)));
            }
            else if (e.PropertyName == nameof(IPageStyle.BackgroundStretch))
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BackgroundStretch)));
            }
            else if (e.PropertyName == nameof(IPageStyle.PageHeight) ||
                e.PropertyName == nameof(IPageStyle.PageWidth) ||
                e.PropertyName == nameof(IPageStyle.PageSize))
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BorderDrawing)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BorderVisibility)));
            }
            else
            {


                //var menuCanvasItems = MenuPage.MenuCanvasItems.ToList();
                //MenuPage.RenderMenuCanvasItems(menuCanvasItems);
                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PageHeight)));
                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PageWidth)));
                ////PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ItemsToShowInCanvas)));
                //UpdateCanvasItems();
            }
        }

        public BookPageViewModel(IMenuPageCanvas menuPage)
        {
            _MenuPage = menuPage as MenuPage;
            //_MenuPage.Menu.MenuStyleChanged += MenuStyleChanged;
            menuPage.ObjectChangeState += MenuPage_ObjectChangeState;



        }

        private void MenuPage_ObjectChangeState(object _object, string member)
        {

            //ObjectChangeState?.Invoke(this, nameof(ItemsToShowInCanvas));
            UpdateCanvasItems(MenuPage);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ItemsToShowInCanvas)));
        }

        //private void MenuStyleChanged(IStyleSheet oldStyle, IStyleSheet newStyle)
        //{

        //    if (oldStyle != null && oldStyle.Styles.ContainsKey("page"))
        //        (oldStyle.Styles["page"] as MenuPresentationModel.MenuStyles.PageStyle).PropertyChanged -= BookPageViewModel_PropertyChanged;

        //    var menuCanvasItems = MenuPage.Menu.MenuCanvasItems.ToList();
        //    foreach (var menuPage in MenuPage.Menu.Pages)
        //        menuPage.RenderMenuCanvasItems(menuCanvasItems);

        //    if (newStyle != null && newStyle.Styles.ContainsKey("page"))
        //        (newStyle.Styles["page"] as MenuPresentationModel.MenuStyles.PageStyle).PropertyChanged += BookPageViewModel_PropertyChanged;

        //    UpdateCanvasItems();
        //    //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ItemsToShowInCanvas)));
        //}

        /// <MetaDataID>{91c8ec2a-3fda-4180-a7d8-b34e4bcd5056}</MetaDataID>
        //public BookPageViewModel(XElement pageXML)
        //{

        //    IEnumerable<XElement> itemsXML = pageXML.Elements("DesignerItems").Elements("DesignerItem");
        //    foreach (XElement itemXML in itemsXML)
        //    {
        //        Guid id = new Guid(itemXML.Element("ID").Value);
        //        DesignerItem item = DeserializeDesignerItem(itemXML, id, 0, 0);
        //        DesignerItems.Add(item);
        //    }
        //}


        /// <MetaDataID>{2a5253bb-3fa4-41bb-8783-209b1f3386be}</MetaDataID>
        public BookPageViewModel()
        {

        }

        public BookPageViewModel(IMenuPageCanvas menuPage, BookViewModel bookViewModel) : this(menuPage)
        {
            this.BookViewModel = bookViewModel;
            this.BookViewModel.PropertyChanged += BookViewModel_PropertyChanged;

            if (bookViewModel.MenuStylesheet != null && bookViewModel.MenuStylesheet.Styles.ContainsKey("page"))
                (bookViewModel.MenuStylesheet.Styles["page"] as MenuPresentationModel.MenuStyles.PageStyle).PropertyChanged += BookPageViewModel_PropertyChanged;
        }

        public BookPageViewModel(BookViewModel bookViewModel)
        {
            this.BookViewModel = bookViewModel;
            this.BookViewModel.PropertyChanged += BookViewModel_PropertyChanged;

            if (bookViewModel.MenuStylesheet != null && bookViewModel.MenuStylesheet.Styles.ContainsKey("page"))
                (bookViewModel.MenuStylesheet.Styles["page"] as MenuPresentationModel.MenuStyles.PageStyle).PropertyChanged += BookPageViewModel_PropertyChanged;
        }

        private void BookViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(BookViewModel.SelectedMenuStyle))
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BorderUri)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BorderAsStream)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BorderDrawing)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BorderVisibility)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BorderMargin)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BackgroundUri)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BackgroundMargin)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BackgroundStretch)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PageWidth)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PageHeight)));

            }
        }


        ///// <MetaDataID>{8c143893-ca7f-4732-bbd6-b6c496a4cee1}</MetaDataID>
        //private static DesignerItem DeserializeDesignerItem(XElement itemXML, Guid id, double OffsetX, double OffsetY)
        //{
        //    DesignerItem item = new DesignerItem(id, null);
        //    item.Width = Double.Parse(itemXML.Element("Width").Value, CultureInfo.InvariantCulture);
        //    item.Height = Double.Parse(itemXML.Element("Height").Value, CultureInfo.InvariantCulture);
        //    item.ParentID = new Guid(itemXML.Element("ParentID").Value);
        //    item.IsGroup = Boolean.Parse(itemXML.Element("IsGroup").Value);
        //    Canvas.SetLeft(item, Double.Parse(itemXML.Element("Left").Value, CultureInfo.InvariantCulture) + OffsetX);
        //    Canvas.SetTop(item, Double.Parse(itemXML.Element("Top").Value, CultureInfo.InvariantCulture) + OffsetY);
        //    Canvas.SetZIndex(item, Int32.Parse(itemXML.Element("zIndex").Value));
        //    Object content = XamlReader.Load(XmlReader.Create(new StringReader(itemXML.Element("Content").Value)));
        //    item.Content = content;
        //    return item;
        //}
        public event PropertyChangedEventHandler PropertyChanged;

        //internal void OnDragLeave(DragEventArgs e)
        //{


        //    foreach (var canvasItem in ItemsToShowInCanvas)
        //        canvasItem.DragDropOn = false;

        //}

        //internal void OnDragEnter(DragEventArgs e)
        //{
        //    foreach (var canvasItem in ItemsToShowInCanvas)
        //        canvasItem.DragDropOn = true;
        //    Refresh();
        //    //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ItemsToShowInCanvas)));
        //}

        ///// <MetaDataID>{1d279f8c-63c5-41a3-a22c-d6577aea723b}</MetaDataID>
        //internal List<DesignerItem> DesignerItems = new List<DesignerItem>();
        ///// <MetaDataID>{f117ce2f-5ff8-40e8-8fd6-8c31dd1bdac5}</MetaDataID>
        //internal void AddDesignerItem(DesignerItem designerItem)
        //{
        //    if (designerItem != null && !DesignerItems.Contains(designerItem))
        //        DesignerItems.Add(designerItem);
        //}
        ///// <MetaDataID>{cfffe366-2f41-4b62-b7b0-97ad3f2d0466}</MetaDataID>
        //internal void RemoveDesignerItem(DesignerItem designerItem)
        //{
        //    if (designerItem != null && DesignerItems.Contains(designerItem))
        //        DesignerItems.Add(designerItem);
        //}
    }

    /// <MetaDataID>{4ba89366-1250-43f1-96cf-7f9667bff6cc}</MetaDataID>
    struct AccentImageKey
    {
        public IMenuCanvasAccent HeadingAccent;
        public int AccentIndex;

        public AccentImageKey(IMenuCanvasAccent accent, int i)
        {
            HeadingAccent = accent;
            AccentIndex = i;
        }
        public static bool operator ==(AccentImageKey left, AccentImageKey right)
        {
            return left.AccentIndex == right.AccentIndex && left.HeadingAccent == right.HeadingAccent;
        }
        public static bool operator !=(AccentImageKey left, AccentImageKey right)
        {
            return !(left == right);
        }
    }

}
