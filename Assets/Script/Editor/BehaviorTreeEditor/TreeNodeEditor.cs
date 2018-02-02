using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using BehaviorTree;

public class TreeNodeEditor : EditorWindow {
    private NodeValue nodeValue = null;
    // 保存窗口中所有节点
    private List<NodeValue> nodeValueList = new List<NodeValue>();
    // 当前选择的节点
    private NodeValue selectNodeValue = null;
    // 鼠标的位置
    private Vector2 mousePosition;
    // 添加连线
    private bool makeTransitionMode = false;

    private static TreeNodeEditor window;
    private static bool isInit = false;
    [MenuItem("Window/CreateTree")]
    public static void ShowWindow()
    {
        window = EditorWindow.GetWindow<TreeNodeEditor>();
        window.Show();
        isInit = false;
    }
    
    private void OnGUI()
    {
        if (!isInit)
        {
            isInit = true;
            nodeValue = Node.NodeAssetToNodeValue();
            nodeValueList = Node.nodeValueList;
        }

        Event _event = Event.current;
        mousePosition = _event.mousePosition;

        if ((_event.button == 1) && (_event.type == EventType.MouseDown) && (!makeTransitionMode)) // 鼠标右键
        {
            int selectIndex = 0;
            selectNodeValue = GetMouseInNode(out selectIndex);

            int menuType = (selectNodeValue != null) ? 1 : 0;
            ShowMenu(menuType);
        }

        // 在连线状态，按下鼠标
        if (makeTransitionMode && _event.type == EventType.MouseDown)
        {
            int selectIndex = 0;
            NodeValue newSelectNodeValue = GetMouseInNode(out selectIndex);
            // 如果按下鼠标时，选中了一个节点，则将 新选中根节点 添加为 selectNode 的子节点
            if (selectNodeValue != newSelectNodeValue)
            {
                selectNodeValue.childNodeList.Add(newSelectNodeValue);
            }
            
            // 取消连线状态
            makeTransitionMode = false;
            // 清空选择节点
            selectNodeValue = null;
        }

        if (makeTransitionMode && selectNodeValue != null)
        {
            Rect mouseRect = new Rect(mousePosition.x, mousePosition.y, 10, 10);
            DrawNodeCurve(selectNodeValue.position, mouseRect);
        }

        // 开始绘制节点 
        // 注意：必须在  BeginWindows(); 和 EndWindows(); 之间 调用 GUI.Window 才能显示
        BeginWindows();
        for (int i = 0; i < nodeValueList.Count; i++)
        {
            NodeValue nodeValue = nodeValueList[i];
            string name = Enum.GetName(typeof(NodeType), nodeValue.NodeType);
            nodeValue.position = GUI.Window(i, nodeValue.position, DrawNodeWindow, name);
            DrawToChildCurve(nodeValue);
        }
        EndWindows();

        if (window != null)
        {
            Rect windowPos = window.position;
            if (GUI.Button(new Rect(window.position.width * 0.5f - 50, window.position.height - 40, 100, 40), "Save"))
            {
                Node.NodeValueToNodeAsset(nodeValue);
            }
        }

        Repaint();
    }

    void DrawNodeWindow(int id)
    {
        NodeValue nodeValue = nodeValueList[id];
        NodeEditor.Draw(nodeValue);
        GUI.DragWindow();
    }

    // 获取鼠标所在位置的节点
    private NodeValue GetMouseInNode(out int index)
    {
        index = 0;
        NodeValue selectNode = null;
        for (int i = 0; i < nodeValueList.Count; i++)
        {
            NodeValue nodeValue = nodeValueList[i];
            // 如果鼠标位置 包含在 节点的 Rect 范围，则视为可以选择的节点
            if (nodeValue.position.Contains(mousePosition))
            {
                selectNode = nodeValue;
                index = i;
                break;
            }
        }

        return selectNode;
    }

    private void ShowMenu(int type)
    {  
        GenericMenu menu = new GenericMenu();
        if (type == 0)
        {
            // 添加一个新节点
            menu.AddItem(new GUIContent("Add Node"), false, AddNode);
        }
        else
        {
            // 连线子节点
            menu.AddItem(new GUIContent("Make Transition"), false, MakeTransition);
            menu.AddSeparator("");
            // 删除节点
            menu.AddItem(new GUIContent("Delete Node"), false, DeleteNode);
        }

        menu.ShowAsContext();
        Event.current.Use();
    }

    // 添加节点
    private void AddNode()
    {
        NodeValue newNodeValue = new NodeValue();
        newNodeValue.position = new Rect(mousePosition.x, mousePosition.y, 100, 100);
        nodeValueList.Add(newNodeValue);

        if (nodeValue == null)
        {
            nodeValue = newNodeValue;
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
        int selectIndex = 0;
        selectNodeValue = GetMouseInNode(out selectIndex);
        if (selectNodeValue != null)
        {
            nodeValueList[selectIndex].Release();
            nodeValueList.RemoveAt(selectIndex);
        }
    }

    /// 每帧绘制从 节点到所有子节点的连线
    private void DrawToChildCurve(NodeValue nodeValue)
    {
        for (int i = nodeValue.childNodeList.Count - 1; i >= 0; --i)
        {
            NodeValue childNode = nodeValue.childNodeList[i];
            // 删除无效节点
            if (childNode == null || childNode.isRelease)
            {
                nodeValue.childNodeList.RemoveAt(i);
                continue;
            }
            DrawNodeCurve(nodeValue.position, childNode.position);
        }
    }

    // 绘制线
    public static void DrawNodeCurve(Rect start, Rect end)
    {
        Vector3 startPos = new Vector3(start.x + start.width / 2, start.y + start.height, 0);
        Vector3 endPos = new Vector3(end.x + end.width / 2, end.y, 0);
        Handles.DrawLine(startPos, endPos);
    }
}