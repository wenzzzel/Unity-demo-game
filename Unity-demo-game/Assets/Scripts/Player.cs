using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Azure.Messaging.WebPubSub;
using Unity.VisualScripting;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text;
using System;

public class Player : MonoBehaviour
{

    public int movementSpeed;
    public Rigidbody2D rigidBody;
    private Vector2 movementInput;
    private PlayerPosition playerPosition = new PlayerPosition()
    {
        PlayerID = Guid.NewGuid(),
        X = 0f,
        Y = 0f
    };

    private string connectionString = @"Endpoint=https://ewenzelwebpubsub.webpubsub.azure.com;AccessKey=E1d65qvhNYr4TQmwBoMH+7rFWz8qMZYYh63NO0dTDNw=;Version=1.0;";
    private string hub = "hub";
    private WebPubSubServiceClient serviceClient;

    // Start is called before the first frame update
    void Start()
    {
        serviceClient = new WebPubSubServiceClient(connectionString, hub);
        InvokeRepeating("sendPlayerPosition", 3f, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        movementInput.x = Input.GetAxis("Horizontal");
        movementInput.y = Input.GetAxis("Vertical");
    }

    void FixedUpdate()
    {
        rigidBody.MovePosition(rigidBody.position + movementInput * movementSpeed * Time.fixedDeltaTime);
        playerPosition.X = rigidBody.position.x;
        playerPosition.Y = rigidBody.position.y;
    }

    private async Task sendPlayerPosition() => await serviceClient.SendToAllAsync(JsonSerializer.Serialize(playerPosition));
}
