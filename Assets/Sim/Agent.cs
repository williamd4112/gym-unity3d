using UnityEngine;
using System.Collections;

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public delegate void OnReceive(ref byte[] data);

public class Agent : MonoBehaviour {

    [SerializeField]
    private int m_AgentID = 0;
    [SerializeField]
    private int m_BufferSize = 5;
    [SerializeField]
    private SocketRawDataListener[] m_RawDataListeners;

    public event OnReceive m_OnReceiveEvents;

    private bool m_IsShutdown = false;

    private Thread m_MainThread;

	// Use this for initialization
	void Start () {
        m_MainThread = System.Threading.Thread.CurrentThread;
        foreach (SocketRawDataListener l in m_RawDataListeners)
        {
            m_OnReceiveEvents += l.OnReceiveRawData;
        }
        StartCoroutine(SubscribeAgentServer());
    }

    void OnClientConnect(AgentClient client)
    {
        Debug.Log("Client " + client + " connected with agent " + m_AgentID);
        client.AllocateBuffer(m_BufferSize);

        /* Make sure calling OnClientConnect in MainThread since Coroutine required */
        Debug.Assert(System.Threading.Thread.CurrentThread.Equals(m_MainThread));

        /* Start receive loop */
        StartCoroutine(ClientHandler(client));
    }

    void OnDestroy()
    {
        Debug.Log("Client diconnected with agent " + m_AgentID);
        AgentServer.Instance.UnsubscribeClient(m_AgentID, OnClientConnect);
        m_IsShutdown = true;
    }

    IEnumerator SubscribeAgentServer()
    {
        /* Wait for AgentServer instantce ready */
        while (AgentServer.Instance == null) {
            yield return new WaitForEndOfFrame();
        }

        /* Wait for AgentServer ready */
        while (!AgentServer.Instance.Ready) {
            yield return new WaitForEndOfFrame();
        }

        Debug.Log("Connect with AgentServer (" + AgentServer.Instance.Ready + ")");

        AgentServer.Instance.SubscribeClient(m_AgentID, OnClientConnect);
        Debug.Log("Agent " + m_AgentID + " subscribe AgentServer for client " + m_AgentID);

        yield return new WaitForEndOfFrame();
    }

    IEnumerator ClientHandler(AgentClient client)
    {
        client.DisconnectionEvents += (AgentClient c) => { m_IsShutdown = true; };

        Debug.Log("Agent " + m_AgentID + " start receiveing.");
        while (!m_IsShutdown)
        {
            yield return new WaitUntil(() => {
                try
                {
                    if (m_IsShutdown)
                    {
                        return true;
                    }
                    return (client.ClientSocket.Poll(1, SelectMode.SelectRead));
                }
                catch (SocketException)
                {
                    m_IsShutdown = true;
                    return true;
                }
            });
                
            if (!m_IsShutdown)
            {
                byte[] rawData = client.Receive();
                if (rawData != null)
                {
                    m_OnReceiveEvents.Invoke(ref rawData);
                }
            }
        }
        Debug.Log("Agent " + m_AgentID + " stop receiveing.");
    }
}
