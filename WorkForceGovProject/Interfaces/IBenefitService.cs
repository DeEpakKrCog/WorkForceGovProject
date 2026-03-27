using WorkForceGovProject.Models;

namespace WorkForceGovProject.Interfaces
{
    /// <summary>
    /// Benefit Service Interface
    /// Handles benefit and program operations
    /// </summary>
    public interface IBenefitService
    {
        // Benefit Retrieval
        Task<Benefit> GetBenefitByIdAsync(int id);
        Task<IEnumerable<Benefit>> GetCitizenBenefitsAsync(int citizenId);
        Task<IEnumerable<Benefit>> GetActiveBenefitsAsync(int citizenId);

        // Benefit Management
        Task<(bool Success, string Message, Benefit Benefit)> AllocateBenefitAsync(int citizenId, int programId, string benefitType, decimal amount, string description = "");
        Task<(bool Success, string Message)> CompleteBenefitAsync(int benefitId);
        Task<(bool Success, string Message)> WithdrawBenefitAsync(int benefitId);

        // Program Management
        Task<EmploymentProgram> GetProgramByIdAsync(int id);
        Task<IEnumerable<EmploymentProgram>> GetAllProgramsAsync();
        Task<IEnumerable<EmploymentProgram>> GetActiveProgramsAsync();
        Task<(bool Success, string Message, EmploymentProgram Program)> CreateProgramAsync(EmploymentProgram program);
        Task<(bool Success, string Message)> UpdateProgramAsync(EmploymentProgram program);

        // Statistics
        Task<decimal> GetTotalBenefitAmountAsync(int citizenId);
        Task<int> GetActiveBenefitCountAsync(int citizenId);
        Task<IEnumerable<Benefit>> GetBenefitsByTypeAsync(int citizenId, string benefitType);
    }
}
