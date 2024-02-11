using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using static DialogueNode;

public class BaseNode : Node
{
    public virtual string GetString()
    {
        return null;
    }
    public virtual Sprite GetSprite()
    {
        return null;
    }
    public virtual List<DialogueOption> GetOptions()
    {
        return null;
    }
}