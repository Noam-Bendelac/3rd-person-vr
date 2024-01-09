using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using VectorSwizzle;

public class SpringFollower : MonoBehaviour
{
    
    private Transform _following;
    public Transform following {
        get => _following;
        set {
            // teleport instead of spring
            transform.position = value.position;
            _following = value;
        }
    }
    
    public bool smooth = true;
    
    public float k = 0.9f;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = following.position;
    }

    // Update is called once per frame
    void Update()
    {
        var diff = following.position.xz() - transform.position.xz();
        Vector2 newXZ;
        if (smooth) {
            newXZ = (transform.position.xz() + k * diff * Time.deltaTime);
        } else {
            newXZ = following.position.xz();
        }
        
        var newP = transform.position;
        newP.x = newXZ.x;
        newP.z = newXZ.y;
        transform.position = newP;
        
    }
}
