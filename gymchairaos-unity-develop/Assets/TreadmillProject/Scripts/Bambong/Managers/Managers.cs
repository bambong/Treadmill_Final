
using Gymchair.Core.Mgr;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static bool isQuit = false;
    static Managers instance; 
    static Managers Instance
    {
        get
        {
            if (!isQuit)
            {
                Init();
            }
            return instance;
        }
    }

    #region CoreManager

    private SceneManagerEx scene = new SceneManagerEx();
    private SoundManager sound = new SoundManager();
    private DataManager data = new DataManager();
    private TokenInputManager token = new TokenInputManager();
    public static SoundManager Sound { get => Instance.sound; }
    public static SceneManagerEx Scene { get => Instance.scene; }
    public static DataManager Data { get => Instance.data; }
    public static TokenInputManager Token { get => Instance.token; }
    public static MonoBehaviour MonoForCoroutine { get => Instance; }

    #endregion

    void Start()
    {
        Init();
    }

    static void Init()
    {
        if (instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
            if (go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }

            DontDestroyOnLoad(go);
            instance = go.GetComponent<Managers>();
            Scene.Init();
            Sound.Init();
            Token.Init();
        }
    }
 
    public static void Clear()
    {
       
    }
    private void OnApplicationQuit()
    {
        isQuit = true;
     
    }
}
