using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using Debug = DefaultNamespace.Debug.Debug;

public class AudioSystemController : MonoBehaviour
{
    public List<string> allowedExtensions = new List<string> { ".mp3", ".wav", ".ogg", ".flac" };
    public List<AudioClip> bgmClips;

    private void Awake()
    {
        string bgmsFolderPath = Path.Combine(Application.streamingAssetsPath, "bgms");

        string[] audioFiles = Directory.GetFiles(bgmsFolderPath);

        for (int i = 0; i < audioFiles.Length; i++)
        {
            string filePath = audioFiles[i];
            string extension = Path.GetExtension(filePath).ToLower();

            // Check if the file has an allowed extension
            if (allowedExtensions.Contains(extension))
            {
                // Load the audio clip from the file
                StartCoroutine(LoadAudioClip(filePath, extension));
            }
        }
    }

    private IEnumerator LoadAudioClip(string filePath, string extension)
    {
        string url = "file://" + filePath;

        if (extension == ".flac")
        {
            WWW www = new WWW(url);

            yield return www;

            if (!string.IsNullOrEmpty(www.error))
            {
                Debug.LogError(www.error);
                yield break;
            }
            
            AudioClip audioClip = www.GetAudioClip();
            
            Debug.Log($"Loaded audio clip: {audioClip.name}---------------------------------------------");
            bgmClips.Add(audioClip);
        }
        else
        {
            using UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(
                url,
                GetAudioType(extension)
            );
            
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
                yield break;
            }

            AudioClip audioClip = DownloadHandlerAudioClip.GetContent(www);
            
            Debug.Log($"Loaded audio clip: {audioClip.name}---------------------------------------------");
            bgmClips.Add(audioClip);
        }
    }

    private AudioType GetAudioType(string extension)
    {
        switch (extension)
        {
            case ".mp3":
                return AudioType.MPEG;
            case ".wav":
                return AudioType.WAV;
            case ".ogg":
                return AudioType.OGGVORBIS;
            // case ".flac":
            //     return AudioType.;
            default:
                throw new NotSupportedException($"Unsupported audio format: {extension}");
        }
    }

    private void Start()
    {
    }
}