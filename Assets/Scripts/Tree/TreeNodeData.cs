using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 数据实例
/// </summary>
public class TreeNodeData
{
    /// <summary>
    /// 空构造特殊使用
    /// </summary>
    public TreeNodeData() { }

    public TreeNodeData(int id, int pid, int layer, string msg, string path)
    {
        Msg = msg;
        PID = pid;
        Id = id;
        Layer = layer;
        Path = path;
    }

    private string _msg = "";
    private int _pid = -1;
    private int _id = -1;
    private int _layer = -1;
    private string _path = "";

    public string Msg { get => _msg; private set => _msg = value; }
    public int PID { get => _pid; private set => _pid = value; }
    public int Id { get => _id; private set => _id = value; }
    public int Layer { get => _layer; private set => _layer = value; }
    public string Path { get => _path; private set => _path = value; }
}

/// <summary>
/// 容器管理器，处理容器数据实例
/// 将数据与管理分离开
/// </summary>
public class TreeNodeDataContainerMgr
{

    public TreeNodeDataContainerMgr()
    {
        _rootContainer = new TreeNodeDataContainer();
    }
    // 这是个树状图容器的根结点，只此一例，外部只读
    private TreeNodeDataContainer _rootContainer;
    // 临时容器，负责指向目标容器，但不涉及实例构造
    private TreeNodeDataContainer _tmpContainer;


    private int _layerTar = -1;
    private int _pidTar = -1;

    /// <summary>
    /// 属性器，提供外部访问
    /// </summary>
    public TreeNodeDataContainer RootContainer { get => _rootContainer; }
    /// <summary>
    /// 添加容器
    /// </summary>
    /// <param name="container">容器</param>
    public void Add(TreeNodeDataContainer container)
    {
        if (container != null && container.Data != null) 
        {
            if (container.Data.Layer > -1)
            {
                var result = Search(container.Data.Layer, container.Data.PID);
                //if (result != null && layer != 0)
                //    Debug.Log(string.Format("父亲layer = {0} 文件夹={1}", result.Data.Layer, result.Data.Msg));
                AddStart(container, result == null && container.Data.Layer == 0 ? _rootContainer : result);
            }
        }
    }

    /// <summary>
    /// 开始执行节点容器的添加操作
    /// </summary>
    /// <param name="container">节点容器</param>
    /// <param name="parent">父容器</param>
    private void AddStart(TreeNodeDataContainer container, TreeNodeDataContainer parent)
    {
        if (container != null && parent != null)
        {
            // 目前默认是不处理同key值数据的，如果需要更新后面再考虑
            //Debug.Log(string.Format("layer = {0} msg = {1} pid = {2}", container.Data.Layer, container.Data.Msg, container.Data.PID));
            if (!parent.ContainerDicts.ContainsKey(container.Data.Id))
            {
               // Debug.Log(string.Format("Add msg = {0}", container.Data.Msg));
                parent.ContainerDicts.Add(container.Data.Id, container);
            }
        }
    }
    /// <summary>
    /// 从根结点遍历，查找
    /// </summary>
    /// <param name="layer">容器所在层级</param>
    /// <param name="pid">父容器ID</param>
    /// <returns></returns>
    private TreeNodeDataContainer Search(int layer, int pid)
    {
        _layerTar = layer - 1;
        _pidTar = pid;
        _tmpContainer = default;
        SearchStart(_rootContainer);
        return _tmpContainer;
    }
    /// <summary>
    /// 从指定节点开始查找，虽然不知道什么时候会用到
    /// </summary>
    /// <param name="container">容器</param>
    /// <param name="layer">容器所在层级</param>
    /// <returns></returns>
    private TreeNodeDataContainer Search(TreeNodeDataContainer container, int layer)
    {
        _layerTar = layer - 1;
        
        _tmpContainer = default;
        SearchStart(container);
        return _tmpContainer;
    }

    /// <summary>
    /// 开始查找，递归查找
    /// </summary>
    /// <param name="container">查找的起始点容器</param>
    private void SearchStart(TreeNodeDataContainer container)
    {
        foreach (var value in container.ContainerDicts.Values)
        {
            if (value.Data.Layer == _layerTar && value.Data.Id == _pidTar)
            {
                _tmpContainer = value;
                return;
            }
            if (_tmpContainer == null)
                SearchStart(value);
        }
    }

}

/// <summary>
/// 每一个数据节点容器都由两部分组成
/// 1.子容器，存放孩子节点
/// 2.数据，存放自己本身的数据
/// 构造函数两个，
/// 1.默认根结点，无参数构造
/// 2.提供数据节点构造
/// </summary>
public class TreeNodeDataContainer
{
    // 容器，存容器对象里面已经包含的有数据了，当这个容器被构造的时候，其下所属的子容器存储字典也会被构造
    // 相当于创建了一个存储容器的容器
    private Dictionary<int, TreeNodeDataContainer> _containerDicts;
    // 数据，
    private TreeNodeData _data = default;
    
    public Dictionary<int, TreeNodeDataContainer> ContainerDicts { get => _containerDicts; set => _containerDicts = value; }
    public TreeNodeData Data { get => _data; }
    // 容器所在层级，-1 是默认层级，0是第一层级
    public int Layer { get => _data == default ? -1 : _data.Layer; }

    public TreeNodeDataContainer()
    {
        _containerDicts = new Dictionary<int, TreeNodeDataContainer>();
    }

    public TreeNodeDataContainer(TreeNodeData data)
    {
        _data = data;
        _containerDicts = new Dictionary<int, TreeNodeDataContainer>();
    }

}

/// <summary>
/// 按照PID升序排序
/// </summary>
public class NodeDataOrderBy_PID_asc : IComparer<TreeNodeData>
{
    public int Compare(TreeNodeData x, TreeNodeData y)
    {
        return x.PID.CompareTo(y.PID);
    }

}
/// <summary>
/// 按照PID降序排序
/// </summary>
public class NodeDataOrderBy_PID_dec : IComparer<TreeNodeData>
{
    public int Compare(TreeNodeData x, TreeNodeData y)
    {
        return y.PID.CompareTo(x.PID);
    }

}
// EXP枚举器接口调用举例
// datas.Sort(new NodeDataOrderBy_PID_asc());