using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicPlayer : MonoBehaviour 
{
	public static MusicPlayer instance = null;


	private void Awake()
	{
		DontDestroyOnLoad(this.gameObject);

		if (instance == null)
		{
			instance = this; 
		}
		else if (instance != this)
		{
			Destroy(gameObject);
		}
	}

	// Use this for initialization
	void Start ()
	{
		if (SceneManager.GetActiveScene().name == "Menu" || SceneManager.GetActiveScene().name == "MenuStart")
		{
			GetComponent<AudioSource>().Play();
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
