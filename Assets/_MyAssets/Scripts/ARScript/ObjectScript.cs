using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Lean.Touch;
using System.Linq;

public class ObjectScript : MonoBehaviour {
	public enum State
	{
		ON_TARGET,
		LOST_TARGET,
		ON_AIR,
		ON_LP,
		ON_LP_LOCAL
	}
	public State curState = State.ON_TARGET;

	public string title;
	public bool isAnimatorChild;
	public bool isAnimalSound;
	public float scaleMin;
	public float scaleMax;
	public float offset;
	public Spelling spelling;
	public Vector3 targetLostRot;

	private GameObject parentObject;
	private Vector3 myScale;
	private float ypos;
	//private int animIndex;

	// private List<string> animList = new List<string>();
	private Vector3 actualRot;

	private AudioClip engAudio;
	// private AudioClip jpnAudio;
	// private AudioClip animalAudio;
	private AudioSource audioSource;

	private GameObject destpos;
	private Camera mainCam;

	private AnimateSoundScript animScript; 

	// Use this for initialization
	void Start ()
	{
		animScript = GetComponentInChildren<AnimateSoundScript>();

		if (SceneManager.GetActiveScene().name == "LPScene" || SceneManager.GetActiveScene().name == "LPLocalScene")
		{
			curState = State.ON_LP;

			// PlayAnimation();
		}

		if(SceneManager.GetActiveScene().name == "LPLocalScene")
		{
			if(gameObject.tag == "sun")
			{
				transform.GetChild(0).localScale = new Vector3(0.8f, 0.8f, 0.8f);
			}
		}

		if(gameObject.name != "sun")
		{
			GetComponent<LeanScale>().ScaleMin = new Vector3(scaleMin, scaleMin, scaleMin);
			GetComponent<LeanScale>().ScaleMax = new Vector3(scaleMax, scaleMax, scaleMax);
		}

		GetComponent<LeanScale>().ScaleClamp = true; 

		myScale = transform.localScale;

		ypos = transform.localPosition.y;

		if (curState != State.ON_LP)
		{
			parentObject = gameObject.transform.parent.gameObject;
		}		

		// Get audio source.
		audioSource = GetComponent<AudioSource>();

	}
	
	// Update is called once per frame
	void Update ()
	{
		// Place the object infornt of the camera.
		if (curState == State.LOST_TARGET)
		{
			Vector3 destpos = new Vector3(Controller.Instance.endPos.transform.position.x,
				Controller.Instance.endPos.transform.position.y + offset,
				Controller.Instance.endPos.transform.position.z);

			transform.position = Vector3.Lerp(transform.position, destpos, Time.deltaTime * 10);
		}

		DragAndRotate();
	}

	// Drags with fingers to rotate 
	void DragAndRotate()
	{
		// Drag and Rotate object
		var leanFingers = LeanTouch.GetFingers(true, false, 0);

		var screenDelta = Vector2.zero;

		// Rotate 
		if(leanFingers.Count == 1)
		{
			screenDelta = LeanGesture.GetScreenDelta(leanFingers);
		}

		// For linear interpolation purpose only.
		actualRot = Vector3.Lerp(actualRot, screenDelta, Time.deltaTime * 20.0f);

		// if do not want to use linear interpolation 
		// actualRot = dist; 

		if (gameObject.name == "king" || gameObject.name == "queen")
		{
			// For 180 Degree Rotation 
			transform.Rotate(0, -actualRot.x / 4.0f, 0, Space.Self);
		}
		else
		{
			// For 360 Degree Rotation 
			transform.Rotate(actualRot.y / 4.0f, 0, 0, Space.World);
			// For local space rotation 
			transform.Rotate(0, -actualRot.x / 4.0f, 0, Space.Self);
		}

		// For scalling sun
		if (gameObject.name == "sun")
		{
			var scale = transform.GetChild(0).transform.localScale;

			var twoFingers = LeanTouch.GetFingers(true, true, 2);

			var pinchScale = LeanGesture.GetPinchScale(twoFingers, 0);

			scale *= pinchScale;

			scale.x = Mathf.Clamp(scale.x, scaleMin, scaleMax);
			scale.y = Mathf.Clamp(scale.y, scaleMin, scaleMax);
			scale.z = Mathf.Clamp(scale.z, scaleMin, scaleMax);

			transform.GetChild(0).transform.localScale = scale;
		}
		// For scalling other object.
		else
		{
			GetComponent<LeanScale>().ScaleMin = new Vector3(scaleMin, scaleMin, scaleMin);
			GetComponent<LeanScale>().ScaleMax = new Vector3(scaleMax, scaleMax, scaleMax);
		}

	}

	// Reset position for this object. 
	public void ResetPos ()
	{
		transform.localPosition = new Vector3(0, ypos, 0);
	}

	// Reset Rotation for this object 
	public void ResetRot ()
	{
		transform.rotation = Quaternion.identity;
	}

	// Reset Scale for this object to original size. 
	public void ResetScale ()
	{
		transform.localScale = myScale;
	}

	// Reset all (Position, Rotation and Scale)
	public void ResetAll ()
	{
		// Reset all pos, rot and scale
		ResetPos();
		ResetRot();
		ResetScale();
	}

	// Found target 
	public void OnFoundTarget ()
	{
		Controller.Instance.particle.SetActive(false);
		Controller.Instance.particle.SetActive(true);
		Controller.Instance.spellingArea.SetActive(false);

		curState = State.ON_TARGET;

		string n = gameObject.name;

		spelling = Controller.Instance.spellingData.spellingList.Find(x => x.English == n);

		Controller.Instance.curObjSpellingData = spelling;

		Debug.Log("English : " + spelling.English);
		Debug.Log("Myanmar : " + spelling.Myanmar);

		// Make the first letter to Upper Case.
		string pathName = n.First().ToString().ToUpper() + n.Substring(1);

		// Default audio file for object.
		engAudio = Resources.Load("Pronunciation/ENUS/" + pathName) as AudioClip;

		PlayAnimation();

		ResetAll();

		DoEnable();
	}

	// Play animation if available 
	public void PlayAnimation()
	{
		if (animScript != null)
		{
			animScript.PlayAnimSound();
		}
	}

	// On Lost Target 
	public void OnLostTarget ()
	{
		curState = State.LOST_TARGET;

		ResetAll();		

		// Set the position to infront of the camera.
		transform.position = Controller.Instance.startPos.transform.position;

		// Set rotation 
		transform.rotation = Quaternion.Euler(targetLostRot);
	}

	// On Disable 
	public void OnDisableObject ()
	{
		DoDisable();
	}

	// Play pronunciation
	public void PlayAudio (string netCode)
	{
		// get the object name 
		string n = gameObject.name;

		// Make the first letter to Upper Case.
		string pathName = n.First().ToString().ToUpper() + n.Substring(1);

		switch (netCode)
		{
			case "us":
				audioSource.clip = Resources.Load("Pronunciation/ENUS/" + pathName) as AudioClip;
				break;

			case "uk":
				audioSource.clip = Resources.Load("Pronunciation/ENUK/" + pathName) as AudioClip;
				break;

			case "mm":
				audioSource.clip = Resources.Load("Pronunciation/MYA/" + pathName) as AudioClip;
				break;

			case "jp":
				audioSource.clip = Resources.Load("Pronunciation/JPN/" + pathName) as AudioClip;
				break;

			case "id":
				audioSource.clip = Resources.Load("Pronunciation/IND/" + pathName) as AudioClip;
				break;

			case "cn":
				audioSource.clip = Resources.Load("Pronunciation/CHN/" + pathName) as AudioClip;
				break;

			case "ph":
				audioSource.clip = Resources.Load("Pronunciation/PHP/" + pathName) as AudioClip;
				break;

			case "kr":
				audioSource.clip = Resources.Load("Pronunciation/KOR/" + pathName) as AudioClip;
				break;

			case "vn":
				audioSource.clip = Resources.Load("Pronunciation/VIE/" + pathName) as AudioClip;
				break; 

			case "th":
				break;

			default:
				break;
		}

		audioSource.loop = false;

		audioSource.volume = Controller.Instance.soundVolume;
		audioSource.Play();
	}

	// When mouse click and change animation 
	private void OnMouseUpAsButton()
	{
		if(animScript != null && curState != State.ON_LP)
		{
			animScript.PlayAnimSound();
		}
	}

	// Delete 
	public void OnDelete ()
	{
		audioSource.Stop();

		DoDisable();
	}

	// Enable the object 
	public void DoEnable()
	{
		var rendererComponents = GetComponentsInChildren<Renderer>(true);
		var colliderComponents = GetComponentsInChildren<Collider>(true);
		var canvasComponents = GetComponentsInChildren<Canvas>(true);

		// Enable rendering:
		foreach (var component in rendererComponents)
			component.enabled = true;

		// Enable colliders:
		foreach (var component in colliderComponents)
			component.enabled = true;

		// Enable canvas':
		foreach (var component in canvasComponents)
			component.enabled = true;
	}

	// Disable the object 
	public void DoDisable()
	{
		var rendererComponents = GetComponentsInChildren<Renderer>(true);
		var colliderComponents = GetComponentsInChildren<Collider>(true);
		var canvasComponents = GetComponentsInChildren<Canvas>(true);

		// Disable rendering:
		foreach (var component in rendererComponents)
			component.enabled = false;

		// Disable colliders:
		foreach (var component in colliderComponents)
			component.enabled = false;

		// Disable canvas':
		foreach (var component in canvasComponents)
			component.enabled = false;
	}
}
