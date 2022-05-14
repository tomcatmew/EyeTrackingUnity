using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Linq;
using System.Text;
using GazeFlowAPI;
using System;

public class TCPConnection : MonoBehaviour
{
    public Transform UICircle;

    public Image panel;
    public Image needle;
    public Image top;
    public Text label;

    public Material front_mirror;
    public Material side_mirror;

    private Color mirrorTransparent;
    private Color mirrorOpaque;

    private Color UITransparent;
    private Color UIllOpaque;

    private float panelTime = 0.0f;
    private float frontMirrorTime = 0.0f;
    private float sideMirrorTime = 0.0f;

    CGazeFlowAPI gazeFlowAPI = new CGazeFlowAPI();
    private float gazeX = 0.0f;
    private float gazeY = 0.0f;

    private float newUIX = 0.0f;
    private float newUIY = 0.0f;

    float scale(float OldMin, float OldMax, float NewMin, float NewMax, float OldValue)
    {

        float OldRange = (OldMax - OldMin);
        float NewRange = (NewMax - NewMin);
        float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;

        return (NewValue);
    }

    bool EyeContactSideMirror()
    {
        if ((0 < newUIX) && (newUIX < 346) && (193 < newUIY) && (newUIY < 694))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    bool EyeContactBotPanel()
    {
        if ((615 < newUIX) && (newUIX < 1228) && (0 < newUIY) && (newUIY < 386))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    bool EyeContactFrontMirror()
    {
        if ((1459 < newUIX) && (newUIX < 1920) && (696 < newUIY) && (newUIY < 888))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    bool EyeResetArea()
    {
        if ((560 < newUIX) && (newUIX < 1360) && (400 < newUIY) && (newUIY < 980))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void Start()
    {
        mirrorTransparent = new Color(front_mirror.color.r, front_mirror.color.g, front_mirror.color.b, 0.0f);
        mirrorOpaque = new Color(front_mirror.color.r, front_mirror.color.g, front_mirror.color.b, 1.0f);

        UITransparent = new Color(panel.color.r, panel.color.g, panel.color.b, 0.0f);
        UIllOpaque = new Color(panel.color.r, panel.color.g, panel.color.b, 1.0f);

        panel.color = UITransparent;
        needle.color = UITransparent;
        top.color = UITransparent;
        label.color = UITransparent;
        front_mirror.color = mirrorTransparent;
        side_mirror.color = mirrorTransparent;
        //To get your AppKey register at http://gazeflow.epizy.com/GazeFlowAPI/register/
        string AppKey = "AppKeyTrial";
        gazeFlowAPI.Connect("127.0.0.1", 43333, AppKey);
        StartCoroutine(TCPRoutine());
    }

    IEnumerator TCPRoutine()
    {
        while (true)
        {
            CGazeData GazeData = gazeFlowAPI.ReciveGazeDataSyn();
            if (GazeData == null)
            {
                Debug.Log("Disconected");
                yield return null;
            }
            else
            {
                gazeX = GazeData.GazeX;
                gazeY = GazeData.GazeY;

                newUIX = scale(0, 1920, 0, 1920, gazeX);
                newUIY = scale(0, 1080, 1080, 0, gazeY);
                //Debug.Log(String.Format("Gaze: {0} , {1}", newUIX, newUIY));
                Vector3 new_pos = new Vector3(newUIX, newUIY, 0);
                UICircle.position = new_pos;
                //UICircle.position = Vector3.Lerp(UICircle.position, new_pos, Time.deltaTime);
                //Debug.Log(String.Format("Gaze: {0} , {1}", GazeData.GazeX, GazeData.GazeY));
                //Debug.Log(String.Format("Head: {0} , {1}, {2}", GazeData.HeadX, GazeData.HeadY, GazeData.HeadZ));
                //Debug.Log(String.Format("Head rot : {0} , {1}, {2}", GazeData.HeadYaw, GazeData.HeadPitch, GazeData.HeadRoll));
                //Debug.Log("");
            }
            yield return new WaitForSeconds(0.2f);
        }
    }

    void Update()
    {
        /*
        CGazeData GazeData = gazeFlowAPI.ReciveGazeDataSyn();
        if (GazeData == null)
        {
            Debug.Log("Disconected");
            return;
        }
        else
        {
            gazeX = GazeData.GazeX;
            gazeY = GazeData.GazeY;

            newUIX = scale(0, 1920, 0, 1920, gazeX);
            newUIY = scale(0, 1080, 1080, 0, gazeY);
            //Debug.Log(String.Format("Gaze: {0} , {1}", newUIX, newUIY));
            Vector3 new_pos = new Vector3(newUIX, newUIY, 0);
            UICircle.position = new_pos;
            //UICircle.position = Vector3.Lerp(UICircle.position, new_pos, Time.deltaTime);
            //Debug.Log(String.Format("Gaze: {0} , {1}", GazeData.GazeX, GazeData.GazeY));
            //Debug.Log(String.Format("Head: {0} , {1}, {2}", GazeData.HeadX, GazeData.HeadY, GazeData.HeadZ));
            //Debug.Log(String.Format("Head rot : {0} , {1}, {2}", GazeData.HeadYaw, GazeData.HeadPitch, GazeData.HeadRoll));
            //Debug.Log("");
        }
        */
        if (EyeContactBotPanel())
        {
            panelTime += Time.deltaTime;
            if (panelTime > 1)
                panelTime = 1;
            Color PanelColor = Color.Lerp(UITransparent, UIllOpaque, panelTime);
            Debug.Log(panelTime);
            panel.color = PanelColor;
            needle.color = PanelColor;
            top.color = PanelColor;
            label.color = PanelColor;
        }
        /*
        else
        {
            panelTime -= Time.deltaTime;
            if (panelTime < 0)
                panelTime = 0;
            Color PanelColor = Color.Lerp(UITransparent, UIllOpaque, panelTime);
            panel.color = PanelColor;
            needle.color = PanelColor;
            top.color = PanelColor;
            label.color = PanelColor;
        }
        */
        if (EyeContactSideMirror())
        {
            sideMirrorTime += Time.deltaTime;
            if (sideMirrorTime > 1)
                sideMirrorTime = 1;
            Color SideMirrorColor = Color.Lerp(mirrorTransparent, mirrorOpaque, sideMirrorTime);
            side_mirror.color = SideMirrorColor;
        }
        /*
        else
        {
            sideMirrorTime -= Time.deltaTime;
            if (sideMirrorTime < 0)
                sideMirrorTime = 0;
            Color SideMirrorColor = Color.Lerp(mirrorTransparent, mirrorOpaque, sideMirrorTime);
            side_mirror.color = SideMirrorColor;
        }
        */
        if (EyeContactFrontMirror())
        {
            frontMirrorTime += Time.deltaTime;
            if (frontMirrorTime > 1)
                frontMirrorTime = 1;
            Color FrontMirrorColor = Color.Lerp(mirrorTransparent, mirrorOpaque, frontMirrorTime);
            front_mirror.color = FrontMirrorColor;
        }
        /*
        else
        {
            frontMirrorTime -= Time.deltaTime;
            if (frontMirrorTime < 0)
                frontMirrorTime = 0;
            Color FrontMirrorColor = Color.Lerp(mirrorTransparent, mirrorOpaque, frontMirrorTime);
            front_mirror.color = FrontMirrorColor;
        }
        */
        if (EyeResetArea())
        {
            panelTime -= Time.deltaTime;
            sideMirrorTime -= Time.deltaTime;
            frontMirrorTime -= Time.deltaTime;
            if (panelTime < 0)
                panelTime = 0;
            if (sideMirrorTime < 0)
                sideMirrorTime = 0;
            if (frontMirrorTime < 0)
                frontMirrorTime = 0;
            Color PanelColor = Color.Lerp(UITransparent, UIllOpaque, panelTime);
            Color SideColor = Color.Lerp(mirrorTransparent, mirrorOpaque, sideMirrorTime);
            Color FrontColor = Color.Lerp(mirrorTransparent, mirrorOpaque, frontMirrorTime);
            panel.color = PanelColor;
            needle.color = PanelColor;
            top.color = PanelColor;
            label.color = PanelColor;
            side_mirror.color = SideColor;
            front_mirror.color = FrontColor;
        }

    }
    
}
