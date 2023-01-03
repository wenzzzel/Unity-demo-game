using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public int movementSpeed;
    public Rigidbody2D rigidBody;
    private Vector2 movementInput;

    // Start is called before the first frame update
    void Start()
    {
        
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
    }
}
