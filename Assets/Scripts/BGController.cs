using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Entity;
using Enum;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEditor;
using UnityEngine.Timeline;
using Screen = UnityEngine.Device.Screen;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

public class BGController : MonoBehaviour
{
    public Layer[] layers;

    // public Transform[] layers;
    private List<Vector2> _layerRatios = new List<Vector2>();
    public float parallaxScale = 1f;
    public Transform scale;

    // public InputActionProperty mouseActionProperty;
    // private InputAction _mouseAction;
    // public bool fullBackgroundUsed = false;
    public float verticleConstraint = 1f;
    public float horizontalConstraint = 1f;

    public bool reverse = false;
    public float damping = 0.5f;
    public AnimationCurve dampingCurve;

    private Vector2 _lastSize = Vector2.zero;
    private float _scale => scale.localScale.x;

    private Vector2 _screenCenter;

    private Vector2 _lastMousePosition = Vector2.zero;

    public LayerTexture[] layerSeasonTimeTextures;
    private Dictionary<LayerType, Dictionary<Season, Dictionary<TimeOfDay, Texture2D>>> _layerSeasonTimeTextureMap;
    private Dictionary<LayerType, TransitionMaterial> _layerMaterialMap;

    void Start()
    {
        ReScale();
        ReSizeCamera();

        // _mouseAction = mouseActionProperty.action;
        _screenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);

        RecordScreenInfo();

        _layerSeasonTimeTextureMap = layerSeasonTimeTextures.ToDictionary(
            x => x.layerType,
            x => x.seasonTextures.ToDictionary(
                y => y.season,
                y => y.timeTextures.ToDictionary(
                    z => z.time,
                    z => z.timeTexture
                )
            )
        );
        _layerMaterialMap = layerSeasonTimeTextures.ToDictionary(
            x => x.layerType,
            x => new TransitionMaterial(x.material)
        );

        // var a = _layerSeasonTimeTextureMap[LayerType.bg][Season.Autumn][TimeOfDay.Day];
        // var m = _layerMaterialMap[LayerType.bg];
        // m.SetFloat("_time",1f);
        // Debug.Log(_layerMaterialMap[LayerType.bg].time);
        // Debug.Log(_layerMaterialMap[LayerType.bg].textureBefore);
        // Debug.Log(_layerMaterialMap[LayerType.bg].textureAfter);
        // _layerMaterialMap[LayerType.bg].time = 1;
        // _layerMaterialMap[LayerType.bg].textureAfter = _layerMaterialMap[LayerType.bg].textureBefore;

        for (int i = 0; i < layers.Length; i++)
        {
            // _currentCoroutines[i] = null;
            _tweensDictionary[i] = null;
        }
    }

    void Update()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();

        MoveToPosition(mousePosition);
    }

    void MoveToPosition(Vector2 mousePosition)
    {
        if (mousePosition == _lastMousePosition) return;
        _lastMousePosition = mousePosition;

        mousePosition.x = Mathf.Clamp(mousePosition.x, 0, Screen.width);
        mousePosition.y = Mathf.Clamp(mousePosition.y, 0, Screen.height);

        Vector2 screenOffset = mousePosition - _screenCenter;
        screenOffset *= new Vector2(horizontalConstraint, verticleConstraint);
        if (reverse) screenOffset = -screenOffset;

        foreach (var (layer, index) in layers.Select((layer, index) => (layer, index)))
        {
            // layer.transform.position = screenOffset * _layerRatios[index];

            // if (_currentCoroutines[index] != null)
            // {
            //     StopCoroutine(_currentCoroutines[index]);
            // }
            // _currentCoroutines[index] =
            //     StartCoroutine(DampToPosition(layer.transform, screenOffset * _layerRatios[index]));

            if (_tweensDictionary[index] != null)
            {
                _tweensDictionary[index].Kill();
            }

            var scaledScreenOffset = screenOffset * _layerRatios[index];
            _tweensDictionary[index] =
                layer.transform.DOMove(
                    new Vector3(scaledScreenOffset.x, scaledScreenOffset.y, layer.transform.position.z), damping,
                    false);
        }
    }

    private Dictionary<int, TweenerCore<Vector3, Vector3, VectorOptions>> _tweensDictionary = new();

    // private Dictionary<int, Coroutine> _currentCoroutines = new Dictionary<int, Coroutine>();
    // IEnumerator DampToPosition(Transform layer, Vector3 targetPosition)
    // {
    //     float time = 0f;
    //     while (time < damping)
    //     {
    //         time += Time.deltaTime;
    //         float value = dampingCurve.Evaluate(time / damping);
    //         layer.position = Vector3.Lerp(layer.position, targetPosition, value);
    //         yield return null;
    //     }
    //
    //     yield return null;
    // }

    void RecordScreenInfo()
    {
        _layerRatios.Clear();
        // RecordScreenInfo(layers[2]);

        foreach (Layer layer in layers)
        {
            _layerRatios.Add(RecordScreenInfoByFullBackground(layer.transform));
        }
        // if (fullBackgroundUsed)
        // {
        //     foreach (Layer layer in layers)
        //     {
        //         _layerRatios.Add(RecordScreenInfoByFullBackground(layer.transform));
        //     }
        // }
        // else
        // {
        //     Layer layer0 = layers[0];
        //     foreach (Layer layer in layers)
        //     {
        //         float distance = layer0.transform.localPosition.z - layer.transform.localPosition.z;
        //         _layerRatios.Add((distance + 1) * new Vector2(horizontalConstraint, verticleConstraint));
        //     }
        // }
    }

    Vector2 RecordScreenInfoByFullBackground(Transform layer)
    {
        Camera mainCamera = Camera.main!;

        Transform bgLayer = layer;
        MeshRenderer[] bgMeshRenderers = bgLayer.GetComponentsInChildren<MeshRenderer>();
        Bounds bgBounds = new Bounds();
        bool isFirst = true;
        foreach (MeshRenderer meshRenderer in bgMeshRenderers)
        {
            if (isFirst)
            {
                bgBounds = meshRenderer.bounds;
                isFirst = false;
            }
            else
            {
                bgBounds.Encapsulate(meshRenderer.bounds);
            }
        }

        // Debug.DrawLine(bgBounds.min, bgBounds.max, Color.red, 60f);

        Vector3 bgMaxPoint = bgBounds.max;
        Vector3 cameraMaxPoint = mainCamera.ViewportToWorldPoint(new Vector3(1f, 1f, 0));
        float xMaxDistance = bgMaxPoint.x - cameraMaxPoint.x;
        float yMaxDistance = bgMaxPoint.y - cameraMaxPoint.y;
        Debug.Log("xMaxDistance: " + xMaxDistance + " yMaxDistance: " + yMaxDistance);
        var xRatio = xMaxDistance * 2 / Screen.width;
        var yRatio = yMaxDistance * 2 / Screen.height;
        Debug.Log("_xRatio: " + xRatio + " _yRatio: " + yRatio);

        /*// Ray ray = mainCamera.ViewportPointToRay(new Vector3(1f, 1f, 0));
        // Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, LayerMask.GetMask("bg"));
        // Debug.DrawLine(ray.origin, hitInfo.point, Color.red, 100f);
        // float xMaxDistance2 = bgBounds.max.x - hitInfo.point.x;
        // float yMaxDistance2 = bgBounds.max.y -hitInfo.point.y;
        // Debug.Log("xMaxDistance2: " + xMaxDistance2 + " yMaxDistance2: " + yMaxDistance2);
        // _xRatio = xMaxDistance2 * 2 / Screen.width;
        // _yRatio = yMaxDistance2 * 2 / Screen.height;
        // Debug.Log("_xRatio: " + _xRatio + " _yRatio: " + _yRatio);*/

        return new Vector2(xRatio, yRatio);
    }

    Vector2 RecordScreenInfoByConstaint(Vector2 constraint)
    {
        return constraint;
    }

    void ReScale()
    {
        Debug.Log("Re-scaling background layers");
        Transform layer0 = layers[0].transform;
        foreach (Layer layer in layers)
        {
            if (layer0 == layer.transform) continue;
            float distance = layer0.localPosition.z - layer.transform.localPosition.z;
            // Debug.Log(distance);
            layer.transform.localScale = (1 + distance * parallaxScale) * layer0.localScale;
        }
    }

    void ReSizeCamera()
    {
        Debug.Log("Resizing camera");
        Vector2 size = Handles.GetMainGameViewSize();
        if ((size != _lastSize) && (layers != null && layers.Length > 0))
            // if (true)
        {
            Camera mainCamera = Camera.main!;

            Transform layer0 = layers[0].transform;
            MeshRenderer meshRenderer = layer0.GetComponentInChildren<MeshRenderer>();
            Bounds bounds = meshRenderer.bounds;
            // Debug.Log(bounds.extents);
            // Debug.DrawLine(bounds.min, bounds.max, Color.red, 10f);
            // Debug.DrawLine(bounds.min, bounds.min + Vector3.right * bounds.size.x, Color.green, 10f);


            Vector3 center = bounds.center;
            Vector3 p1 = mainCamera.WorldToScreenPoint(center + bounds.size.x * Vector3.right);
            Vector3 p0 = mainCamera.WorldToScreenPoint(center);
            float boundScreenViewWidth = (p1.x - p0.x);
            float screenWidthAfterScale = Screen.width * _scale;
            // Debug.Log("Bound screen view width: " + boundScreenViewWidth + " \nScreen width after scale: " + screenWidthAfterScale);
            if (boundScreenViewWidth >= screenWidthAfterScale)
            {
                Debug.Log("width : height < 16 : 10 ");
                return;
            }

            Debug.Log("width : height > 16 : 10 ");
            float screenWidthRadio = boundScreenViewWidth / screenWidthAfterScale;
            Debug.Log(screenWidthRadio);
            mainCamera.orthographicSize *= screenWidthRadio;

            // float halfWidth = bounds.extents.x;
            // Vector3 centerPointOnMaxRightEdge = center + Vector3.right * halfWidth;
            // Vector3 centerPointOnMaxLeftEdge = center + Vector3.left * halfWidth;
            // Debug.DrawLine(center, centerPointOnMaxRightEdge, Color.blue, 10f);
            // Debug.DrawLine(center, centerPointOnMaxLeftEdge, Color.cyan, 10f);
        }

        _lastSize = size;
    }

    public void SetReversed(bool value)
    {
        reverse = value;
    }

    public void SetParallaxScale(float value)
    {
        parallaxScale = value;
    }

    public void SetHorizontalConstraint(float value)
    {
        horizontalConstraint = value;
    }

    public void SetVerticalConstraint(float value)
    {
        verticleConstraint = value;
    }
    
    /*
     * TODO: Load settings from PlayerSettingPref
     */
    public void InitializeFromSettings()
    {
            
    }
    
    /*
     * TODO: 
     */
    public void GetMetaSettings()
    {
        
    }
    
#if UNITY_EDITOR
    BGController()
    {
        // Debug.Log("Initializing BGController");
        // EditorApplication.update += ReSizeCamera;
    }

    private void OnValidate()
    {
        ReScale();
        // ReSizeCamera();
        RecordScreenInfo();
    }
#endif
}

#if UNITY_EDITOR
[InitializeOnLoad]
public static class GameViewSizeWatcher
{
    static Vector2 lastSize = Vector2.zero;

    static GameViewSizeWatcher()
    {
        EditorApplication.update += GetGameViewSize;
    }

    static void GetGameViewSize()
    {
        Vector2 size = Handles.GetMainGameViewSize();
        if (size != lastSize)
        {
            // Debug.Log("Game view size changed to " + size);
        }

        lastSize = size;
    }
}

#endif