using UnityEngine;

[System.Serializable]
    public class StoreRandomItemData
    {
        public ItemData data;
        [Range(0,100)]public int Chance;
    }
