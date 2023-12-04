using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UI;

public class ObjectSwitcher : MonoBehaviour
{
    public GameObject[] scenarios;
    private int current = 0;
    private List<GameObject> instantiatedObjects = new List<GameObject>();

    public Button sphereButton;
    public Button cubeButton;
    private int flag = 0;

    public GameObject cube;
    public GameObject sphere;

    void Start()
    {
        foreach (GameObject obj in scenarios)
        {
            obj.SetActive(false);
        }
        cube.SetActive(false);
        sphere.SetActive(false);
        scenarios[0].SetActive(true);

        sphereButton.onClick.AddListener(ChangeObjectToSphere);
        cubeButton.onClick.AddListener(ChangeObjectToCube);
    }

    public void NextObject()
    {
        current = (current + 1) % scenarios.Length;
        SwitchObject();
    }

    public void PreviousObject()
    {
        current = (current - 1 + scenarios.Length) % scenarios.Length;
        SwitchObject();
    }

    private void SwitchObject()
    {
        cube.SetActive(false);
        sphere.SetActive(false);
        foreach (GameObject obj in scenarios)
        {
            obj.SetActive(false);
        }
        foreach (GameObject obj in instantiatedObjects)
        {
            obj.SetActive(false);
            Destroy(obj);
        }
        instantiatedObjects.Clear();


        if (current == 6) sphere.SetActive(true);
        else scenarios[current].SetActive(true);
    }


    [SerializeField]
    private ARRaycastManager ARM;
    private ARPlaneManager APM;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private void Awake()
    {
        ARM = GetComponent<ARRaycastManager>();
        APM = GetComponent<ARPlaneManager>();
    }

    private void OnEnable()
    {
        EnhancedTouch.TouchSimulation.Enable();
        EnhancedTouch.EnhancedTouchSupport.Enable();
        EnhancedTouch.Touch.onFingerDown += FingerDown;
    }

    private void OnDisable()
    {
        EnhancedTouch.TouchSimulation.Disable();
        EnhancedTouch.EnhancedTouchSupport.Disable();
        EnhancedTouch.Touch.onFingerDown -= FingerDown;
    }

    private void FingerDown(EnhancedTouch.Finger finger)
    {
        if (finger.index != 0) return;
        if (ARM.Raycast(finger.currentTouch.screenPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            foreach (ARRaycastHit hit in hits)
            {
                Pose pose = hit.pose;

                GameObject obj = null;

                if (current == 6)
                {
                    switch (flag)
                    {
                        case 0:
                            obj = Instantiate(sphere, pose.position, pose.rotation);
                            break;
                        case 1:
                            obj = Instantiate(cube, pose.position, pose.rotation);
                            break;
                    }
                }
                else
                {
                    obj = Instantiate(scenarios[current], pose.position, pose.rotation);
                    scenarios[current].SetActive(false);
                }

                float scaleFactor = 0.1f; // Adjust this value as needed
                if (current == 1) scaleFactor = 0.3f;

                if (current == 5) obj.transform.localScale = new Vector3(0.5f, 0.5f, 1.0f);
                else obj.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);


                instantiatedObjects.Add(obj);


            }
        }
    }

    private void ChangeObjectToSphere()
    {
        sphere.SetActive(true);
        cube.SetActive(false);
        flag = 0;
    }

    private void ChangeObjectToCube()
    {
        cube.SetActive(true);
        sphere.SetActive(false);
        flag = 1;
    }
}