using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

class TreeView
{
    public TreeView(Transform root, GameObject template, int horSpace = 25, int verSpace = 2)
    {
        _root = root;
        _template = template;
        _horSpace = horSpace;
        _verSpace = verSpace;
        _treeNodes = new List<TreeNode>();
        TreeNodeBtnClickContent += EmptyClick;
        TreeNodeBtnClickDrop += EmptyClick;
        TreeNodeBtnClickTogo += EmptyClick;
    }

    // 间隔
    private int _horSpace = 2;
    private int _verSpace = 25;

    // 计算时产生的临时变量
    private int _hierarchy = -1;
    private int _yIndex = 0;
    private bool _isRefreshing = false;

    // 节点的宽高
    private int _width;
    private int _height;
    public int Width { get => _width; set => _width = value; }
    public int Height { get => _height; set => _height = value; }

    private readonly string _strHeader = "  ";

    /// <summary>
    /// 需要指定挂载MonoBehavior脚本的ScroolView树形容器
    /// </summary>
    private Transform _monoTreeView;
    public Transform MonoTreeView { get => _monoTreeView; set => _monoTreeView = value; }

    // 树形图的根结点物体
    private Transform _root;
    // 树形图节点模板预制体
    private GameObject _template;
    // 节点列表
    private List<TreeNode> _treeNodes;
    private List<TreeNode> _treeNodesClone;

    private Dictionary<int, Dictionary<int, TreeNode>> _treeNodeDict;
    private Dictionary<int, Dictionary<int, TreeNode>> _treeNodeDictClone;

    #region 代理任务

    /// <summary>
    /// 代理，内容点击
    /// </summary>
    public WhileTreeNodeBtnClickContent TreeNodeBtnClickContent;
    /// <summary>
    /// 代理，Drop点击
    /// </summary>
    public WhileTreeNodeBtnClickDrop TreeNodeBtnClickDrop;
    /// <summary>
    /// 代理，Toggle点击
    /// </summary>
    public WhileTreeNodeBtnClickTogo TreeNodeBtnClickTogo;

    // 空方法，防止委托容器出现空调用
    public void EmptyClick(TreeNode node)
    {
        
    }

    #endregion

    #region 树状图操作

    /// <summary>
    /// 销毁树形图
    /// </summary>
    public void DestroyTree()
    {
        if (_treeNodes != null)
        {
            foreach (var node in _treeNodes)
            {
                Object.DestroyImmediate(node.gameObject);
            }
            _treeNodes.Clear();
        }
    }

    /// <summary>
    /// 刷新树状布局
    /// </summary>
    public void ReFreshTree()
    {
        //上一轮刷新还未结束
        if (_isRefreshing) return;

        _isRefreshing = true;
        _yIndex = 0;
        _hierarchy = 0;

        //复制一份菜单
        _treeNodesClone = new List<TreeNode>(_treeNodes);

        //用复制的菜单进行刷新计算
        for (int i = 0; i < _treeNodesClone.Count; i++)
        {

            var tNode = _treeNodesClone[i];
            //剔除不需要的元素
            if (tNode == null || !tNode.gameObject.activeSelf)
            {
                continue;
            }

            tNode.transform.GetComponent<RectTransform>().localPosition = new Vector3(tNode.Hierarchy * _horSpace, _yIndex, 0);
            _yIndex += (-(Height + _verSpace));
            if (tNode.Hierarchy > _hierarchy)
            {
                _hierarchy = tNode.Hierarchy;
            }

            //如果子元素是展开的，继续向下刷新
            if (tNode.IsExpanding)
            {
                ReFreshTreeNode(tNode);
            }
            // 5修3传 天刀3势力电池 回电 海军钢板 损控 4抗 大白C4

            tNode.IconUpdate();

            tNode = null;
        }

        //重新计算滚动视野的区域
        float x = _hierarchy * _horSpace + Width;
        float y = Mathf.Abs(_yIndex);
        MonoTreeView.GetComponent<ScrollRect>().content.sizeDelta = new Vector2(x, y);

        //清空复制的菜单
        _treeNodesClone.Clear();

        _isRefreshing = false;
    }

    /// <summary>
    /// 刷新节点数据
    /// </summary>
    /// <param name="node"></param>
    public void ReFreshTreeNode(TreeNode node)
    {

    }

    /// <summary>
    /// 创建树形图，外部调用
    /// </summary>
    /// <param name="rootContainer">根结点</param>
    public void BuildTree(TreeNodeDataContainer rootContainer)
    {
        // 指定根结点的父节点为default
        BuildTree(rootContainer, default);
    }

    /// <summary>
    /// 创建树形图
    /// </summary>
    /// <param name="rootContainer">树根节点</param>
    /// <param name="parentNode">节点的父节点</param>
    private void BuildTree(TreeNodeDataContainer rootContainer, TreeNode parentNode)
    {
        var data = rootContainer.Data;
        var go = Object.Instantiate(_template);
        var goNode = go.GetComponent<TreeNode>();
        goNode.Data = data;

        if (parentNode == default)
        {
            goNode.Hierarchy = 0;
        }
        else
        {
            goNode.Hierarchy = parentNode.Hierarchy + 1;
            goNode.Parent = parentNode;
            parentNode.AddChild(goNode);
        }

        go.transform.name = TreeNodeItemName.TreeNode;
        // 消息设定
        // TODO
        go.transform.Find(TreeNodeItemName.TexContent).GetComponent<Text>().text = data.IsEmpty() ? string.Format("{0}+", _strHeader) : string.Format("{0}{1}", _strHeader, data.Msg);
        go.transform.SetParent(_root);
        go.transform.localPosition = Vector3.zero;
        go.transform.localScale = Vector3.one;
        go.transform.localRotation = Quaternion.Euler(Vector3.zero);
        go.SetActive(parentNode == default ? true : false);

        _treeNodes.Add(goNode);

        //goNode.IsExpanding = false;

        parentNode = goNode;

        foreach (var container in rootContainer.ContainerDicts)
        {
            BuildTree(container.Value, parentNode);
        }
    }

    #endregion
}


/// <summary>
/// 针对目录搜索并且构建的一个方法类，目前不具有通性
/// </summary>
public static class DirectorySearcher
{
    private static TreeNodeDataContainerMgr _mgr = default;

    private static int _curLayer = -1;
    //private static int _parentId = -1;

    /// <summary>
    /// 以指定目录文件所在为根结点，搜索构建并返回构建结果，数据存储在容器中，树状图方式, 返回所修改的mgr
    /// </summary>
    /// <param name="rootDir">指定目录</param>
    /// <param name="mgr">结果所存放在的数据容器管理集合中</param>
    public static void Serach(string rootDir, ref TreeNodeDataContainerMgr mgr)
    {
        Clear();
        _mgr = mgr;
        TraverseDirectory(rootDir, -1);
    }
    /// <summary>
    /// 以指定目录文件所在为根结点，搜索构建并返回构建结果，数据存储在容器中，树状图方式, 不返回所修改的mgr
    /// </summary>
    /// <param name="rootDir">指定目录</param>
    /// <param name="mgr">结果所存放在的数据容器管理集合中</param>
    public static void Search(string rootDir)
    {
        Clear();
        _mgr = new TreeNodeDataContainerMgr();
        TraverseDirectory(rootDir, -1);
    }

    /// <summary>
    /// 遍历该目录下的所有文件
    /// </summary>
    public static void TravertseFile(string rootDir)
    {
        string[] files = Directory.GetFiles(rootDir);

        foreach (var path in files)
        {
            var name = path.Split('\\')[1];
            Debug.Log(name);
        }
    }

    /// <summary>
    /// 递归调用，查询的核心
    /// </summary>
    /// <param name="parentDir">需要遍历的节点路径</param>
    public static void TraverseDirectory(string parentDir, int pid)
    {
        var root = new DirectoryInfo(parentDir);
        _curLayer++;

        var tmpId = -1;
        foreach (var d in root.GetDirectories())
        {
            tmpId++;
            //Debug.Log(string.Format("当前layer = {0} 文件夹={1}", _curLayer, d.Name));
            _mgr.Add(new TreeNodeDataContainer(new TreeNodeData(tmpId, _curLayer == 0 ? -1 : pid, _curLayer, d.Name, d.FullName.Replace('\\', '/'))));
            TraverseDirectory(d.FullName, tmpId);

        }
        _curLayer--;
    }
    /// <summary>
    /// 信号量清空
    /// </summary>
    private static void Clear()
    {
        _mgr = null;
        _curLayer = -1;
        //_parentId = -1;
    }

}