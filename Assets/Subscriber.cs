using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using System.Security.Cryptography;



public class Subscriber
{

    #region private members 	
    private bool reading;
    private volatile Queue tData;
    private volatile Queue bData;
    private bool connected;
    private volatile String serverMessage;
    private TcpClient socketConnection;
    private Thread clientReceiveThread;
    #endregion

    // Use this for initialization 

    public Subscriber()
    {
        bData = new Queue();
        tData = new Queue();
    }

    //returns whether the socket is connected or not
    public bool isConnected()
    {
        return connected;
    }

    //attempts to connect the socket
    public void attemptConnection(string IPAddress, int portNumber)
    {
        Debug.Log("IP Address is " + IPAddress + " & portNumber is " + portNumber);
        //try-catch temporary
        try
        {
            //socketConnection = new TcpClient("127.0.0.1", 8888);
            socketConnection = new TcpClient(IPAddress, portNumber);
            ConnectToTcpServer();
            reading = true;
            connected = true;
            Debug.Log("Connection successful.");
        }
        catch (Exception e)
        {
            connected = false;
            Debug.Log("Connection unsuccessful.");
            Debug.Log(e.ToString());
        }
    }

    /// <summary> 	

    /// Setup socket connection. 	

    /// </summary> 	

    public void ConnectToTcpServer()
    {

        try
        {
            clientReceiveThread = new Thread(new ThreadStart(ListenForData));
            clientReceiveThread.IsBackground = true;
            clientReceiveThread.Start();
        }
        catch (Exception e)
        {
            Debug.Log("On client connect exception " + e);
        }

    }


    /// <summary> 	
    /// Runs in background clientReceiveThread; Listens for incoming data. 	
    /// </summary>
    public void ListenForData()
    {

        try
        {
            Byte[] bytes = new Byte[1024];

            while (reading)
            {

                // Get a stream object for reading 				

                using (NetworkStream stream = socketConnection.GetStream())
                {
                    int length;

                    // Read incomming stream into byte array. 					
                    while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        var incomingData = new byte[length];
                        Array.Copy(bytes, 0, incomingData, 0, length);

                        // Convert byte array to string message. 						
                        serverMessage = Encoding.ASCII.GetString(incomingData);

                        if ((serverMessage != null))
                        {
                            if (serverMessage.Equals("End of file."))
                            {
                                reading = false;
                                Debug.Log("done reading data - reading value: " + reading);
                                return;
                            }
                            else if(!serverMessage.Equals(null))
                            {
								Debug.Log("Server Message: " + serverMessage);
								
								handleHashing(stream, serverMessage);
                            }
                        }
                    }
                }

            }
        }

        catch (SocketException socketException)
        {
            Debug.Log("Socket exception: " + socketException);
        }
    }

	/// <summary>
	/// Receives data from publisher. This function is utilized in handleHashing() 
	/// mainly because reusing ListenForData() causes issues (I think because of the 
	/// multi-threading).
	/// </summary>
	/// <returns></returns>
	public string receiveFromPub()
	{
			byte[] bytesFromPub = new byte[10025];
			string dataFromPub = null;
			NetworkStream nStream = socketConnection.GetStream();

			nStream.Read(bytesFromPub, 0, (int)bytesFromPub.Length); // Bytes from Publisher
			dataFromPub = System.Text.Encoding.ASCII.GetString(bytesFromPub); // Converts the bytes to ASCII
			
			return dataFromPub;
	}

	public bool getReading()
    {
        return reading;
    }

    public Queue getTData()
    {
        return tData;
    }

    public Queue getBData()
    {
        return bData;
    }

    public String getMessage()
    {
        return serverMessage;
    }

    /// <summary> 	
    /// Send message to server using socket connection. 	
    /// </summary> 	
    public void SendMessage(string message)
    {
        if (socketConnection == null)
        {
            Debug.Log("There is no connection established.");
            return;
        }
        try
        {
            // Get a stream object for writing. 			
            NetworkStream stream = socketConnection.GetStream();

            if (stream.CanWrite)
            {
                // Convert string message to byte array.                 
                byte[] clientMessageAsByteArray = Encoding.ASCII.GetBytes(message);
                // Write byte array to socketConnection stream.                 
                stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);

                Debug.Log("Client hash value sent -- awaiting response from publisher...");
            }

        }

        catch (SocketException socketException)
        {
            Debug.Log("Socket exception: " + socketException);
        }

    }

    private void sort(string message)
    {
        int numCommas = 0;
        foreach (char c in message)
        {
            if (c == ',')
            {
                numCommas++;
            }
        }
        if (numCommas == 9)
        {
            Debug.Log("enqueing beam data");
            Debug.Log("beam " + message);
            bData.Enqueue(message);
        }
        else
        {
            Debug.Log("enqueing target data");
            Debug.Log("target " + message);
            tData.Enqueue(message);
        }
    }

	/// <summary>
	/// Method for creating SHA-1 Hashes
	/// </summary>
	/// <param name="csvLine"></param> CSV line we wish to hash.
	private string hashSHA1(string csvLine)
	{
		SHA1CryptoServiceProvider hashMaker = new SHA1CryptoServiceProvider();
		hashMaker.ComputeHash(ASCIIEncoding.ASCII.GetBytes(csvLine)); // Creates a hash of our csvLine data
		byte[] hashBytes = hashMaker.Hash; // move hashed byte values into byte array
		StringBuilder sb = new StringBuilder();
		foreach (byte b in hashBytes)
		{
			sb.Append(b.ToString("X2")); // "X2" converts bytes to a hex format
		}
		return sb.ToString();
	}
	
	/// <summary>
	/// Handles data verification by hashing the data received and receiving data from
	/// the publisher.
	/// </summary>
	/// <param name="nStream"></param>	An open network socket.
	/// <param name="csvLine"></param>	Current line of a CSV file.
	/// <returns></returns>
	public void handleHashing(NetworkStream nStream, String csvLine)
	{
		string csvHash = hashSHA1(csvLine); // hash csv line
		string receivedHash;

		// receiveFromPub() returns a string. We make a substring of that returned string
		// because we make a large byte array to recieve just incase a large hash value
		// is being sent over the network. However, since the hash values must match exactly,
		// we accomdate for the extra spaces in the byte array by making a substring of that
		// received hash value with the length of our made hash value.
		receivedHash = receiveFromPub().Substring(0, csvHash.Length);

		Debug.Log("Created Hash: " + csvHash + ".");
		Debug.Log("Received Hash: " + receivedHash + ".");

		if (receivedHash.Equals(csvHash))
		{
			Debug.Log("Hash matches.");
			// Correct data, we can enqueue
			sort(csvLine);		
		}
		else
		{
			Debug.Log("Hash is different.");
		}
		
	}
}
