using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

    public int size = 12;
    public GameObject node;
    public float diff = 1.0f;
    public float visc = 0.5f;
    public Node[,] nodeArray;
    public List<GameObject> sphere;
    float amount;
    float time = 0;
    float timer = 2.0f;
    int spheresReleased;
    Rigidbody currentSphere;
    bool once = true;
	// Use this for initialization
	void Start () {
        nodeArray = new Node[size,size];
        DrawGrid();
        SetVel();
	}

    void SetVel()
    {
        int j = 0;
        for(int i = 0; i < size; i++)
        {
            nodeArray[i, j].SetInitals(0.3f, 0.0f, 5);
        }
    }
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;

        if(time > timer)
        {

            if (once)
            {
                for (int i = 0; i < sphere.Count; i++)
                {
                    sphere[i].GetComponent<Rigidbody>().AddRelativeForce(new Vector3(100.0f, 0.0f));
                }
                once = false;
            }
    

        }
        VelocityStep();
        DensityStep();
	}

    private void DrawGrid()
    {
        float spacing = 12.0f / size;
        float xStartPos = 1.5f;
        float yStartPos = 0.0f;
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                GameObject newNode = Instantiate(node, new Vector3(xStartPos, 0, yStartPos), new Quaternion(1.0f, 0, 0, 1));
                newNode.transform.localScale = new Vector3(spacing, spacing, spacing);
                newNode.transform.SetParent(gameObject.transform);
                nodeArray[i, j] = newNode.GetComponent<Node>();
                xStartPos += spacing;
            }
            xStartPos = 1.5f;
            yStartPos += spacing;
        }
    }

    public void VelocityStep()
    {
        VelocitySource();
        SwapHVelocity();
        DiffuseHVelocity();
        SwapVVelocity();
        DiffuseVerticalVelocity();
        ProjectVelocity();
        SwapHVelocity();
        SwapVVelocity();
        AdvectHorizontalVelocity();
        AdvectVerticalVelocity();
        ProjectVelocity();
    }

    #region Velocity Solver

    private void AdvectVerticalVelocity()
    {

        float dt0 = Time.deltaTime * size;
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                float x = i - dt0 * nodeArray[i, j].previousHorizontalVelocity;
                float y = j - dt0 * nodeArray[i, j].previousVerticalVelocity;

                if (x < 0.5f)
                {
                    x = 0.5f;
                    //x = size - 1;
                }
                if (x > (size - 1) + 0.5f)
                {
                     x  = (size-1) + 0.5f;
                   // x = 0;
                }

                int i0 = (int)x;
                if(i0 >= size)
                {
                    i0 = size - 1;
                }
                int i1 = i0 + 1;
                if (i1 == size)
                {
                    i1 = 0;
                }

                if (y < 0.5f)
                {
                    y = 0.5f;
                    // y = size-1;
                }
                if (y > (size - 1) + 0.5f)
                {
                     y = (size - 1) + 0.5f;
                   // y = 0;
                }

                int j0 = (int)y;
                if(j0 >= size)
                {
                    j0 = size - 1;
                }
                int j1 = j0 + 1;
                //if j1 - size ==0
                if (j1 == size)
                {
                    j1 = 0;
                }
                float s1 = x - i0;
                float s0 = 1 - s1;
                float t1 = y - j0;
                float t0 = 1 - t1;


                nodeArray[i, j].verticalVelocity = s0 * (t0 * nodeArray[i0, j0].previousVerticalVelocity + t1 * nodeArray[i0, j1].previousVerticalVelocity)
                    + s1 * (t0 * nodeArray[i1, j0].previousVerticalVelocity + t1 * nodeArray[i1, j1].previousVerticalVelocity);

            }
        }
    }

    private void AdvectHorizontalVelocity()
    {
        float dt0 = Time.deltaTime * size;
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                float x = i - dt0 * nodeArray[i, j].previousHorizontalVelocity;
                float y = j - dt0 * nodeArray[i, j].previousVerticalVelocity;

                if (x < 0.5f)
                {
                    x = 0.5f;
                    //x = size - 1;
                }
                if (x > (size - 1) + 0.5f)
                {
                    x = (size - 1) + 0.5f;
                    // x = 0;
                }

                int i0 = (int)x;
                if (i0 >= size)
                {
                    i0 = size - 1;
                }
                int i1 = i0 + 1;
                if (i1 == size)
                {
                    i1 = 0;
                }

                if (y < 0.5f)
                {
                    y = 0.5f;
                    // y = size-1;
                }
                if (y > (size - 1) + 0.5f)
                {
                    y = (size - 1) + 0.5f;
                    // y = 0;
                }

                int j0 = (int)y;
                if (j0 >= size)
                {
                    j0 = size - 1;
                }
                int j1 = j0 + 1;
                //if j1 - size ==0
                if (j1 == size)
                {
                    j1 = 0;
                }
                float s1 = x - i0;
                float s0 = 1 - s1;
                float t1 = y - j0;
                float t0 = 1 - t1;


                nodeArray[i, j].horizontalVelocity = s0 * (t0 * nodeArray[i0, j0].previousHorizontalVelocity + t1 * nodeArray[i0, j1].previousHorizontalVelocity)
                    + s1 * (t0 * nodeArray[i1, j0].previousHorizontalVelocity + t1 * nodeArray[i1, j1].previousHorizontalVelocity);

            }
        }
    }

    private void ProjectVelocity()
    {
        float h = 1.0f / size;

      for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {

                int l = i - 1;
                int m = i + 1;
                int n = j - 1;
                int o = j + 1;

                if (i == 0)
                {
                    l = size - 1;
                }
                if (i == size - 1)
                {
                    m = 0;
                }

                if (j == 0)
                {
                    n = size - 1;
                }
                if (j == size - 1)
                {
                    o = 0;
                }


                nodeArray[i, j].previousVerticalVelocity = -0.5f * h * (nodeArray[m, j].horizontalVelocity - nodeArray[l, j].horizontalVelocity
                     + nodeArray[i, o].verticalVelocity - nodeArray[i, n].verticalVelocity);
              //  Debug.Log(nodeArray[i, j].previousVerticalVelocity + "prev vel");
                nodeArray[i, j].previousHorizontalVelocity = 0;
            }
        }

        //div =v0 
        //p = u0
        //u = u
        //v = v

        for (int k = 0; k < 20; k++)
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    int l = i - 1;
                    int m = i + 1;
                    int n = j - 1;
                    int o = j + 1;

                    if (i == 0)
                    {
                        l = size - 1;
                    }
                    if (i == size - 1)
                    {
                        m = 0;
                    }

                    if (j == 0)
                    {
                        n = size - 1;
                    }
                    if (j == size - 1)
                    {
                        o = 0;
                    }

                    nodeArray[i,j].previousHorizontalVelocity = (nodeArray[i,j].previousVerticalVelocity + nodeArray[l,j].previousHorizontalVelocity
                        + nodeArray[m,j].previousHorizontalVelocity + nodeArray[i,n].previousHorizontalVelocity + nodeArray[i,o].previousHorizontalVelocity)/4;
                }
            }
        }
       for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                int l = i - 1;
                int m = i + 1;
                int n = j - 1;
                int o = j + 1;

                if (i == 0)
                {
                    l = size - 1;
                }
                if (i == size - 1)
                {
                    m = 0;
                }

                if (j == 0)
                {
                    n = size - 1;
                }
                if (j == size - 1)
                {
                    o = 0;
                }

                nodeArray[i, j].horizontalVelocity -= 0.5f * (nodeArray[m, j].previousHorizontalVelocity - nodeArray[l, j].previousHorizontalVelocity) / h;
                nodeArray[i, j].verticalVelocity -= 0.5f * (nodeArray[i, o].previousHorizontalVelocity - nodeArray[i, n].previousHorizontalVelocity) / h;
            }
        }
    }

    private void DiffuseHVelocity()
    {

        float a = Time.deltaTime * visc * size * size;

        for (int k = 0; k < 20; k++)
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    int l = i - 1;
                    int m = i + 1;
                    int n = j - 1;
                    int o = j + 1;

                    if (i == 0)
                    {
                        l = size - 1;
                    }
                    if (i == size-1)
                    {
                        m = 0;
                    }

                    if (j == 0)
                    {
                        n = size - 1;
                    }
                    if (j == size-1)
                    {
                        o = 0;
                    }

                    nodeArray[i, j].horizontalVelocity = (nodeArray[i, j].previousHorizontalVelocity + a * (nodeArray[l, j].horizontalVelocity + nodeArray[m, j].horizontalVelocity
                        + nodeArray[i, n].horizontalVelocity + nodeArray[i, o].horizontalVelocity)) / (1 + 4 * a);
                }
            }
        }
    }

    private void DiffuseVerticalVelocity()
    {
        float a = Time.deltaTime * visc * size * size;

        for (int k = 0; k < 20; k++)
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    int l = i - 1;
                    int m = i + 1;
                    int n = j - 1;
                    int o = j + 1;

                    if (i == 0)
                    {
                        l = size - 1;
                    }
                    if (i == size - 1)
                    {
                        m = 0;
                    }

                    if (j == 0)
                    {
                        n = size - 1;
                    }
                    if (j == size - 1)
                    {
                        o = 0;
                    }


                    nodeArray[i, j].verticalVelocity = (nodeArray[i, j].previousVerticalVelocity + a * (nodeArray[l, j].verticalVelocity + nodeArray[m, j].verticalVelocity
                        + nodeArray[i, n].verticalVelocity + nodeArray[i, o].verticalVelocity)) / (1 + 4 * a);
                }
            }
        }
    }

    private void SwapVVelocity()
    {
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                nodeArray[i, j].SwapVerticalVelocity();
            }
        }
    }

    private void SwapHVelocity()
    {
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                nodeArray[i, j].SwapHorizontalVelocity();
            }
        }
    }

    private void VelocitySource()
    {
       
        for(int i =0; i < size; i ++)
        {
            for(int j =0; j < size; j++)
            {
                nodeArray[i, j].AddVelocitySource();
            }
        }
    }
    #endregion

    #region Density Solver

    public void DensityStep()
    {
        DensitySource();
        SwapDensityAround();
        DiffuseDensity();
        AdvectDensity();

    }

    private void SwapDensityAround()
    {
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                nodeArray[i, j].SwapDensity();
            }
        }
    }

    private void DensitySource()
    {

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                nodeArray[i, j].AddSource();
            }
        }
    }

    private void AdvectDensity()
    {
        float dt0 = Time.deltaTime * size;
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                float x = i - dt0 * nodeArray[i, j].horizontalVelocity;
                float y = j - dt0 * nodeArray[i, j].verticalVelocity;
                if (x < 0.5f)
                {
                    x = 0.5f;
                    //x = size - 1;
                }
                if (x > (size - 1) + 0.5f)
                {
                    x = (size - 1) + 0.5f;
                    // x = 0;
                }

                int i0 = (int)x;
                if (i0 >= size)
                {
                    i0 = size - 1;
                }
                int i1 = i0 + 1;
                if (i1 == size)
                {
                    i1 = 0;
                }

                if (y < 0.5f)
                {
                    y = 0.5f;
                    // y = size-1;
                }
                if (y > (size - 1) + 0.5f)
                {
                    y = (size - 1) + 0.5f;
                    // y = 0;
                }

                int j0 = (int)y;
                if (j0 >= size)
                {
                    j0 = size - 1;
                }
                int j1 = j0 + 1;
                //if j1 - size ==0
                if (j1 == size)
                {
                    j1 = 0;
                }
                float s1 = x - i0;
                float s0 = 1 - s1;
                float t1 = y - j0;
                float t0 = 1 - t1;

                //d
                //d0
                //u
                //v
                nodeArray[i, j].density = s0 * (t0 * nodeArray[i0, j0].previousDensity + t1 * nodeArray[i0, j1].previousDensity)
                    + s1 * (t0 * nodeArray[i1, j0].previousDensity + t1 * nodeArray[i1, j1].previousDensity);

            }
        }
    }

    private void DiffuseDensity()
    {
        float a = Time.deltaTime * diff * size * size;

        for (int k = 0; k < 20; k++)
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    int l = i - 1;
                    int m = i + 1;
                    int n = j - 1;
                    int o = j + 1;
                    if (i == 0)
                    {
                        l = size - 1;
                    }
                    if (i == size - 1)
                    {
                        m = 0;
                    }

                    if (j == 0)
                    {
                        n = size - 1;
                    }
                    if (j == size - 1)
                    {
                        o = 0;
                    }

                    nodeArray[i, j].density = (nodeArray[i, j].previousDensity + a * (nodeArray[l, j].density + nodeArray[m, j].density
                        + nodeArray[i, n].density + nodeArray[i, o].density)) * (1/(1 + 4 * a));
                }
            }
        }
    }


    #endregion
}
