﻿using Kingmaker.Blueprints;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Utility;
using System;
using UnityEngine;

namespace TabletopTweaks.NewComponents {
    class DestinedArcanaComponent: UnitFactComponentDelegate,
        IInitiatorRulebookHandler<RuleCastSpell>,
        IRulebookHandler<RuleCastSpell>, ISubscriber,
        IInitiatorRulebookSubscriber {

        public void OnEventAboutToTrigger(RuleCastSpell evt) {
            if (evt.Spell != null && evt.Spell.Spellbook != null && evt.Spell.Blueprint.Type == AbilityType.Spell && evt.Spell.Blueprint.Range == AbilityRange.Personal) {
                int level = evt.Context.SpellLevel - 1;
                if (level > 8 || level < 0) { return; }
                ApplyBuff(evt.Context, level);
            }
        }

        private void ApplyBuff(MechanicsContext mechanicsContext, int buff) {
            _ = Owner.AddBuff(Buffs[buff].Get(), mechanicsContext, new Rounds(1).Seconds);
        }

        public void OnEventDidTrigger(RuleCastSpell evt) {
            
        }
#pragma warning disable 0649
        [SerializeField]
        public BlueprintBuffReference[] Buffs;
#pragma warning restore 0649
    }
}
