using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextObjScript : MonoBehaviour
{
	public Text text;
	public GameObject particle; 

	private Vector2 pos;

	public float destYpos;

	// Use this for initialization
	void Start ()
	{
		pos.y = transform.position.y;

		destYpos = Screen.height / 2 - 100;
	}
	
	// Update is called once per frame
	void Update ()
	{
		transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, destYpos, transform.position.z), 5 * Time.deltaTime);
	}

	public void SetChar (char c)
	{
		text.text = c.ToString(); 
	}

	public void PlayParticle ()
	{
		particle.SetActive(true);
	}
}
