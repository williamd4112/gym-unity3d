using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class EventQueueMonoBehaviour : MonoBehaviour {

    private Queue<Action> m_EventQueue;

    protected void AddEvent(Action a)
    {
        lock (m_EventQueue)
        {
            m_EventQueue.Enqueue(a);
        }
    }

    // Use this for initialization
    protected virtual void Start () {
        m_EventQueue = new Queue<Action>();
    }

    // Update is called once per frame
    protected virtual void Update () {
        lock (m_EventQueue)
        {
            while (m_EventQueue.Count > 0)
            {
                Action e = m_EventQueue.Dequeue();
                e.Invoke();
            }
        }
    }
}
