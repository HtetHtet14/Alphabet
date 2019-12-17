using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {

	public static MusicManager Instance = null;

	public AudioSource audioSource;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(this.gameObject);
		}
		else if (Instance != this)
		{
			Destroy(gameObject);
		}
	}

	public void PlayMusic ()
	{
		audioSource.Play();
	}

	public void StopMusic()
	{
		audioSource.Pause();
	}

	// Use this for initialization
	void Start ()
	{
		audioSource = GetComponent<AudioSource>();

		// audioSource.volume = SettingManager.Instance.musicSlider.value;
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public void UpdateMusicVolume (float _value)
	{
		audioSource.volume = _value;
	}
}
