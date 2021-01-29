using Volo.Abp.Dapper;
using Volo.Abp.Modularity;

namespace MyCompanyName.MyProjectName
{
    [DependsOn(typeof(AbpDapperModule))]
    public class MyProjectNameDapperModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
        }
    }
}