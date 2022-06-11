using AutoMapper;
using Data_Access.BindingModels;
using Data_Access.Models;

namespace Data_Access.Profiles {
    public class ModelsProfiles : Profile {
        public ModelsProfiles()
        {
         // Source => targer
         CreateMap<Character, Character>();
         CreateMap<PostCharacterModel, Character>();
         CreateMap<PutCharacterModel, Character>();
        }
    }
}