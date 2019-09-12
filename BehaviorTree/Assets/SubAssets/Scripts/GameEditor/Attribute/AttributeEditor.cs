using UnityEngine;
using System;
using System.Reflection;

/// <summary>
/// 设置枚举名称
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public class EnumAttirbute : PropertyAttribute
{
    /// <summary>
    /// 枚举名称
    /// </summary>
    public string name;
    public EnumAttirbute(string name)
    {
        this.name = name;
    }
}

public class EnumNames
{
    public static string[] GetEnumNames<T>()
    {
        string[] names = Enum.GetNames(typeof(T));
        string[] values = new string[names.Length];
        for (int i = 0; i < names.Length; ++i)
        {
            FieldInfo info = typeof(T).GetField(names[i]);
            EnumAttirbute[] enumAttributes = (EnumAttirbute[])info.GetCustomAttributes(typeof(EnumAttirbute), false);
            values[i] = enumAttributes.Length == 0 ? names[i] : enumAttributes[0].name;
        }
        return values;
    }

    public static string GetEnumName<T>(int index)
    {
        string[] names = Enum.GetNames(typeof(T));
        string value = string.Empty;
        for (int i = 0; i < names.Length; ++i)
        {
            if (i != index)
            {
                continue;
            }
            FieldInfo info = typeof(T).GetField(names[i]);
            EnumAttirbute[] enumAttributes = (EnumAttirbute[])info.GetCustomAttributes(typeof(EnumAttirbute), false);
            value = enumAttributes.Length == 0 ? names[i] : enumAttributes[0].name;
        }

        return value;
    }

    public static int GetEnumIndex<T>(T t)
    {
        int enumIndex = 0;
        string[] names = System.Enum.GetNames(typeof(T));
        for (int i = 0; i < names.Length; ++i)
        {
            string name = System.Enum.GetName(typeof(T), t);
            if (name.CompareTo(names[i]) == 0)
            {
                enumIndex = i;
                break;
            }
        }

        return enumIndex;
    }

    public static T GetEnum<T>(int index)
    {
        string[] names = System.Enum.GetNames(typeof(T));
        string selectName = names[index];
        T t = (T)System.Enum.Parse(typeof(T), selectName);
        return t;
    }
}