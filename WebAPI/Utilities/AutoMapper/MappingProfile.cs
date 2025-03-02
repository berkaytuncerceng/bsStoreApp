using AutoMapper;
using Entities.DTOs;
using Entities.Models;

namespace WebAPI.Utilities.AutoMapper
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			// CreateMap<Source, Destination>();
			CreateMap<BookDtoToUpdate, Book>(); // BookDtoToUpdate'dan Book'a dönüşüm yap burda kullanıcı isteğinden dtoya alıp booka çeviricem
		}
	}
}
