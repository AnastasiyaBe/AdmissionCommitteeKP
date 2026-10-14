namespace AdmissionCommittee.Application.DTO;

public class EntranceTestDto
{
    public int TestId { get; set; }
    public string Name { get; set; } = null!;
    public string Form { get; set; } = null!;
    public decimal MinScore { get; set; }
}