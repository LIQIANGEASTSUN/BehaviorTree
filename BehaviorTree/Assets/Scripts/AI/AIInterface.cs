using System;
using System.Collections.Generic;
using BehaviorTree;

/*
 * 对AI实现方法的抽象
 * 逻辑决策需要数据的抽象：环境变量
 */
public interface IAIPerformer
{
    void UpdateParameter(string name, bool para);
    void UpdateParameter(string name, int para);
    void UpdateParameter(string name, float para);

    bool GetParameterValue(string parameterName, ref int value);

    bool GetParameterValue(string parameterName, ref float value);

    bool GetParameterValue(string parameterName, ref bool value);

}

/*
 * 只定义外部关心的接口
 */
public interface ICSActionsProvider
{
    bool IsIdle();

    bool CanBeAttacked();
}

/*
 * 只定义外部关心的接口
 */
public interface INAActionsProvider
{
    //检查NormalArticle现在是否能被采集，参数代表是否只计算工作中的角色
    bool CanBeCollected(bool justIncludeDoing);
    //检查NormalArticle现在是否能被建造
    bool CanBeBuilt(bool justIncludeDoing);
    //检查NormalArticle现在是否能被攻击
    bool CanBeAttacked(bool justIncludeDoing);

}

// 被攻击
public interface IBeAttack
{
    void BeAttack(string hitSound, float damage);
}