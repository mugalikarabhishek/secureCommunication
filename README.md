ClientServer-secureCommunication

Client and server chat application that implements cryptographic algorithm

Client can send message to server . client has the option to select the AES,RSA algorithms for encrytion. used in built C# implementation for AES.

User has the option to select C# RSA's implmentation or C# user implmentation. RSA implementation in which rsa algorithm has been implmeneted without using any inbuilt C# library.

Used C#, Visual studio 2010.

Server is a console application which decrypts the message. Client intiates the alogrithm and passes on the key to server.

Client selects algorithm. key is generated for both client and server. for the purpose of demo the paramters required to decrypt message is passed on to server.Server displays encrypted message and original decrypted messages.
