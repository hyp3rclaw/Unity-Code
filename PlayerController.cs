using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public Transform viewPoint; 

    public float mouseSensitivity=1f;

    private float verticalRotStore;

    private Vector2 mouseInput;

    public bool invertLook;

    public float moveSpeed=5f,runSpeed=8f;

    private float activemoveSpeed;

    private Vector3 moveDir, movement;

    public CharacterController charCon;

    private Camera cam;

    public float jumpForce=12f,gravityMod=2.5f;

    public Transform groundCheckPoint;
    
    private bool isGrounded;

    public LayerMask groundLayers;


    void Start()
    {

        Cursor.lockState = CursorLockMode.Locked; // setting cursor boundary
        

        cam=Camera.main;
    }

   
    void Update()
    {
        //looking left-right
        mouseInput= new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSensitivity;

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x,transform.rotation.eulerAngles.y + mouseInput.x,transform.rotation.eulerAngles.z);
        
        //looking up-down
        verticalRotStore += mouseInput.y;

        verticalRotStore = Mathf.Clamp(verticalRotStore,-60f,60f);

        if(invertLook)
        {
        viewPoint.rotation=Quaternion.Euler(verticalRotStore,viewPoint.rotation.eulerAngles.y,viewPoint.rotation.eulerAngles.z);
        }
        else
        {
        viewPoint.rotation=Quaternion.Euler(-verticalRotStore,viewPoint.rotation.eulerAngles.y,viewPoint.rotation.eulerAngles.z);  
        }

        //movement of player    
        moveDir = new Vector3(Input.GetAxisRaw("Horizontal"),0f,Input.GetAxisRaw("Vertical"));

        if(Input.GetKey(KeyCode.LeftShift))
        {

                activemoveSpeed=runSpeed;

        }else
        {
            activemoveSpeed = moveSpeed;
        }

        float yVel=movement.y;

        movement = ((transform.forward * moveDir.z)+(transform.right *moveDir.x)).normalized * activemoveSpeed; //player facing in correct direction

        movement.y= yVel;

        if(!charCon.isGrounded)
        {
            movement.y=0f;
        }
        isGrounded = Physics.Raycast(groundCheckPoint.position, Vector3.down, .25f, groundLayers);
        if(Input.GetButtonDown("Jump") && isGrounded )
        {
            movement.y = jumpForce;
        }
        

        movement.y+=Physics.gravity.y *  Time.deltaTime * gravityMod;

        //transform.position+=movement * moveSpeed * Time.deltaTime; //player facing and moving in correct direction per frame
        
        //charCon.Move(movement * moveSpeed * Time.deltaTime); //charCon.Move applies movement to the player

        charCon.Move(movement * Time.deltaTime);

    }

private void LateUpdate() 
{

cam.transform.position = viewPoint.position;
cam.transform.rotation= viewPoint.rotation;        

}
}
