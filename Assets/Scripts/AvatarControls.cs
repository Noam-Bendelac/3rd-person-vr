using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.XR;
using static UnityEngine.XR.CommonUsages;
// using UnityEngine.XR.CommonUsages.trigger;

public class AvatarControls : MonoBehaviour
{
    
    public Transform leftHand;
    
    public float speed = 5;
    
    UnityEngine.XR.InputDevice leftController;
    
    Rigidbody rb;
    
    bool prevTrigger;
    
    public new Collider collider;
    float distToGround;
    
    // Start is called before the first frame update
    void Start()
    {
        // var leftControllers = new List<UnityEngine.XR.InputDevice>();
        // UnityEngine.XR.InputDevices.GetDevicesWithRole(UnityEngine.XR.InputDeviceRole.LeftHanded, leftControllers);
        var leftHandedControllers = new List<UnityEngine.XR.InputDevice>();
        var desiredCharacteristics =
            UnityEngine.XR.InputDeviceCharacteristics.HeldInHand
            | UnityEngine.XR.InputDeviceCharacteristics.Left;
        UnityEngine.XR.InputDevices.GetDevicesWithCharacteristics(desiredCharacteristics, leftHandedControllers);
        leftController = leftHandedControllers[0];
        
        rb = GetComponent<Rigidbody>();
        
        distToGround = collider.bounds.extents.y;
    }

    // Update is called once per frame
    void Update()
    {
        
        
        // jumping
        float trigAnalog;
        leftController.TryGetFeatureValue(trigger, out trigAnalog);
        bool trigPress = trigAnalog > 0;
        
        // if (trigPress && !prevTrigger && grounded()) {
        if (trigPress && !prevTrigger) {
            // impulses are ok for Update; anything else needs to be in FixedUpdate
            rb.AddForce(Vector3.up * 15, ForceMode.Impulse);
            
        }
        
        prevTrigger = trigPress;
        
    }
    
    void FixedUpdate() {
        // horizontal movement based on left stick
        
        Vector2 stick;
        leftController.TryGetFeatureValue(primary2DAxis, out stick);
        
        var pointDir = leftHand.forward * stick[1] + leftHand.right * stick[0];
        pointDir.y = 0;
        
        // transform.position += pointDir * speed * Time.deltaTime;
        var xzVel = pointDir * speed;
        rb.velocity = new Vector3(xzVel.x, rb.velocity.y, xzVel.z);
        
    }
    
    bool grounded() {
        return Physics.Raycast(collider.transform.position, -Vector3.up, distToGround + 2f);
    }
}
