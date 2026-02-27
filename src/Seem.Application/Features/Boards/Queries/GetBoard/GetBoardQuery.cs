using MediatR;
using Seem.Application.Features.Boards.DTOs;

namespace Seem.Application.Features.Boards.Queries.GetBoard;

public record GetBoardQuery(Guid Id) : IRequest<BoardWithTasksDto>;
