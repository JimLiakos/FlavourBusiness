using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace CashierStationDevice.aadeUtil
{
    /// <MetaDataID>{96eb02e9-ea08-4b53-a034-ff51f7ff46c6}</MetaDataID>
    public class VatPartyInfo
    {
        public static string GetPartyInfo(string callerVat, string authUser,string authPassword,string vat)
        {

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(@"https://www1.gsis.gr:443/webtax2/wsgsis/RgWsPublic/RgWsPublicPort");
            webRequest.Headers.Add(@"SOAP:Action");
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";


            System.Xml.Linq.XDocument soapEnvelopeXml = System.Xml.Linq.XDocument.Parse(VatPartyInfoRequest);
            var env= System.Xml.Linq.XNamespace.Get("http://schemas.xmlsoap.org/soap/envelope/");
            var ns1 = XNamespace.Get("http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext1.0.xsd");
            var ns = System.Xml.Linq.XNamespace.Get("http://gr/gsis/rgwspublic/RgWsPublic.wsdl");
            var m = System.Xml.Linq.XNamespace.Get("http://gr/gsis/rgwspublic/RgWsPublic.wsdl");

            var usernameToken = soapEnvelopeXml.Root.Element(env+ "Header").Element(ns1+"Security").Element(ns1+ "UsernameToken");
            usernameToken.Element(ns1 + "Username").Value= authUser;
            usernameToken.Element(ns1 + "Password").Value= authPassword;
            soapEnvelopeXml.Root.Element(env + "Body").Element(ns + "rgWsPublicAfmMethod").Element("RgWsPublicInputRt_in").Element(ns+"afmCalledBy").Value= callerVat;
            soapEnvelopeXml.Root.Element(env + "Body").Element(ns + "rgWsPublicAfmMethod").Element("RgWsPublicInputRt_in").Element(ns + "afmCalledFor").Value = vat;

            using (Stream stream = webRequest.GetRequestStream())
            {
                soapEnvelopeXml.Save(stream);
            }
            using (WebResponse response = webRequest.GetResponse())
            {
                using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                {
                    string soapResult = rd.ReadToEnd();
                    var soapResultDoc = XDocument.Parse(soapResult);

                    var partyElement = soapResultDoc.Root.Element(env + "Body").Element(m + "rgWsPublicAfmMethodResponse").Element("RgWsPublicBasicRt_out");
                    //https://github.com/kamilakis/rgwspublic/blob/master/structs.go
                }
            }
            return "";
        }



        const string VatPartyInfoRequest = @"<?xml version=""1.0"" encoding=""UTF-8""?>
    <env:Envelope
     xmlns:env=""http://schemas.xmlsoap.org/soap/envelope/""
     xmlns:ns=""http://gr/gsis/rgwspublic/RgWsPublic.wsdl""
     xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
     xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
     xmlns:ns1=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext1.0.xsd"">
     <env:Header>
     <ns1:Security>
     <ns1:UsernameToken>
     <ns1:Username></ns1:Username>
     <ns1:Password></ns1:Password>
     </ns1:UsernameToken>
     </ns1:Security>
     </env:Header>
     <env:Body>
     <ns:rgWsPublicAfmMethod>
     <RgWsPublicInputRt_in xsi:type=""ns:RgWsPublicInputRtUser"">
     <ns:afmCalledBy></ns:afmCalledBy>
     <ns:afmCalledFor></ns:afmCalledFor>
     </RgWsPublicInputRt_in>
     <RgWsPublicBasicRt_out xsi:type=""ns:RgWsPublicBasicRtUser"">
     <ns:afm xsi:nil=""true""/>
     <ns:stopDate xsi:nil=""true""/>
     <ns:postalAddressNo xsi:nil=""true""/>
     <ns:doyDescr xsi:nil=""true""/>
     <ns:doy xsi:nil=""true""/>
     <ns:onomasia xsi:nil=""true""/>
     <ns:legalStatusDescr xsi:nil=""true""/>
     <ns:registDate xsi:nil=""true""/>
     <ns:deactivationFlag xsi:nil=""true""/>
     <ns:deactivationFlagDescr xsi:nil=""true""/>
     <ns:postalAddress xsi:nil=""true""/>
     <ns:firmFlagDescr xsi:nil=""true""/>
     <ns:commerTitle xsi:nil=""true""/>
     <ns:postalAreaDescription xsi:nil=""true""/>
     <ns:INiFlagDescr xsi:nil=""true""/>
     <ns:postalZipCode xsi:nil=""true""/>
     </RgWsPublicBasicRt_out>
     <arrayOfRgWsPublicFirmActRt_out xsi:type=""ns:RgWsPublicFirmActRtUserArray""/>
     <pCallSeqId_out xsi:type=""xsd:decimal"">0</pCallSeqId_out>
     <pErrorRec_out xsi:type=""ns:GenWsErrorRtUser"">
     <ns:errorDescr xsi:nil=""true""/>
     <ns:errorCode xsi:nil=""true""/>
     </pErrorRec_out>
     </ns:rgWsPublicAfmMethod>
     </env:Body>
    </env:Envelope>";
    }
}
