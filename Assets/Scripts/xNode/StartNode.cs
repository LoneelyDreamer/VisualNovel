using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[NodeWidth(100)]
public class StartNode : BaseNode
{
    [Output] public int exit;
    private string _start = "Start";
    public override string GetString()
    {
        return _start;
    }
}