using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;


[RequireComponent(typeof(RGBDDataFetcher))]
public class RGBObservationProducer : ObservationProducer {

    private byte[] m_RGBBuffer;

    private RGBDDataFetcher m_RGBDataFetcher;

    public override byte[] GetObservation()
    {
        m_RGBDataFetcher.GetRGBObservation(ref m_RGBBuffer);
        return m_RGBBuffer;
    }

    public override void GetObservation(out byte[] buffer)
    {
        buffer = m_RGBBuffer;
        m_RGBDataFetcher.GetRGBObservation(ref buffer);
    }

    public override int GetObservationSize()
    {
        return m_RGBDataFetcher.GetRGBTextureSize();
    }

    void Start ()
    {
        m_RGBDataFetcher = GetComponent<RGBDDataFetcher>();
        m_RGBBuffer = new byte[m_RGBDataFetcher.GetRGBTextureSize()];
	}
	
}
