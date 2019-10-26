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
    }

    public void OnDestroy()
    {

    }

    public void OnGUI(TreeNodeWindow window)
    {
        _treeNodeWindow = window;
        _behaviorDrawView.Init(_treeNodeWindow, this);

        NodeValue currentNode = _behaviorDrawModel.GetCurrentSelectNode();
        List<NodeValue> nodeList = _behaviorDrawModel.GetNodeList();

        _behaviorDrawView.Draw(_treeNodeWindow.position, currentNode, nodeList);
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

    public void AddNodeType(NODE_TYPE nodeType, string nodeName, int identification)
    {
        Node_Draw_Info_Item item = new Node_Draw_Info_Item(nodeType);
        item.SetName(nodeName);
        item.SetIdentification(identification);
        string name = string.Format("{0}/{1}", _nodeTypeName, nodeName);
        KeyValuePair<string, Node_Draw_Info_Item> kv = new KeyValuePair<string, Node_Draw_Info_Item>(name, item);
        _nodeArr.Add(kv);
    }

}

public class Node_Draw_Info_Item
{
    public string _nodeName = string.Empty;
    public NODE_TYPE _nodeType;
    public int _identification = -1;

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

    public void SetIdentification(int identification)
    {
        _identification = identification;
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

    private List<NODE_TYPE[]> nodeList = new List<NODE_TYPE[]>() {
        new NODE_TYPE[] { NODE_TYPE.SELECT, NODE_TYPE.PARALLEL },  // 组合节点
        new NODE_TYPE[]{ NODE_TYPE.DECORATOR_INVERTER, NODE_TYPE.DECORATOR_UNTIL_SUCCESS}, // 修饰节点
    };
    private string[] typeNameArr = { "组合节点", "修饰节点" };
    private void SetInfoList()
    {
        for (int i = 0; i < nodeList.Count; ++i)
        {
            Node_Draw_Info drawInfo = new Node_Draw_Info(typeNameArr[i]);
            NODE_TYPE[] arr = nodeList[i];
            for (NODE_TYPE nodeType = arr[0]; nodeType <= arr[1]; ++nodeType)
            {
                drawInfo.AddNodeType(nodeType);
                infoList.Add(drawInfo);
            }
        }

        
        {
            // 条件节点
            Node_Draw_Info conditionDrawInfo = new Node_Draw_Info("条件节点");
            infoList.Add(conditionDrawInfo);

            // 行为节点
            Node_Draw_Info actionDrawInfo = new Node_Draw_Info("行为节点");
            infoList.Add(actionDrawInfo);

            List<CustomIdentification> nodeList = CustomNode.Instance.GetNodeList();
            for (int i = 0; i < nodeList.Count; ++i)
            {
                CustomIdentification customIdentification = nodeList[i];
                if (customIdentification.NodeType == NODE_TYPE.CONDITION)
                {
                    conditionDrawInfo.AddNodeType(NODE_TYPE.CONDITION, customIdentification.Name, (int)customIdentification.Identification);
                }
                else if (customIdentification.NodeType == NODE_TYPE.ACTION)
                {
                    actionDrawInfo.AddNodeType(NODE_TYPE.ACTION, customIdentification.Name, (int)customIdentification.Identification);
                }
            }

        }
    }

    public List<Node_Draw_Info> InfoList {
        get
        {
            return infoList;
        }
    }

}

public class BehaviorDrawView
{
    private TreeNodeWindow _treeNodeWindow = null;
    private BehaviorDrawController _drawController = null;

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

    public void Draw( Rect windowsPosition, NodeValue currentNode, List<NodeValue> nodeList)
    {
        _nodeList = nodeList;

        Rect rect = GUILayoutUtility.GetRect(0f, 0, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        scrollRect = rect;

        contentRect.x = rect.x;
        contentRect.y = rect.y;

        //创建 scrollView  窗口  
        scrollPos = GUI.BeginScrollView(scrollRect, scrollPos, contentRect);
        {
            NodeMakeTransition(currentNode, nodeList);

            DrawNodeWindows(nodeList);
            SortChild(nodeList);

            ResetScrollPos(nodeList);
        }
        GUI.EndScrollView();  //结束 ScrollView 窗口  
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
                    if (BehaviorManager.behaviorChangeSelectId != null)
                    {
                        int nodeId = (null != nodeValue) ? nodeValue.id : -1;
                        BehaviorManager.behaviorChangeSelectId(nodeId);
                    }
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
                string name = nodeValue.nodeName; // 
                //if (string.IsNullOrEmpty(name))
                //{
                //    if (nodeValue.NodeType < (int)NODE_TYPE.CONDITION)
                //    {
                //        nodeValue.nodeName = NodeEditor.GetTitle((NODE_TYPE)nodeValue.NodeType);
                //    }
                //}

                name = string.Format("{0}_{1}", name, nodeValue.id);
                Rect rect = GUI.Window(i, RectTool.RectTToRect(nodeValue.position), DrawNodeWindow, name);
                nodeValue.position = RectTool.RectToRectT(rect);
                DrawToChildCurve(nodeValue);
            }
        };

        _treeNodeWindow.DrawWindow(CallBack);
    }

    void DrawNodeWindow(int id)
    {
        NodeValue nodeValue = _nodeList[id];
        NodeEditor.Draw(nodeValue, BehaviorManager.Instance.CurrentSelectId);
        GUI.DragWindow();
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
                    BehaviorManager.behaviorAddNode((Node_Draw_Info_Item)userData, mousePosition);
                }
            };

            List<Node_Draw_Info> nodeList = _drawController._behaviorDrawModel.InfoList;
            for (int i = 0; i < nodeList.Count; ++i)
            {
                Node_Draw_Info draw_Info = nodeList[i];
                for (int j = 0; j < draw_Info._nodeArr.Count; ++j)
                {
                    KeyValuePair<string, Node_Draw_Info_Item> kv = draw_Info._nodeArr[j];
                    string itemName = string.Format("Add Node/{0}", kv.Key);
                    menu.AddItem(new GUIContent(itemName), false, CallBack, kv.Value);
                }
            }
        }
        else
        {
            if (null != currentNode && nodeValue.id == currentNode.id && (NODE_TYPE)nodeValue.NodeType < NODE_TYPE.CONDITION)
            {
                // 连线子节点
                menu.AddItem(new GUIContent("Make Transition"), false, MakeTransition);
                menu.AddSeparator("");
            }
            // 删除节点
            menu.AddItem(new GUIContent("Delete Node"), false, DeleteNode);

            if (nodeValue.parentNodeID >= 0)
            {
                menu.AddItem(new GUIContent("Remove Parent"), false, RemoveParentNode);
            }
        }

        menu.ShowAsContext();
        Event.current.Use();
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
            DrawNodeCurve(nodeValue.position, childNode.position);
        }
    }

    // 绘制线
    public static void DrawNodeCurve(RectT start, RectT end)
    {
        Handles.color = Color.black;
        Vector3 startPos = new Vector3(start.x + start.width / 2, start.y + start.height, 0);
        Vector3 endPos = new Vector3(end.x + end.width / 2, end.y, 0);
        Handles.DrawLine(startPos, endPos);
        Handles.color = Color.white;
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
