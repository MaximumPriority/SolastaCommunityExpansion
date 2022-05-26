﻿using System.Collections.Generic;
using SolastaCommunityExpansion.Builders;
using SolastaCommunityExpansion.Builders.Features;
using SolastaCommunityExpansion.CustomDefinitions;
using SolastaCommunityExpansion.Models;
using SolastaModApi.Extensions;
using static RuleDefinitions;
using static SolastaModApi.DatabaseHelper;

namespace SolastaCommunityExpansion.Classes.Monk.Subclasses
{
    public static class ZenArcher
    {
        private const string ZenArrowTag = "ZenArrow";

        // Zen Archer's Monk weapons are all ranged weapons.
        private static readonly List<WeaponTypeDefinition> MonkWeapons = new()
        {
            WeaponTypeDefinitions.ShortbowType,
            WeaponTypeDefinitions.LongbowType,
            WeaponTypeDefinitions.DartType,
        };

        public static CharacterSubclassDefinition Build()
        {
            return CharacterSubclassDefinitionBuilder
                .Create("ClassMonkTraditionZenArcher", DefinitionBuilder.CENamespaceGuid)
                .SetOrUpdateGuiPresentation(Category.Subclass,
                    CharacterSubclassDefinitions.RangerMarksman.GuiPresentation.SpriteReference)
                .AddFeaturesAtLevel(3, BuildLevel03Features())
                .AddFeaturesAtLevel(6, BuildLevel06Features())
                .AddFeaturesAtLevel(11, BuildLevel11Features())
                .AddFeaturesAtLevel(17, BuildLevel17Features())
                .AddToDB();
        }

        public static bool IsMonkWeapon(RulesetCharacter character, ItemDefinition item)
        {
            if (character == null || item == null)
            {
                return false;
            }

            var typeDefinition = item.WeaponDescription?.WeaponTypeDefinition;

            if (typeDefinition == null)
            {
                return false;
            }

            return character.HasSubFeatureOfType<ZenArcherMarker>()
                   && MonkWeapons.Contains(typeDefinition);
        }

        private static FeatureDefinition[] BuildLevel03Features()
        {
            return new[]
            {
                FeatureDefinitionProficiencyBuilder
                    .Create("ClassMonkZenArcherCombat", Monk.GUID)
                    .SetGuiPresentation(Category.Feature)
                    .SetProficiencies(ProficiencyType.Weapon,
                        WeaponTypeDefinitions.LongbowType.Name,
                        WeaponTypeDefinitions.ShortbowType.Name)
                    .SetCustomSubFeatures(
                        new ZenArcherMarker(),
                        //TODO: add feature that doubles range of ranged monk attacks, but not more than to 16/32 cells
                        //TODO: add bonus 1-handed thrown/ranged attack
                        RangedAttackInMeleeDisadvantageRemover.Marker, //TODO: move to level 06 features
                        new AddTagToWeaponAttack(ZenArrowTag, IsZenArrowAttack)
                    )
                    .AddToDB(),
                BuildZenArrow()
            };
        }

        private static FeatureDefinition BuildZenArrow()
        {
           var technique = FeatureDefinitionPowerBuilder
                .Create("ClassMonkZenArrowTechnique", Monk.GUID)
                .SetGuiPresentation(Category.Power)
                .SetActivationTime(ActivationTime.OnAttackHit)
                .SetRechargeRate(RechargeRate.AtWill)
                .SetCostPerUse(0)
                .SetCustomSubFeatures(new ReactionAttackModeRestriction(
                    (mode, _, _) => mode.AttackTags.Contains(ZenArrowTag)
                ))
                .AddToDB();

            var prone = FeatureDefinitionPowerSharedPoolBuilder
                .Create("ClassMonkZenArrowProne", Monk.GUID)
                .SetGuiPresentation(Category.Power)
                .SetSharedPool(technique)
                .SetActivationTime(ActivationTime.NoCost)
                .SetEffectDescription(new EffectDescriptionBuilder()
                    .SetDurationData(DurationType.Round, 1)
                    .SetTargetingData(Side.Enemy, RangeType.Touch, 1, TargetType.Individuals)
                    .SetTargetFiltering(TargetFilteringMethod.CharacterOnly)
                    .SetSavingThrowData(
                        true,
                        true,
                        AttributeDefinitions.Dexterity,
                        true,
                        EffectDifficultyClassComputation.AbilityScoreAndProficiency,
                        AttributeDefinitions.Wisdom
                    )
                    .SetEffectForms(new EffectFormBuilder()
                        .HasSavingThrow(EffectSavingThrowType.Negates)
                        .SetLevelAdvancement(EffectForm.LevelApplianceType.No, LevelSourceType.ClassLevel)
                        .SetMotionForm(MotionForm.MotionType.FallProne, 0)
                        .Build())
                    .Build())
                .AddToDB();

            var push = FeatureDefinitionPowerSharedPoolBuilder
                .Create("ClassMonkZenArrowPush", Monk.GUID)
                .SetGuiPresentation(Category.Power)
                .SetSharedPool(technique)
                .SetActivationTime(ActivationTime.NoCost)
                .SetEffectDescription(new EffectDescriptionBuilder()
                    .SetDurationData(DurationType.Round, 1)
                    .SetTargetingData(Side.Enemy, RangeType.Touch, 1, TargetType.Individuals)
                    .SetTargetFiltering(TargetFilteringMethod.CharacterOnly)
                    .SetSavingThrowData(
                        true,
                        true,
                        AttributeDefinitions.Strength,
                        true,
                        EffectDifficultyClassComputation.AbilityScoreAndProficiency,
                        AttributeDefinitions.Wisdom
                    )
                    .SetEffectForms(new EffectFormBuilder()
                        .HasSavingThrow(EffectSavingThrowType.Negates)
                        .SetLevelAdvancement(EffectForm.LevelApplianceType.No, LevelSourceType.ClassLevel)
                        .SetMotionForm(MotionForm.MotionType.PushFromOrigin, 2)
                        .Build())
                    .Build())
                .AddToDB();

            var distract = FeatureDefinitionPowerSharedPoolBuilder
                .Create("ClassMonkZenArrowDistract", Monk.GUID)
                .SetGuiPresentation(Category.Power)
                .SetSharedPool(technique)
                .SetActivationTime(ActivationTime.NoCost)
                .SetEffectDescription(new EffectDescriptionBuilder()
                    .SetDurationData(DurationType.Round, 1)
                    .SetTargetingData(Side.Enemy, RangeType.Touch, 1, TargetType.Individuals)
                    .SetTargetFiltering(TargetFilteringMethod.CharacterOnly)
                    .SetSavingThrowData(
                        true,
                        true,
                        AttributeDefinitions.Wisdom,
                        true,
                        EffectDifficultyClassComputation.AbilityScoreAndProficiency,
                        AttributeDefinitions.Wisdom
                    )
                    .SetEffectForms(new EffectFormBuilder()
                        .HasSavingThrow(EffectSavingThrowType.None)
                        .SetLevelAdvancement(EffectForm.LevelApplianceType.No, LevelSourceType.ClassLevel)
                        .HasSavingThrow(EffectSavingThrowType.Negates)
                        .SetConditionForm(ConditionDefinitionBuilder
                            .Create("ClassMonkZenArrowDistractCondition", Monk.GUID)
                            .SetGuiPresentation(Category.Condition,
                                ConditionDefinitions.ConditionDazzled.GuiPresentation.SpriteReference)
                            .SetDuration(DurationType.Round, 1)
                            .SetTurnOccurence(TurnOccurenceType.EndOfTurn)
                            .SetConditionType(ConditionType.Detrimental)
                            .SetSpecialInterruptions(ConditionInterruption.Attacks)
                            .SetFeatures(FeatureDefinitionCombatAffinityBuilder
                                .Create("ClassMonkZenArrowDistractFeature", Monk.GUID)
                                .SetGuiPresentationNoContent(true)
                                .SetMyAttackAdvantage(AdvantageType.Disadvantage)
                                .AddToDB())
                            .AddToDB(), ConditionForm.ConditionOperation.Add)
                        .Build())
                    .Build())
                .AddToDB();

            PowerBundleContext.RegisterPowerBundle(technique, true, prone, push, distract);

            return technique;
        }

        private static FeatureDefinition[] BuildLevel06Features()
        {
            return System.Array.Empty<FeatureDefinition>();
        }

        private static FeatureDefinition[] BuildLevel11Features()
        {
            return System.Array.Empty<FeatureDefinition>();
        }

        private static FeatureDefinition[] BuildLevel17Features()
        {
            return System.Array.Empty<FeatureDefinition>();
        }
        
        private static bool IsZenArrowAttack(RulesetAttackMode mode, RulesetItem weapon, RulesetCharacter character)
        {
            return mode is { Reach: false, Magical: false, Ranged: true } && Monk.IsMonkWeapon(character, mode);
        }

        private class ZenArcherMarker
        {
        }
    }
}
