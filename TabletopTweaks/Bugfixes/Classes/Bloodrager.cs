﻿using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.ElementsSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.Bugfixes.Classes {
    class Bloodrager {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                Main.LogHeader("Patching Bloodrager");

                PatchBaseClass();
                PatchPrimalist();
                PatchReformedFiend();
                PatchArcaneBloodrage();
                PatchGreaterArcaneBloodrage();
                PatchTrueArcaneBloodrage();
            }
            static void PatchBaseClass() {
                PatchSpellbook();
                PatchAbysalBulk();

                void PatchAbysalBulk() {
                    if (ModSettings.Fixes.Bloodrager.Base.IsDisabled("AbysalBulk")) { return; }
                    var BloodragerAbyssalBloodlineBaseBuff = Resources.GetBlueprint<BlueprintBuff>("2ba7b4b3b87156543b43d0686404655a");
                    var BloodragerAbyssalDemonicBulkBuff = Resources.GetBlueprint<BlueprintBuff>("031a8053a7c02ab42ad53f50dd2e9437");
                    var BloodragerAbyssalDemonicBulkEnlargeBuff = Resources.GetModBlueprint<BlueprintBuff>("BloodragerAbyssalDemonicBulkEnlargeBuff");

                    var ApplyBuff = new ContextActionApplyBuff() {
                        m_Buff = BloodragerAbyssalDemonicBulkEnlargeBuff.ToReference<BlueprintBuffReference>(),
                        AsChild = true,
                        Permanent = true
                    };
                    var RemoveBuff = new ContextActionRemoveBuff() {
                        m_Buff = BloodragerAbyssalDemonicBulkEnlargeBuff.ToReference<BlueprintBuffReference>()
                    };
                    var AddFactContext = BloodragerAbyssalBloodlineBaseBuff.GetComponent<AddFactContextActions>();

                    AddFactContext.Activated.Actions.OfType<Conditional>().Where(a => a.Comment.Equals("Demonic Bulk")).First().AddActionIfTrue(ApplyBuff);
                    AddFactContext.Deactivated.Actions.OfType<Conditional>().Where(a => a.Comment.Equals("Demonic Bulk")).First().IfTrue = null;
                    AddFactContext.Deactivated.Actions.OfType<Conditional>().Where(a => a.Comment.Equals("Demonic Bulk")).First().AddActionIfTrue(RemoveBuff);
                }
                void PatchSpellbook() {
                    if (ModSettings.Fixes.Bloodrager.Base.IsDisabled("Spellbook")) { return; }
                    BlueprintSpellbook BloodragerSpellbook = Resources.GetBlueprint<BlueprintSpellbook>("e19484252c2f80e4a9439b3681b20f00");
                    var BloodragerSpellKnownTable = BloodragerSpellbook.SpellsKnown;
                    var BloodragerSpellPerDayTable = BloodragerSpellbook.SpellsPerDay;
                    BloodragerSpellbook.CasterLevelModifier = 0;
                    BloodragerSpellKnownTable.Levels = new SpellsLevelEntry[] {
                        SpellTools.CreateSpellLevelEntry(0),
                        SpellTools.CreateSpellLevelEntry(0),
                        SpellTools.CreateSpellLevelEntry(0),
                        SpellTools.CreateSpellLevelEntry(0),
                        SpellTools.CreateSpellLevelEntry(0,2),
                        SpellTools.CreateSpellLevelEntry(0,3),
                        SpellTools.CreateSpellLevelEntry(0,4),
                        SpellTools.CreateSpellLevelEntry(0,4,2),
                        SpellTools.CreateSpellLevelEntry(0,4,3),
                        SpellTools.CreateSpellLevelEntry(0,5,4),
                        SpellTools.CreateSpellLevelEntry(0,5,4,2),
                        SpellTools.CreateSpellLevelEntry(0,5,4,3),
                        SpellTools.CreateSpellLevelEntry(0,6,5,4),
                        SpellTools.CreateSpellLevelEntry(0,6,5,4,2),
                        SpellTools.CreateSpellLevelEntry(0,6,5,4,3),
                        SpellTools.CreateSpellLevelEntry(0,6,6,5,4),
                        SpellTools.CreateSpellLevelEntry(0,6,6,5,4),
                        SpellTools.CreateSpellLevelEntry(0,6,6,5,4),
                        SpellTools.CreateSpellLevelEntry(0,6,6,6,5),
                        SpellTools.CreateSpellLevelEntry(0,6,6,6,5),
                        SpellTools.CreateSpellLevelEntry(0,6,6,6,5)
                    };
                    BloodragerSpellPerDayTable.Levels = new SpellsLevelEntry[] {
                        SpellTools.CreateSpellLevelEntry(0),
                        SpellTools.CreateSpellLevelEntry(0),
                        SpellTools.CreateSpellLevelEntry(0),
                        SpellTools.CreateSpellLevelEntry(0),
                        SpellTools.CreateSpellLevelEntry(0,1),
                        SpellTools.CreateSpellLevelEntry(0,1),
                        SpellTools.CreateSpellLevelEntry(0,1),
                        SpellTools.CreateSpellLevelEntry(0,1,1),
                        SpellTools.CreateSpellLevelEntry(0,1,1),
                        SpellTools.CreateSpellLevelEntry(0,2,1),
                        SpellTools.CreateSpellLevelEntry(0,2,1,1),
                        SpellTools.CreateSpellLevelEntry(0,2,1,1),
                        SpellTools.CreateSpellLevelEntry(0,2,2,1),
                        SpellTools.CreateSpellLevelEntry(0,3,2,1,1),
                        SpellTools.CreateSpellLevelEntry(0,3,2,1,1),
                        SpellTools.CreateSpellLevelEntry(0,3,2,2,1),
                        SpellTools.CreateSpellLevelEntry(0,3,3,2,1),
                        SpellTools.CreateSpellLevelEntry(0,4,3,2,1),
                        SpellTools.CreateSpellLevelEntry(0,4,3,2,2),
                        SpellTools.CreateSpellLevelEntry(0,4,3,3,2),
                        SpellTools.CreateSpellLevelEntry(0,4,4,3,2)
                    };
                    Main.LogPatch("Patched", BloodragerSpellPerDayTable);
                }
            }
            static void PatchPrimalist() {
                PatchRagePowerFeatQualifications();
                PatchPrimalistRageBuffs();

                void PatchRagePowerFeatQualifications() {
                    if (ModSettings.Fixes.Bloodrager.Archetypes["Primalist"].IsDisabled("RagePowerFeatQualifications")) { return; }
                    var PrimalistTakeRagePowers4 = Resources.GetBlueprint<BlueprintProgression>("8eb5c34bb8471a0438e7eb3994de3b92");
                    var PrimalistTakeRagePowers8 = Resources.GetBlueprint<BlueprintProgression>("db2710cd915bbcf4193fa54083e56b27");
                    var PrimalistTakeRagePowers12 = Resources.GetBlueprint<BlueprintProgression>("e43a7bfd5c90a514cab1c11b41c550b1");
                    var PrimalistTakeRagePowers16 = Resources.GetBlueprint<BlueprintProgression>("b6412ff44f3a82f499d0dd6748a123bc");
                    var PrimalistTakeRagePowers20 = Resources.GetBlueprint<BlueprintProgression>("5905a80d5934248439e83612d9101b4b");

                    PatchPrimalistTakeRagePowers(PrimalistTakeRagePowers4, 4);
                    PatchPrimalistTakeRagePowers(PrimalistTakeRagePowers8, 8);
                    PatchPrimalistTakeRagePowers(PrimalistTakeRagePowers12, 12);
                    PatchPrimalistTakeRagePowers(PrimalistTakeRagePowers16, 16);
                    PatchPrimalistTakeRagePowers(PrimalistTakeRagePowers20, 20);

                    void PatchPrimalistTakeRagePowers(BlueprintProgression PrimalistTakeRagePowers, int level) {
                        var PrimalistRagePowerSelection = Resources.GetModBlueprint<BlueprintFeatureSelection>("PrimalistRagePowerSelection");
                        PrimalistTakeRagePowers.LevelEntries = new LevelEntry[] {
                            new LevelEntry {
                                Level = level,
                                Features = {
                                    PrimalistRagePowerSelection,
                                    PrimalistRagePowerSelection
                                }
                            }
                        };
                        Main.LogPatch("Patched", PrimalistTakeRagePowers);
                    }
                }
                static void PatchPrimalistRageBuffs() {
                    if (ModSettings.Fixes.Bloodrager.Archetypes["Primalist"].IsDisabled("FixBrokenRagePowers")) { return; }
                    var BloodragerStandartRageBuff = Resources.GetBlueprint<BlueprintBuff>("5eac31e457999334b98f98b60fc73b2f");
                    PatchCelestialTotem();
                    PatchDaemonTotem();
                    PatchFiendTotem();
                    PatchPowerfulStance();
                    PatchScentRagePower();

                    void PatchCelestialTotem() {

                        var CelestialTotemLesserFeature = Resources.GetBlueprint<BlueprintFeature>("aba61e0b0e66bf3439cc247ee89fddae");
                        var CelestialTotemLesserBuff = Resources.GetBlueprint<BlueprintBuff>("fe27c0d9b9dc6a74aa88887b561ad5f3");

                        CelestialTotemLesserFeature.AddComponent<BuffExtraEffects>(c => {
                            c.m_CheckedBuff = BloodragerStandartRageBuff.ToReference<BlueprintBuffReference>();
                            c.m_ExtraEffectBuff = CelestialTotemLesserBuff.ToReference<BlueprintBuffReference>();
                        });
                        Main.LogPatch("Patched", CelestialTotemLesserFeature);

                        var CelestialTotemFeature = Resources.GetBlueprint<BlueprintFeature>("5156331dc888e9347ae6fc81ad3f3cec");
                        var CelestialTotemAreaBuff = Resources.GetBlueprint<BlueprintBuff>("7bf740b33eaa2534e91def3cef142e00");

                        CelestialTotemFeature.AddComponent<BuffExtraEffects>(c => {
                            c.m_CheckedBuff = BloodragerStandartRageBuff.ToReference<BlueprintBuffReference>();
                            c.m_ExtraEffectBuff = CelestialTotemAreaBuff.ToReference<BlueprintBuffReference>();
                        });
                        Main.LogPatch("Patched", CelestialTotemFeature);

                        var CelestialTotemGreaterFeature = Resources.GetBlueprint<BlueprintFeature>("774f79845d1683a43aa42ebd2a549497");
                        var CelestialTotemGreaterBuff = Resources.GetBlueprint<BlueprintBuff>("e31276241f875254cb102329c0b55ba7");

                        CelestialTotemGreaterFeature.GetComponent<PrerequisiteArchetypeLevel>().Group = Prerequisite.GroupType.Any;

                        CelestialTotemGreaterFeature.AddComponent<BuffExtraEffects>(c => {
                            c.m_CheckedBuff = BloodragerStandartRageBuff.ToReference<BlueprintBuffReference>();
                            c.m_ExtraEffectBuff = CelestialTotemGreaterBuff.ToReference<BlueprintBuffReference>();
                        });
                        Main.LogPatch("Patched", CelestialTotemGreaterFeature);
                    }

                    void PatchDaemonTotem() {
                        var DaemonTotemLesserFeature = Resources.GetBlueprint<BlueprintFeature>("45102fd7aab96f94d81ec80768549e12");
                        var DaemonTotemLesserBaseBuff = Resources.GetBlueprint<BlueprintBuff>("a8957a1c6f212244d969645bc2fa7c25");

                        DaemonTotemLesserFeature.AddComponent<BuffExtraEffects>(c => {
                            c.m_CheckedBuff = BloodragerStandartRageBuff.ToReference<BlueprintBuffReference>();
                            c.m_ExtraEffectBuff = DaemonTotemLesserBaseBuff.ToReference<BlueprintBuffReference>();
                        });
                        Main.LogPatch("Patched", DaemonTotemLesserFeature);

                        var DaemonTotemFeature = Resources.GetBlueprint<BlueprintFeature>("d673c30720e8e7c4bb0903dc3c9ab649");
                        var DaemonTotemBuff = Resources.GetBlueprint<BlueprintBuff>("a4195deeb13eb9c4b93b6987839b60c7");

                        DaemonTotemFeature.AddComponent<BuffExtraEffects>(c => {
                            c.m_CheckedBuff = BloodragerStandartRageBuff.ToReference<BlueprintBuffReference>();
                            c.m_ExtraEffectBuff = DaemonTotemBuff.ToReference<BlueprintBuffReference>();
                        });
                        Main.LogPatch("Patched", DaemonTotemFeature);

                        var DaemonTotemGreaterFeature = Resources.GetBlueprint<BlueprintFeature>("9a2f0ffe517d221459640a4ad85710d7");
                        var DaemonTotemGreaterBuff = Resources.GetBlueprint<BlueprintBuff>("2cf21dce5ecc791449f3106fcd0b60c3");

                        DaemonTotemGreaterFeature.AddComponent<BuffExtraEffects>(c => {
                            c.m_CheckedBuff = BloodragerStandartRageBuff.ToReference<BlueprintBuffReference>();
                            c.m_ExtraEffectBuff = DaemonTotemGreaterBuff.ToReference<BlueprintBuffReference>();
                        });
                        Main.LogPatch("Patched", DaemonTotemGreaterFeature);
                    }

                    void PatchFiendTotem() {
                        var PrimalistRagePowersBuff = Resources.GetBlueprint<BlueprintBuff>("ecc22ca1eea1bf6488a0d7c6ee2527d8");

                        var FiendTotemLesserFeature = Resources.GetBlueprint<BlueprintFeature>("76437492f801f054ba536473ad2fde79");
                        var FiendTotemLesserRageBuff = Resources.GetBlueprint<BlueprintBuff>("d0649010d93907745a44034ad6eeeb5e");
                        PrimalistRagePowersBuff.AddConditionalBuff(FiendTotemLesserFeature, FiendTotemLesserRageBuff);
                        Main.LogPatch("Patched", FiendTotemLesserFeature);

                        var FiendTotemFeature = Resources.GetBlueprint<BlueprintFeature>("ce449404eeb4a7c499fbe0248056174f");
                        var FiendTotemRageBuff = Resources.GetBlueprint<BlueprintBuff>("4f524a75bb13f7c40806b0c19dc06fe4");
                        PrimalistRagePowersBuff.AddConditionalBuff(FiendTotemFeature, FiendTotemRageBuff);
                        Main.LogPatch("Patched", FiendTotemFeature);

                        var FiendTotemGreaterFeature = Resources.GetBlueprint<BlueprintFeature>("1105632657d94d940a43707a3a57b006");
                        var FiendTotemGreaterRageBuff = Resources.GetBlueprint<BlueprintBuff>("c84ca8f21f63c8249a192f34195f8787");
                        PrimalistRagePowersBuff.AddConditionalBuff(FiendTotemGreaterFeature, FiendTotemGreaterRageBuff);
                        Main.LogPatch("Patched", FiendTotemGreaterFeature);
                    }

                    void PatchPowerfulStance() {
                        var PowerfulStanceSwitchBuff = Resources.GetBlueprint<BlueprintBuff>("539e480bcfe6d6f48bdd90418240b50f");
                        var PowerfulStanceEffectBuff = Resources.GetBlueprint<BlueprintBuff>("aabad91034e5c7943986fe3e83bfc78e");

                        PowerfulStanceSwitchBuff.AddComponent<BuffExtraEffects>(c => {
                            c.m_CheckedBuff = BloodragerStandartRageBuff.ToReference<BlueprintBuffReference>();
                            c.m_ExtraEffectBuff = PowerfulStanceEffectBuff.ToReference<BlueprintBuffReference>();
                        });
                        Main.LogPatch("Patched", PowerfulStanceSwitchBuff);
                    }

                    void PatchScentRagePower() {
                        var PrimalistRagePowersBuff = Resources.GetBlueprint<BlueprintBuff>("ecc22ca1eea1bf6488a0d7c6ee2527d8");

                        var ScentFeature = Resources.GetBlueprint<BlueprintFeature>("6e5d57a733d1eea46a9022a304f2c728");
                        var ScentRageBuff = Resources.GetBlueprint<BlueprintBuff>("879e6a7ed8101404d8e4f1bc25c0d34f");
                        PrimalistRagePowersBuff.AddConditionalBuff(ScentFeature, ScentRageBuff);
                        Main.LogPatch("Patched", ScentFeature);
                    }
                }
            }
            static void PatchReformedFiend() {
                PatchHatredAgainstEvil();
                PatchDamageReduction();

                void PatchHatredAgainstEvil() {
                    if (ModSettings.Fixes.Bloodrager.Archetypes["ReformedFiend"].IsDisabled("HatredAgainstEvil")) { return; }
                    var BloodragerClass = Resources.GetBlueprint<BlueprintCharacterClass>("d77e67a814d686842802c9cfd8ef8499");
                    var ReformedFiendBloodrageBuff = Resources.GetBlueprint<BlueprintBuff>("72a679f712bd4f69a07bf03d5800900b");
                    var rankConfig = ReformedFiendBloodrageBuff.GetComponent<ContextRankConfig>();

                    rankConfig.m_BaseValueType = ContextRankBaseValueType.ClassLevel;
                    rankConfig.m_Class = new BlueprintCharacterClassReference[] { BloodragerClass.ToReference<BlueprintCharacterClassReference>() };
                    rankConfig.m_UseMin = true;
                }
                void PatchDamageReduction() {
                    if (ModSettings.Fixes.Bloodrager.Archetypes["ReformedFiend"].IsDisabled("DamageReduction")) { return; }
                    var ReformedFiendDamageReductionFeature = Resources.GetBlueprint<BlueprintFeature>("2a3243ad1ccf43d5a5d69de3f9d0420e");
                    ReformedFiendDamageReductionFeature.GetComponent<AddDamageResistancePhysical>().BypassedByAlignment = true;
                }
            }

            static void PatchArcaneBloodrage() {
                var BloodragerClass = Resources.GetBlueprint<BlueprintCharacterClass>("d77e67a814d686842802c9cfd8ef8499");
                var BloodragerStandartRageBuff = Resources.GetBlueprint<BlueprintBuff>("5eac31e457999334b98f98b60fc73b2f");
                var BloodragerArcaneSpellFeature = Resources.GetBlueprint<BlueprintFeature>("3584b932341ecf14fbaaa87bf337c2cf");
                var BloodragerArcaneSpellAbility = Resources.GetBlueprint<BlueprintAbility>("3151dfeeb202e38448d1fea1e8bc237e");
                
                var Blur = Resources.GetBlueprint<BlueprintAbility>("14ec7a4e52e90fa47a4c8d63c69fd5c1");
                var BlurBuff = Resources.GetBlueprint<BlueprintBuff>("dd3ad347240624d46a11a092b4dd4674");

                var ProtectionFromArrows = Resources.GetBlueprint<BlueprintAbility>("c28de1f98a3f432448e52e5d47c73208");
                var ProtectionFromArrowsBuff = Resources.GetBlueprint<BlueprintBuff>("241ee6bd8c8767343994bce5dc1a95e0");
                var ProtectionFromArrowsArcaneBloodragerBuff = Helpers.CreateBuff("ProtectionFromArrowsArcaneBloodrageBuff", bp => {
                    bp.m_DisplayName = ProtectionFromArrows.m_DisplayName;
                    bp.m_Description = ProtectionFromArrows.m_Description;
                    bp.m_DescriptionShort = ProtectionFromArrows.m_DescriptionShort;
                    bp.m_Icon = ProtectionFromArrowsBuff.m_Icon;
                    bp.AddComponent<AddDamageResistancePhysical>(c => {
                        c.m_WeaponType = BlueprintReferenceBase.CreateTyped<BlueprintWeaponTypeReference>(null);
                        c.Or = true;
                        c.Material = Kingmaker.Enums.Damage.PhysicalDamageMaterial.Adamantite;
                        c.BypassedByMagic = true;
                        c.MinEnhancementBonus = 1;
                        c.Alignment = Kingmaker.Enums.Damage.DamageAlignment.Good;
                        c.Reality = Kingmaker.Enums.Damage.DamageRealityType.Ghost;
                        c.BypassedByMeleeWeapon = true;
                        c.m_CheckedFactMythic = BlueprintReferenceBase.CreateTyped<BlueprintUnitFactReference>(null);
                        c.Value = new ContextValue() {
                            Value = 10
                        };
                        c.UsePool = true;
                        c.Pool = new ContextValue() {
                            ValueType = ContextValueType.Rank
                        };
                    });
                    var crc = Helpers.CreateContextRankConfig();
                    crc.m_BaseValueType = ContextRankBaseValueType.ClassLevel;
                    crc.m_Progression = ContextRankProgression.MultiplyByModifier;
                    crc.m_StepLevel = 10;
                    crc.m_UseMax = true;
                    crc.m_Max = 100;
                    crc.m_Class = new BlueprintCharacterClassReference[] {
                        BloodragerClass.ToReference<BlueprintCharacterClassReference>()
                    };
                    bp.AddComponent(crc);
                });

                var ResistFire = Resources.GetBlueprint<BlueprintAbility>("ddfb4ac970225f34dbff98a10a4a8844");
                var ResistFireBuff = Resources.GetBlueprint<BlueprintBuff>("468877871a8e3ba41813a9697ec4eb4e");

                var ResistCold = Resources.GetBlueprint<BlueprintAbility>("5368cecec375e1845ae07f48cdc09dd1");
                var ResistColdBuff = Resources.GetBlueprint<BlueprintBuff>("dfedc0bf1d93f024d85546314c42b56a");

                var ResistElectricity = Resources.GetBlueprint<BlueprintAbility>("90987584f54ab7a459c56c2d2f22cee2");
                var ResistElectricityBuff = Resources.GetBlueprint<BlueprintBuff>("17aee23103aee674082ff9891c82ae2f");

                var ResistAcid = Resources.GetBlueprint<BlueprintAbility>("fedc77de9b7aad54ebcc43b4daf8decd");
                var ResistAcidBuff = Resources.GetBlueprint<BlueprintBuff>("8d8f20391422c0e41a1650e7a9b7a21f");

                var ResistSonic = Resources.GetBlueprint<BlueprintAbility>("8d3b10f92387c84429ced317b06ad001");
                var ResistSonicBuff = Resources.GetBlueprint<BlueprintBuff>("c0f3b16ff3f79b749b121905d659a2d4");

                BlueprintBuff BloodragerArcaneSpellBlurSwitchBuff = BloodlineTools.CreateArcaneBloodrageSwitchBuff(
                    "BloodragerArcaneSpellBlurSwitchBuff",
                    "Arcane Bloodrage: Blur",
                    BloodragerArcaneSpellAbility,
                    BloodragerStandartRageBuff,
                    BlurBuff);

                BlueprintBuff BloodragerArcaneSpellProtectionFromArrowsSwitchBuff = BloodlineTools.CreateArcaneBloodrageSwitchBuff(
                    "BloodragerArcaneSpellProtectionFromArrowsSwitchBuff",
                    "Arcane Bloodrage: Protection From Arrows",
                    BloodragerArcaneSpellAbility,
                    BloodragerStandartRageBuff,
                    ProtectionFromArrowsArcaneBloodragerBuff);

                BlueprintBuff BloodragerArcaneSpellResistFireSwitchBuff = BloodlineTools.CreateArcaneBloodrageSwitchBuff(
                    "BloodragerArcaneSpellResistFireSwitchBuff",
                    "Arcane Bloodrage: Resist Fire",
                    BloodragerArcaneSpellAbility,
                    BloodragerStandartRageBuff,
                    ResistFireBuff);

                BlueprintBuff BloodragerArcaneSpellResistColdSwitchBuff = BloodlineTools.CreateArcaneBloodrageSwitchBuff(
                    "BloodragerArcaneSpellResistColdSwitchBuff",
                    "Arcane Bloodrage: Resist Cold",
                    BloodragerArcaneSpellAbility,
                    BloodragerStandartRageBuff,
                    ResistColdBuff);

                BlueprintBuff BloodragerArcaneSpellResistElectricitySwitchBuff = BloodlineTools.CreateArcaneBloodrageSwitchBuff(
                    "BloodragerArcaneSpellResistElectricitySwitchBuff",
                    "Arcane Bloodrage: Resist Electricity",
                    BloodragerArcaneSpellAbility,
                    BloodragerStandartRageBuff,
                    ResistElectricityBuff);

                BlueprintBuff BloodragerArcaneSpellResistAcidSwitchBuff = BloodlineTools.CreateArcaneBloodrageSwitchBuff(
                    "BloodragerArcaneSpellResistAcidSwitchBuff",
                    "Arcane Bloodrage: Resist Acid",
                    BloodragerArcaneSpellAbility,
                    BloodragerStandartRageBuff,
                    ResistAcidBuff);

                BlueprintBuff BloodragerArcaneSpellResistSonicSwitchBuff = BloodlineTools.CreateArcaneBloodrageSwitchBuff(
                    "BloodragerArcaneSpellResistSonicSwitchBuff",
                    "Arcane Bloodrage: Resist Sonic",
                    BloodragerArcaneSpellAbility,
                    BloodragerStandartRageBuff,
                    ResistSonicBuff);

                var AllBloodragerArcaneSpellSwitchBuffs = new List<BlueprintBuff>() {
                    BloodragerArcaneSpellBlurSwitchBuff,
                    BloodragerArcaneSpellProtectionFromArrowsSwitchBuff,
                    BloodragerArcaneSpellResistFireSwitchBuff,
                    BloodragerArcaneSpellResistColdSwitchBuff,
                    BloodragerArcaneSpellResistElectricitySwitchBuff,
                    BloodragerArcaneSpellResistAcidSwitchBuff,
                    BloodragerArcaneSpellResistSonicSwitchBuff
                };

                BlueprintAbility BloodragerArcaneSpellBlurToggle = BloodlineTools.CreateArcaneBloodrageToggle(
                    "BloodragerArcaneSpellBlurToggle",
                    Blur,
                    BloodragerArcaneSpellAbility,
                    BloodragerArcaneSpellBlurSwitchBuff,
                    AllBloodragerArcaneSpellSwitchBuffs);

                BlueprintAbility BloodragerArcaneSpellProtectionFromArrowsToggle = BloodlineTools.CreateArcaneBloodrageToggle(
                    "BloodragerArcaneSpellProtectionFromArrowsToggle",
                    ProtectionFromArrows,
                    BloodragerArcaneSpellAbility,
                    BloodragerArcaneSpellProtectionFromArrowsSwitchBuff,
                    AllBloodragerArcaneSpellSwitchBuffs);

                BlueprintAbility BloodragerArcaneSpellResistFireToggle = BloodlineTools.CreateArcaneBloodrageToggle(
                    "BloodragerArcaneSpellResistFireToggle",
                    ResistFire,
                    BloodragerArcaneSpellAbility,
                    BloodragerArcaneSpellResistFireSwitchBuff,
                    AllBloodragerArcaneSpellSwitchBuffs);
                
                BlueprintAbility BloodragerArcaneSpellResistColdToggle = BloodlineTools.CreateArcaneBloodrageToggle(
                    "BloodragerArcaneSpellResistColdToggle",
                    ResistCold,
                    BloodragerArcaneSpellAbility,
                    BloodragerArcaneSpellResistColdSwitchBuff,
                    AllBloodragerArcaneSpellSwitchBuffs);

                BlueprintAbility BloodragerArcaneSpellResistElectricityToggle = BloodlineTools.CreateArcaneBloodrageToggle(
                    "BloodragerArcaneSpellResistElectricityToggle",
                    ResistElectricity,
                    BloodragerArcaneSpellAbility,
                    BloodragerArcaneSpellResistElectricitySwitchBuff,
                    AllBloodragerArcaneSpellSwitchBuffs);
                
                BlueprintAbility BloodragerArcaneSpellResistAcidToggle = BloodlineTools.CreateArcaneBloodrageToggle(
                    "BloodragerArcaneSpellResistAcidToggle",
                    ResistAcid,
                    BloodragerArcaneSpellAbility,
                    BloodragerArcaneSpellResistAcidSwitchBuff,
                    AllBloodragerArcaneSpellSwitchBuffs);
                
                BlueprintAbility BloodragerArcaneSpellResistSonicToggle = BloodlineTools.CreateArcaneBloodrageToggle(
                    "BloodragerArcaneSpellResistSonicToggle",
                    ResistSonic,
                    BloodragerArcaneSpellAbility,
                    BloodragerArcaneSpellResistSonicSwitchBuff,
                    AllBloodragerArcaneSpellSwitchBuffs);


                BloodragerArcaneSpellAbility.GetComponent<AbilityVariants>().m_Variants = new BlueprintAbilityReference[] {
                    BloodragerArcaneSpellBlurToggle.ToReference<BlueprintAbilityReference>(),
                    BloodragerArcaneSpellProtectionFromArrowsToggle.ToReference<BlueprintAbilityReference>(),
                    BloodragerArcaneSpellResistFireToggle.ToReference<BlueprintAbilityReference>(),
                    BloodragerArcaneSpellResistColdToggle.ToReference<BlueprintAbilityReference>(),
                    BloodragerArcaneSpellResistElectricityToggle.ToReference<BlueprintAbilityReference>(),
                    BloodragerArcaneSpellResistAcidToggle.ToReference<BlueprintAbilityReference>(),
                    BloodragerArcaneSpellResistSonicToggle.ToReference<BlueprintAbilityReference>()
                };

                Main.LogPatch("Patched", BloodragerArcaneSpellAbility);
            }

            static void PatchGreaterArcaneBloodrage() {
                var BloodragerStandartRageBuff = Resources.GetBlueprint<BlueprintBuff>("5eac31e457999334b98f98b60fc73b2f");
                var BloodragerArcaneGreaterSpell = Resources.GetBlueprint<BlueprintAbility>("31dbadf586920494b87e8e95452af998");

                var Displacement = Resources.GetBlueprint<BlueprintAbility>("903092f6488f9ce45a80943923576ab3");
                var DisplacementBuff = Resources.GetBlueprint<BlueprintBuff>("00402bae4442a854081264e498e7a833");

                var Haste = Resources.GetBlueprint<BlueprintAbility>("486eaff58293f6441a5c2759c4872f98");
                var HasteBuff = Resources.GetBlueprint<BlueprintBuff>("03464790f40c3c24aa684b57155f3280");
                var SlowBuff = Resources.GetBlueprint<BlueprintBuff>("0bc608c3f2b548b44b7146b7530613ac");

                BlueprintBuff BloodragerArcaneGreaterSpellHasteActivationBuff = Helpers.CreateBuff("BloodragerArcaneGreaterSpellHasteActivationBuff", bp => {
                    bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                    bp.IsClassFeature = true;
                    bp.SetName("Greater Arcane Bloodrage: Haste");
                    bp.m_Description = HasteBuff.m_Description;
                    bp.AddComponent<AddFactContextActions>(c => {
                        c.Activated = new ActionList() {
                            Actions = new GameAction[] {
                                new Conditional() {
                                    name = "HasteActivationBuffCondition",
                                    Comment = "",
                                    ConditionsChecker = new ConditionsChecker() {
                                        Conditions = new Condition[] {
                                            new ContextConditionHasBuff {
                                                m_Buff = SlowBuff.ToReference<BlueprintBuffReference>()
                                            }
                                        }
                                    },
                                    IfTrue = new ActionList() {
                                        Actions = new GameAction[] {
                                            new ContextActionRemoveBuff() {
                                                m_Buff = SlowBuff.ToReference<BlueprintBuffReference>()
                                            }
                                        }
                                    },
                                    IfFalse = new ActionList() {
                                        Actions = new GameAction[] {
                                            new ContextActionApplyBuff() {
                                                m_Buff = HasteBuff.ToReference<BlueprintBuffReference>(),
                                                Permanent = true,
                                                AsChild = true,
                                                DurationValue = new ContextDurationValue(),
                                                IsFromSpell = false
                                            }
                                        }
                                    }
                                }
                            }
                        };
                        c.Deactivated = new ActionList();
                        c.NewRound = new ActionList();
                    });
                });

                BlueprintBuff BloodragerArcaneGreaterSpellDisplacementSwitchBuff = BloodlineTools.CreateArcaneBloodrageSwitchBuff(
                    "BloodragerArcaneGreaterSpellDisplacementSwitchBuff",
                    "Greater Arcane Bloodrage: Displacement",
                    BloodragerArcaneGreaterSpell,
                    BloodragerStandartRageBuff,
                    DisplacementBuff);
                
                BlueprintBuff BloodragerArcaneGreaterSpellHasteSwitchBuff = BloodlineTools.CreateArcaneBloodrageSwitchBuff(
                    "BloodragerArcaneGreaterSpellHasteSwitchBuff",
                    "Greater Arcane Bloodrage: Haste",
                    BloodragerArcaneGreaterSpell,
                    BloodragerStandartRageBuff,
                    BloodragerArcaneGreaterSpellHasteActivationBuff);

                var AllBloodragerArcaneGreaterSpellSwitchBuffs = new List<BlueprintBuff>() {
                    BloodragerArcaneGreaterSpellDisplacementSwitchBuff,
                    BloodragerArcaneGreaterSpellHasteSwitchBuff
                };


                BlueprintAbility BloodragerArcaneSpellGreaterDisplacementToggle = BloodlineTools.CreateArcaneBloodrageToggle(
                    "BloodragerArcaneSpellGreaterDisplacementToggle",
                    Displacement,
                    BloodragerArcaneGreaterSpell,
                    BloodragerArcaneGreaterSpellDisplacementSwitchBuff,
                    AllBloodragerArcaneGreaterSpellSwitchBuffs);
                
                BlueprintAbility BloodragerArcaneSpellGreaterHasteToggle = BloodlineTools.CreateArcaneBloodrageToggle(
                    "BloodragerArcaneSpellGreaterHasteToggle",
                    Haste,
                    BloodragerArcaneGreaterSpell,
                    BloodragerArcaneGreaterSpellHasteSwitchBuff,
                    AllBloodragerArcaneGreaterSpellSwitchBuffs);

                BloodragerArcaneGreaterSpell.GetComponent<AbilityVariants>().m_Variants = new BlueprintAbilityReference[] {
                    BloodragerArcaneSpellGreaterDisplacementToggle.ToReference<BlueprintAbilityReference>(),
                    BloodragerArcaneSpellGreaterHasteToggle.ToReference<BlueprintAbilityReference>()
                };

                Main.LogPatch("Patched", BloodragerArcaneGreaterSpell);
            }

            static void PatchTrueArcaneBloodrage() {
                var BloodragerStandartRageBuff = Resources.GetBlueprint<BlueprintBuff>("5eac31e457999334b98f98b60fc73b2f");
                var BloodragerArcaneTrueSpellAbility = Resources.GetBlueprint<BlueprintAbility>("9d4d7f56d2d87f643b5ef990ef481094");

                var BeastShapeIVShamblingMoundAbility = Resources.GetBlueprint<BlueprintAbility>("b140c323981ba0a45a3bee5a1a57f493");
                var BeastShapeIVShamblingMoundBuff = Resources.GetBlueprint<BlueprintBuff>("50ab9c820eb9cf94d8efba3632ad5ce2");
                var BloodragerArcaneTrueSpellBeastShapIVShamblingMoundActivationBuff = CreateBloodragerTrueArcaneSpellRagePolymorphActivationBuff(
                    "BloodragerArcaneTrueSpellBeastShapIVShamblingMoundActivationBuff",
                    "True Arcane Bloodrage: Beast Shape IV (Shambling Mound)",
                    BeastShapeIVShamblingMoundBuff);

                var BeastShapeIVSmilodonAbility = Resources.GetBlueprint<BlueprintAbility>("502cd7fd8953ac74bb3a3df7e84818ae");
                var BeastShapeIVSmilodonBuff = Resources.GetBlueprint<BlueprintBuff>("c38def68f6ce13b4b8f5e5e0c6e68d08");
                var BloodragerArcaneTrueSpellBeastShapIVSmilodonActivationBuff = CreateBloodragerTrueArcaneSpellRagePolymorphActivationBuff(
                    "BloodragerArcaneTrueSpellBeastShapIVSmilodonActivationBuff",
                    "True Arcane Bloodrage: Beast Shape IV (Smilodon)",
                    BeastShapeIVSmilodonBuff);

                var BeastShapeIVWyvernAbility = Resources.GetBlueprint<BlueprintAbility>("3fa892e5e3efa364fb3d2692738a7c15");
                var BeastShapeIVWyvernBuff = Resources.GetBlueprint<BlueprintBuff>("dae2d173d9bd5b14dbeb4a1d9d9b0edc");
                var BloodragerArcaneTrueSpellBeastShapIVWyvernActivationBuff = CreateBloodragerTrueArcaneSpellRagePolymorphActivationBuff(
                    "BloodragerArcaneTrueSpellBeastShapIVWyvernActivationBuff",
                    "True Arcane Bloodrage: Beast Shape IV (Wyvern)",
                    BeastShapeIVWyvernBuff);

                var FormOfTheDragonIAbility = Resources.GetBlueprint<BlueprintAbility>("f767399367df54645ac620ef7b2062bb");

                var FormOfTheDragonIBlackAbility = Resources.GetBlueprint<BlueprintAbility>("baeb1c45b53de864ca0c10784ce447f0");
                var FormOfTheDragonIBlackBuff = Resources.GetBlueprint<BlueprintBuff>("268fafac0a5b78c42a58bd9c1ae78bcf");
                var BloodragerArcaneTrueSpellFormOfTheDragonIBlackActivationBuff = CreateBloodragerTrueArcaneSpellRagePolymorphActivationBuff(
                    "BloodragerArcaneTrueSpellFormOfTheDragonIBlackActivationBuff",
                    "True Arcane Bloodrage: Dragonkind I (Black)",
                    FormOfTheDragonIBlackBuff);

                var FormOfTheDragonIBlueAbility = Resources.GetBlueprint<BlueprintAbility>("7e889430ba65f724c81702101346e39a");
                var FormOfTheDragonIBlueBuff = Resources.GetBlueprint<BlueprintBuff>("b117bc8b41735924dba3fb23318f39ff");
                var BloodragerArcaneTrueSpellFormOfTheDragonIBlueActivationBuff = CreateBloodragerTrueArcaneSpellRagePolymorphActivationBuff(
                    "BloodragerArcaneTrueSpellFormOfTheDragonIBlueActivationBuff",
                    "True Arcane Bloodrage: Dragonkind I (Blue)",
                    FormOfTheDragonIBlueBuff);

                var FormOfTheDragonIBrassAbility = Resources.GetBlueprint<BlueprintAbility>("2271bc6960317164aa61363ebe7c0228");
                var FormOfTheDragonIBrassBuff = Resources.GetBlueprint<BlueprintBuff>("17d330af03f5b3042a4417ab1d45e484");
                var BloodragerArcaneTrueSpellFormOfTheDragonIBrassActivationBuff = CreateBloodragerTrueArcaneSpellRagePolymorphActivationBuff(
                    "BloodragerArcaneTrueSpellFormOfTheDragonIBrassActivationBuff",
                    "True Arcane Bloodrage: Dragonkind I (Brass)",
                    FormOfTheDragonIBrassBuff);

                var FormOfTheDragonIBronzeAbility = Resources.GetBlueprint<BlueprintAbility>("f1103c097be761e489ee27a8d49a373b");
                var FormOfTheDragonIBronzeBuff = Resources.GetBlueprint<BlueprintBuff>("1032d4ffb1c56444ca5bfce2c778614d");
                var BloodragerArcaneTrueSpellFormOfTheDragonIBronzeActivationBuff = CreateBloodragerTrueArcaneSpellRagePolymorphActivationBuff(
                    "BloodragerArcaneTrueSpellFormOfTheDragonIBronzeActivationBuff",
                    "True Arcane Bloodrage: Dragonkind I (Bronze)",
                    FormOfTheDragonIBronzeBuff);

                var FormOfTheDragonICopperAbility = Resources.GetBlueprint<BlueprintAbility>("7ecab895312f8b541a712f965ee7afdb");
                var FormOfTheDragonICopperBuff = Resources.GetBlueprint<BlueprintBuff>("a4cc7169fb7e64a4a8f53bdc774341b1");
                var BloodragerArcaneTrueSpellFormOfTheDragonICopperActivationBuff = CreateBloodragerTrueArcaneSpellRagePolymorphActivationBuff(
                    "BloodragerArcaneTrueSpellFormOfTheDragonICopperActivationBuff",
                    "True Arcane Bloodrage: Dragonkind I (Copper)",
                    FormOfTheDragonICopperBuff);

                var FormOfTheDragonIGoldAbility = Resources.GetBlueprint<BlueprintAbility>("12e6785ca0f97a145a7c02a5f0fd155c");
                var FormOfTheDragonIGoldBuff = Resources.GetBlueprint<BlueprintBuff>("89669cfba3d9c15448c23b79dd604c41");
                var BloodragerArcaneTrueSpellFormOfTheDragonIGoldActivationBuff = CreateBloodragerTrueArcaneSpellRagePolymorphActivationBuff(
                    "BloodragerArcaneTrueSpellFormOfTheDragonIGoldActivationBuff",
                    "True Arcane Bloodrage: Dragonkind I (Gold)",
                    FormOfTheDragonIGoldBuff);

                var FormOfTheDragonIGreenAbility = Resources.GetBlueprint<BlueprintAbility>("9d649b9e77bcd3d4ea0f91b8512a3744");
                var FormOfTheDragonIGreenBuff = Resources.GetBlueprint<BlueprintBuff>("02611a12f38bed340920d1d427865917");
                var BloodragerArcaneTrueSpellFormOfTheDragonIGreenActivationBuff = CreateBloodragerTrueArcaneSpellRagePolymorphActivationBuff(
                    "BloodragerArcaneTrueSpellFormOfTheDragonIGreenActivationBuff",
                    "True Arcane Bloodrage: Dragonkind I (Green)",
                    FormOfTheDragonIGreenBuff);

                var TransformationAbility = Resources.GetBlueprint<BlueprintAbility>("27203d62eb3d4184c9aced94f22e1806");
                var TransformationBuff = Resources.GetBlueprint<BlueprintBuff>("287682389d2011b41b5a65195d9cbc84");

                // SwitchBuffs
                BlueprintBuff BloodragerArcaneTrueSpellBeastShapeIVShamblingMoundSwitchBuff = BloodlineTools.CreateArcaneBloodrageSwitchBuff(
                    "BloodragerArcaneTrueSpellBeastShapeIVShamblingMoundSwitchBuff",
                    "True Arcane Bloodrage: Beast Shape IV (Shambling Mound)",
                    BloodragerArcaneTrueSpellAbility,
                    BloodragerStandartRageBuff,
                    BloodragerArcaneTrueSpellBeastShapIVShamblingMoundActivationBuff);

                BlueprintBuff BloodragerArcaneTrueSpellBeastShapeIVSmilodonSwitchBuff = BloodlineTools.CreateArcaneBloodrageSwitchBuff(
                    "BloodragerArcaneTrueSpellBeastShapeIVSmilodonSwitchBuff",
                    "True Arcane Bloodrage: Beast Shape IV (Smilodon)",
                    BloodragerArcaneTrueSpellAbility,
                    BloodragerStandartRageBuff,
                    BloodragerArcaneTrueSpellBeastShapIVSmilodonActivationBuff);

                BlueprintBuff BloodragerArcaneTrueSpellBeastShapeIVWyvernSwitchBuff = BloodlineTools.CreateArcaneBloodrageSwitchBuff(
                    "BloodragerArcaneTrueSpellBeastShapeIVWyvernSwitchBuff",
                    "True Arcane Bloodrage: Beast Shape IV (Wyvern)",
                    BloodragerArcaneTrueSpellAbility,
                    BloodragerStandartRageBuff,
                    BloodragerArcaneTrueSpellBeastShapIVWyvernActivationBuff);

                BlueprintBuff BloodragerArcaneTrueSpellFormOfTheDragonIBlackSwitchBuff = BloodlineTools.CreateArcaneBloodrageSwitchBuff(
                    "BloodragerArcaneTrueSpellFormOfTheDragonIBlackSwitchBuff",
                    "True Arcane Bloodrage: Dragonkind I (Black)",
                    BloodragerArcaneTrueSpellAbility,
                    BloodragerStandartRageBuff,
                    BloodragerArcaneTrueSpellFormOfTheDragonIBlackActivationBuff);

                BlueprintBuff BloodragerArcaneTrueSpellFormOfTheDragonIBlueSwitchBuff = BloodlineTools.CreateArcaneBloodrageSwitchBuff(
                    "BloodragerArcaneTrueSpellFormOfTheDragonIBlueSwitchBuff",
                    "True Arcane Bloodrage: Dragonkind I (Blue)",
                    BloodragerArcaneTrueSpellAbility,
                    BloodragerStandartRageBuff,
                    BloodragerArcaneTrueSpellFormOfTheDragonIBlueActivationBuff);

                BlueprintBuff BloodragerArcaneTrueSpellFormOfTheDragonIBrassSwitchBuff = BloodlineTools.CreateArcaneBloodrageSwitchBuff(
                    "BloodragerArcaneTrueSpellFormOfTheDragonIBrassSwitchBuff",
                    "True Arcane Bloodrage: Dragonkind I (Brass)",
                    BloodragerArcaneTrueSpellAbility,
                    BloodragerStandartRageBuff,
                    BloodragerArcaneTrueSpellFormOfTheDragonIBrassActivationBuff);

                BlueprintBuff BloodragerArcaneTrueSpellFormOfTheDragonIBronzeSwitchBuff = BloodlineTools.CreateArcaneBloodrageSwitchBuff(
                    "BloodragerArcaneTrueSpellFormOfTheDragonIBronzeSwitchBuff",
                    "True Arcane Bloodrage: Dragonkind I (Bronze)",
                    BloodragerArcaneTrueSpellAbility,
                    BloodragerStandartRageBuff,
                    BloodragerArcaneTrueSpellFormOfTheDragonIBronzeActivationBuff);

                BlueprintBuff BloodragerArcaneTrueSpellFormOfTheDragonICopperSwitchBuff = BloodlineTools.CreateArcaneBloodrageSwitchBuff(
                    "BloodragerArcaneTrueSpellFormOfTheDragonICopperSwitchBuff",
                    "True Arcane Bloodrage: Dragonkind I (Copper)",
                    BloodragerArcaneTrueSpellAbility,
                    BloodragerStandartRageBuff,
                    BloodragerArcaneTrueSpellFormOfTheDragonICopperActivationBuff);

                BlueprintBuff BloodragerArcaneTrueSpellFormOfTheDragonIGoldSwitchBuff = BloodlineTools.CreateArcaneBloodrageSwitchBuff(
                    "BloodragerArcaneTrueSpellFormOfTheDragonIGoldSwitchBuff",
                    "True Arcane Bloodrage: Dragonkind I (Gold)",
                    BloodragerArcaneTrueSpellAbility,
                    BloodragerStandartRageBuff,
                    BloodragerArcaneTrueSpellFormOfTheDragonIGoldActivationBuff);

                BlueprintBuff BloodragerArcaneTrueSpellFormOfTheDragonIGreenSwitchBuff = BloodlineTools.CreateArcaneBloodrageSwitchBuff(
                    "BloodragerArcaneTrueSpellFormOfTheDragonIGreenSwitchBuff",
                    "True Arcane Bloodrage: Dragonkind I (Green)",
                    BloodragerArcaneTrueSpellAbility,
                    BloodragerStandartRageBuff,
                    BloodragerArcaneTrueSpellFormOfTheDragonIGreenActivationBuff);

                BlueprintBuff BloodragerArcaneTrueSpellTransformationSwitchBuff = BloodlineTools.CreateArcaneBloodrageSwitchBuff(
                    "BloodragerArcaneTrueSpellTransformationSwitchBuff",
                    "True Arcane Bloodrage: Transformation",
                    BloodragerArcaneTrueSpellAbility,
                    BloodragerStandartRageBuff,
                    TransformationBuff);

                var AllBloodragerArcaneTrueSpellSwitchBuffs = new List<BlueprintBuff>() {
                    BloodragerArcaneTrueSpellBeastShapeIVShamblingMoundSwitchBuff,
                    BloodragerArcaneTrueSpellBeastShapeIVSmilodonSwitchBuff,
                    BloodragerArcaneTrueSpellBeastShapeIVWyvernSwitchBuff,
                    BloodragerArcaneTrueSpellFormOfTheDragonIBlackSwitchBuff,
                    BloodragerArcaneTrueSpellFormOfTheDragonIBlueSwitchBuff,
                    BloodragerArcaneTrueSpellFormOfTheDragonIBrassSwitchBuff,
                    BloodragerArcaneTrueSpellFormOfTheDragonIBronzeSwitchBuff,
                    BloodragerArcaneTrueSpellFormOfTheDragonICopperSwitchBuff,
                    BloodragerArcaneTrueSpellFormOfTheDragonIGoldSwitchBuff,
                    BloodragerArcaneTrueSpellFormOfTheDragonIGreenSwitchBuff,
                    BloodragerArcaneTrueSpellTransformationSwitchBuff
                };

                // Toggles
                BlueprintAbility BloodragerArcaneSpellTrueBeastShapeIVShamblingMoundToggle = BloodlineTools.CreateArcaneBloodrageToggle(
                    "BloodragerArcaneSpellTrueBeastShapeIVShamblingMoundToggle",
                    BeastShapeIVShamblingMoundAbility,
                    BloodragerArcaneTrueSpellAbility,
                    BloodragerArcaneTrueSpellBeastShapeIVShamblingMoundSwitchBuff,
                    AllBloodragerArcaneTrueSpellSwitchBuffs);

                BlueprintAbility BloodragerArcaneSpellTrueBeastShapeIVSmilodonToggle = BloodlineTools.CreateArcaneBloodrageToggle(
                    "BloodragerArcaneSpellTrueBeastShapeIVSmilodonToggle",
                    BeastShapeIVSmilodonAbility,
                    BloodragerArcaneTrueSpellAbility,
                    BloodragerArcaneTrueSpellBeastShapeIVSmilodonSwitchBuff,
                    AllBloodragerArcaneTrueSpellSwitchBuffs);

                BlueprintAbility BloodragerArcaneSpellTrueBeastShapeIVWyvernToggle = BloodlineTools.CreateArcaneBloodrageToggle(
                    "BloodragerArcaneSpellTrueBeastShapeIVWyvernToggle",
                    BeastShapeIVWyvernAbility,
                    BloodragerArcaneTrueSpellAbility,
                    BloodragerArcaneTrueSpellBeastShapeIVWyvernSwitchBuff,
                    AllBloodragerArcaneTrueSpellSwitchBuffs);

                BlueprintAbility BloodragerArcaneSpellTrueFormOfTheDragonIBlackToggle = BloodlineTools.CreateArcaneBloodrageToggle(
                    "BloodragerArcaneSpellTrueFormOfTheDragonIBlackToggle",
                    FormOfTheDragonIBlackAbility,
                    BloodragerArcaneTrueSpellAbility,
                    BloodragerArcaneTrueSpellFormOfTheDragonIBlackSwitchBuff,
                    AllBloodragerArcaneTrueSpellSwitchBuffs);

                BlueprintAbility BloodragerArcaneSpellTrueFormOfTheDragonIBlueToggle = BloodlineTools.CreateArcaneBloodrageToggle(
                    "BloodragerArcaneSpellTrueFormOfTheDragonIBlueToggle",
                    FormOfTheDragonIBlueAbility,
                    BloodragerArcaneTrueSpellAbility,
                    BloodragerArcaneTrueSpellFormOfTheDragonIBlueSwitchBuff,
                    AllBloodragerArcaneTrueSpellSwitchBuffs);

                BlueprintAbility BloodragerArcaneSpellTrueFormOfTheDragonIBrassToggle = BloodlineTools.CreateArcaneBloodrageToggle(
                    "BloodragerArcaneSpellTrueFormOfTheDragonIBrassToggle",
                    FormOfTheDragonIBrassAbility,
                    BloodragerArcaneTrueSpellAbility,
                    BloodragerArcaneTrueSpellFormOfTheDragonIBrassSwitchBuff,
                    AllBloodragerArcaneTrueSpellSwitchBuffs);

                BlueprintAbility BloodragerArcaneSpellTrueFormOfTheDragonIBronzeToggle = BloodlineTools.CreateArcaneBloodrageToggle(
                    "BloodragerArcaneSpellTrueFormOfTheDragonIBronzeToggle",
                    FormOfTheDragonIBronzeAbility,
                    BloodragerArcaneTrueSpellAbility,
                    BloodragerArcaneTrueSpellFormOfTheDragonIBronzeSwitchBuff,
                    AllBloodragerArcaneTrueSpellSwitchBuffs);

                BlueprintAbility BloodragerArcaneSpellTrueFormOfTheDragonICopperToggle = BloodlineTools.CreateArcaneBloodrageToggle(
                    "BloodragerArcaneSpellTrueFormOfTheDragonICopperToggle",
                    FormOfTheDragonICopperAbility,
                    BloodragerArcaneTrueSpellAbility,
                    BloodragerArcaneTrueSpellFormOfTheDragonICopperSwitchBuff,
                    AllBloodragerArcaneTrueSpellSwitchBuffs);

                BlueprintAbility BloodragerArcaneSpellTrueFormOfTheDragonIGoldToggle = BloodlineTools.CreateArcaneBloodrageToggle(
                    "BloodragerArcaneSpellTrueFormOfTheDragonIGoldToggle",
                    FormOfTheDragonIGoldAbility,
                    BloodragerArcaneTrueSpellAbility,
                    BloodragerArcaneTrueSpellFormOfTheDragonIGoldSwitchBuff,
                    AllBloodragerArcaneTrueSpellSwitchBuffs);

                BlueprintAbility BloodragerArcaneSpellTrueFormOfTheDragonIGreenToggle = BloodlineTools.CreateArcaneBloodrageToggle(
                    "BloodragerArcaneSpellTrueFormOfTheDragonIGreenToggle",
                    FormOfTheDragonIGreenAbility,
                    BloodragerArcaneTrueSpellAbility,
                    BloodragerArcaneTrueSpellFormOfTheDragonIGreenSwitchBuff,
                    AllBloodragerArcaneTrueSpellSwitchBuffs);

                BlueprintAbility BloodragerArcaneSpellTrueTransformationToggle = BloodlineTools.CreateArcaneBloodrageToggle(
                    "BloodragerArcaneSpellTrueTransformationToggle",
                    TransformationAbility,
                    BloodragerArcaneTrueSpellAbility,
                    BloodragerArcaneTrueSpellTransformationSwitchBuff,
                    AllBloodragerArcaneTrueSpellSwitchBuffs);

                BloodragerArcaneTrueSpellAbility.GetComponent<AbilityVariants>().m_Variants = new BlueprintAbilityReference[] {
                    BloodragerArcaneSpellTrueBeastShapeIVShamblingMoundToggle.ToReference<BlueprintAbilityReference>(),
                    BloodragerArcaneSpellTrueBeastShapeIVSmilodonToggle.ToReference<BlueprintAbilityReference>(),
                    BloodragerArcaneSpellTrueBeastShapeIVWyvernToggle.ToReference<BlueprintAbilityReference>(),
                    BloodragerArcaneSpellTrueFormOfTheDragonIBlackToggle.ToReference<BlueprintAbilityReference>(),
                    BloodragerArcaneSpellTrueFormOfTheDragonIBlueToggle.ToReference<BlueprintAbilityReference>(),
                    BloodragerArcaneSpellTrueFormOfTheDragonIBrassToggle.ToReference<BlueprintAbilityReference>(),
                    BloodragerArcaneSpellTrueFormOfTheDragonIBronzeToggle.ToReference<BlueprintAbilityReference>(),
                    BloodragerArcaneSpellTrueFormOfTheDragonICopperToggle.ToReference<BlueprintAbilityReference>(),
                    BloodragerArcaneSpellTrueFormOfTheDragonIGoldToggle.ToReference<BlueprintAbilityReference>(),
                    BloodragerArcaneSpellTrueFormOfTheDragonIGreenToggle.ToReference<BlueprintAbilityReference>(),
                    BloodragerArcaneSpellTrueTransformationToggle.ToReference<BlueprintAbilityReference>(),
                };

                Main.LogPatch("Patched", BloodragerArcaneTrueSpellAbility);
            }

            static BlueprintBuff CreateBloodragerTrueArcaneSpellRagePolymorphActivationBuff(
                string blueprintName,
                string displayName,
                BlueprintBuff polymorphBuff) {
                return Helpers.CreateBuff(blueprintName, bp => {
                    bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                    bp.IsClassFeature = true;
                    bp.SetName(displayName);
                    bp.m_Description = polymorphBuff.m_Description;
                    bp.AddComponent<AddFactContextActions>(c => {
                        c.Activated = new ActionList() {
                            Actions = new GameAction[] {
                                new ContextActionRemoveBuffsByDescriptor() {
                                    NotSelf = true,
                                    SpellDescriptor = SpellDescriptor.Polymorph
                                },
                                new ContextActionApplyBuff() {
                                    m_Buff = polymorphBuff.ToReference<BlueprintBuffReference>(),
                                    Permanent = true,
                                    AsChild = true,
                                    DurationValue = new ContextDurationValue(),
                                    IsFromSpell = false
                                }
                            }
                        };
                    });
                });
            }
            
        }
    }
}
