using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using FinanceFacade;

namespace CashierStationDevice
{
  
    /// <MetaDataID>{6a6a78fc-14d2-463e-ba92-5f1d0cdc7af8}</MetaDataID>
    public class SignatureData
    {
        /// <MetaDataID>{f433e43b-215e-4b87-9338-00c8b7c8b3a7}</MetaDataID>
        public string Signuture;
        /// <MetaDataID>{f72ff942-add9-44ed-ad4a-82471e69b5db}</MetaDataID>
        public string QRCode;
        /// <MetaDataID>{15a3e4ee-6b15-4803-a53b-bd6b910b76c4}</MetaDataID>
        public string Error;
    }

    /// <MetaDataID>{8a0d2caa-73f7-4246-9d7a-761e7c615ec4}</MetaDataID>
    public interface IDocumentSignDevice
    {
        /// <MetaDataID>{aede50b0-94d9-489d-8004-43b48f0cf455}</MetaDataID>
        SignatureData SignDocument(string document, EpsilonLineData epsilonLineData);
        ///// <MetaDataID>{25b2b301-3c76-4ae2-a527-20b718ec4660}</MetaDataID>
       // string PrepareEpsilonLine(EpsilonLineData epsilonLineData);

        /// <MetaDataID>{54a4b79f-96ff-417b-b6d1-cc6f37d3d50c}</MetaDataID>
        bool IsOnline { get; }

        /// <MetaDataID>{e61c0373-b9cc-4c85-94e5-b1e52ab602cf}</MetaDataID>
        List<string> CheckStatusForError();
        event EventHandler<EventArgs> DeviceStatusChanged;
    }

    /// <MetaDataID>{dc5829b2-2002-46cd-a6d6-78d83524f4ec}</MetaDataID>
    public abstract class DocumentSignDevice : IDocumentSignDevice
    {
        /// <MetaDataID>{839dfd31-5e97-45dd-979b-f031ff5b3b2f}</MetaDataID>
        public abstract bool IsOnline { get; }

        public abstract event EventHandler<EventArgs> DeviceStatusChanged;

        /// <MetaDataID>{65f3531b-2c42-41a7-bd1f-4ddc1e79d8e2}</MetaDataID>
        public abstract List<string> CheckStatusForError();

        /// <MetaDataID>{92dc3ec1-416a-4e37-8471-108f4181322c}</MetaDataID>
        public abstract string PrepareEpsilonLine(EpsilonLineData epsilonLineData);

        /// <MetaDataID>{4b6cc4f2-bd55-4ab9-a141-767ca18cd9bc}</MetaDataID>
        public abstract SignatureData SignDocument(string document, EpsilonLineData epsilonLineData);

        /// <MetaDataID>{7dbf720f-5fba-49ed-be53-7d53eea2bd88}</MetaDataID>
        static IDocumentSignDevice _CurrentDocumentSignDevice;
        /// <MetaDataID>{3bbfd8a1-db71-4ba3-96b5-4922d11e714e}</MetaDataID>
        public static IDocumentSignDevice CurrentDocumentSignDevice
        {
            get
            {
                if (_CurrentDocumentSignDevice == null)
                    _CurrentDocumentSignDevice = new SamtecNext();

                return _CurrentDocumentSignDevice;
            }
        }

        /// <MetaDataID>{4b29ac26-4d1c-4179-8600-bd9032274035}</MetaDataID>
        public static void Init(IDocumentSignDevice documentSignDevice)
        {

            _CurrentDocumentSignDevice = documentSignDevice;
        }

    }

    

    /// <MetaDataID>{f94d8727-ac50-4dc5-a0e2-196c0bd469d1}</MetaDataID>
    public class EpsilonLineData
    {
        /// <MetaDataID>{264bdcb0-8ea6-4fd7-b30b-2498f7aa95cd}</MetaDataID>
        public string afm_publisher;
        /// <MetaDataID>{e2872758-c255-4170-8358-1c4b25ddec52}</MetaDataID>
        public string afm_recipient;

        /// <MetaDataID>{810456e2-9dc3-4fed-93eb-123587c6d875}</MetaDataID>
        public string transactionTypeID;
        /// <MetaDataID>{043ef1ba-dd94-4a64-9e82-bc1543ca5ed8}</MetaDataID>
        public string series;
        /// <MetaDataID>{a65e5997-8ff8-4b26-bf14-de14b4f12020}</MetaDataID>
        public string taxDocNumber;
        /// <MetaDataID>{e3baaedd-8b35-4b8e-8b18-4f960b336f1c}</MetaDataID>
        public decimal net_a;
        /// <MetaDataID>{fdb4efa5-c23f-44ae-b6ce-40c953efd154}</MetaDataID>
        public decimal net_b;
        /// <MetaDataID>{8e70d8c2-6147-4534-b670-fb3ed4fb88f8}</MetaDataID>
        public decimal net_c;
        /// <MetaDataID>{91f253d5-c6f0-46a5-8b40-fa7f6ab65591}</MetaDataID>
        public decimal net_d;
        /// <MetaDataID>{a8f92d42-00a9-4464-93b2-21733848cb08}</MetaDataID>
        public decimal net_e;
        /// <MetaDataID>{578ad4d1-2744-4273-be8f-40dfa9c75d90}</MetaDataID>
        public decimal vat_a;
        /// <MetaDataID>{9480690a-ac7e-488f-aa49-22594f6500cc}</MetaDataID>
        public decimal vat_b;
        /// <MetaDataID>{1c47b3bf-f6a4-48f1-a40e-1cc5faf2c1d2}</MetaDataID>
        public decimal vat_c;
        /// <MetaDataID>{abfbb1d5-493a-40cd-9ef6-04a0654ceb7d}</MetaDataID>
        public decimal vat_d;
        /// <MetaDataID>{77a0602b-e95a-46cd-a551-67bfbee393b9}</MetaDataID>
        public decimal total_to_pay_poso;
    }







}


//7.1. Table 1, Reply codes / error codes
//+---+----------------------------------+----------------------------------------+
//|Hex| Meaning                          | Suggested action                       |
//+---+----------------------------------+----------------------------------------+
//| 00| No errors - success              | None                                   |
//| 01| Wrong number of fields           | Check the command's field count        |
//| 02| Field too long                   | A field is long: check it & retry      |
//| 03| Field too small                  | A field is small: check it & retry     |
//| 04| Field fixed size mismatch        | A field size is wrong: check it & retry|
//| 05| Field range or type check failed | Check ranges or types in command       |
//| 06| Bad request code | Correct the request code(unknown)                      |
//| 09| Printing type bad | Correct the specified printing style                  |
//| 0A| Cannot execute with day open | Issue a Z report to close the day          |
//| 0B| RTC programming requires jumper | Short the 'clock' jumper and retry      |
//| 0C| RTC date or time invalid | Check the date / time range.Also check         |
//|   |                                  | if date is prior to a date of a fiscal |
//|   |                                  | record.                                |
//| 0D| No records in fiscal period | No suggested action; the operation can-     |
//|   |                                  | not be executed in the specified period|
//| 0E| Device is busy in another task   | Wait for the device to get ready       |
//| 0F| No more header records allowed   | No suggested action; the header pro-   |
//|   |                                  | gramming cannot be executed because the|
//|   |                                  | fiscal memory cannot hold more records |
//| 10| Cannot execute with block open   | The specified command requires no open |
//|   |                                  | signature block for proceeding.Close   |
//|   |                                  | the block and retry.                   |
//| 11| Block not open                   | The specified command requires a signa-|
//|   |                                  | ture block to be open to execute.Open  |
//|   |                                  | a block and retry.                     |
//| 12| Bad data stream                  | Means that the passed data to be signed|
//|   |                                  | are of incorrect format. The expected  |
//|   |                                  | format is in HEX (hexadecimal) pairs,  |
//|   |                                  | so expected field must have an even    |
//|   |                                  | size and its contents must be in range |
//|   |                                  | '0'-'9' or 'A'-'F' inclusive.          |
//| 13| Bad signature field              | Means that the passed signature is of  |
//|   |                                  | incorrect format.The expected format   |
//|   |                                  | is of 40 characters formatted as 20 HEX|
//|   |                                  | (hexadecimal) pairs.                   |
//| 14| Z closure time limit             | Means that 24 hours passed from the    |
//|   |                                  | last Z closure.Issue a Z and retry.    |
//| 15| Z closure not found              | The specified Z closure number does not|
//|   |                                  | exist.Pass an existing Z number.       |
//| 16| Z closure record bad             | The requested Z record is unreadable   |
//|   |                                  | (damaged). Device requires service     |
//| 17| User browsing in progress        | The user is accessing the device by    |
//|   |                                  | manual operation.The protocol usage    |
//|   |                                  | is suspended until the user terminates |
//|   |                                  | the keyboard browsing.Just wait or     |
//|   |                                  | inform application user.               |
//| 18| Signature daily limit reached    | The max number of signatures in a day  |
//|   |                                  | have been issued.A Z closure is needed |
//|   |                                  | to free the daily storage memory.      |
//| 19| Printer paper end detected       | Replace the paper roll and retry       |
//| 1A| Printer is offline               | Printer disconnection. Service required|
//| 1B| Fiscal unit is offline           | Fiscal disconnection. Service required.|
//| 1C| Fatal hardware error             | Mostly fiscal errors.Service required. |
//| 1D| Fiscal unit is full              | Need fiscal replacement.Service        |
//| 1E| No data passed for signature     | Need to pass some data to close block  |
//| 1F| Signature does not exist         | Correct requested signature number     |
//| 20| Battery fault detected           | If problem persists, service required  |
//| 21| Recovery in progress             | This command is not allowed when a     |
//|   |                                  | recovery has started.Finish the        |
//|   |                                  | recovery procedure and retry           |
//| 22| Recovery only after CMOS reset   | Attempted to initiate a recovery       |
//|   |                                  | procedure without a previous CMOS      |
//|   |                                  | reset.The recovery is not needed.     |
//| 23| Real-Time Clock needs programming| This means that the RTC has invalid    |
//|   |                                  | data and needs to be reprogrammed. As  |
//|   |                                  | a consequence, service is needed.      |
//| 24| Z closure date warning           | This is an error returned by a closure |
//|   |                                  | request, when the RTC's date has a     |
//|   |                                  | value at least 48 hours later than the |
//|   |                                  | last closure time stamp (see XZreport) |
//| 25| Bad character in stream          | This error is returned when a stream   |
//|   |                                  | sent contains one or more invalid      |
//|   |                                  | characters.A table of allowed binary  |
//|   |                                  | values is defined in 'table 2'. This   |
//|   |                                  | error means that device has rejected   |
//|   |                                  | the specified frame.A filtering of    |
//|   |                                  | data sent to the device* must* be      |
//|   |                                  | performed by host.                     |
//+---+----------------------------------+----------------------------------------+


