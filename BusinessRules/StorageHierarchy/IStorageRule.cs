using System.Collections.Generic;
using Infrastructure.Container.Entities;

namespace StorageHierarchy
{
    public interface IStorageRule
    {
        List<Container> OrganiseStorage();

        List<Container> OrganiseWithdrawal();

        bool DoesApply(IEnumerable<int> types);
    }
}
