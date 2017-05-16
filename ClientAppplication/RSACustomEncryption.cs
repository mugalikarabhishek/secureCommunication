using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClientAppplication
{
    class RSACustomEncryption
    {
        // How many times the rabin miller test of primality must be applied to the number to test
        private const Int32 rabinMillerConfidence = 20;

        /// <summary>
        /// First prime number
        /// </summary>
        private BigInteger p;

        /// <summary>
        /// Second prime number
        /// </summary>
        private BigInteger q;

        /// <summary>
        /// The exponent of the private key
        /// </summary>
        private BigInteger eC;

        /// <summary>
        /// The product p*q
        /// </summary>
        private BigInteger n;

        /// <summary>
        /// The product (p-1)(q-1)
        /// </summary>
        private BigInteger phi;

        /// <summary>
        /// The value of the expression: (1+k*phi)/eC
        /// </summary>
        private BigInteger d;

        /// <summary>
        /// Boolean indicating that all necessary properties of the algorithm were calculated and ready
        /// </summary>
        private System.Boolean algorithmReady;

        /// <summary>
        /// The size of the RSA key
        /// </summary>
        private System.UInt32 keySize;

        /// <summary>
        /// The variable holding which actions are available to execute, based on what parameters
        /// the constructor with one parameter takes
        /// </summary>
      //  private ActionsAvailableEnum actions;

        #region PRIVATE_METHODS
        /// <summary>
        /// Method to generate the primes used in this algorithm
        /// If the number is not prime, a new number is generated
        /// This method is faster and surely better for keys up to 2048 bits, but appears from 2048 the
        /// problem that we will never get a prime number, see TestingRemarks.txt
        /// </summary>
        /// <remarks>For implementation details see http://www.di-mgt.com.au/rsa_alg.html</remarks>
        private void Generate_Primes()
        {
            UInt32 pLength = (UInt32)(this.keySize / 2.0), qLength = keySize - pLength;

            do
            {
                this.p = GenerateOddNBitNumber(pLength);

            } while (!RabinMillerPrimeTest(this.p, rabinMillerConfidence));//while (!this.p.RabinMillerTest(rabinMillerConfidence));                

            do
            {
                this.q = GenerateOddNBitNumber(qLength);

            } while (!RabinMillerPrimeTest(this.q, rabinMillerConfidence));

        }

        /// <summary>
        /// Method to generate the primes used in this algorithm
        /// If the number is not prime, then it is incremented by 2 and tested again for primality.
        /// Tested for generation of 512 bit key, generation run for 10 minutes, no success. (P.S. I verify
        /// in PrepareAlgorithm() if encryption/decryption succeeds before claiming that the key is ready)
        /// </summary>
        /// <remarks></remarks>
        private void Generate_Primes2()
        {
            UInt32 pLength = (UInt32)(this.keySize / 2.0), qLength = keySize - pLength;

            this.p = GenerateOddNBitNumber(pLength);
            this.q = GenerateOddNBitNumber(qLength);
            while (!RabinMillerPrimeTest(this.p, rabinMillerConfidence * 10))
            {
                this.p += 2;
            }

            while (!RabinMillerPrimeTest(this.q, rabinMillerConfidence * 10))
            {
                this.q += 2;
            }
        }

        /// <summary>
        /// Method to calculate n
        /// </summary>
        private void CalculateN()
        {
            this.n = this.p * this.q;
        }

        /// <summary>
        /// Method to calculate phi
        /// </summary>
        private void CalculatePhi()
        {
            this.phi = (this.p - 1) * (this.q - 1);
        }

        /// <summary>
        /// Method to select eC from the interval 1 &lt; e &lt; φ(n)
        /// </summary>
        private void SelectE()
        {
            BigInteger generatedE = GenerateOddNBitNumber((UInt32)(keySize / 2.0));

            for (; ; generatedE++)
            {
                if (AreRelativePrime(this.phi, generatedE))
                {
                    this.eC = generatedE;
                    break;
                }
            }


        }

        /// <summary>
        /// The method is the standart one used to find d, based on the RSA algorithm
        /// TO be used only with small numbers. With a key of 1024 bit it was executing for 15 minutes and no success
        /// </summary>
        private void CalculateD()
        {
            BigInteger tmp_D;

            for (int k = 2; ; k++)
            {
                this.d = (this.phi * k + 1) / this.eC;
                tmp_D = (this.phi * k + 1) % this.eC;

                // We verify that d is integral, so in this case tmp_D must be 0
                if (tmp_D == 0)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// This method calculates d using modulo inverse algorithm
        /// </summary>
        private void CalculateD_2()
        {
            this.d = (Extended_GDC(this.eC, this.phi, true))[1];
        }

        #endregion PRIVATE_METHODS

        public BigInteger GetP
        {
            get
            {
                if (this.algorithmReady == true) return this.p;
                return 0;
            }
        }

        public BigInteger GetQ
        {
            get
            {
                if (this.algorithmReady == true) return this.q;
                return 0;
            }
        }

        public BigInteger GetN
        {
            get
            {
                if (this.algorithmReady == true) return this.n;
                return 0;
            }
        }

        public BigInteger GetPhi
        {
            get
            {
                if (this.algorithmReady == true) return this.phi;
                return 0;
            }
        }

        public BigInteger GetD
        {
            get
            {
                if (this.algorithmReady == true) return this.d;
                return 0;
            }
        }

        public BigInteger GetE
        {
            get
            {
                if (this.algorithmReady == true) return this.eC;
                return 0;
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
        /// USer can change key size
        /// </summary>
        public UInt32 KeySize
        {
            get { return this.keySize; }
            // If the key will be set to 8, Generate_Primes() method will run indefinitly, because
            // the only odd numbers that will be generated will be 15 and 13 always
            set { this.keySize = value < 6144 && value > 8 ? value : 1024; }
        }

       
        #region PUBLIC_CONSTRUCTORS

        /// <summary>
        /// Constructor with one param.
        /// </summary>
        /// <param name="keyData">keyData must be null, if all the parameters will be calculated. 
        /// keyData[0] = e; keyData[1] = n; keyData[2] = d; => encrypt and decrypt to be available;
       ///</param>
        public RSACustomEncryption(Object[] keyData)
        {
            this.p = 0;
            this.q = 0;
            this.phi = 0; 
            this.algorithmReady = false;
            this.eC = 0;
            this.n = 0;
            this.keySize = 1024;
          
        }
        #endregion PUBLIC_CONSTRUCTORS

        #region PUBLIC_METHODS
        /// <summary>
        /// Must be called be called before using the algorithm
        /// </summary>
        public void PrepareAlgorithm()
        {
            algorithmReady = false;
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
                this.algorithmReady = true;
            }
        }

        ///<summary>
        /// Encrypts the passed Int64 number
        /// </summary>
        /// <param name="m">Number to encrypt</param>
        /// <returns>the resulting Int64</returns>
        public BigInteger Encrypt(BigInteger mToPow)
        {
            //if ((Int32)(ActionsAvailable & ActionsAvailableEnum.Encrypt) == 0)
            //    return -1;

            BigInteger exp = new BigInteger(this.eC);
            BigInteger modN = new BigInteger(this.n);
            BigInteger modRes = mToPow.modPow(exp, modN);

            return modRes;
        }

        /// <summary>
        /// Decrypts the passed BigInteger number
        /// </summary>
        /// <param name="c">Number to decrypt</param>
        /// <returns>the resulting BigInteger</returns>
        public BigInteger Decrypt(BigInteger c)
        {
            BigInteger cToPow = new BigInteger(c);
            BigInteger exp = new BigInteger(this.d);
            BigInteger modN = new BigInteger(this.n);
            BigInteger modRes = cToPow.modPow(exp, modN);

            return modRes;
        }
        #endregion PUBLIC_METHODS


        #region STATIC_METHODS

        

        /// <summary>
        /// Two integers are termed relative prime if the only common factor between them is 1
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static System.Boolean AreRelativePrime(BigInteger phi, BigInteger e)
        {
            // The numbers are relatively prime only if x equals 1. The cycle above will end anyway
            return GreatestCommonDivizor(phi, e) == 1;
        }

        /// <summary>
        /// Algorithm to find greatest common divisor
        /// </summary>
        /// <param name="X">First number</param>
        /// <param name="Y">Second number</param>
        /// <returns>True if no common divisor greater than 1 is possible, False otherwise</returns>
        public static BigInteger GreatestCommonDivizor(BigInteger x, BigInteger y)
        {
            BigInteger tmp;

            if (x < y)
            {
                tmp = x;
                x = y;
                y = tmp;
            }

            while (true)
            {
                tmp = x % y;
                x = y;
                y = tmp;

                if (y == 0) break;
            }

            // This will be the GCD
            return x;
        }

        /// <summary>
        /// This method implements the extended euclidian GDC algorithm and the modulo inverse algorithm
        /// </summary>
        /// <param name="a">The number for which to apply the chosen algorithm</param>
        /// <param name="modulus">The modulus</param>
        /// <param name="calcOnlyModuloInverse">If = true, then modulo inverse algorithm will be applied, else EEGCD</param>
        /// <returns>array[3]. In the case of EEGCD: array[0] = multiplicative inverse of a to modulus
        /// array[1] = multiplicative inverse of modulus to a
        /// array[2] = the GCD
        /// 
        /// so that array[2] = a*array[0] + modulus*array[1];
        /// 
        /// In the case of modulo inverse algorithm: array[0] == array[2] = 0; array[1] = positive modulo inverse of a to modulus.
        /// This is the difference from the EEGCD algorithm, which can return negative. This one is used for RSA
        /// </returns>
        public static BigInteger[] Extended_GDC(BigInteger a, BigInteger modulus, Boolean calcOnlyModuloInverse)
        {
            BigInteger x, lastX, b_, y, lastY, a_, quotient, temp, temp2, temp3;
            BigInteger[] result;

            if (modulus == 0) return new BigInteger[] { 1, 0, a };

            // We assure ourselves that the two algorithms below will give good results in any case
            if (a < modulus)
            {
                x = 0; lastX = 1; b_ = modulus;
                y = 1; lastY = 0; a_ = a;
            }
            else
            {
                x = 1; lastX = 0; b_ = a;
                y = 0; lastY = 1; a_ = modulus;
            }

            if (calcOnlyModuloInverse)
            {
                // modulo inverse calculation
                // http://snippets.dzone.com/posts/show/4256
                while (a_ > 0)
                {
                    temp = a_;
                    quotient = b_ / a_;     // If not BigInteger used, then use direct cast to Int32

                    a_ = b_ % temp;
                    b_ = temp;
                    temp = y;

                    y = lastY - quotient * temp;
                    lastY = temp;
                }

                lastY %= modulus;

                if (lastY < 0) lastY = (lastY + modulus) % modulus;
                result = new BigInteger[] { 0, lastY, 0 };
            }
            else
            {
                // Extended euclidian algorithm
                // http://everything2.com/title/Extended+Euclidean+algorithm
                // The only good implementation of the full Extended Euclidian Algorithm that I found
                while (a_ > 1)
                {
                    quotient = b_ / a_;     // If not BigInteger used, then use direct cast to Int32
                    temp = x - quotient * y;
                    temp2 = lastX - quotient * lastY;
                    temp3 = b_ - quotient * a_;

                    x = y; lastX = lastY; b_ = a_;
                    y = temp; lastY = temp2; a_ = temp3;
                }

                if (a_ == 0) result = new BigInteger[] { x, lastX, b_ };
                else result = new BigInteger[] { y, lastY, a_ };
            }

            return result;
        }

        /// <summary>
        /// Method to write de n-1 as pow(2,s)*d, d odd
        /// </summary>
        /// <param name="n">The number from which to extract pow(2,s)*d</param>
        /// <returns>n-1 as pow(2,s)*d. [0] is s, [1] is d</returns>
        /// <remarks>
        /// This transformation can be done more easily, if to use 'UInt32 mask = 0x01' and to do
        /// bit comparison, like in BigInteger RabinMiller test, but since I have no access to the 
        /// data that represents the BigInteger number, I will do a more costly operation
        /// </remarks>
        public static BigInteger[] Get2sdFromNminus1(BigInteger n)
        {
            // Even number passed
            if (n % 2 == 0) return new BigInteger[] { 0, 0 };

            BigInteger tmp = n - new BigInteger(1);
            Int32 counter = 0;
            BigInteger remainder;

            while (true)
            {
                tmp = tmp / 2;
                remainder = tmp % 2;
                counter++;

                // if remainder is different from 0, then we reached an odd number
                if (remainder != 0) break;
            }

            // counter is s, tmp is d, odd
            return new BigInteger[] { counter, tmp };
        }

        /// <summary>
        /// A method that will generate a random number of n bits
        /// Created this method because the one in the BigInteger 
        /// class is generating only 32 bit numbers, even if we specify more. 
        /// Didn't want to touch the BigInteger code
        /// </summary>
        /// <remarks>Reccomendations Reference http://www.di-mgt.com.au/rsa_alg.html</remarks>
        /// <param name="nrBits">number of bits in the generated number</param>
        /// <returns>A BigInteger instance of the n bit number</returns>
        public static BigInteger GenerateOddNBitNumber(UInt32 nrBits)
        {
            //String tmp = "";
            Int32 nr;
            BigInteger b = new BigInteger();

            Random rand = new Random((Int32)DateTime.Now.Ticks);

            // This cycle can be changed to generate not one bit, but more bits at a time
            for (UInt32 i = 0; i < nrBits; i++)
            {
                nr = rand.Next(2);

                // The generated binary number will be calculated like this, because
                // the assignation of the bits is done from the lower to high ones
                // tmp = nr.ToString() + tmp;
                if (nr == 1) b.setBit(i);
            }

            // We assure ourselves that the number is odd, by setting the lower bit to 1
            b.setBit(0);

            // this ensures that the high bit of n is also set
            b.setBit(nrBits - 2);
            b.setBit(nrBits - 1);

            return b;
        }

        /// <summary>
        /// Probabilistic Rabin Milled test for primality
        /// </summary>
        /// <param name="n">The number to test</param>
        /// <param name="confidence">Confidence of the test</param>
        /// <returns>True if probable prime, false otherwise</returns>
        /// <remarks>See wikipedia for more info on the algorithm, 
        /// and BigInteger class from CodeProject</remarks>
        public static Boolean RabinMillerPrimeTest(BigInteger n, Int32 confidence)
        {
            Boolean result = false;
            // The number to test must not be an even number or less than 4
            if (n % 2 == 0 || n < 4) return result;

            // We can test here if the passed number is in the first 1000 primes, without running the algorithm
            //if (n.bitCount() < 32 && PrimeInFirst1000(n.IntValue())) return true;

            Random rand = new Random((Int32)DateTime.Now.Ticks);
            UInt32 randIntNrBits;
            Int32 bitCount;
            BigInteger n_2 = n - new BigInteger(2), x, d, s, n_1 = n - new BigInteger(1);
            BigInteger a;   //random a
            bitCount = n_2.bitCount();

            {
                BigInteger[] tmp2 = Get2sdFromNminus1(n);
                s = tmp2[0];
                d = tmp2[1];
            }

            while (confidence-- > 0)
            {
                // Pick up a random BigInteger from 2 to n-2
                while (true)
                {
                    while (true)
                    {
                        randIntNrBits = (UInt32)(rand.NextDouble() * bitCount);
                        if (randIntNrBits > 2) break;
                    }

                    a = GenerateOddNBitNumber(randIntNrBits);

                    if (a < n_2 || a != 0) break;
                }

                // TODO Add here GCD test between a and n? Not in algorithm, but in BigInteger class used

                x = ModuloExp(a, d, n); // this is much slower than a.modPow(d,n) from BigInteger class.
                result = false;
                if (x == 1 || x == n_1) { result = true; continue; }

                for (BigInteger r = 0; r < s; r += 1)
                {
                    x = (x * x) % n;
                    if (x == 1) { result = false; break; }
                    if (x == n_1) { result = true; break; }
                }

                if (result == false) break;
            }

            return result;
        }


        public static BigInteger ModuloExp(BigInteger baze, BigInteger exp, BigInteger modulus)
        {
            BigInteger c = new BigInteger(1);

            while (exp > 0)
            {
                if ((exp & 1) == 1)
                    c = (c * baze) % modulus;
                exp = exp >> 1;
                baze = (baze * baze) % modulus;
            }
            return c;
        }

        #endregion STATIC_METHODS
    }
}
