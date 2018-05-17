using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace MyAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class MyAnalyzerAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "MyAnalyzer";

        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.AnalyzerTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.AnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.AnalyzerDescription), Resources.ResourceManager, typeof(Resources));
        private const string Category = "Naming";

        private static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSymbolAction(FindController, SymbolKind.NamedType);
        }

        private static void FindController(SymbolAnalysisContext context)
        {
            var namedTypeSymbol = (INamedTypeSymbol)context.Symbol;
            var baseController = context.Compilation.GetTypeByMetadataName("System.Web.Mvc.Controller");

            var baseTypes = GetBaseClasses(namedTypeSymbol);

            if (baseTypes.Contains(baseController))
                if (!namedTypeSymbol.Name.EndsWith("Controller")) {
                    var diagnostic = Diagnostic.Create(Rule, namedTypeSymbol.Locations[0], namedTypeSymbol.Name);
                    context.ReportDiagnostic(diagnostic);
                }
        }

        private static ImmutableArray<INamedTypeSymbol> GetBaseClasses(INamedTypeSymbol type)
        {
            if (type == null || type.TypeKind == TypeKind.Error)
                return ImmutableArray<INamedTypeSymbol>.Empty;

            if (type.BaseType != null && type.BaseType.TypeKind != TypeKind.Error)
                return GetBaseClasses(type.BaseType).Add(type.BaseType);

            return ImmutableArray<INamedTypeSymbol>.Empty;
        }
    }
}
