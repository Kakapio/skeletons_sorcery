using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    private CharacterController cc;
    
    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        // Get raw axes so we do not have floaty acceleration.
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        // Account for diagonal movement speed.
        if (input.x != 0 && input.y != 0)
            input *= 0.7071f;
        
        Vector3 movement = transform.forward * input.y + transform.right * input.x;
        cc.Move(movement * speed * Time.deltaTime);
    }
}
