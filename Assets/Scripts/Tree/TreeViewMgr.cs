using System.Collections.Generic;
using UnityEngine;

public class TreeViewMgr : MMGR
{
    // 容器根结点
    [SerializeField, Tooltip("树容器")]
    private Transform m_Root = default;
    // 容器模板
    [SerializeField, Tooltip("树节点")]
    private GameObject m_Template = default;
    [SerializeField, Tooltip("树状图对象")]
    private Transform m_TreeView = default;

    [SerializeField, Tooltip("树状图对象")]
    private int m_Width = 175;
    [SerializeField, Tooltip("树状图对象")]
    private int m_Height = 25;

    // treeview 树状图管理实例
    private TreeView _treeView;
    internal TreeView TreeView { get => _treeView; }

    private void Awake()
    {
        Register(this);
        InitTreeView();
    }

    private void OnEnable()
    {
        
    }

    private void Start()
    {
        // 外部读取数据实例
        // TODO
    }

    private void OnDestroy()
    {
        UnRegister(this);
    }
    /// <summary>
    /// 树状图数据初始化
    /// </summary>
    private void InitTreeView()
    {
        _treeView = new TreeView(m_Root, m_Template)
        {
            Width = m_Width,
            Height = m_Height,
            MonoTreeView = m_TreeView
        };

        _treeView.TreeNodeBtnClickContent += WhilNodeClickContent;
        _treeView.TreeNodeBtnClickTogo += WhileNodeClickTogo;
        _treeView.TreeNodeBtnClickDrop += WhileNodeClickDrop;
    }

    /// <summary>
    /// 代理事件的执行，内容被点击
    /// </summary>
    /// <param name="node">事件节点</param>
    public void WhilNodeClickContent(TreeNode node)
    {
        Debug.Log("Node is click!");
        FileBase.TmpStrPath = node.Data.IsExist() ? node.Data.Path : default;
        // 获取节点下的非目录文件
        FileSearcher.ToArrayInfoWhole(out var files);
        if (files.IsExist())
            Mgr<UIPannelMgr>().SetDrop(files, node.Data);
       
    }
    /// <summary>
    /// 代理事件的执行，下拉箭头被点击
    /// </summary>
    /// <param name="node">事件节点</param>
    public void WhileNodeClickDrop(TreeNode node)
    {

    }
    /// <summary>
    /// 代理事件的执行，Togo被点击
    /// </summary>
    /// <param name="node">事件节点</param>
    public void WhileNodeClickTogo(TreeNode node)
    {

    }
}



