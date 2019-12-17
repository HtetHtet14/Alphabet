using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnsButScript : MonoBehaviour
{
	public int id; 
	public Text ansText;
	public AudioClip audioClip;

	private AudioSource audioSource;

	// Use this for initialization
	void Start ()
	{
		audioSource = LPLocalManager.instance.gameObject.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	// On click play audio.
	public void PlayAudio ()
	{
		audioSource.clip = audioClip;
		audioSource.Play();
	}

	// On set answer string 
	public void OnSetAnswerString (string str)
	{
		ansText.text = str + '.';

		audioClip = Resources.Load("LpLocalSentence/" + str) as AudioClip;
	}
}
