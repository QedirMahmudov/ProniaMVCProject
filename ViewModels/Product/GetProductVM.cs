namespace ProniaMVCProject.ViewModels
{
    public class GetProductVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string SKU { get; set; }

        //reletional properties
        public string CategoryName { get; set; }
        public string MainImage { get; set; }
    }
}
