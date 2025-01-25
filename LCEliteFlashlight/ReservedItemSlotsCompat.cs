using ReservedItemSlotCore.Data;
using UnityEngine;

namespace LCEliteFlashlight
{
    public static class ReservedItemSlotsCompat
    {
        public static void AddItemsToReservedItemSlots()
        {
            // Arguments: ItemDisplayName, ParentBone(enum), PositionOffset, RotationOffset
            ReservedItemData myReservedItemData = new ReservedItemData("Elite-flashlight", PlayerBone.Spine3, new Vector3(0.2f, 0.25f, 0), new Vector3(90, 0, 0));

            // It's okay if the mod that created this item slot isn't enabled on the client. Nothing will break
            ReservedItemSlotData.TryAddItemDataToReservedItemSlot(myReservedItemData, "flashlight");
        }
    }
}
