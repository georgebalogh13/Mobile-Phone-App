namespace Phone_project_API.DTOs
{
    public class PhoneDto
    {
        public string Brand { get; set; }
        public string Model { get; set; }
        public decimal Price { get; set; }

        override
        public string ToString()
        {
            return $"Brand: {Brand}, Model: {Model}, Price: {Price}";
        }

    }
}
