using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[NodeWidth(100)]
public class FinishNode : BaseNode
{
    [Input] public int entry;
    private string _stop = "Stop";
    public override string GetString()
    {
        return _stop;
    }
}