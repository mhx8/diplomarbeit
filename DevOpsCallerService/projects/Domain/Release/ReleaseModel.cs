namespace Domain.Release
{
    public class ReleaseModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ReleaseStatus Status { get; set; }

        public int EnvironmentId { get; set; }

        public string Url { get; set; }

        public bool AlreadyHigherVersionDeployedOrTriggered { get; set; }
    }
}
