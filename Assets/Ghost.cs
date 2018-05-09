using System;
using System.Collections;
using System.Collections.Generic;
using Assets;
using UnityEngine;
using Random = System.Random;

public class Ghost : MonoBehaviour {


	// Use this for initialization
	void Start () {
		
	}


    public float interval = 1.0f;
    public float timer = 0.0f;
    public float speed = 7.0f;
	// Update is called once per frame
	void Update ()
	{
	    if (curPacket != null)
	    {
	        var dist = Time.deltaTime* speed;
	        var distV = transform.position - curPacket.curPos;
            transform.position = Vector3.Lerp(transform.position, curPacket.curPos, dist / distV.magnitude);
	        transform.LookAt(curPacket.headPos);
	    }
        
        
        var packet = Socket.Recv();
	    while (packet != null)
	    {
	        curPacket = packet;
	        packet = Socket.Recv();
        }
	}

    private Packet curPacket = null;
    void dealPacket(Packet motion)
    {
        transform.position = motion.curPos;
        transform.Translate(motion.movSpeed*Time.deltaTime);
        transform.LookAt(motion.headPos);
    }
}
