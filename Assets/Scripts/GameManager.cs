using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject Structure;
    public GameObject Canvas;
    public float rotationSpeed = 1;
    public float TapSpeed = 0.2f;

    private float touchStart = 0;
    private float displacement;
    private float prevRotation;

    private float timeTapBegan;

    // Start is called before the first frame update
    void Start()
    {
        Canvas.SetActive(false);
        prevRotation = 0f;
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
                    if(Time.timeScale != 0)
                    {
                        Time.timeScale = 0;
                        Canvas.SetActive(true);
                    }
                    else
                    {
                        Time.timeScale = 1;
                    }
                }
            }
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

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Canvas.SetActive(false);
    }
}
