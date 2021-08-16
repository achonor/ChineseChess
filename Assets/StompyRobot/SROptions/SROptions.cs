using System.ComponentModel;
using UnityEngine;

public delegate void SROptionsPropertyChanged(object sender, string propertyName);

public partial class SROptions : INotifyPropertyChanged
{
    private static readonly SROptions _current = new SROptions();

    public static SROptions Current
    {
        get { return _current; }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void OnStartup()
    {
        SRDebug.Instance.AddOptionContainer(Current);
    }

    public event SROptionsPropertyChanged PropertyChanged;
    
#if UNITY_EDITOR
    [JetBrains.Annotations.NotifyPropertyChangedInvocator]
#endif
    public void OnPropertyChanged(string propertyName)
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, propertyName);
        }

        if (InterfacePropertyChangedEventHandler != null)
        {
            InterfacePropertyChangedEventHandler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    private event PropertyChangedEventHandler InterfacePropertyChangedEventHandler;

    event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
    {
        add { InterfacePropertyChangedEventHandler += value; }
        remove { InterfacePropertyChangedEventHandler -= value; }
    }



    [DisplayName("最小搜索深度")]
    [Increment(1)]
    [NumberRange(6, 20)]
    public int MinSearchDepth {
        get {
            return GameConst.Instance.MinSearchDepth;
        }
        set {
            GameConst.Instance.MinSearchDepth = value;
        }
    }
    [DisplayName("最长搜索时间")]
    [Increment(500)]
    [NumberRange(1000, 50000)]
    public int MaXSearchDuration {
        get {
            return GameConst.Instance.MaXSearchDuration;
        }
        set {
            GameConst.Instance.MaXSearchDuration = value;
        }
    }
}
