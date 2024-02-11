using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[NodeWidth(300)]
public class DialogueNode : BaseNode
{
    [Input] public int entry;

    [TextArea(3, 10)]
    public string dialogueText;
    public string speakerName;

    public bool dialogueOptions = false;

    public Sprite speakerSprite;
    public List<DialogueOption> dialogueOptionsList = new List<DialogueOption>();

    public override string GetString()
    {
        return "DialogNode/" + speakerName + '/' + dialogueText;
    }

    public override Sprite GetSprite()
    {
        return speakerSprite;
    }
    public override List<DialogueOption> GetOptions()
    {
        return dialogueOptionsList;
    }

    [System.Serializable]
    public class DialogueOption
    {
        public string portName;
        public string optionText;

        public DialogueOption(string _portName, string _optionText)
        {
            portName = _portName;
            optionText = _optionText;
        }
    }

}