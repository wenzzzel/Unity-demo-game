using UnityEngine;
using Azure.Messaging.WebPubSub;
using System;
using System.Collections.Concurrent;

public class WebSocketListeningEntity : MonoBehaviour
{
    private PlayerPosition myBroadcastedPosition = new PlayerPosition()
    {
        PlayerID = Guid.NewGuid(),
        X = 0f,
        Y = 0f
    };
    private string webSocketConnectionString = @"Endpoint=https://ewenzelwebpubsub.webpubsub.azure.com;AccessKey=E1d65qvhNYr4TQmwBoMH+7rFWz8qMZYYh63NO0dTDNw=;Version=1.0;";
    private string webSocketHub = "hub";
    private WebPubSubServiceClient webSocketServiceClient;
    private Uri webSocketUrl;
    private WebSocketClient webSocketClient;
    private ConcurrentQueue<string> webSocketQueue;

    void Start()
    {
        ConnectToWebSocket();
    }

    void Update()
    {
        SubscribeToWebsocket();
    }

    void FixedUpdate()
    {
        
    }

    private void ConnectToWebSocket()
    {
        webSocketServiceClient = new WebPubSubServiceClient(webSocketConnectionString, webSocketHub);
        webSocketUrl = webSocketServiceClient.GetClientAccessUri();
        webSocketClient = new WebSocketClient(webSocketUrl.ToString());
        webSocketClient.Connect();
        webSocketQueue = webSocketClient.receiveQueue;
    }

    private void SubscribeToWebsocket()
    {
        if (webSocketClient.IsConnectionOpen())
        {
            string msg;
            while (webSocketQueue.TryPeek(out msg))
            {
                webSocketQueue.TryDequeue(out msg);
                MoveMe(msg);
            }
        }
    }

    private void MoveMe(string msg)
    {
        Debug.Log("Server: " + msg);
        JsonUtility.FromJsonOverwrite(msg, myBroadcastedPosition);
        transform.position = new Vector2(myBroadcastedPosition.X, myBroadcastedPosition.Y);
    }
}
