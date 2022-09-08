namespace AuthAPI.Domain.BindingModels; 

public class PasswordPutModel {
    public string UserId { get; set; }
    public string CurrentPassword { get; set; }
    public string NewPassword { get; set; }
}