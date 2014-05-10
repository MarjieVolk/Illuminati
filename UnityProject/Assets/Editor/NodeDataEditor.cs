using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

class NodeDataEditor : Editor
{

    private static bool spritesFoldout = false;
    private static bool ownershipFoldout = false;

    public override void OnInspectorGUI() {
        NodeData node = (NodeData) target;

        node.power = EditorGUILayout.IntField("Power", node.power);

        spritesFoldout = doFoldout("Sprites", spritesFoldout, () => {
            node.normalSprite = (Sprite)EditorGUILayout.ObjectField("Normal", node.normalSprite, typeof(Sprite), false);
            node.highlightSprite = (Sprite)EditorGUILayout.ObjectField("Highlight", node.highlightSprite, typeof(Sprite), false);
            node.ownedNormalSprite = (Sprite)EditorGUILayout.ObjectField("Normal Owned", node.ownedNormalSprite, typeof(Sprite), false);
            node.ownedHighlightSprite = (Sprite)EditorGUILayout.ObjectField("Highlight Owned", node.ownedHighlightSprite, typeof(Sprite), false);
        });

        ownershipFoldout = doFoldout("Ownership", ownershipFoldout, () => {
            node.startingOwner = (PlayerData) EditorGUILayout.ObjectField("Owner", node.startingOwner, typeof(PlayerData), true);
            node.isStartNode = EditorGUILayout.Toggle("Start Node", node.isStartNode);
            node.isSecondaryStartNode = EditorGUILayout.Toggle("Secondary Start Node", node.isSecondaryStartNode);
        });

        node.archetype = EditorGUILayout.TextField("Archetype", node.archetype);
    }

    private bool doFoldout(string name, bool isOpen, System.Action fill) {
        isOpen = EditorGUILayout.Foldout(isOpen, name);
        if (isOpen) {
            EditorGUI.indentLevel += 1;
            fill();
            EditorGUI.indentLevel -= 1;
        }
        return isOpen;
    }
}
