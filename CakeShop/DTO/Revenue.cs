using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CakeShop.DTO
{
    public class Revenue
    {
        private int typeId;
        private String typeName;
        private int month;
        private int? totalPrice;

        public Revenue() { }
        public Revenue(int typeid, string typename, int month, int? totalprice)
        {
            this.TypeId = typeid;
            this.TypeName = typename;
            this.Month = month;
            this.TotalPrice = totalprice;
        }
        public int TypeId { get => typeId; set => typeId = value; }
        public string TypeName { get => typeName; set => typeName = value; }
        public int Month { get => month; set => month = value; }
        public int? TotalPrice { get => totalPrice; set => totalPrice = value; }
    }
}
