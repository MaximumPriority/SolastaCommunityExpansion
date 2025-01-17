﻿using SolastaCommunityExpansion.Builders.Features;
using static SolastaCommunityExpansion.Api.DatabaseHelper.FeatureDefinitionSenses;

namespace SolastaCommunityExpansion.Level20.Features;

internal sealed class
    ProficiencyRogueBlindSenseBuilder : FeatureDefinitionBuilder<FeatureDefinitionSense,
        ProficiencyRogueBlindSenseBuilder>
{
    private const string ProficiencyRogueBlindSenseName = "ZSProficiencyRogueBlindSense";
    private const string ProficiencyRogueBlindSensedGuid = "30c27691f42f4705985c638d38fadc21";

    internal static readonly FeatureDefinitionSense ProficiencyRogueBlindSense
        = CreateAndAddToDB(ProficiencyRogueBlindSenseName, ProficiencyRogueBlindSensedGuid);

    private ProficiencyRogueBlindSenseBuilder(string name, string guid) : base(SenseBlindSight2, name, guid)
    {
        Definition.GuiPresentation.Title = "Feature/&ProficiencyRogueBlindSenseTitle";
        Definition.GuiPresentation.Description = "Feature/&ProficiencyRogueBlindSenseDescription";
        Definition.GuiPresentation.hidden = false;
    }

    private static FeatureDefinitionSense CreateAndAddToDB(string name, string guid)
    {
        return new ProficiencyRogueBlindSenseBuilder(name, guid).AddToDB();
    }
}
