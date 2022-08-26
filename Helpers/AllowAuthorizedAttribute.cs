using Microsoft.AspNetCore.Authorization;
using ProductAPI.Domain.Enum;

namespace ProductAPI.Helpers {
    public class AllowAuthorizedAttribute : AuthorizeAttribute {
        public AllowAuthorizedAttribute(params AccessRole[] roles) : base() {
              Roles = String.Join(",", Enum.GetNames(typeof(AccessRole)));
        }
    }
}