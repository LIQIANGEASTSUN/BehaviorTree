using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using BehaviorTree;
using System.IO;

public class BehaviorManager
{
    public static readonly BehaviorManager Instance = new BehaviorManager();

    public delegate void BehaviorChangeRootNode(int nodeId);
    public delegate void BehaviorChangeSelectId(int nodeId);
    public delegate void BehaviorAddNode(Node_Draw_Info_Item info, Vector3 mousePosition, int openSubTree);
    public delegate void BehaviorDeleteNode(int nodeId);
    public delegate void BehaviorRemoveParentNode(int nodeId);
    public delegate void BehaviorLoadFile(string fileName);
    public delegate void BehaviorSaveFile(string fileName);
    public delegate void BehaviorDeleteFile(string fileName);
    public delegate void BehaviorNodeAddChild(int parentId, int childId);
    public delegate void BehaviorNodeParameter(int nodeId, BehaviorParameter parameter, bool isAdd);
    public delegate void BehaviorParameterChange(BehaviorParameter parameter, bool isAdd);
    public delegate void BehaviorNodeChangeParameter(int nodeId, string oldParameter, string newParameter);
    public delegate void BehaviorRuntimePlay(BehaviorPlayType state);
    public delegate void BehaviorAddDelConditionGroup(int nodeId, int groupId, bool isAdd);
    public delegate void BehaviorShowChildNode(int nodeId, bool show);
    public delegate void BehaviorOpenSubTree(int nodeId);
    public delegate void BehaviorChangeSubTreeEntryNode(int subTreeNodeId, int nodeId);

    private string _filePath = string.Empty;
    private string _fileName = string.Empty;
    private BehaviorTreeData _behaviorTreeData;
    private BehaviorPlayType _playState = BehaviorPlayType.STOP;

    // 当前选择的节点
    private int _currentSelectId = 0;
    // 当前选择的子树节点
    private int _currentOpenSubTreeId = -1;

    public static BehaviorChangeRootNode behaviorChangeRootNode;
    public static BehaviorChangeSelectId behaviorChangeSelectId;
    public static BehaviorAddNode behaviorAddNode;
    public static BehaviorDeleteNode behaviorDeleteNode;
    public static BehaviorRemoveParentNode behaviorRemoveParentNode;
    public static BehaviorLoadFile behaviorLoadFile;
    public static BehaviorSaveFile behaviorSaveFile;
    public static BehaviorDeleteFile behaviorDeleteFile;
    public static BehaviorNodeAddChild behaviorNodeAddChild;
    public static BehaviorNodeParameter behaviorNodeParameter;
    public static BehaviorParameterChange parameterChange;
    public static BehaviorNodeChangeParameter behaviorNodeChangeParameter;
    public static BehaviorRuntimePlay behaviorRuntimePlay;
    public static BehaviorAddDelConditionGroup behaviorAddDelConditionGroup;
    public static BehaviorShowChildNode behaviorShowChildNode;
    public static BehaviorOpenSubTree behaviorOpenSubTree;
    public static BehaviorChangeSubTreeEntryNode behaviorChangeSubTreeEntryNode;

    public void Init()
    {
        _filePath = "Assets/SubAssets/GameData/BehaviorTree/";
        _fileName = string.Empty;
        _behaviorTreeData = new BehaviorTreeData();

        behaviorChangeRootNode += ChangeRootNode;
        behaviorChangeSelectId += ChangeSelectId;
        behaviorAddNode += AddNode;
        behaviorDeleteNode += DeleteNode;
        behaviorLoadFile += LoadFile;
        behaviorSaveFile += SaveFile;
        behaviorDeleteFile += DeleteFile;
        behaviorNodeAddChild += NodeAddChild;
        behaviorRemoveParentNode += RemoveParentNode;
        behaviorNodeParameter += NodeParameterChange;
        parameterChange += ParameterChange;
        behaviorNodeChangeParameter += NodeChangeParameter;
        behaviorRuntimePlay += RuntimePlay;
        behaviorAddDelConditionGroup += NodeAddDelConditionGroup;
        behaviorShowChildNode += ShowChildNode;
        behaviorOpenSubTree += OpenSubTree;
        behaviorChangeSubTreeEntryNode += ChangeSubTreeEntryNode;

        _currentSelectId = -1;
        _currentOpenSubTreeId = -1;

        _playState = BehaviorPlayType.STOP;
    }

    public void OnDestroy()
    {
        behaviorChangeRootNode -= ChangeRootNode;
        behaviorChangeSelectId -= ChangeSelectId;
        behaviorAddNode -= AddNode;
        behaviorDeleteNode -= DeleteNode;
        behaviorLoadFile -= LoadFile;
        behaviorSaveFile -= SaveFile;
        behaviorDeleteFile -= DeleteFile;
        behaviorNodeAddChild -= NodeAddChild;
        behaviorRemoveParentNode -= RemoveParentNode;
        behaviorNodeParameter -= NodeParameterChange;
        parameterChange -= ParameterChange;
        behaviorNodeChangeParameter -= NodeChangeParameter;
        behaviorRuntimePlay -= RuntimePlay;
        behaviorAddDelConditionGroup -= NodeAddDelConditionGroup;
        behaviorShowChildNode -= ShowChildNode;
        behaviorOpenSubTree -= OpenSubTree;
        behaviorChangeSubTreeEntryNode -= ChangeSubTreeEntryNode;

        _playState = BehaviorPlayType.STOP;

        AssetDatabase.Refresh();
        UnityEngine.Caching.ClearCache();
    }

    public void Update()
    {
        CheckNode(_behaviorTreeData.nodeList);
        CheckSubTree(_behaviorTreeData.nodeList);
    }

    public string FileName
    {
        get { return _fileName; }
        set { _fileName = value; }
    }

    public string FilePath
    {
        get { return _filePath; }
    }

    public string GetFilePath(string fileName)
    {
        string path = string.Format("{0}{1}.bytes", FilePath, fileName);
        return path;
    }

    public int CurrentSelectId
    {
        get { return _currentSelectId; }
    }

    public BehaviorTreeData BehaviorTreeData
    {
        get { return _behaviorTreeData; }
        set { _behaviorTreeData = value;
            _fileName = _behaviorTreeData.fileName;
        }
    }

    public int CurrentOpenSubTreeId
    {
        get { return _currentOpenSubTreeId; }
    }

    public BehaviorPlayType PlayType
    {
        get { return _playState; }
    }

    public NodeValue CurrentNode
    {
        get
        {
            return GetNode(_currentSelectId);
        }
    }

    public List<NodeValue> NodeList
    {
        get
        {
            return _behaviorTreeData.nodeList;
        }
    }

    private void LoadFile(string fileName)
    {
        string path = GetFilePath(fileName);
        if (!File.Exists(path))
        {
            if (!EditorUtility.DisplayDialog("提示", "文件不存在", "yes"))
            { }
            return;
        }

        _playState = BehaviorPlayType.STOP;
        NodeNotify.SetPlayState((int)_playState);

        BehaviorReadWrite readWrite = new BehaviorReadWrite();
        BehaviorTreeData behaviorTreeData = readWrite.ReadJson(path);
        if (null == behaviorTreeData)
        {
            //ProDebug.Logger.LogError("file is null:" + fileName);
            return;
        }

        _fileName = fileName;
        _behaviorTreeData = behaviorTreeData;
        _currentSelectId = -1;
        _currentOpenSubTreeId = -1;

        for (int i = 0; i < _behaviorTreeData.nodeList.Count; ++i)
        {
            _behaviorTreeData.nodeList[i].showChildNode = true;
            _behaviorTreeData.nodeList[i].show = true;
        }

        BehaviorRunTime.Instance.Reset(behaviorTreeData);
    }

    private void SaveFile(string fileName)
    {
        if (_behaviorTreeData == null)
        {
            //ProDebug.Logger.LogError("rootNode is null");
            return;
        }

        string path = GetFilePath(fileName);
        if (File.Exists(path))
        {
            if (!EditorUtility.DisplayDialog("已存在文件", "是否替换新文件", "替换", "取消"))
            {
                return;
            }
        }

        // 如果项目总不包含该路径，创建一个
        string directory = Path.GetDirectoryName(path);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        BehaviorReadWrite readWrite = new BehaviorReadWrite();
        _behaviorTreeData.fileName = fileName;
        readWrite.WriteJson(_behaviorTreeData, path);
    }

    private void DeleteFile(string fileName)
    {
        string path = GetFilePath(fileName);
        if (!File.Exists(path))
        {
            if (!EditorUtility.DisplayDialog("提示", "文件不存在", "yes"))
            { }
            return;
        }

        File.Delete(path);
    }

    private void NodeAddChild(int parentId, int childId)
    {
        NodeValue parentNode = GetNode(parentId);
        NodeValue childNode = GetNode(childId);

        if (null == parentNode || null == childNode)
        {
            //ProDebug.Logger.LogError("node is null");
            return;
        }

        string msg = string.Empty;
        bool result = true;
        if (childNode.parentNodeID >= 0)
        {
            result = false;
            if (childNode.parentNodeID != parentId)
            {
                msg = "已经有父节点";
            }
            else
            {
                msg = "不能重复添加父节点";
            }
        }

        if (parentNode.parentNodeID >= 0 && parentNode.parentNodeID == childNode.id)
        {
            msg = "不能添加父节点作为子节点";
            result = false;
        }

        if (!result && TreeNodeWindow.window != null)
        {
            TreeNodeWindow.window.ShowNotification(msg);
        }

        if (!result)
        {
            return;
        }

        // 修饰节点只能有一个子节点
        if (parentNode.NodeType >= (int)NODE_TYPE.DECORATOR_INVERTER && parentNode.NodeType <= (int)NODE_TYPE.DECORATOR_UNTIL_SUCCESS)
        {
            for (int i = 0; i < parentNode.childNodeList.Count; ++i)
            {
                NodeValue node = GetNode(parentNode.childNodeList[i]);
                if (null != node)
                {
                    node.parentNodeID = -1;
                }
            }
            parentNode.childNodeList.Clear();
        }

        parentNode.childNodeList.Add(childNode.id);
        childNode.parentNodeID = parentNode.id;
    }

    private void RemoveParentNode(int nodeId)
    {
        NodeValue nodeValue = GetNode(nodeId);
        if (nodeValue.parentNodeID < 0)
        {
            return;
        }

        NodeValue parentNode = GetNode(nodeValue.parentNodeID);
        if (null != parentNode)
        {
            for (int i = 0; i < parentNode.childNodeList.Count; ++i)
            {
                int childId = parentNode.childNodeList[i];
                NodeValue childNode = GetNode(childId);
                if (childNode.id == nodeValue.id)
                {
                    parentNode.childNodeList.RemoveAt(i);
                    break;
                }
            }
        }
        
        nodeValue.parentNodeID = -1;
    }

    private void NodeParameterChange(int nodeId, BehaviorParameter parameter, bool isAdd)
    {
        NodeValue nodeValue = GetNode(nodeId);
        if (null == nodeValue)
        {
            return;
        }

        if (isAdd)
        {
            AddParameter(nodeValue.parameterList, parameter, true);
        }
        else
        {
            DelParameter(nodeValue.parameterList, parameter);
        }

        for (int i = 0; i < nodeValue.parameterList.Count; ++i)
        {
            nodeValue.parameterList[i].index = i;
        }
    }

    private void NodeChangeParameter(int nodeId, string oldParameter, string newParameter)
    {
        NodeValue nodeValue = GetNode(nodeId);
        if (null == nodeValue)
        {
            return;
        }

        BehaviorParameter parameter = null;
        for (int i = 0; i < _behaviorTreeData.parameterList.Count; ++i)
        {
            BehaviorParameter temp = _behaviorTreeData.parameterList[i];
            if (temp.parameterName.CompareTo(newParameter) == 0)
            {
                parameter = temp;
            }
        }

        if (null == parameter)
        {
            return;
        }

        for (int i = 0; i < nodeValue.parameterList.Count; ++i)
        {
            BehaviorParameter temp = nodeValue.parameterList[i];
            if (temp.parameterName.CompareTo(parameter.parameterName) == 0)
            {
                nodeValue.parameterList[i] = parameter.Clone();
                break;
            }
        }

        for (int i = 0; i < nodeValue.parameterList.Count; ++i)
        {
            nodeValue.parameterList[i].index = i;
        }

    }

    private void RuntimePlay(BehaviorPlayType state)
    {
        NodeNotify.SetPlayState((int)state);
        _playState = state;
    }

    private void ParameterChange(BehaviorParameter parameter, bool isAdd)
    {
        if (isAdd)
        {
            AddParameter(_behaviorTreeData.parameterList, parameter);
        }
        else
        {
            DelParameter(_behaviorTreeData.parameterList, parameter);
        }

        for (int i = 0; i < _behaviorTreeData.parameterList.Count; ++i)
        {
            _behaviorTreeData.parameterList[i].index = i;
        }
    }

    private bool AddParameter(List<BehaviorParameter> parameterList, BehaviorParameter parameter, bool repeatAdd = false)
    {
        bool result = true;
        if (string.IsNullOrEmpty(parameter.parameterName))
        {
            string meg = string.Format("条件参数不能为空", parameter.parameterName);
            TreeNodeWindow.window.ShowNotification(meg);
            result = false;
        }

        for (int i = 0; i < parameterList.Count; ++i)
        {
            BehaviorParameter tempParameter = parameterList[i];
            if (!repeatAdd && tempParameter.parameterName.CompareTo(parameter.parameterName) == 0)
            {
                string meg = string.Format("条件参数:{0} 已存在", parameter.parameterName);
                TreeNodeWindow.window.ShowNotification(meg);
                result = false;
                break;
            }
        }

        if (result)
        {
            BehaviorParameter newParameter = parameter.Clone();
            parameterList.Add(newParameter);
        }

        return result;
    }

    private void DelParameter(List<BehaviorParameter> parameterList, BehaviorParameter parameter)
    {
        for (int i = 0; i < parameterList.Count; ++i)
        {
            BehaviorParameter tempParameter = parameterList[i];
            if (tempParameter.parameterName.CompareTo(parameter.parameterName) == 0)
            {
                parameterList.RemoveAt(i);
                break;
            }
        }
    }

    private void NodeAddDelConditionGroup(int nodeId, int groupId, bool isAdd)
    {
        NodeValue nodeValue = GetNode(nodeId);
        if (null == nodeValue)
        {
            return;
        }

        if (isAdd)
        {
            for (int i = 0; i < nodeValue.conditionGroupList.Count + 1; ++i)
            {
                ConditionGroup conditionGroup = nodeValue.conditionGroupList.Find(a => a.index == i);
                if (null == conditionGroup)
                {
                    conditionGroup = new ConditionGroup();
                    conditionGroup.index = i;
                    nodeValue.conditionGroupList.Add(conditionGroup);
                    break;
                }
            }

            if (nodeValue.conditionGroupList.Count <= 0)
            {
                ConditionGroup conditionGroup = new ConditionGroup();
                conditionGroup.index = 0;
                nodeValue.conditionGroupList.Add(conditionGroup);
            }
        }
        else
        {
            for (int i = 0; i < nodeValue.conditionGroupList.Count; ++i)
            {
                if (nodeValue.conditionGroupList[i].index == groupId)
                {
                    nodeValue.conditionGroupList.RemoveAt(i);
                }
            }
        }
    }

    private void OpenSubTree(int nodeId)
    {
        _currentOpenSubTreeId = nodeId;
    }

    private void ChangeSubTreeEntryNode(int subTreeNodeId, int nodeId)
    {
        NodeValue nodeValue = GetNode(nodeId);
        if (null == nodeValue)
        {
            return;
        }

        NodeValue subTreeNode = GetNode(subTreeNodeId);
        if (null == subTreeNode)
        {
            return;
        }

        for (int i = 0; i < NodeList.Count; ++i)
        {
            if (NodeList[i].parentSubTreeNodeId == nodeValue.parentSubTreeNodeId)
            {
                NodeList[i].subTreeEntry = (NodeList[i].id == nodeId);
                if (NodeList[i].subTreeEntry)
                {
                    subTreeNode.childNodeList.Clear();
                    subTreeNode.childNodeList.Add(nodeId);
                }
            }
        }
    }

    private void ShowChildNode(int nodeId, bool show)
    {
        NodeValue nodeValue = GetNode(nodeId);
        if (null == nodeValue)
        {
            return;
        }

        SetChildNodeShow(nodeValue, show);
    }

    private void SetChildNodeShow(NodeValue nodeValue, bool show)
    {
        for (int i = 0; i < nodeValue.childNodeList.Count; ++i)
        {
            int childId = nodeValue.childNodeList[i];
            NodeValue childNode = GetNode(childId);
            if (null == childNode)
            {
                continue;
            }

            childNode.show = show;
            SetChildNodeShow(childNode, show);
        }
    }

    private void ChangeRootNode(int rootNodeId)
    {
        _behaviorTreeData.rootNodeId = rootNodeId;

        for (int i = 0; i < NodeList.Count; ++i)
        {
            NodeList[i].isRootNode = (NodeList[i].id == rootNodeId);
        }
    }

    private void ChangeSelectId(int nodeId)
    {
        _currentSelectId = nodeId;
    }

    public NodeValue GetNode(int nodeId)
    {
        for (int i = 0; i < NodeList.Count; ++i)
        {
            NodeValue nodeValue = NodeList[i];
            if (nodeValue.id == nodeId)
            {
                return nodeValue;
            }
        }

        return null;
    }

    // 添加节点
    private void AddNode(Node_Draw_Info_Item info, Vector3 mousePosition, int openSubTreeId)
    {
        NodeValue newNodeValue = new NodeValue();
        newNodeValue.id = GetNewNodeId();
        if (_behaviorTreeData.rootNodeId < 0)
        {
            _behaviorTreeData.rootNodeId = newNodeValue.id;
            newNodeValue.isRootNode = true;
        }

        newNodeValue.nodeName = info._nodeName;
        newNodeValue.identification = info._identification;
        newNodeValue.NodeType = (int)info._nodeType;
        newNodeValue.parentNodeID = -1;
        newNodeValue.function = NodeDescript.GetFunction((NODE_TYPE)info._nodeType);

        RectT rectT = new RectT();
        Rect rect = new Rect(mousePosition.x, mousePosition.y, rectT.width, rectT.height);
        newNodeValue.position = RectTool.RectToRectT(rect);

        newNodeValue.parentSubTreeNodeId = openSubTreeId;

        NodeList.Add(newNodeValue);

        if (openSubTreeId >= 0)
        {
            bool hasEntryNode = false;
            for (int i = 0; i < NodeList.Count; ++i)
            {
                if (NodeList[i].parentSubTreeNodeId == openSubTreeId
                    && (NodeList[i].subTreeEntry))
                {
                    hasEntryNode = true;
                    break;
                }
            }
            if (!hasEntryNode)
            {
                ChangeSubTreeEntryNode(newNodeValue.parentSubTreeNodeId, newNodeValue.id);
            }
        }
    }

    private int GetNewNodeId()
    {
        int id = -1;
        int index = -1;
        while (id == -1)
        {
            ++index;
            id = index;
            for (int i = 0; i < NodeList.Count; ++i)
            {
                if (NodeList[i].id == index)
                {
                    id = -1;
                }
            }
        }

        return id;
    }

    // 删除节点
    private void DeleteNode(int nodeId)
    {
        for (int i = 0; i < NodeList.Count; ++i)
        {
            NodeValue nodeValue = NodeList[i];
            if (nodeValue.id != nodeId)
            {
                continue;
            }

            RemoveParentNode(nodeValue.id);
            for (int j = 0; j < nodeValue.childNodeList.Count; ++j)
            {
                int childId = nodeValue.childNodeList[j];
                NodeValue childNode = GetNode(childId);
                childNode.parentNodeID = -1;
            }

            NodeList.RemoveAt(i);

            if (nodeValue.NodeType == (int)NODE_TYPE.SUB_TREE)
            {
                for (int j = NodeList.Count - 1; j >= 0; --j)
                {
                    NodeValue node = NodeList[j];
                    if (node.parentSubTreeNodeId == nodeValue.id)
                    {
                        NodeList.RemoveAt(j);
                    }
                }
            }
            break;
        }
    }

    private static int[] rootNodeArr = null;
    private static void CheckNode(List<NodeValue> nodeValueList)
    {
        int rootNodeCount = 0;
        NodeValue invalidNodeValue = null;
        rootNodeArr = new int[nodeValueList.Count];

        bool rootNodeHasParent = false;
        // 开始绘制节点 
        // 注意：必须在  BeginWindows(); 和 EndWindows(); 之间 调用 GUI.Window 才能显示
        for (int i = 0; i < nodeValueList.Count; i++)
        {
            NodeValue nodeValue = nodeValueList[i];
            if (nodeValue.isRootNode)
            {
                rootNodeArr[rootNodeCount] = nodeValue.id;
                ++rootNodeCount;
                if (nodeValue.parentNodeID >= 0)
                {
                    rootNodeHasParent = true;
                }
            }

            if (((NODE_TYPE)nodeValue.NodeType == NODE_TYPE.CONDITION || (NODE_TYPE)nodeValue.NodeType == NODE_TYPE.ACTION) && nodeValue.childNodeList.Count > 0)
            {
                invalidNodeValue = nodeValue;  // 叶节点 不能有子节点
            }
        }

        string meg = string.Empty;
        if (rootNodeCount > 1)
        {
            meg = string.Format("有多个根节点:");
            for (int i = 0; i < rootNodeCount; ++i)
            {
                meg += string.Format("_{0} ", rootNodeArr[i]);
            }
        }
        else if (rootNodeCount == 0)
        {
            meg = "必须有一个根节点";
        }
        else if (rootNodeHasParent)
        {
            meg = string.Format("跟节点_{0} 不能有父节点", rootNodeArr[0]);
        }

        if (null != invalidNodeValue)
        {
            int index = EnumNames.GetEnumIndex<NODE_TYPE>((NODE_TYPE)invalidNodeValue.NodeType);
            string name = EnumNames.GetEnumName<NODE_TYPE>(index);
            meg = string.Format("节点:{0} {1} 不能有子节点", invalidNodeValue.id, name);
        }

        if (TreeNodeWindow.window != null && !string.IsNullOrEmpty(meg))
        {
            TreeNodeWindow.window.ShowNotification(meg);
        }
    }

    private static Dictionary<int, int> subTreeEntryHash = new Dictionary<int, int>();
    private static void CheckSubTree(List<NodeValue> nodeValueList)
    {
        subTreeEntryHash.Clear();
        // 开始绘制节点 
        // 注意：必须在  BeginWindows(); 和 EndWindows(); 之间 调用 GUI.Window 才能显示
        for (int i = 0; i < nodeValueList.Count; i++)
        {
            NodeValue nodeValue = nodeValueList[i];
            if (nodeValue.parentSubTreeNodeId < 0)
            {
                continue;
            }

            if (!subTreeEntryHash.ContainsKey(nodeValue.parentSubTreeNodeId))
            {
                subTreeEntryHash[nodeValue.parentSubTreeNodeId] = 0;
            }

            if (nodeValue.subTreeEntry)
            {
                subTreeEntryHash[nodeValue.parentSubTreeNodeId]++;
            }
        }

        string meg = string.Empty;
        foreach (var kv in subTreeEntryHash)
        {
            int subTreeId = kv.Key;
            int count = kv.Value;
            if (count == 0)
            {
                meg = string.Format("子树_{0} 没有入口节点", subTreeId);
                break;
            }
            if (count > 1)
            {
                meg = string.Format("子树_{0} 只能有一个入口节点", subTreeId);
                break;
            }
        }

        if (TreeNodeWindow.window != null && !string.IsNullOrEmpty(meg))
        {
            TreeNodeWindow.window.ShowNotification(meg);
        }
    }

}
