namespace ProductAPI.Domain.BindingModels {
    public class AddressPostModel {
        public string Street { get; set; }
        public int Number { get; set; }
        public string Country { get; set; }
        public int Zip { get; set; }
    }
}