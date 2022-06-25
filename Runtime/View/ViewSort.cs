namespace MzFrame
{
    /// <summary>
    /// MzFrame 通过 ESortLayer 枚举来实现层级管理。
    /// 修改它后请重新初始化。
    /// </summary>
    public enum ViewSort
    {
        Lobby = 0,

        Operation = 10,

        SubSystem = 15,

        Load = 20,

        Tip = 30,

        Warning = 40,

        Guide = 50,

        Debug = 100,
    }
}