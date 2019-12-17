using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using DG.Tweening;
using System.Text.RegularExpressions;

public class SettingManager : MonoBehaviour
{
	public static SettingManager Instance;

	public Slider musicSlider;
	public Slider soundSlider;

	public int maxLanguage = 6;

	public GameObject infoText; 

	public List<GameObject> languageList = new List<GameObject>();

	public List<GameObject> curSelectedList = new List<GameObject>();

	private string curLanguages = "";

	private void Awake()
	{
		Instance = this; 
	}

	void Start()
    {
		infoText.SetActive(false);

		if (!PlayerPrefs.HasKey("CUR_LANGUAGES"))
		{
			curLanguages = "MmVnCnJp";
			PlayerPrefs.SetString("CUR_LANGUAGES", curLanguages);
		}
		else
		{
			curLanguages = PlayerPrefs.GetString("CUR_LANGUAGES");
		}

		var lan = Regex.Split(curLanguages, @"(?<!^)(?=[A-Z])");

		foreach (var item in lan)
		{
			var language = languageList.Find(x => x.name == item);
			language.GetComponent<Toggle>().isOn = true;
		}

		// Sound Slider 
		if(!PlayerPrefs.HasKey("SOUND_VOLUME"))
		{
			soundSlider.value = 0.5f;
		}
		else
		{
			soundSlider.value = PlayerPrefs.GetFloat("SOUND_VOLUME");
		}

		// Music Slider 
		if(!PlayerPrefs.HasKey("MUSIC_VOLUME"))
		{
			musicSlider.value = 0.5f;
		}
		else
		{
			musicSlider.value = PlayerPrefs.GetFloat("MUSIC_VOLUME");
		}
	}

    // Update is called once per frame
    void Update()
    {
        
    }

	public void OnClickLanguage (string _lanStr)
	{
		var btnMn = languageList.Find(x => x.name == _lanStr).GetComponent<Toggle>();

		if (btnMn.isOn)
		{
			curSelectedList.Add(languageList.Find(x => x.name == _lanStr));

			if(curSelectedList.Count > 5)
			{
				DisableRemainButtons();
			}
		}
		else
		{
			// curLanguages.Replace(_lanStr, "");

			curSelectedList.Remove(languageList.Find(x => x.name == _lanStr));

			if(curSelectedList.Count < 6)
			{
				EnableRemainButtons();
			}
		}

		Debug.Log("======== Toggle is called ========");

		curLanguages = "";

		foreach (var item in languageList)
		{
			if (item.GetComponent<Toggle>().isOn)
			{
				curLanguages += item.name;
			}
		}

		PlayerPrefs.SetString("CUR_LANGUAGES", curLanguages);
	}

	void DisableRemainButtons ()
	{
		infoText.SetActive(true);

		foreach (var item in languageList)
		{
			if(!item.GetComponent<Toggle>().isOn)
			{
				item.GetComponent<Toggle>().interactable = false; 
			}
		}
	}

	void EnableRemainButtons ()
	{
		infoText.SetActive(false);

		foreach (var item in languageList)
		{
			if(!item.GetComponent<Toggle>().isOn)
			{
				item.GetComponent<Toggle>().interactable = true; 
			}
		}
	}

	public void OnMusicValueChange (float _value)
	{
		if (SceneManager.GetActiveScene().name == "MainMenu")
		{
			MusicManager.Instance.audioSource.volume = _value;
		}

		PlayerPrefs.SetFloat("MUSIC_VOLUME", _value);
	}

	public void OnSoundValueChange (float _value)
	{
		if (SceneManager.GetActiveScene().name == "ARScene")
		{
			Controller.Instance.UpdateSoundVolume(_value);
		}

		PlayerPrefs.SetFloat("SOUND_VOLUME", _value);
	}

	public void OnClose ()
	{
		transform.DOScale(0, 0.25f)
			.SetEase(Ease.InBack);
	}
}
