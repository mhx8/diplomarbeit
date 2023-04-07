namespace Domain.Release
{
    public enum ReleaseStatus
    {
        NotDeployed = 0,
        WaitingForApproval = 1,
        Succeeded = 2,
        InProgress = 3,
        Failed = 4
    }
}
