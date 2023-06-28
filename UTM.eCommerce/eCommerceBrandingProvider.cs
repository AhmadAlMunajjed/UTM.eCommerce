using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace UTM.eCommerce;

[Dependency(ReplaceServices = true)]
public class eCommerceBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "eCommerce";
}
