using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Scene Objects")]
    public GameObject Structure;
    public GameObject HUD;
    public Ball ball;

    [Header("Game Parameters")]
    public float rotationSpeed = 1;
    public float TapSpeed = 0.2f;
    public int level = 0;

    [Header("UI elements")]
    public GameObject EndingMenu;
    public TextMeshProUGUI EndingText;
    public TextMeshProUGUI EndingNextLevelText;
    public GameObject PauseMenu;
    public TextMeshProUGUI HUDLevel;

    private float touchStart = 0;
    private float displacement;
    private float prevRotation;
    
    private LevelGenerator levelGenerator;
    private PlayerData data;
    private bool canPause = true;
    private bool isPaused = false;

    private float timeTapBegan;

    public void GoToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    void Start()
    {
        // initializing class members
        levelGenerator = GetComponent<LevelGenerator>();
        prevRotation = 0f;

        // set up UI elements
        PauseMenu.SetActive(false);
        EndingMenu.SetActive(false);
        
        // set up Action Event Handling
        ball.OnEndingReached += HandleEnding;

        // load data from player save
        data = SaveSystem.LoadPlayer();
        if(data != null)
        {
            level = data.level;
        }
        HUDLevel.text = level.ToString();

        // generate the first few rings
        levelGenerator.SetupLevel(level);

        //If not first level
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        HandleTouch();
    }

    /// <summary>
    /// Determine if victory or defeat occured based on event from ball collision
    /// </summary>
    /// <param name="isVictory"></param>
    private void HandleEnding(bool isVictory)
    {
        canPause = false;
        Time.timeScale = 0;

        if (isVictory)
        {
            EndingText.text = "Victory!";
            EndingNextLevelText.text = "Next Level";
            level++;
            SaveSystem.SavePlayer(level);
        }
        else
        {
            EndingText.text = "Game Over!";
            EndingNextLevelText.text = "Try Again?";
        }

        EndingMenu.SetActive(true);

    }

    /// <summary>
    ///  restarts the level
    /// </summary>
    public void RestartLevel()
    {
        // load itself 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    void HandleTouch()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                DragStart(touch);
                timeTapBegan =  Time.time;
            }
            if (touch.phase == TouchPhase.Moved)
            {
                Dragging(touch);
            }
            if (touch.phase == TouchPhase.Ended)
            {
                DragRelease(touch);

                // Check for a quick tap
                if(Time.time - timeTapBegan < TapSpeed)
                {
                    SingleTap();
                }
            }
        }
    }

    void SingleTap()
    {
        if (canPause && !isPaused)
        {
            Time.timeScale = 0;
            isPaused = true;
            PauseMenu.SetActive(true);
        }
        else if (isPaused)
        {
            Time.timeScale = 1;
            isPaused = false;
            PauseMenu.SetActive(false);
        }
    }
    void DragStart(Touch touch)
    {
        touchStart = touch.position.x;
    }
    void Dragging(Touch touch)
    {
        if(Time.timeScale > 0)
        {
            displacement = (touch.position.x - touchStart) * rotationSpeed;
            Structure.transform.eulerAngles = new Vector3(0, prevRotation - displacement, 0);
        }
    }
    void DragRelease(Touch touch)
    {
        //remember the rotation of the structure for next drag
        prevRotation = Structure.transform.rotation.eulerAngles.y;
    }

}
