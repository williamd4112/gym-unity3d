using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;

using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class RGBDDataFetcher : PostEffectsBase {

    [SerializeField]
    private Material m_DepthMaterial;

    [SerializeField]
    private RenderTexture m_ScreenTexture;
    [SerializeField]
    private RenderTexture m_DepthTexture;

    [SerializeField]
    private bool m_IsRenderToScreen = true;

    public int GetDepthTextureSize()
    {
        return m_DepthTexture.width * m_DepthTexture.height * Marshal.SizeOf(typeof(Color));
    }

    public int GetRGBTextureSize()
    {
        return m_ScreenTexture.width * m_ScreenTexture.height * Marshal.SizeOf(typeof(Color));
    }

    public void GetRGBObservation(ref byte[] bytes)
    {
        GetRenderTextureByteData(m_ScreenTexture, ref bytes);
    }

    public void GetDepthObservation(ref byte[] bytes)
    {
        GetRenderTextureByteData(m_DepthTexture, ref bytes);
    }

    void OnEnable()
    {
        GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth;
    }

    void OnDisable()
    {
        GetComponent<Camera>().depthTextureMode &= ~DepthTextureMode.Depth;
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (m_DepthMaterial)
        {
            Graphics.Blit(source, m_DepthTexture, m_DepthMaterial);
        }
        Graphics.Blit(source, m_ScreenTexture);

        if (m_IsRenderToScreen)
        {
            Graphics.Blit(source, destination);
        }
    }

    override public bool CheckResources()
    {
        return isSupported;
    }

    static public void GetRenderTextureByteData(RenderTexture renderTexture, ref byte[] bytes)
    {
        Texture2D texture2d = GetRTPixels(renderTexture);
        Color[] colors = texture2d.GetPixels(0, 0, renderTexture.width, renderTexture.height);

        ColorArrayToByteArray(colors, ref bytes);
    }

    static public Texture2D GetRTPixels(RenderTexture rt)
    {
        // Remember currently active render texture
        RenderTexture currentActiveRT = RenderTexture.active;

        // Set the supplied RenderTexture as the active one
        RenderTexture.active = rt;

        // Create a new Texture2D and read the RenderTexture image into it
        Texture2D tex = new Texture2D(rt.width, rt.height);
        tex.ReadPixels(new Rect(0, 0, tex.width, tex.height), 0, 0);

        // Restorie previously active render texture
        RenderTexture.active = currentActiveRT;
        return tex;
    }

    static public byte[] ColorArrayToByteArray(Color[] colors, ref byte[] bytes)
    {
        if (colors == null || colors.Length == 0)
            return null;

        int length = bytes.Length;

        GCHandle handle = default(GCHandle);
        try
        {
            handle = GCHandle.Alloc(colors, GCHandleType.Pinned);
            IntPtr ptr = handle.AddrOfPinnedObject();
            Marshal.Copy(ptr, bytes, 0, length);
        }
        finally
        {
            if (handle != default(GCHandle))
                handle.Free();
        }

        return bytes;
    }
}
