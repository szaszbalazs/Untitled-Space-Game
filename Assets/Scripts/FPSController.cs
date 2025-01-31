using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
{
    public Camera FpsCamera;
    public float walkspeed = 6f;
    public float runSpeed =  2f;
    public float jumpPower = 7f;
    public float gravity = 10f;

    public float lookSpeed = 2f;
    public float lookXlimit = 45f;

    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    public bool canMove = true;

    CharacterController characterController;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Bugs: Player can sprint sideways
        //      


        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        bool isRunning = Input.GetKey(KeyCode.LeftShift);


        //TODO: Movement ne az updateben legyen meghívva 200x framenként condition nélkül.

        float curSpeedX = canMove ? ((isRunning && characterController.isGrounded && Input.GetAxis("Vertical") >= 1)? runSpeed : walkspeed) * Input.GetAxis("Vertical") : 0;

        //smelly code
        float curSpeedY = canMove ? (characterController.isGrounded ? walkspeed : walkspeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        //Jumping
        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpPower;
        }
        else 
        {
            moveDirection.y = movementDirectionY;
        }

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        //Rotation

        characterController.Move(moveDirection * Time.deltaTime);

        if (canMove) 
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXlimit,lookXlimit);
            FpsCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0,Input.GetAxis("Mouse X") * lookSpeed, 0);
        }


        if (Input.GetKey(KeyCode.G))
        {
            SwitchCamera(FpsCamera);
        } 
    }

    void SwitchCamera(Camera camera)
    {
        
    }
}
