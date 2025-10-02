using UnityEngine;

namespace Maffin.InvetorySystem.Items
{
    [CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
    public class Item : ScriptableObject
    {
        public string itemName;
        [TextArea] public string description;
        public uint StackSize;
    }
}