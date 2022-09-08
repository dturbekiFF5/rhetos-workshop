using Rhetos.Dsl;
using Rhetos.Dsl.DefaultConcepts;
using System.ComponentModel.Composition;

namespace Bookstore.RhetosExtension {
    [Export(typeof(IConceptInfo))]
    [ConceptKeyword("MonitoredRecord")]
    public class MonitoredRecordInfo : IConceptInfo {
        [ConceptKey]
        public EntityInfo Entity { get; set; }
    }

    [Export(typeof(IConceptMacro))]
    public class MonitoredRecordMacro : IConceptMacro<SifarnikInfo> {
        public IEnumerable<IConceptInfo> CreateNewConcepts(SifarnikInfo conceptInfo, IDslModel existingConcepts) {

            var newConcepts = new List<IConceptInfo>();



            // DateTime CreatedAt { CreationTime; DenyUserEdit; }
            var createdAtProperty = new DateTimePropertyInfo {
                DataStructure = conceptInfo.Entity,
                Name = "CreatedAt"
            };
            newConcepts.Add(createdAtProperty);

            newConcepts.Add(
                new CreationTimeInfo {
                    Property = createdAtProperty
                });
            newConcepts.Add(
                new DenyUserEditPropertyInfo {
                    Property = createdAtProperty
                });

            // // Logging { AllProperties; }
            var loggingProperty = new EntityLoggingInfo {
                Entity = conceptInfo.Entity
            };
            newConcepts.Add(loggingProperty);

            newConcepts.Add(
                new AllPropertiesLoggingInfo {
                    EntityLogging = loggingProperty
                });

            return newConcepts;
        }
    }
}
