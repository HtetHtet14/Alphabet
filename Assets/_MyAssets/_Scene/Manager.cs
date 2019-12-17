using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
public class Manager : MonoBehaviour
{
	private AssetReference localNo;

	private List<IResourceLocation> remoteNos;
	public AssetLabelReference number;
	// Start is called before the first frame update
	void Start()
	{
		//DisplayNos();
		Addressables.LoadResourceLocationsAsync(number.labelString).Completed += LocationLoded;
	}

	private void LocationLoded(AsyncOperationHandle<IList<IResourceLocation>> obj)
	{
		remoteNos = new List<IResourceLocation>(obj.Result);
		StartCoroutine(SpawnRemoteNos());
	}

	private IEnumerator SpawnRemoteNos()
	{
		yield return new WaitForSeconds(1f);
		float x0ff = -4.0f;

		for (int i = 0; i < remoteNos.Count; i++)
		{
			Vector3 spawnPosition = new Vector3(x0ff, 3, 0);

			Addressables.InstantiateAsync(remoteNos[i], spawnPosition, Quaternion.identity);

			x0ff = x0ff + 2.5f;
			yield return new WaitForSeconds(1f);
		}
	}

	private void DisplayNos()
	{
		//Display local prefabs
		//localNo.InstantiateAsync(Vector3.zero, Quaternion.identity);
		Debug.Log(number.labelString);
	}


}
