using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoyScript : MonoBehaviour
{
	public ParticleSystem[] parts;

	float minDivider;
	float maxDivider;

	// Use this for initialization
	void Start ()
	{
		minDivider = 1 / parts[0].main.startSize.constantMin;
		maxDivider = 1 / parts[0].main.startSize.constantMax;
	}
	
	// Update is called once per frame
	void Update ()
	{
		foreach (var item in parts)
		{
			var main = item.main;

			main.startSize = Random.Range(transform.localScale.x / minDivider, transform.localScale.x / maxDivider);
		}
	}
}
