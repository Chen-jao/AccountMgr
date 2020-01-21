using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropdownMgr : IMonoStart
{

    // 文件清单
    private Dropdown _dropdown;

    private TreeNodeData _data = default;

    public DropdownMgr(Dropdown drop)
    {
        _dropdown = drop;
    }

    public void Awake()
    {
        
    }

    public void Enable()
    {
        Clear();
    }

    public void Start()
    {
        // 注册回调函数,选择下拉文件
        _dropdown.onValueChanged.AddListener(result => OnValeChange_EncrptyFile());
    }


    /*
        下面是UI功能     
    */

    /// <summary>
    /// 返回选择的下拉列表数据
    /// </summary>
    public string DropText => _dropdown.options[_dropdown.value].text;

    /// <summary>
    /// 下拉列表清空
    /// </summary>
    public void Clear() => _dropdown.ClearOptions();

    /// <summary>
    /// 添加内容到下拉列表
    /// </summary>
    /// <param name="msg">数据</param>
    public void Add(string msg)
    {
        if(_dropdown.IsExist())
            _dropdown.options.Add(new Dropdown.OptionData(msg));
    }

    /// <summary>
    /// 获取下拉列表数据的接口
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private string GetDropText()
    {
        // 当且仅当这个组件存在，并且处在激活状态，而且还得有数据的情况下才会返回正确的结果，否则返回默认结果
        if (_dropdown.IsExist() && _dropdown.gameObject.activeInHierarchy && _dropdown.options.Count > 0)
            return _dropdown.options[_dropdown.value].text;
        else
            return default;
    }

    /// <summary>
    /// 设置下拉列表中的默认值
    /// </summary>
    public void SetData(TreeNodeData data)
    {
        // 需要手动更新name列表里面的内容，默认的话容易出问题，造成不显示，并且重置value
        // value 在手动框选关联down时 并不会主动更新自己，
        _data = data;
        _dropdown.value = 0;
        _dropdown.captionText.text = _dropdown.options.Count > 0 ? _dropdown.options[0].text : "default";
    }

    // dropdown type回调事件方法
    private void OnValeChange_EncrptyFile()
    {
        // TODO
        // 数据处理

        if (_data.IsExist())
        {
            var path = string.Format("{0}/{1}", _data.Path, _dropdown.captionText.text);            
            FileReader.Read(path, out var contents);
            MMGR.Mgr<AdministorMgr>().Decrypt(contents, out var dMsg);
            JsonParser.ToObject<DataAccountPassword>(dMsg, out var df);
            MMGR.Mgr<ReultPannelMgr>().Show(new DataAccountPassword()
            {
                Account = df.Account,
                Pwd = df.Pwd
            });
        }

        
    }

    // 基础点数 ATK-100 POWER-100 MENTALITY-100
    // 系数乘算
    /*
     * 基准数因子：固定数值，公式定常量
     * 协调数因子：可变数值，调节变常量
     * 
     * 衰减因子，数值越大，衰减效果影响越大
     * 调和因子，数值越大，调和效果影响越明显
     * 
     * Phsysic
     *   计算结果 = 基础ATK数值 * (1 + Power加成率)
     *   Power加成率 = (sqrt(power) / 10 + sqrt(strength) / 100) * Power加成衰减率 * (base factor 0.5 + (±0.?)
     *   Power加成衰减率 = （1 - （衰减因子 - 调和因子））* Power调和系数
     *   Power调和系数 =  (1 - sqrt(abs(power -strength) / sqrt(abs(power + strength)))
     *   
     *   最终结果
     *      result = base * (1 + (sqrt(power) / 10 + sqrt(stregnth) / 100) * (1 - (衰减因子 - 调和因子)) * (基准数因子 0.5 + 协调数因子(±0.?))
     *   
     *   
     *   衰减系数，当主值>副值时，超出副值的一部分会存在主值衰减的现象
     *   
     *   调和系数，当主值>副值时，超出副值的一部分会存在主值衰减的现象，当两者接近时，调和系数越高，用来缓和衰减系数
     *   
     *  
     * Magic
     * MAGATK = ATK * (1 + Sqrt(Mentality) / 10 * (基准因子数 + (±协调因子数)))
     * exp:
     *      MAGATK = 100 * (1 + Sqrt(100) / 10 * (0.5 + (±0.1)))
     *  Tranlslate：Normal caculate magic atk = base atk 100 * (1 + Sqrt(power 100) / 10 * (basic factor 0.5 + (±0))) Result physic atk = 150
     *      MAGDEF = DEF * (1 + Sqrt(Corporeity) / 10 * (0.2 + (±0.05)))
     *  Tranlslate：Normal caculate magic def = base def 100 * (1 + Sqrt(def 100) / 10 * (basic factor 0.2 + (±0))) Result physic atk = 120
     */


    /*
     *
     * A B 
     * A > B 则
     * (A - B ) / A
     * (1000 - 100) / 1000 = 0.9
     * 
     * 系数 = sqrt(abs(A -B) / sqrt(abs(A + B))
     * 
     * 
     * 
     */

    /*
     * 
     * 当数值差距过大时会影响这个数值及其明显 
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     */

}