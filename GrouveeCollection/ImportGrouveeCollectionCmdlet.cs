using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using GrouveeCollectionParser;

namespace GrouveeCollection
{
    /// <summary>
    /// <para type="synopsis">Imports and parses a grouvee collection .csv file.</para>
    /// <para type="description">Imports and parses a grouvee collection .csv file which can be created</para>
    /// <para type="description">via the 'Export your collection to a CSV file' function on grouvee.com</para>
    /// </summary>    
    [Cmdlet(VerbsCommon.Get, "GrouveeCollection")]
    [OutputType(typeof(GrouveeGame))]
    public class ImportGrouveeCollectionCmdlet : Cmdlet
    {
        /// <summary>
        /// <para type="description">The fully qualified or relative path to the grouvee collection .csv file.</para>
        /// </summary>
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
        [Alias("path")]
        public string FilePath { get; set; }

        /// <summary>
        /// <para type="description">ProcessRecord Block.</para>
        /// </summary>
        protected override void ProcessRecord()
        {
            base.ProcessRecord();
            if(!File.Exists(FilePath))
            {
                WriteError(new ErrorRecord(new ItemNotFoundException($"Cannot find path '{FilePath}' because it does not exist."), "PathNotFound", ErrorCategory.ObjectNotFound, FilePath));
                return;
            }

            try
            {
                var collection = Task.Run(async () => await GrouveeCollectionParser.GrouveeCollection.ImportAsync(FilePath)).Result;
                collection.ForEach(x => WriteObject(x));
            }
            catch (PipelineStoppedException)
            {
                // Nothing to do here, the try block is simply to handle exceptions when the user aborts the command
                return;
            }
            catch (AggregateException ex)
            {
                foreach (var error in ex.InnerExceptions)
                {
                    WriteError(new ErrorRecord(error, "UnknownError", ErrorCategory.NotSpecified, null));
                }
                return;
            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "UnknownError", ErrorCategory.NotSpecified, null));
                return;
            }

        }
    }
}
