using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class LoadOnClick : MonoBehaviour {

	//public GameObject loadingImage;

	//loads first level
	public void LoadScene(int level)
	{
		//loadingImage.SetActive(true);
		SceneManager.LoadScene (level);
	}
}
