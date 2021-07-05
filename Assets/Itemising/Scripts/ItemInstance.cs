using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Collections;
using UnityEngine;

namespace Itemising
{
    public class ItemInstance : MonoBehaviour
    {
        #region Variables

        //-------------Public Properties-------------
        public int Id => id;
        public ItemGeneric SourceItem {
            get {
                if (sourceItem != null) return sourceItem;
                sourceItem = GlobalVars.Instance.itemManager.GetItem(id);
                return sourceItem;
            }
        }
        public string GetDisplayMeasure => SourceItem.GetDisplayMeasure(amount);
        public Sprite GetSprite => overrideSourceSprites ? sprite : SourceItem.GetSprite(spriteIndex);

        //--------------Serialised Protected-------------

        [SerializeField]
        protected int id;

        [SerializeField] //editor todo: make this allow float or int only depending on source measurement type
        protected float amount;

        //sprite
        [SerializeField]
        protected bool overrideSourceSprites = false;
        [SerializeField,
         Min(-1)] //editor todo: only show this field if source allows alt sprites, also limit input values, also hide if override source sprites is true
        [Tooltip("To ignore alternate sprites, index should be -1.")] //editor todo: make this a help box
        protected int spriteIndex = -1;
        [SerializeField] //editor todo: only show if override source sprites is true
        protected Sprite sprite;

        //--------------Non-Serialised Protected---------
        protected ItemGeneric sourceItem;

        #endregion

        private void OnValidate() {
            ItemGeneric item = SourceItem;
        }

        private void Awake() { }

        // Start is called before the first frame update
        void Start() {
            ItemGeneric item = SourceItem; //set this item's source
        }

        // Update is called once per frame
        void Update() { }
    }
}