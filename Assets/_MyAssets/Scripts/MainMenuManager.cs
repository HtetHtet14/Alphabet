using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;


public class MainMenuManager : MonoBehaviour
{
	public GameObject startPanel;
	public GameObject menuPanel;
	public GameObject infoPanel; 
	public GameObject loadingPanel;
	public GameObject exitPanel;
	public Slider loading;
	public GameObject blackBG;
	public GAui activatePanel;
	public Text versionText;
	public GameObject settingPanel; 

	public GameObject parentPanel;
	

	[Space]
	public GAui logoStart;
	public GAui butStart;

	public GameObject laodingPanel;

	public Image progressBar; 

	private AsyncOperation async = null;

	private GameObject curActivePanel; 


	private void Awake()
	{
		Application.targetFrameRate = 60;
	}

	// Use this for initialization
	void Start ()
	{
		blackBG.SetActive(false);

		versionText.text = "Version " + Application.version;

		// Enable the GUI animator ui panel
		infoPanel.SetActive(true);
		exitPanel.SetActive(true);
		activatePanel.gameObject.SetActive(true);

		settingPanel.transform.DOScale(0, 0);

		// Check to show start menu or main menu.
		if (PlayerPrefs.GetInt("START_MENU", 1) == 1)
		{
			startPanel.SetActive(true);

			menuPanel.SetActive(false);
			loadingPanel.SetActive(false);
			infoPanel.SetActive(false);

			StartCoroutine(StartAnimate());
		}
		else
		{
			ShowMainMenu();

			PlayerPrefs.SetInt("START_MENU", 1);
		}

		ZPlayerPrefs.Initialize("360Ed@R0cK", "AlphabetAR");
	}

	// On click toggle button 
	public void OnClickToggle (bool _isOn)
	{
		if(_isOn)
		{
			Debug.Log("Toggle is on");
			PlayerPrefs.SetInt("PARENT_GUIDE", 1);
		}
		else
		{
			Debug.Log("Toggle is off");
			PlayerPrefs.SetInt("PARENT_GUIDE", 0);

		}
	}


	// On Click Ok at parent panel
	public void OnClickParentOk ()
	{
		// StartMainMenu();

		parentPanel.GetComponent<GAui>().MoveOut();

		blackBG.SetActive(false);
	}

	// Show setting 
	public void OnClickSetting ()
	{
		settingPanel.transform.DOScale(1, 0.25f)
			.SetEase(Ease.OutBack);
	}

	// Show main menu 
	void StartMainMenu ()
	{
		

	}

	void ShowMainMenu()
	{
		startPanel.SetActive(false);
		menuPanel.SetActive(true);

		GAui[] gAuis = menuPanel.GetComponentsInChildren<GAui>();

		foreach (var item in gAuis)
		{
			item.MoveIn();
		}
	}

	IEnumerator StartAnimate ()
	{
		yield return new WaitForSeconds(0.1f);

		logoStart.MoveIn();
		butStart.MoveIn();

		if (PlayerPrefs.GetInt("PARENT_GUIDE", 0) == 0)
		{
			// Show parent guide.
			parentPanel.SetActive(true);
			parentPanel.GetComponent<GAui>().MoveIn();
			blackBG.SetActive(true);
		}
		else
		{
			parentPanel.SetActive(false);
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			if(curActivePanel == activatePanel.gameObject)
			{
				activatePanel.MoveOut();
				blackBG.SetActive(false);

				curActivePanel = menuPanel;
			}
			else
			{
				blackBG.SetActive(true);
				exitPanel.GetComponent<GAui>().MoveIn();

				curActivePanel = exitPanel;
			}
		}
	}
	

	//
	public void OnClickStart ()
	{
		ShowMainMenu();
	}

	public void OnClickInfo ()
	{
		infoPanel.SetActive(true);

		infoPanel.GetComponent<GAui>().MoveIn();

		blackBG.SetActive(true);
	}

	public void OnClickInfoBack ()
	{
		infoPanel.GetComponent<GAui>().MoveOut();

		blackBG.SetActive(false);
	}

	// On Click website
	public void OnClickWeb(string url)
	{
		Application.OpenURL(url);
	}

	public void OnClickAR ()
	{
		int purchase = ZPlayerPrefs.GetInt("ACTIVATED", 0);

		// Testing only 
		// TestDev
		// LoadingARScene();
		// return; 

		if (purchase == 0)
		{
			blackBG.SetActive(true);
			// SceneManager.LoadScene("QRScene");
			activatePanel.MoveIn();
			curActivePanel = activatePanel.gameObject;
		}
		else
		{
			LoadingARScene();
		}
	}

	// Start show the loading scene 
	void LoadingARScene ()
	{
		loadingPanel.SetActive(true);

		StartCoroutine(LoadALevel("ARScene"));
	}

	public void OnClickLP ()
	{
		SceneManager.LoadScene("LPMenu");
		// loadingPanel.SetActive(true);
	}

	// Info Windwos
	public void OnClickCredit ()
	{

	}

	public void OnClickMusic ()
	{

	}

	// Exit Window.
	public void OnClickYes ()
	{
		Application.Quit();
	}

	public void OnClickNo ()
	{
		blackBG.SetActive(false);
		exitPanel.GetComponent<GAui>().MoveOut();
	}

	//
	public void OnClickActivate ()
	{
		// loadingPanel.SetActive(true);
		// StartCoroutine(LoadALevel("QRScene"));

		SceneManager.LoadScene("QRScene"); 
	}

	//
	public void OnClickDemo ()
	{
		blackBG.SetActive(false);
		activatePanel.gameObject.SetActive(false);

		loadingPanel.SetActive(true);
		StartCoroutine(LoadALevel("DemoScene"));
	}


	public void OnClickBack ()
	{
		blackBG.SetActive(false);
		activatePanel.MoveOut();

		curActivePanel = menuPanel;
	}

	private IEnumerator LoadALevel(string levelName)
	{
		loadingPanel.gameObject.SetActive(true);

		async = SceneManager.LoadSceneAsync(levelName);

		Debug.Log(async.progress);

		while (!async.isDone)
		{
			loading.value = async.progress;

			yield return null;
		}
	}
	
	// Testing 

	public void OnClickTest ()
	{
		SceneManager.LoadScene("ARScene");
	}
}
