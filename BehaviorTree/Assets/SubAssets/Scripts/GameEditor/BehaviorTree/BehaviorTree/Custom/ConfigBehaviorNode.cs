using System.Collections;
using System;
namespace BehaviorTree
{
    public static class IDENTIFICATION
    {
        // 行为-做饭
        public const int COOKING = 11000;

        // 行为-吃饭
        public const int EAT = 11001;

        // 行为-移动到目标
        public const int MOVE = 11002;

        // 行为-看电视
        public const int WATCH_TV = 11003;

        // 通用条件节点
        public const int CONDITION_CUSTOM = 20002;
    }


    public class ConfigBehaviorNode
    {
        private Action<CustomIdentification> ConfigEvent;
     
        public void Init()
        {


            #region Human
            Config<NodeActionCooking>("行为-做饭", IDENTIFICATION.COOKING);
            Config<NodeActionEat>("行为-吃饭", IDENTIFICATION.EAT);
            Config<NodeActionMove>("行为-移动到目标", IDENTIFICATION.MOVE);
            Config<NodeActionWatchTV>("行为-看电视", IDENTIFICATION.WATCH_TV);
            #endregion


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