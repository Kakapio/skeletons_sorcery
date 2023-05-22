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
        Shoot();
    }

    private void Move()
    {
        // Get raw axes so we do not have floaty acceleration.
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        
        // Prevent extra fast diagonal movement.
        input.Normalize();
        
        Vector3 movement = transform.forward * input.y + transform.right * input.x;
        cc.Move(movement * speed * Time.deltaTime);
    }

    private void Shoot()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().ScreenToWorldPoint(mousePos);
        mousePos -= transform.position;
        Debug.Log($"Mouse Pos relative to player is: " + mousePos.ToString());
    }
}
