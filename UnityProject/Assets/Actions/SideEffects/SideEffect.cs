using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

interface SideEffect
{
    void additionalEffect(NodeData performr, Targetable target, bool isWin);
}
