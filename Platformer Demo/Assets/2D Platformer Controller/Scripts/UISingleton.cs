using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISingleton : MonoBehaviour {

	public static UISingleton instance = null;

	// Use this for initialization
	void Awake () {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
		//initial setup;
		DontDestroyOnLoad(gameObject);
	}
}
