using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamenU2Paradigmas.Dtos.Common
{
    public class ItemsDto<T>
    {
        public T Items { get; set; }
    }
}
