﻿using ModKit.Utility;
using SolastaCommunityExpansion.Api;
using UnityEngine.AddressableAssets;

namespace SolastaCommunityExpansion.Models;

internal static class RemoveBugVisualModelsContext
{
    internal static void Load()
    {
        if (Main.Settings.RemoveBugVisualModels)
        {
            // Spiderlings, fire spider, kindred spirit spider, BadlandsSpider(normal, conjured and wildshaped versions)
            const string assetReference_spider_1 = "362fc51df586d254ab182ef854396f82";
            //CrimsonSpiderling, PhaseSpider, SpectralSpider, CrimsonSpider, deep spider(normal, conjured and wildshaped versions)
            const string assetReference_spider_2 = "40b5fe532a9a0814097acdb16c74e967";
            // spider queen
            const string assetReference_spider_3 = "8fc96b2a8c5fcc243b124d31c63df5d9";
            //Giant_Beetle, Small_Beetle, Redeemer_Zealot, Redeemer_Pilgrim
            const string assetReference_beetle = "04dfcec8c8afb8642a80c1116de218d4";
            //Young_Remorhaz, Remorhaz
            const string assetReference_Remorhaz = "ded896e0c4ef46144904375ecadb1bb1";

            /*
             * Can this be removed?
            // list of targeted prefab guid references (shared between at least 20 monsters)
            List<string> listofBugstrings = new List<string>
            {
            assetReference_spider_1,
            assetReference_spider_2,
            assetReference_spider_3,
            assetReference_beetle,
            assetReference_Remorhaz
            };
            */

            // replacement monster and model references

            var brownBear = DatabaseHelper.MonsterDefinitions.BrownBear;
            var bearPrefab = new AssetReference("cc36634f504fa7049a4499a91749d7d5");

            var wolf = DatabaseHelper.MonsterDefinitions.Wolf;
            var wolfPrefab = new AssetReference("6e02c9bcfb5122042a533e7732182b1d");

            var ape = DatabaseHelper.MonsterDefinitions.Ape_MonsterDefinition;
            var apePrefab = new AssetReference("8f4589a9a294b444785fab045256a713");

            var listofAllMonsters = DatabaseRepository.GetDatabase<MonsterDefinition>().GetAllElements();

            // check every monster for targeted prefab guid references
            foreach (var monster in listofAllMonsters)
            {
                // get monster asset reference for prefab guid comparison
                var value =
                    monster.MonsterPresentation.GetFieldValue<MonsterPresentation, AssetReference>(
                        "malePrefabReference");

                // swap bears for spiders
                if (value.AssetGUID == assetReference_spider_1 || value.AssetGUID == assetReference_spider_2 ||
                    value.AssetGUID == assetReference_spider_3)
                {
                    monster.MonsterPresentation.malePrefabReference = bearPrefab;
                    monster.MonsterPresentation.femalePrefabReference = bearPrefab;
                    monster.GuiPresentation.spriteReference = brownBear.GuiPresentation.SpriteReference;
                    monster.bestiarySpriteReference = brownBear.BestiarySpriteReference;
                    monster.MonsterPresentation.monsterPresentationDefinitions = brownBear.MonsterPresentation
                        .MonsterPresentationDefinitions;
                }

                // swap apes for remorhaz
                if (value.AssetGUID == assetReference_Remorhaz)
                {
                    monster.MonsterPresentation.malePrefabReference = apePrefab;
                    monster.MonsterPresentation.femalePrefabReference = apePrefab;
                    monster.GuiPresentation.spriteReference = ape.GuiPresentation.SpriteReference;
                    monster.bestiarySpriteReference = ape.BestiarySpriteReference;
                    monster.MonsterPresentation.monsterPresentationDefinitions = ape.MonsterPresentation
                        .MonsterPresentationDefinitions;
                }

                // swap wolves for beetles
                if (value.AssetGUID == assetReference_beetle)
                {
                    monster.MonsterPresentation.malePrefabReference = wolfPrefab;
                    monster.MonsterPresentation.femalePrefabReference = wolfPrefab;
                    monster.GuiPresentation.spriteReference = wolf.GuiPresentation.SpriteReference;
                    monster.bestiarySpriteReference = wolf.BestiarySpriteReference;
                    monster.MonsterPresentation.monsterPresentationDefinitions = wolf.MonsterPresentation
                        .MonsterPresentationDefinitions;

                    // changing beetle scale to suit replacement model
                    monster.MonsterPresentation.maleModelScale = 0.655f;
                    monster.MonsterPresentation.femaleModelScale = 0.655f;
                }
            }
        }
    }
}
