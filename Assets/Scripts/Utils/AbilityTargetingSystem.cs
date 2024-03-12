using System;
using System.Collections;
using System.Collections.Generic;
using Castle.CustomUtil;
using Castle.CustomUtil.EventManager;
using MVC.Model;
using MVC.View;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Castle.CustomUtil
{
    public class AbilityTargetingSystem
    {
        private List<HeroSlotView> heroSlots; // Danh sách các HeroSlot trong game

        public AbilityTargetingSystem(List<HeroSlotView> heroSlots)
        {
            this.heroSlots = heroSlots;
        }

        public HeroSlotView GetTarget(AbilityModel ability, HeroSlotView casterSlot)
        {
            switch (ability.target)

            {
                case AbilityTarget.Self:
                    return casterSlot;

                case AbilityTarget.Behind:
                    // SlotIndex 0 là slot đầu tiên, không có slot nào ở phía trước
                    int behindIndex = casterSlot.Data.SlotIndex + 1;
                    if (behindIndex < heroSlots.Count) // Kiểm tra xem có slot nào ở phía trước không
                    {
                        return heroSlots[behindIndex]; // Slot phía trước
                    }
                    break;

                case AbilityTarget.Random:
                    // Trả về một slot ngẫu nhiên, trừ slot hiện tại của caster
                    List<HeroSlotView> otherSlots = new List<HeroSlotView>(heroSlots);
                    otherSlots.Remove(casterSlot);
                    if (otherSlots.Count > 0)
                    {
                        return otherSlots[UnityEngine.Random.Range(0, otherSlots.Count)];
                    }
                    break;
                    // Thêm các logic targeting khác nếu cần
            }

            return null; // Trả về null nếu không tìm thấy target phù hợp
        }
    }
}