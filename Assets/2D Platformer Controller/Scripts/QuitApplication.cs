using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class QuitApplication : MonoBehaviour {

	public void Quit()
	{
	#if UNITY_STANDALONE
		//Quit the application
		Application.Quit ();
	#endif

	#if UNITY_EDITOR
		//stop playing scene
		UnityEditor.EditorApplication.isPlaying = false;
	#endif
	}
}
