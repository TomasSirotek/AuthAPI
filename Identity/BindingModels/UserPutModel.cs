using System.ComponentModel.DataAnnotations;

namespace ProductAPI.Identity.BindingModels {
    public class UserPutModel {
        [Required]
        public string Id { get; set; }
        
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
        [Required]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }
        
        public List<string> Roles { get; set; }
        
        public string Password { get; set; }
    
        public bool IsActivated { get; set; }
    }
}