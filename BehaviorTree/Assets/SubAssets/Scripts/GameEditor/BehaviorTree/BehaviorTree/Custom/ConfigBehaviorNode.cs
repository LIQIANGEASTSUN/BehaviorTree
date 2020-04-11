using System.Collections;
using System;
namespace BehaviorTree
{

    public class ConfigBehaviorNode
    {
        private Action<CustomIdentification> ConfigEvent;
     
        public void Init()
        {

            Config<NodeActionRequestSkillState>("切换状态节点", 10000);


            #region Human
            Config<NodeActionCooking>("行为-做饭", 11000);
            Config<NodeActionEat>("行为-吃饭", 11001);
            Config<NodeActionMove>("行为-移动到目标", 11002);
            Config<NodeActionWatchTV>("行为-看电视", 11003);
            #endregion



            Config<NodeConditionCustom>("通用条件节点", 20002);

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