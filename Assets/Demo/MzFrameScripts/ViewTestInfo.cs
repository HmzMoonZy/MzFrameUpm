using UnityEngine;

public partial class ViewTestInfo
{
    public override void OnOpened(params object[] args)
    {
        Debug.Log($"{ViewName} : OnOpened!");
    }

    private float time = 0;
    public override void Update(float deltaTime)
    {
        time += deltaTime;
        if (time >= 1)
        {
            time = 0;
            Debug.Log("一秒过去了...");
        }
    }

    private void __OnClickBtn_Button()
    {
        Debug.Log("Click!");
    }
}