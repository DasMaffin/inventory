using System.ComponentModel;
using UnityEngine;

namespace Maffin.InvetorySystem.Items
{
    [CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
    public class Item : ScriptableObject
    {
        public string itemName;
        [TextArea] public string description;
        [DefaultValue(10)]
        public uint StackSize;
        public Sprite Icon;
    }
}