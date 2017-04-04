using UnityEngine;
using UnityEngine.Assertions;

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
    private int m_BufferSize = 12;

    [SerializeField]
    private SocketRawDataListener[] m_RawDataListeners;

    [SerializeField]
    private ObservationProducer m_ObservationProducer;
    [SerializeField]
    private RewardProducer m_RewardProducer;
    [SerializeField]
    private TerminationProducer m_TerminationProducer;

    public event OnReceive m_OnReceiveEvents;

    private bool m_IsShutdown = false;

    /* For Debugging (assure Coroutine starting at MainThread) */
    private Thread m_MainThread;

    private Coroutine m_SubscribeRoutine;
    private Coroutine m_ClientRoutine;

	// Use this for initialization
	void Start () {
        Assert.IsTrue(m_ObservationProducer != null);
        Assert.IsTrue(m_RewardProducer != null);
        Assert.IsTrue(m_TerminationProducer != null);

        m_MainThread = System.Threading.Thread.CurrentThread;
        foreach (SocketRawDataListener l in m_RawDataListeners)
        {
            m_OnReceiveEvents += l.OnReceiveRawData;
        }
        m_SubscribeRoutine = StartCoroutine(SubscribeAgentServer());
    }

    void OnClientConnect(AgentClient client)
    {
        Debug.Log("Client " + client + " connected with agent " + m_AgentID);
        m_IsShutdown = false;
        client.AllocateBuffer(m_BufferSize);

        /* Make sure calling OnClientConnect in MainThread since Coroutine required */
        Assert.IsTrue(System.Threading.Thread.CurrentThread.Equals(m_MainThread));

        /* Start receive loop */
        m_ClientRoutine = StartCoroutine(ClientHandler(client));
    }

    void OnDestroy()
    {
        Debug.Log("Client disconnected with agent " + m_AgentID);
        AgentServer.Instance.UnsubscribeClient(m_AgentID, OnClientConnect);
        m_IsShutdown = true;
    }

    IEnumerator SubscribeAgentServer()
    {
        /* Wait for AgentServer instantce ready */
        yield return new WaitUntil(() => { return AgentServer.Instance != null && AgentServer.Instance.Ready; });

        Debug.Log("Connect with AgentServer (" + AgentServer.Instance.Ready + ")");

        AgentServer.Instance.SubscribeClient(m_AgentID, OnClientConnect);
        Debug.Log("Agent " + m_AgentID + " subscribe AgentServer for client " + m_AgentID);

        StopCoroutine(m_SubscribeRoutine);
    }

    IEnumerator ClientHandler(AgentClient client)
    {
        client.DisconnectionEvents += (AgentClient c) => { m_IsShutdown = true; };

        byte[] sendBuffer = new byte[m_ObservationProducer.GetObservationSize() 
            + m_RewardProducer.GetRewardSize() 
            + m_TerminationProducer.GetTerminationSize()];

        Debug.Log("Agent " + m_AgentID + " start receiveing.");
        while (!m_IsShutdown)
        {
            /* Wait for socket ready */
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
                /* Receive data from client */
                byte[] rawData = client.Receive();
                if (rawData != null)
                {
                    m_OnReceiveEvents.Invoke(ref rawData);
                }

                /* Wait for next frame */
                yield return new WaitForEndOfFrame();

                /* Respond with Observation, Reward */
                byte[] observation = m_ObservationProducer.GetObservation();
                byte[] reward = m_RewardProducer.GetReward();
                byte[] done = m_TerminationProducer.GetTermination();

                System.Buffer.BlockCopy(observation, 0, sendBuffer, 0, observation.Length);
                System.Buffer.BlockCopy(reward, 0, sendBuffer, observation.Length, reward.Length);
                System.Buffer.BlockCopy(done, 0, sendBuffer, observation.Length + reward.Length, done.Length);
                client.Send(sendBuffer);
            }
        }
        Debug.Log("Agent " + m_AgentID + " stop receiveing.");
        StopCoroutine(m_ClientRoutine);
    }
}
