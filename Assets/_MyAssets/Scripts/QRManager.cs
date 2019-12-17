using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using ZXing;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class QRManager : MonoBehaviour {

	public GameObject pleaseWait;
	public RawImage rawImage;
	public GUIAnim infoPanel;
	public Text infoText; 

	private WebCamTexture camTexture;
	private Rect screenRect;

	private bool isGetCode = false;
	public int width;
	public int height;

	private QRCodeResult result;

	// Use this for initialization
	void Start ()
	{
		pleaseWait.SetActive(false);

		// height = Screen.height;

		// screenRect = new Rect(Screen.width/2 - width/2, Screen.height/2 - height/2, width, height);
		// screenRect = new Rect(0, 0, 400, 400);
		// screenRect = new Rect(0, 0, Screen.width, Screen.height);

		camTexture = new WebCamTexture
		{
			requestedWidth = width,
			requestedHeight = height
		};


		Screen.orientation = ScreenOrientation.LandscapeLeft;	// Rotate phone right 


		// bgRawImage.texture = qrTexture;
		rawImage.texture = camTexture;

#if UNITY_IPHONE
		rawImage.transform.localScale = new Vector3(rawImage.transform.localScale.x, -rawImage.transform.localScale.y, 1);
#endif


		if (camTexture != null)
		{
			camTexture.Play();
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	void OnGUI()
	{
		// drawing the camera on screen
		// GUI.DrawTexture(screenRect, camTexture, ScaleMode.ScaleAndCrop);
		// do the reading — you might want to attempt to read less often than you draw on the screen for performance sake

		if (isGetCode)
		{
			//GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), texture);
			return;
		}

		try
		{
			if (!camTexture.isPlaying)
				return; 

			IBarcodeReader barcodeReader = new BarcodeReader();
			// decode the current frame
			var result = barcodeReader.Decode(camTexture.GetPixels32(),
				camTexture.width, camTexture.height);

			//Debug.Log(camTexture.width + " and " + camTexture.height);

			if (result != null)
			{
				Debug.Log("DECODED TEXT FROM QR: " +result.Text);

				StartCoroutine(ApiCall(result.Text));

				// Show please wait
				pleaseWait.SetActive(true);

				isGetCode = true; 
			}

		}
		catch (Exception ex) { Debug.LogWarning(ex.Message); }
	}

	IEnumerator ApiCall (string qr)
	{
		QRCodeRequest request = new QRCodeRequest
		{
			qrcode = qr,
			imei = SystemInfo.deviceUniqueIdentifier,
		};

		

		string jsonString = JsonUtility.ToJson(request);

		string link = "https://salty-thicket-31328.herokuapp.com/api/interAlphabet/qr/activate";

		Dictionary<string, string> header = new Dictionary<string, string>
			{
				{ "Content-Type", "application/json" }
			};

		byte[] body = Encoding.UTF8.GetBytes(jsonString);

		WWW www = new WWW(link, body, header);

		yield return www;

		result = JsonUtility.FromJson<QRCodeResult>(www.text);

		if (result.code == 2)
		{
			ZPlayerPrefs.SetInt("ACTIVATED", 1);
		}

		// Implemented result here.
		infoPanel.gameObject.SetActive(true);
		infoPanel.MoveIn();
		infoText.text = result.message;

		Debug.Log(www.text);
	}


	// On Click Back Button 
	public void OnClickBack()
	{
		camTexture.Stop();

		ResetOrientation();

		SceneManager.LoadScene("MainMenu");
	}

	// On Click ok from pop up 
	public void OnClickOk ()
	{
		camTexture.Stop();

		ResetOrientation();

		if (result.code == 2)
		{
			SceneManager.LoadScene("ARScene");
		}
		else
		{
			SceneManager.LoadScene("MainMenu");
		}
	}

	// Reset Screen orientation 
	void ResetOrientation ()
	{
		Screen.orientation = ScreenOrientation.AutoRotation;

		Screen.autorotateToPortrait = false;
		Screen.autorotateToPortraitUpsideDown = false; 
		Screen.autorotateToLandscapeLeft = true;
		Screen.autorotateToLandscapeRight = true; 
	}
}

[Serializable]
public class QRCodeRequest
{
	public string qrcode;
	public string imei;
}

[Serializable]
public class QRCodeResult
{
	public int code;
	public string message; 
}
