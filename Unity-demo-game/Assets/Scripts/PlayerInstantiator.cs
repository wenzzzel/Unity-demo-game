using Azure.Messaging.WebPubSub;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Websocket.Client;

public class PlayerInstantiator : MonoBehaviour
{
    public Player player;
    private string localPlayerId;

    private List<PlayerPosition> connectedPlayers = new List<PlayerPosition>();

    public GameObject playerToInstantiate;

    private string webSocketConnectionString = @"Endpoint=https://ewenzelwebpubsub.webpubsub.azure.com;AccessKey=E1d65qvhNYr4TQmwBoMH+7rFWz8qMZYYh63NO0dTDNw=;Version=1.0;";
    private string webSocketHub = "hub";
    private WebPubSubServiceClient webSocketServiceClient;
    private Uri webSocketUrl;
    private WebSocketClient webSocketClient;
    private ConcurrentQueue<string> webSocketQueue;

    // Start is called before the first frame update
    void Start()
    {
        localPlayerId = player.playerPosition.PlayerID;
        Debug.Log("PlayerInstantiator says: " + localPlayerId);
        ConnectToWebSocket();
    }

    // Update is called once per frame
    void Update()
    {
        SubscribeToWebsocket();
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
                CheckForNewPlayers(msg);
            }
        }
    }

    private void CheckForNewPlayers(string msg)
    {
        var player = JsonUtility.FromJson<PlayerPosition>(msg);

        if (connectedPlayers.Any(x => x.PlayerID == player.PlayerID))
            return;

        connectedPlayers.Add(player);
        Debug.Log("A new player has entered the game! Total count is now: " + connectedPlayers.Count.ToString());

        if (player.PlayerID == localPlayerId)
            return;

        //Instantiate new GameObject
        Debug.Log("Instantiating new game object");
        Instantiate(playerToInstantiate, new Vector2(player.X, player.Y), Quaternion.identity);
    }
}
