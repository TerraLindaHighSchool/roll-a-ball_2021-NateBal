using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;


public class PlayerController : MonoBehaviour
{
    public float speed = 0;
    [SerializeField] private float jumpStrength = 500;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;
    private Rigidbody rb;
    private int count;
    private bool isOnGround;
    private string nextLevel;
    private int maxCount;
    private float movementX;
    private float movementY;

    // Start is called before the first frame update
    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        rb = GetComponent<Rigidbody>();
        count = 0;

        SetCountText();
        
        winTextObject.SetActive(false);
        LoadLevelData();
        if (sceneName == "WinScreen") 
        {
            winTextObject.SetActive(true);
        }
    }
    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if(count >= maxCount)
        {
            SceneManager.LoadScene(nextLevel);
        }
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(movement * speed);

        Restart();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Pickup"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;

            SetCountText();
        }

        if (other.gameObject.CompareTag("Amogus"))
        {
            other.gameObject.SetActive(false);
            SceneManager.LoadScene("CreditAttempt");
        }
    }

    void OnJump()
    {
        if (isOnGround)
        {
            rb.AddForce(new Vector3(0, jumpStrength, 0));
        }
    }

    void Restart()
    {
        if(transform.position.y < -15)
        {
            SceneManager.LoadScene("Level1");
        }
    }

    void OnLose()
    {
        SceneManager.LoadScene("Level1");
    }
     void LoadLevelData()
    {
        switch(SceneManager.GetActiveScene().name)
        {
            
            case "Level1": maxCount = 10;
                nextLevel = "Level2";
                break;
            case "Level2":
                maxCount = 12;
                nextLevel = "Level3";
                break;
            case "Level3":
                maxCount = 7;
                nextLevel = "WinScreen";
                break;
            case "WinScreen":
                nextLevel = "CreditAttempt";
                break;

        }
    }

    private void OnCollisionStay(Collision otherObject)
    {
        if (otherObject.gameObject.tag == "Platform")
        {
            isOnGround = true;
        }
    }

    private void OnCollisionExit(Collision otherObject)
    {
        if (otherObject.gameObject.tag == "Platform")
        {
            isOnGround = false;
        }
    }

}
