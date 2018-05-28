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
    /// <para type="description">Imports and parses a grouvee collection .csv file which can be created 
    /// via the 'Export your collection to a CSV file' function on grouvee.com</para>
    /// <para type="example"></para>
    /// </summary>    
    /// <example>
    ///   <para>Example 1: Get child items in the current directory</para>
    ///   <para></para>
    ///   <code>PS C:\>Get-GrouveeCollection -Path "C:\Windows\temp\grouvee_export.csv"</code>
    ///   <para>This command will import a grouvee file from the provided path and</para>
    ///   <para>return a list of all games contained within it.</para>
    /// </example>
    [Cmdlet(VerbsCommon.Get, "GrouveeCollection")]
    [OutputType(typeof(GrouveeGame))]
    public class ImportGrouveeCollectionCmdlet : Cmdlet
    {
        /// <summary>
        /// <para type="description">The fully qualified or relative path to the grouvee collection .csv file.</para>
        /// </summary>
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
        [Alias("path")]
        public string Path { get; set; }

        /// <summary>
        /// <para type="description">ProcessRecord Block.</para>
        /// </summary>
        protected override void ProcessRecord()
        {
            base.ProcessRecord();
            if(!File.Exists(Path))
            {
                WriteError(new ErrorRecord(new ItemNotFoundException($"Cannot find path '{Path}' because it does not exist."), "PathNotFound", ErrorCategory.ObjectNotFound, Path));
                return;
            }

            try
            {
                var collection = Task.Run(async () => await GrouveeCollectionParser.GrouveeCollection.ImportAsync(Path)).Result;
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
