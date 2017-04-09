using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public delegate void OnClientDisconnectEvent(AgentClient client);
public delegate void OnClientConnectEvent(AgentClient client);

public class ClientException : Exception { }

/// <summary>
/// Handling socket receive, send, ondisconnect
/// </summary>
public class AgentClient
{
    private int m_ClientId;
    private Socket m_Socket;
    private bool m_Active;
    private byte[] m_Buffer = null;

    public bool Active { get { return m_Active; } }
    public int ClientId { get { return m_ClientId; } }
    public Socket ClientSocket { get { return m_Socket; } }

    public event OnClientDisconnectEvent DisconnectionEvents;

    public AgentClient()
    {
        m_ClientId = -1;
        m_Socket = null;
        m_Active = false;
    }

    public AgentClient(int clientId, Socket socket)
    {
        m_ClientId = clientId;
        m_Socket = socket;
        m_Active = true;
    }

    public void AllocateBuffer(int size)
    {
        m_Buffer = new byte[size];
    }

    public byte[] Receive()
    {
        Debug.Assert(m_Buffer != null);
        int recv_byte = 0;
        try
        {
            recv_byte = m_Socket.Receive(m_Buffer);
            if (recv_byte == 0)
            {
                throw new SocketException();
            }
        }
        catch (SocketException e)
        {
            handleSockException(e);
        }
        return (recv_byte == 0) ? null : m_Buffer;
    }

    public void Send(byte[] buffer)
    {
        try
        {
            m_Socket.Send(buffer);
        }
        catch (SocketException e)
        {
            handleSockException(e);
        }
    }

    public void Close()
    {
        m_Socket.Close();
    }

    public override string ToString()
    {
        return m_Socket.RemoteEndPoint.ToString();
    }

    private void handleSockException(SocketException e)
    {
        bool old_active = m_Active;
        m_Active = false;
        if (old_active && !m_Active)
        {
            DisconnectionEvents.Invoke(this);
        }
    }
}

public class AgentServer : EventQueueMonoBehaviour {

    private static AgentServer instance = null;
    public static AgentServer Instance { get { return instance; } }

    private bool m_Ready = false;
    public bool Ready { get { return m_Ready; } }

    private Thread m_ClientListenerThread;

    private TcpListener m_TcpListener;

    [SerializeField]
    private int m_Port = 821;
    [SerializeField]
    private int m_MaxClients = 1;

    private AgentClient[] m_Clients;
    private OnClientConnectEvent[] m_ClientConnectEvents;
    private Semaphore m_AvailableClientSlot;

    public void SubscribeClient(int id, OnClientConnectEvent e)
    {
        Debug.Assert(id < m_MaxClients);
        m_ClientConnectEvents[id] += e;

        if (m_Clients[id].Active)
        {
            m_ClientConnectEvents[id].Invoke(m_Clients[id]);
        }
    }

    public void UnsubscribeClient(int id, OnClientConnectEvent e)
    {
        Debug.Assert(id < m_MaxClients);
        m_ClientConnectEvents[id] -= e;
    }

    void OnApplicationQuit()
    {
        m_TcpListener.Stop();
        m_ClientListenerThread.Abort();
        foreach (AgentClient c in m_Clients)
        {
            if (c.Active)
            {
                c.Close();
            }
        }
    }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            DestroyImmediate(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
		m_Port = Convert.ToInt32(GetArg ("port")) ;
    }

    override protected void Start () {
        base.Start();
        if (Application.isEditor)
        {
            Application.runInBackground = true;
        }

        m_Clients = new AgentClient[m_MaxClients];
        m_ClientConnectEvents = new OnClientConnectEvent[m_MaxClients];
        for (int i = 0; i < m_MaxClients; i++)
        {
            m_Clients[i] = new AgentClient();
        }

        m_AvailableClientSlot = new Semaphore(m_MaxClients, m_MaxClients);
        m_ClientListenerThread = new Thread(new ThreadStart(listenClient));
        m_ClientListenerThread.IsBackground = true;
        m_ClientListenerThread.Start();
        m_Ready = true;
    }

    void listenClient()
    {
        m_TcpListener = new TcpListener(IPAddress.Any, m_Port);
        m_TcpListener.Start();

        while (true)
        {
            m_AvailableClientSlot.WaitOne();

            Debug.Log("Listening connections...");
            Socket socket = m_TcpListener.AcceptSocket();
            Debug.Log("Connection from " + socket.RemoteEndPoint);
			Debug.Log ("Port :" + m_Port);
            AddEvent(() => {
                int empty_id = findEmptyClientSlot();
                Debug.Log("Allocate slot " + empty_id);

                /* Should not be -1, since wait AvaialableClientSlot */
                Debug.Assert(empty_id != -1);

                /* Set new client in empty slot */
                m_Clients[empty_id] = new AgentClient(empty_id, socket);
                m_Clients[empty_id].DisconnectionEvents += OnClientDisconnect;
                if (m_ClientConnectEvents != null)
                {
                    m_ClientConnectEvents[empty_id].Invoke(m_Clients[empty_id]);
                }
                Debug.Log("Client " + socket.RemoteEndPoint + " connected. [Agent " + empty_id + "]");
            });
        }
    }

    private int findEmptyClientSlot()
    {
        for (int i = 0; i < m_Clients.Length; i++)
        {
            if (!m_Clients[i].Active)
            {
                return i;
            }
        }
        return -1;
    }

    void OnClientDisconnect(AgentClient client)
    {
        //Debug.Log("Client " + client.ClientSocket.RemoteEndPoint + " disconnected.");
		Debug.Log("OnClientDisconnect");
        m_AvailableClientSlot.Release();
    }
	private static string GetArg(string name)
	{
		var args = System.Environment.GetCommandLineArgs();
		for (int i = 0; i < args.Length; i++)
		{
			if (args[i] == name && args.Length > i + 1)
			{
				return args[i + 1];
			}
		}
		return null;
	}

}
