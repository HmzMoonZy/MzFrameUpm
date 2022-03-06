// 
using System.Collections.Generic;
using System;
using MzFrame;
using UnityEngine;
using UnityEngine.UI;
public  partial class ViewTestInfo : ViewInfo{public ViewCompFinder BackGround;
public ViewCompFinder BackGround_Button;
public ViewTestInfo(GameObject viewObject, string name) : base(viewObject, name){BackGround = new ViewCompFinder(viewObject.transform.Find("#BackGround"));
BackGround_Button = new ViewCompFinder(viewObject.transform.Find("#BackGround/$Button"));
}}