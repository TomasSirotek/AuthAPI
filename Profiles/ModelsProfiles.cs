using AutoMapper;
using Data_Access.BindingModels;
using Data_Access.Models;

namespace Data_Access.Profiles {
    public class ModelsProfiles : Profile {
        public ModelsProfiles()
        {
         // Source => target
         CreateMap<Character, Character>();
         CreateMap<PostCharacterModel, Character>();
         CreateMap<PutCharacterModel, Character>();
        }
    }
}