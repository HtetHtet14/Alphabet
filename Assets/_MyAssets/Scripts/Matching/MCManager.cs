using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MCManager : MonoBehaviour
{
	private MCManager mCManager;

	public static MCManager Instance = null;

	public TextAsset ansTextAsset;
	public Text trophyText;
	public Camera mainCam;
	public GameObject dragAnsPrefab;

	public Sprite starNormal;
	public Sprite starSpecial;

	public GameObject banMusic;

	public GameObject celePart;
	public AudioClip celeSound;

	public Sprite[] sprites;
	public GameObject[] qBoxs;
	public GameObject[] ansBoxs;

	public bool isTutorialMode;
	public GAui welcomeTutorialPanel;

	private string[] ansArray;
	private int[] arr = new int[60];
	private int qIndex = 0;

	public int starCount { get; set; }
	private int trophyCount;

	private void Awake()
	{
		Instance = this;
	}

	// Use this for initialization
	void Start()
	{
		// PlayerPrefs.SetInt("TUTORIAL_MODE", 1);
		TutorialMode_isOn();

		trophyCount = PlayerPrefs.GetInt("TrophyCount", 0);
		trophyText.text = trophyCount.ToString();

		ansArray = ansTextAsset.ToString().Split('\n');

		// initialize to arr array 
		for (int i = 0; i < 60; i++)
		{
			arr[i] = i;
		}

		// Shuffle array 
		for (int i = 0; i < arr.Length; i++)
		{
			int rand = Random.Range(0, arr.Length);

			int temp = arr[rand];
			arr[rand] = arr[i];
			arr[i] = temp;
		}

		// Check Music
		if (MusicManager.Instance != null)
		{
			if (MusicManager.Instance.audioSource.isPlaying)
			{
				banMusic.SetActive(false);
				// MusicManager.Instance.StopMusic();
			}
			else
			{
				banMusic.SetActive(true);
				// MusicManager.Instance.PlayMusic();
			}
		}

		StartQuestion();
	}

	
	IEnumerator PrepareQuestion()
	{
		yield return new WaitForSeconds(2);

		StartQuestion();
	}

	//
	public void StartQuestion()
	{
		string name = "";
		Sprite spr;

		if (isTutorialMode)
		{
			int[] tutQIndex = new int[] { 1, 2, 3 };

			// int index = 0;

			for (int i = 0; i < 3; i++)
			{
				name = sprites[tutQIndex[qIndex]].name;

				spr = sprites[tutQIndex[qIndex]];

				qBoxs[i].GetComponent<MCQBox>().SetQuestion(spr, name);
				qBoxs[i].GetComponent<GAui>().MoveIn();

				// Using some expression to avoid -1 from i - 1
				ansBoxs[i == 0 ? 2 : i - 1].GetComponent<MCAnsButton>().SetAnswer(name);
				ansBoxs[i == 0 ? 2 : i - 1].GetComponent<GAui>().MoveIn();

				qIndex++;
			}
		}
		else
		{
			// Shuffle the answer box
			for (int j = 0; j < ansBoxs.Length; j++)
			{
				int rand = Random.Range(0, ansBoxs.Length);

				var temp = ansBoxs[rand];
				ansBoxs[rand] = ansBoxs[j];
				ansBoxs[j] = temp;
			}

			// Set questions and answer 
			for (int i = 0; i < 3; i++)
			{
				if (qIndex >= sprites.Length)
					qIndex = 0;

				name = sprites[arr[qIndex]].name;

				spr = sprites[arr[qIndex]];

				qBoxs[i].GetComponent<MCQBox>().SetQuestion(spr, name);
				qBoxs[i].GetComponent<GAui>().MoveIn();

				// ansBoxs[i].SetActive(true);
				ansBoxs[i].GetComponent<MCAnsButton>().SetAnswer(name);
				ansBoxs[i].GetComponent<GAui>().MoveIn();

				qIndex++;
			}
		}
	}

	public void IncStar()
	{
		starCount++;

		if (starCount == 3)
		{
			isTutorialMode = false;
			MCTutorial.Instance.TutorialEnd();

			// trophyCount++;
			// PlayerPrefs.SetInt("TrophyCount", trophyCount);
			// trophyText.text = trophyCount.ToString();

			starCount = 0;

			StartCoroutine(CloseQuestions());
		}
	}

	IEnumerator CloseQuestions()
	{
		// Show the particle 
		celePart.SetActive(false);
		celePart.SetActive(true);

		// Play sound
		GetComponent<AudioSource>().clip = celeSound;
		GetComponent<AudioSource>().Play();


		yield return new WaitForSeconds(1.0f);

		for (int i = 0; i < 3; i++)
		{
			// Enable the buttons and set it's scale to zero.
			ansBoxs[i].SetActive(true);
			ansBoxs[i].transform.localScale = Vector3.zero;

			// Clean the box
			qBoxs[i].GetComponent<GAui>().MoveOut();
			qBoxs[i].GetComponent<MCQBox>().ResetQBox();
		}

		StartCoroutine(PrepareQuestion());
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			celePart.SetActive(false);
			celePart.SetActive(true);
		}
	}

	// === Button Region === //
	// On Close Welcome button 
	public void OnClickWelcomeTutorialPanel()
	{
		MCTutorial.Instance.finger.SetActive(true);

		welcomeTutorialPanel.MoveOut();

		welcomeTutorialPanel.gameObject.transform.parent.GetComponent<Image>().enabled = false;
	}

	// Back to home 
	public void OnClickBack()
	{
		SceneManager.LoadScene("LPMenu");
	}

	public void OnClickMusic()
	{
		if (MusicManager.Instance != null)
		{
			if (MusicManager.Instance.audioSource.isPlaying)
			{
				banMusic.SetActive(true);
				MusicManager.Instance.StopMusic();
			}
			else
			{
				banMusic.SetActive(false);
				MusicManager.Instance.PlayMusic();
			}
		}
	}

	public void  TutorialMode_isOn()
	{

		if (PlayerPrefs.GetInt("TUTORIAL_MODE", 1) == 1)
		{
			isTutorialMode = true;

			welcomeTutorialPanel.MoveIn();

			welcomeTutorialPanel.gameObject.transform.parent.GetComponent<Image>().enabled = true;

			
		}
		else
		{
			isTutorialMode = false;

			welcomeTutorialPanel.gameObject.SetActive(false);

			MCTutorial.Instance.finger.SetActive(false);

			
		}

		
	}
}