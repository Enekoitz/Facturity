using ArchUnitNET.xUnit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace Facturity.ArchitectureTests;

[Trait("Category", "Architecture")]
public class LayerTests : ArchitectureTestBase
{
    [Fact]
    public void Domain_no_depende_de_Infrastructure_ni_de_Features() =>
        Types().That().Are(DomainLayer)
            .Should().NotDependOnAny(InfrastructureLayer).AndShould().NotDependOnAny(FeaturesLayer)
            .Check(Architecture);

    [Fact]
    public void Infrastructure_no_depende_de_Features() =>
        Types().That().Are(InfrastructureLayer)
            .Should().NotDependOnAny(FeaturesLayer)
            .Check(Architecture);
}