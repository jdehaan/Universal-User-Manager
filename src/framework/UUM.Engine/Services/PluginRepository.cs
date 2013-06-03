﻿using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using UUM.Api.Interfaces;
using UUM.Engine.Interfaces;

namespace UUM.Engine.Services
{
    public class PluginRepository : IPluginRepository
    {
        public void Initialize()
        {
            // MEF loading of available plug-ins
            var catalog = new DirectoryCatalog(".", "UUM.Plugin.*.dll");
            var container = new CompositionContainer(catalog);

            container.SatisfyImportsOnce(this);
        }

        [ImportMany(AllowRecomposition = true)]
        public ObservableCollection<IPlugin> Plugins { get; set; }

    }
}