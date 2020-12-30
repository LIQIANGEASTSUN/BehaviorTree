using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using BehaviorTree;
using System;

public class BehaviorDrawController
{
    private TreeNodeWindow _treeNodeWindow = null;

    public BehaviorDrawModel _behaviorDrawModel = null;
    private BehaviorDrawView _behaviorDrawView = null;

    public void Init()
    {
        _behaviorDrawModel = new BehaviorDrawModel();
        _behaviorDrawView = new BehaviorDrawView();
        _behaviorDrawView.SetModel(_behaviorDrawModel);
    }

    public void OnDestroy()
    {

    }

    public void OnGUI(TreeNodeWindow window)
    {
        _treeNodeWindow = window;
        _behaviorDrawView.Init(_treeNodeWindow, this);

        NodeValue currentNode = _behaviorDrawModel.GetCurrentSelectNode();

        List<NodeValue> nodeList = new List<NodeValue>();
        if (BehaviorManager.Instance.CurrentOpenSubTreeId >= 0)
        {
            nodeList = _behaviorDrawModel.GetSubTreeNode(BehaviorManager.Instance.CurrentOpenSubTreeId);
        }
        else
        {
            nodeList = _behaviorDrawModel.GetBaseNode();
        }

        nodeList = CheckDrawNode(nodeList);

        _behaviorDrawView.Draw(_treeNodeWindow.position, currentNode, nodeList);
    }

    private List<NodeValue> CheckDrawNode(List<NodeValue> nodeList)
    {
        if ( BehaviorManager.Instance.RunTimeInvalidSubTreeHash.Count <= 0)
        {
            return nodeList;
        }

        for (int i = nodeList.Count - 1; i >= 0; --i)
        {
            NodeValue nodeValue = nodeList[i];
            if (nodeValue.NodeType == (int)NODE_TYPE.SUB_TREE)
            {
                if (BehaviorManager.Instance.RunTimeInvalidSubTreeHash.Contains(nodeValue.id))
                {
                    nodeList.RemoveAt(i);
                }
            }
        }

        return nodeList;
    }

}

public class Node_Draw_Info
{
    public string _nodeTypeName;
    public List<KeyValuePair<string, Node_Draw_Info_Item>> _nodeArr = new List<KeyValuePair<string, Node_Draw_Info_Item>>();
    
    public Node_Draw_Info(string name)
    {
        _nodeTypeName = name;
    }

    public void AddNodeType(NODE_TYPE nodeType)
    {
        Node_Draw_Info_Item item = new Node_Draw_Info_Item(nodeType);
        item.GetTypeName();
        string name = string.Format("{0}/{1}", _nodeTypeName, item._nodeName);
        KeyValuePair<string, Node_Draw_Info_Item> kv = new KeyValuePair<string, Node_Draw_Info_Item>(name, item);
        _nodeArr.Add(kv);
    }

    public void AddNodeType(NODE_TYPE nodeType, string nodeName, string identificationName)
    {
        Node_Draw_Info_Item item = new Node_Draw_Info_Item(nodeType);
        item.SetName(nodeName);
        item.SetIdentification(identificationName);
        string name = string.Format("{0}/{1}", _nodeTypeName, nodeName);
        KeyValuePair<string, Node_Draw_Info_Item> kv = new KeyValuePair<string, Node_Draw_Info_Item>(name, item);
        _nodeArr.Add(kv);
    }

}

public class Node_Draw_Info_Item
{
    public string _nodeName = string.Empty;
    public NODE_TYPE _nodeType;
    public string _identificationName = string.Empty;

    public Node_Draw_Info_Item(NODE_TYPE nodeType)
    {
        _nodeType = nodeType;
    }

    public void GetTypeName()
    {
        int index = EnumNames.GetEnumIndex<NODE_TYPE>(_nodeType);
        _nodeName = EnumNames.GetEnumName<NODE_TYPE>(index);
    }

    public void SetName(string name)
    {
        _nodeName = name;
    }

    public void SetIdentification(string identificationName)
    {
        _identificationName = identificationName;
    }

}


public class BehaviorDrawModel
{
    private List<Node_Draw_Info> infoList = new List<Node_Draw_Info>();

    public BehaviorDrawModel()
    {
        infoList.Clear();
        SetInfoList();
    }

    public NodeValue GetCurrentSelectNode()
    {
        return BehaviorManager.Instance.CurrentNode;
    }

    public List<NodeValue> GetNodeList()
    {
        return BehaviorManager.Instance.NodeList;
    }

    public List<NodeValue> GetBaseNode()
    {
        List<NodeValue> nodeList = new List<NodeValue>();
        List<NodeValue> allNodeList = GetNodeList();
        for (int i = 0; i < allNodeList.Count; ++i)
        {
            NodeValue nodeValue = allNodeList[i];
            if (nodeValue.parentSubTreeNodeId < 0)
            {
                nodeList.Add(nodeValue);
            }
        }
        
        return nodeList;
    }

    public List<NodeValue> GetSubTreeNode(int currentOpenSubTreeId)
    {
        List<NodeValue> nodeList = new List<NodeValue>();

        NodeValue subTreeNode = BehaviorManager.Instance.GetNode(currentOpenSubTreeId);
        if (null == subTreeNode)
        {
            return nodeList;
        }

        if (subTreeNode.subTreeType == (int)SUB_TREE_TYPE.NORMAL)
        {
            List<NodeValue> allNodeList = GetNodeList();
            for (int i = 0; i < allNodeList.Count; ++i)
            {
                NodeValue nodeValue = allNodeList[i];
                if (currentOpenSubTreeId >= 0 && nodeValue.parentSubTreeNodeId == currentOpenSubTreeId)
                {
                    nodeList.Add(nodeValue);
                }
            }
        }
        else if (subTreeNode.subTreeType == (int)SUB_TREE_TYPE.CONFIG)
        {
            BehaviorTreeData data = null;
            if (null != BehaviorManager.behaviorReadFile)
            {
                data = BehaviorManager.behaviorReadFile(subTreeNode.subTreeConfig, false);
            }
            if (null != data)
            {
                nodeList.AddRange(data.nodeList);
            }
        }

        return nodeList;
    }

    private List<NODE_TYPE[]> nodeList = new List<NODE_TYPE[]>() {
        new NODE_TYPE[] { NODE_TYPE.SELECT, NODE_TYPE.IF_JUDEG },  // 组合节点
        new NODE_TYPE[]{ NODE_TYPE.DECORATOR_INVERTER, NODE_TYPE.DECORATOR_UNTIL_SUCCESS}, // 修饰节点
    };
    private string[] typeNameArr = { "组合节点", "修饰节点" };
    private void SetInfoList()
    {
        #region Node
        for (int i = 0; i < nodeList.Count; ++i)
        {
            string name = string.Format("{0}/{1}", "Add Node", typeNameArr[i]);
            Node_Draw_Info drawInfo = new Node_Draw_Info(name);
            NODE_TYPE[] arr = nodeList[i];
            for (NODE_TYPE nodeType = arr[0]; nodeType <= arr[1]; ++nodeType)
            {
                drawInfo.AddNodeType(nodeType);
                infoList.Add(drawInfo);
            }
        }
        
        {
            // 条件节点
            string conditionName = string.Format("{0}/{1}", "Add Node", "条件节点");
            Node_Draw_Info conditionDrawInfo = new Node_Draw_Info(conditionName);
            infoList.Add(conditionDrawInfo);

            // 行为节点
            string actionName = string.Format("{0}/{1}", "Add Node", "行为节点");
            Node_Draw_Info actionDrawInfo = new Node_Draw_Info(actionName);
            infoList.Add(actionDrawInfo);

            Dictionary<string, ICustomIdentification<NodeLeaf>> nodeDic = CustomNode.Instance.GetNodeDic();
            foreach (var kv in nodeDic)
            {
                ICustomIdentification<NodeLeaf> customIdentification = kv.Value;
                if (customIdentification.NodeType == NODE_TYPE.CONDITION)
                {
                    conditionDrawInfo.AddNodeType(NODE_TYPE.CONDITION, customIdentification.Name, customIdentification.IdentificationName);
                }
                else if (customIdentification.NodeType == NODE_TYPE.ACTION)
                {
                    actionDrawInfo.AddNodeType(NODE_TYPE.ACTION, customIdentification.Name, customIdentification.IdentificationName);
                }
            }
        }
        #endregion

        #region Sub_Tree
        {
            Node_Draw_Info drawInfo = new Node_Draw_Info("AddSubTree");
            drawInfo.AddNodeType(NODE_TYPE.SUB_TREE);
            infoList.Add(drawInfo);
        }
        #endregion
    }

    public List<Node_Draw_Info> InfoList {
        get
        {
            return infoList;
        }
    }

    public string[] GetOptionArr(ref int selectIndex, ref List<int> nodeList)
    {
        nodeList = new List<int>();
        List<string> optionList = new List<string>();
        nodeList.Add(-1);
        optionList.Add("Base");
        selectIndex = nodeList.Count - 1;
        
        if (BehaviorManager.Instance.CurrentOpenSubTreeId >= 0)
        {
            int nodeId = BehaviorManager.Instance.CurrentOpenSubTreeId;
            NodeValue nodeValue = BehaviorManager.Instance.GetNode(nodeId);
            while (null != nodeValue && nodeValue.NodeType == (int)NODE_TYPE.SUB_TREE)
            {
                nodeList.Insert(1, nodeValue.id);
                selectIndex = nodeList.Count - 1;

                string name = GetNodeName(nodeValue);
                optionList.Insert(1, name);

                if (nodeValue.parentSubTreeNodeId > 0)
                {
                    nodeValue = BehaviorManager.Instance.GetNode(nodeValue.parentSubTreeNodeId);
                }
                else
                {
                    break;
                }
            }
        }

        return optionList.ToArray();
    }

    private string GetNodeName(NodeValue nodeValue)
    {
        int nodeIndex = EnumNames.GetEnumIndex<NODE_TYPE>((NODE_TYPE)nodeValue.NodeType);
        string name = EnumNames.GetEnumName<NODE_TYPE>(nodeIndex);
        return string.Format("{0}_{1}", name, nodeValue.id);
    }
}

public class BehaviorDrawView
{
    private TreeNodeWindow _treeNodeWindow = null;
    private BehaviorDrawController _drawController = null;
    private BehaviorDrawModel _behaviorDrawModel = null;

    // 鼠标的位置
    private Vector2 mousePosition;
    // 添加连线
    private bool makeTransitionMode = false;

    private Vector3 scrollPos = Vector2.zero;
    private Rect scrollRect = new Rect(0, 0, 1500, 1000);
    private Rect contentRect = new Rect(0, 0, 3000, 2000);

    private List<NodeValue> _nodeList = new List<NodeValue>();

    public void Init(TreeNodeWindow window, BehaviorDrawController drawController)
    {
        _treeNodeWindow = window;
        _drawController = drawController;
    }

    public void SetModel(BehaviorDrawModel model)
    {
        _behaviorDrawModel = model;
    }

    public void Draw( Rect windowsPosition, NodeValue currentNode, List<NodeValue> nodeList)
    {
        _nodeList = nodeList;

        int currentOpenSubTreeId = BehaviorManager.Instance.CurrentOpenSubTreeId;
        DrawTielt();
        if (currentOpenSubTreeId != BehaviorManager.Instance.CurrentOpenSubTreeId)
        {
            return;
        }

        Rect rect = GUILayoutUtility.GetRect(0f, 0, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        scrollRect = rect;

        contentRect.x = rect.x;
        contentRect.y = rect.y;

        //创建 scrollView  窗口  
        scrollPos = GUI.BeginScrollView(scrollRect, scrollPos, contentRect);
        {
            NodeMakeTransition(currentNode, nodeList);

            DrawNodeWindows(nodeList);

            for (int i = 0; i < nodeList.Count; ++i)
            {
                NodeValue nodeValue = nodeList[i];

                nodeValue.childNodeList.Sort((a, b) =>
                {
                    NodeValue nodeA = BehaviorManager.Instance.GetNode(a);
                    NodeValue nodeB = BehaviorManager.Instance.GetNode(b);
                    return (int)(nodeA.position.x - nodeB.position.x);
                });
            }

            SortChild(nodeList);

            ResetScrollPos(nodeList);
        }
        GUI.EndScrollView();  //结束 ScrollView 窗口  
    }

    private string[] optionArr = new string[] { "Base", "SubTree"};
    private void DrawTielt()
    {
        int selectIndex = 0;
        List<int> idList = new List<int>();
        string[] optionArr = _behaviorDrawModel.GetOptionArr(ref selectIndex, ref idList);
        int option = selectIndex;

        option = GUILayout.Toolbar(option, optionArr, EditorStyles.toolbarButton, GUILayout.Width(optionArr.Length * 200));
        if (option != selectIndex)
        {
            if (null != BehaviorManager.behaviorOpenSubTree)
            {
                int nodeId = idList[option];
                BehaviorManager.behaviorOpenSubTree(nodeId);
            }
        }

    }

    private void NodeMakeTransition(NodeValue currentNode, List<NodeValue> nodeList)
    {
        Event _event = Event.current;
        mousePosition = _event.mousePosition;

        if (_event.type == EventType.MouseDown)
        {
            if (_event.button == 0)  // 鼠标左键
            {
                if (makeTransitionMode)
                {
                    NodeValue nodeValue = GetMouseInNode(nodeList);
                    // 如果按下鼠标时，选中了一个节点，则将 新选中根节点 添加为 selectNode 的子节点
                    if (null != nodeValue && currentNode.id != nodeValue.id)
                    {
                        if (null != BehaviorManager.behaviorNodeAddChild)
                        {
                            BehaviorManager.behaviorNodeAddChild(currentNode.id, nodeValue.id);
                        }
                    }

                    // 取消连线状态
                    makeTransitionMode = false;
                }
                else
                {
                    NodeValue nodeValue = GetMouseInNode(nodeList);
                    ClickNode(nodeValue);
                }
            }
            
            if (_event.button == 1)  // 鼠标右键
            {
                if ((!makeTransitionMode))
                {
                    NodeValue nodeValue = GetMouseInNode(nodeList);
                    ShowMenu(currentNode, nodeValue);
                }
            }
        }

        if (makeTransitionMode && currentNode != null)
        {
            RectT mouseRect = new RectT(mousePosition.x, mousePosition.y, 10, 10);
            DrawNodeCurve(currentNode.position, mouseRect);
        }
    }

    // 绘制节点
    private void DrawNodeWindows(List<NodeValue> nodeList)
    {
        Action CallBack = () =>
        {
            for (int i = 0; i < nodeList.Count; i++)
            {
                NodeValue nodeValue = nodeList[i];
                int xConst = 330;
                int yConst = 0;
                if (nodeValue.position.x < xConst || nodeValue.position.y < yConst)
                {
                    float x = (nodeValue.position.x < xConst ? (xConst - nodeValue.position.x + 30): 0);
                    float y = (nodeValue.position.y < yConst ? (yConst - nodeValue.position.y + 30): 0);

                    Vector2 offset = new Vector2(x, y);
                    nodeValue.position.x += offset.x;
                    nodeValue.position.y += offset.y;

                    SyncChildNodePosition(nodeValue, offset);
                }

                GUI.enabled = !BehaviorManager.Instance.CurrentOpenConfigSubTree();
                string name = string.Format("{0}", nodeValue.nodeName);
                GUIContent nameGui = new GUIContent(name, name);
                Rect rect = GUI.Window(i, RectTool.RectTToRect(nodeValue.position), DrawNodeWindow, nameGui);
                if (!BehaviorManager.Instance.CurrentOpenConfigSubTree())
                {
                    ResetNodePosition(nodeValue, rect);
                }
                GUI.enabled = true;

                if (nodeValue.NodeType != (int)NODE_TYPE.SUB_TREE)
                {
                    DrawToChildCurve(nodeValue);
                }
            }
        };

        _treeNodeWindow.DrawWindow(CallBack);
    }

    void DrawNodeWindow(int id)
    {
        if (id >= _nodeList.Count)
        {
            return;
        }
        NodeValue nodeValue = _nodeList[id];
        NodeEditor.Draw(nodeValue, BehaviorManager.Instance.CurrentSelectId);
        GUI.DragWindow();
    }

    private void ResetNodePosition(NodeValue nodeValue, Rect rect)
    {
        Vector2 nodePos = new Vector2(nodeValue.position.x, nodeValue.position.y);
        nodeValue.position = RectTool.RectToRectT(rect);

        if (!nodeValue.moveWithChild)
        {
            return;
        }

        childHash = new HashSet<int>();
        Vector2 offset = new Vector2(rect.x, rect.y) - nodePos;
        SyncChildNodePosition(nodeValue, offset);
    }

    private HashSet<int> childHash = new HashSet<int>();
    private void SyncChildNodePosition(NodeValue nodeValue, Vector2 offset)
    {
        for (int i = 0; i < nodeValue.childNodeList.Count; ++i)
        {
            int childId = nodeValue.childNodeList[i];
            NodeValue childNode = BehaviorManager.Instance.GetNode(childId);
            childNode.position.x += offset.x;
            childNode.position.y += offset.y;

            if (childHash.Contains(childNode.id))
            {
                //ProDebug.Logger.LogError(//ProDebug.Logger.StrConcat(nodeValue.id, "    ", childNode.id));
                break;
            }
            childHash.Add(childNode.id);
            SyncChildNodePosition(childNode, offset);
        }
    }

    private int _lastClickNodeTime = 0;
    private void ClickNode(NodeValue nodeValue)
    {
        if (null == nodeValue)
        {
            return;
        }

        if (BehaviorManager.behaviorChangeSelectId != null)
        {
            int nodeId = (null != nodeValue) ? nodeValue.id : -1;
            BehaviorManager.behaviorChangeSelectId(nodeId);
        }

        if (nodeValue.NodeType == (int)NODE_TYPE.SUB_TREE)
        {
            int currentTime = (int)(Time.realtimeSinceStartup * 1000);
            if (currentTime - _lastClickNodeTime <= 200)
            {
                if (null != BehaviorManager.behaviorOpenSubTree)
                {
                    BehaviorManager.behaviorOpenSubTree(nodeValue.id);
                }
            }
            _lastClickNodeTime = currentTime;
        }

    }

    // 获取鼠标所在位置的节点
    private NodeValue GetMouseInNode(List<NodeValue> nodeList)
    {
        NodeValue selectNode = null;
        for (int i = 0; i < nodeList.Count; i++)
        {
            NodeValue nodeValue = nodeList[i];
            // 如果鼠标位置 包含在 节点的 Rect 范围，则视为可以选择的节点
            if (RectTool.RectTToRect(nodeValue.position).Contains(mousePosition))
            {
                selectNode = nodeValue;
                break;
            }
        }

        return selectNode;
    }

    private void ShowMenu(NodeValue currentNode, NodeValue nodeValue)
    {
        int menuType = (nodeValue != null) ? 1 : 0;

        GenericMenu menu = new GenericMenu();
        if (menuType == 0)
        {
            GenericMenu.MenuFunction2 CallBack = (object userData) => {
                if (null != BehaviorManager.behaviorAddNode)
                {
                    BehaviorManager.behaviorAddNode((Node_Draw_Info_Item)userData, mousePosition, BehaviorManager.Instance.CurrentOpenSubTreeId);
                }
            };

            List<Node_Draw_Info> nodeList = _drawController._behaviorDrawModel.InfoList;
            for (int i = 0; i < nodeList.Count; ++i)
            {
                Node_Draw_Info draw_Info = nodeList[i];
                for (int j = 0; j < draw_Info._nodeArr.Count; ++j)
                {
                    KeyValuePair<string, Node_Draw_Info_Item> kv = draw_Info._nodeArr[j];
                    //string itemName = string.Format("Add Node/{0}", kv.Key);
                    string itemName = string.Format("{0}", kv.Key);
                    GenericMenuAddItem(menu, new GUIContent(itemName), CallBack, kv.Value);
                }
            }
        }
        else
        {
            if (null != currentNode && nodeValue.id == currentNode.id && (NODE_TYPE)nodeValue.NodeType < NODE_TYPE.CONDITION)
            {
                // 连线子节点
                GenericMenuAddItem(menu, new GUIContent("Make Transition"), MakeTransition);
                menu.AddSeparator("");
            }
            // 删除节点
            GenericMenuAddItem(menu, new GUIContent("Delete Node"), DeleteNode);

            if (nodeValue.parentNodeID >= 0)
            {
                GenericMenuAddItem(menu, new GUIContent("Remove Parent"), RemoveParentNode);
            }
        }

        menu.ShowAsContext();
        Event.current.Use();
    }

    private void GenericMenuAddItem(GenericMenu menu, GUIContent content, GenericMenu.MenuFunction func)
    {
        if (!BehaviorManager.Instance.CurrentOpenConfigSubTree())
        {
            menu.AddItem(content, false, func);
        }
        else
        {
            menu.AddDisabledItem(content);
        }
    }

    private void GenericMenuAddItem(GenericMenu menu, GUIContent content, GenericMenu.MenuFunction2 func, object userData)
    {
        if (!BehaviorManager.Instance.CurrentOpenConfigSubTree())
        {
            menu.AddItem(content, false, func, userData);
        }
        else
        {
            menu.AddDisabledItem(content);
        }
    }

    // 连线子节点
    private void MakeTransition()
    {
        makeTransitionMode = true;
    }

    // 删除节点
    private void DeleteNode()
    {
        NodeValue nodeValue = GetMouseInNode(_nodeList);

        if (!EditorUtility.DisplayDialog("提示", "确定要删除节点吗", "Yes", "No"))
        {
            return;
        }

        if (null != BehaviorManager.behaviorDeleteNode)
        {
            BehaviorManager.behaviorDeleteNode(nodeValue.id);
        }
    }

    // 移除父节点
    private void RemoveParentNode()
    {
        NodeValue nodeValue = GetMouseInNode(_nodeList);
        if (!EditorUtility.DisplayDialog("提示", "确定要删除父节点吗", "Yes", "No"))
        {
            return;
        }

        if (null != BehaviorManager.behaviorRemoveParentNode)
        {
            BehaviorManager.behaviorRemoveParentNode(nodeValue.id);
        }
    }

    /// 每帧绘制从 节点到所有子节点的连线
    private void DrawToChildCurve(NodeValue nodeValue)
    {
        for (int i = nodeValue.childNodeList.Count - 1; i >= 0; --i)
        {
            int childId = nodeValue.childNodeList[i];
            NodeValue childNode = BehaviorManager.Instance.GetNode(childId);
            if (null == nodeValue || null == childNode)
            {
                continue;
            }
            if (BehaviorManager.Instance.RunTimeInvalidSubTreeHash.Count > 0 && BehaviorManager.Instance.RunTimeInvalidSubTreeHash.Contains(childNode.id))
            {
                continue;
            }

            DrawNodeCurve(nodeValue.position, childNode.position);
            DrawLabel(i.ToString(), nodeValue.position, childNode.position);
        }
    }

    // 绘制线
    public static void DrawNodeCurve(RectT start, RectT end)
    {
        Handles.color = Color.black;
        Vector3 startPos = new Vector3(start.x + start.width / 2, start.y + start.height, 0);
        Vector3 endPos = new Vector3(end.x + end.width / 2, end.y, 0);
        //Handles.DrawLine(startPos, endPos);

        Vector3 middle = (startPos + endPos) * 0.5f;
        DrawArrow(startPos, endPos, Color.black);
        Handles.color = Color.white;
    }

    public static void DrawLabel(string msg, RectT start, RectT end)
    {
        Vector3 startPos = new Vector3(start.x + start.width / 2, start.y + start.height, 0);
        Vector3 endPos = new Vector3(end.x + end.width / 2, end.y, 0);

        Vector2 pos = (startPos + endPos) * 0.5f;// + (endPos - startPos) * 0.1f;

        GUIStyle style = new GUIStyle();
        style.fontSize = 20;
        style.normal.textColor = Color.green;
        Handles.Label(pos, msg, style);
    }

    private static void DrawArrow(Vector2 from, Vector2 to, Color color)
    {
        Handles.BeginGUI();
        Handles.color = color;
        Handles.DrawAAPolyLine(3, from, to);
        Vector2 v0 = from - to;
        v0 *= 10 / v0.magnitude;
        Vector2 v1 = new Vector2(v0.x * 0.866f - v0.y * 0.5f, v0.x * 0.5f + v0.y * 0.866f);
        Vector2 v2 = new Vector2(v0.x * 0.866f + v0.y * 0.5f, v0.x * -0.5f + v0.y * 0.866f);
        Vector2 middle = (from + to) * 0.5f;
        Handles.DrawAAPolyLine(3, middle + v1, middle, middle + v2);
        Handles.EndGUI();
    }

    private void SortChild(List<NodeValue> nodeList)
    {
        for (int i = 0; i < nodeList.Count; ++i)
        {
            NodeValue nodeValue = nodeList[i];

            nodeValue.childNodeList.Sort((a, b) =>
            {
                NodeValue nodeA = BehaviorManager.Instance.GetNode(a);
                NodeValue nodeB = BehaviorManager.Instance.GetNode(b);
                return (int)(nodeA.position.x - nodeB.position.x);
            });
        }
    }

    private void ResetScrollPos(List<NodeValue> nodeList)
    {
        if (nodeList.Count <= 0)
        {
            return;
        }

        NodeValue rightmostNode = null;
        NodeValue bottomNode = null;
        for (int i = 0; i < nodeList.Count; ++i)
        {
            NodeValue nodeValue = nodeList[i];
            if (rightmostNode == null || (nodeValue.position.x > rightmostNode.position.x))
            {
                rightmostNode = nodeValue;
            }
            if (bottomNode == null || (nodeValue.position.y > bottomNode.position.y))
            {
                bottomNode = nodeValue;
            }
        }

        if ((rightmostNode.position.x + rightmostNode.position.width) > contentRect.width)
        {
            contentRect.width = rightmostNode.position.x + rightmostNode.position.width + 50;
        }

        if ((bottomNode.position.y + bottomNode.position.height) > contentRect.height)
        {
            contentRect.height = bottomNode.position.y + +bottomNode.position.height + 50;
        }
    }

}
