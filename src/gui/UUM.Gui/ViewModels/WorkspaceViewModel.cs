﻿using System.Collections.ObjectModel;
using System.IO;
using System.Windows;

using Catel.Data;
using Catel.IoC;
using Catel.Logging;
using Catel.MVVM;
using Catel.MVVM.Tasks;
using Catel.Services;

using UUM.Api.Interfaces;
using UUM.Engine.Models;

namespace UUM.Gui.ViewModels
{
    /// <summary>
    ///     Views the workspace where all projects could be loaded including all pluging, userpool and everything else.
    /// </summary>
    public class WorkspaceViewModel : ViewModelBase
    {
        /// <summary>UUM Project files|*.uumx|All files|*.*</summary>
        private const string UumProjectFileFilter = "UUM Project files|*.uumx|All files|*.*";
        
		private readonly ILog _log = LogManager.GetCurrentClassLogger();

        public WorkspaceViewModel()
        {
            var splashScreenService = DependencyResolver.Resolve<ISplashScreenService>();
            splashScreenService.Enqueue(new ActionTask("Loading plug-ins", OnLoadPlugins));
            splashScreenService.Commit();
            NewProject = new Command(OnNewProjectExecuted, OnNewProjectCanExecute);
            SaveProject = new Command(OnSaveProjectExecuted, OnSaveProjectCanExecute);
            LoadProject = new Command(OnLoadProjectExecuted, OnLoadProjectCanExecute);
            CloseProject = new Command(OnCloseProjectExecute, OnCloseProjectCanExecute);
            ApplicationExit = new Command(OnApplicationExitExecuted);    
        }
        
        private void OnLoadPlugins(ITaskProgressTracker tracker)
        {
            var pluginRepository = DependencyResolver.Resolve<IPluginRepository>();
            pluginRepository.Initialize();
            Plugins = pluginRepository.Plugins;  
        }

        #region Title

        /// <summary>
        ///     Gets the title of the view model.
        /// </summary>
        /// <value>The title.</value>
        public override string Title
        {
            get { return "Workspace"; }
        }
		
        #endregion
		
        #region Property: Project

        public ProjectModel Project
        { get; set; }

        #endregion
		
        #region Property: Plugins

        public ObservableCollection<IPlugin> Plugins
        { get; set; }

        #endregion

        #region Command: NewProject

        public Command NewProject { get; private set; }

        private void OnNewProjectExecuted()
        {
            Project = new ProjectModel();
           // LogManager.AddListener(new FileLogListener("UUM.log"));
            _log.Info("NewProject command executed");
        }

        private bool OnNewProjectCanExecute()
        {
            return Project == null;
        }

        #endregion

        #region Command: SaveProject

        public Command SaveProject { get; private set; }

        private void OnSaveProjectExecuted()
        {
            var saveFileService = DependencyResolver.Resolve<ISaveFileService>();
            saveFileService.Filter = UumProjectFileFilter;
            if (saveFileService.DetermineFile())
            {
                string fileName = saveFileService.FileName;
                Project.Save(fileName, SerializationMode.Xml);
                
               //  LogManager.AddListener(new FileLogListener("UUM.log"));
               _log.Info("SaveProject command executed: '{0}'", fileName);
            }
        }

        private bool OnSaveProjectCanExecute()
        {
            return Project != null;
        }

        #endregion

        #region Command: LoadProject

        public Command LoadProject { get; private set; }

        private void OnLoadProjectExecuted()
        {
            var openFileService = DependencyResolver.Resolve<IOpenFileService>();
            openFileService.Filter = UumProjectFileFilter;
            if (openFileService.DetermineFile())
            {
                string fileName = openFileService.FileName;
				using (var stream = File.OpenRead(fileName))
				{
                	Project = ProjectModel.Load(stream, SerializationMode.Xml);
				}
                
                _log.Info("LoadProject command executed: '{0}'", fileName);
            }
        }

        private bool OnLoadProjectCanExecute()
        {
            return Project == null;
        }

        #endregion

        #region Command: CloseProject

        public Command CloseProject { get; private set; }

        private void OnCloseProjectExecute()
        {
            Project = null;
        }

        private bool OnCloseProjectCanExecute()
        {
            return Project != null;
        }

        #endregion

        #region Command: ApplicationExit

        public Command ApplicationExit { get; private set; }

        private void OnApplicationExitExecuted()
        {
            var messageService = DependencyResolver.Resolve<IMessageService>();
            var task = messageService.Show("Are you sure you want to exit application?",
                                    "Are you sure?", MessageButton.YesNo);
            task.RunSynchronously();
            if (task.Result == MessageResult.Yes)
            {
                Application.Current.Shutdown();
                // Do it!
            }
        }

        #endregion
    }
}