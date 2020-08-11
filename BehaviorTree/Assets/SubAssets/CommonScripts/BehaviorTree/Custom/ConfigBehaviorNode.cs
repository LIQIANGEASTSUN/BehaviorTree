using System.Collections;
using System;
namespace BehaviorTree
{

    public class ConfigBehaviorNode
    {
        private Action<CustomIdentification> ConfigEvent;
     
        public void Init()
        {
#if false
            #region 角色通用
            Config<CSMActionDead>("生物/死亡");
            Config<CSMActionDead>("怪物/死亡");

            Config<CSMActionBind>("生物/绑定行为");
            Config<CSMActionBind>("怪物/绑定行为");

            Config<CSMActionReady>("生物/准备行为");
            Config<CSMActionReady>("怪物/准备行为");

            Config<CSMActionChangeToDoFunction>("生物/改变执行的功能");
            Config<CSMActionChangeToDoFunction>("怪物/改变执行的功能");

            Config<CSMConditionHasTargetChanged>("生物/预期目标改变了");
            Config<CSMConditionHasTargetChanged>("怪物/预期目标改变了");
            #endregion

            #region CharacterAI
            // Action ///////////////////////////////////////////////////////////////////////////////
            Config<CSActionAttack>("生物/攻击行为");
            Config<CSActionClick>("生物/被点击");
            Config<CSActionCollect>("生物/采集行为");
            Config<CSActionIdle>("生物/Idle");
            Config<CSActionIdleStroll>("生物/Idle 溜达");
            Config<CSActionIdleDecision>("生物/Idle 决策");
            Config<CSActionMove>("生物/Move移动");
            Config<CSActionDrag>("生物/拖拽");
            Config<CSActionDrop>("生物/拖拽放下");
            Config<CSActionSleep>("生物/睡觉");
            Config<CSActionSleepFindHome>("生物/查找睡觉的屋");
            Config<CSActionStayHome>("生物/待在屋子");
            Config<CSActionTransport>("生物/运输");
            Config<CSActionTransportWaitting>("生物/运输等待中");
            Config<CSActionTransportFindCell>("生物/运输查找地格");
            Config<CSActionBuild>("生物/建造");
            Config<CSActionDecision>("生物/采集建造攻击决策");
            // Condition ////////////////////////////////////////////////////////////////////////////
            Config<CSConditionEnergy>("生物/能量");
            #endregion

            #region MonstarAI
            // Action ///////////////////////////////////////////////////////////////////////////////
            Config<MActionMerge>("怪物/合成");
            Config<MActionMergeDecision>("怪物/合成决策");
            Config<MActionPreBeMerge>("怪物/将要被合成");
            Config<MActionPollute>("怪物/污染");
            Config<MActionPolluteDecision>("怪物/污染决策");
            Config<MActionSteal>("怪物/偷窃");
            Config<MActionStealDecision>("怪物/偷窃决策");
            Config<MActionStealIdle>("怪物/偷窃Idle");
            Config<MActionStealThrow>("怪物/偷窃Throw");
            Config<MActionStealIdleStroll>("怪物/偷窃Idle溜达");
            Config<MActionStealDestroy>("怪物/销毁偷窃道具");
            Config<MActionStealIdleDecision>("怪物/偷窃Idle决策");
            Config<MActionIdle>("怪物/Idle");
            Config<MActionIdleStroll>("怪物/Idle 溜达");
            Config<MActionIdleDecision>("怪物/Idle 决策");
            Config<MActionDecision>("怪物/决策");
            Config<MActionMove>("怪物/移动");
            Config<MActionDeadDrop>("怪物/死亡掉落");
            Config<MConditionHP>("怪物/血量判定");
            Config<MConditionActiveCD>("怪物/是否可以活动");
            Config<MActionBeAttack>("怪物/被攻击");

            #endregion

            #region Article
            Config<NAActionBuild>("道具/建造");
            Config<NAActionRecoverSleep>("道具/休息、恢复");
            Config<NAActionStayHome>("道具/待在屋子");
            Config<NAActionTreasureDrop>("道具/宝箱掉落");
            Config<NAActionTreasureUnLock>("道具/宝箱解锁");
            Config<NAActionCollectDrop>("道具/采集掉落");
            Config<NAActionCollectRecover>("道具/采集恢复");
            Config<NAActionBeAttack>("道具/攻击、击杀");
            Config<NAActionClickAttachedDrop>("道具/点击掉落挂载道具");
            Config<NAActionUnLockCell>("道具/点击解锁地块");
            Config<NAActionUnLockCellDelay>("道具/延迟解锁地块");
            Config<NAActionBeStolen>("道具/被偷");

            Config<NAConditionAttached>("道具/挂载道具判断");
            Config<NAActionClick>("道具/点击一般行为");
            Config<NAActionDrag>("道具/拖拽行为");
            Config<NAActionBubbleBigUnLock>("道具/大泡泡解锁");
            Config<NAActionBubbleBigOpen>("道具/大泡泡打开");
            Config<NAActionBubbleBigTimeOut>("道具/大泡泡生存时间到了");
            Config<NAActionDrop>("道具/拖拽放下");
            Config<NAActionUpgrade>("道具/道具升级");
            Config<NAActionThunderAdhere>("道具/被雷吸附");
            #endregion

            #region Bubble
            Config<BubbleActionClick>("泡泡/点击泡泡");
            Config<BubbleConditionClickDrop>("泡泡/有地块可掉落");
            Config<BubbleActionNoCellNotify>("泡泡/无地块可用通知");
            #endregion

            #region Flotage
            Config<FlotageActionClick>("漂浮物/点击掉落");
            Config<FlotageActionMove>("漂浮物/漂浮物移动");
            Config<FlotageActionDrag>("漂浮物/漂浮物拖拽");
            Config<FlotageActionReadyDrop>("漂浮物/漂浮物准备掉落");
            #endregion

            #region Thunder
            Config<ThunderActionDrag>("雷/拖拽");
            Config<ThunderActionDrop>("雷/拖拽放下");
            Config<ThunderActionAdhere>("雷/吸附道具");
            Config<ThunderActionAdhereInterupt>("雷/雷吸附道具中断");
            #endregion

            #region Turret
            Config<TurretActionSearchCell>("炮塔/炮塔查找地块");
            Config<TurretActionAttack>("炮塔/炮塔攻击");
            #endregion

            #region Sprite
            Config<SpriteActionTimeDrop>("通用/时间掉落");
            Config<SpriteActionTimeDropFull>("通用/时间掉落掉满");
            Config<SpriteActionDead>("通用/道具死亡");
            Config<SpriteConditionDropCellCheck>("通用/掉落地块检测");
            Config<SpriteActionSurvialDrop>("通用/生存时间结束掉落");
            Config<SpriteActionSummoner>("通用/召唤巨龙");
            Config<SpriteActionClickNotify>("通用/可点击通知");
            Config<SpriteActionClickEnable>("通用/点击了可以掉落的Sprite");
            Config<SpriteActionUnableClickDropNotify>("通用/点击掉落不能执行通知");
            Config<SpriteActionClickDrop>("通用/点击掉落");
            Config<SpriteActionClickDropDead>("通用/点击掉落死亡");
            Config<SpriteActionTipsNotify>("通用/Tips 通知");
            Config<SpriteConditionEnableClickDrop>("通用/点击掉落能否执行");
            #endregion

            #region MergeOutputCloud
            Config<MergeOutputCloudAction>("合成雨云");
            #endregion

#endif

            Config<NodeConditionCustom>("通用条件节点");
        }

        private void Config<T>(string name)
        {
            CustomIdentification customIdentification = new CustomIdentification(name, typeof(T));

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