using System;
using UnityEngine;
using UnityEditor;

public static class EditorUtils
{
    [MenuItem("CONTEXT/BoxCollider2D/StretchToMatchSprite")]
    private static void Match(MenuCommand menuCommand)
    {
        var box = menuCommand.context as BoxCollider2D;
        var sr = box.GetComponent<SpriteRenderer>();

        if (sr == null) return;

        var offset = new Vector2(.5f - sr.sprite.pivot.x / sr.sprite.rect.width,
            .5f - sr.sprite.pivot.y / sr.sprite.rect.height);

        Undo.RecordObject(box, "Box props");

        box.offset = new Vector2(offset.x * sr.size.x, offset.y * sr.size.y);
        box.size = sr.size;
    }
}