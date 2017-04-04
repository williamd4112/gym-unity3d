using UnityEngine;
using System.Collections;

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class Agent : MonoBehaviour {

    private string AGENT_SERVER_TAG = "Server";

    [SerializeField]
    private int m_AgentID = 0;
    [SerializeField]
    private int m_BufferSize = 5;

    private Thread m_ClientReceiveThread;

	// Use this for initialization
	void Start () {
        StartCoroutine(SubscribeAgentServer());
    }
	
	// Update is called once per frame
	void Update () {

    }

    void OnClientConnect(AgentClient client)
    {
        Debug.Log("Client " + client + " connected with agent " + m_AgentID);
        client.AllocateBuffer(m_BufferSize);

        /* Start receive loop */
        startReceive(client);
    }

    void OnDestroy()
    {
        Debug.Log("Client diconnected with agent " + m_AgentID);
        AgentServer.Instance.UnsubscribeClient(m_AgentID, OnClientConnect);
    }

    void OnApplicationQuit()
    {
        if (m_ClientReceiveThread != null)
        {
            m_ClientReceiveThread.Abort();
        }
    }

    IEnumerator SubscribeAgentServer()
    {
        /* Wait for AgentServer instantce ready */
        while (AgentServer.Instance == null) {
            yield return new WaitForEndOfFrame();
        }

        /* Wait for AgentServer ready */
        while (!AgentServer.Instance.Ready)
        {
            yield return new WaitForEndOfFrame();
        }

        Debug.Log("Connect with AgentServer (" + AgentServer.Instance.Ready + ")");

        AgentServer.Instance.SubscribeClient(m_AgentID, OnClientConnect);
        Debug.Log("Agent " + m_AgentID + " subscribe AgentServer for client " + m_AgentID);

        yield return new WaitForEndOfFrame();
    }

    void startReceive(AgentClient client)
    {
        bool receving = true;
        client.DisconnectionEvents += (AgentClient c) => { receving = false; };

        m_ClientReceiveThread = new Thread(() =>
        {
            Debug.Log("Agent " + m_AgentID + " start receiveing.");
            while (receving)
            {
                byte[] rawData = client.Receive();
                if (rawData != null )
                {
                    string res = System.Text.Encoding.UTF8.GetString(rawData);
                    Debug.Log("Agent " + m_AgentID + " recieved : " + res);
                }
            }
            Debug.Log("Agent " + m_AgentID + " stop receiveing.");
        });
        m_ClientReceiveThread.IsBackground = true;
        m_ClientReceiveThread.Start();
    }
}
