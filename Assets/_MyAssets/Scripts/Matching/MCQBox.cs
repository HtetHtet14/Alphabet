using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MCQBox : MonoBehaviour {

	public Image charImg;
	public Image star;
	public Text ansText;
	public AudioSource audioSource;

	public string rightAns;

	private AudioSource correctSound; 

	// Use this for initialization
	void Start ()
	{
		ansText.enabled = false;

		correctSound = GetComponent<AudioSource>();
		correctSound.volume = PlayerPrefs.GetFloat("SOUND_VOLUME");
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	//
	public void SetQuestion (Sprite _spr, string ans)
	{
		charImg.sprite = _spr;
		charImg.SetNativeSize();

		rightAns = ans;

		// Make the first letter to Upper Case.
		string pathName = ans.First().ToString().ToUpper() + ans.Substring(1);

		audioSource.clip = Resources.Load("Pronunciation/ENUS/" + pathName) as AudioClip;
	}

	public void SetRightAnswer (bool right)
	{
		if(right)
		{
			ansText.text = rightAns;
			ansText.enabled = true;
			star.sprite = MCManager.Instance.starSpecial;
			star.transform.GetChild(0).gameObject.SetActive(true);

			//Disable collider
			GetComponent<Collider2D>().enabled = false;

			correctSound.Play();

			StartCoroutine(PlaySound());
		}
		else
		{
			// Wrong
		}
	}

	IEnumerator PlaySound ()
	{
		yield return new WaitForSeconds(0.3f);

		audioSource.Play();

		yield return new WaitForSeconds(0.5f);

		MCManager.Instance.IncStar();
	}

	public void ResetQBox()
	{
		ansText.enabled = false;
		star.sprite = MCManager.Instance.starNormal;
		star.transform.GetChild(0).gameObject.SetActive(false);

		GetComponent<Collider2D>().enabled = true;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		
	}
}
