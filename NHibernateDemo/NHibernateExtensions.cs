using Microsoft.Extensions.DependencyInjection;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Dialect;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;

public static class NHibernateExtensions
{
    public static IServiceCollection AddNHibernate(this IServiceCollection services, string connectionString)
    {
        // set serilog as default logger for nhibernate
        NHibernateLogger.SetLoggersFactory(new NHibernate.Logging.Serilog.SerilogLoggerFactory());

        // standard config for nhibernate(could have been done in appsettings.json
        var mapper = new ModelMapper();
        mapper.AddMappings(typeof(NHibernateExtensions).Assembly.ExportedTypes);
        HbmMapping domainMapping = mapper.CompileMappingForAllExplicitlyAddedEntities();

        var configuration = new Configuration();

        configuration.DataBaseIntegration(c =>
        {
            c.Dialect<MsSql2012Dialect>();
            c.ConnectionString = connectionString;
            c.KeywordsAutoImport = Hbm2DDLKeyWords.AutoQuote;
            //c.SchemaAction = SchemaAutoAction.Validate;
            c.LogFormattedSql = true;
            c.LogSqlInConsole = true;
        });
        configuration.AddMapping(domainMapping);

        var sessionFactory = configuration.BuildSessionFactory();

        // recreate db each time
        SchemaExport se = new SchemaExport(configuration);
        se.Drop(true, true);
        se.Create(true, true);

        services.AddSingleton(sessionFactory);
        services.AddScoped(factory => sessionFactory.OpenSession());

        return services;
    }
}