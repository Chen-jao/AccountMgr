using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreeNode : MonoBehaviour
{
    
    private TreeNode _parent;
    [HideInInspector]
    /// <summary>
    /// 父对象
    /// </summary>
    public TreeNode Parent { get => _parent; set => _parent = value; }
    /// <summary>
    /// 子对象列表
    /// </summary>
    private List<TreeNode> _childNodes;

    
    private TreeNodeData _data;
    /// <summary>
    /// 存放节点数据
    /// </summary>
    public TreeNodeData Data { get => _data; set => _data = value; }
    
    private int _hierarchy;
    [HideInInspector]
    /// <summary>
    /// 层级
    /// </summary>
    public int Hierarchy { get => _hierarchy; set => _hierarchy = value; }

    private bool _isExpanding = false;
    [HideInInspector]
    /// <summary>
    /// 子节点列表展开状态
    /// </summary>
    public bool IsExpanding { get => _isExpanding; set => _isExpanding = value; }

    
    private bool _isRefreshing = false;
    [HideInInspector]
    /// <summary>
    /// 当前节点刷新状态
    /// </summary>
    public int ChildNodeCount { get => _childNodes.Count; }


    private void Awake()
    {
        _childNodes = new List<TreeNode>();

        // ADD BTN LISTERNER
        transform.Find(TreeNodeItemName.BtnContent).GetComponent<Button>().onClick.AddListener(this.OnBtnClick_Content);
        transform.Find(TreeNodeItemName.BtnTogo).GetComponent<Toggle>().onValueChanged.AddListener(restult=>this.OnBtnClick_Togo());
        transform.Find(TreeNodeItemName.BtnDrop).GetComponent<Button>().onClick.AddListener(this.OnBtnClick_Drop);
    }

    #region 节点操作
    /// <summary>
    /// 添加子节点
    /// </summary>
    /// <param name="child"></param>
    public void AddChild(TreeNode child)
    {
        if(_childNodes != null)
        {
            _childNodes.Add(child);
        }
        else
        {
            Debug.Log(string.Format("子对象容器不存在，无法添加子节点的节点!"));
        }
    }
    /// <summary>
    /// 返回子节点列表
    /// </summary>
    /// <param name="childrens"></param>
    public void RequireChild(out List<TreeNode> childrens)
    {
        childrens = _childNodes;
    }
    /// <summary>
    /// 由索引获取子节点
    /// </summary>
    /// <param name="index">索引</param>
    /// <param name="child">子节点返回</param>
    public void RequireChild(int index, out TreeNode child)
    {
        child = _childNodes.Count > index ? _childNodes[index] : default;
    }

    /// <summary>
    /// 删除子节点
    /// </summary>
    /// <param name="child"></param>
    public void RemoveChild(TreeNode child)
    {
        if(_childNodes != null && _childNodes.Count > 0)
        {
            _childNodes.Remove(child);
        }
    }

    #endregion

    #region 状态切换与UI更新
    /// <summary>
    /// 切换到关闭界面
    /// </summary>
    public void SwitchShut()
    {
        for (var i = 0; i < ChildNodeCount; i++)
        {
            RequireChild(i, out var node);
            if (node == null) continue;
            node.SwitchShut();
            node.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 切换到展开界面
    /// </summary>
    public void SwitchOpen()
    {
       
        for (var i = 0; i < ChildNodeCount; i++)
        {
            RequireChild(i, out var node);
            if (node == null) continue;
            if (node.IsExpanding)
                node.SwitchOpen();
            node.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// 图标UI更新
    /// </summary>
    public void IconUpdate()
    {
        var rect = transform.Find(TreeNodeItemName.BtnDrop).GetComponent<RectTransform>();
        if (IsExpanding)
            rect.localRotation = Quaternion.Euler(0, 0, 0);
        else
            rect.localRotation = Quaternion.Euler(0, 0, 90);
    }

    #endregion

    #region 点击事件

    /// <summary>
    /// 当按钮togo被点击
    /// </summary>
    private void OnBtnClick_Togo()
    {

    }
    /// <summary>
    /// 当按钮下拉被点击
    /// </summary>
    private void OnBtnClick_Drop()
    {
        if (_isRefreshing) return;
        _isRefreshing = true;
        if (IsExpanding)
        {
            IconUpdate();
            IsExpanding = false;
            SwitchShut();
        }
        else
        {
            IconUpdate();
            IsExpanding = true;
            SwitchOpen();
        }

        //刷新树状图
        MMGR.Mgr<TreeViewMgr>().TreeView.ReFreshTree();

        _isRefreshing = false;
    }
    /// <summary>
    /// 当按钮内容被点击
    /// </summary>
    private void OnBtnClick_Content()
    {
        MMGR.Mgr<TreeViewMgr>().TreeView.TreeNodeBtnClickContent(this);
    }

    #endregion
}
