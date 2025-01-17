﻿using System.Diagnostics.CodeAnalysis;
using HarmonyLib;
using SolastaCommunityExpansion.Models;

namespace SolastaCommunityExpansion.Patches.GameUi.CharacterInspection;

[HarmonyPatch(typeof(CharacterPlateDetailed), "OnPortraitShowed")]
[SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase", Justification = "Patch")]
internal static class CharacterPlateDetailed_OnPortraitShowed
{
    internal static void Postfix(CharacterPlateDetailed __instance)
    {
        int classesCount;
        char separator;
        var guiCharacter = __instance.GuiCharacter;

        if (guiCharacter.RulesetCharacterHero != null)
        {
            separator = '\n';
            classesCount = guiCharacter.RulesetCharacterHero.ClassesAndLevels.Count;
        }
        else
        {
            separator = '\\';
            classesCount = guiCharacter.Snapshot.Classes.Length;
        }

        __instance.classLabel.Text = MulticlassGameUiContext.GetAllClassesLabel(guiCharacter, separator) ??
                                     __instance.classLabel.Text;
        __instance.classLabel.TMP_Text.fontSize = MulticlassGameUiContext.GetFontSize(classesCount);
    }
}
