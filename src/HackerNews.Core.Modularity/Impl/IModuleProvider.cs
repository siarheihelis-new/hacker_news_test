namespace HackerNews.Core.Modularity.Impl
{
    internal interface IModuleProvider
    {
        /// <summary>
        ///   Gets the modules by section.
        /// </summary>
        /// <param name="sectionName">Name of the section.</param>
        IEnumerable<ModuleInfo> GetModulesBySection(string sectionName);
    }
}
