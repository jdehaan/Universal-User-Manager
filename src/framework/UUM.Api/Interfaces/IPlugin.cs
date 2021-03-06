﻿using System;
using UUM.Api.Models;

namespace UUM.Api.Interfaces
{
	/// <summary>
	/// Interface used for fetching and distinguishing each Plugin uniquely, according to its Name, Description, Users, Repository, 
	/// Prarameters and Parameters type.
	/// </summary>
    public interface IPlugin: IIdentifiable
    {

        /// <summary>
        /// Short name of the plugin
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Detailled description
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Factory method to build a repository from the parameters
        /// </summary>
        /// <param name="parameters">Initialized parameters</param>
        /// <returns>A repository capable of exchanging data with the source/target system</returns>
        IRepository GetRepository(ParametersBase parameters);

        /// <summary>
        /// Return a set of default parameters for this plugin
        /// </summary>
        /// <returns>Parameters initialized with default values</returns>
    	ParametersBase GetParameters();

    	/// <summary>
    	/// Return the type of the parameters for this plugin
    	/// </summary>
    	/// <returns></returns>
    	Type GetParametersType();

    	/// <summary>
    	/// Return the type of the user in source for this plugin
    	/// </summary>
    	/// <returns></returns>
    	Type GetUserInSourceType();
    	
    }
}