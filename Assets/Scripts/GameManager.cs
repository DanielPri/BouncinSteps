using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject Structure;
    public GameObject PauseMenu;
    public GameObject EndingMenu;
    public GameObject HUD;
    public Ball ball;
    public float rotationSpeed = 1;
    public float TapSpeed = 0.2f;
    public int level = 0;

    private float touchStart = 0;
    private float displacement;
    private float prevRotation;
    private TextMeshProUGUI EndingText;
    private TextMeshProUGUI HUDLevel;
    private LevelGenerator levelGenerator;
    private PlayerData data;

    private float timeTapBegan;

    void Start()
    {
        // initializing class members
        levelGenerator = GetComponent<LevelGenerator>();
        EndingText = EndingMenu.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        HUDLevel = HUD.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
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
    }

    /// <summary>
    /// Determine if victory or defeat occured based on event from ball collision
    /// </summary>
    /// <param name="isVictory"></param>
    private void HandleEnding(bool isVictory)
    {
        if (isVictory)
        {
            EndingText.text = "Victory!";
            EndingMenu.SetActive(true);
            level++;
            SaveSystem.SavePlayer(level);
        }
        else
        {
            EndingText.text = "Game Over!";
            EndingMenu.SetActive(true);
        }
        
    }

    /// <summary>
    ///  restarts the level
    /// </summary>
    public void RestartLevel()
    {
        // load itself 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Update is called once per frame
    void Update()
    {
        HandleTouch();
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
        if (Time.timeScale != 0)
        {
            Time.timeScale = 0;
            PauseMenu.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            PauseMenu.SetActive(false);
        }
    }
    void DragStart(Touch touch)
    {
        touchStart = touch.position.x;
    }
    void Dragging(Touch touch)
    {
        displacement = (touch.position.x - touchStart) * rotationSpeed;
        Structure.transform.eulerAngles = new Vector3(0, prevRotation - displacement, 0);
    }
    void DragRelease(Touch touch)
    {
        //remember the rotation of the structure for next drag
        prevRotation = Structure.transform.rotation.eulerAngles.y;
    }

}
