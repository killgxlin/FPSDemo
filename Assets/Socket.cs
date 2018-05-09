using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets
{
    public class Packet
    {
        public Vector3 movSpeed;
        public Vector3 headPos;
        public Vector3 curPos;
        public double dealTime;
    }

    public class Socket
    {
        public static List<Packet> Queue = new List<Packet>();

        public static void Send(Packet motion)
        {
            Queue.Add(motion);
        }

        public static Packet Recv()
        {
            if (Queue.Count == 0)
                return null;

            var head = Queue[0];
            if (head.dealTime > Time.time)
                return null;

            Queue.RemoveAt(0);
            return head;
            
        }

        public static void Clear()
        {
            Queue.Clear();
        }
    }
}
