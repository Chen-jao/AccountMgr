
public delegate void WhileTreeNodeClick(TreeNode node);

/*
 这几个事件完全可以再btn触发事件里面处理
 但是，可能会在别的地方做些什么奇怪的事情？
     */

/// <summary>
/// 这个是当树状图节点内容被点击的委托事件
/// </summary>
/// <param name="node"></param>
public delegate void WhileTreeNodeBtnClickContent(TreeNode node);
/// <summary>
/// 这个是当树状图节点选中状态被点击的委托事件
/// </summary>
/// <param name="node"></param>
public delegate void WhileTreeNodeBtnClickTogo(TreeNode node);
/// <summary>
/// 这个是当树状图节点下拉箭头被点击的委托事件
/// </summary>
/// <param name="node"></param>
public delegate void WhileTreeNodeBtnClickDrop(TreeNode node);