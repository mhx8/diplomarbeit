using System.Collections.Generic;

namespace Domain.WorkItem
{
    public class FeatureModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<DeploymentTaskModel> Tasks { get; set; }
    }
}
