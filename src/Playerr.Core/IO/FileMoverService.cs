using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics.CodeAnalysis;

namespace Playerr.Core.IO
{
    public interface IFileMoverService
    {
        bool ImportFile(string sourceFile, string destinationFile);
    }

    [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
    [SuppressMessage("Microsoft.Globalization", "CA2101:SpecifyMarshalingForPInvokeStringArguments")]
    [SuppressMessage("Microsoft.Interoperability", "CA5392:UseDefaultDllImportSearchPathsAttribute")]
    [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
    [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public class FileMoverService : IFileMoverService
    {
        public bool ImportFile(string sourceFile, string destinationFile)
        {
            if (!File.Exists(sourceFile))
            {
                Console.WriteLine($"[FileMover] Source file not found: {sourceFile}");
                return false;
            }

            // Ensure destination directory exists
            var destDir = Path.GetDirectoryName(destinationFile);
            if (destDir != null && !Directory.Exists(destDir))
            {
                Directory.CreateDirectory(destDir);
            }

            // 1. Try Hardlink (Atomic Move)
            try
            {
                // Note: Hardlinks cannot cross volumes/partitions or work on network shares.
                // This is expected behavior when:
                // - Using network shares (SMB/CIFS/NFS)
                // - Downloads and library are on different Docker volumes
                // - Source and destination are on different filesystems
                // In these cases, we automatically fall back to copy.
                if (TryCreateHardLink(sourceFile, destinationFile))
                {
                    Console.WriteLine($"[FileMover] Hardlink created successfully: {destinationFile}");
                    return true;
                }
                else
                {
                     Console.WriteLine($"[FileMover] Hardlink not supported (different volumes or network share detected). Using copy method.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FileMover] Hardlink failed ({ex.Message}). Using copy method instead.");
            }

            // 2. Fallback to Copy (Standard file copy)
            try
            {
                Console.WriteLine($"[FileMover] Copying file: {sourceFile} -> {destinationFile}");
                Console.WriteLine($"[FileMover] Note: Copy method is used for network shares and cross-volume transfers in Docker.");
                File.Copy(sourceFile, destinationFile, overwrite: true);
                Console.WriteLine($"[FileMover] File copied successfully.");
                return true;
            }
            catch (Exception ex)
            {
                 Console.WriteLine($"[FileMover] Copy failed: {ex.Message}");
                 Console.WriteLine($"[FileMover] Verify that the destination path is writable and has sufficient space.");
                 return false;
            }
        }

        private bool TryCreateHardLink(string source, string destination)
        {
            // Delete destination if it exists (overwrite behavior for import)
            if (File.Exists(destination))
            {
                 File.Delete(destination);
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return CreateHardLink(destination, source, IntPtr.Zero);
            }
            else
            {
                // Unix (Linux/macOS)
                return Link(source, destination) == 0;
            }
        }

        // Windows Kernel32
        [DllImport("Kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool CreateHardLink(string lpFileName, string lpExistingFileName, IntPtr lpSecurityAttributes);

        // Unix Libc
        [DllImport("libc", SetLastError = true, EntryPoint = "link")]
        private static extern int Link(string oldpath, string newpath);
    }
}
