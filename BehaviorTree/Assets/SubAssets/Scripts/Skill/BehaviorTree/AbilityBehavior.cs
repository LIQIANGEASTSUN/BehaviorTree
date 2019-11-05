using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
//using GenPB;

public class AbilityBehavior : IAbilityEnvironment, IAction
{
    //private Skill _skill;
    private NodeBase _rootNode = null;
    private IConditionCheck _iconditionCheck = null;
    //private AbilityInputExtend _abilityInputExtend = null;

    protected const string Skill_State       = "Skill_State";
    protected const string SkillRequestState = "Skill_Change_State";
    protected const string EnableFire        = "EnableFire";
    protected const string EnergyEnougth     = "EnergyEnougth";
    protected const string EnableActive      = "EnableActive";
    protected const string Hold              = "Hold";
    protected const string Phase_End         = "PhaseEnd";
    protected const string FocoFull          = "FocoFull";

    protected Dictionary<int, string> _btnNameDic = new Dictionary<int, string>() {
        //{(int)AbilityButtonType.GENERAL,      "GenericBtn" }, // 普通技能按钮
        //{(int)AbilityButtonType.SKILL_DEPUTY, "DeputyBtn" },  // 副技能按钮
        //{(int)AbilityButtonType.SKILL_UNIQUE, "UniqueBtn" },  // 大招按钮
        //{(int)AbilityButtonType.FIRE,         "FireBtn" },    // 释放按钮
        //{(int)AbilityButtonType.TRANSFER,     "TransferBtn" },// 传送按钮
    };

    protected Dictionary<int, string> _skillConfigDic = new Dictionary<int, string>() {
        //{ (int)SkillHandleType.CONTINUOUS_FIRE,     "AbilityGeneric"},
        //{ (int)SkillHandleType.DEPUTY_SKILL_FOCO,   "AbliityFocoFull"},
        //{ (int)SkillHandleType.DEPUTY_SKILL_THROW,  "AbilityThrow" },
    };

    public AbilityBehavior(/*Skill skill*/)
    {
        //_skill = skill;

        Init();
    }

    public void Init()
    {
        _iconditionCheck = new ConditionCheck();
        //_abilityInputExtend = new AbilityInputExtend();

        string configName = string.Empty;
        //if (!_skillConfigDic.TryGetValue(_skill.SkillData.HandleType, out configName))
        //{
        //    return;
        //}

        TextAsset textAsset = Resources.Load<TextAsset>(configName);
        if (null == textAsset)
        {
            return;
        }
        BehaviorAnalysis analysis = new BehaviorAnalysis();
        _rootNode = analysis.Analysis(textAsset.text, this, _iconditionCheck);

        Clear();
    }

    public void Update()
    {
        UpdateEnvironment();

        if (null != _rootNode)
        {
            _rootNode.Execute();
        }
    }

    #region ConditionCheck
    public ConditionCheck ConditionCheck
    {
        get { return (ConditionCheck)_iconditionCheck; }
    }

    //public void Input(AbilityButtonType buttonType, AbilityHandleType handleType)
    //{
    //    if (handleType == AbilityHandleType.PRESS)  // 只处理 Down、Up
    //    {
    //        return;
    //    }

    //    string btnName = string.Empty;
    //    if (!_btnNameDic.TryGetValue((int)buttonType, out btnName))
    //    {
    //        return;
    //    }

    //    Debug.LogError("Input:" + btnName + "   " + (int)handleType);
    //    ConditionCheck.SetParameter(btnName, (int)handleType);
    //}

    //public void ChangeState(SkillConfigSkillPhaseType phaseType)
    //{
    //    if (phaseType >= SkillConfigSkillPhaseType.INITIAL_PHASE && phaseType <= SkillConfigSkillPhaseType.NONE)
    //    {
    //        ConditionCheck.SetParameter(Skill_State, (int)phaseType);
    //    }
    //}

    #endregion

    //public void ChangeState(SkillPhaseType type)
    //{
    //    Debug.LogError("ChangeState:" + type);
    //    ConditionCheck.SetParameter(Hold, false);
    //    ConditionCheck.SetParameter(Phase_End, false);
    //}

    public void SkillEnd()
    {
        Clear();
    }

    private void Clear()
    {
        ConditionCheck.SetParameter(Skill_State, 4);
        ConditionCheck.SetParameter(Hold, false);
        ConditionCheck.SetParameter(Phase_End, false);
        ConditionCheck.SetParameter(FocoFull, false);
    }

    //public void Receive(int weaponId, SkillPhaseType type, float focoPercentage, int ret)
    //{
    //    _abilityInputExtend.RemoveRequest(type);
    //}

    //private void Request(int roleId, int weaponId, int type, float focoPercentage)
    //{
    //    if (_abilityInputExtend.IsWaitRequest((SkillPhaseType)type))
    //    {
    //        return;
    //    }

    //    Debug.LogError("Request: roleId:" + roleId + "    weaponId:" + weaponId + "    type:" + (SkillPhaseType)type);
    //    _abilityInputExtend.AddRequest((SkillPhaseType)type);
    //    SkillEventHandler.Instance.SendSkillOper(roleId, _skill.weaponId, (int)type, focoPercentage, 0);
    //    if (type == (int)SkillPhaseType.START)
    //    {
    //        _skill.ResetCoolDown();
    //    }
    //    else if (type == (int)SkillPhaseType.FIRE_PHASE)
    //    {
    //        _skill.ResetNextShotTime();
    //    }
    //}

    //public void SkillEvent(SkillConfigSkillPhaseType phaseType, SkillConfigSkillCustomEvent customEvent)
    //{
    //    if (customEvent.EventBase.EventType != (int)SkillEventType.HOLD)
    //    {
    //        return;
    //    }
    //    //Debug.LogError("Hold:" + phaseType + "    " + (SkillEventType)(customEvent.EventBase.EventType));
    //    ConditionCheck.SetParameter(Hold, true);
    //}

    //public void PhaseEnd(SkillConfigSkillPhaseType phaseType)
    //{
    //    //Debug.LogError("PhaseEnd:" + phaseType);
    //    ConditionCheck.SetParameter(Phase_End, true);
    //}

    #region IAction
    public bool DoAction(int nodeId, List<BehaviorParameter> parameterList)
    {
        bool result = true;
        for (int i = 0; i < parameterList.Count; ++i)
        {
            bool value = DoAction(nodeId, parameterList[i]);
            if (!value)
            {
                result = false;
            }
        }

        return result;
    }

    public bool DoAction(int nodeId, BehaviorParameter parameter)
    {
        if (parameter.parameterName.CompareTo(SkillRequestState) == 0)
        {
            Debug.LogError("nodeId:" + nodeId);
            //float percentage = (null != _skill.FocoEnergia) ? _skill.FocoEnergia.GetPercentage() : 0;
            //Request(_skill.RoleId, _skill.weaponId, parameter.intValue, percentage);
        }

        return true;
    }
    #endregion

    #region IAbilityEnvironment
    public void UpdateEnvironment()
    {
        if (null == ConditionCheck)
        {
            return;
        }
        int result = 0;

        //Debug.LogError("EnableFire:" + _skill.EnableFire(ref result));
        //ConditionCheck.SetParameter(EnableFire, _skill.EnableFire(ref result));

        ////Debug.LogError("EnergyEnougth:" + _skill.SkillEnergyEnough());
        //ConditionCheck.SetParameter(EnergyEnougth, _skill.SkillEnergyEnough());

        //ConditionCheck.SetParameter(EnableActive, _skill.EnableActive(ref result));
        //ConditionCheck.SetParameter(Skill_State, (int)_skill.skillStateMachine.currentState.phaseType);

        FocoEnergia();
    }

    private void FocoEnergia()
    {
        //if (null == _skill || null == _skill.FocoEnergia || !_skill.FocoEnergia.IsStart())
        //{
        //    return;
        //}
        //bool focoFull = (_skill.FocoEnergia.GetPercentage() >= 1);
        //ConditionCheck.SetParameter(FocoFull, focoFull);
        //if (focoFull)
        //{
        //    Debug.LogError("FocoFull");
        //}
    }
    #endregion

}
