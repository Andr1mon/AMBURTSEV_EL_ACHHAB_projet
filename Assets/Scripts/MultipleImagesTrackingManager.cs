using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class MultipleImage : MonoBehaviour 
{
    [SerializeField] GameObject[] prefabsToSpawn;

    private ARTrackedImageManager _arTrackedImageManager;
    private Dictionary<string, GameObject> _arObjects;
    private Dictionary<string, int> objectNumber; 
    static public int[] objectsState;
    

    // Get the reference of ARTrackedImageManager
    private void Awake() 
    {
        _arTrackedImageManager = GetComponent<ARTrackedImageManager>();
        _arObjects = new Dictionary<string, GameObject>();
        objectNumber = new Dictionary<string, int>();
        objectsState = new int[255];
    }

    private void Start() 
    {
        _arTrackedImageManager.trackedImagesChanged += onTrackedImageChanged;
        int k = 1;
        foreach(GameObject prefab in prefabsToSpawn)
        {
            GameObject newARObject = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            newARObject.name = prefab.name;
            newARObject.gameObject.SetActive(false);
            _arObjects.Add(newARObject.name, newARObject);
            objectNumber.Add(newARObject.name, k++);
            objectsState[objectNumber[newARObject.name]] = -1;
        }
    }

    private void OnDestroy() 
    {
        _arTrackedImageManager.trackedImagesChanged += onTrackedImageChanged;
    }

    private void onTrackedImageChanged(ARTrackedImagesChangedEventArgs eventArgs) 
    {
        // Identify the change on the Tracked image
        foreach(ARTrackedImage trackedImage in eventArgs.added) 
        {
            UpdateTrackedImage(trackedImage);
        }
        foreach(ARTrackedImage trackedImage in eventArgs.updated) 
        {
            UpdateTrackedImage(trackedImage);
        }
        foreach(ARTrackedImage trackedImage in eventArgs.removed) 
        {
            _arObjects[trackedImage.referenceImage.name].gameObject.SetActive(false);
        }
    }

    private void UpdateTrackedImage(ARTrackedImage trackedImage) 
    {
        // Check tracking status of the tracked image
        if(trackedImage.trackingState is TrackingState.Limited or TrackingState.None) 
        {
            _arObjects[trackedImage.referenceImage.name].gameObject.SetActive(false);
            return;
        }
        // Show, hide or position the gameObject on the tracked image 
        if (prefabsToSpawn != null) 
        {
            _arObjects[trackedImage.referenceImage.name].gameObject.SetActive(true);
            _arObjects[trackedImage.referenceImage.name].transform.position = trackedImage.transform.position;
            objectsState[objectNumber[trackedImage.referenceImage.name]] = objectNumber[trackedImage.referenceImage.name];
        }
    }
}
