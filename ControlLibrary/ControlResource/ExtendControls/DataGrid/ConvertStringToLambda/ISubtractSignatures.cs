using System;

namespace ControlResource.ExtendControlStyle.DataGrid.ConvertStringToLambda
{
    internal interface ISubtractSignatures : IAddSignatures
    {
        void F(DateTime x, DateTime y);
        void F(DateTime? x, DateTime? y);
    }
}
