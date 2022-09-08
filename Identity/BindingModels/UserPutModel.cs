using System.ComponentModel.DataAnnotations;

namespace AuthAPI.Identity.BindingModels {
    public class UserPutModel {
        [Required]
        public string Id { get; set; }
        
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        
        public List<string> Roles { get; set; }

        public bool IsActivated { get; set; }
    }
}