using UnityEngine;

namespace poetools.Core
{
    [CreateAssetMenu]
    public class Tag : ScriptableObject
    {
        [SerializeField]
        private string id;

        public string Id => id;

        protected bool Equals(Tag other)
        {
            return base.Equals(other) && id == other.id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Tag)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                // ReSharper disable NonReadonlyMemberInGetHashCode
                return (base.GetHashCode() * 397) ^ (id != null ? id.GetHashCode() : 0);
            }
        }

        public static bool operator ==(Tag a, Tag b)
        {
            return a && b && a.id == b.id;
        }

        public static bool operator !=(Tag a, Tag b)
        {
            return !(a == b);
        }

        public static Tag FromString(string id)
        {
            var result = CreateInstance<Tag>();
            result.id = id;
            return result;
        }
    }
}
