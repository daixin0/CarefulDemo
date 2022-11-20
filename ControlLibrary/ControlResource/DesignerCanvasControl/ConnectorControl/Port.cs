using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Careful.Controls.DesignerCanvasControl.ConnectorControl
{
    public class Port<TEntity>
    {
        public Port()
        {
        }

        public Port(TEntity entity)
        {
            _entity = entity;
        }
        TEntity _entity;

        public TEntity Entity
        {
            get { return _entity; }
            set { _entity = value; }
        }
    }
}
