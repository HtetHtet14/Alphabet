using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LPMenu : MonoBehaviour {

	public GAui butLP;
	public GAui butMC;
	public GAui butLPLocal;

	public GameObject banMusic; 

	public GameObject loadingPanel;
	public Slider loadingBar;
	public Image progressBar;

	private AsyncOperation async = null; 

	// Use this for initialization
	void Start ()
	{
		StartCoroutine(StartAnimate());

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
	}

	IEnumerator StartAnimate ()
	{
		yield return new WaitForSeconds(0.1f);

		butLP.MoveIn();
		butMC.MoveIn();
		butLPLocal.MoveIn();
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public void OnClickMC ()
	{
		// SceneManager.LoadScene("Matching"); 
		StartCoroutine(LoadALevel("Matching"));
	}

	public void OnClickLP ()
	{
		// SceneManager.LoadScene("LPScene");
		StartCoroutine(LoadALevel("LPScene"));
	}

	public void OnClickLPLocal ()
	{
		StartCoroutine(LoadALevel("LPLocalScene"));
	}

	public void OnClickHome ()
	{
		PlayerPrefs.SetInt("START_MENU", 0);

		SceneManager.LoadScene("MainMenu");
	}

	public void OnClickMusic ()
	{
		if (MusicManager.Instance != null)
		{
			if(MusicManager.Instance.audioSource.isPlaying)
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

	private IEnumerator LoadALevel(string levelName)
	{
		loadingPanel.gameObject.SetActive(true);

		async = SceneManager.LoadSceneAsync(levelName);

		Debug.Log(async.progress);

		while (!async.isDone)
		{
			loadingBar.value = async.progress;

			yield return null;
		}
	}
}
