using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class StaticData : MonoSingleTon<StaticData>
{
    //����Ļ�����Ϣ
    public Dictionary<int,FootBallInfo> m_footBallInfo=new();
    [SerializeField] List<Material> m_footBallMat = new();

    //�����ɫ���ϵĻ�����Ϣ   ����Ĳ�ͬ��ÿ�����������в�ͬ���·�
    public Dictionary<int,Dictionary<int,FootBallInfo>> m_playerClothData=new();
    [SerializeField] List<Material> playerClothList = new();
    protected override void Awake()
    {
        base.Awake();
        InitFootballInfo();
        InitPlayerCloth();
    }
    /// <summary>
    /// ��ʼ��������Ϣ
    /// </summary>
    private void InitFootballInfo()
    {
        m_footBallInfo.Add(0,new FootBallInfo {coins=0,material=m_footBallMat[0] });
        m_footBallInfo.Add(1,new FootBallInfo {coins=500,material=m_footBallMat[1] });
        m_footBallInfo.Add(2,new FootBallInfo {coins=1000,material=m_footBallMat[2] });
    }
    /// <summary>
    /// ��ʼ���·���Ϣ
    /// </summary>
    private void InitPlayerCloth()
    {
        int t = 0;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (!m_playerClothData.ContainsKey(i))
                {
                    m_playerClothData.Add(i,new Dictionary<int, FootBallInfo >());
                }
                m_playerClothData[i].Add(j,new FootBallInfo(){coins=t*200,material=playerClothList[t] });
                t++;
            }
        }
    }
    public FootBallInfo GetFootBallInfo(int i)
    {
        return m_footBallInfo[i];
    }

    public FootBallInfo GetPlayerCloth(int i,int j)
    {
        return m_playerClothData[i][j];
    }
        
}
