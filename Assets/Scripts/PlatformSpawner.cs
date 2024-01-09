using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    
    public Transform rightHand;
    
    public int numPlatforms = 3;
    
    // public Transform platformsGO;
    GameObject[] platforms;
    int idx = 0;
    
    UnityEngine.XR.InputDevice rightController;
    float prevTrigger = 0;
    
    
    // Start is called before the first frame update
    void Start()
    {
        var rightHandedControllers = new List<UnityEngine.XR.InputDevice>();
        var desiredCharacteristics =
            UnityEngine.XR.InputDeviceCharacteristics.HeldInHand
            | UnityEngine.XR.InputDeviceCharacteristics.Right;
        UnityEngine.XR.InputDevices.GetDevicesWithCharacteristics(desiredCharacteristics, rightHandedControllers);
        rightController = rightHandedControllers[0];
        
        platforms = new GameObject[numPlatforms];
        
        Material opaque = Resources.Load("PlatformOpaque", typeof(Material)) as Material;
        
        for (int i = 0; i < platforms.Length; i++) {
            // var clone = Instantiate(gameObject, platformsGO, true);
            var clone = Instantiate(gameObject, transform.parent);
            // clone.transform = transform; // needed?
            clone.SetActive(false);
            // disable THIS SCRIPT on the clone
            clone.GetComponent<PlatformSpawner>().enabled = false;
            clone.GetComponent<Renderer>().material = opaque;
            clone.GetComponent<Collider>().enabled = true;
            platforms[i] = clone;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // follow world pos and rot of right hand (scale doesn't work, no need)
        transform.position = rightHand.position;
        transform.rotation = rightHand.rotation * Quaternion.Euler(Vector3.right * 90);
        
        // place clones on trigger;
        float trigger;
        rightController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.trigger, out trigger);
        if (trigger > 0 && !(prevTrigger > 0)) {
            // rising edge
            var plat = platforms[idx];
            plat.SetActive(true);
            plat.transform.localPosition = transform.localPosition;
            plat.transform.localRotation = transform.localRotation;
            plat.transform.localScale = transform.localScale;
            idx = (idx + 1) % platforms.Length;
        }
        
        prevTrigger = trigger;
    }
}
