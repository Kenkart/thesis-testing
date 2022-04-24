using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ViveInput : MonoBehaviour
{
    public PlayerReplay playerReplay;
    public BodySourceView bsv;

    public SteamVR_Action_Vibration hapticAction;

    private SteamVR_Action_Boolean grabPinch;
    private SteamVR_Action_Boolean grabGrib;
    private SteamVR_Action_Boolean teleport;
    //private SteamVR_Action_Vector2 trackpad;

    private bool isRecording;
    private float recordingTime;

    private static bool isPlaying;
    public float csvTime;

    private void Start()
    {
        grabPinch = SteamVR_Actions.default_GrabPinch;
        grabGrib = SteamVR_Actions.default_GrabGrip;
        teleport = SteamVR_Actions.default_Teleport;
    }

    void Update()
    {
        if (grabPinch.GetStateDown(SteamVR_Input_Sources.Any) && !isPlaying)
        {
            Debug.Log("Loading...");
            csvTime = 0;
            isPlaying = true;
            playerReplay.Load();
        }

        if (teleport.GetStateDown(SteamVR_Input_Sources.Any))
        {
            if (!isRecording)
            {
                Debug.Log("Recording...");
                isRecording = true;
            }
            else
            {
                Debug.Log("Saving...");
                isRecording = false;
                recordingTime = 0;
                playerReplay.Save();
                playerReplay.ResetRecording();
            }
        }

        if (grabGrib.GetStateDown(SteamVR_Input_Sources.Any))
        {
            
        }

        if (isRecording)
        {
            playerReplay.AddJoints(recordingTime);
            recordingTime += Time.deltaTime;
        }

        if (isPlaying)
        {
            csvTime += Time.deltaTime;
        }

        // TODO: Test haptic
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SteamVR_Actions.default_Haptic[SteamVR_Input_Sources.LeftHand].Execute(0, 1, 10, 1);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            SteamVR_Actions.default_Haptic[SteamVR_Input_Sources.RightHand].Execute(0, 1, 10, 1);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            SteamVR_Actions.default_Haptic[SteamVR_Input_Sources.LeftFoot].Execute(0, 1, 10, 1);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            SteamVR_Actions.default_Haptic[SteamVR_Input_Sources.RightFoot].Execute(0, 1, 10, 1);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            SteamVR_Action_Vibration[] actions = SteamVR_Input.actionsVibration;
            actions[0].Execute(0, 1, 10, 1, SteamVR_Input_Sources.RightFoot);
            Debug.Log("debug: " + actions.Length);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            hapticAction.Execute(0, 1, 10, 1, SteamVR_Input_Sources.LeftHand);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            hapticAction.Execute(0, 1, 10, 1, SteamVR_Input_Sources.RightHand);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            hapticAction.Execute(0, 1, 10, 1, SteamVR_Input_Sources.LeftFoot);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            hapticAction.Execute(0, 1, 10, 1, SteamVR_Input_Sources.RightFoot);
        }


    }

    public static IEnumerator WaitForControllerPress()
    {
        while (true)
        {
            if (SteamVR_Actions.default_GrabPinch.GetStateDown(SteamVR_Input_Sources.Any) || Input.GetKeyDown(KeyCode.Space))
            {
                break;
            }
            yield return null;
        }
    }

    public static void StopPlaying()
    {
        isPlaying = false;
    }
}
