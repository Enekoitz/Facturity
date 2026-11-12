using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using Facturity.Modules.Auth;
using Facturity.Modules.Customer;
using Facturity.Modules.Invoicing;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace Facturity.ArchitectureTests;

public abstract class ArchitectureTestBase
{
    private const string Modulo = @"^Facturity\.Modules\.[^.]+";

    protected static readonly Architecture Architecture = new ArchLoader()
        .LoadAssemblies(
            typeof(AuthModule).Assembly,
            typeof(CustomerModule).Assembly,
            typeof(InvoicingModule).Assembly,
            typeof(Program).Assembly)
        .Build();

    protected static readonly IObjectProvider<IType> DomainLayer =
        Types().That().ResideInNamespace(Modulo + @"\.Domain(\.|$)").As("Domain");

    protected static readonly IObjectProvider<IType> FeaturesLayer =
        Types().That().ResideInNamespace(Modulo + @"\.Features(\.|$)").As("Features");
    
    protected static readonly IObjectProvider<IType> InfrastructureLayer =
        Types().That().ResideInNamespace(Modulo + @"\.Infrastructure(\.|$)").As("Infrastructure");
}