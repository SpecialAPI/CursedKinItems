using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;

namespace CursedKinItems
{
    public class CursedBandana : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Cursed Bandana"; 
            string resourceName = "CursedKinItems/Resources/green_bandana"; 
            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<CursedBandana>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Kaliber's Fury";
            string longDesc = "The Bandanna worn by the kin that betrayed Kaliber. It is imbued with her rage.\n\nThe more her curse grows, the more hard it is to un-jam a gun, but her curse also grant power as well.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "spapi");
            item.quality = PickupObject.ItemQuality.C;
            item.CanBeDropped = false;
            item.CanBeSold = false;
        }

        protected override void Update()
        {
            base.Update();
            if(this.m_pickedUp && this.m_owner != null)
            {
                this.curse = this.m_owner.stats.GetStatValue(PlayerStats.StatType.Curse);
                if(this.curse != this.last_curse)
                {
                    this.RemoveStat(PlayerStats.StatType.ReloadSpeed);
                    this.RemoveStat(PlayerStats.StatType.Damage);
                    float dmg_value = this.curse * 0.05f;
                    float speed_value = this.curse + 1;
                    this.AddStat(PlayerStats.StatType.Damage, dmg_value, StatModifier.ModifyMethod.ADDITIVE);
                    this.AddStat(PlayerStats.StatType.ReloadSpeed, speed_value, StatModifier.ModifyMethod.MULTIPLICATIVE);
                    this.m_owner.stats.RecalculateStats(this.m_owner, true, false);
                    this.last_curse = this.curse;
                }
            }
        }

        private void AddStat(PlayerStats.StatType statType, float amount, StatModifier.ModifyMethod method = StatModifier.ModifyMethod.ADDITIVE)
        {
            StatModifier statModifier = new StatModifier();
            statModifier.amount = amount;
            statModifier.statToBoost = statType;
            statModifier.modifyType = method;
            foreach (StatModifier statModifier2 in this.passiveStatModifiers)
            {
                bool flag = statModifier2.statToBoost == statType;
                if (flag)
                {
                    return;
                }
            }
            bool flag2 = this.passiveStatModifiers == null;
            if (flag2)
            {
                this.passiveStatModifiers = new StatModifier[]
                {
                    statModifier
                };
                return;
            }
            this.passiveStatModifiers = this.passiveStatModifiers.Concat(new StatModifier[]
            {
                statModifier
            }).ToArray<StatModifier>();
        }

        private void RemoveStat(PlayerStats.StatType statType)
        {
            List<StatModifier> list = new List<StatModifier>();
            for (int i = 0; i < this.passiveStatModifiers.Length; i++)
            {
                bool flag = this.passiveStatModifiers[i].statToBoost != statType;
                if (flag)
                {
                    list.Add(this.passiveStatModifiers[i]);
                }
            }
            this.passiveStatModifiers = list.ToArray();
        }

        private float curse = 0;

        private float last_curse = 0;
    }
}
