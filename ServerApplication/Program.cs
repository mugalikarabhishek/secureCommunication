using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Security;
using System.Security.Cryptography;
using System.Xml;
using System.IO;

namespace ServerApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            //initialize the AES Crypto class to use inbuilt AES function
            AesCryptoServiceProvider Crypto = new AesCryptoServiceProvider();
            Crypto.Padding = PaddingMode.Zeros;

            //to send public key to client.export parameters is false sets public key
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
            RSAParameters RSAKeyInfo = RSA.ExportParameters(false);

            #region NetworkVariable
            //all network variable which will be used for communication layer
            IPAddress[] ipAddress = Dns.GetHostAddresses("localhost");
            TcpListener serverSocket = new TcpListener(ipAddress[1], 8888);
            int requestCount = 0;
            TcpClient clientSocket = default(TcpClient);
            serverSocket.Start();
            Console.WriteLine(" >> Server Started");
            clientSocket = serverSocket.AcceptTcpClient();
            Console.WriteLine(" >> Accept connection from client");
            requestCount = 0;
            #endregion

            // to read the initial selection from client
            string firstdatafromclient = "";
            string decryptdata = "";
            int numberofbytesread = 0;

            //varaible to store d and n
            BigInteger d = new BigInteger();
            BigInteger n = new BigInteger();
            while ((true))
            {
                try
                {
                    // to start server and read data
                    requestCount = requestCount + 1;
                    NetworkStream networkStream = clientSocket.GetStream();
                    byte[] bytesFrom = new byte[10025];

                    numberofbytesread = networkStream.Read(bytesFrom, 0, (int)clientSocket.ReceiveBufferSize);
                    string dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom);
                    // resize array depending on the number of bytes recieved.
                    Array.Resize(ref bytesFrom, numberofbytesread);

                    // to understand what type of alogithm the clients wants to use
                    if (requestCount == 1)
                    {
                        //get the first request from client regarding algorithm choice
                        dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf("$"));
                        firstdatafromclient = dataFromClient;
                    }

                    //if client says cancel reinitiaze the request count to 0
                    if (dataFromClient.Contains("Cancel"))
                    {
                        requestCount = 0; firstdatafromclient = "";
                    }
                    // if client says USEAES read AES key and IV and use AE Library decryption
                    if (firstdatafromclient == "UseAES")
                    {
                        //get the key and IV for AES from client to decrypt.
                        if (requestCount == 2)
                        {
                            Crypto.Key = bytesFrom;
                        }
                        //get IV and use for AES implementation
                        if (requestCount == 3)
                        {
                            if (firstdatafromclient == "UseAES")
                            {
                                Crypto.IV = bytesFrom;
                            }
                        }
                        //display what is recieved from client
                        Console.WriteLine(" >> Data from client - " + dataFromClient.TrimEnd('\0'));

                        // decryption. decypt  when the data is message
                        if (firstdatafromclient == "UseAES" && requestCount != 1 && requestCount != 2
                            && requestCount != 3 && requestCount != 0)
                        {
                            // to get time for decryption
                            var watch = System.Diagnostics.Stopwatch.StartNew();
                            // actual decryption
                            decryptdata = DecryptBytes(Crypto, bytesFrom);
                            Console.WriteLine(" >> Decrypted data  from client - " + decryptdata);
                            // the display the total time taken for decryption
                            watch.Stop();
                            var elapsedMs = watch.ElapsedMilliseconds;
                        }
                    }

                    //use RSA in built function for decryption
                    if (firstdatafromclient == "UseRSA")
                    {
                        if (requestCount == 1)
                        {
                            Console.WriteLine("Client wants to use USE-RSA");
                            string key = RSA.ToXmlString(false); //false uses only public key to convert RSA to string.
                            string serverResponse = key;//get the key
                            Byte[] sendBytes = Encoding.ASCII.GetBytes(serverResponse);//sent key to client
                            networkStream.Write(sendBytes, 0, sendBytes.Length);
                            Console.WriteLine("Public key sent to Client");
                        }

                        if (requestCount != 1 && requestCount != 0)
                        {
                            // to get time for decryption
                            var watch = System.Diagnostics.Stopwatch.StartNew();
                            Console.WriteLine(" >> Encrypted  data  from client - " + dataFromClient.TrimEnd('\0'));
                            //call to decryption function to decrypt data using RSA inbuilt functions.
                            byte[] decryptedData = RSADecrypt(bytesFrom, RSA.ExportParameters(true), false);
                            string decrypted = Encoding.ASCII.GetString(decryptedData);
                            Console.WriteLine(" >> Decrypted data  from client - " + decrypted);
                            // the display the total time taken for decryption
                            watch.Stop();
                            var elapsedMs = watch.ElapsedMilliseconds;
                        }
                    }
                    //use RSA manual implementation function for decryption
                    if (firstdatafromclient == "UseRSAImplementation")
                    {
                        if (requestCount == 1)
                        {
                            //display client request to use RSA
                            Console.WriteLine(" >> Data from client - " + dataFromClient.TrimEnd('\0'));
                        }
                        if (requestCount == 2)
                        {
                            //get the key and split D and N for decryption
                            string keyBytes = Encoding.ASCII.GetString(bytesFrom);
                            string[] key = keyBytes.Split(',');
                            string GetD = key[0];
                            string GetN = key[1];

                            //get the key data to decrypt
                            d = (new BigInteger(Convert.ToInt64(GetD)));
                            n = (new BigInteger(Convert.ToInt64(GetN)));
                            Console.WriteLine(" >> Data From Client -- Key Recieved  ");
                        }
                        // after recieving key and initial request get actual data to decrypt
                        if (requestCount > 2)
                        {
                            string data = Encoding.ASCII.GetString(bytesFrom);
                            Console.WriteLine(" >> Data from client - " + data);
                           
                            // to get time for decryption
                            var watch = System.Diagnostics.Stopwatch.StartNew();
                            // start decryption process by sepearating individual bytes data and converting it into Big integer
                            BigInteger bigData = (new BigInteger(bytesFrom));
                            List<BigInteger> dataList = new List<BigInteger>();
                            byte[] byteData = new byte[5];
                            int count = 0;
                            int j = 0;
                            //this sepearates the big data for decryption
                            for (int i = 0; i < bytesFrom.Length; i++)
                            {
                                count++;
                                if (count <= 5)
                                {
                                    byteData[j] = bytesFrom[i];
                                    j++;
                                }

                                if (count == 5)
                                {
                                    count = 0;
                                    j = 0;
                                    string stringData = Encoding.ASCII.GetString(byteData);
                                    BigInteger decryptBig = (new BigInteger(Convert.ToInt64(stringData)));
                                    dataList.Add(decryptBig);
                                    byteData = new byte[5];
                                }
                            }

                            BigInteger decryptedStringBlock;
                            string decryptstring = string.Empty;
                            foreach (BigInteger item in dataList)
                            {
                                decryptedStringBlock = Decrypt(item, d, n);
                                //pass each individual seperated big data and d,n for decryption
                                byte[] decry = decryptedStringBlock.getBytes();
                                Array.Resize(ref decry, 1);
                                decryptstring = decryptstring + Encoding.ASCII.GetString(decry);
                            }
                            // the total time to decrypt
                            watch.Stop();
                            var elapsedMs = watch.ElapsedMilliseconds;
                            Console.WriteLine("Decrypted text >> " + decryptstring);
                        }
                    }
                    //flush the network
                    networkStream.Flush();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }

            clientSocket.Close();
            serverSocket.Stop();
            Console.WriteLine(" >> exit");
            Console.ReadLine();

        }
        /// <summary>
        /// Decrypts the passed  number using AES in built library functions
        /// </summary>
        /// <param name="symAlg">Key and IV information required for decrytping</param>
        /// <param name="inBytes">data to decrypt</param>
        /// <returns>the resulting decrypted data</returns>
        public static string DecryptBytes(SymmetricAlgorithm symAlg, byte[] inBytes)
        {
            ICryptoTransform xfrm = symAlg.CreateDecryptor();
            byte[] outBlock = xfrm.TransformFinalBlock(inBytes, 0, inBytes.Length);
            return Encoding.ASCII.GetString(outBlock);

        }

        /// <summary>
        /// Decrypts the passed  number using RSA in built
        /// </summary>
        /// <param name="DataToDecrypt">data to decrypt</param>
        /// <param name="DoOAEPPadding">Specifies padding</param>
        /// <returns>the resulting decrypted data</returns>
        static public byte[] RSADecrypt(byte[] DataToDecrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
        {
            try
            {
                byte[] decryptedData;
                //Create a new instance of RSACryptoServiceProvider.
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    //Import the RSA Key information. This needs
                    //to include the private key information.
                    RSA.ImportParameters(RSAKeyInfo);

                    //Decrypt the passed byte array and specify OAEP padding.  
                    decryptedData = RSA.Decrypt(DataToDecrypt, DoOAEPPadding);
                }
                return decryptedData;
            }
            //Catch and display a CryptographicException  
            //to the console.
            catch (CryptographicException e)
            {
                Console.WriteLine(e.ToString());

                return null;
            }
        }
        /// <summary>
        /// Decrypts the passed BigInteger number uses RSA manual Implementation for decryption
        /// </summary>
        /// <param name="c">Number to decrypt</param>
        /// <returns>the resulting BigInteger</returns>
        public static BigInteger Decrypt(BigInteger c, BigInteger d, BigInteger n)
        {
            BigInteger cToPow = new BigInteger(c);
            BigInteger exp = new BigInteger(d);
            BigInteger modN = new BigInteger(n);
            BigInteger modRes = cToPow.modPow(exp, modN);

            return modRes;
        }

    }
}
