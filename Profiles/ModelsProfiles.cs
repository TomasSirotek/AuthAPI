using AutoMapper;
using ProductAPI.BindingModels;
using ProductAPI.Models;

namespace ProductAPI.Profiles {
    public class ModelsProfiles : Profile {
        public ModelsProfiles()
        {
         // Source => target
         CreateMap<Product, Product>();
         CreateMap<PostProductModel,Product>();
         CreateMap<PutCharacterModel,Product>();
         CreateMap<Category,Category>();
        }
    }
}