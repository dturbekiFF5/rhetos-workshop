using Rhetos.Dsl;
using Rhetos.Dsl.DefaultConcepts;
using System.ComponentModel.Composition;

namespace Bookstore.RhetosExtension {
    [Export(typeof(IConceptInfo))]
    [ConceptKeyword("Sifarnik")]
    public class SifarnikInfo : IConceptInfo {
        [ConceptKey]
        public EntityInfo Entity { get; set; }
    }

    [Export(typeof(IConceptMacro))]
    public class SifarnikMacro : IConceptMacro<SifarnikInfo> {
        public IEnumerable<IConceptInfo> CreateNewConcepts(SifarnikInfo conceptInfo, IDslModel existingConcepts) {
            
            var newConcepts = new List<IConceptInfo>();


            
            // ShortString Code 
            var codeProperty = new ShortStringPropertyInfo {
                DataStructure = conceptInfo.Entity,
                Name = "Code"
            };
            newConcepts.Add(codeProperty);

            newConcepts.Add(
                new AutoCodePropertyInfo {
                    Property = codeProperty
                });

            newConcepts.Add(new ShortStringPropertyInfo {
                DataStructure = conceptInfo.Entity,
                Name = "Name"
            });

            return newConcepts;
        }
    }
}
