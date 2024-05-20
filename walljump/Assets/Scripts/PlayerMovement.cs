using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D myRigidbody;
    public float sprintSpeed = 10f;
    public float walkSpeed = 5f;
    public float wallJumpingPowerX = 5f;
    public float wallJumpingPowerY = 3f;
    private float moveSpeed = 0;
    private float horizontalInput;
    private static float delay = 0;
    private bool isRunning;
    private bool isGrounded;
    private bool onWall;
    private bool pass = false;
    private static int sceneIndex = 0;
    private string[] sceneNames;

    void Start() {
        sceneNames = new string[] {"1", "2", "3"};
    }
    
    // Update is called once per frame
    void Update()
    {
        Restart();

        if(Input.GetKeyDown(KeyCode.Space)) {
            if(isGrounded) {
                myRigidbody.velocity = Vector2.up * 7;
                isGrounded = false;
            } else if(onWall) {
                myRigidbody.velocity = new Vector2(-transform.localScale.x * wallJumpingPowerX, wallJumpingPowerY); 
                transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
                delay = 0.5f;
            }
        }
    
        if(Input.GetKey(KeyCode.LeftShift)) {
            isRunning = true;
        } else {
            isRunning = false;
        }

        if(delay <= 0) {
            horizontalInput = Input.GetAxisRaw("Horizontal");
            if(horizontalInput > 0) {
                transform.localScale = new Vector3(1, 1, 1);
                myRigidbody.velocity = new Vector2(0, myRigidbody.velocity.y);
            } else if(horizontalInput < 0) {
                transform.localScale = new Vector3(-1,1,1);
                myRigidbody.velocity = new Vector2(0, myRigidbody.velocity.y);
            }
        } else {
            delay -= Time.deltaTime;
            horizontalInput = 0;
        }
    }

    private void FixedUpdate() {
        
        Vector3 moveDirection = new Vector3(horizontalInput, 0, 0).normalized;
        if(isRunning) {
            moveSpeed = sprintSpeed;
        } else {
            moveSpeed = walkSpeed;
        }
        if(horizontalInput != 0) {
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
        }
        
    }
    void OnCollisionEnter2D(Collision2D other){
        if(other.gameObject.tag == "Ground") {
            isGrounded = true;
        }

        if(other.gameObject.tag == "Wall") {
            onWall = true; 
        }

        if(other.gameObject.tag == "Door" &&  pass) {
            LoadScene();
        }

        if(other.gameObject.tag == "Key") {
            pass = true;
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if(other.gameObject.tag == "Wall") {
            onWall = false;
        }
    }

    private void Restart() {
        if(Input.GetKeyDown(KeyCode.R)) {
            if(sceneIndex == 2) {
                sceneIndex = 0;
            }
            SceneManager.LoadScene(sceneNames[sceneIndex]); 
        }
    }

    private void LoadScene() {
        if(sceneIndex < sceneNames.Length) {
            sceneIndex++;
            SceneManager.LoadScene(sceneNames[sceneIndex]);
        }
    }

}
