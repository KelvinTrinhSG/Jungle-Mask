using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameController_mask : MonoBehaviour
{
    public static GameController_mask singleton;
    private GameObject player, gui;
    private Text scoreText;
    [SerializeField]
    private AudioClip scoreMaxSound;
    private AudioSource sound;

    private float score;
    private int maxScore;
    [SerializeField] private bool isSoundStop;
    [SerializeField] private bool pause,isGame;
    public float speed_up;
    public GameObject panel_loading;

    public GameSpeedFetcher speedFetcher; // Gán trong Unity Inspector

    private void Awake()
    {
        if(singleton==null)
        {
           singleton=this;
        }
        else{
           Destroy(gameObject);
        }
        maxScore =PlayerPrefs.GetInt("score_mask",0);
        
    }

    void Start()
    {
        sound = GameObject.Find("AudioManager").GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player");
        gui = GameObject.Find("GUI");
        scoreText = gui.transform.GetChild(1).GetChild(0).GetComponent<Text>();
        sound.Play();
        Debug.Log(GameSettings.Instance.adjustSpeed);
        speed_up += GameSettings.Instance.adjustSpeed;
        Debug.Log("speed_up" + speed_up);
    }

    void Update()
    {
        RestartMenuPopup();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseButton();
        }
        else if(Input.GetKeyDown(KeyCode.Return))
        {
            if (gui.transform.GetChild(0).GetChild(5).gameObject.activeSelf)
            {
                ResumeButton();
            }
            else
            {
                RestartButton();
            }
        }

        if(!pause)
        {
            speed_up +=0.00009f;
        }
    }

    public bool hasFetchedSpeed = false;

    private void RestartMenuPopup()
    {
        if(player == null)
        {
            //if (player == null && !hasFetchedSpeed)
            //{
            //    // Điều chỉnh độ khó của game ở đây
            //    speedFetcher.FetchSpeedFromScore((int)score);
            //    hasFetchedSpeed = true; // Đánh dấu đã gọi rồi
            //}

            PlayerPrefs.SetInt("score", (int)score);

            gui.transform.GetChild(2).transform.gameObject.SetActive(false);
            gui.transform.GetChild(0).transform.gameObject.SetActive(true);
            gui.transform.GetChild(0).GetChild(5).gameObject.SetActive(false);
            gui.transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<Text>().text = scoreText.text;

            if(isGame)
              scoreText.text=Convert.ToString(Mathf.Round(score));
            else
              scoreText.text=maxScore+""; 

            if (isSoundStop)
            {
                sound.Stop();
            }
        }
        else
        {
            ScoreIncrement();
        }
    }

    public void RestartButton()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1;
    }

    public void ExitButton()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitButton()
    {
       panel_loading.SetActive(true); 
       SceneManager.LoadScene(0);
    }

    public void PauseButton()
    {
        pause=true;
        gui.transform.GetChild(2).transform.gameObject.SetActive(false);
        gui.transform.GetChild(0).transform.gameObject.SetActive(true);
        gui.transform.GetChild(0).transform.gameObject.GetComponent<Animator>().enabled = false;
        gui.transform.GetChild(0).transform.position = Vector3.zero;
        Time.timeScale = 0;
    }

    public void ResumeButton()
    {
        pause=false;
        gui.transform.GetChild(0).transform.gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    private void ScoreIncrement()
    {
        score += 1f * Time.deltaTime;
        scoreText.text = Convert.ToString(Mathf.Round(score));

        if(score >= maxScore)
        {
            //sound.PlayOneShot(scoreMaxSound);
            maxScore = Convert.ToInt32(score);
            PlayerPrefs.SetInt("score_mask",maxScore);
        }
    }
}
