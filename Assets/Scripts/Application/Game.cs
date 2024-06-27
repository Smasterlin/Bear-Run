using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
[RequireComponent(typeof(ObjectPool))]
[RequireComponent(typeof(StaticData))]
[RequireComponent(typeof(SoundManager))]
public class Game : MonoSingleTon<Game>
{
    public ObjectPool objectPool;
    public StaticData staticData;
    public SoundManager soundManager;
    Slider sliProgress;
    float totalTime = 2.5f;
    float timer;
    TextMeshProUGUI txt_number;
    float number;
    float speed;
    protected override void Awake()
    {
        base.Awake();
        sliProgress = FindObjectOfType<Slider>();
        timer = 0;
        number = 0;
        txt_number = FindObjectOfType<TextMeshProUGUI>();
        speed = 50;
    }
    private void Start()
    {
        //单例模式
        objectPool = ObjectPool.Instance;
        staticData = StaticData.Instance;
        soundManager = SoundManager.Instance;

        DontDestroyOnLoad(gameObject);

        //初始化 startUpController
        RegisterController(Const.E_startUp, typeof(StartUpController));

        //游戏启动
        SendEvent(Const.E_startUp);

        Invoke("LoadNextScenes", 2.4f);
    }
    private void Update()
    {
        if (timer<=totalTime)
        {
            timer += Time.deltaTime;
            sliProgress.value = timer / totalTime;
            //number = Mathf.MoveTowards(number,100,speed*Time.deltaTime);
            txt_number.text = (sliProgress.value*100).ToString("f2")+"%";
        }
    }
    private void LoadNextScenes()
    {
        LoadScene(1);
    }
    public void LoadScene(int level)
    {
        ArgScenes e = new()
        {
            sceneIndex = SceneManager.GetActiveScene().buildIndex
        };

        //退出场景事件
        SendEvent(Const.E_exitScene,e);

        SceneManager.LoadScene(level);
    }
    /// <summary>
    /// 进入新场景的时候加载的方法
    /// </summary>
    /// <param name="level"></param>
    private void OnLevelWasLoaded(int level)
    {
        Debug.Log("进入新场景" +level) ;
        ArgScenes e = new() 
        {
           sceneIndex = SceneManager.GetActiveScene().buildIndex
        };
        
        SendEvent(Const.E_enterScene,e);
    }


    void SendEvent(string eventName,object data=null)
    {
        MVC.SendEvent(eventName,data);
    }

    protected void RegisterController(string eventName, Type controller)
    {
        MVC.RegisterController(eventName, controller);
    }
}
