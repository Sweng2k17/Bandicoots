using System;

using System.Collections;

using System.Collections.Generic;

using System.Net.Sockets;

using System.Text;

using System.Threading;

using UnityEngine;



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
    public void attemptConnection()
    {
        //try-catch temporary
        try
        {
            socketConnection = new TcpClient("127.0.0.1", 8888);
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
                            else
                            {
                                Debug.Log("data enqueued");
                                sort(serverMessage);
                            }
                        }
                        //data.Enqueue(serverMessage);

                        //Debug.Log(serverMessage);

                        Debug.Log("server message received as: " + serverMessage);
                    }
                }

            }
        }

        catch (SocketException socketException)
        {
            Debug.Log("Socket exception: " + socketException);
        }
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

    public void SendMessage()
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

                string clientMessage = "This is a message from one of your clients.";

                // Convert string message to byte array.                 

                byte[] clientMessageAsByteArray = Encoding.ASCII.GetBytes(clientMessage);

                // Write byte array to socketConnection stream.                 

                stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);

                Debug.Log("Client sent message - should be received by server");

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

}
