ClientServer-secureCommunication

Client and server chat application that implements cryptographic algorithm

Client can send message to server . client has the option to select the AES,RSA algorithms for encrytion. used in built C# implementation for AES.

User has the option to select C# RSA's implmentation or C# user implmentation. RSA implementation in which rsa algorithm has been implmeneted without using any inbuilt C# library.

Used C#, Visual studio 2010.

Server is a console application which decrypts the message. Client intiates the alogrithm and passes on the key to server.

Client selects algorithm. key is generated for both client and server. for the purpose of demo the paramters required to decrypt message is passed on to server.Server displays encrypted message and original decrypted messages.

Instruction to run Client-Server Project
Tools required for code overview - Microsoft Visual Studio 2010, .Net 4.5
Steps:-
1.	Run ServerApplication. Server is initialized first and then the client connects to server. Make sure that client application is started first.
2.	Run ClientApplication. Client will get connected to server  
3.	Select Algorithm and then client can send message to server.
