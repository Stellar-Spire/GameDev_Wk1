using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    private Rigidbody rb;
    private int pickupCount;
    public Timer timer;
    public bool gameOver;
    GameObject resetPoint;
    bool resetting = false;
    Color originalColor;

    //Controllers
    public GameController gameController;
    [Header("UI")]
    public GameObject inGamePanel;
    public GameObject winPanel;
    public TMP_Text scoreText;
    public TMP_Text timerText;
    public TMP_Text WinTimeText;
    public GameObject pausePanel;
    public GameObject gameoverPanel;
    public CameraController cameraController;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        rb = GetComponent<Rigidbody>();
        //Get the number of pickups in our scene
        pickupCount = GameObject.FindGameObjectsWithTag("Pick Up").Length;
        //Run the check pickups function
        SetCountText();
        //Get the timer object and start the timer
        timer = FindObjectOfType<Timer>();
        timer.StartTime();
        //Turn on our in game panel
        inGamePanel.SetActive(true);
        //Turn off our in game panel
        winPanel.SetActive(false);
        pausePanel.SetActive(false);
        gameoverPanel.SetActive(false);
        gameOver = false;
        resetPoint = GameObject.Find("Reset Point");
        originalColor = GetComponent<Renderer>().material.color;
        gameController = FindObjectOfType<GameController>();
        cameraController = FindObjectOfType<CameraController>();
    }
    private void Update()
    {
        timerText.text = " " + timer.GetTime().ToString("F2");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (resetting)
            return;
        if (gameOver == true)
            return;
        float moveHorizontal = Input.GetAxis("Vertical");
        float moveVertical = Input.GetAxis("Horizontal");

        Vector3 movement = new Vector3(-moveHorizontal, 0, moveVertical);

        if(cameraController.cameraStyle == CameraStyle.Free)
        {
            //Rotates the player to the direction of the camera
            transform.eulerAngles = Camera.main.transform.eulerAngles;
            //Translates the input vectors into coordinates
            movement = transform.TransformDirection(movement);
        }

        rb.AddForce(movement * speed);

        if (gameController.controlType == ControlType.WorldTilt)
            return;
    }
    private void OnTriggerEnter(Collider other)
    {
       if(other.tag == "Pick Up")
        {
            Destroy(other.gameObject);
            //Dectrement the pickup count
            pickupCount -= 1;
            //Run the check pickups function
            SetCountText();
            //Get the timer object
            timer = FindObjectOfType<Timer>();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Respawn"))
        {
            if(SceneManager.GetActiveScene().name == "MyScene")
            {
                gameoverPanel.SetActive(true);
                rb.velocity = Vector3.zero;
                resetting = true;
            }
            else 
                StartCoroutine(ResetPlayer());

            if (collision.gameObject.CompareTag("Wall"))
            {
                if (gameController.wallType == WallType.Punishing)
                    StartCoroutine(ResetPlayer());
            }
        }
    }
    public IEnumerator ResetPlayer()
    {
        resetting = true;
        GetComponent<Renderer>().material.color = Color.black;
        rb.velocity = Vector3.zero;
        Vector3 startPos = transform.position;
        float resetSpeed = 2f;
        var i = 0.0f;
        var rate = 1.0f / resetSpeed;
        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;
            transform.position = Vector3.Lerp(startPos, resetPoint.transform.position, i);
            yield return null;
;        }
        GetComponent<Renderer>().material.color = originalColor;
        resetting = false;
    }
    void SetCountText()
    {
        //Display the amount of pickups left in our scene
        scoreText.text = " " + pickupCount;

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

