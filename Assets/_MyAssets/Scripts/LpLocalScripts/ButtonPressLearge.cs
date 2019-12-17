using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPressLearge : MonoBehaviour
{
	private Button button; 

	// Use this for initialization
	void Start ()
	{
		button = GetComponent<Button>();		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetMouseButtonUp(0))
		{
			transform.localScale = Vector3.one;
		}
	}

	public void OnClick ()
	{
		Debug.Log("Calling Onclick");

		if (gameObject.tag == "Answer")
		{
			transform.localScale = new Vector3(1.1f, 1.1f, 0);
		}
		else
		{
			transform.localScale = new Vector3(1.2f, 1.2f, 0);
		}
	}
}
