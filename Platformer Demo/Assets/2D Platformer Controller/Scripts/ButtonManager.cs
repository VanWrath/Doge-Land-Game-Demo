using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour {

	public AudioClip highlightSound;
	public AudioClip clickSound;
	public AudioClip mainMenuMusic;
	[HideInInspector] public bool inMainMenu = true;

	private ShowPanels showPanels;

	// Use this for initialization
	void Awake () {
		showPanels = GetComponent <ShowPanels> ();
	}

	public void StartButtonClicked()
	{
		SoundManager.instance.PlaySingle (clickSound);
		SceneManager.LoadScene (1);
		showPanels.HideMenuPanel ();
		showPanels.HideBG ();
		inMainMenu = false;
	}

	public void Retry(){
		SoundManager.instance.PlaySingle (clickSound);
		SceneManager.LoadScene (1);
		showPanels.HideGameOverPanel ();
	}
	public void ControlsButtonClicked()
	{
		SoundManager.instance.PlaySingle (clickSound);
		if (inMainMenu == true) { 
			showPanels.HideMenuPanel ();
			showPanels.ShowControlsPanel ();
		} else {
			showPanels.HidePauseMenu ();
			showPanels.ShowControlsPanel ();
		}
	}

	public void BackButtonClicked()
	{
		SoundManager.instance.PlaySingle (clickSound);
		if (inMainMenu == true) {
			showPanels.HideControlsPanel ();
			showPanels.ShowMenuPanel ();
		}
		else //if in game, so show pause menu 
		{
			showPanels.HideControlsPanel ();
			showPanels.ShowPausePanel ();
		}
	}

	public void MainButtonClicked()
	{
		SoundManager.instance.PlaySingle (clickSound);
		//load main menu
		SceneManager.LoadScene (0);
		showPanels.HideFinishPanel ();
		showPanels.HideGameOverPanel ();
		showPanels.HidePauseMenu ();
		showPanels.ShowBG ();
		showPanels.ShowMenuPanel ();
		inMainMenu = true;
		SoundManager.instance.PlayMusic (mainMenuMusic);
		GameManager.instance.ResumeTime ();
	}

	public void PlayHighlightSound()
	{
		SoundManager.instance.PlayerFX (highlightSound);
	}
}
