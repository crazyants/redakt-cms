using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Redakt.Data.Repository;
using Redakt.Model;

namespace Redakt.Data.Mongo.Repository
{
    public class FieldTypeRepository : Repository<FieldType>, IFieldTypeRepository
    {
        public FieldTypeRepository(IConnection connection)
            : base(connection)
        {
        }
    }
}
