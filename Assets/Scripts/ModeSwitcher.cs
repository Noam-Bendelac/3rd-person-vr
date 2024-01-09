using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ModeSwitcher : MonoBehaviour
{
    
    UnityEngine.XR.InputDevice rightController;
    
    bool prevButton = false;
    
    // public Transform[] targets;
    public Transform stationaryTarget;
    public Transform stationaryAvatarPosition;
    public Transform avatar;
    public Transform tetheredAvatarPosition;
    
    // 0: stationary; 1: tethered no smoothing; 2: tethered smoothing
    int mode = 0;
    
    SpringFollower follower;
    
    public TextMeshPro indicator;
    
    // Start is called before the first frame update
    void Start()
    {
        var rightHandedControllers = new List<UnityEngine.XR.InputDevice>();
        var desiredCharacteristics =
            UnityEngine.XR.InputDeviceCharacteristics.HeldInHand
            | UnityEngine.XR.InputDeviceCharacteristics.Right;
        UnityEngine.XR.InputDevices.GetDevicesWithCharacteristics(desiredCharacteristics, rightHandedControllers);
        
        rightController = rightHandedControllers[0];
        
        follower = gameObject.GetComponent<SpringFollower>();
        
        print("MS follower" + (follower));
        // print("MS targets" + (targets));
        
        updateToMode();
    }

    // Update is called once per frame
    void Update()
    {
        // secondaryButton
        bool button;
        rightController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.secondaryButton, out button);
        if (button && !prevButton) {
            // rising edge
            mode = (mode + 1) % 3;
            updateToMode();
        }
        
        prevButton = button;
    }
    
    void updateToMode() {
        if (mode == 0) {
            avatar.position = stationaryAvatarPosition.position;
        } else {
            avatar.position = tetheredAvatarPosition.position;
        }
        follower.following = mode == 0 ? stationaryTarget : avatar;
        follower.smooth = mode == 2;
        indicator.text = mode == 0 ? "A" : mode == 1 ? "B" : "C";
    }
}
