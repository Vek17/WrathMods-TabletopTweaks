﻿using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.Feats {
    static class ShatterDefenses {
        public static void AddShatterDefensesBuffs() {
            var ShatterDefenses = Resources.GetBlueprint<BlueprintFeature>("61a17ccbbb3d79445b0926347ec07577");

            var ShatterDefensesDisplayBuff = Helpers.CreateBuff("ShatterDefensesBuff", bp => {
                bp.m_Icon = ShatterDefenses.m_Icon;
                bp.Stacking = StackingType.Prolong;
                bp.SetName("Shattered Defenses");
                bp.SetDescription("An opponent you affect with Shatter Defenses is flat-footed to your attacks.");
            });
            var ShatterDefensesBuff = Helpers.CreateBuff("ShatterDefensesBuff", bp => {
                bp.m_Icon = ShatterDefenses.m_Icon;
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.Stacking = StackingType.Stack;
                bp.AddComponent<AddFactContextActions>(c => {
                    c.Activated = Helpers.CreateActionList(
                        new ContextActionApplyBuff (){
                            m_Buff = ShatterDefensesDisplayBuff.ToReference<BlueprintBuffReference>(),
                            DurationValue = new ContextDurationValue() {
                                m_IsExtendable = false,
                                Rate = DurationRate.Rounds,
                                DiceCountValue = 0,
                                BonusValue = 2
                            },
                            AsChild = true
                        }
                    );
                    c.NewRound = Helpers.CreateActionList();
                    c.Deactivated = Helpers.CreateActionList();
                });
                bp.SetName("Shattered Defenses");
                bp.SetDescription("An opponent you affect with Shatter Defenses is flat-footed to your attacks.");
            });
            var ShatterDefensesMythicBuff = Helpers.CreateBuff("ShatterDefensesMythicBuff", bp => {
                bp.m_Icon = ShatterDefenses.m_Icon;
                bp.SetName("Shattered Defenses (Mythic)");
                bp.SetDescription("An opponent affected by Shatter Defenses is flat-footed to all attacks.");
            });
            var ShatterDefensesMythicFeat = Helpers.CreateBlueprint<BlueprintFeature>("ShatterDefensesMythicFeat", bp => {
                bp.m_Icon = ShatterDefenses.m_Icon;
                bp.SetName("Shatter Defenses (Mythic)");
                bp.SetDescription("An opponent you affect with Shatter Defenses is flat-footed to all attacks, not just yours.");
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[] { FeatureGroup.MythicFeat };
                bp.AddPrerequisiteFeature(ShatterDefenses);
            });
            if (ModSettings.Fixes.Feats.IsDisabled("ShatterDefenses")) { return; }
            if (ModSettings.Fixes.Feats.IsDisabled("ShatterDefenses")) { return; }
            FeatTools.AddAsMythicFeat(ShatterDefensesMythicFeat);
        }
    }
}