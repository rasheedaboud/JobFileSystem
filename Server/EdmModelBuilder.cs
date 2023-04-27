using JobFileSystem.Shared.Contacts;
using JobFileSystem.Shared.DTOs;
using JobFileSystem.Shared.Estimates;
using JobFileSystem.Shared.Interfaces;
using JobFileSystem.Shared.JobFiles;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace NdtTracking.Wasm.Server
{
    public class ModelBuilder
    {
        private readonly ODataConventionModelBuilder _builder;
        public IEdmModel Build() => _builder.GetEdmModel();

        private ModelBuilder()
        {
            _builder ??= new ODataConventionModelBuilder();
        }

        public static ModelBuilder New() => new ModelBuilder();

        public ModelBuilder AddModel<T>() where T : class, IId
        {
            var type = typeof(T).Name;
            var name = $"{type.Split("D")[0]}sOdata";
            if (_builder is null) new ModelBuilder();
            var result = _builder.EntitySet<T>(name).EntityType;
            _builder.EntityType<T>().HasKey(_ => _.Id);

            _builder.EntityType<T>().Property(_ => _.Id).IsNullable();
            return this;
        }
        public ModelBuilder AddComplex<T>() where T : class
        {
            if (_builder is null) new ModelBuilder();
            var result = _builder.ComplexType<T>();
            return this;
        }

    }
    public static class EdmModel
    {
        public static IEdmModel BuildEdmModel()
        {
            return ModelBuilder.New()
                               .AddModel<JobFileDto>()
                               .AddModel<ContactDto>()
                               .AddModel<EstimateDto>()
                               .AddModel<MaterialTestReportDto>()
                               .Build();
        }
    }
}
