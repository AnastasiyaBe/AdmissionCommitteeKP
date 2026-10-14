using AdmissionCommittee.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AdmissionCommittee.Web.Controllers;

[Route("test")]
public class TestController : Controller
{
    private readonly ISpecialtyService _specialties;
    private readonly IApplicantService _applicants;
    private readonly IApplicationService _applications;
    private readonly IReportService _reports;

    public TestController(
        ISpecialtyService specialties,
        IApplicantService applicants,
        IApplicationService applications,
        IReportService reports)
    {
        _specialties = specialties;
        _applicants = applicants;
        _applications = applications;
        _reports = reports;
    }

    [HttpGet("specialties")]
    public async Task<IActionResult> Specialties() => Json(await _specialties.GetAllAsync());

    [HttpGet("applicants")]
    public async Task<IActionResult> Applicants() => Json(await _applicants.GetAllAsync());

    [HttpGet("applications")]
    public async Task<IActionResult> Applications() => Json(await _applications.GetAllAsync());

    [HttpGet("competitive/{specialtyId:int}")]
    public async Task<IActionResult> Competitive(int specialtyId)
        => Json(await _reports.GetCompetitiveListAsync(specialtyId));

    [HttpGet("passing-score/{specialtyId:int}")]
    public async Task<IActionResult> PassingScore(int specialtyId)
        => Json(new { specialtyId, score = await _reports.GetCurrentPassingScoreAsync(specialtyId) });

    [HttpGet("dynamics")]
    public async Task<IActionResult> Dynamics(DateTime? from, DateTime? to)
    {
        var f = from ?? DateTime.Now.AddDays(-30);
        var t = to ?? DateTime.Now;
        return Json(await _reports.GetApplicationDynamicsAsync(f, t));
    }

    [HttpGet("plan")]
    public async Task<IActionResult> Plan() => Json(await _reports.GetAdmissionPlanReportAsync());

    [HttpGet("search")]
    public async Task<IActionResult> Search(string q) => Json(await _applicants.SearchAsync(q));
}