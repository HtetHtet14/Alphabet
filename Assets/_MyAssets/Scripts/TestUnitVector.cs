using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TestUnitVector : MonoBehaviour
{
	public GameObject obj; 

	public GameObject[] objs; 

    // Start is called before the first frame update
    void Start()
    {
		// obj.transform.position = Vector2.zero;

		float x = 250 + Mathf.Sin(45);
		float y = 250 + Mathf.Cos(45);

		obj.transform.position = new Vector2(x, y) + (Vector2)transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
		{
			var v = Vector2.zero;
		}
    }
}
