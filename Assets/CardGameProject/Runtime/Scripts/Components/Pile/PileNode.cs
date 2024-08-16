using CardGameProject;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGameProject
{
    public class PileNode : MonoBehaviour, IPileNode
    {
        PileBehaviour _pile;
        GameObject _attached = null;
        Vector2 _positionOffset = Vector2.zero;

        public GameObject Attached => _attached;

        public Vector2 OffsetPosition
        {
            get
            {
                return _positionOffset;
            }
            set
            {
                _positionOffset = value;
            }
        }

        public void Attach(GameObject go)
        {
            _attached = go;
        }
        public void Detach()
        {
            _attached = null;
        }

        public void Setup(PileBehaviour pile)
        {
            this._pile = pile;
        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }
    }
}