using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CakeShop.DTO
{
    public class CakeDto
    {
        private int id;
        private string name;
        private string photo;
        private string description;
        private int quantity;
        private string price;
        private int type_id;

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string Photo { get => photo; set => photo = value; }
        public string Description { get => description; set => description = value; }
        public string Price { get => price; set => price = value; }
        public int Type_id { get => type_id; set => type_id = value; }
        public int Quantity { get => quantity; set => quantity = value; }

        public CakeDto() { }
        public CakeDto(int id, string name, string photo,string description,string price, int type_id)
        {
            this.Id = id;
            this.Name = name;
            this.Photo = photo;
            this.Description = description;
            this.Price = price;
            this.Type_id = type_id;
        }
        public CakeDto(int id, string name, string photo, string description, string price,int quantity ,int type_id)
        {
            this.Id = id;
            this.Name = name;
            this.Photo = photo;
            this.Description = description;
            this.Price = price;
            this.Quantity = quantity;
            this.Type_id = type_id;
        }
        public CakeDto(CakeDto cakeDto)
        {
            this.Id = cakeDto.Id;
            this.Name = cakeDto.Name;
            this.Photo = cakeDto.Photo;
            this.Description = cakeDto.Description;
            this.Price = cakeDto.Price;
            this.Quantity = cakeDto.Quantity;
            this.Type_id = cakeDto.Type_id;
        }
        
    }
}
