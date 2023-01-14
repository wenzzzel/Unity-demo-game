using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Azure.Messaging.WebPubSub;
using Unity.VisualScripting;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text;
using System;
using Websocket.Client;
using System.Net.WebSockets;
using System.Collections.Concurrent;
using System.Threading;
using System.Reactive;
using UnityEditor.Experimental.GraphView;
using UnityEditor.PackageManager;
using System.Net;

public class OtherPlayer : MonoBehaviour
{
    public int testvalue;

    public Transform myTransform;
    public Rigidbody2D rigidBody;
    public PlayerPosition playerPosition = new PlayerPosition()
    {
        PlayerID = Guid.NewGuid(),
        X = 0f,
        Y = 0f
    };

    private string connectionString = @"Endpoint=https://ewenzelwebpubsub.webpubsub.azure.com;AccessKey=E1d65qvhNYr4TQmwBoMH+7rFWz8qMZYYh63NO0dTDNw=;Version=1.0;";
    private string hub = "hub";
    private WebPubSubServiceClient serviceClient;
    private Uri webSocketUrl;
    private WebSocketClient client;

    void Start()
    {
        serviceClient = new WebPubSubServiceClient(connectionString, hub);
        webSocketUrl = serviceClient.GetClientAccessUri();
        //server = "wss://" + host;
        Debug.Log(webSocketUrl.ToString());
        client = new WebSocketClient(webSocketUrl.ToString());
        client.Connect();
    }

    void Update()
    {
        var cqueue = client.receiveQueue;
        string msg;
        while (cqueue.TryPeek(out msg))
        {
            cqueue.TryDequeue(out msg);
            HandleMessage(msg);
        }
    }

    private void HandleMessage(string msg)
    {
        Debug.Log("Server: " + msg);
        JsonUtility.FromJsonOverwrite(msg, playerPosition);
        myTransform.position = new Vector2(playerPosition.X, playerPosition.Y);
    }

    void FixedUpdate()
    {
        
    }
}
