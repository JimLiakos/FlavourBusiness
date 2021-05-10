using System;

using OOAdvantech.Collections.Generic;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;

namespace MenuModel
{
    /// <MetaDataID>{ffd97b11-b13b-4b1f-acb3-08566035ecfb}</MetaDataID>
    public class MagnitudeOptionsGroup : PreparationOptionsGroup
    {
        public override void AddPreparationOption(IPreparationScaledOption preparationOption)
        {
            throw new NotSupportedException("AddPreparationOption not supported. Use NewPreparationOption istead");
        }

    }
}