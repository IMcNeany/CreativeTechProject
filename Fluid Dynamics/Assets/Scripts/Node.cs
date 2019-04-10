using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {

    public float verticalVelocity = 0.3f;
    public float horizontalVelocity = 0.2f;
    public float previousVerticalVelocity;
    public float previousHorizontalVelocity;

    public float density;
    public float previousDensity;
    public float horzValue =0;
    public float vertValue = 0;

    //each node is going to have a version of each func, using the array to naviagte around
  

    // Use this for initialization
    void Start () {
      
	}
	
	// Update is called once per frame
	void Update () {
        SetArrowDir();
        transform.GetChild(0).TransformDirection(new Vector3(vertValue, 90, horzValue));
        //transform.position = transform.position;

      //  float angleArrow = Mathf.Acos(Vector3.Dot(new Vector3(0, 1, 0), force) / force.magnitude);
       // Vector3 axisArrowRot = Vector3.Cross(new Vector3(0, 1, 0), force);
       // arrow.transform.rotation = Quaternion.identity * Quaternion.LookRotation(force, axisArrowRot * angleArrow);

      
    }
    public void SetInitals(float H = 0, float v = 0, float D = 0)
    {
        horizontalVelocity = H;
        verticalVelocity = v;
        density = D;
    }
    private void OnTriggerEnter(Collider collision)
    {
        //if sphere tiny push in 
        if(collision.name == "Sphere")
        {
            collision.gameObject.GetComponent<SphereBounds>().initialForce(horizontalVelocity, verticalVelocity);
          //  Debug.Log("sphere");
            Rigidbody particles = collision.attachedRigidbody;

            SetArrowDir();
                
            Debug.Log("horz " + horzValue*density + "ver" + vertValue*density);

            // particles.AddForce(new Vector3(horzValue , 0, vertValue) * (density));
            if(density == 0)
            {
                density = 1;
            }
            particles.AddRelativeForce(new Vector3(horzValue, 0, vertValue)* (density));
             
           // Debug.Log("force " + (new Vector3(horzValue, 0, vertValue)*density));
           // particles.angularVelocity = 
          //  horz = horizontalVelocity * density;
        //    vert = verticalVelocity * density;
            //  Debug.Log("hortz final value " + horzValue * density);

           // transform.GetChild(0).transform.LookAt(new Vector3(horzValue*density, 90, vertValue * density));

            particles.velocity = Vector3.zero;
            particles.angularVelocity = Vector3.zero;
        }
        //x and y dir
    }

    void SetArrowDir()
    {
        horzValue = 0;
        if (horizontalVelocity < 0)
        {

            float value = Mathf.Log10(Math.Abs(horizontalVelocity));
            horzValue = value * -1;
        }
        else
        {
            horzValue = Mathf.Log10(horizontalVelocity);
        }


        vertValue = 0;
        if (verticalVelocity < 0)
        {

            float value = Mathf.Log10(Math.Abs(verticalVelocity));
            vertValue = value * -1;
        }
        else
        {
            vertValue = Mathf.Log10(verticalVelocity);
        }
    }

    #region Density


    public void AddSource()
    {
        if (density > 10)
        {
            density = 1;
        }
        else
        {
             density += Time.deltaTime * previousDensity;
          //  density += amount;
        }
   
    }

    public void SwapDensity()
    {
        float temp = previousDensity;
        previousDensity = density;
        density = previousDensity;
    }

  


    #endregion

    #region Velocity
    
    public void AddVelocitySource()
    {
       
        horizontalVelocity += Time.deltaTime * previousHorizontalVelocity;
        verticalVelocity += Time.deltaTime * previousVerticalVelocity;
    }

    public void SwapHorizontalVelocity()
    {
        float temp = previousHorizontalVelocity;
        previousHorizontalVelocity = horizontalVelocity;
        horizontalVelocity = previousHorizontalVelocity;
       // horizontalVelocity = 0;
    }

    public void SwapVerticalVelocity()
    {
        float temp = previousVerticalVelocity;
        previousVerticalVelocity = verticalVelocity;
         verticalVelocity = previousVerticalVelocity;
      //  verticalVelocity = 0;
    }

    #endregion
}
