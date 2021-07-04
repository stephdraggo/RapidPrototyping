using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Itemising
{
    [CreateAssetMenu(fileName = "Item Handler", menuName = "Itemising/Item Handler")]
    public class ItemManager : ScriptableObject
    {
        //make sure there is only one item handler!

        #region Variables

        //--------------Serialised Protected-------------
        [SerializeField]
        protected ItemGeneric[] items;

        #endregion

        #region Methods

        /// <summary>
        /// Tries to return an item whose id matches the one passed. Returns the first item in the array if no matching id found.
        /// </summary>
        /// <param name="id">id to search for</param>
        /// <returns>item</returns>
        public ItemGeneric GetItem(int id) {
            foreach (ItemGeneric item in items) {
                if (item == null) {
                    Debug.LogWarning("The item handler contains null items. Remove null items to optimise handler.");
                    break;
                }

                if (item.Id == id) return item;
            }

            Debug.LogWarning("ID not found, returning first non-null item.");
            foreach (ItemGeneric item in items) {
                if (item != null) return item;
            }

            throw new NullReferenceException("No items found in handler. Unable to return an item.");
        }

        #endregion
    }

    //todo
    //in editor make sure no items share ids
}