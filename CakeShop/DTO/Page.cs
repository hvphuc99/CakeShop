namespace CakeShop.DTO
{
    public class page
    {
        private int firstIndex;
        private int finalIndex;
        public page() { }
        public page(int first, int final) { this.firstIndex = first; this.finalIndex = final; }
        public int FirstIndex { get => firstIndex; set => firstIndex = value; }
        public int FinalIndex { get => finalIndex; set => finalIndex = value; }
    }
}
