using Azure.Messaging.WebPubSub;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using UnityEditor.PackageManager;
using UnityEngine;
using Websocket.Client;

public class WebSocketListener : MonoBehaviour
{
    public Player player;

    private string connectionString = @"Endpoint=https://ewenzelwebpubsub.webpubsub.azure.com;AccessKey=E1d65qvhNYr4TQmwBoMH+7rFWz8qMZYYh63NO0dTDNw=;Version=1.0;";
    private string hub = "hub";
    private WebPubSubServiceClient serviceClient;
    private Uri webSocketUrl;

    // Start is called before the first frame update
    void Start()
    {
        ReadPlayerPositions();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ReadPlayerPositions()
    {
        serviceClient = new WebPubSubServiceClient(connectionString, hub);
        webSocketUrl = serviceClient.GetClientAccessUri();
        var client = new WebsocketClient(webSocketUrl);
        client.ReconnectTimeout = null;
        client.MessageReceived.Subscribe(msg => Debug.Log($"Message received: {msg}")
        );;
        client.Start();
    }
}
