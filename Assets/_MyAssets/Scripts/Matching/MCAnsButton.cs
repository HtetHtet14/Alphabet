using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MCAnsButton : MonoBehaviour {

	public Text ansText; 

	private string ans; 

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	//
	public void SetAnswer (string _ans)
	{
		ans = _ans;

		ansText.text = _ans; 
	}

	public void OnMouseDown()
	{
		GameObject obj = Instantiate(MCManager.Instance.dragAnsPrefab, transform.position, Quaternion.identity);
		obj.GetComponent<MCDragObj>().SetText(ans);
		obj.GetComponent<MCDragObj>().clickedObj = this.gameObject;

		gameObject.SetActive(false);
	}
}
