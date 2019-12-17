using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Vuforia;
using System.Linq;
using DG.Tweening;

using Image = UnityEngine.UI.Image;

public class Controller : MonoBehaviour{

	public static Controller Instance = null;

	public GameObject demoInfo;	// text panel for demoscene.

	public GameObject particle;
	public GameObject arCamera;
	public GameObject mainUI;
	public GameObject previewPanel;
	public RawImage previewImg;

	public float soundVolume;
	public GameObject settingPanel; 

	public GameObject[] btnSounds;	// To hide sound buttons.
	public List<GameObject> languageButtonList = new List<GameObject>();
	public List<GameObject> curSelectedLanguageButtonList = new List<GameObject>();

	public Text title;
	public Text angleText; 

	public GameObject startPos; 
	public GameObject endPos;
	public AudioClip enterSound;
	public GAui btnSnd;

	public TextAsset spellingJson;

	public GameObject spellingArea;

	public GameObject clickParticle;

	public GameObject curShowingObj;

	public SpellingData spellingData;

	private bool isAngle = true;
	private AudioSource audioSource;
	private Texture2D ss;

	private Vector3 endStartPos;

	public Spelling curObjSpellingData; 

	// 
	private void Awake()
	{
		Instance = this; 
	}

	// Use this for initialization
	void Start ()
	{
		Application.targetFrameRate = 60;

		particle.SetActive(false);

		previewPanel.SetActive(false);

		audioSource = GetComponent<AudioSource>();

		endStartPos = endPos.transform.localPosition;

		soundVolume = PlayerPrefs.GetFloat("SOUND_VOLUME");

		UpdateSoundVolume(soundVolume);

		if(MusicManager.Instance != null)
		{
			MusicManager.Instance.StopMusic();
		}

		// Disable all sond button 
		foreach (var item in btnSounds)
		{
			item.SetActive(false);
		}

		settingPanel.transform.DOScale(0, 0);

		// Get the list of the spelling data.
		spellingData = JsonUtility.FromJson<SpellingData>(spellingJson.text);
	}

	public void UpdateSoundVolume (float _value)
	{
		soundVolume = _value;
		audioSource.volume = _value;

		PlayerPrefs.SetFloat("SOUND_VOLUME", _value);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			arCamera.transform.rotation =
				Quaternion.Euler(arCamera.transform.rotation.x, arCamera.transform.rotation.y,
				arCamera.transform.eulerAngles.z + 180);
		}

		/*
		if(Input.GetKeyDown(KeyCode.S))
		{
			OnClickSound();
		}

		if(Input.GetKeyDown(KeyCode.A))
		{
			ApplyLanguagesPos();
		}

		// For testing only		
		
		if (curShowingObj != null)
		{
			if (isAngle)
			{
				angleText.text = "Angle\nX : " + curShowingObj.transform.localEulerAngles.x.ToString("0") +
					"\nY : " + curShowingObj.transform.localEulerAngles.y.ToString("0") +
					"\nZ : " + curShowingObj.transform.localEulerAngles.z.ToString("0");
			}
			else
			{
				angleText.text = "Scale\nX : " + curShowingObj.transform.localScale.x.ToString("0.00") +
					"\nY : " + curShowingObj.transform.localScale.y.ToString("0.00") +
					"\nZ : " + curShowingObj.transform.localScale.z.ToString("0.00");
			}
		}
		*/
	}


	// Reset Camera when the object is lost
	public void ResetCamera ()
	{
		if (arCamera == null)
			return; 

		arCamera.transform.position = Vector3.zero;
		arCamera.transform.rotation = Quaternion.identity;
	}

	// Call when found the new target.
	public void FoundNewTarget (GameObject newObj)
	{
		audioSource.volume = soundVolume;
		audioSource.clip = enterSound;
		audioSource.Play();

		endPos.transform.localPosition = endStartPos;

		Scene activeScene = SceneManager.GetActiveScene();

		if (activeScene.name == "DemoScene")
		{
			if (demoInfo != null)
			{
				demoInfo.SetActive(false);
			}
		}

		// Enable the title text
		title.gameObject.SetActive(true);

		// Enable the sound buttons 
		foreach (var item in btnSounds)
		{
			item.SetActive(true);
		}

		if (curShowingObj == null)
		{
			curShowingObj = newObj;
			title.text = curShowingObj.name.ToUpper();
			return; 
		}
		else
		{
			if (curShowingObj != newObj)
			{
				curShowingObj.GetComponent<ObjectScript>().OnDisableObject();

				// Stop playing current object's sound 
				curShowingObj.GetComponent<AudioSource>().Stop();

				curShowingObj = newObj;				
			}
			else
			{
				// On found target. and placing the object on target.
				curShowingObj.GetComponent<ObjectScript>().OnFoundTarget();
			}

			// Set the title 
			title.text = curShowingObj.name.ToUpper();
		}
	}


	// Apply the position and data for each sound data 
	void ApplyLanguagesPos()
	{
		int buttonCount = SettingManager.Instance.curSelectedList.Count;

		Debug.Log(Mathf.Cos(Mathf.Deg2Rad * 45));
		Debug.Log(Mathf.Sin(Mathf.Deg2Rad * 45));

		var angle = 90;

		if (buttonCount > 1)
		{
			angle = 90 / (buttonCount < 5 ? (buttonCount - 1) : (buttonCount - 2));
		}

		// var angle = 90 / (buttonCount - 2);

		curSelectedLanguageButtonList.Clear();

		foreach (var item in SettingManager.Instance.curSelectedList)
		{
			var btn = languageButtonList.Find(x => x.name == item.name);

			btn.GetComponent<Image>().DOFade(0, 0);

			curSelectedLanguageButtonList.Add(btn);
		}

		for (int i = 0; i < buttonCount; i++)
		{
			var magnitude = 125;
			var pos = Vector2.zero;

			if (i == 0)
			{
				pos = new Vector2(Mathf.Cos(Mathf.Deg2Rad * 45) * magnitude,
					Mathf.Sin(Mathf.Deg2Rad * 45) * magnitude);
			}
			else
			{
				magnitude = 225;

				var adjAngle = angle * (i - 1);

				if (buttonCount < 5)
				{

					pos = new Vector2(Mathf.Cos(Mathf.Deg2Rad * (adjAngle + angle / 2)) * magnitude,
						Mathf.Sin(Mathf.Deg2Rad * (adjAngle + angle / 2)) * magnitude);
				}
				else if (buttonCount == 2)
				{
					pos = new Vector2(Mathf.Cos(Mathf.Deg2Rad * 45) * magnitude,
						Mathf.Sin(Mathf.Deg2Rad * 45) * magnitude);
				}
				else
				{
					pos = new Vector2(Mathf.Cos(Mathf.Deg2Rad * adjAngle) * magnitude,
						Mathf.Sin(Mathf.Deg2Rad * adjAngle) * magnitude);
				}
			}

			curSelectedLanguageButtonList[i].transform.DOLocalMove(new Vector3(pos.x, pos.y, 0), 0.25f)
				.SetEase(Ease.OutBack);
			curSelectedLanguageButtonList[i].GetComponent<Image>().DOFade(1, 0.25f);
		}
	}


	#region Button Region

	private bool showSound = false;

	public void OnClickSound ()
	{
		spellingArea.SetActive(false);
				
		if (!showSound)
		{
			showSound = true;

			ApplyLanguagesPos();
		}
		else
		{
			showSound = false;

			foreach (var item in curSelectedLanguageButtonList)
			{
				item.transform.DOLocalMove(Vector3.zero, 0.25f)
					.SetEase(Ease.InBack);
				item.GetComponent<Image>().DOFade(0, 0.25f);
			}
		}
		
	}


	/* DEL Camera Direction doesn't support from Vuforia 1.7
	private bool cameraMode = false;

	public void OnCameraChangeMode()
	{
		CameraDevice.CameraDirection currentDir = CameraDevice.Instance.GetCameraDirection();

		cameraMode = !cameraMode;

		var arCam = GameObject.Find("ARCamera");

		arCam.GetComponent<CameraFocusController>().StartAfterVuforia();

		if (cameraMode)
		{
			RestartCamera(CameraDevice.CameraDirection.CAMERA_FRONT);
			Debug.Log("Back Camera");
		}
		else
		{
			RestartCamera(CameraDevice.CameraDirection.CAMERA_BACK);
			Debug.Log("Front Camera");
		}
	}

	private void RestartCamera(CameraDevice.CameraDirection newDir)
	{
		CameraDevice.Instance.Stop();
		CameraDevice.Instance.Deinit();
		CameraDevice.Instance.Init(newDir);
		CameraDevice.Instance.Start();	
	}
	*/
	
	// On Click Pronuncation 
	public void OnClickPronun (string code)	// Code is mean country internet code .jp, .mm, .kr etc....
	{
		if (curShowingObj == null)
			return;

		// Play the pronuncation
		curShowingObj.GetComponent<ObjectScript>().PlayAudio(code);

		// // Don't show the spelling for US English and UK English.
		if (code == "us" || code == "uk")
		{
			return;
		}		

		// Display spelling.
		spellingArea.SetActive(true);		

		Vector3 btnPos = Vector3.zero;

		// create the first char to upper case letter.
		string codeName = code.First().ToString().ToUpper() + code.Substring(1);

		btnPos = btnSnd.transform.Find(codeName).transform.localPosition;

		if (btnPos.x < 100 && btnPos.y < 100)
		{
			if (curSelectedLanguageButtonList.Count > 1)
			{
				btnPos.x = 160;
				btnPos.y = 160;
			}
		}
		
		spellingArea.transform.localPosition = new Vector3(btnPos.x + 40, btnPos.y + btnPos.y/5, btnPos.z);

		Text displayText = null;
		string letter = "";

		// StopCoroutine(TypeLetter(displayText, letter));
		StopAllCoroutines();

		switch (code)
		{
			case "us":
				displayText = spellingArea.transform.GetChild(0).GetComponent<Text>();
				letter = curObjSpellingData.English;
				break;

			case "uk":
				displayText = spellingArea.transform.GetChild(0).GetComponent<Text>();
				letter = curObjSpellingData.English;
				break;

			case "mm":
				displayText = spellingArea.transform.GetChild(0).GetComponent<Text>();
				letter = curObjSpellingData.Myanmar;
				break;

			case "jp":
				displayText = spellingArea.transform.GetChild(0).GetComponent<Text>();
				letter = curObjSpellingData.Japanese;
				break;

			case "id":
				displayText = spellingArea.transform.GetChild(0).GetComponent<Text>();
				letter = curObjSpellingData.Indonesian;
				break;

			case "cn":
				displayText = spellingArea.transform.GetChild(0).GetComponent<Text>();
				letter = curObjSpellingData.Chinese;
				break;

			case "ph":
				displayText = spellingArea.transform.GetChild(0).GetComponent<Text>();
				letter = curObjSpellingData.Philippines;
				break;

			case "kr":				
				displayText = spellingArea.transform.GetChild(0).GetComponent<Text>();
				letter = curObjSpellingData.Korean;
				break;

			case "vn":
				displayText = spellingArea.transform.GetChild(0).GetComponent<Text>();
				letter = curObjSpellingData.Vietnam;
				break; 

			case "th":
				break;

			default:
				break;
		}

		StartCoroutine(TypeLetter(displayText, letter));
	}

	// Display the text with typing 
	IEnumerator TypeLetter (Text _displayText, string _letter)
	{
		_displayText.text = "";

		yield return new WaitForSeconds(0.1f);

		for (int i = 0; i < _letter.Length; i++)
		{
			_displayText.text += _letter[i].ToString();

			yield return new WaitForSeconds(0.1f);
		}
	}

	// On Press Camera
	public void OnClickCamera()
	{
		StartCoroutine(TakeScreenshotAndSave());
	}

	//
	public void OnClickHome ()
	{
		PlayerPrefs.SetInt("START_MENU", 0);

		SceneManager.LoadScene("MainMenu");
	}

	// On Press Delete button 
	public void OnClickDelete ()
	{
		if (curShowingObj == null)
			return;

		curShowingObj.GetComponent<ObjectScript>().OnDelete();

		title.text = "";

		// Disable all sond button 
		foreach (var item in btnSounds)
		{
			item.SetActive(false);
		}
	}

	// Click Setting 
	public void OnClickSetting ()
	{
		settingPanel.transform.DOScale(1, 0.25f)
			.SetEase(Ease.OutBack);
		// settingPanel.GetComponent<Image>().DOFade(1, 0.25f);
	}

	public void OnCloseSetting ()
	{
		settingPanel.transform.DOScale(0, 0.25f)
			.SetEase(Ease.InBack);
	}

	// On Click Scale
	public void OnClickScale ()
	{
		if (isAngle)
			isAngle = false;
		else
			isAngle = true; 
	}

	//
	public void OnClickSavePhoto ()
	{
		previewPanel.SetActive(false);

		var time = DateTime.Now;

		string fileName = time.ToString("AR dd-MM-yy hhmmss") + ".png";

		// Save the screenshot to Gallery/Photos
		NativeGallery.SaveImageToGallery(ss, "AlphabetInter", fileName);
	}

	//
	public void OnClickCancel ()
	{
		previewPanel.SetActive(false);
	}

	// 
	public void OnClickDemoLink ()
	{
		Application.OpenURL ("https://www.360ed.org/alphabet-ar");
	}

	#endregion


	// Show the spelling foe each object when click the language button 


	private IEnumerator TakeScreenshotAndSave()
	{
		mainUI.SetActive(false);

		yield return new WaitForEndOfFrame();

		ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
		ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
		ss.Apply();

		previewImg.texture = ss;
		previewPanel.SetActive(true);

		mainUI.SetActive(true);
	}
}


[Serializable]
public class SpellingData
{
	public List<Spelling> spellingList;
}


[Serializable]
public class Spelling
{
	public string English;
	public string Indonesian;
	public string Philippines;
	public string Chinese;
	public string Korean;
	public string Japanese;
	public string Myanmar;
	public string Vietnam;
}
