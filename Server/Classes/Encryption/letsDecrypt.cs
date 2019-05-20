using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Server.Classes.Encryption {
    public class LetsDecrypt {

        public string _privateKey;

        public string encryptedbytes;

        public UnicodeEncoding _encoder = new UnicodeEncoding ();

        public JObject Decrypteddataobject { get; set; }

        public LetsDecrypt (JObject newdata, string key) {
            _privateKey = key;

            JObject testObject = new JObject ();
            foreach (var item in newdata) {
                if (item.Key == "naam" || item.Key == "BSN" || item.Key == "geb_datum" || item.Key == "organisatie") {
                    testObject.Add (item.Key, Decrypt (item.Value.ToString (), _privateKey));
                    System.Console.WriteLine (item.ToString ());
                }
                if (item.Key == "Zsm" || item.Key == "Radicalen" || item.Key == "LokalePGA" || item.Key == "Detentie") {
                    JObject jObj = JObject.FromObject (item.Value);
                    JObject testObjectZRLD = new JObject ();
                    foreach (var testObjectInner in jObj) {
                        testObjectZRLD.Add (testObjectInner.Key, Decrypt (testObjectInner.Value.ToString (), _privateKey));
                    }
                    testObject.Add (item.Key, testObjectZRLD);
                }
            }
            Decrypteddataobject = testObject;
        }

        public JObject showDecrypted () {
            return Decrypteddataobject;
        }

        public string Decrypt (string textToDecrypt, string publicKeyString) {
            var bytesToDescrypt = Encoding.UTF8.GetBytes (textToDecrypt);

            using (var rsa = new RSACryptoServiceProvider (2048)) {
                try {

                    // server decrypting data with public key                    
                    FromXmlString1 (rsa, _privateKey);

                    var resultBytes = Convert.FromBase64String (textToDecrypt);
                    var decryptedBytes = rsa.Decrypt (resultBytes, true);
                    var decryptedData = Encoding.UTF8.GetString (decryptedBytes);
                    return decryptedData.ToString ();
                } finally {
                    rsa.PersistKeyInCsp = false;
                }
            }
        }

        public void FromXmlString1 (RSACryptoServiceProvider rsa, string xmlString) {
            RSAParameters parameters = new RSAParameters ();

            XmlDocument xmlDoc = new XmlDocument ();
            xmlDoc.LoadXml (xmlString);

            if (xmlDoc.DocumentElement.Name.Equals ("RSAKeyValue")) {
                foreach (XmlNode node in xmlDoc.DocumentElement.ChildNodes) {
                    switch (node.Name) {
                        case "Modulus":
                            parameters.Modulus = Convert.FromBase64String (node.InnerText);
                            break;
                        case "Exponent":
                            parameters.Exponent = Convert.FromBase64String (node.InnerText);
                            break;
                        case "P":
                            parameters.P = Convert.FromBase64String (node.InnerText);
                            break;
                        case "Q":
                            parameters.Q = Convert.FromBase64String (node.InnerText);
                            break;
                        case "DP":
                            parameters.DP = Convert.FromBase64String (node.InnerText);
                            break;
                        case "DQ":
                            parameters.DQ = Convert.FromBase64String (node.InnerText);
                            break;
                        case "InverseQ":
                            parameters.InverseQ = Convert.FromBase64String (node.InnerText);
                            break;
                        case "D":
                            parameters.D = Convert.FromBase64String (node.InnerText);
                            break;
                    }
                }
            } else {
                throw new Exception ("Invalid XML RSA key.");
            }

            rsa.ImportParameters (parameters);
        }

    }

}