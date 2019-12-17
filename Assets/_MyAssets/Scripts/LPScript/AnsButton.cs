using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Lean.Touch;

public class AnsButton : MonoBehaviour {

	public string ans;
	public Text ansText;
	public Image frameImg;
	public Sprite frameNormal;
	public Sprite framePress; 

	private GameObject menuObj;
	private Button button; 

	// Use this for initialization
	void Start ()
	{
		button = GetComponent<Button>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	private void OnMouseDown()
	{
		transform.localScale = new Vector3(0.8f, 0.8f, 0.8f); 
	}

	private void OnMouseUp()
	{
		transform.localScale = Vector3.one;
	}

	// Set the attribute for this button 
	public void OnSetAnswer (string _ans, bool showSpr)
	{
		GetComponent<Button>().interactable = true; 
		GetComponent<GAui>().MoveIn();

		if (menuObj != null)
			Destroy(menuObj);

		ansText.enabled = true; 

		ans = _ans;
		ansText.text = _ans.ToUpper();

		Sprite spr = Resources.Load<Sprite>("Edit/"+_ans);

	

		if (showSpr)
		{
			ansText.enabled = false; 
		}
		else
		{
			ansText.enabled = true; 
	
		}
	}
/*
	public void OnSetAnswer (GameObject obj)
	{
		GetComponent<Button>().interactable = true;
		GetComponent<GAui>().MoveIn();

		

		if (menuObj == null)
		{
			menuObj = obj; 
		}
		else
		{
			Destroy(menuObj);
			menuObj = obj; 
		}

		ansText.text = obj.GetComponent<ObjectScript>().title;

		ansText.enabled = false;

		string name = obj.GetComponent<ObjectScript>().title;

		Sprite spr = Resources.Load<Sprite>("Edit/"+name);

	
	}
*/
	// on click on the button 
	/*public void OnClickButton ()
	{
		frameImg.sprite = framePress;
	}

	public void OnReleaseButton ()
	{
		frameImg.sprite = frameNormal;
	}*/
}
