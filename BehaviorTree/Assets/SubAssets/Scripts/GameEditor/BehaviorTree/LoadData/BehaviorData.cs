using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using LitJson;

public class BehaviorData
{

    #region  BehaviorTree
    private Dictionary<string, BehaviorTreeData> _behaviorDic = new Dictionary<string, BehaviorTreeData>();
    public void LoadData(byte[] loadByteData)
    {
        AnalysisBin.AnalysisData(loadByteData, Analysis);
    }

    private void Analysis(byte[] byteData)
    {
        string content = System.Text.Encoding.Default.GetString(byteData);
        BehaviorTreeData behaviorTreeData = JsonMapper.ToObject<BehaviorTreeData>(content);
        _behaviorDic[behaviorTreeData.fileName] = behaviorTreeData;
    }

    public BehaviorTreeData GetBehaviorInfo(string handleFile)
    {
        BehaviorTreeData skillHsmData = null;
        if (_behaviorDic.TryGetValue(handleFile, out skillHsmData))
        {
            return skillHsmData;
        }

        return skillHsmData;
    }
    #endregion

}
