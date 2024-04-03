using System.Collections.Generic;
using Infrastructure.Container.Entities;
using Infrastructure.Container.Services;

namespace StorageHierarchy
{
    public interface IRuleStorageHierarchyCalculator
    {
        List<Container> ApplyStorageRule(List<Container> list, IContainerManager containerManager);
        List<Container> ApplyWithdrawalRule(List<Container> list, IContainerManager containerManager);
    }
}