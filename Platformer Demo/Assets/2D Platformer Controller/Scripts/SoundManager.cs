using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
	
	public AudioSource efxSource;
	public AudioSource playerfxSource;
	public AudioSource musicSource;
	public static SoundManager instance = null; //singleton

	// Use this for initialization
	void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else if(instance != this)
		{
			Destroy(gameObject);
		}
		DontDestroyOnLoad(gameObject);
	}

	public void PlaySingle(AudioClip clip)
	{
		efxSource.clip = clip;
		efxSource.Play();
	}

	public void PlayerFX(AudioClip clip)
	{
		playerfxSource.clip = clip;
		playerfxSource.Play ();
	}

	public void PlayMusic(AudioClip clip)
	{
		musicSource.Stop ();
		musicSource.clip = clip;
		musicSource.Play ();
	}
}
