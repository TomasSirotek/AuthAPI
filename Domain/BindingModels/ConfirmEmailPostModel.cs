namespace ProductAPI.Domain.BindingModels {

    public class ConfirmEmailPostModel {
        public string UserId { get; set; }
        public string Token { get; set; }
    }
}