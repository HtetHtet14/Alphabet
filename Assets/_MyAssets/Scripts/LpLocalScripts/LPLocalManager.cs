using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LPLocalManager : MonoBehaviour {
	public static LPLocalManager instance = null;

	[Space]
	[Header("Normal iPhone")]
	public GameObject iphoneCanvas;
	[Header("iPhone X")]
	public GameObject iphoneXCanvas;
	public GameObject bannImgeX;
	public GAui continueWindowX;
	public GameObject blackX;
	public GAui readyImgX;
	public GAui trophyImgX;
	public Text trophyCountX;
	
	public GameObject[] ansButtonsX;
	public GameObject[] starX;
	public GameObject[] starParticleX;


	[Space]
	public TextAsset sentenceFile;
	public GameObject objStandPoint;
	public GameObject buttonPrefab;
	public GameObject contentTransform;
	public AudioClip createSound; 
	public AudioClip correctSound;
	public AudioClip wrongSound;
	public AudioClip celebrateSound;
	public AudioClip answerMoveInSound; 
	public AudioClip trophyMoveOut;
	public AudioSource starAudioSource;
	public AudioSource celebrateAudioSource; 
	public GAui trophiImage;
	public GameObject particle;
	public Text trophyCountText;
	public GameObject bannImage;
	public GAui continueWindow;
	public GameObject black; 

	public GameObject[] ansButtons;
	public GameObject[] prefabs;

	public GameObject[] stars;
	public GameObject[] starParticles; 
	public Sprite starWonSprite;
	public Sprite starLoseSprite;
	public GAui readyImg;
	public GameObject qParticle;
	
	// [Header("For Nga Pyinn")]
	// public Sprite[] sprites; 

	public string[] rawSentences;
	private List<string> sentences = new List<string>();

	private int curAnswerId;
	private string rightAnswer;
	private GameObject curObject;

	private int starCount; 

	private int trophyCount;
	private bool showingAns = false;
	private List<GameObject> qButtonList = new List<GameObject>();

	// Random Questions Indexs 
	private int qIndex; 
	private int[] arr = new int[60];

	// Awake 
	private void Awake()
	{
		instance = this; 
	}

	// Use this for initialization
	void Start ()
	{
#if UNITY_IPHONE

		if (UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPhoneX)
		{
			iphoneXCanvas.SetActive(true);
			iphoneCanvas.SetActive(false);
			// GameObject ratiFitter = iphoneXCanvas.transform.Find("RatioFitter").gameObject;

			ansButtons = ansButtonsX;
			stars = starX;
			starParticles = starParticleX;
			bannImage = bannImgeX;
			continueWindow = continueWindowX;
			black = blackX;
			readyImg = readyImgX;
			trophiImage = trophyImgX;
			trophyCountText = trophyCountX;

		}
		else
		{
			iphoneCanvas.SetActive(true);
			iphoneXCanvas.SetActive(false);
			// GameObject ratiFitter = iphoneCanvas.transform.Find("RatioFitter").gameObject;
		}
#else

		
		iphoneCanvas.SetActive(true);
		iphoneXCanvas.SetActive(false);

		// GameObject ratiFitter = iphoneXCanvas.transform.Find("RatioFitter").gameObject;


#endif	


		trophyCountText.text = PlayerPrefs.GetInt("LPL_TrophyCount", 0).ToString();
		trophyCount = PlayerPrefs.GetInt("LPL_TrophyCount", 0);

		black.SetActive(false);

		qIndex = 0;		

		// Set value 
		for (int i = 0; i < arr.Length; i++)
		{
			arr[i] = i; 
		}

		int temp;
		
		// Shuffle the arrays 
		for (int i = 0;  i < arr.Length; i++)
		{
			int ran = Random.Range(0, arr.Length);

			temp = arr[ran];
			arr[ran] = arr[i];
			arr[i] = temp;
		}
		
		// Parse the sentence 
		string str = sentenceFile.text;
		rawSentences = str.Split('\n');

		// Referesh the sentences list. 
		RefershScentencesList();

		// Stop music 
		GameObject o = GameObject.Find("MusicPlayer");
		if (o != null)
		{
			if (!o.GetComponent<AudioSource>().isPlaying)
			{
				// o.GetComponent<AudioSource>().Play();
				bannImage.SetActive(true);
			}
			else
			{
				bannImage.SetActive(false);
			}
		}

		// If all done, Start asking questions
		StartCoroutine(ShowQuestions());
	}

	IEnumerator ShowQuestions()
	{
		yield return new WaitForSeconds(0.2f);

		readyImg.MoveIn();

		yield return new WaitForSeconds(1);

		readyImg.MoveOut();

		yield return new WaitForSeconds(1);

		OnClickQuestion(arr[qIndex]);
	}

	// Referesh the scentences list
	void RefershScentencesList ()
	{
		sentences.Clear();

		foreach (string sentence in rawSentences)
		{
			sentences.Add(sentence.Remove(sentence.Length - 1));
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	// On click question 
	public void OnClickQuestion (int _index)
	{
		qIndex += 1; 

		RefershScentencesList();

		curAnswerId = _index;

		int ansid = Random.Range(0, 3);

		ansButtons[ansid].GetComponent<AnsButScript>().OnSetAnswerString(sentences[curAnswerId]);
		rightAnswer = sentences[curAnswerId];
		sentences.RemoveAt(curAnswerId);

		for (int i = 0; i < 3; i++)
		{
			if (i == ansid)
				continue;

			int ranAnsId = Random.Range(0, sentences.Count - 1);

			ansButtons[i].GetComponent<AnsButScript>().OnSetAnswerString(sentences[ranAnsId]);
			sentences.RemoveAt(ranAnsId);
		}

		// Instantiate object. 
		if (curObject != null)
		{
			Destroy(curObject);
		}

		curObject = Instantiate(prefabs[_index], objStandPoint.transform.position, Quaternion.identity);
		curObject.GetComponent<ObjectScript>().curState = ObjectScript.State.ON_LP;
		// curObject.GetComponent<ObjectScript>().PlayAnimation();

		if (curObject.GetComponent<Animation>() != null)
		{
			curObject.GetComponent<Animation>().Play();
		}

		// Show particle
		qParticle.SetActive(true);

		starAudioSource.clip = createSound;
		starAudioSource.Play();

		// Move In answer 
		if (showingAns)
		{
			showingAns = false; 

			foreach (GameObject obj in ansButtons)
			{
				obj.GetComponent<GAui>().MoveOut();
			}

			StartCoroutine(MoveInAns());
		}
		else
		{
			showingAns = true; 

			foreach (GameObject obj in ansButtons)
			{
				obj.GetComponent<GAui>().MoveIn();				
			}
		}
	}

	// Start moving the answer list 
	IEnumerator MoveInAns ()
	{
		yield return new WaitForSeconds(1);

		showingAns = true; 

		foreach (GameObject obj in ansButtons)
		{
			obj.GetComponent<Button>().interactable = true; 

			obj.GetComponent<GAui>().MoveIn();
		}
	}

	// On click answer
	public void OnClickAnswer (Text ansText)
	{
		qParticle.SetActive(false);
		ansText.text = ansText.text.Remove(ansText.text.Length - 1);

		if (ansText.text == rightAnswer)
		{
			Debug.Log("You Correct.");
			starAudioSource.clip = correctSound;
			starAudioSource.Play();

			starCount++;
			stars[starCount - 1].GetComponent<Image>().sprite = starWonSprite;
			starParticles[starCount - 1].SetActive(true);

			// Set right 1. 1 is true answer. 
			//qButtonList[curAnswerId].GetComponent<ScrollButtonScript>().SetRight(1);

			if (starCount == 3)
			{
				StartCoroutine(ShowTrophy());

				Destroy(curObject);

				foreach (GameObject obj in ansButtons)
				{
					obj.GetComponent<GAui>().MoveOut();
				}

				return; 
			}
		}
		else
		{
			Debug.Log("Your answer is wrond.");
			starAudioSource.clip = wrongSound;
			starAudioSource.Play();

			ResetScore();
		}

		// Move out buttons
		foreach (GameObject obj in ansButtons)
		{
			obj.GetComponent<Button>().interactable = false; 

			obj.GetComponent<GAui>().MoveOut();
		}

		// Process to next question
		StartCoroutine(NextQuestion());
	}

	// Start next questions.
	IEnumerator NextQuestion()
	{
		yield return new WaitForSeconds(1.5f);

		// OnClickQuestion(curAnswerId + 1);

		OnClickQuestion(arr[qIndex]);
	}

	// Reset the score when the player get trophy
	void ResetScore ()
	{
		starCount = 0;
		for (int i = 0; i < 3; i++)
		{
			stars[i].GetComponent<Image>().sprite = starLoseSprite;
			starParticles[i].SetActive(false);
		}
	}

	// OnClick reset button
	public void OnReset ()
	{
		if (curObject == null)
			return; 

		//curObject.GetComponent<ObjectScript>().ResetRot();
	}

	// OnClick delete button (Empty)
	public void OnDelete ()
	{

	}

	// OnClick Home, go to mainmenu
	public void OnHome()
	{
		SceneManager.LoadScene("LPMenu");
	}

	// On click music on / off 
	public void OnClickMusic()
	{
		if (MusicManager.Instance != null)
		{
			if (MusicManager.Instance.audioSource.isPlaying)
			{
				bannImgeX.SetActive(true);
				bannImage.SetActive(true);
				MusicManager.Instance.StopMusic();
			}
			else
			{
				bannImage.SetActive(false);
				bannImgeX.SetActive(false);
				MusicManager.Instance.PlayMusic();
			}
		}
	}

	// When the user click continue 
	public void OnClickContinue ()
	{
		black.SetActive(false);

		continueWindow.MoveOut();

		StartCoroutine(GameContinue());
	}

	// When the user click cancel
	public void OnClickCancel ()
	{
		SceneManager.LoadScene("LPMenu");
	}

	// Continue from trophy 
	IEnumerator GameContinue ()
	{
		yield return new WaitForSeconds(2f);

		OnClickQuestion(arr[qIndex]);
	}

	// Show and add +1 trophy when the player get 3 stars.
	IEnumerator ShowTrophy()
	{
		yield return new WaitForSeconds(0.5f);

		trophiImage.gameObject.SetActive(true);
		particle.SetActive(false);
		particle.SetActive(true);
		trophiImage.MoveIn();

		celebrateAudioSource.clip = celebrateSound;
		celebrateAudioSource.Play();

		yield return new WaitForSeconds(3);

		celebrateAudioSource.clip = trophyMoveOut;
		celebrateAudioSource.Play();

		trophiImage.MoveOut();

		ResetScore();

		yield return new WaitForSeconds(1f);

		// Increase trophy count 
		trophyCount++;

		PlayerPrefs.SetInt("LPL_TrophyCount", trophyCount);

		trophyCountText.text = trophyCount.ToString();

		yield return new WaitForSeconds(0.5f);

		// Ask the user to start next round. 
		//OnClickQuestion(arr[qIndex]);
		black.SetActive(true);
		continueWindow.MoveIn();
	}
}
