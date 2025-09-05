using AutoMapper;
using ByteInoTaskManager.Models;
using ByteInoTaskManager.Models.DTOs;

namespace ByteInoTaskManager.Mappings
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            // Category
            CreateMap<Category, CategoryDTO>().ReverseMap();

            // TaskItem -> DTO 
            CreateMap<TaskItem, TaskItemDTO>()
                .ForMember(d => d.CategoryName, opt => opt.MapFrom(s => s.Category != null ? s.Category.Name : null))
                .ForMember(d => d.UserId, opt => opt.MapFrom(s => s.User != null ? s.User.Id : null))
                .ForMember(d => d.IsCompleted, opt => opt.MapFrom(s => s.Status == Models.TaskStatus.Finished))
                .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status));


            // TaskItemDTO -> TaskItem (Create/Edit)
            CreateMap<TaskItemDTO, TaskItem>()
                .ForMember(d => d.Id, opt => opt.Ignore()) 
                .ForMember(d => d.Status, opt => opt.MapFrom(s => s.IsCompleted ? Models.TaskStatus.Finished : Models.TaskStatus.Pending))
                .ForMember(d => d.UserId, opt => opt.Ignore())
                .ForMember(d => d.User, opt => opt.Ignore())
                .ForMember(d => d.Category, opt => opt.Ignore())
                .ForMember(d=>d.Date,opt=>opt.Ignore());

            // User
            CreateMap<ApplicationUser, ApplicationUserDTO>().ReverseMap();

            // Register
            CreateMap<RegisterDTO, ApplicationUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));
        }
    }
}
