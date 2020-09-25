using System;

namespace Careful.Controls.DataGridControl.ConvertStringToLambda
{
    internal interface ISubtractSignatures : IAddSignatures
    {
        void F(DateTime x, DateTime y);
        void F(DateTime? x, DateTime? y);
    }
}
