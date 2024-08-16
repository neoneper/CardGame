using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GMBEditor
{
    public static class GMBEditorStyles
    {
        private static Sprite _sprite_unknow = null;

        public static Sprite SpriteUnKnow
        {
            get
            {
                if (_sprite_unknow == null)
                {
                    _sprite_unknow = AssetDatabase.LoadAssetAtPath<Sprite>(EditorStringsProvider._PATH_GMB_RESOURCES_.Combine("Icons/Undefined.png"));
                }

                return _sprite_unknow;

            }
        }
        public static Texture2D TextureUnKnow
        {
            get
            {
                return SpriteUnKnow.texture;
            }
        }
    }
}