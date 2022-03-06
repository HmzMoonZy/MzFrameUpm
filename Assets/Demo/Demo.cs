using System.Collections;
using System.Collections.Generic;
using MzFrame;
using UnityEngine;

public class Demo : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        ViewManager.Init();
    }


    private ViewTestInfo testInfo;
    private ViewWhiteInfo whiteInfo;
    private void Update()
    {
        ViewManager.Update();

        
        if (Input.GetKeyDown(KeyCode.A))
        {
            if(testInfo == null || !testInfo.IsVisible)
                testInfo = ViewManager.OpenView<ViewTestInfo>(null);
            else
                ViewManager.CloseView<ViewTestInfo>(null);
        }
        
        if (Input.GetKeyDown(KeyCode.S))
        {
            if(whiteInfo == null || !whiteInfo.IsVisible)
                whiteInfo = ViewManager.OpenView<ViewWhiteInfo>(null);
            else
                ViewManager.CloseView<ViewWhiteInfo>(null);
        }
    }
}
