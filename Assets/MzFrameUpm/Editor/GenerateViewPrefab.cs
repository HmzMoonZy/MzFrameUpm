using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
using UnityEditor;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.UI;

public class GenerateViewPrefab : Editor
{
    [MenuItem("GameObject/UI/Mz-DefaultCanvas")]
    private static void GenerateDefaultViewCanvas()
    {
        var view = new GameObject("ViewDefault", 
            typeof(Canvas),
            typeof(CanvasScaler), 
            typeof(GraphicRaycaster), 
            typeof(MzFrame.ViewConfig));
            view.layer = 5;

        var canvas = view.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = FindObjectsOfType<Camera>().Where(camera => camera.transform.name.Equals("ViewCamera")).ToList()[0];
        canvas.planeDistance = 100;
        canvas.additionalShaderChannels = AdditionalCanvasShaderChannels.None;
        canvas.pixelPerfect = true;

        var scaler = view.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
        
        
        var raycaster = view.GetComponent<GraphicRaycaster>();
    }
    
    [MenuItem("GameObject/UI/Mz-StaticCanvas")]
    private static void GenerateStaticViewCanvas()
    {
        var view = new GameObject("ViewStatic", 
            typeof(Canvas),
            typeof(CanvasScaler),
            typeof(MzFrame.ViewConfig));
        view.layer = 5; //UI
        
        var canvas = view.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = FindObjectsOfType<Camera>().Where(camera => camera.transform.name.Equals("ViewCamera")).ToList()[0];
        canvas.planeDistance = 100;
        canvas.additionalShaderChannels = AdditionalCanvasShaderChannels.None;
        canvas.pixelPerfect = true;

        var scaler = view.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
    }
}
