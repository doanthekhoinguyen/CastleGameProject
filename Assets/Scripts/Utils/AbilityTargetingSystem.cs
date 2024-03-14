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
        private List<HeroSlotView> heroSlots;
        private List<HeroSlotView> enemySlots;

        public AbilityTargetingSystem(List<HeroSlotView> heroSlots, List<HeroSlotView> enemySlots)
        {
            this.heroSlots = heroSlots;
            this.enemySlots = enemySlots;
        }


        public HeroSlotView GetTarget(AbilityModel ability, HeroSlotView casterSlot)
        {
            List<HeroSlotView> targetSlots = heroSlots.Contains(casterSlot) ? heroSlots : enemySlots;
            List<HeroSlotView> oppositeSlots = heroSlots.Contains(casterSlot) ? enemySlots : heroSlots;

            switch (ability.target)

            {
                case AbilityTarget.Self:
                    return casterSlot;

                case AbilityTarget.Behind:
                    // Tìm target phía sau trong cùng phe
                    int behindIndex = targetSlots.IndexOf(casterSlot) + 1;
                    if (behindIndex < targetSlots.Count)
                    {
                        return targetSlots[behindIndex];
                    }
                    break;


                case AbilityTarget.Random:
                    if (oppositeSlots.Count > 0)
                    {
                        return oppositeSlots[UnityEngine.Random.Range(0, oppositeSlots.Count)];
                    }
                    break;
                    //// Trả về một slot ngẫu nhiên, trừ slot hiện tại của caster
                    //List<HeroSlotView> otherSlots = new List<HeroSlotView>(heroSlots);
                    //otherSlots.Remove(casterSlot);
                    //if (otherSlots.Count > 0)
                    //{
                    //    return otherSlots[UnityEngine.Random.Range(0, otherSlots.Count)];
                    //}
                    //break;
                    // Thêm các logic targeting khác nếu cần
            }

            return null; // Trả về null nếu không tìm thấy target phù hợp
        }
    }
}