using BuberDinner.Application.Menus.Commands.CreateMenu;
using BuberDinner.Contracts.Menus;
using Mapster;
using MenuSection = BuberDinner.Domain.Menus.Entities.MenuSection;
using MenuItem = BuberDinner.Domain.Menus.Entities.MenuItem;
using BuberDinner.Domain.Menus;
using BuberDinner.Domain.Menus.ValueObjects;

namespace BuberDinner.Api.Common.Mapping;

public class MenuMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config
            .NewConfig<(CreateMenuRequest Request, Guid HostId), CreateMenuCommand>()
            .Map(dest => dest.HostId, src => src.HostId)
            .Map(dest => dest, src => src.Request);

        config
            .NewConfig<Menu, MenuResponse>()
            .Map(dest => dest.Id, src => MenuId.Create(src.Id.Value).Value)
            .Map(dest => dest.AverageRating, src => src.AverageRating.Value)
            .Map(dest => dest.HostId, src => src.HostId.Value)
            .Map(dest => dest.DinnerIds, src => src.DinnerIds.Select(dinnerId => dinnerId.Value))
            .Map(
                dest => dest.MenuReviewIds,
                src => src.MenuReviewIds.Select(menuReviewId => menuReviewId.Value)
            );

        config.NewConfig<MenuSection, MenuSectionResponse>().Map(dest => dest.Id, src => src.Id.Value);

        config.NewConfig<MenuItem, MenuItemResponse>().Map(dest => dest.Id, src => src.Id.Value);
    }
}
