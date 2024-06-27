using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Const
{
    //事件
    public const string E_exitScene = "E_exitScene";
    public const string E_enterScene = "E_enterScene";
    public const string E_startUp = "E_startUp";
    public const string E_endGame = "E_endGame";
    public const string E_HitAddTime = "E_HitAddTime";
    public const string E_UIFinalScore = "E_UIFinalScore";

    /// ui事件
    public const string E_UpdateDis = "E_UpdateDis";
    public const string E_UpdateCoins = "E_UpdateCoins";
    public const string E_PauseGame = "E_PauseGame";
    public const string E_Resume = "E_Resume";
    public const string E_BriberyClick = "E_BriberyClick";
    public const string E_ContinueGame = "E_ContinueGame";

    public const string E_HitItem = "E_HitItem";
    public const string E_BuyTools = "E_BuyTools";
    public const string E_BuyFootball = "E_BuyFootball";
    public const string E_EquipFootball = "E_EquipFootball";
    public const string E_BuyCloth = "E_BuyCloth";
    public const string E_EquipCloth = "E_EquipCloth";
    public const string E_Setting = "E_Setting";
    public const string E_ResetData = "E_ResetData";

    //射进进球
    public const string E_HitGoalTrigger = "E_HitGoalTrigger";
    public const string E_OnGoalClick = "E_OnGoalClick";
    public const string E_ShootGoal = "E_ShootGoal";


    //View
    public const string V_PlayerMove = "V_PlayerMove";
    public const string V_PlayerAnim = "V_PlayerAnim";
    public const string V_Board = "V_Board";
    public const string V_Pause = "V_Pause";
    public const string V_Resume = "V_Resume";
    public const string V_UIDead = "V_UIDead";
    public const string V_UIFinalScore = "V_UIFinalScore";
    public const string V_BuyTools = "V_BuyTools";
    public const string V_MainMenu = "V_MainMenu";
    public const string V_UIShop = "V_UIShop";
    public const string V_UISetting = "V_UISetting";
    //Model
    public const string G_GameModel = "G_GameModel";
}
public enum INPUTDIRECTION
{
    NULL,
    LEFT,
    RIGHT,
    UP,
    DOWN,
}

public enum ITEMKIND
{
    ITEMINVINCIBLE,
    ITEMMAGNET,
    ITEMMULTIPLY,
}
public enum ITEMSTATE
{
    BUY,
    UNBUY,
    EQUIP
}