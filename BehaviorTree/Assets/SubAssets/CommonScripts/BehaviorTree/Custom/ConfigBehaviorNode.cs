using System.Collections;
using System;
namespace BehaviorTree
{
    public static class IDENTIFICATION
    {
        #region Character Action
        /// <summary>
        /// CharacterAI：攻击
        /// </summary>
        public const int CharacterAttack = 100000;

        /// <summary>
        /// CharacterAI:被点击
        /// </summary>
        public const int CharacterClick = 100015;

        /// <summary>
        /// CharacterAI: 采集
        /// </summary>
        public const int CharacterCollect = 100020;

        //CharacterAI: 被选中
        public const int CharacterHold = 100030;
        //CharacterAI: 在屋子闲置
        public const int CharacterHome = 100040;

        /// <summary>
        /// CharacterAI: 原地等待
        /// </summary>
        public const int CharacterIdle = 100050;

        /// <summary>
        /// CharacterAI: 移动行为
        /// </summary>
        public const int CharacterMove = 100060;

        /// <summary>
        /// CharacterAI: 拖拽
        /// </summary>
        public const int CharacterDrag = 100065;

        /// <summary>
        /// CharacterAI: 拖拽放下
        /// </summary>
        public const int CharacterDrop = 100066;

        /// <summary>
        /// CharacterAI: 睡觉 ，不同于回屋
        /// </summary>
        public const int CharacterSleep = 100070;

        /// <summary>
        /// CharacterAI: 寻找可以睡觉的龙屋 
        /// </summary>
        public const int CharacterFindSleepHome = 100071;

        /// <summary>
        /// CharacterAI：运输
        /// </summary>
        public const int CharacterTransport = 100080;

        /// <summary>
        /// CharacterAI: 运输，没地方运输，等待的行为
        /// </summary>
        public const int CharacterTransportWaitting = 100081;

        /// <summary>
        /// CharacterAI: 运输查找地格
        /// </summary>
        public const int CharacterTransportFindCell = 100082;

        /// <summary>
        /// CharacterAI: 死亡
        /// </summary>
        public const int CharacterDead = 100100;

        /// <summary>
        /// CharacterAI: 建造
        /// </summary>
        public const int CharacterBuild = 100110;

        /// <summary>
        /// CharacterAI: 采集、建造、攻击 决策
        /// </summary>
        public const int CharacterDecision = 100120;

        /// <summary>
        /// CharacterAI:绑定
        /// </summary>
        public const int CharacterBind = 100130;

        /// <summary>
        /// CharacterAI:准备
        /// </summary>
        public const int CharacterReady = 100133;

        /// <summary>
        /// CharacterAI:改变要执行的目标
        /// </summary>
        public const int ChangeToDoFunction = 100140;

        #endregion

        #region Monster Action
        /// <summary>
        /// MonsterAI: 合成
        /// </summary>
        public const int MonsterMerge = 105000;

        /// <summary>
        /// MonsterAI: 合成决策
        /// </summary>
        public const int MonsterMergeDecision = 105001;

        /// <summary>
        /// MonsterAI: 污染
        /// </summary>
        public const int MonsterPollute = 105010;

        /// <summary>
        /// MonsterAI: 计算污染哪个道具
        /// </summary>
        public const int MonsterPolluteFind = 105011;

        /// <summary>
        /// MonsterAI: 污染决策
        /// </summary>
        public const int MonsterPolluteDecision = 105012;

        /// <summary>
        /// MonsterAI: 偷窃
        /// </summary>
        public const int MonsterSteal = 105020;

        /// <summary>
        /// MonsterAI: 计算偷窃哪个道具
        /// </summary>
        public const int MonsterStealFind = 105021;

        /// <summary>
        /// MonsterAI: 偷窃决策
        /// </summary>
        public const int MonsterStealDecision = 105022;

        /// <summary>
        /// MonsterAI: 偷窃 Idle
        /// </summary>
        public const int MonsterStealIdle = 105023;

        /// <summary>
        /// MonsterAI: 偷窃，扔掉偷窃物
        /// </summary>
        public const int MonsterStealThrow = 105024;

        /// <summary>
        /// MonsterAI: Idle
        /// </summary>
        public const int MonsterIdle = 105030;

        /// <summary>
        /// MonsterAI:执行功能决策
        /// </summary>
        public const int MonsterDecision = 105040;

        /// <summary>
        /// MonsterAI; 移动
        /// </summary>
        public const int MonsterMove = 105050;

        /// <summary>
        /// MonsterAI:死亡掉落
        /// </summary>
        public const int MonsterDeadDrop = 105060;

        /// <summary>
        /// MonsterAI:血量判定
        /// </summary>
        public const int MonsterConditionHP = 105070;

        /// <summary>
        /// MonsterAI:被攻击
        /// </summary>
        public const int MonsterBeAttack = 105080;
        #endregion

        ///////////////////////////////////////////////////

        #region Character Codition
        /// <summary>
        /// CharacterAI: 能量判断
        /// </summary>
        public const int CharacterEnergy = 110000;

        public const int CharacterTargetHasChanged = 110010;

        #endregion

        #region Article Action
        /// <summary>
        /// 生存时间结束掉落
        /// </summary>
        public const int SurvialDrop = 200000;

        /// <summary>
        /// Article 建造
        /// </summary>
        public const int ArticleBuild = 200001;

        /// <summary>
        /// 休息、睡觉恢复
        /// </summary>
        public const int RecoverSleep = 200002;

        /// <summary>
        /// 宝箱掉落
        /// </summary>
        public const int TreasureDrop = 200003;

        /// <summary>
        /// 点击掉落
        /// </summary>
        public const int ClickDrop = 200004;

        /// <summary>
        /// 时间掉落
        /// </summary>
        public const int TimeDrop = 200005;

        /// <summary>
        /// 采集掉落
        /// </summary>
        public const int CollectDrop = 200006;
        public const int CollectRecover = 200010;

        /// <summary>
        /// 攻击、击杀
        /// </summary>
        public const int BeAttack = 200007;

        /// <summary>
        /// 召唤巨龙
        /// </summary>
        public const int SummonerDragon = 200008;

        /// <summary>
        /// Article 死亡
        /// </summary>
        public const int ArticleDead = 200009;

        /// <summary>
        /// 宝箱解锁
        /// </summary>
        public const int TreasureUnlock = 200011;

        /// <summary>
        /// 点击掉落身上挂的道具
        /// </summary>
        public const int ClickDropAttach = 200050;

        /// <summary>
        /// 身上是否挂载道具条件
        /// </summary>
        public const int ConditionAttachArticle = 200060;

        /// <summary>
        /// 道具解锁地块
        /// </summary>
        public const int ArticleUnLockCell = 200070;

        public const int ClickArticleGeneral = 200080;

        public const int DragArticle = 200090;

        /// <summary>
        /// 通知可以点击了
        /// </summary>
        public const int EnableClickNotify = 200100;
        /// <summary>
        /// 大泡泡解锁
        /// </summary>
        public const int BubbleBigUnLock = 200110;
        /// <summary>
        /// 大泡泡释放
        /// </summary>
        public const int BubbleBigRelease = 200111;

        /// <summary>
        /// 拖拽放下道具
        /// </summary>
        public const int DropArticle = 200120;

        /// <summary>
        /// s掉落地块检测
        /// </summary>
        public const int DropCellCheck = 200130;

        /// <summary>
        /// 点击掉落无地块通知
        /// </summary>
        public const int NoCellNotify = 200140;
       
        /// <summary>
        /// 道具升级
        /// </summary>
        public const int ArticleUpGrade = 200150;

        /// <summary>
        /// 点击掉落死亡
        /// </summary>
        public const int ClickDropDead = 200160;

        /// <summary>
        /// 点击了可以点击掉落的道具
        /// </summary>
        public const int ClickEnableClickArticle = 200161;

        /// <summary>
        /// 时间掉落掉满
        /// </summary>
        public const int TimeDropFull = 200170;

        #endregion

        #region BubbleAction

        /// <summary>
        /// 泡泡点击
        /// </summary>
        public const int BubbleClick = 300000;

        /// <summary>
        /// 泡泡掉落有空地格
        /// </summary>
        public const int BubbleDropCellCheck = 300010;

        /// <summary>
        /// 泡泡掉落无地块可用
        /// </summary>
        public const int BubbleNotCellNotify = 300020;
        #endregion

        #region Flotage
        /// <summary>
        /// 漂浮物点击掉落
        /// </summary>
        public const int FlotageClickDrop = 400000;

        /// <summary>
        /// 漂浮物移动
        /// </summary>
        public const int FlotageMove = 400010;

        /// <summary>
        /// 漂浮物拖拽
        /// </summary>
        public const int FlotageDrag = 400020;
        #endregion

        #region Cloud
        /// <summary>
        /// 云点击
        /// </summary>
        public const int CloudClick = 500000;

        /// <summary>
        /// 云可点击通知
        /// </summary>
        public const int CloudClickNotify = 500001;

        /// <summary>
        /// 点击了可以掉落的云
        /// </summary>
        public const int CloudClickEnableDrop = 500002;

        /// <summary>
        /// 云点击死亡掉落
        /// </summary>
        public const int CloudClickDeadDrop = 500003;

        /// <summary>
        /// 云时间掉落
        /// </summary>
        public const int CloudTimeDrop = 500010;

        /// <summary>
        /// 云生存时间死亡
        /// </summary>
        public const int CloudSurvialDead = 500020;

        /// <summary>
        /// 云死亡
        /// </summary>
        public const int CloudDead = 500090;

        /// <summary>
        /// 云掉落地块检测
        /// </summary>
        public const int CloudDropCellCheck = 500100;

        #endregion

        // 条件-通用条件节点
        public const int CONDITION_CUSTOM = 20002;
    }



    public class ConfigBehaviorNode
    {
        private Action<CustomIdentification> ConfigEvent;
     
        public void Init()
        {
#if false
            #region CharacterAI
            // Action ///////////////////////////////////////////////////////////////////////////////
            Config<CSActionAttack>("生物/攻击行为", IDENTIFICATION.CharacterAttack);
            Config<CSActionClick>("生物/被点击", IDENTIFICATION.CharacterClick);
            Config<CSActionCollect>("生物/采集行为", IDENTIFICATION.CharacterCollect);
            Config<CSActionIdle>("生物/Idle", IDENTIFICATION.CharacterIdle);
            Config<CSActionMove>("生物/Move移动", IDENTIFICATION.CharacterMove);
            Config<CSActionDrag>("生物/拖拽", IDENTIFICATION.CharacterDrag);
            Config<CSActionDrop>("生物/拖拽放下", IDENTIFICATION.CharacterDrop);
            Config<CSActionSleep>("生物/睡觉", IDENTIFICATION.CharacterSleep);
            Config<CSActionSleepFindHome>("生物/查找睡觉的屋", IDENTIFICATION.CharacterFindSleepHome);
            Config<CSActionTransport>("生物/运输", IDENTIFICATION.CharacterTransport);
            Config<CSActionTransportWaitting>("生物/运输等待中", IDENTIFICATION.CharacterTransportWaitting);
            Config<CSActionTransportFindCell>("生物/运输查找地格", IDENTIFICATION.CharacterTransportFindCell);
            Config<CSActionDead>("生物/死亡", IDENTIFICATION.CharacterDead);
            Config<CSActionBuild>("生物/建造", IDENTIFICATION.CharacterBuild);
            Config<CSActionDecision>("生物/采集建造攻击决策", IDENTIFICATION.CharacterDecision);
            Config<CSActionBind>("生物/绑定行为", IDENTIFICATION.CharacterBind);
            Config<CSActionReady>("生物/准备行为", IDENTIFICATION.CharacterReady);
            Config<CSActionChangeToDoFunction>("生物/改变执行的功能", IDENTIFICATION.ChangeToDoFunction);


            // Condition ////////////////////////////////////////////////////////////////////////////
            Config<CSConditionEnergy>("生物/能量", IDENTIFICATION.CharacterEnergy);
            Config<CSConditionHasTargetChanged>("生物/预期目标改变了", IDENTIFICATION.CharacterTargetHasChanged);

            #endregion

            #region MonstarAI
            // Action ///////////////////////////////////////////////////////////////////////////////
            Config<MActionMerge>("怪物/合成", IDENTIFICATION.MonsterMerge);
            Config<MActionMergeDecision>("怪物/合成决策", IDENTIFICATION.MonsterMergeDecision);
            Config<MActionPollute>("怪物/污染", IDENTIFICATION.MonsterPollute);
            Config<MActionPolluteFind>("怪物/计算污染点", IDENTIFICATION.MonsterPolluteFind);
            Config<MActionPolluteDecision>("怪物/污染决策", IDENTIFICATION.MonsterPolluteDecision);
            Config<MActionSteal>("怪物/偷窃", IDENTIFICATION.MonsterSteal);
            Config<MActionStealFind>("怪物/计算偷窃点", IDENTIFICATION.MonsterStealFind);
            Config<MActionStealDecision>("怪物/偷窃决策", IDENTIFICATION.MonsterStealDecision);
            Config<MActionStealIdle>("怪物/偷窃Idle", IDENTIFICATION.MonsterStealIdle);
            Config<MActionStealThrow>("怪物/偷窃Throw", IDENTIFICATION.MonsterStealThrow);
            Config<MActionIdle>("怪物/Idle", IDENTIFICATION.MonsterIdle);
            Config<MActionDecision>("怪物/决策", IDENTIFICATION.MonsterDecision);
            Config<MActionMove>("怪物/移动", IDENTIFICATION.MonsterMove);
            Config<MActionDeadDrop>("怪物/死亡掉落", IDENTIFICATION.MonsterDeadDrop);
            Config<MConditionHP>("怪物/血量判定", IDENTIFICATION.MonsterConditionHP);
            Config<MActionBeAttack>("怪物/被攻击", IDENTIFICATION.MonsterBeAttack);
            #endregion

            #region Article
            Config<NAActionSurvialDrop>("道具/生存时间结束掉落", IDENTIFICATION.SurvialDrop);
            Config<NAActionBuild>("道具/建造", IDENTIFICATION.ArticleBuild);
            Config<NAActionRecoverSleep>("道具/休息、恢复", IDENTIFICATION.RecoverSleep);
            Config<NAActionTreasureDrop>("道具/宝箱掉落", IDENTIFICATION.TreasureDrop);
            Config<NAActionTreasureUnLock>("道具/宝箱解锁", IDENTIFICATION.TreasureUnlock);
            Config<NAActionClickDrop>("道具/点击掉落", IDENTIFICATION.ClickDrop);
            Config<NAActionTimeDrop>("道具/时间掉落", IDENTIFICATION.TimeDrop);
            Config<NAActionTimeDropFull>("道具/时间掉落掉满", IDENTIFICATION.TimeDropFull);
            Config<NAActionCollectDrop>("道具/采集掉落", IDENTIFICATION.CollectDrop);
            Config<NAActionCollectRecover>("道具/采集恢复", IDENTIFICATION.CollectRecover);
            Config<NAActionBeAttack>("道具/攻击、击杀", IDENTIFICATION.BeAttack);
            Config<NAActionSummoner>("道具/召唤巨龙", IDENTIFICATION.SummonerDragon);
            Config<NAActionDead>("道具/道具死亡", IDENTIFICATION.ArticleDead);
            Config<NAActionClickAttachedDrop>("道具/点击掉落挂载道具", IDENTIFICATION.ClickDropAttach);
            Config<NAActionUnLockCell>("道具/点击解锁地块", IDENTIFICATION.ArticleUnLockCell);

            Config<NAConditionAttached>("道具/挂载道具判断", IDENTIFICATION.ConditionAttachArticle);
            Config<NAActionClick>("道具/点击一般行为", IDENTIFICATION.ClickArticleGeneral);
            Config<NAActionDrag>("道具/拖拽行为", IDENTIFICATION.DragArticle);
            Config<NAActionClickNotify>("道具/可以点击通知", IDENTIFICATION.EnableClickNotify);
            Config<NAActionBubbleUnLock>("道具/大泡泡解锁", IDENTIFICATION.BubbleBigUnLock);
            Config<NAActionBubbleRelease>("道具/大泡泡释放", IDENTIFICATION.BubbleBigRelease);
            Config<NAActionDrop>("道具/拖拽放下", IDENTIFICATION.DropArticle);
            Config<NAConditionDropCheck>("道具/掉落地块检测", IDENTIFICATION.DropCellCheck);
            Config<NAActionNoCellNotify>("道具/无地块通知", IDENTIFICATION.NoCellNotify);
            Config<NAActionUpgrade>("道具/道具升级", IDENTIFICATION.ArticleUpGrade);
            Config<NAActionClickDropDead>("道具/点击掉落死亡", IDENTIFICATION.ClickDropDead);
            Config<NAActionClickEnable>("道具/点击了可以点击掉落的道具", IDENTIFICATION.ClickEnableClickArticle);
            #endregion

            #region Bubble
            Config<BubbleActionClick>("泡泡/点击泡泡", IDENTIFICATION.BubbleClick);
            Config<BubbleConditionClickDrop>("泡泡/有地块可掉落", IDENTIFICATION.BubbleDropCellCheck);
            Config<BubbleActionNoCellNotify>("泡泡/无地块可用通知", IDENTIFICATION.BubbleNotCellNotify);
            #endregion

            #region Flotage
            Config<FlotageActionClick>("漂浮物/点击掉落", IDENTIFICATION.FlotageClickDrop);
            Config<FlotageActionMove>("漂浮物/漂浮物移动", IDENTIFICATION.FlotageMove);
            Config<FlotageActionDrag>("漂浮物/漂浮物拖拽", IDENTIFICATION.FlotageDrag);
            #endregion

            #region Cloud
            Config<CloudActionClick>("云/点击云掉落", IDENTIFICATION.CloudClick);
            Config<CloudActionClickNotify>("云/云可点击通知", IDENTIFICATION.CloudClickNotify);
            Config<CloudActionClickEnable>("云/点击了可以掉落的云", IDENTIFICATION.CloudClickEnableDrop);
            Config<CloudActionTimeDrop>("云/云时间掉落", IDENTIFICATION.CloudTimeDrop);
            Config<CloudActionClickDropDead>("云/云点击死亡掉落", IDENTIFICATION.CloudClickDeadDrop);
            Config<CloudActionDead>("云/云死亡", IDENTIFICATION.CloudDead);
            Config<CloudActionSurvialDrop>("云/云生存时间死亡", IDENTIFICATION.CloudSurvialDead);

            Config<CloudConditionDropCheck>("云/云掉落地块检测", IDENTIFICATION.CloudDropCellCheck);
            #endregion

#endif

            Config<NodeConditionCustom>("通用条件节点", IDENTIFICATION.CONDITION_CUSTOM);
        }

        private void Config<T>(string name, int identification)
        {
            CustomIdentification customIdentification = new CustomIdentification(name, identification, typeof(T));

            if (null != ConfigEvent)
            {
                ConfigEvent(customIdentification);
            }
        }

        public void AddEvent(Action<CustomIdentification> callBack)
        {
            ConfigEvent += callBack;
        }

        public void RemoveEvent(Action<CustomIdentification> callBack)
        {
            ConfigEvent -= callBack;
        }

    }

}