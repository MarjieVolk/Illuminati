using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NodeData))]
class NodeDataEditor : Editor
{
    private NodeData node;

    private static bool spritesFoldout = false;
    private static bool statsFoldout = false;
    private static bool ownershipFoldout = false;

    void OnEnable() {
        node = (NodeData) target;
    }

    public override void OnInspectorGUI() {
        statsFoldout = doFoldout("Stats", statsFoldout, () => {
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("Attack\t\tDefense");
            EditorGUILayout.EndHorizontal();

            Array values = Enum.GetValues(typeof(DominationType));
            foreach (DominationType type in values) {
                EditorGUILayout.BeginHorizontal();
                node.getAttackSkill(type).value = EditorGUILayout.IntField("" + type.ToString(), node.getAttackSkill(type).value);
                node.getDefenseSkill(type).value = EditorGUILayout.IntField(node.getDefenseSkill(type).value);
                EditorGUILayout.EndHorizontal();
            }
        });

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
