using System.Collections;
using System;
namespace BehaviorTree
{

    public class ConfigBehaviorNode
    {
        private Action<CustomIdentification> ConfigEvent;
     
        public void Init()
        {
#if true

            #region 行为节点
            Config<PlayerAttackAction>("Player/攻击");
            Config<PlayerMoveAction>("Player/移动");
            Config<PlayerPatrolAction>("Player/巡逻");
            Config<PlayerReplenishEnergyAction>("Player/补充能量");
            Config<PlayerSearchEnemyAction>("Player/搜索敌人");

            #endregion


            #region 条件节点
            Config<PlayerEnougthEnergyCondition>("Player/能量是否足够判断");
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