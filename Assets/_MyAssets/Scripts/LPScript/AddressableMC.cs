using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

public class AddressableMC : MonoBehaviour
{
    private List<IResourceLocation> remoteNos;
    public AssetLabelReference label;


    // Start is called before the first frame update
    void Start()
    {
        Load_Asset();
    }

    private void Load_Asset()
    {
        Addressables.LoadResourceLocationsAsync(label.labelString).Completed += OnDownload_Objects;
    }
   

    private void OnDownload_Objects(AsyncOperationHandle<IList<IResourceLocation>> obj)
    {
        Debug.Log("Download assets complete.");

        remoteNos = new List<IResourceLocation>(obj.Result);
         int i = 0;
      //  Addressables.LoadAssetAsync<GameObject>(remoteNos[0]);


        for (i = 0; i < remoteNos.Count; i++)
        {
            Addressables.LoadAssetAsync<GameObject>(remoteNos[i]).Completed += (loadedAsset) =>
            {
                // Debug.Log("Result : " + loadedAsset.Result)
                

            };
/*            Vector3 spawnPosition = new Vector3(-0.4f, -0.1f, 0f);
            var game = Addressables.InstantiateAsync(remoteNos[i], spawnPosition, Quaternion.identity);
            Debug.Log("3D Model" + game + remoteNos.Count);*/
        }

    }


}
