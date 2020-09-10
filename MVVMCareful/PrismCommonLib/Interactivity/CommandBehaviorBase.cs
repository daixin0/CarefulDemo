// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PrismCommonLib.Interactivity
{
    /// <summary>
    /// Base behavior to handle connecting a <see cref="Control"/> to a Command.
    /// </summary>
    /// <typeparam name="T">The target object must derive from Control</typeparam>
    /// <remarks>
    /// CommandBehaviorBase can be used to provide new behaviors for commands.
    /// </remarks>
    public class CommandBehaviorBase<T> where T : UIElement
    {
        private ICommand command;
        private object commandParameter;
        private readonly WeakReference targetObject;
        private readonly EventHandler commandCanExecuteChangedHandler;


        /// <summary>
        /// Constructor specifying the target object.
        /// </summary>
        /// <param name="targetObject">The target object the behavior is attached to.</param>
        public CommandBehaviorBase(T targetObject)
        {
            this.targetObject = new WeakReference(targetObject);

            this.commandCanExecuteChangedHandler = new EventHandler(this.CommandCanExecuteChanged);
        }

        /// <summary>
        /// Corresponding command to be execute and monitored for <see cref="ICommand.CanExecuteChanged"/>
        /// </summary>
        public ICommand Command
        {
            get { return command; }
            set
            {
                if (this.command != null)
                {
                    this.command.CanExecuteChanged -= this.commandCanExecuteChangedHandler;
                }

                this.command = value;
                if (this.command != null)
                {
                    this.command.CanExecuteChanged += this.commandCanExecuteChangedHandler;
                    UpdateEnabledState();
                }
            }
        }

        /// <summary>
        /// The parameter to supply the command during execution
        /// </summary>
        public object CommandParameter
        {
            get { return this.commandParameter; }
            set
            {
                if (this.commandParameter != value)
                {
                    this.commandParameter = value;
                    this.UpdateEnabledState();
                }
            }
        }

        /// <summary>
        /// Object to which this behavior is attached.
        /// </summary>
        protected T TargetObject
        {
            get
            {
                return targetObject.Target as T;
            }
        }


        /// <summary>
        /// Updates the target object's IsEnabled property based on the commands ability to execute.
        /// </summary>
        protected virtual void UpdateEnabledState()
        {
            if (TargetObject == null)
            {
                this.Command = null;
                this.CommandParameter = null;
            }
            else if (this.Command != null)
            {
                TargetObject.IsEnabled = this.Command.CanExecute(this.CommandParameter);
            }
        }

        private void CommandCanExecuteChanged(object sender, EventArgs e)
        {
            this.UpdateEnabledState();
        }

        /// <summary>
        /// Executes the command, if it's set, providing the <see cref="CommandParameter"/>
        /// </summary>
        protected virtual void ExecuteCommand(object parameter)
        {
            if (this.Command != null)
            {
                if (this.CommandParameter != null)
                {
                    this.Command.Execute(this.CommandParameter);
                }
                else
                {
                    this.Command.Execute(parameter);
                }
            }
        }
    }
}