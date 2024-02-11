using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using XNode;
using XNodeEditor;
using System.Linq;

[CustomNodeEditor(typeof(DialogueNode))]
public class DialogNodeDrawer : NodeEditor
{
    private DialogueNode dialogNode;
    private bool showNodeSettings = false;
    private bool showDialogSettings = false;

    private string newDialogueOption = "";
    private string newDialogueOptionOutput = "";
    private int currentNodeTab = 0;
    private int nodePortToDelete = 0;

    public override void OnBodyGUI()
    {
        if (dialogNode == null)
        {
            dialogNode = target as DialogueNode;
        }

        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("entry"));

        showNodeSettings = EditorGUILayout.BeginFoldoutHeaderGroup(showNodeSettings, "Node Settings"); 
        if (showNodeSettings)
        {
            currentNodeTab = GUILayout.Toolbar(currentNodeTab, new string[] { "Add Output", "Remove Output" });
            switch (currentNodeTab)
            {
                case 0:
                    EditorGUILayout.PrefixLabel("PortName");
                    newDialogueOption = EditorGUILayout.TextField(newDialogueOption);
                    EditorGUILayout.PrefixLabel("OptionText");
                    newDialogueOptionOutput = EditorGUILayout.TextField(newDialogueOptionOutput);

                    if (GUILayout.Button("Create new options"))
                    {
                        bool noDialog = (newDialogueOption.Length == 0);
                        bool noOptions = (newDialogueOptionOutput.Length == 0);

                        if (noDialog)
                        {
                            EditorUtility.DisplayDialog("Error creating port", "No dialog was entered.", "Ok");
                            return;
                        }
                        if (noOptions)
                        {
                            EditorUtility.DisplayDialog("Error creating port", "No output port was specified.", "Ok");
                            return;
                        }

                        bool mathesExistingOutput = false;

                        foreach (NodePort p in dialogNode.DynamicOutputs)
                        {
                            if (p.fieldName == newDialogueOptionOutput)
                            {
                                mathesExistingOutput = true;
                                break;
                            }
                        }

                        if (mathesExistingOutput)
                        {
                            EditorUtility.DisplayDialog("Error creating port", "The requested port is already in use.", "Ok");
                            return;
                        }

                        dialogNode.AddDynamicOutput(typeof(int), Node.ConnectionType.Multiple, Node.TypeConstraint.None, newDialogueOption);
                        dialogNode.dialogueOptionsList.Add(new DialogueNode.DialogueOption(newDialogueOption, newDialogueOptionOutput));
                    }
                    break;

                case 1:
                    if (dialogNode.DynamicOutputs.Count() == 0)
                    {
                        EditorGUILayout.HelpBox("You do not have dynamic ports on this node.", MessageType.Warning);
                    }
                    else
                    {
                        EditorGUILayout.PrefixLabel("Choose Port");

                        List<string> outputs = new List<string>();
                        foreach (NodePort p in dialogNode.DynamicOutputs)
                        {
                            outputs.Add(p.fieldName);
                        }

                        nodePortToDelete = EditorGUILayout.Popup(nodePortToDelete, outputs.ToArray());

                        if (GUILayout.Button("Delet selected node"))
                        {
                            foreach (DialogueNode.DialogueOption d in dialogNode.dialogueOptionsList)
                            {
                                dialogNode.dialogueOptionsList.Remove(d);
                                break;
                            }


                            dialogNode.RemoveDynamicPort(dialogNode.DynamicOutputs.ElementAt(nodePortToDelete));
                        }
                    }
                    break;

            }

        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        showDialogSettings = EditorGUILayout.BeginFoldoutHeaderGroup(showDialogSettings, "Dialog Settings");
        if (showDialogSettings)
        {
            dialogNode.speakerSprite = (Sprite)EditorGUILayout.ObjectField("SpeakerSprite", dialogNode.speakerSprite, typeof(Sprite), false);
            dialogNode.speakerName = EditorGUILayout.TextField("SpeakerName", dialogNode.speakerName);
            dialogNode.dialogueText = EditorGUILayout.TextField("DialogueText", dialogNode.dialogueText);
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        EditorGUIUtility.labelWidth = 120;
        dialogNode.dialogueOptions = EditorGUILayout.Toggle("Show dialog options", dialogNode.dialogueOptions);

        if (dialogNode.dialogueOptions)
        {
            foreach (NodePort p in dialogNode.DynamicOutputs)
            {
                NodeEditorGUILayout.PortField(p);
            }
        }

        serializedObject.Update();


    }
}
