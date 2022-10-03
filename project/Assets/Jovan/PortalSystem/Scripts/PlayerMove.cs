using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public CharacterController controller;
    public GameObject player;
    public float fallThreshold;
    public Transform respawnPoint;
   
    Vector3 lastPos;
    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask; 

    Vector3 velocity;
    bool isGrounded;
    AudioSource walkAudio;
    bool isMoving = false;
    //GameObject playerFell;
    
    void Start(){
        Cursor.visible = false;
        walkAudio = GetComponent<AudioSource>();
        //playerFell = GameObject.Find ("Player");
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        

        Vector3 move = transform.right * x + transform.forward * z;
        //print(move);

        controller.Move(move * speed * Time.deltaTime);

        if (player.transform.position != lastPos){
            isMoving = true;
        }
        else{
            isMoving = false;
        }
        if (isMoving){
            if (!walkAudio.isPlaying){
                walkAudio.volume = Random.Range(0.3f, 0.5f);
                walkAudio.pitch = Random.Range(0.7f, 1.0f);
                walkAudio.Play();
            }
        }
        else{
            walkAudio.Stop();
        }
        lastPos = player.transform.position;
        //walkAudio.Play();

         

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        //print(isMoving);

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    void FixedUpdate() {
        if (transform.position.y < fallThreshold){
            transform.position = respawnPoint.position;
        }
    }

 
    

    
}
