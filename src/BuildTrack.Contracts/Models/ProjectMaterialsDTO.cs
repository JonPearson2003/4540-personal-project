namespace BuildTrack.Contracts.Models;

public sealed record ProjectMaterialDto(
    int ProjectMaterialId,
	int ProjectId,
	string Name,
	string Description,
	decimal Price,
	int Quantity
);
