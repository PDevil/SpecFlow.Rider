using JetBrains.ProjectModel;
using JetBrains.ReSharper.Features.ReSpeller.Analyzers;
using JetBrains.ReSharper.Features.ReSpeller.Analyzers.HighlightingGenerators;
using JetBrains.ReSharper.Features.ReSpeller.Analyzers.Scopes;
using JetBrains.ReSharper.Features.ReSpeller.Daemon;
using JetBrains.ReSharper.Features.ReSpeller.Daemon.TextExtraction;
using ReSharperPlugin.SpecflowRiderPlugin.Psi;

namespace ReSharperPlugin.SpecflowRiderPlugin.Daemon.SpellCheck;

[SolutionComponent]
public class GherkinTokenTextExtractor : ElementTextExtractor<GherkinToken>
{
    public GherkinTokenTextExtractor(
        ISpellingAndGrammarDataBuilder reReSpellerDataBuilder,
        IRequiredSpellCheckingModesProvider requiredRequiredSpellCheckingModesProvider,
        GrammarAndSpellingMeasurements measurements
    ) : base(reReSpellerDataBuilder, requiredRequiredSpellCheckingModesProvider, measurements)
    {
    }

    public override bool Extract(GherkinToken node, ElementTextExtractorContext context)
    {
        context.Collector.EnrichWithReSpellerData(
            SpellingAndGrammarDataBuilder, context.File, node, SpellCheckingMode.Orthography, CheckingContext.Comment,
            SparseTextToCheck.FromNode(node)
        );
        return true;
    }
}