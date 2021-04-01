namespace WebApiTest.Models
{
    public class OrderItemModel
    {
        public short LineNumber { get; set; }

        public int ProductID { get; set; }

        public int StudentPersonID { get; set; }

        public decimal Price { get; set; }
    }

    public class OrderDelModel
    {
        public short LineNumber { get; set; }

        public int OrderID { get; set; }
    }
}
