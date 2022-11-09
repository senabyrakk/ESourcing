using System.ComponentModel.DataAnnotations.Schema;

namespace Orders.Domain.Entities.Base
{
    public abstract class EntityBase : IEntityBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Id { get; protected set; }

        public EntityBase Clone()
        {
            return (EntityBase)this.MemberwiseClone();
        }
    }
}
