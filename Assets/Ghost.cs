using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using Assets;
using UnityEngine;
using Random = System.Random;

public class Ghost : MonoBehaviour {


	// Use this for initialization
	void Start () {
		
	}


    public float interval = 1.0f;
    public float timer = 0.0f;
    public float lineSpeed = 7.0f;

	public float angleSpeed = Mathf.PI * 10;
	// Update is called once per frame
	void Update ()
	{
	    if (curPacket != null)
	    {
		    var dstHead = curPacket.headPos - curPacket.curPos;
		    transform.position = Vector3.MoveTowards(transform.position, curPacket.curPos, lineSpeed * Time.deltaTime);
		    transform.forward =
			    Vector3.RotateTowards(transform.forward, dstHead, angleSpeed * Time.deltaTime, angleSpeed * Time.deltaTime);
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
