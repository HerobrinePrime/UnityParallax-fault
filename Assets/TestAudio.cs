using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using UnityEngine;
using CSCore;
using CSCore.SoundIn;
using CSCore.Codecs.WAV;
using CSCore.CoreAudioAPI;
using DefaultNamespace;
using DG.Tweening;
using PimDeWitte.UnityMainThreadDispatcher;
using Debug = UnityEngine.Debug;


public class TestAudio : MonoBehaviour
{
    private bool _isRunning = true;
    public AudioSource audioSource;
    public List<string> ignoredProcessNames;

    private static TestAudio _instance;

    public static TestAudio Instance => _instance;

    void Start()
    {
        // StartCoroutine(TestAudiosadf());
        // TestAudioPlaying();
        _instance = this;

        StartTest();
    }

    // IEnumerator TestAudiosadf()
    // {
    //     while (true)
    //     {
    //         Debug.Log("TestAudio");
    //         Thread t = new Thread(Run);
    //         t.Start();
    //         yield return new WaitForSeconds(1);
    //     }
    // }

    private void StartTest()
    {
        _deviceEnumerator = new MMDeviceEnumerator();
        _device = _deviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);

        Thread t = new Thread(() =>
        {
            while (_isRunning)
            {
                if (!(ApplicationSetting.IsMuted || UIController.muted))
                {
                    GraduallyMuteAudio(TestAudioPlaying());
                }
                else
                {
                    Debug.Log("Audio is muted");
                }
                Thread.Sleep(1000);
            }
        });
        t.Name = "TestAudioPlaying";
        t.IsBackground = true;
        t.SetApartmentState(ApartmentState.MTA);
        t.Start();

        // Process[] processes = Process.GetProcesses();
        // foreach (Process process in processes)
        // {
        //     Debug.Log(process.ProcessName);
        // }
    }

    /*
     * TODO: Test when device is changed, if it still works or not.
     */
    private MMDeviceEnumerator _deviceEnumerator;
    private MMDevice _device;
    private AudioSessionManager2 _sessionManager;

    public bool TestAudioPlaying()
    {
        _sessionManager = AudioSessionManager2.FromMMDevice(_device);
        var sessions = _sessionManager.GetSessionEnumerator();
        Debug.Log("--------------------------------------------------------------");
        foreach (var session in sessions)
        {
            using (session)
            {
                var control = session.QueryInterface<AudioSessionControl2>();

                float peakValue = session.QueryInterface<AudioMeterInformation>().PeakValue;
                try
                {
                    var process = control.Process;
                    if (process == null || control.ProcessID == 0)
                    {
                        Debug.Log(control.ProcessID + " - " + "System - release");
                        continue;
                    }

                    ;


                    if (ignoredProcessNames.Contains(process.ProcessName.ToLower()))
                    {
                        Debug.Log(process.Id + " - " + process.ProcessName + " - " + peakValue + " - ignored");
                        continue;
                    }

                    if (peakValue > 0.01f)
                    {
                        Debug.Log(process.Id + " - " + process.ProcessName + " - " + peakValue + " - playing");
                        return true;
                    }
                    else
                    {
                        Debug.Log(process.Id + " - " + process.ProcessName + " - " + peakValue + " - release");
                    }
                }
                catch (Exception e)
                {
                    Debug.Log(e.Message);
                }
            }
        }

        Debug.Log("--------------------------------------------------------------");
        return false;
    }

    public void GraduallyMuteAudio(bool mute)
    {
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            // audioSource.volume = mute ? 0 : 1;
            audioSource.DOFade(mute ? 0 : UIController.volume, 0.5f);
        });
    }

    private void OnDestroy()
    {
        _sessionManager?.Dispose();
        _device?.Dispose();
        _deviceEnumerator?.Dispose();
        _isRunning = false;
    }

    private void OnApplicationQuit()
    {
        _isRunning = false;
    }
}