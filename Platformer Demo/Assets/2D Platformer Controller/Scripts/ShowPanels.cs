using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowPanels : MonoBehaviour {

	public static ShowPanels instance = null;

	public GameObject menuPanel;
	public GameObject pausePanel;
	public GameObject controlsPanel;
	public GameObject gameOverPanel;
	public GameObject finishPanel;
	public GameObject menuBG;

	void Awake()
	{
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
		//initial setup;
		//DontDestroyOnLoad(gameObject);
	}

	public void ShowPausePanel()
	{
		pausePanel.SetActive (true);
	}

	public void HidePauseMenu()
	{
		pausePanel.SetActive (false);
	}

	public void ShowControlsPanel()
	{
		controlsPanel.SetActive (true);
	}

	public void HideControlsPanel()
	{
		
		controlsPanel.SetActive (false);
	}

	public void ShowMenuPanel()
	{
		menuPanel.SetActive (true);
	}

	public void HideMenuPanel()
	{
		menuPanel.SetActive (false);
	}

	public void ShowGameOverPanel()
	{
		gameOverPanel.SetActive (true);
	}

	public void HideGameOverPanel()
	{
		gameOverPanel.SetActive (false);
	}

	public void ShowFinishPanel()
	{
		finishPanel.SetActive (true);
	}

	public void HideFinishPanel()
	{
		finishPanel.SetActive (false);
	}

	public void ShowBG()
	{
		menuBG.SetActive (true);
	}

	public void  HideBG()
	{
		menuBG.SetActive (false);
	}
}
