using System;
using System.Collections.Generic;
using System.Linq;
using Redakt.Core.Cache;
using Redakt.Data.Repository;
using Redakt.Model;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyModel;
using System.Reflection;

namespace Redakt.Core.Services
{
    public interface IFieldTypeService
    {
        Task<FieldType> Get(string id);
        Task<IList<FieldType>> GetAll();
        Task Save(FieldType fieldType);
        IList<IFieldEditor> GetAllFieldEditors();
    }

    public class FieldTypeService : IFieldTypeService
    {
        private readonly ICache _cache;
        private readonly IFieldTypeRepository _fieldTypeRepository;
        private static List<IFieldEditor> _fieldEditors;

        public FieldTypeService(IFieldTypeRepository fieldTypeRepository, ICache cache)
        {
            _fieldTypeRepository = fieldTypeRepository;
            _cache = cache;
        }

        public Task<FieldType> Get(string id)
        {
            return _cache.AddOrGetExistingAsync(id, s => _fieldTypeRepository.GetAsync(s));
        }

        public Task<IList<FieldType>> GetAll()
        {
            return _fieldTypeRepository.FindAsync(s => true);
        }

        public Task Save(FieldType fieldType)
        {
            return _fieldTypeRepository.SaveAsync(fieldType);
        }

        public IList<IFieldEditor> GetAllFieldEditors()
        {
            if (_fieldEditors != null) return _fieldEditors;

            var type = typeof(IFieldEditor);
            _fieldEditors = new List<IFieldEditor>();
            foreach (var library in DependencyContext.Default.RuntimeLibraries)
            {
                if (library.Name.StartsWith("Microsoft.") || library.Name.StartsWith("System.")) continue;

                try
                {
                    var assembly = Assembly.Load(new AssemblyName(library.Name));
                    foreach (var assemblyType in assembly.GetTypes())
                    {
                        if (type.IsAssignableFrom(assemblyType)) _fieldEditors.Add(Activator.CreateInstance(assemblyType) as IFieldEditor);
                    }
                }
                catch
                {
                    // Could not load assembly
                }
            }

            return _fieldEditors;
        }
    }
}
