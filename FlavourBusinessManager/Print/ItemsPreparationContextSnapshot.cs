using FinanceFacade;
using FlavourBusinessFacade.RoomService;
using FlavourBusinessManager.Printing;
using FlavourBusinessManager.RoomService;
using MenuModel.JsonViewModel;
using OOAdvantech.Json;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Remoting.RestApi.Serialization;
using OOAdvantech.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace FlavourBusinessManager.Printing
{
    /// <summary>
    /// Specifies the snapshots of the items group which are related to meal course and prepared at specific preparation station.
    /// When the user commits to the items of the order, then for the part of the items that concerns a preparation station,
    /// the software creates a snapshot and print it. 
    /// In case user adds new item or change an already exist item, the software  create another snapshot and print it with Heading, order change.   
    /// </summary>
    /// <MetaDataID>{1c4e0cca-adaf-4bfb-9ec8-664cb16a7f43}</MetaDataID>
    [BackwardCompatibilityID("{1c4e0cca-adaf-4bfb-9ec8-664cb16a7f43}")]
    [Persistent()]
    public class ItemsPreparationContextSnapshots
    {
        /// <summary>
        /// Defines the identity of ItemsPreparationContext
        /// ItemsPreparationContext has the items of mealCourse which prepared at specific preparation station
        /// </summary>
        /// <MetaDataID>{5c6c332d-2ef9-4a46-b0b3-ec3b5dbce1a3}</MetaDataID>
        [PersistentMember()]
        [BackwardCompatibilityID("+2")]
        public string Identity;

        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;

        /// <MetaDataID>{33cee0f7-b075-4c0e-9cb9-6f91fab9f4e8}</MetaDataID>
        protected ItemsPreparationContextSnapshots()
        {

        }

        /// <MetaDataID>{7f0d2912-a88a-4eae-93fd-3f068761478d}</MetaDataID>
        [ObjectActivationCall]
        public void ObjectActivation()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(SnapshotsJson))
                    Snapshots = OOAdvantech.Json.JsonConvert.DeserializeObject<List<ItemsPreparationContextSnapshot>>(SnapshotsJson);
                else
                    Snapshots = new List<ItemsPreparationContextSnapshot>();


            }
            catch (Exception error)
            {

                throw;
            }
        }

        /// <MetaDataID>{29a91b72-ef1b-43c7-a7fa-91e78f248e60}</MetaDataID>
        [BeforeCommitObjectStateInStorageCall]
        public void BeforeCommitObjectState()
        {
            SnapshotsJson = OOAdvantech.Json.JsonConvert.SerializeObject(Snapshots);
        }

        /// <summary>
        /// Creates an object of "ItemsPreparationContextSnapshots" for the snapshots of items which prepared,
        /// at the preparation station and are related to specific meal course.
        /// </summary>
        /// <param name="itemsPreparationContext">
        /// Defines the items which prepared
        /// at the preparation station and are related to specific meal course.
        /// </param>
        /// <MetaDataID>{82ed8ff0-cb03-4585-b717-c0e23013ca45}</MetaDataID>
        public ItemsPreparationContextSnapshots(ItemsPreparationContext itemsPreparationContext)
        {
            Identity = GetIdentity(itemsPreparationContext);

            Timestamp = DateTime.UtcNow;

            MealCourse = itemsPreparationContext.MealCourse;
            ItemsPreparationContext = itemsPreparationContext;

            Snapshots = new List<ItemsPreparationContextSnapshot>() { new ItemsPreparationContextSnapshot(itemsPreparationContext.PreparationItems) };


            SnapshotsJson = JsonConvert.SerializeObject(Snapshots);
        }

        /// <summary>
        /// Creates the identity of the items group which are related to meal course and prepared at specific preparation station
        /// The identity is the combine of the preparation station identity  and the persistent uri of meal course.
        /// </summary>
        /// <param name="itemsPreparationContext">
        /// Defines the items which prepared
        /// at the preparation station and are related to specific meal course.
        /// </param>
        /// <returns>
        /// Return the combined identity
        /// </returns>
        /// <MetaDataID>{7c22a862-081d-44c7-906e-40996df8ba14}</MetaDataID>
        public static string GetIdentity(ItemsPreparationContext itemsPreparationContext)
        {
            string mealCourseUri = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(itemsPreparationContext.MealCourse).GetPersistentObjectUri(itemsPreparationContext.MealCourse);
            string preparationStationIdentity = itemsPreparationContext.PreparationStationIdentity;
            return preparationStationIdentity + ";" + mealCourseUri;
        }

        /// <summary>
        /// This operation produce a signature for items snapshot
        /// Each snapshot has a specific signature related to the items, the items quantities and the options change
        /// </summary>
        /// <param name="itemsPreparationContext">
        /// Defines the items which prepared
        /// at the preparation station and are related to specific meal course.
        /// </param>
        /// <returns>
        /// The snapshot signature
        /// </returns>
        /// <MetaDataID>{cbf59990-2602-4edc-8f1c-508f1a03e5ae}</MetaDataID>
        public static string GetSnapshotSignature(ItemsPreparationContext itemsPreparationContext)
        {
            string snapshotIdentity = "";
            foreach (ItemPreparation itemPreparation in itemsPreparationContext.PreparationItems)
            {
                snapshotIdentity += itemPreparation.uid + "_" + itemPreparation.Quantity;
                foreach (var optionChange in itemPreparation.OptionsChanges)
                    snapshotIdentity += (optionChange as OptionChange).OptionUri + (optionChange as OptionChange).NewLevelUri;
            }
            return snapshotIdentity;
        }

        /// <summary>
        /// This operation produce a signature for items snapshot
        /// Each snapshot has a specific signature related to the items, the items quantities and the options change
        /// </summary>
        /// <param name="itemsPreparationContextSnapshot">
        /// Defines a items preparation snapshot
        /// </param>
        /// <returns>
        /// The snapshot signature
        /// </returns>
        public static string GetSnapshotSignature(ItemsPreparationContextSnapshot itemsPreparationContextSnapshot)
        {
            string snapshotIdentity = "";
            foreach (ItemPreparationSnapshot itemPreparation in itemsPreparationContextSnapshot.PreparationItems)
            {
                snapshotIdentity += itemPreparation.uid + "_" + itemPreparation.Quantity;
                foreach (var optionChange in itemPreparation.OptionsChanges)
                    snapshotIdentity += (optionChange as OptionChangeSnapshot).OptionUri + (optionChange as OptionChangeSnapshot).NewLevelUri;
            }
            return snapshotIdentity;
        }

        /// <summary>
        ///Creates a printing document for the snapshot 
        /// </summary>
        /// <param name="itemsPreparationContextSnapshot"></param>
        /// <param name="companyHeader"></param>
        /// <param name="servicePointDescriptionLine"></param>
        /// <param name="comments"></param>
        /// <returns></returns>
        /// <MetaDataID>{89e1c246-7a46-414f-8173-732b9ab81894}</MetaDataID>
        public string GeRawPrint(ItemsPreparationContextSnapshot itemsPreparationContextSnapshot, FlavourBusinessManager.Printing.CompanyHeader companyHeader, string servicePointDescriptionLine, string comments)
        {
            List<ItemPreparationSnapshot> preparationItems = itemsPreparationContextSnapshot.PreparationItems;
            //RawPrint = GeRawPrint(itemsPreparationContext, new CompanyHeader(), "", "");

            System.Collections.Generic.Dictionary<string, decimal> taxesSums = new System.Collections.Generic.Dictionary<string, decimal>();

            var stream = typeof(ItemsPreparationContextSnapshots).Assembly.GetManifestResourceStream("FlavourBusinessManager.Resources.order.bin");
            byte[] data = new byte[stream.Length];
            stream.Read(data, 0, (int)stream.Length);


            string printReport = System.Text.Encoding.UTF8.GetString(data, 0, data.Length);

            System.IO.StringWriter rawPrintingWriter = new System.IO.StringWriter();
            System.IO.StringReader report = new System.IO.StringReader(printReport);


            string lineString = report.ReadLine();

            #region writes receipt header area
            while (lineString != null && lineString.IndexOf("&") == -1)//Ampersand & defines the end of header area and the start of details
            {  

                #region DateTime line
                if (lineString.IndexOf("^") != -1 || lineString.IndexOf("%") != -1)
                {
                    int len = lineString.Length;

                    int year = System.DateTime.Now.Year - 2000;



                    string yearstr = year.ToString();
                    if (yearstr.Length == 1)
                        yearstr = "0" + yearstr;

                    lineString = lineString.Replace("^", string.Format("{0}/{1}/{2}", DateTime.Now.Day, DateTime.Now.Month, yearstr));

                    string hourStr = DateTime.Now.Hour.ToString();
                    if (hourStr.Length == 1)
                        hourStr = "0" + hourStr;

                    string minuteStr = DateTime.Now.Minute.ToString();
                    if (minuteStr.Length == 1)
                        minuteStr = "0" + minuteStr;
                    lineString = lineString.Replace("%", string.Format("{0}:{1}", hourStr, minuteStr));
                }
                #endregion

                // Series receipt number
                string series_autonumber = "";// (transaction.GetPropertyValue("Series") + "  " + transaction.GetPropertyValue("AutoNumber")).Trim();

                if (lineString.IndexOf("@") != -1)
                    lineString = FixLengthReplace(series_autonumber, lineString, "@").Trim();

                //table number 
                #region Defines the table (service point) description 
                if (lineString.IndexOf("`") != -1)
                {
                    if (!string.IsNullOrEmpty(servicePointDescriptionLine))
                        lineString = FixLengthReplace(servicePointDescriptionLine, lineString, "`");
                    else
                    {
                        lineString = report.ReadLine();
                        continue;
                    }
                }
                #endregion

                #region company header
                //string title,
                if (lineString.IndexOf("$") != -1)
                    lineString = FixLengthReplace(companyHeader.Title, lineString, "$").Trim();

                //string productUser,
                if (lineString.IndexOf("=") != -1)
                    lineString = FixLengthReplace(companyHeader.Subtitle, lineString, "=").Trim();


                //string address,
                if (lineString.IndexOf("£") != -1)
                    lineString = FixLengthReplace(companyHeader.Address, lineString, "£").Trim();

                //string organizationVat,
                if (lineString.IndexOf("§") != -1)
                    lineString = FixLengthReplace(companyHeader.FiscalData, lineString, "§").Trim();

                //string ContuctInfo,
                if (lineString.IndexOf("©") != -1)
                    lineString = FixLengthReplace(companyHeader.ContatInfo, lineString, "©").Trim();
                #endregion

                //Order Code
                if (lineString.IndexOf("~") != -1)
                {
                    string orderCode = MealCourse.SortID.ToString();

                    if (!string.IsNullOrWhiteSpace(orderCode))
                        lineString = FixLengthReplace(orderCode, lineString, "~");
                    lineString = null;
                }
                if (lineString != null)
                    rawPrintingWriter.WriteLine(lineString);
                lineString = report.ReadLine();
            }

            #endregion

            string transactionItemTemplateLine = lineString;
            string ingrSpace = null;

            lineString = report.ReadLine();
            while (lineString != null && lineString.IndexOf("##") == -1)
            {
                ingrSpace += lineString;
                lineString = report.ReadLine();
            }


            foreach (var preparationItem in preparationItems)
            {
                //foreach (var taxAmount in transactionItem.Taxes)
                //{
                //    if (!taxesSums.ContainsKey(taxAmount.AccountID))
                //        taxesSums.Add(taxAmount.AccountID, 0);

                //    decimal totalOnVat = taxesSums[taxAmount.AccountID];
                //    totalOnVat += transactionItem.Price;
                //    taxesSums[taxAmount.AccountID] = transactionItem.Price;
                //}


                string currOrderItemLine = transactionItemTemplateLine;
                currOrderItemLine = FixLengthReplace(((double)preparationItem.Quantity).ToString(), currOrderItemLine, "&", null, TextJustify.Right);
                currOrderItemLine = FixLengthReplace(preparationItem.Description, currOrderItemLine, "*");
                currOrderItemLine = FixLengthReplace(GetPriceAsString(preparationItem.ModifiedItemPrice), currOrderItemLine, "!", "!!", TextJustify.Right);

                //foreach (var taxAmount in transactionItem.Taxes)
                currOrderItemLine = FixLengthReplace("", currOrderItemLine, "|", null, TextJustify.Right);

                rawPrintingWriter.WriteLine(currOrderItemLine);
            }

            lineString = report.ReadLine();
            string neglineString = "";

            #region writes the receipt footer
            while (lineString != null) //footer
            {

                #region Discount
                if (lineString.IndexOf("?!") != -1)
                {
                    string discDescription = "";
                    //if (transaction.DiscountRate != 0)
                    //{
                    //    discDescription = (transaction.DiscountRate * 100).ToString() + "%";
                    //}

                    //if (transaction.DiscountAmount != 0)
                    //{
                    //    if (string.IsNullOrWhiteSpace(discDescription))
                    //        discDescription = transaction.DiscountAmount.ToString("C");
                    //    else
                    //        discDescription = discDescription + " & " + transaction.DiscountAmount.ToString("C");
                    //}

                    lineString = FixLengthReplace(discDescription, lineString, "?!");
                    if (string.IsNullOrWhiteSpace(discDescription))
                        lineString = "";
                }

                #endregion

                decimal amount = GetAmount(preparationItems);
                // Total
                if (lineString.IndexOf("?") != -1)
                    lineString = FixLengthReplace(GetPriceAsString(amount), lineString, "?");


                //string thankfull message,
                if (lineString.IndexOf("¥") != -1)
                    lineString = FixLengthReplace(companyHeader.Thankful, lineString, "¥").Trim();


                // Comments
                if (lineString.IndexOf("!@#$%^") != -1)
                {
                    System.IO.StringReader LastString = new System.IO.StringReader(comments);
                    string LastStringline = LastString.ReadLine();
                    if (LastStringline == null)
                        lineString = "";

                    while (LastStringline != null)
                    {
                        string NextLine = LastString.ReadLine();
                        if (NextLine != null)
                            rawPrintingWriter.WriteLine(LastStringline);
                        else
                            lineString = LastStringline;
                        LastStringline = NextLine;
                    }
                }

                // aade document sign 
                if (lineString.IndexOf("!@") != -1)
                {
                    //if (transaction.GetPropertyValue("Signature") != null)
                    //{

                    //    string docSignature = transaction.GetPropertyValue("Signature");
                    //    do
                    //    {
                    //        rawPrintingWriter.WriteLine(FixLengthReplaceWithRemain(docSignature, lineString, "!@", out docSignature));
                    //    }
                    //    while (docSignature != null);
                    //}
                }
                else if (lineString.IndexOf(" | ") != -1)
                {
                    // taxes sums
                    foreach (var entry in taxesSums)
                    {
                        string VatLine = "" + entry.Key.ToString() + "%   " + GetPriceAsString((double)entry.Value);//entry.Value.ToString();
                        rawPrintingWriter.WriteLine(VatLine);
                    }
                }
                else if (lineString.IndexOf("$qrcode$") != -1)
                {
                    // Qr code
                    string QRCode = GetQRCodeData(GetQRCode(ItemsPreparationContext));
                    if (!string.IsNullOrWhiteSpace(QRCode))
                    {
                        lineString = lineString.Replace("$qrcode$", QRCode);
                        rawPrintingWriter.WriteLine(lineString);
                    }
                    else
                        lineString = lineString.Replace("$qrcode$", "");

                }
                else
                {
                    rawPrintingWriter.WriteLine(lineString);
                    if (neglineString.Length > 0)
                    {
                        rawPrintingWriter.WriteLine(neglineString);
                        neglineString = "";
                    }
                }
                lineString = report.ReadLine();

            }



            #endregion
            string finalOrderStr = rawPrintingWriter.ToString();


            byte[] outByteStr = System.Text.ASCIIEncoding.Default.GetBytes(finalOrderStr);

            return finalOrderStr;




        }



        /// <MetaDataID>{b7cea776-6ede-4208-a202-08e57e95e3d9}</MetaDataID>
        private static string GetQRCode(ItemsPreparationContext itemsPreparationContext)
        {
            return "";
        }

        /// <MetaDataID>{ab559476-eb87-4015-87de-e5bdfba32a9e}</MetaDataID>
        private static decimal GetAmount(List<ItemPreparationSnapshot> preparationItems)
        {
            decimal amount = preparationItems.OfType<ItemPreparation>().Sum(x => (decimal)(x.ModifiedItemPrice * x.Quantity));
            return amount;
        }

        /// <MetaDataID>{8fb4e05e-6e4e-4905-a1a1-b53554af73c2}</MetaDataID>
        private static string FixLengthReplace(string text, string lineString, string signChar, string startSign = null, FlavourBusinessManager.Printing.TextJustify textJustify = TextJustify.Left)
        {
            if (text == null)
                text = "";

            if (startSign == null)
                startSign = signChar;
            int startPos = lineString.IndexOf(startSign);
            int endPos = lineString.LastIndexOf(signChar);


            string TableNumberStr = null;
            if (text.ToString().Length > endPos - startPos + 1)
                TableNumberStr = text.ToString().Substring(0, endPos - startPos + 1);
            else
                TableNumberStr = text.ToString();

            while (TableNumberStr.Length < endPos - startPos + 1)
            {
                if (textJustify == TextJustify.Left)
                    TableNumberStr += " ";
                else
                    TableNumberStr = TableNumberStr.Insert(0, " ");
            }




            string TableChars = null;
            while (startPos <= endPos)
            {
                startPos++;
                TableChars += signChar;
            }
            lineString = lineString.Replace(TableChars, TableNumberStr);
            int rtr = 0;
            return lineString;
        }


        /// <MetaDataID>{5a282bff-d032-41f3-8af9-bae5edd6da04}</MetaDataID>
        private static string GetPriceAsString(object price)
        {
            return string.Format("{0:N2}", price);
        }
        /// <MetaDataID>{6415718f-71f9-4c25-8c85-0683a7ca1f1b}</MetaDataID>
        private static string GetQRCodeData(string qrData)
        {
            //Convert ASCII/Decimal
            string ESC = Convert.ToString((char)27);
            string GS = Convert.ToString((char)29);
            string center = ESC + "a" + (char)1; //align center
            string left = ESC + "a" + (char)0; //align left
            string bold_on = ESC + "E" + (char)1; //turn on bold mode
            string bold_off = ESC + "E" + (char)0; //turn off bold mode
            string cut = ESC + "d" + (char)1 + GS + "V" + (char)66; //add 1 extra line before partial cut
            string initp = ESC + (char)64; //initialize printer
            string buffer = "";// store all the data that want to be printed"; //store all the data that want to be printed

            //buffer += "  .  \n"; //to enter or add newline: \n
            //buffer += left;
            //buffer += "This text is align left!\n"; //align left is already set as default if not specified
            //                                        //Print QRCode

            //Encoding.GetEncoding(1253);
            Encoding m_encoding = Encoding.GetEncoding("iso-8859-1"); //set encoding for QRCode
            int store_len = (qrData).Length + 3;
            byte store_pL = (byte)(store_len % 256);
            byte store_pH = (byte)(store_len / 256);
            buffer += initp; //initialize printer
            buffer += center;
            buffer += m_encoding.GetString(new byte[] { 29, 40, 107, 4, 0, 49, 65, 50, 0 });
            buffer += m_encoding.GetString(new byte[] { 29, 40, 107, 3, 0, 49, 67, 8 });
            buffer += m_encoding.GetString(new byte[] { 29, 40, 107, 3, 0, 49, 69, 48 });
            buffer += m_encoding.GetString(new byte[] { 29, 40, 107, store_pL, store_pH, 49, 80, 48 });
            buffer += qrData;
            buffer += m_encoding.GetString(new byte[] { 29, 40, 107, 3, 0, 49, 81, 48 });
            //Cut Receipt
            //buffer += cut + initp;
            //Send to Printer
            return buffer;
        }
        /// <MetaDataID>{34fd7936-c106-48b1-a7f9-a950386de9fc}</MetaDataID>
        private static string FixLengthReplaceWithRemain(string inputString, string lineString, string signChar, out string remainString)
        {
            string startSign = null;
            if (startSign == null)
                startSign = signChar;
            int startPos = lineString.IndexOf(startSign);
            int endPos = lineString.LastIndexOf(signChar) + signChar.Length - 1;


            string TableNumberStr = null;
            if (inputString.ToString().Length > endPos - startPos + 1)
            {
                TableNumberStr = inputString.ToString().Substring(0, endPos - startPos + 1);
                remainString = inputString.ToString().Substring(TableNumberStr.Length);
            }
            else
            {
                remainString = null;
                TableNumberStr = inputString.ToString();
            }

            while (TableNumberStr.Length < endPos - startPos + 1)
                TableNumberStr += " ";
            string TableChars = null;
            while (startPos <= endPos)
            {
                startPos += signChar.Length;
                TableChars += signChar;
            }
            lineString = lineString.Replace(TableChars, TableNumberStr);
            int rtr = 0;
            return lineString;
        }

        /// <summary>
        /// This operation updates the snapshots of the items which prepared
        /// at the preparation station and are related to specific meal course. 
        /// </summary>
        /// <param name="itemsPreparationContext">
        /// Defines the items which prepared
        /// at the preparation station and are related to specific meal course.
        /// </param>
        /// <MetaDataID>{d722d9cc-448b-4a30-802e-87a3c7d008d9}</MetaDataID>
        internal void Update(ItemsPreparationContext itemsPreparationContext)
        {

            var snapshotSignature = GetSnapshotSignature(itemsPreparationContext);
            ItemsPreparationContextSnapshot newSnapShot = null;

            lock (Snapshots)
            {
                if (!LastSnapshotHasTheSameSignature(snapshotSignature))
                {
                    newSnapShot = new ItemsPreparationContextSnapshot(itemsPreparationContext.PreparationItems);
                    this.Snapshots.Add(newSnapShot);
                }
            }
            if (newSnapShot != null)
            {

                (ServicePointRunTime.ServicesContextRunTime.Current.InternalPrintManager as PrintManager).OnNewPrinting();
                //new ItemsPreparationContextSnapshot(itemsPreparationContext.PreparationItems)
                Timestamp = DateTime.UtcNow;
                //}
            }


        }

        internal bool LastSnapshotHasTheSameSignature(string signature)
        {
            var lastSnapshot = Snapshots.OrderBy(x => x.TimeStamp).LastOrDefault();
            return lastSnapshot != null && GetSnapshotSignature(lastSnapshot) == signature;



        }

        /// <summary>
        /// This operation update the specific snapshot that it has been printed.
        /// </summary>
        /// <param name="documentIdentity">
        /// Defines the identity of snapshot
        /// </param>
        internal void SnapshotPrinted(string documentIdentity)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                var snapshot = Snapshots.Where(x => x.SnapshotIdentity == documentIdentity).FirstOrDefault();
                snapshot.Printed = true;
                stateTransition.Consistent = true;
            }

        }


     
        /// <MetaDataID>{1e5bf834-30aa-481e-a0c8-443235fc39c5}</MetaDataID>
        public object RawPrint { get; private set; }


        /// <summary>
        /// Defines the snapshots of the items which prepared
        /// at the preparation station and are related to specific meal course.
        /// Any time where client change the items which prepared
        /// at the preparation station and are related to specific meal course
        /// the software produce a  new snapshot 
        /// </summary>
        // <MetaDataID>{543c4627-7319-4b2b-9652-66c84b359bd0}</MetaDataID>
        public List<ItemsPreparationContextSnapshot> Snapshots;

        /// <summary>
        ///  Defines the date where ItemsPreparationContextSnapshots created
        /// </summary>
        /// <MetaDataID>{a8f415e0-453f-4d7c-af26-0af70b08902b}</MetaDataID>
        public DateTime Timestamp { get; private set; }

        /// <exclude>Excluded</exclude>
        IMealCourse _MealCourse;

        /// <summary>
        /// Defines the mealcoure of items
        /// </summary>
        public IMealCourse MealCourse
        {
            get
            {

                if (_MealCourse == null)
                {
                    string mealCourseUri = Identity.Split(';')[1];
                    _MealCourse = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this).GetObject(mealCourseUri) as MealCourse;
                }
                return _MealCourse;
            }
            private set
            {
                _MealCourse = value;
            }
        }

        /// <summary>
        /// Defines the items which prepared
        /// at the preparation station and are related to specific meal course.
        /// </summary>
        public ItemsPreparationContext ItemsPreparationContext { get; private set; }

        /// <summary>
        /// Defines the identity of preparation station where  prepared the items of snapshots
        /// </summary>
        /// <MetaDataID>{0dbb2d02-d520-4fe5-8f98-94b5105b4978}</MetaDataID>
        public string PreparationStationIdentity
        {
            get
            {
                return Identity.Split(';').FirstOrDefault();
            }
        }

        /// <MetaDataID>{683512d6-3824-4bad-bfe8-4a3c254c9a66}</MetaDataID>
        [PersistentMember]
        [BackwardCompatibilityID("+4")]
        private string SnapshotsJson;




        
    }

    /// <MetaDataID>{e98a4b3a-6d2f-4883-99ba-5baed071a0d9}</MetaDataID>
    enum TextJustify
    {
        Left = 1,
        Right = 2,
        Center = 3
    }

    /// <MetaDataID>{848685a6-a1e8-438c-8c27-a7d9ea983c05}</MetaDataID>
    public class CompanyHeader
    {
        /// <MetaDataID>{904deb7e-9e69-4ec4-b3ad-531334ee11f5}</MetaDataID>
        string _Title;
        /// <MetaDataID>{53f82f10-dca0-4511-90f8-b3e1158a90d2}</MetaDataID>
        public string Title
        {
            get => _Title;
            set
            {
                if (_Title != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Title = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
        /// <MetaDataID>{167876a5-1820-455c-aca4-233c72225fd3}</MetaDataID>
        string _Subtitle;
        /// <MetaDataID>{206009ec-6240-4335-a14f-a3d3590721d5}</MetaDataID>
        public string Subtitle
        {
            get => _Subtitle;
            set
            {
                if (_Subtitle != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Subtitle = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
        /// <MetaDataID>{69e25577-f93d-473b-a30e-768430ee626a}</MetaDataID>
        string _ContatInfo;//email telephone etc

        /// <MetaDataID>{c202f2f2-3b32-49f0-95e2-af79bb3e9eae}</MetaDataID>
        public string ContatInfo
        {
            get => _ContatInfo;
            set
            {
                if (_ContatInfo != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ContatInfo = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <MetaDataID>{1590b9d2-c40e-4a8d-a74d-9665d2f977b6}</MetaDataID>
        string _FiscalData;//vat and etc
        /// <MetaDataID>{2a622010-32f9-457a-b4fe-66902f556e9a}</MetaDataID>
        public string FiscalData
        {
            get => _FiscalData;
            set
            {
                if (_FiscalData != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _FiscalData = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <MetaDataID>{f379f0e3-ffd0-4119-8d34-2029d1947605}</MetaDataID>
        string _Address;// defines the company address

        /// <MetaDataID>{e507875b-b5c5-4ef8-a195-61843b537bed}</MetaDataID>
        public string Address
        {
            get => _Address;
            set
            {
                if (_Address != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Address = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <MetaDataID>{8e63f3db-c59c-4ec9-a865-84b8ea7de1b0}</MetaDataID>
        string _Thankful;
        /// <MetaDataID>{4cb9c309-6d2e-4927-9c56-d42c6f24b011}</MetaDataID>
        public string Thankful
        {
            get => _Thankful;
            set
            {
                if (_Thankful != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Thankful = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

    }

    /// <MetaDataID>{b4ac8c26-28bb-42cb-80a1-99b07d50e485}</MetaDataID>
    public class ItemsPreparationContextSnapshot
    {
        /// <MetaDataID>{f54668ba-c576-4fdb-a7cb-5b4523564681}</MetaDataID>
        public ItemsPreparationContextSnapshot(List<IItemPreparation> preparationItems)
        {
            //SnapshotIdentity = snapshotIdentity;

            var lastChangedItem = preparationItems.OfType<ItemPreparation>().OrderBy(x => x.StateTimestamp).Last();

            var ticks = new DateTime(2022, 1, 1).Ticks;
            var uniqueId = (lastChangedItem.StateTimestamp.Ticks - ticks).ToString("x");

            SnapshotIdentity = lastChangedItem.uid + "_" + uniqueId;
            //clone

            PreparationItems = preparationItems.OfType<ItemPreparation>().Select(x => new ItemPreparationSnapshot(x)).ToList();

            //string jsonEx = OOAdvantech.Json.JsonConvert.SerializeObject(preparationItems, JSonSerializeSettings.TypeRefSerializeSettings);

            //PreparationItems = OOAdvantech.Json.JsonConvert.DeserializeObject<List<IItemPreparation>>(jsonEx, JSonSerializeSettings.TypeRefDeserializeSettings);
        }
        public ItemsPreparationContextSnapshot()
        {

        }




        [JsonIgnore]
        public DateTime TimeStamp
        {
            get
            {
                var lastChangedItem = PreparationItems.OfType<ItemPreparationSnapshot>().OrderBy(x => x.StateTimestamp).Last();

                return lastChangedItem.StateTimestamp;

            }
        }



        /// <MetaDataID>{64d5d6f8-42e6-4d07-8222-af43f6c0abba}</MetaDataID>
        public string SnapshotIdentity { get; set; }
        /// <MetaDataID>{3c0d3788-a91a-4ec5-9b4e-1aa0310bac7a}</MetaDataID>
        public List<ItemPreparationSnapshot> PreparationItems { get; set; }

        /// <MetaDataID>{1da758ba-a7ce-4669-ad43-8f3fe2a2add0}</MetaDataID>
        public bool Printed { get; set; }
    }

}