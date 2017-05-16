using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClientAppplication
{
   public partial class RSACustomEncryption
    {
          public BigInteger GetP
        {
            get
            {
                 return this.p;
                
            }
        }

        public BigInteger GetQ
        {
            get
            {
                 return this.q;
                
            }
        }

        public BigInteger GetN
        {
            get
            {
                   return this.n;
                 
            }
        }

        public BigInteger GetPhi
        {
            get
            {
                return this.phi;
               
            }
        }

        public BigInteger GetD
        {
            get
            {
                 return this.d;
                
            }
        }

        public BigInteger GetE
        {
            get
            {
                return this.eC;
                
            }
        }

        public System.Boolean IsAlgorithmReady
        {
            get
            {
                return this.algorithmReady;
            }
        }

        /// <summary>
        /// I specified max size to 6144. Until 2020 the recommended key size is 2048, so...
        /// </summary>
        public UInt32 KeySize
        {
            get { return this.keySize; }
            // If the key will be set to 8, Generate_Primes() method will run indefinitly, because
            // the only odd numbers that will be generated will be 15 and 13 always
            set { this.keySize = value < 6144 && value > 8 ? value : 1024; }
        }

        /// <summary>
        /// Returns actions that can be performed
        /// </summary>
    //    public RSACustomEncryption.ActionsAvailableEnum ActionsAvailable
      //  {
      //      get { return this.actions; }
      //  }

        

        /// <summary>
        /// The variable holding which actions are available to execute, based on what parameters
        /// the constructor with one parameter takes
        /// </summary>
        //private ActionsAvailableEnum actions;



        #region PUBLIC_CONSTRUCTORS

        /// <summary>
        /// Constructor with one param.
        /// </summary>
        /// <param name="keyData">keyData must be null, if all the parameters will be calculated. 
        /// keyData[0] = e; keyData[1] = n; keyData[2] = d; => encrypt and decrypt to be available;
        /// keyData[0] = e; keyData[1] = n; keyData[2] = null; => encrypt only to be available;
        /// keyData[0] = null; keyData[1] = n; keyData[2] = d; => decrypt only to be available;</param>
        public RSACustomEncryption(Object[] keyData)
        {
            this.p = 0;
            this.q = 0;
            this.phi = 0;

            if (keyData != null && keyData.Length == 3)
            {
                if (keyData[0] != null && keyData[2] != null)
                {
                    this.eC = new BigInteger((String)(keyData[0]), 10);
                    this.n = new BigInteger((String)(keyData[1]), 10);
                    this.d = new BigInteger((String)(keyData[2]), 10);
                  //  this.actions = ActionsAvailableEnum.Decrypt | ActionsAvailableEnum.Encrypt;
                }

                if (keyData[0] != null && keyData[2] == null)
                {
                    this.eC = new BigInteger((String)(keyData[0]), 10);
                    this.n = new BigInteger((String)(keyData[1]), 10);
                    //this.actions = ActionsAvailableEnum.Encrypt;
                }

                if (keyData[0] == null && keyData[2] != null)
                {
                    this.d = new BigInteger((String)(keyData[2]), 10);
                    this.n = new BigInteger((String)(keyData[1]), 10);
                //    this.actions = ActionsAvailableEnum.Decrypt;
                }

                this.keySize = (UInt32)this.n.bitCount();
              //  this.algorithmReady = true;
            }
            else
            {
                this.algorithmReady = false;
                this.eC = 0;
                this.n = 0;
                this.keySize = 1024;
               // this.actions = ActionsAvailableEnum.Decrypt | ActionsAvailableEnum.Encrypt;
            }
        }
        #endregion PUBLIC_CONSTRUCTORS

        #region PUBLIC_METHODS
        /// <summary>
        /// Must be called be called before using the algorithm
        /// </summary>
        public void PrepareAlgorithm()
        {
           // algorithmReady = false;
         //   this.actions = ActionsAvailableEnum.Decrypt | ActionsAvailableEnum.Encrypt;
            BigInteger messageToTest = new BigInteger(77);

            while (!algorithmReady)
            {
                this.Generate_Primes();
                //this.Generate_Primes2();
                this.CalculateN();
                this.CalculatePhi();
                this.SelectE();

                // For a key of 1024 bits, the d was found in 192 miliseconds
                this.CalculateD_2();

                // This block is necessary to test if really the algorithm works(in the case when RabinMillerTest is wrong)
                {
                    BigInteger encryptedMessage = Encrypt(messageToTest);
                    BigInteger decryptedMessage = Decrypt(encryptedMessage);

                    if (decryptedMessage != messageToTest) continue;
                }
                // For a key of 1024 bits, I waited 15 minutes, and this method still was executing, I stopped it. 
                // So it's obvious the use of modulo inverse
                // this.CalculateD();
                //Int32 bC = this.n.bitCount();
                this.algorithmReady = true;
            }
        }
    }
}
        #endregion