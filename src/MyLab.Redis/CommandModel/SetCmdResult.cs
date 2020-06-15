using System;
using System.Collections.Generic;
using System.Text;

namespace MyLab.Redis.CommandModel
{
    /// <summary>
    /// Set command result 
    /// </summary>
    public enum SetCmdResult
    {
        /// <summary>
        /// Value not defined
        /// </summary>
        Undefined,
        /// <summary>
        /// No one action done
        /// </summary>
        NoEffect,
        /// <summary>
        /// New entity added
        /// </summary>
        Added,
        /// <summary>
        /// Existent entity updates
        /// </summary>
        Updated
    }
}
