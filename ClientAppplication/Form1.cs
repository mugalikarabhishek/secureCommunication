using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Security;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace ClientAppplication
{
    public partial class Form1 : Form
    {

        # region networkvariables
        System.Net.Sockets.TcpClient clientSocket = new System.Net.Sockets.TcpClient();
        #endregion

        #region variables
        /// <summary>
        /// The get key for AES
        /// </summary>
        byte[] EncryptKey;

        /// <summary>
        /// The get IV for AES
        /// </summary>
        byte[] IntialVector;


        /// <summary>
        /// The Encrypt using inbuilt AES
        /// </summary>
        AesCryptoServiceProvider Crypto = new AesCryptoServiceProvider();

        /// <summary>
        /// The Encrypt using inbuilt RSA
        /// </summary>
        RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();

        /// <summary>
        /// The Encrypt using Manual implementation of RSA
        /// </summary>
        private RSACustomEncryption rSACE;

        /// <summary>
        /// The get encrypted BIG integer byte after RSA implementation
        /// </summary>
        BigInteger encryptedByte;

        /// <summary>
        /// to Add each individual encrypted big integer to list in RSA implemenatation
        /// </summary>
        List<BigInteger> a;

        # endregion

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //to display client was connecteed to server
            msg("Client Started");
            clientSocket.Connect("127.0.0.1", 8888);
            label1.Text = "Client Socket Program - Server Connected ...";

            //AES encryption generate key and IV and initialize padding for encryption decryption
            Crypto.Padding = PaddingMode.Zeros;
            int a = Crypto.KeySize;//get keysize
            Crypto.GenerateKey();
            Crypto.GenerateIV();
            EncryptKey = Crypto.Key;
            IntialVector = Crypto.IV;

            //disable controls at start
            DisableControls();
        }

        public void msg(string mesg)
        {
            textBox1.Text = textBox1.Text + Environment.NewLine + " >> " + mesg;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            NetworkStream serverStream = clientSocket.GetStream();
            var watch = System.Diagnostics.Stopwatch.StartNew();

            byte[] outStream = EncryptString(Crypto, textBox2.Text.Trim());

            // the code that you want to measure comes here
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            serverStream.Write(outStream, 0, outStream.Length);
            serverStream.Flush();
            textBox2.Text = "";
            textBox2.Focus();
        }

        /// <summary>
        ///Event to send AES key to server which will enable the sendIV button
        /// </summary>
        private void button2_Click(object sender, EventArgs e)
        {
            NetworkStream serverStream2 = clientSocket.GetStream();
            byte[] outStream2 = EncryptKey;//System.Text.Encoding.ASCII.GetBytes(textBox2.Text + "$");
            //Crypto.Key = EncryptKey;
            serverStream2.Write(outStream2, 0, outStream2.Length);
            serverStream2.Flush();
            sndIVBtn.Enabled = true;
            sndIVBtn.Visible = true;
            sndKyBtn.Visible = false;
        }

        /// <summary>
        ///Encrypt using inbuilt Crypto library function for AES
        /// </summary>
        public byte[] EncryptString(SymmetricAlgorithm symAlg, string inString)
        {
            byte[] inBlock = Encoding.ASCII.GetBytes(inString);
            var watch = System.Diagnostics.Stopwatch.StartNew();
            ICryptoTransform xfrm = symAlg.CreateEncryptor();
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            byte[] outBlock = xfrm.TransformFinalBlock(inBlock, 0, inBlock.Length);

            return outBlock;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (useAESrdioBtn.Checked == true)
            {
                NetworkStream serverStream3 = clientSocket.GetStream();
                byte[] outStream3 = System.Text.Encoding.ASCII.GetBytes("UseAES" + "$");
                // byte[] outStream = EncryptString(Crypto, textBox2.Text + "$");

                serverStream3.Write(outStream3, 0, outStream3.Length);
                serverStream3.Flush();


                sndKyBtn.Enabled = true;
                sndKyBtn.Visible = true;

                algorithmgroupBox.Enabled = false;
                cnclBtn.Enabled = true;
                cnclBtn.Visible = true;
            }
        }

        /// <summary>
        ///Event to send AES IV to server which will enable the textbox and send message button button
        /// </summary>
        private void button3_Click(object sender, EventArgs e)
        {
            NetworkStream serverStream1 = clientSocket.GetStream();
            byte[] outStream1 = IntialVector;
            serverStream1.Write(outStream1, 0, outStream1.Length);
            serverStream1.Flush();

            textBox2.Visible = true;
            sndMsgBtn.Enabled = true;
            sndMsgBtn.Visible = true;
            sndIVBtn.Visible = false;
        }

        /// <summary>
        ///Event to send client intention to use RSA
        /// </summary>
        private void useRSArdiobtn_CheckedChanged(object sender, EventArgs e)
        {
            if (useRSArdiobtn.Checked == true)
            {

                // to tell client about intention to use RSA.
                NetworkStream serverStream3 = clientSocket.GetStream();
                byte[] outStream3 = System.Text.Encoding.ASCII.GetBytes("UseRSA" + "$");
                serverStream3.Write(outStream3, 0, outStream3.Length);

                // to recieve public key and read data from server.Server generates key and sends it to client so client has 
                //public keys

                byte[] inStream = new byte[10025];
                int read = serverStream3.Read(inStream, 0, (int)clientSocket.ReceiveBufferSize);
                Array.Resize(ref inStream, read);
                string returndata = System.Text.Encoding.ASCII.GetString(inStream);
                RSA.FromXmlString(returndata);

                //enable disable controls
                serverStream3.Flush();
                textBox1.Text = "Public key recieved";
                textBox2.Visible = true;
                sndRsabtn.Enabled = true;
                sndRsabtn.Visible = true;
                cnclBtn.Visible = true;
                algorithmgroupBox.Enabled = false;

            }

        }

        /// <summary>
        /// To send msg using RSA library implementation
        /// </summary>
        private void sndRsabtn_Click(object sender, EventArgs e)
        {

            NetworkStream serverStream = clientSocket.GetStream();
            ///to get the time to encrypt
            var watch = System.Diagnostics.Stopwatch.StartNew();

            byte[] dataToEncrypt = Encoding.ASCII.GetBytes(textBox2.Text);
            // Encrypt using RSA inbulit function
            byte[] encrypteddata = RSAEncrypt(dataToEncrypt, RSA.ExportParameters(false), false);
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            //to send data to client
            serverStream.Write(encrypteddata, 0, encrypteddata.Length);
            serverStream.Flush();
            textBox2.Text = "";
            textBox2.Focus();
        }

        /// <summary>
        /// Encrypt the passed  number using RSA in built
        /// </summary>
        /// <param name="DataToEncrypt">data to encrypt</param>
        /// <param name="RSAKeyInfo">Specifies key information</param>
        /// <param name="DoOAEPPadding">Specifies padding</param>
        /// <returns>the resulting encrypted data</returns>
        public byte[] RSAEncrypt(byte[] DataToEncrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
        {
            try
            {
                byte[] encryptedData;

                //Import the RSA Key information. This only needs
                //toinclude the public key information.
                RSA.ImportParameters(RSAKeyInfo);

                //Encrypt the passed byte array and specify OAEP padding.  
                encryptedData = RSA.Encrypt(DataToEncrypt, DoOAEPPadding);

                return encryptedData;
            }
            //Catch and display a CryptographicException  
            //to the console.
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        /// <summary>
        /// To Disable controls so that user cant perform unwanted action like send message prior
        /// to selecting algorithm
        /// </summary>
        private void DisableControls()
        {
            label2.Enabled = false;
            textBox2.Visible = false;
            sndMsgBtn.Enabled = false;
            sndMsgBtn.Visible = false;
            sndKyBtn.Enabled = false;
            sndKyBtn.Visible = false;
            sndIVBtn.Enabled = false;
            sndIVBtn.Visible = false;
            sndRsabtn.Enabled = false;
            sndRsabtn.Visible = false;
            RsaInitializebtn.Visible = false;
            sndMsgBtn.Visible = false;
            cnclBtn.Visible = false;
            sndmsgrsaIBtn.Visible = false;
            hashBtn.Visible = false;


        }

        /// <summary>
        ///Event is used to cancel and start from beginning
        /// </summary>/
        private void cnclBtn_Click(object sender, EventArgs e)
        {
            //let server know that the present transactions are cancelled
            NetworkStream serverStream3 = clientSocket.GetStream();
            byte[] outStream3 = System.Text.Encoding.ASCII.GetBytes("Cancel" + "$");
            serverStream3.Write(outStream3, 0, outStream3.Length);
            serverStream3.Flush();
            DisableControls();

            //disable all radio button selections.
            rsaimplementationrRdioBtn.Checked = false;
            useRSArdiobtn.Checked = false;
            useAESrdioBtn.Checked = false;
            hashRdioBtn.Checked = false;
            algorithmgroupBox.Enabled = true;
        }

        // used to initialzie the users intention of using RSA
        private void RsaInitializebtn_Click(object sender, EventArgs e)
        {
            // We create a new instance of custom encryption class. Since we will prepare the algorithm, we pass
            // false and null as parameters
            NewRSACustomEncryptionInstance(false, null);

            //send private key to server
            string PK = string.Format("{0},{1}", rSACE.GetD.ToString(), rSACE.GetN.ToString());

            //convert to bytes and send private key to server
            byte[] PK1 = Encoding.ASCII.GetBytes(PK);
            NetworkStream serverStream3 = clientSocket.GetStream();
            serverStream3.Write(PK1, 0, PK1.Length);
            serverStream3.Flush();

            //enable tthe controls to send message
            textBox2.Enabled = true;
            textBox2.Visible = true;
            sndmsgrsaIBtn.Visible = true;
            textBox2.Visible = true;
            RsaInitializebtn.Visible = false;
        }

        /// <summary>
        /// to initialize values for RSA manual implementation
        /// </summary>
        /// <param name="keyData">to initialize initial key values<param>
        /// <param name="importedKeyData">If key is imported from outside</param>
        private void NewRSACustomEncryptionInstance(Boolean importedKeyData, Object[] keyData)
        {
            rSACE = new RSACustomEncryption(keyData);
            rSACE.KeySize = 16;
            PrepareRSAAlgorithm();
        }

        /// <summary>
        /// To Prepare RSA algorithm with  the neccesary private key and public key
        /// </summary>
        private void PrepareRSAAlgorithm()
        {
            rSACE.PrepareAlgorithm();
        }

        /// <summary>
        /// To let server know that client wants to use RSA implementation
        /// </summary>
        private void rsaimplementationrRdioBtn_CheckedChanged(object sender, EventArgs e)
        {
            if (rsaimplementationrRdioBtn.Checked == true)
            {

                // to tell server about intention to use RSA.
                NetworkStream serverStream3 = clientSocket.GetStream();
                byte[] outStream3 = System.Text.Encoding.ASCII.GetBytes("UseRSAImplementation" + "$");
                serverStream3.Write(outStream3, 0, outStream3.Length);
                serverStream3.Flush();
                RsaInitializebtn.Visible = true;
                DisableControls();
                algorithmgroupBox.Enabled = false;
                cnclBtn.Visible = true;
                RsaInitializebtn.Visible = true;
            }
        }
        /// <summary>
        /// To send msg using RSA manual implementation
        /// </summary>
        private void sndmsgrsaIBtn_Click(object sender, EventArgs e)
        {
            // to get the time needed to encrypt
            var watch = System.Diagnostics.Stopwatch.StartNew();
            //Convert to byte to perform necesary encryption
            Byte[] bytesBlockToEncrypt = Encoding.ASCII.GetBytes(textBox2.Text);
            string encryptedtext = string.Empty;
            encryptedByte = new BigInteger();
            a = new List<BigInteger>();

            //encrypt each byte of data
            for (int j = 0; j < bytesBlockToEncrypt.Length; j++)
            {

                encryptedByte = rSACE.Encrypt(new BigInteger(bytesBlockToEncrypt[j]));
                int bit = encryptedByte.bitCount();
                a.Add(encryptedByte);

                //get bit count of biginteger to send it to server
                if (bit <= 10)
                {
                    string abc = "00"+ encryptedByte.ToString();
                    encryptedtext = encryptedtext + abc;
                }

                else  if (bit > 10 && bit < 15 && encryptedByte.data[0] < 10000)
                {
                    string abc = '0' + encryptedByte.ToString();
                    encryptedtext = encryptedtext + abc;
                }
                 
                else
                {   //create a single combined string to send.
                    encryptedtext = encryptedtext + encryptedByte.ToString();
                }
            }
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            foreach (BigInteger item in a)
            {
                //to copnvert to string and get bytes
                string abc = item.ToString(2);
            }



            //send encrypted data to server
            NetworkStream serverStream3 = clientSocket.GetStream();
            byte[] outStream3 = System.Text.Encoding.ASCII.GetBytes(encryptedtext);
            serverStream3.Write(outStream3, 0, outStream3.Length);
            serverStream3.Flush();

        }
        // radio hash function btn to select hash function
        private void hashRdioBtn_CheckedChanged(object sender, EventArgs e)
        {
            if (hashRdioBtn.Checked==true)
            {
                textBox2.Visible = true;
                hashBtn.Visible = true;
                algorithmgroupBox.Enabled = false;
                cnclBtn.Visible = true;
            }
        }

        // hash button to get hash function.MD5 hash is used 
        private void hashBtn_Click(object sender, EventArgs e)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                string hashValue = CreateHash(md5Hash, textBox2.Text);

                MessageBox.Show("The MD5 hash of " + textBox2.Text + " is: " + hashValue + "."); 
            }
        }
        // create md5 hash function
        static string CreateHash(MD5 md5HashValue, string text)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5HashValue.ComputeHash(Encoding.UTF8.GetBytes(text));
            string value = Encoding.ASCII.GetString(data);
            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder hashBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                hashBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return hashBuilder.ToString();
        }
    }
}
