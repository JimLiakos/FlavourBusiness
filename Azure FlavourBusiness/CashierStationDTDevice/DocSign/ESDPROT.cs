using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ESD_DTool.Helper
{
    /// <MetaDataID>{a99c0bf8-4ec4-4a4e-be94-102d14549564}</MetaDataID>
    class ESDPROT
    {
        #region  DLL references
        [DllImport("DocMsign.DLL")]
        public static extern Int16 FSL_CheckCFiles();

        [DllImport("DocMsign.DLL")]
        public static extern Int16 FSL_CheckCEFiles();

        [DllImport("DocMsign.DLL")]
        public static extern Int16 FSL_VerifyCFiles();

        [DllImport("DocMsign.DLL")]
        public static extern Int16 GetSignPath([MarshalAs(UnmanagedType.AnsiBStr)] ref string path);

        [DllImport("DocMsign.DLL")]
        public static extern Int16 VB_FSL_DisableCheckZFiles(int en);

        [DllImport("DocMsign.DLL")]
        public static extern Int16 FSL_Command(string strBase, string strCmd, string strDevFile, string strP1, string strP2);
        [DllImport("DocMsign.DLL")]
        public static extern Int16 RET_FSL_SignDocument(string VBstrInfile, [In, Out, MarshalAs(UnmanagedType.AnsiBStr)] ref string Sign, [In, Out, MarshalAs(UnmanagedType.AnsiBStr)] ref string FilePath);
        [DllImport("DocMsign.DLL")]
        public static extern Int16 CVB_FSL_SignDocument(string VBstrInfile, [MarshalAs(UnmanagedType.AnsiBStr)] ref string Sign);
        [DllImport("DocMsign.DLL")]
        public static extern Int16 N_FSL_SignDocument(string VBstrInfile, [MarshalAs(UnmanagedType.AnsiBStr)] ref string Sign, [MarshalAs(UnmanagedType.AnsiBStr)] ref string Sign2);

        [DllImport("DocMsign.DLL")]
        public static extern Int16 CVB_FSL_SelectDevice(string SerialNo, byte TType, string IPP, string VBstrBaseDir, byte port);

        [DllImport("DocMsign.DLL")]
        public static extern Int16 CVB_FSL_IssueZreport();
        [DllImport("DocMsign.DLL")]
        public static extern Int16 CVB_FSL_ReleaseDevice();
        [DllImport("DocMsign.DLL")]
        public static extern void VB_FSL_SetProgress(int fEnable);
        [DllImport("DocMsign.DLL")]
        public static extern void FSL_SetDebug(long fnDebug, byte strLogFilename, Boolean fDebugEnable);
        [DllImport("DocMsign.DLL")]
        public static extern void VB_FSL_SetDebug(int fnDebug);
        [DllImport("DocMsign.DLL")]
        public static extern void VB_FSL_ErrorToString(int iRet, [MarshalAs(UnmanagedType.AnsiBStr)] ref string strDescription, int ln);
        [DllImport("DocMsign.DLL")]
        public static extern void VB_FSL_ErrorsUI(int st);
        [DllImport("DocMsign.DLL")]
        public static extern Int16 FSL_SetLanguage(int st);  //'Gia ellhnika 0 
        [DllImport("DocMsign.DLL")]
        public static extern void FSL_SetBackup(string bk);
        [DllImport("DocMsign.DLL")]
        public static extern Int16 CVB_FSL_PopupControl();

        [DllImport("DocMsign.DLL")]
        public static extern Int16 SVC_PendingRecCFG(string bk);

        [DllImport("DocMsign.DLL")]
        public static extern void SVC_ShowPendingRecCFG();


        [DllImport("DocMsign.DLL")]
        public static extern Int16 SVC_Process(string workpath, string args);



        [DllImport("DocMsign.DLL")]
        public static extern Int16 XCVB_FSL_CheckData(string AFM_publisher, string AFMRecipient, string CustomerID, string InvoiceType, string Seira, string InvoiceNo, string NET_A, string NET_B, string NET_C, string NET_D, string NET_E, string VAT_A, string VAT_B, string VAT_C, string VAT_D, string Total, string Currency, [param: MarshalAs(UnmanagedType.LPTStr), Out()] StringBuilder Signature);

        [DllImport("DocMsign.DLL")]
        public static extern Int16 CVB_FSL_InvData(string AFM_publisher, string AFMRecipient, string CustomerID, string InvoiceType, string Seira, string InvoiceNo, string NET_A, string NET_B, string NET_C, string NET_D, string NET_E, string VAT_A, string VAT_B, string VAT_C, string VAT_D, string Total, string Currency, [In, Out, MarshalAs(UnmanagedType.AnsiBStr)] ref string Signature);
        [DllImport("DocMsign.DLL")]
        public static extern Int16 Upload_STXT_FILE(ref int ercode, string Server, int Port, string pathfilename, string UfileName, string password);
        [DllImport("DocMsign.DLL")]
        public static extern void SHA1_GetSignature(string data, [In, Out, MarshalAs(UnmanagedType.AnsiBStr)] ref string Signature20, [In, Out, MarshalAs(UnmanagedType.AnsiBStr)] ref string Signature40);
        [DllImport("DocMsign.DLL")]
        public static extern Int16 RegisterDevice(string key);

        [DllImport("DocMsign.DLL")]
        public static extern Int16 CVB_FSL_GetStat([MarshalAs(UnmanagedType.AnsiBStr)] ref string strStat);

        [DllImport("DocMsign.DLL")]
        public static extern Int16 XFSL_GetStat([MarshalAs(UnmanagedType.AnsiBStr)] ref string strStat);

        [DllImport("DocMsign.DLL")]
        public static extern Int16 CVB_FSL_GetSerialNo([MarshalAs(UnmanagedType.AnsiBStr)] ref string strsno);

        [DllImport("DocMsign.DLL")]
        public static extern Int16 CRegisterDevice(string key, int pos);
        [DllImport("DocMsign.DLL")]
        public static extern Int16 CReadRegisteredDevice([MarshalAs(UnmanagedType.AnsiBStr)] ref string key, int pos);

        [DllImport("DocMsign.DLL")]
        public static extern void N_FSL_Get_E_Range(string path, string DateStart, string DateEnd, string ZnoStart, string ZnoEnd, int type);

        [DllImport("DocMsign.DLL")]
        public static extern Int16 UploadPendingReceipts(ref int ercode, string Server, int Port, string password);
        #endregion


        public string RetErr = "";
        public bool usespath = true;
        public string firstpath = @"c:\out\";
        public string secondpath = @"c:\out\backup\";
        //  Util UTL = new Util();
        public string currentpath = "";

        public bool DebugActive = false;
        public bool ContinuouslyActive = false;
        public bool CheckingCfilesActive = false;
        public bool ProgressWinActive = true;



        public void ShowServiceCFG()
        {

            SVC_ShowPendingRecCFG();
        }

        public int ServiceCFGSet(string data, Devices dev)
        {
            int iRet = 0;
            int res = SelectESDDevice(dev);
            if (res == 0)
            {
                iRet = SVC_PendingRecCFG(data);
            }
            CVB_FSL_ReleaseDevice();
            return (iRet);
        }


        public int CMD_Service(string path, string args)
        {
            int iRet = 0;

            iRet = SVC_Process(path, args);
            if (iRet > 0)
            {
                VB_FSL_ErrorToString(iRet, ref RetErr, 100);
            }
            return (iRet);
        }


        public int ActivateDefaultDevice(Devices CDev)
        {

            int iRet = RegisterDevice(CDev.ActivationCode);

            return (iRet);
        }
        public int ActivateDevice(Devices CDev, int pos)
        {

            int iRet = CRegisterDevice(CDev.ActivationCode, pos);

            return (iRet);
        }

        void mydebug(byte strMessage)
        {
            Console.Write(strMessage);
        }

        public long conwrited(string message)
        {

            Console.Write(message);
            return 0;
        }
        internal void SetOutputFolder(string outputFolder)
        {
            if(string.IsNullOrWhiteSpace(outputFolder))
            {
                firstpath = CashierStationDevice.Properties.Settings.Default.fpath;
                secondpath = CashierStationDevice.Properties.Settings.Default.spath;
                usespath = CashierStationDevice.Properties.Settings.Default.enspath;
            }
            else
            {
                firstpath = outputFolder + @"\out\";
                secondpath = outputFolder + @"\out\backup\";
            }
        }


        public int SelectESDDevice(Devices CDEv)
        {
            int res = -1;
            string ReturnedErrorStr = "";
            string serno = CDEv.SerialNO;

            if (CheckingCfilesActive)
                VB_FSL_DisableCheckZFiles(0);
            else
                VB_FSL_DisableCheckZFiles(1);

            // string ddebug = "\\debuglog.txt";
            // byte debug1byte;
            // debug1byte = Convert.ToByte(ddebug);

            if (DebugActive)
                VB_FSL_SetDebug(1);                                                          // Disables the Library Debug messages  
                                                                                             // string debugpath = "C:\\temp\\debug.txt";
                                                                                             //FSL_SetDebug(conwrited("123"),Convert.ToByte(debugpath),true);

            else
                VB_FSL_SetDebug(0);


            FSL_SetLanguage(0);                                                              // 0 = Greek Language , 1= English

            //     if (ProgressWinActive)
            //    VB_FSL_SetProgress(1);                                                       // 0 = Disable Signing Progress Window, 1= Enable 
            //   else
            VB_FSL_SetProgress(1);                                                           // 0 = Disable Signing Progress Window, 1= Enable 
            VB_FSL_ErrorsUI(0);                                                              // 0= Disables The Warning - Error -Status Messages, 1= Enables these messages.

            if (usespath) FSL_SetBackup(secondpath);                                         // secondary backup path (Usually another disk)  
                                                                                             // serno = "***********";
            if (CDEv.IsEthernet)
            {

                res = CVB_FSL_SelectDevice(serno, 2, CDEv.EthernetIP, firstpath, 0);         // "***********"  = ESD Serial Number if all paces are '*' works with every serial.                 
                                                                                             //                                                                              // 2 = Connection type for Ethernet Communication 
                                                                                             //                                                                               // "192.168.0.10" = Ethernet IP Field In case of these type of connection 
                                                                                             //                                                                              // "C\out\ = Path where the _a, _b files are stored after signing procedure
                                                                                             //                                                                              // 0 =Ignored
            }
            else if (CDEv.IsProxy)
            {
                res = CVB_FSL_SelectDevice(serno, 3, CDEv.ProxyIP, firstpath, 0);            // "***********"  = ESD Serial Number if all paces are '*' works with every serial.                 
                                                                                             //                                                                              // 2 = Connection type for Proxy Communication 
                                                                                             //                                                                               // "192.168.0.10" = Proxy IP Field In case of these type of connection 
                                                                                             //                                                                              // "C\out\ = Path where the _a, _b files are stored after signing procedure
                                                                                             //    
            }
            else
            {
                res = CVB_FSL_SelectDevice(serno, 1, "", firstpath, CDEv.ComNo);             // "***********"  = ESD Serial Number if all paces are '*' works with every serial. 
                                                                                             //                                                                              // 1 = Connection type for Serial Communication 
                                                                                             //                                                                              // "" = Ethernet or Proxy IP Field In case of these type of connection 
                                                                                             //                                                                              // "C\out\ = Path where the _a, _b files are stored after signing procedure
                                                                                             //                                                                              // 1 =COM1
            }

            if (res > 0)
            {
                //     VB_FSL_ErrorToString(res, ref ReturnedErrorStr, 100);
                //     RetErr = ReturnedErrorStr;
                CVB_FSL_ReleaseDevice();
                return (res);
            }
            return (0);
        }

        public int Upload_Pending_DATA(Devices CDev, string data, ref string reply, ref int repeattm)
        {
            string ReturnedErrorStr = "";
            string[] dar = data.Split(';');
            string filename = "empty";
            int pp = 0;
            if (CDev.SerialNO != "***********") filename = CDev.SerialNO;
            resend:
            int res = SelectESDDevice(CDev);
            if (res == 0)
            {
                string[] sar = data.Split('|');
                res = UploadPendingReceipts(ref pp, sar[0], Convert.ToInt16(sar[1]), sar[2]);
                if (res > 0)
                {
                    VB_FSL_ErrorToString(res, ref ReturnedErrorStr, 100);
                    CVB_FSL_ReleaseDevice();
                    reply = ReturnedErrorStr;
                    RetErr = ReturnedErrorStr;
                    repeattm = pp;
                    return (res);
                }
                reply = "";
                repeattm = pp;
            }
            else
            {
                if (res == 0x0e) goto resend;
            }

            CVB_FSL_ReleaseDevice();
            return (res);
        }

        public int Upload_S_DATA(Devices CDev, string data, ref string reply)
        {
            string ReturnedErrorStr = "";
            string[] dar = data.Split(';');
            string filename = "empty";
            int pp = 0;
            if (CDev.SerialNO != "***********") filename = CDev.SerialNO;
            resend:
            int res = SelectESDDevice(CDev);
            if (res == 0)
            {
                string[] sar = data.Split('|');
                res = Upload_STXT_FILE(ref pp, sar[0], Convert.ToInt16(sar[1]), "", "", sar[2]);
                if (res > 0)
                {
                    VB_FSL_ErrorToString(res, ref ReturnedErrorStr, 100);
                    CVB_FSL_ReleaseDevice();
                    reply = ReturnedErrorStr;
                    return (res);
                }
                reply = pp.ToString();
            }
            else
            {
                if (res == 0x0e) goto resend;
            }

            CVB_FSL_ReleaseDevice();
            return (res);
        }
        public int Sign_E_DATA(Devices CDev, string data, ref string reply)
        {
            string ReturnedErrorStr = "";
            string[] dar = data.Split(';');
            string filename = "empty";
            if (CDev.SerialNO != "***********") filename = CDev.LastConnectedClient.Replace(".", "_");//CDev.SerialNO;
            resend:
            int res = SelectESDDevice(CDev);
            if (res == 0)
            {
                res = CVB_FSL_InvData(dar[0], dar[1], dar[2], dar[3], dar[4], dar[5], dar[6], dar[7], dar[8], dar[9], dar[10], dar[11], dar[12], dar[13], dar[14], dar[15], dar[16], ref reply);
                if (res > 0)
                {
                    VB_FSL_ErrorToString(res, ref ReturnedErrorStr, 100);
                    CVB_FSL_ReleaseDevice();
                    File.Delete(String.Format("{0}{1}.RT", currentpath, filename));
                    return (res);
                }
            }
            else
            {
                if (res == 0x0e) goto resend;
            }
            File.Delete(String.Format("{0}{1}.RT", currentpath, filename));
            CVB_FSL_ReleaseDevice();
            return (res);
        }
        public int SignDocument(Devices CDev, string dataFile, ref string SG)
        {
            string ReturnedErrorStr = "";
            string ReturnedSignature = "";
            string ReturnedSecSignature = "";

            resend:
            int res = SelectESDDevice(CDev);
            if (res == 0)
            {

                if (File.Exists(dataFile))
                {
                    VB_FSL_DisableCheckZFiles(1);

                    res = N_FSL_SignDocument(dataFile, ref ReturnedSignature, ref ReturnedSecSignature);

                    if (res > 0)
                    {
                        ReturnedSignature = "";
                        VB_FSL_ErrorToString(res, ref ReturnedErrorStr, 100);
                        CVB_FSL_ReleaseDevice();
                        SG = ReturnedSignature;
                        return (res);
                    }
                }
            }
            else
            {
                if (res == 0x0e) goto resend;
            }
            CVB_FSL_ReleaseDevice();
            SG = ReturnedSecSignature;
            return (res);
        }

        public int SignData(Devices CDev, string dataFile, string vstream, ref string SG)
        {
            string ReturnedErrorStr = "";
            string ReturnedSignature = "";
            string ReturnedSecSignature = "";

            resend:
            int res = SelectESDDevice(CDev);
            if (res == 0)
            {

                //res = CVB_FSL_SignDocument(String.Format("{0}{1}.RT", currentpath, filename), ref ReturnedSignature);
                if (File.Exists(dataFile))
                {
                    res = CVB_FSL_SignDocument(dataFile, ref ReturnedSignature);

                    if (res > 0)
                    {
                        ReturnedSignature = "";
                        VB_FSL_ErrorToString(res, ref ReturnedErrorStr, 100);
                        CVB_FSL_ReleaseDevice();
                        SG = ReturnedSignature;
                        // File.Delete(String.Format("{0}{1}.RT", currentpath, filename));
                        return (res);
                    }
                    string[] dar = vstream.Split(';');
                    res = CVB_FSL_InvData(dar[0], dar[1], dar[2], dar[3], dar[4], dar[5], dar[6], dar[7], dar[8], dar[9], dar[10], dar[11], dar[12], dar[13], dar[14], dar[15], dar[16], ref ReturnedSecSignature);
                    if (res > 0)
                    {
                        ReturnedSignature = "";
                        VB_FSL_ErrorToString(res, ref ReturnedErrorStr, 100);
                        CVB_FSL_ReleaseDevice();
                        SG = ReturnedSignature;
                        return (res);
                    }
                }
            }
            else
            {
                if (res == 0x0e) goto resend;
            }
            //File.Delete(String.Format("{0}{1}.RT", currentpath, filename));
            CVB_FSL_ReleaseDevice();
            SG = ReturnedSecSignature;
            return (res);
        }
        public int ReadDeviceID(Devices CDev, ref string reply)
        {
            string ReturnedErrorStr = "";
            resend:
            int iret = SelectESDDevice(CDev);
            if (iret > 0)
            {
                CVB_FSL_ReleaseDevice();
                return (iret);
            }
            else
            {
                if (iret == 0x0e) goto resend;
            }

            int res = CVB_FSL_GetSerialNo(ref reply);
            if (res > 0)
            {
                VB_FSL_ErrorToString(res, ref ReturnedErrorStr, 100);
                CVB_FSL_ReleaseDevice();
                return (res);
            }

            CVB_FSL_ReleaseDevice();
            return (0);
        }

        public int IsDevON(Devices CDev)
        {
            resend:
            int iret = SelectESDDevice(CDev);
            if (iret == 0)
            {
                CVB_FSL_ReleaseDevice();
            }
            else
            {
                if (iret == 0x0e) goto resend;
            }
            return (iret);
        }
        public int ReadDevStat(Devices CDev, ref string reply)
        {
            string ReturnedErrorStr = "";
            resend:
            int iret = SelectESDDevice(CDev);
            if (iret > 0)
            {
                CVB_FSL_ReleaseDevice();
                return (iret);
            }
            else
            {
                if (iret == 0x0e) goto resend;
            }

            int res = CVB_FSL_GetStat(ref reply);
            if (res > 0)
            {
                VB_FSL_ErrorToString(res, ref ReturnedErrorStr, 100);
                CVB_FSL_ReleaseDevice();
                return (res);
            }

            CVB_FSL_ReleaseDevice();
            return (0);
        }
        public int IssueZreport(Devices CDev)
        {
            string ReturnedErrorStr = "";
            resend:
            int iret = SelectESDDevice(CDev);
            if (iret > 0)
            {
                VB_FSL_ErrorToString(iret, ref ReturnedErrorStr, 100);
                CVB_FSL_ReleaseDevice();
                RetErr = ReturnedErrorStr;
                return (iret);
            }
            else
            {
                if (iret == 0x0e) goto resend;
            }

            int res = CVB_FSL_IssueZreport();
            if (res > 0)
            {
                VB_FSL_ErrorToString(res, ref ReturnedErrorStr, 100);
                CVB_FSL_ReleaseDevice();
                RetErr = ReturnedErrorStr;
                return (res);
            }

            CVB_FSL_ReleaseDevice();
            return (0);
        }

        public int Upload2GGPS_Z(Devices CDev, ref string errorstr)
        {
            string ReturnedErrorStr = "";
            int rer = 0;

            Int16 ret = Upload_STXT_FILE(ref rer, "", 80, "", "", "");
            if (ret > 0)
            {
                VB_FSL_ErrorToString(ret, ref ReturnedErrorStr, 100);

            }
            errorstr = ReturnedErrorStr;
            return (ret);
        }
        public int CheckCFiles(Devices CDev)
        {
            string ReturnedErrorStr = "";
            resend:
            int iret = SelectESDDevice(CDev);
            if (iret > 0)
            {
                VB_FSL_ErrorToString(iret, ref ReturnedErrorStr, 100);
                CVB_FSL_ReleaseDevice();
                RetErr = ReturnedErrorStr;
                return (iret);
            }
            else
            {
                if (iret == 0x0e) goto resend;
            }
            int res = FSL_CheckCFiles();
            if (res > 0)
            {
                VB_FSL_ErrorToString(res, ref ReturnedErrorStr, 100);
                CVB_FSL_ReleaseDevice();
                RetErr = ReturnedErrorStr;
                return (res);
            }

            CVB_FSL_ReleaseDevice();
            return (0);
        }


        public int CheckCEFiles(Devices CDev)
        {
            string ReturnedErrorStr = "";
            resend:
            int iret = SelectESDDevice(CDev);
            if (iret > 0)
            {
                VB_FSL_ErrorToString(iret, ref ReturnedErrorStr, 100);
                CVB_FSL_ReleaseDevice();
                RetErr = ReturnedErrorStr;
                return (iret);
            }
            else
            {
                if (iret == 0x0e) goto resend;
            }
            int res = FSL_CheckCEFiles();
            if (res > 0)
            {
                VB_FSL_ErrorToString(res, ref ReturnedErrorStr, 100);
                CVB_FSL_ReleaseDevice();
                RetErr = ReturnedErrorStr;
                return (res);
            }

            CVB_FSL_ReleaseDevice();
            return (0);
        }
        public int VerifyCFiles(Devices CDev)
        {
            string ReturnedErrorStr = "";
            resend:
            int iret = SelectESDDevice(CDev);
            if (iret > 0)
            {
                VB_FSL_ErrorToString(iret, ref ReturnedErrorStr, 100);
                CVB_FSL_ReleaseDevice();
                RetErr = ReturnedErrorStr;
                return (iret);
            }
            else
            {
                if (iret == 0x0e) goto resend;
            }
            int res = FSL_VerifyCFiles();
            if (res > 0)
            {
                VB_FSL_ErrorToString(res, ref ReturnedErrorStr, 100);
                CVB_FSL_ReleaseDevice();
                RetErr = ReturnedErrorStr;
                return (res);
            }

            CVB_FSL_ReleaseDevice();
            return (0);
        }

        public int OpenPopUpCtrl(Devices CDev)
        {
            string ReturnedErrorStr = "";
            resend:
            int iret = SelectESDDevice(CDev);
            if (iret > 0)
            {
                VB_FSL_ErrorToString(iret, ref ReturnedErrorStr, 100);
                CVB_FSL_ReleaseDevice();
                RetErr = ReturnedErrorStr;
                return (iret);
            }
            else
            {
                if (iret == 0x0e) goto resend;
            }
            int res = CVB_FSL_PopupControl();
            if (res > 0)
            {
                VB_FSL_ErrorToString(res, ref ReturnedErrorStr, 100);
                CVB_FSL_ReleaseDevice();
                RetErr = ReturnedErrorStr;
                return (res);
            }

            CVB_FSL_ReleaseDevice();
            return (0);
        }

        public string ReadUnlockKey()
        {
            string retkey = "";
            CReadRegisteredDevice(ref retkey, 0);
            return (retkey);
        }
    }

}
