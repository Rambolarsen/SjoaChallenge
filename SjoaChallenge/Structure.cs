namespace SjoaChallenge
{
    public class Structure
    {
        public Structure(string type, string name, string url, ICollection<Structure> children)
        {
            Type = type;
            Name = name;
            Url = url;
            Children = children;
        }

        public string Type { get; private set; }

        public string Name { get; private set; }

        public string Url { get; private set; }

        public ICollection<Structure> Children { get; private set; } = new List<Structure>();

        public ICollection<Structure> GetAllDescendants()
        {
            return GetAllDescendants(new[] { this }).ToList();
        }

        private IEnumerable<Structure> GetAllDescendants(IEnumerable<Structure>? children)
        {
            var descendants = children?.SelectMany(x => GetAllDescendants(x.Children));
            return children?.Concat(descendants ?? Enumerable.Empty<Structure>()) ?? Enumerable.Empty<Structure>();
        }

        public Structure? GetParent(Structure structure)
        {
            var allNodes = GetAllDescendants();
            var parentsOfSelectedChildren = allNodes.Where(node => node.Children?.Any(x => x.Url == structure.Url) ?? false);
            
            if (!parentsOfSelectedChildren.Any()) return null;

            return parentsOfSelectedChildren.Single();
        }
        public bool IsParentToStructure(Structure structure) => Children?.Any(x => x.Url == structure.Url) ?? false;
    }
}
