using System;
using System.Collections.Generic;

/// <summary>
/// 通用参数
/// </summary>
public class BTCommonConstans
{
    /// <summary>
    /// 执行功能：采集、建造、攻击
    /// </summary>
    public const string DoFunction = "DoFunction";

    /// <summary>
    /// 执行功能的决策来源
    /// </summary>
    public const string DoFunctionReason = "DoFunctionReason";

    /// <summary>
    /// 供CSActionChangeToDoFunction使用，改变角色的DoFunction值
    /// </summary>
    public const string ChangeToDoFunction = "ChangeToDoFunction";

    /// <summary>
    /// 供CSActionChangeToDoFunction使用，改变角色的DoFunction值的原因
    /// </summary>
    public const string ChangeFunctionReason = "ChangeFunctionReason";

    /// <summary>
    /// 掉落功能类型
    /// </summary>
    public const string DropFunctionType = "DropFunctionType";

    /// <summary>
    /// 通知Tips
    /// </summary>
    public const string NotifyTips = "NotifyTips";

    /// <summary>
    /// 点击    Bool
    /// </summary>
    public const string IsClick = "IsClick";

    /// <summary>
    /// 拖拽    Bool
    /// </summary>
    public const string IsDrag = "IsDrag";

    /// <summary>
    /// 拖拽放下   Bool
    /// </summary>
    public const string IsDrop = "IsDrop";

    /// <summary>
    /// 存活
    /// </summary>
    public const string IsSurvial = "IsSurvial";

    /// <summary>
    /// 生存时间结束掉落/生存时间结束掉落
    /// </summary>
    public const string EnableSurvialDrop = "EnableSurvialDrop";

    /// <summary>
    /// 生存时间结束掉落/超过生存时间了
    /// </summary>
    public const string SurvialTimeOut = "SurvialTimeOut";

    /// <summary>
    /// 点击掉落/可以点击掉落
    /// </summary>
    public const string EnableClickDrop = "EnableClickDrop";

    /// <summary>
    /// 点击掉落/剩余掉落次数>0
    /// </summary>
    public const string ClickDropTimesValid = "ClickDropTimesValid";

    /// <summary>
    /// 点击掉落/达到死亡掉落次数
    /// </summary>
    public const string ClickDropDie = "ClickDropDie";

    /// <summary>
    /// 时间掉落/有功能:时间掉落
    /// </summary>
    public const string EnableTimeDrop = "EnableTimeDrop";

    /// <summary>
    /// 时间掉落/时间消亡次数
    /// </summary>
    public const string TimeDieCount = "TimeDieCount";

    /// <summary>
    /// 时间掉落/到掉落时间了
    /// </summary>
    public const string TimeToDrop = "TimeToDrop";


}


public class BTCharacterConstans
{

    #region 玩家方的工人角色
    /// <summary>
    /// 角色/采集/可以采集
    /// </summary>
    public const string CanCollect = "CanCollect";

    /// <summary>
    /// 采集完成
    /// </summary>
    public const string CollectComplete = "CollectComplete";

    /// <summary>
    /// 角色/攻击/可以攻击
    /// </summary>
    public const string CanAttack = "CanAttack";

    /// <summary>
    /// 角色/建造/可以建造
    /// </summary>
    public const string CanBuild = "CanBuild";

    public const string CanTransport = "CanTransport";

    /// <summary>
    /// 睡觉
    /// </summary>
    public const string Sleep = "Sleep";

    /// <summary>
    /// 是否有体力  Bool
    /// </summary>
    public const string HasEnergy = "HasEnergy";

    /// <summary>
    /// 是否能待在龙屋
    /// </summary>
    public const string CanStayHome = "CanStayHome";

    /// <summary>
    /// 有空地格
    /// </summary>
    public const string EmptyCell = "EmptyCell";

    /// <summary>
    /// 休闲状态
    /// </summary>
    public const string Idle = "Idle";

    /// <summary>
    /// 休闲类型
    /// </summary>
    public const string IdleType = "IdleType";

    /// <summary>
    /// 移动到：龙屋、采集点、建造点、攻击目标
    /// </summary>
    public const string MoveTo = "MoveTo";
    #endregion

    #region Monster

    /// <summary>
    /// 怪物：合成CD
    /// </summary>
    public const string MonsterMergeCD = "MonsterMergeCD";

    /// <summary>
    /// 怪物：活动CD
    /// </summary>
    public const string MonsterArticleCD = "MonsterArticleCD";

    /// <summary>
    /// 怪物：上次活动结束时间
    /// </summary>
    public const string MonsterEnableActiveTime = "MonsterEnableActiveTime";

    /// <summary>
    /// 怪物：血量
    /// </summary>
    public const string MonsterHasHP = "MonsterHasHP";

    /// <summary>
    /// 怪物：有污染功能
    /// </summary>
    public const string CanPollute = "CanPollute";

    /// <summary>
    /// 怪物：有合成功能
    /// </summary>
    public const string CanMerge = "CanMerge";

    /// <summary>
    /// 怪物：有偷窃功能
    /// </summary>
    public const string CanSteal = "CanSteal";

    /// <summary>
    /// 怪物：可以被攻击
    /// </summary>
    public const string CanBeAttack = "CanBeAttack";

    /// <summary>
    /// 怪物：攻击者数量
    /// </summary>
    public const string AttackerCount = "AttackerCount";

    /// <summary>
    /// 将要被合成
    /// </summary>
    public const string PreBeMerge = "PreBeMerge";

    /// <summary>
    /// 销魂偷窃道具
    /// </summary>
    public const string StealDestroyArticle = "StealDestroyArticle";
    #endregion
}

public class BTArticleConstans
{

    #region 建造
    /// <summary>
    /// 建造/可以建造
    /// </summary>
    public const string EnableBuild = "EnableBuild";

    /// <summary>
    /// 建造完成
    /// </summary>
    public const string BuildingDone = "BuildingDone";

    /// <summary>
    /// 建造/工人数量
    /// </summary>
    public const string BuildWorkerCount = "BuildWorkerCount";
    #endregion

    #region 睡觉
    /// <summary>
    /// 龙屋/可以休息、睡觉恢复
    /// </summary>
    public const string EnableSleep = "EnableSleep"; 

    /// <summary>
    /// 龙屋/有龙在屋子中睡觉
    /// </summary>
    public const string HasDragonSleep = "HasDragonSleep";
    #endregion

    #region 待在屋子
    /// <summary>
    /// 待在屋子/能待在屋子
    /// </summary>
    public const string EnableStayHome = "EnableStayHome";
    #endregion

    #region 宝箱掉落
    /// <summary>
    /// 宝箱掉落/可以宝箱掉落
    /// </summary>
    public const string EnableTreasureDrop = "EnableTreasureDrop";  

    /// <summary>
    /// 宝箱掉落/宝箱解锁
    /// </summary>
    public const string TreasureUnLock = "TreasureUnLock";
    #endregion

    /// <summary>
    /// 双击道具
    /// </summary>
    public const string IsDoubleClick = "IsDoubleClick";

    #region 时间掉落
    /// <summary>
    /// 时间掉落/时间掉落身上内部掉满
    /// </summary>
    public const string TimeDropFull = "TimeDropFull";
    #endregion

    #region 采集掉落
    /// <summary>
    /// 采集掉落/可以采集掉落
    /// </summary>
    public const string EnableCollect = "EnableCollect";

    /// <summary>
    /// 采集掉落/剩余采集次数
    /// </summary>
    public const string CollectTimesRemained = "CollectTimesRemained";

    /// <summary>
    /// 采集掉落/采集工人数
    /// </summary>
    public const string CollectWorkerCount = "CollectWorkerCount";

    /// <summary>
    /// 采集掉落/采集完是否销毁
    /// </summary>
    public const string CollectEndToDead = "CollectEndToDead";
    #endregion

    #region 攻击
    /// <summary>
    /// 攻击、击杀/可以攻击、击杀
    /// </summary>
    public const string EnableBeAttack = "EnableBeAttack";

    /// <summary>
    /// 攻击、击杀/攻击者数量
    /// </summary>
    public const string AttackerCount = "AttackerCount";

    /// <summary>
    /// 攻击、击杀/正在被攻击
    /// </summary>
    public const string BeAttacking = "BeAttacking";
    #endregion

    /// <summary>
    /// 召唤巨龙/可以召唤巨龙
    /// </summary>
    public const string EnableSummonerDragon = "EnableSummonerDragon";

    /// <summary>
    /// 剩余召唤巨龙次数
    /// </summary>
    public const string EnableSummonerCount = "EnableSummonerCount";

    /// <summary>
    /// 召唤剩余巨龙
    /// </summary>
    public const string SummonerResidueDragon = "SummonerResidueDragon";

    /// <summary>
    /// 点击解锁地块
    /// </summary>
    public const string EnableUnLockCell = "EnableUnLockCell";

    /// <summary>
    /// 跟随其他地块的治愈，使用解锁道具
    /// </summary>
    public const string IsFollowCure = "IsFollowCure";

    /// <summary>
    /// 大泡泡状态
    /// </summary>
    public const string BubbleBigState = "BubbleBigState";

    /// <summary>
    /// 大泡泡解锁了
    /// </summary>
    public const string BubbleBigUnLock = "BubbleBigUnLock";

    /// <summary>
    /// 大泡泡生存时间到了
    /// </summary>
    public const string BubbleBigTimeOut = "BubbleBigTimeOut";

    /// <summary>
    /// 有升级功能
    /// </summary>
    public const string EnableUpgrade = "EnableUpgrade";

    /// <summary>
    /// 能被偷
    /// </summary>
    public const string EnableBeStolen = "EnableBeStolen";

    /// <summary>
    /// 道具吸附的雷个数
    /// </summary>
    public const string BeThunderAdhereCount = "BeThunderAdhereCount";

    /// <summary>
    /// 是炮塔
    /// </summary>
    public const string IsTurret = "IsTurret";
}


public class BTFlotageConstans
{
    /// <summary>
    /// 可以拖拽
    /// </summary>
    public const string EnableDrag = "EnableDrag";

    /// <summary>
    /// 预准备掉落
    /// </summary>
    public const string ReadyDrop = "ReadyDrop";

}

public class BTThunderConstans
{
    /// <summary>
    /// 是否激活
    /// </summary>
    public const string IsActive = "IsActive";

    /// <summary>
    /// 雷吸附道具存在
    /// </summary>
    public const string ThunderAdhereArticleExist = "ThunderAdhereArticleExist";
}

public class BTTurretConstans
{
    /// <summary>
    /// 炮塔目标地块
    /// </summary>
    public const string TurrentTargetCell = "TurrentTargetCell";

    /// <summary>
    /// 炮塔CD中
    /// </summary>
    public const string TurrentCD = "TurrentCD";
}