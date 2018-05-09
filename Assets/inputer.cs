using System;
using System.Collections;
using System.Collections.Generic;
using Assets;
using UnityEngine;
using Random = System.Random;

public class GaussianRNG
{
    private Random rnd = new Random();
    public double GetNormDist(double mean, double stdDev)
    {
        return mean + (RandNormDist() * stdDev);
    }

    public double RandNormDist()
    {
        var u = 0.0;
        var v = 0.0;
        var w = 0.0;
        var c = 0.0;
        do
        {
            u = rnd.NextDouble() * 2.0 - 1.0;
            v = rnd.NextDouble() * 2.0 - 1.0;
            w = u * u + v * v;
        } while (w==0.0||w>=1.0);

        c = Math.Sqrt((-2 * Math.Log(w)) / w);
        return u * c;
    }
}
public class inputer : MonoBehaviour
{

    public float speed = 7f;
    public Camera cam;

    public Vector3 moveDirection;
    public Vector3 headToPos;
    public Vector3 curPos;

    public float interval = 0.1f;
    private float timer = 0.0f;

    public double mean = 200;
    public double dev = 100;

    private GaussianRNG rnd = new GaussianRNG();

    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            var rndTime = rnd.GetNormDist(mean, dev);
            Debug.Log(rndTime);
        }
    }
    void Update()
    {
        curPos = transform.position;
        moveDir(out moveDirection);
        headTo(out headToPos);

        transform.position = curPos;
        transform.Translate(moveDirection * Time.deltaTime);
        transform.LookAt(headToPos);

        if (timeOut())
        {
            timer = 0;

            var rndTime = rnd.GetNormDist(mean,dev) / 1000;
            Socket.Send(new Packet
            {
                curPos = curPos,
                headPos = headToPos,
                movSpeed = moveDirection,
                dealTime = Time.time + rndTime
            });
        }
    }

    private bool timeOut()
    {
        timer += Time.deltaTime;
        if (timer < interval)
            return false;

        return true;
    }

    void OnDestroy()
    {
        Socket.Clear();
    }

    bool moveDir(out Vector3 moveDir)
    {
        moveDir = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            moveDir += Vector3.forward;
        }

        if (Input.GetKey(KeyCode.S))
        {
            moveDir += Vector3.back;
        }

        if (Input.GetKey(KeyCode.A))
        {
            moveDir += Vector3.left;
        }

        if (Input.GetKey(KeyCode.D))
        {
            moveDir += Vector3.right;
        }

        moveDir.Normalize();
        moveDir *= speed;
        return moveDir != Vector3.zero;
    }

    bool headTo(out Vector3 hDir)
    {
        hDir = cam.ScreenToWorldPoint(Input.mousePosition);
        hDir.y = 0;

        return hDir != Vector3.zero;
    }
}
