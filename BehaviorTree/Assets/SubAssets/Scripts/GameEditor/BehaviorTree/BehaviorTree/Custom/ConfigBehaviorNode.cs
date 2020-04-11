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