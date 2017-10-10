using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//Causes UI Objects not to be destryoed when loading new scene.
		DontDestroyOnLoad (this.gameObject);
	}
}
