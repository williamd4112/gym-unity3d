using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;


[RequireComponent(typeof(RGBDDataFetcher))]
public class RGBObservationProducer : ObservationProducer {

    private byte[] m_RGBBuffer;
    private byte[] m_DepthBuffer;

    private RGBDDataFetcher m_RGBDataFetcher;
    private Mutex m_ObservationMutex;

    public override byte[] GetObservation()
    {
        m_ObservationMutex.WaitOne();
        m_RGBDataFetcher.GetRGBObservation(ref m_RGBBuffer);
        m_ObservationMutex.ReleaseMutex();

        return m_RGBBuffer;
    }

    void Start ()
    {
        m_ObservationMutex = new Mutex();
        m_RGBDataFetcher = GetComponent<RGBDDataFetcher>();
        m_RGBBuffer = new byte[m_RGBDataFetcher.GetRGBTextureSize()];
        m_DepthBuffer = new byte[m_RGBDataFetcher.GetDepthTextureSize()];
	}
	
}
