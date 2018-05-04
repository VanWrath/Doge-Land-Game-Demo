using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour {

	//public GameObject gameCanvas;
	public static GameManager instance = null;
	public GameObject ui;
	//public bool isGameFinished;

	private bool isPaused;

	// Use this for initialization
	void Awake()
	{
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
		//initial setup;
		DontDestroyOnLoad(gameObject);

	}

	void InitGame()
	{
		isPaused = false;
		//isGameFinished = false;
	}

	public void GameOver(int score)
	{
		
		//isGameFinished = true;
		ShowPanels.instance.ShowGameOverPanel ();
		(ShowPanels.instance.gameOverPanel.GetComponentsInChildren <Text>())[1].text = "Score: " + score;
	}

	public void FinishLevel(int score)
	{
		//isGameFinished = true;
		ShowPanels.instance.ShowFinishPanel ();
		(ShowPanels.instance.finishPanel.GetComponentsInChildren <Text> ()) [1].text = "Final Score: " + score;
	}

	public void ResumeTime()
	{
		Time.timeScale = 1;
	}

	// Update is called once per frame
	void Update () {

		if(Input.GetButtonDown("Cancel"))
		{
			isPaused = !isPaused;
			if (isPaused == true) {
				ShowPanels.instance.ShowPausePanel ();
				Time.timeScale = 0;
			}
			else if (isPaused == false) {
				ShowPanels.instance.HideControlsPanel ();
				ShowPanels.instance.HidePauseMenu ();
				Time.timeScale = 1;
			}
		}
		/*if (Input.GetButtonDown ("Submit")) {
			if (isGameFinished == true) {
				//switch to main menu scene.
				SceneManager.LoadScene (0);
			}
		}*/
	}

}
