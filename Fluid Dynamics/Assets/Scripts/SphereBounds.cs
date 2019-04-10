using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereBounds : MonoBehaviour {

    bool firstTime = true;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        CheckBounds();
	}

    void CheckBounds()
    {
        if(transform.position.x < 0.6f)
        {
            DisableTrail();
            transform.position = new Vector3(14, -0.509f, transform.position.z);
            EnableTrail();
        }
        else if(transform.position.x > 14)
            {
            DisableTrail();
            transform.position = new Vector3(0.6f, -0.509f, transform.position.z);
            EnableTrail();
        }
        if(transform.position.z < -1 )
        {
            DisableTrail();
            transform.position = new Vector3(transform.position.x, -0.509f, 12);
            EnableTrail();
        }
        else if(transform.position.z > 12)
        {
            DisableTrail();
            transform.position = new Vector3(transform.position.x, -0.509f, -1);
            EnableTrail();
        }
    }

    void DisableTrail()
    {
        this.gameObject.transform.GetChild(0).gameObject.GetComponent<TrailRenderer>().enabled = false;
    }

    void EnableTrail()
    {
        this.gameObject.transform.GetChild(0).gameObject.GetComponent<TrailRenderer>().enabled = true;
    }
    public void initialForce(float horz, float vert)
    {
        if(firstTime)
        {
            gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(horz, 0, vert) * 100);
            firstTime = false;
        }
    }
}
