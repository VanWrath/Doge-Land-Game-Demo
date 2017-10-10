using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

	public GameObject finishZone;
	public GameObject Boss;
	public bool isBossDead;
	public AudioClip levelMusic;

	public static LevelManager instance = null;
	// Use this for initialization

	void Awake()
	{
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
		//initial setup;
		//DontDestroyOnLoad(gameObject);

		Boss.SetActive (false);
		isBossDead = false;
		SoundManager.instance.PlayMusic (levelMusic);
	}

	/*void Start()
	{
		SoundManager.instance.PlayMusic (levelMusic);
	}*/

	public void EnableFinishZone()
	{
		(finishZone.GetComponent <BoxCollider2D>()).enabled = true;
	}

	public void DisableFinishZone ()
	{
		(finishZone.GetComponent <BoxCollider2D>()).enabled = false;
	}

	public void SpawnBoss()
	{
		Boss.SetActive (true);
		//play boss music
	}

	public void DmgBoss()
	{
		Boss.GetComponent <Boss1>().TakeDmg ();
	}

	public void SetBossDead()
	{
		isBossDead = true;
	}
}
