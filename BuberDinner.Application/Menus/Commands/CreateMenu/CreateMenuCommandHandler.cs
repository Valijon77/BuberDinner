using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Domain.Hosts;
using BuberDinner.Domain.Hosts.ValueObjects;
using BuberDinner.Domain.Menus;
using BuberDinner.Domain.Menus.Entities;
using ErrorOr;
using MediatR;

namespace BuberDinner.Application.Menus.Commands.CreateMenu;

public class CreateMenuCommandHandler : IRequestHandler<CreateMenuCommand, ErrorOr<Menu>>
{
    private readonly IMenuRepository _menuRepository;

    public CreateMenuCommandHandler(IMenuRepository menuRepository)
    {
        _menuRepository = menuRepository;
    }

    public async Task<ErrorOr<Menu>> Handle(
        CreateMenuCommand request,
        CancellationToken cancellationToken
    )
    {
        await Task.CompletedTask;
        //Create Menu
        var menu = Menu.Create(
            HostId.Create(request.HostId),
            request.Name,
            request.Description,
            request.Sections.ConvertAll(
                section =>
                    MenuSection.Create(
                        section.Name,
                        section.Description,
                        section.Items.ConvertAll(
                            item => MenuItem.Create(item.Name, item.Description)
                        )
                    )
            )
        );
        //Persist Menu
        _menuRepository.Add(menu);

        //Return Menu
        return menu;
    }
}
