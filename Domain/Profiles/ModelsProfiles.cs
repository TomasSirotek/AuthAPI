using AutoMapper;
using ProductAPI.Domain.BindingModels;
using ProductAPI.Domain.Models;

namespace ProductAPI.Domain.Profiles {
    public class ModelsProfiles : Profile {
        public ModelsProfiles()
        {
            // Source => target
            CreateMap<Product, Product>();
            CreateMap<PostProductModel,Product>();
            CreateMap<PutProductModel,Product>();
            CreateMap<Category,Category>();
        }
    }
}