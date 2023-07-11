using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    private Rigidbody rb;
    private int pickupCount;
    public Timer timer;
    public bool gameOver;

    [Header("UI")]
    public GameObject inGamePanel;
    public GameObject winPanel;
    public TMP_Text scoreText;
    public TMP_Text timerText;
    public TMP_Text WinTimeText;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //Get the number of pickups in our scene
        pickupCount = GameObject.FindGameObjectsWithTag("Pick Up").Length;
        //Run the check pickups function
        CheckPickups();
        //Get the timer object and start the timer
        timer = FindObjectOfType<Timer>();
        timer.StartTime();
        //Turn on our in game panel
        inGamePanel.SetActive(true);
        //Turn off our in game panel
        winPanel.SetActive(false);
        gameOver = false;   
    }

    private void Update()
    {
        timerText.text = "Time " + timer.GetTime().ToString("F2");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gameOver == true)
            return;
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);
        rb.AddForce(movement * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
       if(other.tag == "Pick Up")
        {
            Destroy(other.gameObject);
            //Dectrement the pickup count
            pickupCount -= 1;
            //Run the check pickups function
            CheckPickups();
            //Get the timer object
            timer = FindObjectOfType<Timer>();
        }
    }

    void CheckPickups()
    {
        //Display the amount of pickups left in our scene
        scoreText.text = "Pickups Left: " + pickupCount;

        if (pickupCount == 0)
        {
            WinGame();
        }
    }
    void WinGame()
    {
        //Set the game over to true
        gameOver = true;
        //Stop the timer
        timer.StopTimer();
        //Turn on our win panel
        winPanel.SetActive(true);
        //Turn off our in game panel
        inGamePanel.SetActive(false);
        //Display the timer on the win time text
        WinTimeText.text = "Your time " + timer.GetTime().ToString("F2");

        //Set the velocity of the rigidbody to zero
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
    //Temporary - Remove when doing modules in A2
    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene
            (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}

