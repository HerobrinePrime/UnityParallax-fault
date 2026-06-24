using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using Utils;

public class AudioSystemController : MonoBehaviour
{
    public AudioSource AudioSource;

    public List<string> allowedExtensions = new List<string> { ".mp3", ".wav", ".ogg", ".flac" };
    public List<AudioClip> bgmClips = new List<AudioClip>();
    private List<AudioClip> _tempAudioList = new List<AudioClip>();
    private List<string> newAddedAudioList = new List<string>();
    private List<string> deletedAudioList = new List<string>();
    private int allowedAudioCount = 0;

    private void Awake()
    {
        //Do I need rescan folder when open UI?
        string bgmsFolderPath = Path.Combine(Application.streamingAssetsPath, "bgms");

        string[] audioFiles = Directory.GetFiles(bgmsFolderPath);

        for (int i = 0; i < audioFiles.Length; i++)
        {
            string filePath = audioFiles[i];
            string extension = Path.GetExtension(filePath).ToLower();

            if (allowedExtensions.Contains(extension))
            {
                allowedAudioCount++;
                StartCoroutine(LoadAudioClip(filePath, extension));
            }
        }
    }

    private void Start()
    {
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
            audioClip.name = Path.GetFileName(filePath);
            // Debug.Log($"Loaded audio clip: {audioClip.name}---------------------------------------------");
            // bgmClips.Add(audioClip);
            _tempAudioList.Add(audioClip);
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
            audioClip.name = Path.GetFileName(filePath);
            // Debug.Log($"Loaded audio clip: {audioClip.name}---------------------------------------------");
            // bgmClips.Add(audioClip);
            _tempAudioList.Add(audioClip);
        }

        if (_tempAudioList.Count == allowedAudioCount)
        {
            Debug.Log("Audio loaded");
            CompareAudioList();
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

    private void CompareAudioList()
    {
        List<string> playerPrefAudioList = new List<string>(PlayerSettingPref.Instance.AudioSettings.AudioList);
        List<AudioClip> newAudios = new List<AudioClip>();

        foreach (AudioClip clip in _tempAudioList)
        {
            int index = playerPrefAudioList.FindIndex((listedAudioName) =>
            {
                return listedAudioName.Equals(clip.name);
            });
            if (index != -1)
            {
                playerPrefAudioList.RemoveAt(index);
                bgmClips.Add(clip);
            }
            else
            {
                newAudios.Add(clip);
            }
        }

        if (playerPrefAudioList.Count != 0)
        {
            deletedAudioList = playerPrefAudioList;
            Debug.Log("Audio deleted:");
            foreach (var se in deletedAudioList)
            {
                Debug.Log(se);
            }
        }

        if (newAudios.Count != 0)
        {
            newAddedAudioList = newAudios.Select(x => x.name).ToList();
            Debug.Log("Audio added:");
            foreach (var se in newAddedAudioList)
            {
                Debug.Log(se);
            }
        }


        bgmClips.AddRange(newAudios);
        PlayerSettingPref.Instance.AudioSettings.AudioList = bgmClips.Select(clip => clip.name).ToList();

        Debug.Log("Audio Count: " + bgmClips.Count);
        Debug.Log("Allowed Audio Count: " + allowedAudioCount);
    }
}