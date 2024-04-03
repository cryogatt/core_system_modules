using System.Collections.Generic;
using ContainerTypes;
using Infrastructure.Container.Entities;

namespace StorageHierarchy
{
    interface IStorageHierarchy
    {
        List<ContainerMake> GetStorageOutline();
        
        /// <summary>
        ///     Store containers.
        /// </summary>
        /// <returns></returns>
        List<Container> ApplyStorageRule();

        /// <summary>
        ///     Withdraw containers.
        /// </summary>
        /// <returns></returns>
        List<Container> ApplyWithdrawalRule();
    }
}
