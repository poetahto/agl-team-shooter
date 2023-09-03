using System.Collections.Generic;
using UnityEngine;

namespace poetools.Core
{
    public class TagHolder : MonoBehaviour
    {
        [SerializeField]
        private List<string> quickTags = new List<string>();

        public List<Tag> tags = new List<Tag>();

        private void Awake()
        {
            foreach (string quickTag in quickTags)
                Add(quickTag);
        }

        public bool Has(Tag targetTag)
        {
            return Has(targetTag.Id);
        }

        public bool Has(string tagId)
        {
            foreach (var currentTag in tags)
            {
                if (currentTag.Id == tagId)
                    return true;
            }

            return false;
        }

        public bool HasAny(params string[] desiredTagIds)
        {
            foreach (var tagId in desiredTagIds)
            {
                if (Has(tagId))
                    return true;
            }

            return false;
        }

        public bool HasAny(params Tag[] desiredTags)
        {
            foreach (var currentTag in desiredTags)
            {
                if (Has(currentTag))
                    return true;
            }

            return false;
        }

        public bool HasAll(params string[] desiredTagIds)
        {
            foreach (var tagId in desiredTagIds)
            {
                if (Has(tagId))
                    return true;
            }

            return false;
        }

        public bool HasAll(params Tag[] desiredTags)
        {
            int remaining = desiredTags.Length;

            foreach (var desiredTag in desiredTags)
            {
                if (Has(desiredTag))
                    remaining--;
            }

            return remaining <= 0;
        }

        public void Add(params Tag[] desiredTags)
        {
            foreach (var desiredTag in desiredTags)
            {
                if (!Has(desiredTag))
                    tags.Add(desiredTag);
            }
        }

        public void Add(params string[] tagIds)
        {
            foreach (var tagId in tagIds)
            {
                if (!Has(tagId))
                    tags.Add(Tag.FromString(tagId));
            }
        }
    }

    public static class TagExtensions
    {
        public static void AddTag(this GameObject target, params Tag[] tags)
        {
            var tagHolder = EnsureHasTagHolder(target);
            tagHolder.Add(tags);
        }

        public static void AddTag(this GameObject target, params string[] tags)
        {
            var tagHolder = EnsureHasTagHolder(target);
            tagHolder.Add(tags);
        }

        public static bool HasTag(this GameObject target, Tag tag)
        {
            return HasTag(target, tag.Id);
        }

        public static bool HasTag(this GameObject target, string tag)
        {
            if (target.TryGetComponent(out TagHolder tagHolder))
                return tagHolder.Has(tag);

            return false;
        }

        public static bool HasTag(this Component component, Tag tag) => HasTag(component.gameObject, tag);
        public static bool HasTag(this Component component, string tag) => HasTag(component.gameObject, tag);

        public static bool HasAnyTag(this GameObject target, params Tag[] tags)
        {
            if (target.TryGetComponent(out TagHolder tagHolder))
                return tagHolder.HasAny(tags);

            return false;
        }

        public static bool HasAnyTag(this GameObject target, params string[] tags)
        {
            if (target.TryGetComponent(out TagHolder tagHolder))
                return tagHolder.HasAny(tags);

            return false;
        }

        public static bool HasAllTags(this GameObject target, params Tag[] tags)
        {
            if (target.TryGetComponent(out TagHolder tagHolder))
                return tagHolder.HasAll(tags);

            return false;
        }

        public static bool HasAllTags(this GameObject target, params string[] tags)
        {
            if (target.TryGetComponent(out TagHolder tagHolder))
                return tagHolder.HasAll(tags);

            return false;
        }

        private static TagHolder EnsureHasTagHolder(GameObject target)
        {
            if (!target.TryGetComponent(out TagHolder tagHolder))
                tagHolder = target.AddComponent<TagHolder>();

            return tagHolder;
        }
    }
}
