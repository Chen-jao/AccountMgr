using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPannelMgr : MMGR
{

    [SerializeField]
    private Dropdown _dropdown = default;

    private DropdownMgr _dropdownMgr;

    private void Awake()
    {
        Register(this);
        _dropdownMgr = new DropdownMgr(_dropdown);
        _dropdownMgr.Awake();
    }

    private void OnEnable()
    {
        _dropdownMgr.Enable();
    }

    private void Start()
    {
        _dropdownMgr.Start();
    }

    private void OnDestroy()
    {
        UnRegister(this);
    }

    public void SetDrop(System.IO.FileInfo[] fileInfos, TreeNodeData data)
    {
        _dropdownMgr.Clear();
        foreach(var fileInfo in fileInfos)
        {
            _dropdownMgr.Add(fileInfo.Name);
        }
        _dropdownMgr.SetData(data);
        
    }
}
